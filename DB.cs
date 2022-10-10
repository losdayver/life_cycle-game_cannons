using System;
using System.IO;

namespace game_cannons
{
    public static class DB
    {
        /// <summary>
        /// Список допустимых целей для сохренения игры
        /// </summary>
        public enum SaveTarget
        {
            FILE,
            DB
        }

        /// <summary>
        /// Сохранение результатов игры в соответствующее расположение target
        /// </summary>
        /// <param name="target"> вид сохранения </param>
        public static void SaveResult(SaveTarget target, out bool success)
        {
            if (target == SaveTarget.FILE)
            {
                success = SaveToFile();
            }
            else
            {
                success = false;
            }
        }

        /// <summary>
        /// Операция сохранения в файл
        /// </summary>
        private static bool SaveToFile()
        {
            string path = VARIABLES.DATABASEPATH + "FileDB.txt";
            FileInfo file = new(path);

            if (!file.Exists) { return false; }

            using (StreamWriter fstream = file.AppendText())
            {
                List<string> aliveTanks = new();
                string result = "";
                string tanks = "";
                for (int i = 0; i < Game.session.tanks.Count; i++)
                {
                    if (Game.session.tanks[i].isAlive)
                    {
                        aliveTanks.Add(Game.session.tanks[i].playerName);
                        tanks += Game.session.tanks[i].playerName + " ";
                    }
                }

                if (aliveTanks.Count == 1)
                {
                    result = "victory";
                }
                else if (aliveTanks.Count == 0)
                {
                    result = "draw";
                    tanks = "no winner";
                }
                else  // сюда попасть не должны
                {
                    result = "none";
                }

                fstream.WriteLine(tanks + "; " + result + " ; " + DateTime.Now.ToString());

                return true;
            }
        }
    }
}
