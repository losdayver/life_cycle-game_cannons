using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using SFML.Graphics;
using System.Diagnostics;

namespace game_cannons
{
    /// <summary>
    /// Данный статический класс отвечает за все взаимодействия программы 
    /// с пользовательским интерфейсом и выводом информации на экран
    /// </summary>
    internal static class UI
    {
        static string resourcesPath = Environment.CurrentDirectory;
        //static Texture image = new Texture("resources/test_image.png");

        static UI()
        {
            resourcesPath = resourcesPath.Substring(0, resourcesPath.Length - "bin\\Debug\\net6.0".Length);
        }

        /// <summary>
        /// Главный метод класса UI отвечает за отрисовку графики на экран
        /// </summary>
        public static void Draw()
        {
            
        }
    }
}
