using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using SFML.Graphics;
using System.Diagnostics;
using SFML.Graphics.Glsl;
using System.Numerics;
using SFML.System;

namespace game_cannons
{
    /// <summary>
    /// Данный статический класс отвечает за все взаимодействия программы 
    /// с пользовательским интерфейсом и выводом информации на экран
    /// </summary>
    internal static class UI
    {
        static RenderTexture landTexture;
        static Scene scene = new(1024, 600);
        static uint counter = 50;

        static UI()
        {
            landTexture = scene.GenerateScene(128, 400);
        }

        /// <summary>
        /// Главный метод класса UI отвечает за отрисовку графики на экран
        /// </summary>
        public static void Draw()
        {
            App.window.Clear();

            //Sprite tankSprite = new(greenTankBodyTexture)
            //{ Origin = new(greenTankBodyTexture.Size.X / 2, greenTankBodyTexture.Size.Y),
            //    Position = new (Game.testTank.x, Game.testTank.y) };

            //Sprite tankTracksSprite = new(tankTracksTexture)
            //{ Position = new (Game.testTank.x - 38, Game.testTank.y - 30) };

            //Sprite turretSprite = new(turretTexture)
            //{ Origin = new(0f, 5) };

            //turretSprite.Position  = new(Game.testTank.x, Game.testTank.y - 40);
            //turretSprite.Scale = new(1.5f, 1.5f);
            //turretSprite.Rotation = Game.testTank.turretAngle;

            //if (Game.testTank.turretAngle < 270) tankSprite.Scale = new(-1f, 1f); 

            //App.window.Draw(turretSprite);
            //App.window.Draw(tankSprite);
            //App.window.Draw(tankTracksSprite);

            Sprite sceneSprite = new(landTexture.Texture);
            App.window.Draw(sceneSprite);

            Vector2 centrePoint;
            Vector2[] vector = scene.GetDerivativeVector(counter, 15, out centrePoint);

            VertexArray vertexArray = new(PrimitiveType.Lines, 2);
            vertexArray.Append
            (
                new Vertex
                (
                    new Vector2f(vector[0].X, vector[0].Y), Color.Magenta
                )
            );
            vertexArray.Append
            (
                new Vertex
                (
                    new Vector2f(vector[1].X, vector[1].Y), Color.Magenta
                )
            );

            double angle = (Math.Atan((double)(vector[1].Y - vector[0].Y) / 15)) * (180 / Math.PI);
            Sprite tankTracksSprite = new(TEXTUTRES.TANKTRACKS);

            
            tankTracksSprite.Scale /= 3;
            tankTracksSprite.Origin = new Vector2f(TEXTUTRES.TANKTRACKS.Size.X / 2, TEXTUTRES.TANKTRACKS.Size.Y);
            tankTracksSprite.Position = new(centrePoint.X, centrePoint.Y);
            tankTracksSprite.Rotation = (float)angle;

            counter++;

            //Console.WriteLine(points[0].Position);
            //Console.WriteLine(points[1].Position);

            App.window.Draw(vertexArray);

            App.window.Draw(tankTracksSprite);
        }
    }
}
