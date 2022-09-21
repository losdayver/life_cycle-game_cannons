using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game_cannons
{
    /// <summary>
    /// Данный статический класс описывает логику игры, включая обработку различных состояний игры, но
    /// не отвечает за логику работы самого приложения
    /// </summary>
    internal static class Game
    {
        // Допустимые значения переменной: MENU, SETTINGS, GAME_SESSION
        public static string GAME_STATE = "MENU";

        /// <summary>
        /// Главный метод класса отвечает за обработку логики приложения в один кадр 
        /// </summary>
        public static void Tick()
        {

        }
    }
}
