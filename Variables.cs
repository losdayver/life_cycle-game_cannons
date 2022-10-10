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
    /// Данный статический класс отвечает за хранение информации о нажатых клавишах в приложении
    /// </summary>
    internal static class KEYS
    {
        public static bool KEY_UP = false;
        public static bool KEY_LEFT = false;
        public static bool KEY_DOWN = false;
        public static bool KEY_RIGHT = false;
        public static bool KEY_SPACE = false;

        public static bool MOUSE_LEFT = false;
        public static bool MOUSE_RIGHT = false;
    }

    /// <summary>
    /// Данный статический класс отвечает за хранение информации о общих переменных приложения, 
    /// которые не подходят не под один класс или используются в нескольких аспектах приложения
    /// с одинаковой частотой
    /// </summary>
    internal static class VARIABLES
    {
        public static string RESOURCEPATH = Environment.CurrentDirectory;
        public static string DATABASEPATH = Environment.CurrentDirectory;

        static VARIABLES()
        {
            RESOURCEPATH = RESOURCEPATH.Substring(0, RESOURCEPATH.Length - "bin\\Debug\\net6.0".Length);
            RESOURCEPATH += "\\resources\\";
            DATABASEPATH = DATABASEPATH.Substring(0, DATABASEPATH.Length - "bin\\Debug\\net6.0".Length);
            DATABASEPATH += "\\databases\\";
        }
    }

    /// <summary>
    /// Данный статический класс отвечает за хранения текстур типа SFML.Texture
    /// </summary>
    internal static class TEXTURES
    {
        public static Texture GREENTANKBODY;
        public static Texture TANKTRACKS;
        public static Texture TURRET;
        public static Texture LANDTEXTURE;
        public static Texture ARROW;
        public static Texture BACKGROUND;
        public static Texture BULLET;
        public static Texture ARROW_CURSOR;
        public static Texture ARROW_LEFT;
        public static Texture ARROW_RIGHT;
        public static Texture HP3;
        public static Texture HP2;
        public static Texture HP1;

        static TEXTURES()
        {
            GREENTANKBODY = new(VARIABLES.RESOURCEPATH + "Tanks&shells\\Default size\\tanks_tankGreen_body1.png");
            TANKTRACKS = new(VARIABLES.RESOURCEPATH + "Tanks&shells\\Default size\\tanks_tankTracks2.png");
            TURRET = new(VARIABLES.RESOURCEPATH + "Tanks&shells\\Default size\\tanks_turret4.png");
            LANDTEXTURE = new(VARIABLES.RESOURCEPATH + "lvl_cosmos\\moon.png");
            ARROW = new(VARIABLES.RESOURCEPATH + "Tanks&shells\\Default size\\tank_arrowFull.png");
            BACKGROUND = new(VARIABLES.RESOURCEPATH + "lvl_cosmos\\background_space_2.png");
            BULLET = new(VARIABLES.RESOURCEPATH + "Tanks&shells\\Default size\\bulletTest.png");
            ARROW_CURSOR = new(VARIABLES.RESOURCEPATH + "Icons\\arrow-cursor.png");
            ARROW_LEFT = new(VARIABLES.RESOURCEPATH + "Icons\\arrow_left.png");
            ARROW_RIGHT = new(VARIABLES.RESOURCEPATH + "Icons\\arrow_right.png");
            HP3 = new(VARIABLES.RESOURCEPATH + "Tanks&shells\\Default size\\hp3.png");
            HP2 = new(VARIABLES.RESOURCEPATH + "Tanks&shells\\Default size\\hp2.png");
            HP1 = new(VARIABLES.RESOURCEPATH + "Tanks&shells\\Default size\\hp1.png");

        }
    }
}
