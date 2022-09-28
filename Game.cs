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

namespace game_cannons
{
    /// <summary>
    /// Данный статический класс описывает логику игры, включая обработку различных состояний игры, но
    /// не отвечает за логику работы самого приложения
    /// </summary>
    internal static class Game
    {
        // Допустимые значения переменной: MENU, SETTINGS, GAME_SESSION
        public static string GAME_STATE = "MENU";
        public static Tank testTank;

        static Game()
        {
            testTank = new Tank();
            Scene scnece = new(1024, 640);
        }

        /// <summary>
        /// Главный метод класса отвечает за обработку логики приложения в один кадр 
        /// </summary>
        public static void Tick()
        {
            testTank.Tick();
        }
    }

    public class Tank
    {
        string playerName = "";
        float maxSpeed = 10f;
        float acceleration = 0.2f;
        float currentSpeed = 0f;
        public float turretAngle = 200f;
        float turretRotationSpeed = 2.5f;
        public float x = 0f;
        public float y = 100f;

        public Tank() { }

        public void Tick()
        {
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

            x += currentSpeed;

            if (KEYS.KEY_UP && turretAngle > 180)
                turretAngle -= turretRotationSpeed;
            else if (KEYS.KEY_DOWN && turretAngle < 360)
                turretAngle += turretRotationSpeed;
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
        uint xSize;
        uint ySize;
        
        /// <summary>
        /// Текстура сгенерированной карты -- задается при вызове GenerateScene
        /// </summary>
        RenderTexture? map;
        Image? mapImage;

        public Scene(uint xSize, uint ySize)
        {
            if ((Decimal)(Math.Log(xSize, 2)) % 1 != 0)
                throw new Exception("xSize должно иметь вид 2^n");

            this.xSize = xSize;
            this.ySize = ySize;
        }

        /// <summary>
        /// Процедурно создает ландшафт игрового поля
        /// </summary>
        /// <param name="depth"> Отвечает за глубину прорисовки -- значение не должно превышать xSize </param>
        /// <param name="maxHeight"> Максимальная высота ландшафта -- значение не должно превышать ySize </param>
        /// <returns></returns>
        public RenderTexture GenerateScene(uint depth, uint maxHeight)
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

            RenderTexture renderTexure = new RenderTexture(xSize, ySize);

            for (uint x = 0; x < xSize; x++)
            {
                int startPointer = (int)(((float)depth / xSize) * x);
                int endPointer = startPointer + 1;
                float step = xSize / depth;
                float k = (float)(heights[endPointer] - heights[startPointer]) / step;

                VertexArray line = new VertexArray(PrimitiveType.Lines);
                line.Append(new Vertex(new Vector2f(x, 0), Color.White));
                line.Append(new Vertex(new Vector2f(x, k * (x - step * startPointer) + heights[startPointer] - 1), Color.White));
                    
                renderTexure.Draw(line);
            }

            map = renderTexure;
            mapImage = renderTexure.Texture.CopyToImage();

            Sprite blendSprite = new Sprite(TEXTUTRES.LANDTEXTURE);
            RenderStates renderStates = new(BlendMode.Multiply);
            blendSprite.Draw(renderTexure, renderStates);
            return renderTexure;
        }

        public Vector2[] GetDerivativeVector(uint centre, uint margin, out Vector2 centrePoint)
        {
            uint centreHeight = GetHeight(centre);

            uint leftMax = ySize;
            uint rightMax = ySize;
            uint leftX = centre - margin;
            uint rightX = centre + margin;

            for (uint x = centre + 1; x < centre + margin; x++)
            {
                uint height = GetHeight(x);

                if (height < rightMax)
                {
                    rightMax = height;
                    rightX = x;
                }
            }

            for (uint x = centre - 1; x > centre - margin; x--)
            {
                uint height = GetHeight(x);

                if (height < leftMax)
                {
                    leftMax = height;
                    leftX = x;
                }
                    
            }

            centrePoint = new Vector2(centre, centreHeight);
            return new Vector2[] { new(leftX, leftMax), new(rightX, rightMax) };

            uint GetHeight(uint x)
            {
                for (uint y = 0; y < ySize; y++)
                {
                  
                    if (mapImage.GetPixel(x, y) == Color.White)
                    {
                        return y;
                    }
                }
                        
                return 0;
            }
        }
    }

    //class Map
    //{
    //    int x = 0;
    //    int y = 0;
    //    int x2;
    //    int y2;
    //    byte[] permutationTable;

    //    public Map(int x, int y, int seed = 0)
    //    {
    //        Random rnd = new Random(seed);
    //        permutationTable = new byte[1024];
    //        rnd.NextBytes(permutationTable);
    //        this.x2 = x;
    //        this.y2 = y;
    //    }

    //    public void GenerateMap(int octaves)
    //    {
    //        float res;
    //        Image img = new((uint)this.x2, (uint)this.y2);
    //        int border = 0;

    //        for (int i = 0; i < this.y2; i++)
    //        {
    //            for (int j = 0; j < this.x2; j++)
    //            {
    //                res = OctavePerlin(j, i, octaves);
    //                if (res >= border)
    //                {
    //                    img.SetPixel((uint)j, (uint)i, Color.White);
    //                }
    //                else
    //                {
    //                    img.SetPixel((uint)j, (uint)i, Color.Black);
    //                }
    //            }
    //        }

    //        img.SaveToFile("D:\\Images\\test.jpg");
    //    }

    //    float OctavePerlin(int x, int y, int octaves, float persistence = 0.5f)
    //    {
    //        float amplitude = 1;
    //        float max = 0;
    //        float result = 0;

