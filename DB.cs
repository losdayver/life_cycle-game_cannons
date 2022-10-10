using System;
using System.IO;

namespace game_cannons
{
    public static class DB
    {
        public static void SaveResult(string database)
        {
            if (database == "file")
            {
                SaveToFile();
            }
        }

        public static void SaveToFile()
        {
            string path = VARIABLES.DATABASEPATH + "FileDB.txt";
            FileInfo file = new(path);
            using (StreamWriter fstream = file.AppendText())
            {
                List<string> aliveTanks = new();
                string result = "";
                string tanks = "";
                for (int i = 0; i < Game.session.tanks.Count; i++)
                {
                    if (Game.session.tanks[i].status)
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

                fstream.WriteLine(tanks + "\t\t\t" + result);

            }
        }
    }
}
