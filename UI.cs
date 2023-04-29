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
using SFML.Audio;

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
            if (Game.GAME_STATE == Game.GameState.GAME_SESSION)
            {
                App.window.Clear();
                SessionDrawer.Display();

            }
            else if (Game.GAME_STATE == Game.GameState.MENU)
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
        static Font player_font = new Font(VARIABLES.DEFAULTFONT);

        public static void Display()
        {
            Sprite backGroundSprite = new(TEXTURES.BACKGROUND);
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
                    t.x,
                    t.y - tanksBodySprite.Origin.Y / 4);
                tanksTurretSprite.Rotation = t.turretAngle;
                tanksTurretSprite.Scale /= 2f;
                tanksTurretSprite.Origin = new Vector2f(0, TEXTURES.TURRET.Size.Y * 0.5f);

                if (t.turretAngle < 270)
                    tanksBodySprite.Scale = new(-tanksBodySprite.Scale.X, tanksBodySprite.Scale.Y);

                App.window.Draw(tanksTurretSprite);
                App.window.Draw(tanksBodySprite);
                App.window.Draw(tanksTracksSprite);

                RectangleShape hp_box = new(new Vector2f(25f, 5f));
                hp_box.Position = new(t.x - hp_box.Size.X / 2, t.y - 50);
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

                    RectangleShape fuel_box = new(hp_box);
                    fuel_box.Size = new(hp_box.Size.X * ((float)Game.session.fuel / Game.session.startFuel), hp_box.Size.Y / 2);
                    fuel_box.FillColor = Color.Yellow;
                    fuel_box.Position = new(hp_box.Position.X, hp_box.Position.Y + 8);
                    App.window.Draw(fuel_box);
                }

                Text player_text = new(t.playerName, player_font);
                player_text.CharacterSize = 12;
                player_text.FillColor = Color.White;
                player_text.Position = new(hp_box.Position.X, hp_box.Position.Y - 20);

                App.window.Draw(player_text);
            }

            if (Game.session.bullet != null)
            {
                Sprite bulletSprite = new(TEXTURES.BULLET);
                bulletSprite.Origin = new(0, TEXTURES.BULLET.Size.Y / 2);
                bulletSprite.Position = new(Game.session.bullet.x, Game.session.bullet.y);
                bulletSprite.Rotation = Game.session.bullet.angle;
                App.window.Draw(bulletSprite);
            }
        }
    }

    class Menu
    {
        public static bool resultsButtonPressed = false;
        static Font menu_font = new Font(VARIABLES.DEFAULTFONT);
        uint margin = 20;
        float button_scale = 0.5f;

        //List<Sprite> buttons= new();
        List<Button> buttons = new();

        public Menu()
        {
            buttons.Add(new Button("Играть"));
            buttons.Add(new Button("Статистика"));
            buttons.Add(new Button("Выйти"));
        }
        public void Display() 
        {

            Sprite button_sprite = new(TEXTURES.PLACEHOLDER_BUTTON);
            button_sprite.Scale *= button_scale;
            button_sprite.Position = new(App.width / 2 - TEXTURES.PLACEHOLDER_BUTTON.Size.X * button_scale / 2, 0);

            for (int i = 0; i < buttons.Count; i++)
            {
                button_sprite.Position = 
                    new(button_sprite.Position.X, i * (TEXTURES.PLACEHOLDER_BUTTON.Size.Y * button_scale + margin) + 120);

                Text button_text = new(buttons[i].name, menu_font);
                button_text.CharacterSize = 48;

                button_text.Position = new(button_sprite.Position.X + 30, button_sprite.Position.Y + 30);

                //Проверка наведения мыши на кнопку
                if (button_sprite.GetGlobalBounds().Intersects
                    (
                        new FloatRect
                        (
                            new Vector2f
                            (
                                (float)Mouse.GetPosition().X - App.window.Position.X,
                                Mouse.GetPosition().Y - App.window.Position.Y),
                                new Vector2f(1, 1)
                            )
                        )
                    )
                {
                    Sprite b = new(button_sprite);
                    b.Color = Color.Green;
                    App.window.Draw(b);

                    if (KEYS.MOUSE_LEFT)
                    {
                        if (buttons[i].name == "Играть")
                        {
                            Game.GAME_STATE = Game.GameState.GAME_SESSION;
                            Game.musicMenu.Stop();
                            Game.music.Play();
                        }
                        else if (buttons[i].name == "Статистика" && !resultsButtonPressed)
                        {
                            string path = VARIABLES.DATABASEPATH + "FileDB.txt";
                            FileInfo file = new(path);

                            if (file.Exists) 
                            {
                                StreamReader sr = new StreamReader(file.FullName);

                                string resultsString = sr.ReadToEnd();

                                sr.Close();

                                Console.WriteLine("Результаты поледних боёв");
                                Console.WriteLine(resultsString);

                                resultsButtonPressed = true;
                            }
                        }
                        else if (buttons[i].name == "Выйти")
                        {
                            App.window.Close();
                        }
                    }   
                }
                else
                    App.window.Draw(button_sprite);
                App.window.Draw(button_text);
            }
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
