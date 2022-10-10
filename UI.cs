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
        static Menu menu = new();
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

                SessionDrawer.Display();

            }
            else if (Game.GAME_STATE == "SETTINGS")
            {
                App.window.Clear();
                menu.Display();
            }

        }
    }

    /// <summary>
    /// Данный статический класс отвечает за отрисовку игрового процесса
    /// </summary>
    static class SessionDrawer
    {
        public static void Display()
        {
            Sprite backGroundSprite = new(TEXTURES.BACKGROUND);
            Sprite bulletSprite = new(TEXTURES.BULLET);
            backGroundSprite.Scale /= 2;

            App.window.Draw(backGroundSprite);
            App.window.Draw(new Sprite(Game.session.scene.map.Texture));

            foreach (Tank t in Game.session.tanks)
            {
                if (!t.isAlive) continue;

                Sprite tanksTracksSprite = new(TEXTURES.TANKTRACKS);
                Sprite tanksBodySprite = new(TEXTURES.GREENTANKBODY);
                Sprite tanksTurretSprite = new(TEXTURES.TURRET);

                tanksTracksSprite.Scale /= 3;
                tanksTracksSprite.Origin = new Vector2f(TEXTURES.TANKTRACKS.Size.X / 2, TEXTURES.TANKTRACKS.Size.Y * (1 - 2 / 3));
                tanksTracksSprite.Position = new(t.x, t.y);
                tanksTracksSprite.Rotation = t.angle;

                tanksBodySprite.Scale /= 3;
                tanksBodySprite.Origin = new Vector2f(TEXTURES.GREENTANKBODY.Size.X / 2, TEXTURES.GREENTANKBODY.Size.Y);
                tanksBodySprite.Position = new(t.x, t.y);
                tanksBodySprite.Rotation = t.angle;

                tanksTurretSprite.Position = new(
                    tanksBodySprite.Position.X,
                    tanksBodySprite.Position.Y - tanksBodySprite.Origin.Y / 4);
                tanksTurretSprite.Rotation = t.turretAngle;
                tanksTurretSprite.Scale /= 2f;
                tanksTurretSprite.Origin = new Vector2f(0, TEXTURES.TURRET.Size.Y * 0.5f);

                if (t.turretAngle < 270)
                    tanksBodySprite.Scale = new(-tanksBodySprite.Scale.X, tanksBodySprite.Scale.Y);

                App.window.Draw(tanksTurretSprite);
                App.window.Draw(tanksBodySprite);
                App.window.Draw(tanksTracksSprite);

                RectangleShape hp_box = new(new Vector2f(35f, 6f));
                hp_box.Position = new(t.x - hp_box.Size.X / 2, t.y - 40);
                hp_box.FillColor = Color.Red;

                RectangleShape hp_box1 = new(hp_box);
                hp_box1.Size = new(hp_box.Size.X * ((float)t.hp / t.max_hp), hp_box.Size.Y);
                hp_box1.FillColor = Color.Green;

                App.window.Draw(hp_box);
                App.window.Draw(hp_box1);

                if (t == Game.session.controlledTank)
                {
                    RectangleShape power_box = new(hp_box);
                    power_box.Size = new(hp_box.Size.X * (Game.session.bulletStartSpeed / Game.session.bulletMaxSpeed), hp_box.Size.Y/2);
                    power_box.FillColor = Color.Blue;
                    App.window.Draw(power_box);
                }

                

            }

            if (Game.session.bullet != null)
            {
                bulletSprite.Position = new(Game.session.bullet.x, Game.session.bullet.y);
                App.window.Draw(bulletSprite);
            }


        }
    }

    class Menu
    {
        List<Button> buttons= new();
        public Menu()
        { buttons.Add(new Button("arrow_left"));
            buttons.Add(new Button("arrow_right"));
            buttons.Add(new Button("start"));
        }
        public void Display() { 
        }
    }

    class Button
    {
        public string name;
        public Button (string name)
        {
            this.name = name;
        }
    }
}
