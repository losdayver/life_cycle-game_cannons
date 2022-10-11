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
        public static bool GO_BACK = false;
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
        public static readonly string RESOURCEPATH = Environment.CurrentDirectory;
        public static readonly string DATABASEPATH = Environment.CurrentDirectory;
        public static readonly string SOUNDSPATH;
        public static readonly string DEFAULTFONT;

        static VARIABLES()
        {
            RESOURCEPATH = RESOURCEPATH.Substring(0, RESOURCEPATH.Length - "bin\\Debug\\net6.0".Length);
            RESOURCEPATH += "\\resources\\";
            DATABASEPATH = DATABASEPATH.Substring(0, DATABASEPATH.Length - "bin\\Debug\\net6.0".Length);
            DATABASEPATH += "\\databases\\";
            DEFAULTFONT = RESOURCEPATH + "ArialRegular.ttf";
            SOUNDSPATH = RESOURCEPATH + "Sounds\\";

        }
    }

    /// <summary>
    /// Данный статический класс отвечает за хранения текстур типа SFML.Texture
    /// </summary>
    internal static class TEXTURES
    {
        public static readonly Texture MENU_BACKGROUND;
        public static readonly Texture PLAY_BUTTON;
        public static readonly Texture STATISTICS_BUTTON;
        public static readonly Texture EXIT_BUTTON;
        public static readonly Texture GREENTANKBODY;
        public static readonly Texture TANKTRACKS;
        public static readonly Texture TURRET;
        public static readonly Texture LANDTEXTURE;
        public static readonly Texture ARROW;
        public static readonly Texture BACKGROUND;
        public static readonly Texture BULLET;
        public static readonly Texture ARROW_CURSOR;
        public static readonly Texture ARROW_LEFT;
        public static readonly Texture ARROW_RIGHT;
        public static readonly Texture PLACEHOLDER_BUTTON;

        static TEXTURES()
        {
            PLACEHOLDER_BUTTON = new(VARIABLES.RESOURCEPATH + "menu\\placeholder_button.png");
            MENU_BACKGROUND = new(VARIABLES.RESOURCEPATH + "menu\\menu_background.png");
            PLAY_BUTTON = new(VARIABLES.RESOURCEPATH + "menu\\play_button.png");
            STATISTICS_BUTTON = new(VARIABLES.RESOURCEPATH + "menu\\statistics_button.png");
            EXIT_BUTTON = new(VARIABLES.RESOURCEPATH + "menu\\exit_button.png");  
            GREENTANKBODY = new(VARIABLES.RESOURCEPATH + "Tanks&shells\\Default size\\tanks_tankGreen_body1.png");
            TANKTRACKS = new(VARIABLES.RESOURCEPATH + "Tanks&shells\\Default size\\tanks_tankTracks2.png");
            TURRET = new(VARIABLES.RESOURCEPATH + "Tanks&shells\\Default size\\tanks_turret4.png");
            LANDTEXTURE = new(VARIABLES.RESOURCEPATH + "lvl_cosmos\\moon.png");
            ARROW = new(VARIABLES.RESOURCEPATH + "Tanks&shells\\Default size\\tank_arrowFull.png");
            BACKGROUND = new(VARIABLES.RESOURCEPATH + "lvl_cosmos\\background_space.png");
            BULLET = new(VARIABLES.RESOURCEPATH + "Tanks&shells\\Default size\\bulletTest.png");
            ARROW_CURSOR = new(VARIABLES.RESOURCEPATH + "Icons\\arrow-cursor.png");
            ARROW_LEFT = new(VARIABLES.RESOURCEPATH + "Icons\\arrow_left.png");
            ARROW_RIGHT = new(VARIABLES.RESOURCEPATH + "Icons\\arrow_right.png");
        }
    }
}
