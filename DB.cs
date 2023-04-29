using System;
using System.ComponentModel;
using System.IO;

namespace game_cannons
{
    interface IResultSaver
    {
        void SaveResult();
    }

    interface IResultPrinter
    {
        void PrintResult();
    }

    public class ResultSaver : IResultSaver
    {
        /// <summary>
        /// Сохранение результатов игры
        /// </summary>
        public void SaveResult()
        {
            SaveToFile();
        }

        /// <summary>
        /// Операция сохранения в файл
        /// </summary>
        private bool SaveToFile()
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

        /// <summary>
        /// Операция сохранения в базу данных
        /// </summary>
        private bool SaveToDB()
        {
            return false;
        }
    }

    public class ResultPrinter : IResultPrinter
    {
        /// <summary>
        /// Вывод результатов игры
        /// </summary>
        public void PrintResult()
        {
            PrintToConsole();
        }

        /// <summary>
        /// Операция вывода на консоль
        /// </summary>
        private bool PrintToConsole()
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

            Console.WriteLine(tanks + "; " + result + " ; " + DateTime.Now.ToString());

            return true;
        }
    }

    public class PrinterToSaverAdapter : IResultSaver
    {
        private ResultPrinter resultPrinter = new();

        public void SaveResult() 
        {
            resultPrinter.PrintResult();
        }
    }
}
