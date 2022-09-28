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

            VertexArray points = scene.GetDerivativeVector(500, 100);

            Console.WriteLine(points[0].Position);
            Console.WriteLine(points[1].Position);

            App.window.Draw(points);
        }
    }
}
