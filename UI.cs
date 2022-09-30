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
            scene.GenerateSceneHeights(128, 400);
        }

        /// <summary>
        /// Главный метод класса UI отвечает за отрисовку графики на экран
        /// </summary>
        public static void Draw()
        {
            App.window.Clear();

            App.window.Draw(new Sprite(scene.map.Texture));

            Vector2 centrePoint;
            Vector2[] vector = scene.GetDerivativeVector(counter, 8, out centrePoint);

            double angle = (Math.Atan((double)(vector[1].Y - vector[0].Y) / 8)) * (180 / Math.PI);
            Sprite tankTracksSprite = new(TEXTUTRES.TANKTRACKS);

            
            tankTracksSprite.Scale /= 3;
            tankTracksSprite.Origin = new Vector2f(TEXTUTRES.TANKTRACKS.Size.X / 2, TEXTUTRES.TANKTRACKS.Size.Y * (1 - 1/2));
            tankTracksSprite.Position = new(centrePoint.X, centrePoint.Y);
            tankTracksSprite.Rotation = (float)angle;

            counter+=5;

            if (counter > 1024)
            {
                counter = 0;
                scene.GenerateSceneHeights(128, 400);
            }

            App.window.Draw(tankTracksSprite);
        }
    }
}
