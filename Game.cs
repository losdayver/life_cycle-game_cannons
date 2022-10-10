using SFML.Graphics;
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
        public enum GameState
        {
            MENU,
            SETTINGS,
            GAME_SESSION
        }

        /// <summary>
        /// Допустимые значения переменной: MENU, SETTINGS, GAME_SESSION
        /// </summary>
        public static GameState GAME_STATE = GameState.GAME_SESSION;
        public static Session session = new();

        static Game()
        {
        }

        /// <summary>
        /// Главный метод класса отвечает за обработку логики приложения в один кадр 
        /// </summary>
        public static void Tick()
        {
            if (GAME_STATE == GameState.GAME_SESSION)
            {
                session.Tick();
            }
            else if (GAME_STATE == GameState.MENU)
            {
                
            }
            else if (GAME_STATE == GameState.SETTINGS)
            {
                // Здесь будет код для настроек
            }
        }
    }
    
    /// <summary>
    /// Данный класс описывает пулю, выпускаемую танком при зажатии пробела
    /// </summary>
    public class Bullet
    {
        Session session;
        public float x = 0;
        public float y = 0;
        float acceleration = 0.2f;
        float startSpeed = 10;
        float xSpeed;
        float ySpeed;
        public float angle;

        public Bullet(Tank tank, float startSpeed, Session session)
        {
            x = tank.x;
            y = tank.y - 15; // фиксируем патрон на уровне дула
            angle = tank.turretAngle;
            xSpeed = (float)Math.Cos((double)angle * Math.PI / 180) * startSpeed;
            ySpeed = (float)Math.Sin((double)angle * Math.PI / 180) * startSpeed;

            this.startSpeed = startSpeed;

            this.session = session;
        }

        public void Tick()
        {
            ySpeed += acceleration;
            angle = (float)(Math.Atan((double)ySpeed / xSpeed) * 180 / Math.PI);

            if (xSpeed < 0)
            {
                angle += 180;
            }

            bool checkXInWindow = x > 0 && x < App.window.Size.X;
            if (checkXInWindow)
            {
                bool IsNotCollision = this.y < App.window.Size.Y - session.scene.sceneHeights[(uint)x];
                if (IsNotCollision) // если еще не столкнулся с землей
                {
                    bool turnAlreadyPlused = false; // ниже объяснение
                    for (int i = 0; i < session.tanks.Count; i++)  // проверяем столкновение со всеми танками
                    {
                        bool IsTarget = x >= session.tanks[i].x - 10 && x <= session.tanks[i].x + 10;
                        IsTarget = IsTarget && y >= session.tanks[i].y - 12 && y <= session.tanks[i].y + 10;
                        if (IsTarget && session.tanks[i].isAlive) // если попали в еще живой танк
                        {
                            session.tanks[i].hp--;
                            if (session.tanks[i].hp == 0)
                            {
                                Game.session.tanks[i].isAlive = false;
                            }
                            session.bullet = null; // удаляем патрон
                            session.bulletCreated = false;
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
                    session.scene.Hit(x);  //разрушение ландшафта
                }
            }
            else
            {
                Game.session.bullet = null;
                Game.session.bulletCreated = false;
                Game.session.turn++;
                Game.session.fuel = Game.session.startFuel;
            }
        }
    }

    /// <summary>
    /// Данный класс описывает танк на игровом поле
    /// </summary>
    public class Tank
    {
        Session session;
        public string playerName = "";
        float maxSpeed = 1f;
        float acceleration = 0.2f;
        float currentSpeed = 0f;
        public float turretAngle = 270f;
        float turretRotationSpeed = 2.5f;
        public float x;
        public float y = 0;
        public float angle = 0f;
        /// <summary>
        /// true - жив, false - уничтожен
        /// </summary>
        public bool isAlive = true;
        /// <summary>
        /// Жизни танка
        /// </summary>
        public int max_hp = 4;
        public int hp = 4;
        Vector2[] vector;

        public Tank(Session s, float x, string name) 
        {
            session = s;
            this.x = x;
            this.playerName = name;
        }

        /// <summary>
        /// чтобы танки в каждом кадре корректно стояли на ландшафте
        /// </summary>
        public void Land()
        {
            Vector2 centrePoint;
            vector = session.scene.GetDerivativeVector((uint)x, 8, out centrePoint);

            y = centrePoint.Y;

            angle = (float)((Math.Atan((double)(vector[1].Y - vector[0].Y) / 8)) * (180 / Math.PI));
            
        }

        public void Tick()
        {
            if (KEYS.KEY_LEFT && currentSpeed > -maxSpeed && session.fuel > 0)
            { 
                currentSpeed -= acceleration;
                session.fuel -= 1;
            }
            else if (KEYS.KEY_RIGHT && currentSpeed < maxSpeed && session.fuel > 0)
            {
                currentSpeed += acceleration;
                session.fuel -= 1;
            }
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

            if (KEYS.KEY_UP && turretAngle > 180)
                turretAngle -= turretRotationSpeed;
            else if (KEYS.KEY_DOWN && turretAngle < 360)
                turretAngle += turretRotationSpeed;

            if (KEYS.KEY_SPACE && !session.bulletCreated)
            {
                session.spaceWasPressed = true;
                session.bulletStartSpeed += session.bulletPowerIncrement;

                if (session.bulletStartSpeed > session.bulletMaxSpeed) session.bulletStartSpeed = session.bulletMaxSpeed;
            }

            if (!KEYS.KEY_SPACE && session.spaceWasPressed)
            {
                Bullet bullet = new(this, session.bulletStartSpeed, session);
                session.bullet = bullet;
                session.bulletCreated = true;
                session.bulletStartSpeed = 0;
                session.spaceWasPressed = false;
            }

            
        }
    }

    /// <summary>
    /// Данный класс описывает игровую сцену, на которой происходит основное действие игры
    /// </summary>
    public class Scene
    {
        Session session;
        /// <summary>
        /// Размер сцены по горизонтали в пикселях -- значение должно быть 2^n
        /// </summary>
        uint _xSize = 0;
        public uint xSize 
        { 
            get { return _xSize; }
            set 
            {
                if ((Math.Log(_xSize, 2)) % 1 != 0)
                    throw new Exception("xSize должно иметь вид 2^n");
                else
                {
                    _xSize = value;
                }
            } 
        }
        public uint ySize;
        
        /// <summary>
        /// Текстура сгенерированной карты -- задается при вызове GenerateScene
        /// </summary>
        public RenderTexture? map;

        /// <summary>
        /// Массив высот задается после вызова GenerateScene
        /// </summary>
        public uint[] sceneHeights;

        public Scene(uint xSize, uint ySize, Session session)
        {
            this._xSize = xSize;
            this.ySize = ySize;

            map = new RenderTexture(xSize, ySize);

            sceneHeights = new uint[xSize];

            this.session = session;
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
            map.Clear(Color.Transparent);

            Texture bnwMap = GetTnW();
            Sprite tnWSprite = new Sprite(bnwMap);
            Sprite landSprite = new Sprite(TEXTURES.LANDTEXTURE);

            map.Draw(tnWSprite);
            map.Draw(landSprite, new RenderStates(BlendMode.Multiply));

            bnwMap.Dispose();
            tnWSprite.Dispose();
            landSprite.Dispose();
        }

        /// <summary>
        /// Генерирует прозрачно-белую текстуру на основе sceneHeights
        /// </summary>
        /// <returns> Возвращает данную текстуру </returns>
        private Texture GetTnW()
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

        public void Hit(float x)
        {
            GenerateCrater((uint)x);  // создание кратера
            GenerateMap();
            session.bullet = null;
            session.bulletCreated = false;
            session.turn++;
            session.fuel = session.startFuel;
        }

        public void GenerateCrater(uint x0)
        {
            // взрыв в радиусе 15 от точки падения, чем ближе к центру, тем выше ущерб
            int r = 20;
            for (int x = (int)x0 - r; x <= (int)x0 + r; x++)
            {
                if ((x >= 0 && x < xSize) && (sceneHeights[x] > 0))
                {
                    uint damage = (uint)Math.Sqrt(Math.Pow(r, 2) - Math.Pow(x - x0, 2));
                    if (damage < sceneHeights[x]) 
                        sceneHeights[x] -= damage;
                    else
                        sceneHeights[x] = 0;
                }
                
            }
        }
    }

    /// <summary>
    /// Данный класс описывает игровую сессию -- сцену, танки, и объекты на сцене, 
    /// а также другие параметры, связанные с игровым процессом
    /// </summary>
    public class Session
    {
        public List<Tank> tanks = new();  // список танков
        public int turn = 0;  // для смены хода
        public Tank controlledTank;  // текущий управляемый танк
        public Scene scene;
        public Bullet bullet;
        public bool bulletCreated = false;  // выпущена ли сейчас пуля (чтобы нельзя было прервать полет и
                                            // запустить ее еще раз)
        public float bulletStartSpeed = 0;
        public float bulletMaxSpeed = 18;
        public float bulletPowerIncrement = 0.5f;

        public int startFuel = 40;
        public int fuel = 40;

        public bool spaceWasPressed = false;

        public Session() 
        {
            scene = new(1024, 600, this);
            tanks.Add(new Tank(this, 100, "player1"));
            //tanks.Add(new Tank(this, 500, "player2"));
            tanks.Add(new Tank(this, scene.xSize - 100, "player3"));
            scene.GenerateSceneHeights(128, 300);
        }

        public void Tick()
        {
            int aliveCount = 0;
            for (int i = 0; i < tanks.Count; i++)
            {
                // в каждом кадре все танки должны корректно стоять, даже если не их ход
                // т. к. под ними можем измениться ландшафт от выстрела (+ появление при инициализации)
                if (tanks[i].isAlive)
                {
                    Game.session.tanks[i].Land();
                    aliveCount++;
                }

            }
            if (aliveCount <= 1)
            {
                DB.SaveResult("file");  // TODO: добавить другое решение при окончании игры
                App.window.Close();
            }

            // после смерти танки не удаляются, а зануляются, поэтому
            // для зануленных танков ход пропускается
            while (!tanks[turn % tanks.Count].isAlive) 
            {
                turn++;
            }

            // переход хода
            controlledTank = tanks[turn % tanks.Count];

            // вычисления для хода текущего танка
            controlledTank.Tick();

            // если пуля не null -- высчитывать ее траекторию и поведение
            if (bullet != null)
            {
                bullet.Tick();
            }
        }
    }    
}
