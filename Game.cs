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
        public static Tank testTank;

        static Game()
        {
            testTank = new Tank();
        }

        /// <summary>
        /// Главный метод класса отвечает за обработку логики приложения в один кадр 
        /// </summary>
        public static void Tick()
        {
            testTank.Tick();
        }
    }

    public class Tank
    {
        string playerName = "";
        float maxSpeed = 10f;
        float acceleration = 0.2f;
        float currentSpeed = 0f;
        public float turretAngle = 200f;
        float turretRotationSpeed = 2.5f;
        public float x = 0f;
        public float y = 100f;

        public Tank() { }

        public void Tick()
        {
            if (KEYS.KEY_LEFT && currentSpeed > -maxSpeed)
                currentSpeed -= acceleration;
            else if (KEYS.KEY_RIGHT && currentSpeed < maxSpeed)
                currentSpeed += acceleration;
            else
            {
                currentSpeed += -Math.Sign(currentSpeed) * acceleration;

                if (Math.Abs(currentSpeed) < acceleration * 2)
                    currentSpeed = 0;
            }

            x += currentSpeed;

            if (KEYS.KEY_UP && turretAngle > 180)
                turretAngle -= turretRotationSpeed;
            else if (KEYS.KEY_DOWN && turretAngle < 360)
                turretAngle += turretRotationSpeed;
        }
    }
}
