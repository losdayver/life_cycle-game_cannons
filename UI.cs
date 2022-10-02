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
        static UI()
        {
        }

        /// <summary>
        /// Главный метод класса UI отвечает за отрисовку графики на экран
        /// </summary>
        public static void Draw()
        {
            App.window.Clear();

            Sprite tankTracksSprite = new(TEXTUTRES.TANKTRACKS);
            Sprite tankBodySprite = new(TEXTUTRES.GREENTANKBODY);
            Sprite tankTurretSprite = new(TEXTUTRES.TURRET);
            Sprite backGround = new(TEXTUTRES.BACKGROUND);
            backGround.Scale /= 2;

            tankTracksSprite.Scale /= 3;
            tankTracksSprite.Origin = new Vector2f(TEXTUTRES.TANKTRACKS.Size.X / 2, TEXTUTRES.TANKTRACKS.Size.Y * (1 - 2/3));
            tankTracksSprite.Position = new(Game.session.controlledTank.x, Game.session.controlledTank.y);
            tankTracksSprite.Rotation = Game.session.controlledTank.angle;

            tankBodySprite.Scale /= 3;
            tankBodySprite.Origin = new Vector2f(TEXTUTRES.GREENTANKBODY.Size.X / 2, TEXTUTRES.GREENTANKBODY.Size.Y);
            tankBodySprite.Position = new(Game.session.controlledTank.x, Game.session.controlledTank.y);
            tankBodySprite.Rotation = Game.session.controlledTank.angle;

            tankTurretSprite.Position = new(
                tankBodySprite.Position.X,
                tankBodySprite.Position.Y - tankBodySprite.Origin.Y / 4);
            tankTurretSprite.Rotation = Game.session.controlledTank.turretAngle;
            tankTurretSprite.Scale /= 2f;
            tankTurretSprite.Origin = new Vector2f(0, TEXTUTRES.TURRET.Size.Y * 0.5f);

            if (Game.session.controlledTank.turretAngle < 270)
                tankBodySprite.Scale = new(-tankBodySprite.Scale.X, tankBodySprite.Scale.Y);

            App.window.Draw(backGround);
            App.window.Draw(new Sprite(Game.session.scene.map.Texture));
            App.window.Draw(tankTurretSprite);
            App.window.Draw(tankBodySprite);
            App.window.Draw(tankTracksSprite);
            
        }
    }
}
