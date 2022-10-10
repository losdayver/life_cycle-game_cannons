﻿using SFML.Graphics;
using SFML.Window;
using SFML.System;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;
using static System.Formats.Asn1.AsnWriter;
using System.Diagnostics.Metrics;
using System.Runtime.InteropServices;

namespace game_cannons
{
    /// <summary>
    /// Данный статический класс описывает логику игры, включая обработку различных состояний игры, но
    /// не отвечает за логику работы самого приложения
    /// </summary>
    internal static class Game
    {
        // Допустимые значения переменной: MENU, SETTINGS, GAME_SESSION
        public static string GAME_STATE = "GAME_SESSION";
        public static Session session = new();

        static Game()
        {
        }

        /// <summary>
        /// Главный метод класса отвечает за обработку логики приложения в один кадр 
        /// </summary>
        public static void Tick()
        {
            if (GAME_STATE == "GAME_SESSION")
            {
                session.Tick();
            }
            else if (GAME_STATE == "MENU")
            {
                
            }
            else if (GAME_STATE == "SETTINGS")
            {
                // Здесь будет код для настроек
            }
        }
    }

    public class Bullet
    {
        public float x = 0;
        public float y = 0;
        float acceleration = 0.2f;
        float startSpeed = 10;
        float xSpeed;
        float ySpeed;
        float angle;

        public Bullet(Tank tank, float startSpeed)
        {
            x = tank.x;
            y = tank.y - 15; // фиксируем патрон на уровне дула
            angle = tank.turretAngle;
            xSpeed = (float)Math.Cos((double)angle * Math.PI / 180) * startSpeed;
            ySpeed = (float)Math.Sin((double)angle * Math.PI / 180) * startSpeed;

            this.startSpeed = startSpeed;
        }

        public void Tick()
        {
            bool checkXInWindow = x > 0 && x < App.window.Size.X;
            if (checkXInWindow)
            {
                bool IsNotCollision = this.y < App.window.Size.Y - Game.session.scene.sceneHeights[(uint)x];
                if (IsNotCollision) // если еще не столкнулся с землей
                {
                    bool turnAlreadyPlused = false; // ниже объяснение
                    for (int i = 0; i < 3; i++)  // проверяем столкновение со всеми танками
                    {
                        bool IsTarget = x >= Game.session.tanks[i].x - 10 && x <= Game.session.tanks[i].x + 10;
                        IsTarget = IsTarget && y >= Game.session.tanks[i].y - 10 && y <= Game.session.tanks[i].y + 10;
                        if (IsTarget && Game.session.tanks[i].status) // если попали в еще живой танк
                        {
                            Game.session.tanks[i].hp--;
                            if (Game.session.tanks[i].hp == 0)
                            {
                                Game.session.tanks[i].status = false;
                            }
                            Game.session.bullet = null; // удаляем патрон
                            Game.session.bulletCreated = false;
                            if (!turnAlreadyPlused)  // без этого если за один выстрел ранил 2 цели - +2 хода
                            {
                                Game.session.turn++;
                                turnAlreadyPlused = true;
                            }
                        }
                    }
                    x += xSpeed;
                    y += ySpeed;
                }
                else
                {
                    Game.session.scene.Hit(x, y);  //разрушение ландшафта
                }
            }
            else
            {
                Game.session.bullet = null;
                Game.session.bulletCreated = false;
                Game.session.turn++;
            }

            ySpeed += acceleration;   
        }
    }

    public class Tank
    {
        Session session;
        string playerName = "";
        float maxSpeed = 2f;
        float acceleration = 0.2f;
        float currentSpeed = 0f;
        public float turretAngle = 270f;
        float turretRotationSpeed = 2.5f;
        public float x;
        public float y = 0;
        public float angle = 0f;
        public bool status = true; // true - жив, false - уничтожен
        public int hp = 3; // кол-во допустимых попаданий
        Vector2[] vector;

        public Tank(Session s, float x) 
        {
            session = s;
            this.x = x;
        }

        public void Land()  // чтобы танки в каждом кадре корректно стояли на ландшафте
        {
            Vector2 centrePoint;
            vector = session.scene.GetDerivativeVector((uint)x, 8, out centrePoint);

            y = centrePoint.Y;

            angle = (float)((Math.Atan((double)(vector[1].Y - vector[0].Y) / 8)) * (180 / Math.PI));
            
        }

