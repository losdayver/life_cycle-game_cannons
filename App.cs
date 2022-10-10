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
        public const int width = 1024;
        public const int height = 600;
        public static RenderWindow window = new RenderWindow(new VideoMode(width, height), "Игра Cannons от команды Life-Cycle");

        static App()
        {
            window.SetFramerateLimit(30);
        }

        /// <summary>
        /// Данная функция вызывается для запуска приложения. 
        /// Внутри находистя основной цикл работы приложения, а также обработчики событий
        /// </summary>
        public static void Run()
        {
            // тут короче в эвенты окна добавляется обработка клавиш -- надо почитать про это 
            //TODO
            window.KeyPressed += KeyPressed;
            window.KeyReleased += KeyReleased;
            window.MouseButtonPressed += MousePressed;
            window.MouseButtonReleased += MouseReleased;
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
            setState(Keyboard.Key.Right, ref KEYS.KEY_RIGHT);
            setState(Keyboard.Key.Space, ref KEYS.KEY_SPACE);

            void setState(Keyboard.Key sfmlInput, ref bool key)
            {
                if (e.Code == sfmlInput) key = true;
            }
        }

        /// <summary>
        /// Регистрация отпускания клавиш. В результате выполнения изменятются значения переменных в KEYS
        /// </summary>
        private static void KeyReleased(object sender, KeyEventArgs e)
        {
            var window = (Window)sender;

            setState(Keyboard.Key.Up, ref KEYS.KEY_UP);
            setState(Keyboard.Key.Down, ref KEYS.KEY_DOWN);
            setState(Keyboard.Key.Left, ref KEYS.KEY_LEFT);
            setState(Keyboard.Key.Right, ref KEYS.KEY_RIGHT);
            setState(Keyboard.Key.Space, ref KEYS.KEY_SPACE);

            void setState(Keyboard.Key sfmlInput, ref bool key)
            {
                if (e.Code == sfmlInput) key = false;
            }
        }

        /// <summary>
        /// Регистрация нажатия клавиш мыши. В результате выполнения изменятются значения переменных в KEYS
        /// </summary>
        private static void MousePressed(object sender, MouseButtonEventArgs e)
        {
            var window = (Window)sender;

            setState(Mouse.Button.Left, ref KEYS.MOUSE_LEFT);
            setState(Mouse.Button.Right, ref KEYS.MOUSE_RIGHT);

            void setState(Mouse.Button sfmlInput, ref bool button)
            {
                if (e.Button == sfmlInput) button = true;
            }
        }

        /// <summary>
        /// Регистрация отпускания клавиш мыши. В результате выполнения изменятются значения переменных в KEYS
        /// </summary>
        private static void MouseReleased(object sender, MouseButtonEventArgs e)
        {
            var window = (Window)sender;

            setState(Mouse.Button.Left, ref KEYS.MOUSE_LEFT);
            setState(Mouse.Button.Right, ref KEYS.MOUSE_RIGHT);

            void setState(Mouse.Button sfmlInput, ref bool button)
            {
                if (e.Button == sfmlInput) button = false;
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