    //        for (int i = 0; i < octaves; i++)
    //        {
    //            max += amplitude;
    //            result += Perlin(x, y) * amplitude;
    //            amplitude *= persistence;
    //            x *= 2;
    //            y *= 2;
    //        }

    //        return result / max;
    //    } 

    //    float Perlin(int curx, int cury)
    //    {
    //        curx = curx % this.x2;
    //        cury = cury % this.y2;
    //        int squareNum = FindSquare(curx, cury);
    //        if (squareNum == 0)
    //        {
    //            Console.WriteLine(curx);
    //            Console.WriteLine(cury);
    //            throw new Exception("Wrong square number!");
    //        }
    //        Vector2[] vectors = FindVectors(squareNum, curx, cury);
    //        Vector2[] nodeVectors = new Vector2[4];

    //        for (int i = 0; i < 4; i++)
    //        {
    //            nodeVectors[i] = GetNodeVector();
    //        }


    //        float[] c = new float[4];

    //        for (int i = 0; i < 4; i++)
    //        {
    //            c[i] = Vector2.Dot(vectors[i], nodeVectors[i]);
    //        }

    //        int[] relativeCoordinates = FindRelativeCoordinates(squareNum, curx, cury);
    //        int rx = relativeCoordinates[0];
    //        int ry = relativeCoordinates[1];

    //        float t1 = Lerp(c[0], c[1], rx);
    //        float t2 = Lerp(c[2], c[3], rx);
    //        float res = Lerp(t1, t2, ry);

    //        return res;
    //    }

    //    Vector2 GetNodeVector()
    //    {
    //        Random rnd = new Random();
    //        int x = rnd.Next(1, 5);

    //        switch (x)
    //        {
    //            case 1:
    //                return new Vector2(1, 0);
    //            case 2:
    //                return new Vector2(-1, 0);
    //            case 3:
    //                return new Vector2(0, 1);
    //            default:
    //                return new Vector2(0, -1);
    //        }
    //    }

    //    int[] FindRelativeCoordinates(int sq, int x, int y)
    //    {
    //        int[] nc = new int[2];
    //        int cx = (this.x2 - this.x) / 2;
    //        int cy = (this.y2 - this.y) / 2;

    //        if (sq == 1)
    //        {
    //            nc[0] = x;
    //            nc[1] = y;
    //        }
    //        else if (sq == 2)
    //        {
    //            nc[0] = x - cx;
    //            nc[1] = y;
    //        }
    //        else if (sq == 3)
    //        {
    //            nc[0] = x;
    //            nc[1] = y - cy;
    //        }
    //        else if (sq == 4)
    //        {
    //            nc[0] = x - cx;
    //            nc[1] = y - cy;
    //        }

    //        return nc;
    //    }


    //    int FindSquare(int x, int y)
    //    {
    //        int cx = (this.x2 - this.x) / 2;
    //        int cy = (this.y2 - this.y) / 2;
    //        if (x <= cx && x >= this.x && y >= this.y && y <= cy) return 1;
    //        if (x >= cx && x <= this.x2 && y >= this.y && y <= cy) return 2;
    //        if (x <= cx && x >= this.x && y <= this.y2 && y >= cy) return 3;
    //        if (x >= cx && x <= this.x2 && y <= this.y2 && y >= cy) return 4;
    //        return 0;
    //    }

    //    Vector2[] FindVectors(int sq, int x, int y)
    //    {
    //        Vector2[] nc = new Vector2[4];
    //        int cx = (this.x2 - this.x) / 2;
    //        int cy = (this.y2 - this.y) / 2;

    //        float x1 = 0, y1 = 0, x2 = 0, y2 = 0, x3 = 0, y3 = 0, x4 = 0, y4 = 0;

    //        if (sq == 1)
    //        {
    //            x1 = this.x;
    //            y1 = this.y;
    //            x2 = cx;
    //            y2 = this.y;
    //            x3 = this.x;
    //            y3 = cy;
    //            x4 = cx;
    //            y4 = cy;
    //        }
    //        else if (sq == 2)
    //        {
    //            x1 = cx;
    //            y1 = this.y;
    //            x2 = this.x2;
    //            y2 = this.y;
    //            x3 = cx;
    //            y3 = cy;
    //            x4 = this.x2;
    //            y4 = cy;
    //        }
    //        else if (sq == 3)
    //        {
    //            x1 = this.x;
    //            y1 = cy;
    //            x2 = cx;
    //            y2 = cy;
    //            x3 = this.x;
    //            y3 = this.y2;
    //            x4 = cx;
    //            y4 = this.y2;
    //        }
    //        else if (sq == 4)
    //        {
    //            x1 = cx;
    //            y1 = cy;
    //            x2 = this.x2;
    //            y2 = cy;
    //            x3 = cx;
    //            y3 = this.y2;
    //            x4 = this.x2;
    //            y4 = this.y2;
    //        }

    //        nc[0] = new Vector2((x - x1) / cx, (y - y1) / cy);
    //        nc[1] = new Vector2((x - x2) / cx, (y - y2) / cy);
    //        nc[2] = new Vector2((x - x3) / cx, (y - y3) / cy);
    //        nc[3] = new Vector2((x - x4) / cx, (y - y4) / cy);

    //        return nc;
    //    }

    //    float Lerp(float a, float b, float x)
    //    {
    //        return a + x * (b - a);
    //    }

    //    static float QunticCurve(float t)
    //    {
    //        return t * t * t * (t * (t * 6 - 15) + 10);
    //    }
    //}
}