        public void Tick()
        {
            for (int i = 0; i < 3; i++)
            {
                Game.session.tanks[i].Land();  // в каждом кадре все танки должны корректно стоять, даже если не их ход
                // т. к. под ними можем измениться ландшафт от выстрела (+ появление при инициализации)
            }

            if (KEYS.KEY_LEFT && currentSpeed > -maxSpeed)
                currentSpeed -= acceleration;
            else if (KEYS.KEY_RIGHT && currentSpeed < maxSpeed)
                currentSpeed += acceleration;
            else
            {
                currentSpeed += -Math.Sign(currentSpeed) * acceleration;

                if (Math.Abs(currentSpeed) < acceleration * 2)
                    currentSpeed = 0;
            }

            // не дадим выйти за пределы карты (число 15 подобрано, чтобы даже частично не выйти за карту)
            bool InWindow = x + currentSpeed * (Math.Abs(vector[1].Y / vector[0].Y)) < session.scene.xSize - 15;
            InWindow = InWindow && x + currentSpeed * (Math.Abs(vector[1].Y / vector[0].Y)) > 15;
            if (InWindow)
            {
                x = x + currentSpeed * (Math.Abs(vector[1].Y / vector[0].Y));
            }
            

            /*x = x % session.scene.xSize;

            if (x <= 0)
                x = session.scene.xSize - 1;*/

            if (KEYS.KEY_UP && turretAngle > 180)
                turretAngle -= turretRotationSpeed;
            else if (KEYS.KEY_DOWN && turretAngle < 360)
                turretAngle += turretRotationSpeed;

            if (KEYS.KEY_SPACE && !Game.session.bulletCreated)
            {
                Game.session.spaceWasPressed = true;
                Game.session.startSpeed += 0.5f;
            }

            if (!KEYS.KEY_SPACE && Game.session.spaceWasPressed)
            {
                if (Game.session.startSpeed > 20) Game.session.startSpeed = 20;
                //Console.WriteLine(Game.session.startSpeed);  проверка скорости
                Bullet bullet = new(this, Game.session.startSpeed);
                session.bullet = bullet;
                Game.session.bulletCreated = true;
                Game.session.startSpeed = 0;
                Game.session.spaceWasPressed = false;
            }

            
        }
    }

    /// <summary>
    /// Данный класс описывает игровую сцену, на которой происходит основное действие игры
    /// </summary>
    public class Scene
    {
        /// <summary>
        /// Размер сцены по горизонтали в пикселях -- значение должно быть 2^n
        /// </summary>
        public uint xSize;
        public uint ySize;
        
        /// <summary>
        /// Текстура сгенерированной карты -- задается при вызове GenerateScene
        /// </summary>
        public RenderTexture? map;

        /// <summary>
        /// Массив высот задается после вызова GenerateScene
        /// </summary>
        public uint[] sceneHeights;

        public Scene(uint xSize, uint ySize)
        {
            if ((Decimal)(Math.Log(xSize, 2)) % 1 != 0)
                throw new Exception("xSize должно иметь вид 2^n");

            this.xSize = xSize;
            this.ySize = ySize;

            sceneHeights = new uint[xSize];
        }

        /// <summary>
        /// Процедурно создает ландшафт игрового поля
        /// </summary>
        /// <param name="depth"> Отвечает за глубину прорисовки -- значение не должно превышать xSize </param>
        /// <param name="maxHeight"> Максимальная высота ландшафта -- значение не должно превышать ySize </param>
        /// <returns>Возвращает текстуру сгенерированной сцены</returns>
        public void GenerateSceneHeights(uint depth, uint maxHeight)
        {
            if (depth > ySize || maxHeight > ySize)
                throw new Exception("Неправильно заданы параметры GenerateScene");

            // Эта проверка необходима для предотвращаения утечек памяти
            if (map != null) 
            {
                map.Dispose();
            }

            Random rand = new();
            int[] heights = new int[depth + 1];

            Recurse();
            
            void Recurse(uint offset = 0, uint i = 2)
            {
                if (i > depth) return;

                if (i == 2)
                {
                    heights[0] = rand.Next(0, (int)maxHeight);
                    heights[heights.Length - 1] = (int)heights[0];
                    heights[depth / 2] = rand.Next(0, (int)maxHeight);
                }
                else
                {
                    int last = heights[offset];
                    int next = heights[offset + depth / (i / 2)];

                    if (last > next)
                    {
                        int buffer = last;
                        last = next;
                        next = buffer;
                    }

                    int middle = (next - last) / 2 + last;

                    heights[offset + depth / i] = rand.Next(
                        (int)(middle - middle / (i-2)), 
                        (int)(middle + (int)(ySize - middle) / (i-2))
                        );
                }

                Recurse(offset, i * 2);
                Recurse(offset + depth / i, i * 2);
            }

            for (uint x = 0; x < xSize; x++)
            {
                int startPointer = (int)(((float)depth / xSize) * x);
                int endPointer = startPointer + 1;
                float step = xSize / depth;
                float k = (float)(heights[endPointer] - heights[startPointer]) / step;

                sceneHeights[x] = (uint)(k * (x - step * startPointer) + heights[startPointer] - 1);
            }

            GenerateMap();
        }

