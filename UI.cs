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
using static System.Collections.Specialized.BitVector32;
using SFML.Window;

namespace game_cannons
{
    /// <summary>
    /// Данный статический класс отвечает за все взаимодействия программы 
    /// с пользовательским интерфейсом и выводом информации на экран
    /// </summary>
    internal static class UI
    {
        static UI()
        {
        }

        /// <summary>
        /// Главный метод класса UI отвечает за отрисовку графики на экран
        /// </summary>
        public static void Draw()
        {
            if (Game.GAME_STATE == "GAME_SESSION")
            {
                App.window.Clear();

                Sprite tankTracksSprite = new(TEXTURES.TANKTRACKS);
                Sprite tankBodySprite = new(TEXTURES.GREENTANKBODY);
                Sprite tankTurretSprite = new(TEXTURES.TURRET);
                Sprite backGround = new(TEXTURES.BACKGROUND);
                Sprite bullet = new(TEXTURES.BULLET);
                backGround.Scale /= 2;

                tankTracksSprite.Scale /= 3;
                tankTracksSprite.Origin = new Vector2f(TEXTURES.TANKTRACKS.Size.X / 2, TEXTURES.TANKTRACKS.Size.Y * (1 - 2 / 3));
                tankTracksSprite.Position = new(Game.session.controlledTank.x, Game.session.controlledTank.y);
                tankTracksSprite.Rotation = Game.session.controlledTank.angle;

                tankBodySprite.Scale /= 3;
                tankBodySprite.Origin = new Vector2f(TEXTURES.GREENTANKBODY.Size.X / 2, TEXTURES.GREENTANKBODY.Size.Y);
                tankBodySprite.Position = new(Game.session.controlledTank.x, Game.session.controlledTank.y);
                tankBodySprite.Rotation = Game.session.controlledTank.angle;

                tankTurretSprite.Position = new(
                    tankBodySprite.Position.X,
                    tankBodySprite.Position.Y - tankBodySprite.Origin.Y / 4);
                tankTurretSprite.Rotation = Game.session.controlledTank.turretAngle;
                tankTurretSprite.Scale /= 2f;
                tankTurretSprite.Origin = new Vector2f(0, TEXTURES.TURRET.Size.Y * 0.5f);

                if (Game.session.controlledTank.turretAngle < 270)
                    tankBodySprite.Scale = new(-tankBodySprite.Scale.X, tankBodySprite.Scale.Y);

                if (Game.session.bullet != null)
                {
                    bullet.Position = new(Game.session.bullet.x, Game.session.bullet.y);
                }
                

                App.window.Draw(backGround);
                App.window.Draw(new Sprite(Game.session.scene.map.Texture));
                App.window.Draw(tankTurretSprite);
                App.window.Draw(tankBodySprite);
                App.window.Draw(tankTracksSprite);
                if (Game.session.bullet != null)
                {
                    App.window.Draw(bullet);
                }
                    
            }
            else if (Game.GAME_STATE == "SETTINGS")
            {
                App.window.Clear();
                Sprite s = new(TEXTURES.GREENTANKBODY);


                s.Position = new(Mouse.GetPosition().X - App.window.Position.X, Mouse.GetPosition().Y - App.window.Position.Y);

                App.window.Draw(s);

                if (KEYS.MOUSE_LEFT)
                {
                    Console.WriteLine("Left");
                }
                if (KEYS.MOUSE_RIGHT)
                {
                    Console.WriteLine("Right");
                }
            }
            
        }
    }
}
