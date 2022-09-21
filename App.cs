using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game_cannons
{
    /// <summary>
    /// Данный статический класс описывает приложение в целом -- все его аспекты, 
    /// связанные с самой работой приложения, но не с логикой игры
    /// </summary>
    static class App
    {
        public static RenderWindow window = new SFML.Graphics.RenderWindow(new VideoMode(800, 600), "Тестовое окно");

        /// <summary>
        /// Данная функция вызывается для запуска приложения. 
        /// Внутри находистя основной цикл работы приложения, а также обработчики событий
        /// </summary>
        public static void Run()
        {
            // тут короче в эвенты окна добавляется обработка клавиш -- надо почитать про это 
            //TODO
            window.KeyPressed += KeyPressed;
            window.Closed += OnClose;

            while (window.IsOpen)
            {
                // Запуск обработчика событий запускается каждый кадр приложения
                window.DispatchEvents();

                #region Основная логика программы

                Game.Tick();
                UI.Draw();

                #endregion

                // Обновление экрана
                window.Display();
            }
        }

        /// <summary>
        /// Регистрация нажатия клавиш. В результате выполнения изменятются значения переменных в KEYS
        /// </summary>
        private static void KeyPressed(object sender, KeyEventArgs e)
        {
            var window = (Window)sender;

            if (e.Code == Keyboard.Key.Escape)
            {
                window.Close();
            }

            setState(Keyboard.Key.Up, ref KEYS.KEY_UP);
            setState(Keyboard.Key.Down, ref KEYS.KEY_DOWN);
            setState(Keyboard.Key.Left, ref KEYS.KEY_LEFT);
            setState(Keyboard.Key.Left, ref KEYS.KEY_RIGHT);

            void setState(Keyboard.Key sfmlInput, ref bool key)
            {
                if (e.Code == sfmlInput) key = true;
                else key = false;
            }
        }

        /// <summary>
        /// Метод описывает поведение приложения при закрытии окна
        /// </summary>
        private static void OnClose(object sender, EventArgs e)
        {
            window.Close();
        }
    }
}
