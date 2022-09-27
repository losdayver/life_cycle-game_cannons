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
        static Texture greenTankBodyTexture;
        static Texture tankTracksTexture;
        static Texture turretTexture;
        static RenderTexture landTexture;

        static UI()
        {
            greenTankBodyTexture = new (VARIABLES.RESOURCEPATH + "kenney_tankspack\\PNG\\Default size\\tanks_tankGreen_body1.png");
            tankTracksTexture = new(VARIABLES.RESOURCEPATH + "kenney_tankspack\\PNG\\Default size\\tanks_tankTracks1.png");
            turretTexture = new(VARIABLES.RESOURCEPATH + "kenney_tankspack\\PNG\\Default size\\tanks_turret4.png");

            Scene scene = new();

            landTexture = scene.GenerateScene(512, 500);
        }

        /// <summary>
        /// Главный метод класса UI отвечает за отрисовку графики на экран
        /// </summary>
        public static void Draw()
        {
            App.window.Clear();

            Sprite tankSprite = new(greenTankBodyTexture)
            { Origin = new(greenTankBodyTexture.Size.X / 2, greenTankBodyTexture.Size.Y),
                Position = new (Game.testTank.x, Game.testTank.y) };

            Sprite tankTracksSprite = new(tankTracksTexture)
            { Position = new (Game.testTank.x - 38, Game.testTank.y - 30) };

            Sprite turretSprite = new(turretTexture)
            { Origin = new(0f, 5) };

            turretSprite.Position  = new(Game.testTank.x, Game.testTank.y - 40);
            turretSprite.Scale = new(1.5f, 1.5f);
            turretSprite.Rotation = Game.testTank.turretAngle;

            if (Game.testTank.turretAngle < 270) tankSprite.Scale = new(-1f, 1f); 

            App.window.Draw(turretSprite);
            App.window.Draw(tankSprite);
            App.window.Draw(tankTracksSprite);

            Sprite sceneSprite = new(landTexture.Texture);
            App.window.Draw(sceneSprite);
        }
    }
}
