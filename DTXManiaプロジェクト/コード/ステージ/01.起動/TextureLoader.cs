using FDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTXMania
{
    class TextureLoader : CActivity
    {
        const string BASE = @"Graphics\";

        // Stage
        const string TITLE = @"1_Title\";
        const string CONFIG = @"2_Config\";
        const string SONGSELECT = @"3_SongSelect\";
        const string SONGLOADING = @"4_SongLoading\";
        const string GAME = @"5_Game\";
        const string RESULT = @"6_Result\";
        const string EXIT = @"7_Exit\";

        // InGame
        const string CHARA = @"1_Chara\";
        const string DANCER = @"2_Dancer\";
        const string MOB = @"3_Mob\";
        const string COURSESYMBOL = @"4_CourseSymbol\";
        const string BACKGROUND = @"5_Background\";
        const string TAIKO = @"6_Taiko\";
        const string GAUGE = @"7_Gauge\";
        const string FOOTER = @"8_Footer\";
        const string END = @"9_End\";
        const string EFFECTS = @"10_Effects\";
        // InGame_Effects
        const string FIRE = @"Fire\";
        const string HIT = @"Hit\";
        const string ROLL = @"Roll\";
        const string SPLASH = @"Splash\";


        public TextureLoader()
        {
            // コンストラクタ
            
        }

        internal CTexture TxC(string FileName)
        {
            return CDTXMania.tテクスチャの生成(CSkin.Path(BASE + FileName));
        }

        public void LoadTexture()
        {
            #region 1_タイトル画面
            Title_Background = TxC(TITLE + @"Background.png");
            Title_Menu = TxC(TITLE + @"Menu.png");
            #endregion

            #region 2_コンフィグ画面
            Config_Background = TxC(CONFIG + @"Background.png");
            Config_Cursor = TxC(CONFIG + @"Cursor.png");
            Config_ItemBox = TxC(CONFIG + @"ItemBox.png");
            Config_Arrow = TxC(CONFIG + @"Arrow.png");
            #endregion

            #region 3_選曲画面
            SongSelect_Background = TxC(SONGSELECT + @"Background.png");
            SongSelect_Header = TxC(SONGSELECT + @"Header.png");
            SongSelect_Footer = TxC(SONGSELECT + @"Footer.png");
            SongSelect_Difficulty = TxC(SONGSELECT + @"Difficulty.png");
            SongSelect_Auto = TxC(SONGSELECT + @"Auto.png");
            SongSelect_Level = TxC(SONGSELECT + @"Level.png");
            SongSelect_Branch = TxC(SONGSELECT + @"Branch.png");
            SongSelect_Branch_Text = TxC(SONGSELECT + @"Branch_Text.png");
            SongSelect_Bar_Center = TxC(SONGSELECT + @"Bar_Center.png");
            SongSelect_GenreText = TxC(SONGSELECT + @"GenreText.png");
            SongSelect_Cursor_Left = TxC(SONGSELECT + @"Cursor_Left.png");
            SongSelect_Cursor_Right = TxC(SONGSELECT + @"Cursor_Right.png");
            for (int i = 0; i < 9; i++)
            {
                SongSelect_Bar_Genre[i] = TxC(SONGSELECT + @"Bar_Genre_" + i.ToString() + ".png");
            }
            for (int i = 0; i < 4; i++)
            {
                SongSelect_ScoreWindow[i] = TxC(SONGSELECT + @"ScoreWindow_" + i.ToString() + ".png");
            }

            for (int i = 0; i < 9; i++)
            {
                SongSelect_GenreBack[i] = TxC(SONGSELECT + @"GenreBackground_" + i.ToString() + ".png");
            }
            SongSelect_ScoreWindow_Text = TxC(SONGSELECT + @"ScoreWindow_Text.png");
            #endregion

            #region 演奏画面
            //Taiko_Frame = TxC(@"Graphics\MTaiko\Frame.png");
            //Taiko_Base = TxC(@"");
            #endregion

        }

        public void DisposeTexture()
        {
            Title_Background.Dispose();
            Title_Menu.Dispose();
        }

        #region 1_タイトル画面
        public CTexture Title_Background,
            Title_Menu;
        #endregion

        #region 2_コンフィグ画面
        public CTexture Config_Background,
            Config_Cursor,
            Config_ItemBox,
            Config_Arrow;
        #endregion

        #region 3_選曲画面
        public CTexture SongSelect_Background,
            SongSelect_Header,
            SongSelect_Footer,
            SongSelect_Difficulty,
            SongSelect_Auto,
            SongSelect_Level,
            SongSelect_Branch,
            SongSelect_Branch_Text,
            SongSelect_Bar_Center,
            SongSelect_GenreText,
            SongSelect_Cursor_Left,
            SongSelect_Cursor_Right,
            SongSelect_ScoreWindow_Text;
        public CTexture[] SongSelect_GenreBack = new CTexture[9],
            SongSelect_ScoreWindow = new CTexture[4],
            SongSelect_Bar_Genre = new CTexture[9],
            SongSelect_NamePlate = new CTexture[1];
        #endregion

        #region 演奏画面
        public CTexture[] Chara_Normal, // キャラクター
            Chara_Normal_Cleared,
            Chara_Normal_Maxed,
            Chara_GoGoTime,
            Chara_GoGoTime_Maxed,
            Chara_10Combo,
            Chara_10Combo_Maxed,
            Chara_GoGoStart,
            Chara_GoGoStart_Maxed,
            Chara_Become_Cleared,
            Chara_Become_Maxed;
        public CTexture[] Dancer_1, // 踊り子
            Dancer_2,
            Dancer_3,
            Dancer_4,
            Dancer_5;
        public CTexture[] Mob; // モブ
        public CTexture[] Taiko_Mat; // MTaiko下敷き
        public CTexture Taiko_Frame, // MTaiko
            Taiko_Base,
            Taiko_Don_Left,
            Taiko_Don_Right,
            Taiko_Ka_Left,
            Taiko_Ka_Right,
            Taiko_LevelUp,
            Taiko_LevelDown;
        public CTexture[] Couse_Symbol, // コースシンボル
            Taiko_NamePlate; // ネームプレート
        

        #endregion
    }
}
