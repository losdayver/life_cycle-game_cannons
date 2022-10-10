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
        static Menu menu =new();
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

                App.window.Draw(backGround);
                App.window.Draw(new Sprite(Game.session.scene.map.Texture));

                List<Sprite> tanksTracksSprite = new();
                List<Sprite> tanksBodySprite = new();
                List<Sprite> tanksTurretSprite = new();
                List<Sprite> tanksHPSprite = new();  // health bar

                for (int i = 0; i < Game.session.tanks.Count; i++)
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

                    RectangleShape hp_box = new(new Vector2f(40f, 15f));
                    hp_box.Position = new(Game.session.tanks[i].x - hp_box.Size.X / 2, Game.session.tanks[i].y - 40);
                    hp_box.FillColor = Color.Red;

                    RectangleShape hp_box1 = new(hp_box);
                    hp_box1.Size = new(hp_box.Size.X * ((float)Game.session.tanks[i].hp / Game.session.tanks[i].max_hp), hp_box.Size.Y);
                    hp_box1.FillColor = Color.Green;

                    App.window.Draw(hp_box);
                    App.window.Draw(hp_box1);

                    //if (Game.session.tanks[i].hp == 3)
                    //{
                    //    tanksHPSprite.Add(new(TEXTURES.HP3));
                    //}
                    //else if (Game.session.tanks[i].hp == 2)
                    //{
                    //    tanksHPSprite.Add(new(TEXTURES.HP2));
                    //}
                    //else
                    //{
                    //    tanksHPSprite.Add(new(TEXTURES.HP1));
                    //}

                    //tanksHPSprite[i].Position = new(Game.session.tanks[i].x - 22, 
                    //    tanksTurretSprite[i].Position.Y - 20);

                }

                if (Game.session.bullet != null)
                {
                    bullet.Position = new(Game.session.bullet.x, Game.session.bullet.y);
                }

                for (int i = 0; i < 3; i++)
                {
                    if (Game.session.tanks[i].isAlive)
                    {
                        App.window.Draw(tanksTurretSprite[i]);
                        App.window.Draw(tanksBodySprite[i]);
                        App.window.Draw(tanksTracksSprite[i]);
                        //App.window.Draw(tanksHPSprite[i]);
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
                menu.Display();
                
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
