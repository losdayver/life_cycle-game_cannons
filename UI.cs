﻿using System;
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

                Sprite backGround = new(TEXTURES.BACKGROUND);
                Sprite bullet = new(TEXTURES.BULLET);
                backGround.Scale /= 2;

                List<Sprite> tanksTracksSprite = new();
                List<Sprite> tanksBodySprite = new();
                List<Sprite> tanksTurretSprite = new();
                for (int i = 0; i < 3; i++)
                {                   
                    tanksTracksSprite.Add(new(TEXTURES.TANKTRACKS));
                    tanksBodySprite.Add(new(TEXTURES.GREENTANKBODY));
                    tanksTurretSprite.Add(new(TEXTURES.TURRET));


                    tanksTracksSprite[i].Scale /= 3;
                    tanksTracksSprite[i].Origin = new Vector2f(TEXTURES.TANKTRACKS.Size.X / 2, TEXTURES.TANKTRACKS.Size.Y * (1 - 2 / 3));
                    tanksTracksSprite[i].Position = new(Game.session.tanks[i].x, Game.session.tanks[i].y);
                    tanksTracksSprite[i].Rotation = Game.session.tanks[i].angle;

                    tanksBodySprite[i].Scale /= 3;
                    tanksBodySprite[i].Origin = new Vector2f(TEXTURES.GREENTANKBODY.Size.X / 2, TEXTURES.GREENTANKBODY.Size.Y);
                    tanksBodySprite[i].Position = new(Game.session.tanks[i].x, Game.session.tanks[i].y);
                    tanksBodySprite[i].Rotation = Game.session.tanks[i].angle;

                    tanksTurretSprite[i].Position = new(
                        tanksBodySprite[i].Position.X,
                        tanksBodySprite[i].Position.Y - tanksBodySprite[i].Origin.Y / 4);
                    tanksTurretSprite[i].Rotation = Game.session.tanks[i].turretAngle;
                    tanksTurretSprite[i].Scale /= 2f;
                    tanksTurretSprite[i].Origin = new Vector2f(0, TEXTURES.TURRET.Size.Y * 0.5f);


                    if (Game.session.tanks[i].turretAngle < 270)
                        tanksBodySprite[i].Scale = new(-tanksBodySprite[i].Scale.X, tanksBodySprite[i].Scale.Y);
                }

                if (Game.session.bullet != null)
                {
                    bullet.Position = new(Game.session.bullet.x, Game.session.bullet.y);
                }
                

                App.window.Draw(backGround);
                App.window.Draw(new Sprite(Game.session.scene.map.Texture));
                for (int i = 0; i < 3; i++)
                {
                    if (Game.session.tanks[i].status)
                    {
                        App.window.Draw(tanksTurretSprite[i]);
                        App.window.Draw(tanksBodySprite[i]);
                        App.window.Draw(tanksTracksSprite[i]);
                    }
                    
                }
                
                if (Game.session.bullet != null)
                {
                    App.window.Draw(bullet);
                }
                    
            }
            else if (Game.GAME_STATE == "SETTINGS")
            {
                App.window.Clear();
                Sprite s = new(TEXTURES.ARROW_CURSOR);


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