        /// <summary>
        /// Генерирует изображение сцена на основе sceneHeights;
        /// </summary>
        public void GenerateMap()
        {
            if (map != null)
            {
                map.Dispose();
            }
            map = new RenderTexture(xSize, ySize);
            map.Clear(Color.Transparent);

            Texture bnwMap = GetBnTMap();
            Sprite bnwSprite = new Sprite(bnwMap);
            Sprite landSprite = new Sprite(TEXTURES.LANDTEXTURE);

            map.Draw(bnwSprite);
            map.Draw(landSprite, new RenderStates(BlendMode.Multiply));

            bnwMap.Dispose();
            bnwSprite.Dispose();
            landSprite.Dispose();
        }

        /// <summary>
        /// Генерирует прозрачно-белую текстуру на основе sceneHeights
        /// </summary>
        /// <returns> Возвращает данную текстуру </returns>
        public Texture GetBnTMap()
        {
            Image img = new Image(xSize, ySize, Color.Transparent);

            for (uint x = 0; x < xSize; x++)
            {
                for (uint y = 0; y < sceneHeights[x]; y++)
                {
                    img.SetPixel(x, y, Color.White);
                }
            }
            
            return new Texture(img);
        }

        /// <summary>
        /// Находит вектор по производной в данной точке карты, который будет являться направлением движения танка
        /// </summary>
        /// <param name="centre"> Определяет точку, в которой будет осуществляться вычисление </param>
        /// <param name="margin"> Определяет максимвльный отступ вычислений (поиска локальных максимумов) </param>
        /// <param name="centrePoint"> Параметр возвращает среднюю точку между высотами результирующих точек </param>
        /// <returns> Возвращает массив из 2-х векторов, которые являются координатыми полученных локальных максимумов </returns>
        public Vector2[] GetDerivativeVector(uint centre, uint margin, out Vector2 centrePoint)
        {
            uint centreHeight = sceneHeights[centre];

            uint leftMax = 0;
            uint rightMax = 0;
            uint leftX = centre - margin;
            uint rightX = centre + margin;

            for (uint x = centre + 1; x < centre + margin; x++)
            {
                uint height = sceneHeights[x % xSize];

                if (height > rightMax)
                {
                    rightMax = height;
                    rightX = x;
                }
            }

            for (uint x = centre - 1; x > centre - margin; x--)
            {
                uint height = sceneHeights[x % xSize];

                if (height > leftMax)
                {
                    leftMax = height;
                    leftX = x;
                }
                    
            }

            centrePoint = new Vector2(centre, ySize - (rightMax + leftMax) / 2);
            return new Vector2[] { new(leftX, ySize - leftMax), new(rightX, ySize - rightMax) };
        }

        public void Hit(float x, float y)
        {
            GenerateCrater((uint)x, (uint)y);  // создание кратера
            GenerateMap();
            Game.session.bullet = null;
            Game.session.bulletCreated = false;
            Game.session.turn++;
        }

        public void GenerateCrater(uint x, uint y)
        {
            // взрыв в радиусе 15 от точки падения, чем ближе к центру, тем выше ущерб
            uint damage = 5;
            for (uint i = x - 15; i <= x; i++)
            {
                if ((i > 0 && i < App.window.Size.X) && (sceneHeights[i] > 0))
                {
                    sceneHeights[i] -= damage;
                    damage++;
                }
                
            }
            damage--;
            for (uint i = x+1; i <= x + 15; i++)
            {
                if ((i > 0 && i < App.window.Size.X) && (sceneHeights[i] > 0))
                {
                    sceneHeights[i] -= damage;
                    damage--;
                }
            }
        }
    }

    /// <summary>
    /// Данный класс описывает игровую сессию -- сцену, танки, и объекты на сцене, а также другие параметры, связанные с игровым процессом
    /// </summary>
    public class Session
    {
        public List<Tank> tanks = new();  // список танков
        public int turn = 0;  // для смены хода
        public Tank controlledTank;  // текущий управляемый танк
        public Scene scene = new(1024, 600);
        public Bullet bullet;
        public bool bulletCreated = false;  // выпущена ли сейчас пуля (чтобы нельзя было прервать полет и
                                            // запустить ее еще раз)
        public float startSpeed = 0;
        public bool spaceWasPressed = false;

        public Session() 
        {
            tanks.Add(new Tank(this, 100));
            tanks.Add(new Tank(this, 500));
            tanks.Add(new Tank(this, 900));
            scene.GenerateSceneHeights(128, 300);
        }

        public void Tick()
        {
            int aliveCount = 0;
            for (int i = 0; i < 3; i++)
            {
                if (tanks[i].status)
                {
                    aliveCount++;  // считаем сколько сейчас живых танков
                }
            }
            if (aliveCount <= 1)
            {
                Console.WriteLine("Game has ended!");  // TODO: добавить другое решение при окончании игры
                App.window.Close();
            }
            while (!tanks[turn % tanks.Count].status) // после смерти танки не удаляются, а зануляются, поэтому
                                                        // для зануленных танков ход скипается
            {
                turn++;
            }
            controlledTank = tanks[turn % tanks.Count];  // переход хода по сути переход по индексу списка танков
            controlledTank.Tick();
            if (bullet != null)
            {
                bullet.Tick();
            }
        }
    }

    
}
