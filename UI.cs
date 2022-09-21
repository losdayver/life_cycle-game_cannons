using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.Graphics;

namespace game_cannons
{
    /// <summary>
    /// Данный статический класс отвечает за все взаимодействия программы 
    /// с пользовательским интерфейсом и выводом информации на экран
    /// </summary>
    internal static class UI
    {
        static Texture image = new Texture("resources/test_image.png");
        /// <summary>
        /// Главный метод класса UI отвечает за отрисовку графики на экран
        /// </summary>
        public static void Draw()
        {
            
            Sprite sp = new Sprite(image);
            Sprite sp1 = new Sprite(image);
            sp.Rotation = 30f;
            App.window.Draw(sp);
            App.window.Draw(sp1);

            sp.Dispose();
            sp1.Dispose();
        }
    }
}
