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
        const string BALLOON = @"11_Balloon\";
        const string LANE = @"12_Lane\";
        const string GENRE = @"13_Genre\";
        const string GAMEMODE = @"14_GameMode\";
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
        internal CTextureAf TxCAf(string FileName)
        {
            return CDTXMania.tテクスチャの生成Af(CSkin.Path(BASE + FileName));
        }
        internal CTexture TxCGen(string FileName)
        {
            return CDTXMania.tテクスチャの生成(CSkin.Path(BASE + GAME + GENRE + FileName + ".png"));
        }

        public void LoadTexture()
        {
            #region 共通
            Tile_Black = TxC(@"Tile_Black.png");
            Tile_White = TxC(@"Tile_White.png");
            Menu_Title = TxC(@"Menu_Title.png");
            #endregion
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

            #region 4_読み込み画面
            SongLoading_Plate = TxC(SONGLOADING + @"Plate.png");
            SongLoading_FadeIn = TxC(SONGLOADING + @"FadeIn.png");
            SongLoading_FadeOut = TxC(SONGLOADING + @"FadeOut.png");
            #endregion

            #region 5_演奏画面
            #region 共通
            Notes = TxC(GAME + @"Notes.png");
            Judge_Frame = TxC(GAME + @"Notes.png");
            SenNotes = TxC(GAME + @"SenNotes.png");
            Notes_Arm = TxC(GAME + @"Notes_Arm.png");
            Judge = TxC(GAME + @"Judge.png");

            Judge_Meter = TxC(GAME + @"Judge_Meter.png");
            Bar = TxC(GAME + @"Bar.png");
            Bar_Branch = TxC(GAME + @"Bar_Branch.png");
            
            #endregion
            #region キャラクター
            if (CDTXMania.ConfigIni.nCharaMotionCount != 0)
            {
                Chara_Normal = new CTexture[CDTXMania.ConfigIni.nCharaMotionCount];
                for (int i = 0; i < CDTXMania.ConfigIni.nCharaMotionCount; i++)
                {
                    Chara_Normal[i] = TxC(GAME + CHARA + @"Normal\" + i.ToString() + ".png");
                }
            }
            if (CDTXMania.ConfigIni.nCharaMotionCount_clear != 0)
            {
                Chara_Normal_Cleared = new CTexture[CDTXMania.ConfigIni.nCharaMotionCount_clear];
                for (int i = 0; i < CDTXMania.ConfigIni.nCharaMotionCount_clear; i++)
                {
                    Chara_Normal_Cleared[i] = TxC(GAME + CHARA + @"Clear\" + i.ToString() + ".png");
                }
            }
            if (CDTXMania.ConfigIni.nCharaMotionCount_max != 0)
            {
                Chara_Normal_Maxed = new CTexture[CDTXMania.ConfigIni.nCharaMotionCount_max];
                for (int i = 0; i < CDTXMania.ConfigIni.nCharaMotionCount_max; i++)
                {
                    Chara_Normal_Maxed[i] = TxC(GAME + CHARA + @"Clear_Max\" + i.ToString() + ".png");
                }
            }
            if (CDTXMania.ConfigIni.nCharaMotionCount_gogo != 0)
            {
                Chara_GoGoTime = new CTexture[CDTXMania.ConfigIni.nCharaMotionCount_gogo];
                for (int i = 0; i < CDTXMania.ConfigIni.nCharaMotionCount_gogo; i++)
                {
                    Chara_GoGoTime[i] = TxC(GAME + CHARA + @"GoGo\" + i.ToString() + ".png");
                }
            }
            if (CDTXMania.ConfigIni.nCharaMotionCount_maxgogo != 0)
            {
                Chara_GoGoTime_Maxed = new CTexture[CDTXMania.ConfigIni.nCharaMotionCount_maxgogo];
                for (int i = 0; i < CDTXMania.ConfigIni.nCharaMotionCount_maxgogo; i++)
                {
                    Chara_GoGoTime_Maxed[i] = TxC(GAME + CHARA + @"GoGo_Max\" + i.ToString() + ".png");
                }
            }

            if (CDTXMania.ConfigIni.nCharaAction_10combo != 0)
            {
                Chara_10Combo = new CTexture[CDTXMania.ConfigIni.nCharaAction_10combo];
                for (int i = 0; i < CDTXMania.ConfigIni.nCharaAction_10combo; i++)
                {
                    Chara_10Combo[i] = TxC(GAME + CHARA + @"10combo\" + i.ToString() + ".png");
                }
            }
            if (CDTXMania.ConfigIni.nCharaAction_10combo_max != 0)
            {
                Chara_10Combo_Maxed = new CTexture[CDTXMania.ConfigIni.nCharaAction_10combo_max];
                for (int i = 0; i < CDTXMania.ConfigIni.nCharaAction_10combo_max; i++)
                {
                    Chara_10Combo_Maxed[i] = TxC(GAME + CHARA + @"10combo_Max\" + i.ToString() + ".png");
                }
            }

            if (CDTXMania.ConfigIni.nCharaAction_gogostart != 0)
            {
                Chara_GoGoStart = new CTexture[CDTXMania.ConfigIni.nCharaAction_gogostart];
                for (int i = 0; i < CDTXMania.ConfigIni.nCharaAction_gogostart; i++)
                {
                    Chara_GoGoStart[i] = TxC(GAME + CHARA + @"GoGoStart\" + i.ToString() + ".png");
                }
            }
            if (CDTXMania.ConfigIni.nCharaAction_gogostart_max != 0)
            {
                Chara_GoGoStart_Maxed = new CTexture[CDTXMania.ConfigIni.nCharaAction_gogostart_max];
                for (int i = 0; i < CDTXMania.ConfigIni.nCharaAction_gogostart_max; i++)
                {
                    Chara_GoGoStart_Maxed[i] = TxC(GAME + CHARA + @"GoGoStart_Max\" + i.ToString() + ".png");
                }
            }
            if (CDTXMania.ConfigIni.nCharaAction_clearstart != 0)
            {
                Chara_Become_Cleared = new CTexture[CDTXMania.ConfigIni.nCharaAction_clearstart];
                for (int i = 0; i < CDTXMania.ConfigIni.nCharaAction_clearstart; i++)
                {
                    Chara_Become_Cleared[i] = TxC(GAME + CHARA + @"ClearStart\" + i.ToString() + ".png");
                }
            }
            if (CDTXMania.ConfigIni.nCharaAction_fullgauge != 0)
            {
                Chara_Become_Maxed = new CTexture[CDTXMania.ConfigIni.nCharaAction_fullgauge];
                for (int i = 0; i < CDTXMania.ConfigIni.nCharaAction_fullgauge; i++)
                {
                    Chara_Become_Maxed[i] = TxC(GAME + CHARA + @"FullGauge\" + i.ToString() + ".png");
                }
            }
            Chara_Balloon_Breaking = TxC(GAME + CHARA + @"Breaking.png");
            Chara_Balloon_Broken = TxC(GAME + CHARA + @"Broken.png");
            #endregion
            #region 踊り子
            if (CDTXMania.ConfigIni.nDancerMotionCount != 0)
            {
                Dancer_1 = new CTexture[CDTXMania.ConfigIni.nDancerMotionCount];
                Dancer_2 = new CTexture[CDTXMania.ConfigIni.nDancerMotionCount];
                Dancer_3 = new CTexture[CDTXMania.ConfigIni.nDancerMotionCount];
                Dancer_4 = new CTexture[CDTXMania.ConfigIni.nDancerMotionCount];
                Dancer_5 = new CTexture[CDTXMania.ConfigIni.nDancerMotionCount];
                for (int i = 0; i < CDTXMania.ConfigIni.nDancerMotionCount; i++)
                {
                    Dancer_1[i] = TxC(GAME + DANCER + @"1\" + i.ToString() + ".png");
                    Dancer_2[i] = TxC(GAME + DANCER + @"2\" + i.ToString() + ".png");
                    Dancer_3[i] = TxC(GAME + DANCER + @"3\" + i.ToString() + ".png");
                    Dancer_4[i] = TxC(GAME + DANCER + @"4\" + i.ToString() + ".png");
                    Dancer_5[i] = TxC(GAME + DANCER + @"5\" + i.ToString() + ".png");
                }
            }
            #endregion
            #region モブ
            Mob = new CTexture[30];
            for (int i = 0; i < 30; i++)
            {
                Mob[i] = TxC(GAME + MOB + i.ToString() + ".png");
            }
            #endregion
            #region フッター
            Mob_Footer = TxC(GAME + FOOTER + @"0.png");
            #endregion
            #region 背景
            Background_Up = TxC(GAME + BACKGROUND + @"0\" + @"Up.png");
            Background_Up_Clear = TxC(GAME + BACKGROUND + @"0\" + @"Up_Clear.png");
            Background_Down = TxC(GAME + BACKGROUND + @"0\" + @"Down.png");
            Background_Down_Clear = TxC(GAME + BACKGROUND + @"0\" + @"Down_Clear.png");
            Background_Down_Scroll = TxC(GAME + BACKGROUND + @"0\" + @"Down_Scroll.png");

            #endregion
            #region 太鼓
            Taiko_Background = new CTexture[2];
            Taiko_Background[0] = TxC(GAME + TAIKO + @"1P_Background.png");
            Taiko_Background[1] = TxC(GAME + TAIKO + @"2P_Background.png");
            Taiko_Frame = new CTexture[2];
            Taiko_Frame[0] = TxC(GAME + TAIKO + @"1P_Frame.png");
            Taiko_Frame[1] = TxC(GAME + TAIKO + @"2P_Frame.png");
            Taiko_PlayerNumber = new CTexture[2];
            Taiko_PlayerNumber[0] = TxC(GAME + TAIKO + @"1P_PlayerNumber.png");
            Taiko_PlayerNumber[1] = TxC(GAME + TAIKO + @"2P_PlayerNumber.png");
            Taiko_NamePlate = new CTexture[2];
            Taiko_NamePlate[0] = TxC(GAME + TAIKO + @"1P_NamePlate.png");
            Taiko_NamePlate[1] = TxC(GAME + TAIKO + @"2P_NamePlate.png");
            Taiko_Base = TxC(GAME + TAIKO + @"Base.png");
            Taiko_Don_Left = TxC(GAME + TAIKO + @"Don.png");
            Taiko_Don_Right = TxC(GAME + TAIKO + @"Don.png");
            Taiko_Ka_Left = TxC(GAME + TAIKO + @"Ka.png");
            Taiko_Ka_Right = TxC(GAME + TAIKO + @"Ka.png");
            Taiko_LevelUp = TxC(GAME + TAIKO + @"LevelUp.png");
            Taiko_LevelDown = TxC(GAME + TAIKO + @"LevelDown.png");
            Couse_Symbol = new CTexture[6];
            string[] Couse_Symbols = new string[6]{ "Easy", "Normal", "Hard", "Oni", "Edit", "Shin" };
            for (int i = 0; i < 6; i++)
            {
                Couse_Symbol[i] = TxC(GAME + COURSESYMBOL + Couse_Symbols[i] + ".png");
            }
            Taiko_Score = new CTexture[3];
            Taiko_Score[0] = TxC(GAME + TAIKO + @"Score.png");
            Taiko_Score[1] = TxC(GAME + TAIKO + @"Score_1P.png");
            Taiko_Score[2] = TxC(GAME + TAIKO + @"Score_2P.png");
            Taiko_Combo = new CTexture[2];
            Taiko_Combo[0] = TxC(GAME + TAIKO + @"Combo.png");
            Taiko_Combo[1] = TxC(GAME + TAIKO + @"Combo_Big.png");
            Taiko_Combo_Effect = TxC(GAME + TAIKO + @"Combo_Effect.png");
            #endregion
            #region ゲージ
            Gauge = new CTexture[2];
            Gauge[0] = TxC(GAME + GAUGE + @"1P.png");
            Gauge[1] = TxC(GAME + GAUGE + @"2P.png");
            Gauge_Base = new CTexture[2];
            Gauge_Base[0] = TxC(GAME + GAUGE + @"1P_Base.png");
            Gauge_Base[1] = TxC(GAME + GAUGE + @"2P_Base.png");
            Gauge_Line = new CTexture[2];
            Gauge_Line[0] = TxC(GAME + GAUGE + @"1P_Line.png");
            Gauge_Line[1] = TxC(GAME + GAUGE + @"2P_Line.png");
            Gauge_Rainbow = new CTexture[12];
            for (int i = 0; i < 12; i++)
            {
                Gauge_Rainbow[i] = TxC(GAME + GAUGE + @"Rainbow\" + i.ToString() + ".png");
            }
            Gauge_Soul = TxC(GAME + GAUGE + @"Soul.png");
            Gauge_Soul_Fire = TxC(GAME + GAUGE + @"Fire.png");
            Gauge_Soul_Explosion = TxC(GAME + GAUGE + @"Explosion.png");
            #endregion
            #region 吹き出し
            Balloon_Combo = new CTexture[2];
            Balloon_Combo[0] = TxC(GAME + BALLOON + @"Combo_1P.png");
            Balloon_Combo[1] = TxC(GAME + BALLOON + @"Combo_2P.png");
            Balloon_Roll = TxC(GAME + BALLOON + @"Roll.png");
            Balloon_Balloon = TxC(GAME + BALLOON + @"Balloon.png");
            Balloon_Number_Roll = TxC(GAME + BALLOON + @"Number_Roll.png");
            Balloon_Number_Combo = TxC(GAME + BALLOON + @"Number_Combo.png");

            Balloon_Breaking = new CTexture[6];
            for (int i = 0; i < 6; i++)
            {
                Balloon_Breaking[i] = TxC(GAME + BALLOON + @"Breaking_" + i.ToString() + ".png");
            }
            //Balloon_Broken = TxC(GAME + BALLOON + @"Broken.png");
            #endregion
            #region エフェクト
            Effects_Hit_Explosion = TxCAf(GAME + EFFECTS + @"Hit\Explosion.png");
            if (Effects_Hit_Explosion != null) Effects_Hit_Explosion.b加算合成 = true;
            Effects_Hit_Explosion_Big = TxC(GAME + EFFECTS + @"Hit\Explosion_Big.png");
            if (Effects_Hit_Explosion_Big != null) Effects_Hit_Explosion.b加算合成 = true;
            Effects_Hit_FireWorks = new CTextureAf[2];
            Effects_Hit_FireWorks[0] = TxCAf(GAME + EFFECTS + @"Hit\FireWorks_1P.png");
            if (Effects_Hit_FireWorks[0] != null) Effects_Hit_FireWorks[0].b加算合成 = true;
            Effects_Hit_FireWorks[1] = TxCAf(GAME + EFFECTS + @"Hit\FireWorks_2P.png");
            if (Effects_Hit_FireWorks[1] != null) Effects_Hit_FireWorks[1].b加算合成 = true;


            Effects_Fire = TxC(GAME + EFFECTS + @"Fire.png");
            if (Effects_Fire != null) Effects_Fire.b加算合成 = true;

            Effects_Rainbow = TxC(GAME + EFFECTS + @"Rainbow.png");

            Effects_Splash = new CTexture[30];
            for (int i = 0; i < 30; i++)
            {
                Effects_Splash[i] = TxC(GAME + EFFECTS + @"Splash\" + i.ToString() + ".png");
                if (Effects_Splash[i] != null) Effects_Splash[i].b加算合成 = true;
            }
            Effects_Hit_Great = new CTexture[15];
            Effects_Hit_Great_Big = new CTexture[15];
            Effects_Hit_Good = new CTexture[15];
            Effects_Hit_Good_Big = new CTexture[15];
            for (int i = 0; i < 15; i++)
            {
                Effects_Hit_Great[i] = TxC(GAME + EFFECTS + @"Hit\" + @"Great\" + i.ToString() + ".png");
                Effects_Hit_Great_Big[i] = TxC(GAME + EFFECTS + @"Hit\" + @"Great_Big\" + i.ToString() + ".png");
                Effects_Hit_Good[i] = TxC(GAME + EFFECTS + @"Hit\" + @"Good\" + i.ToString() + ".png");
                Effects_Hit_Good_Big[i] = TxC(GAME + EFFECTS + @"Hit\" + @"Good_Big\" + i.ToString() + ".png");
                //Effects_Hit_Great[i].b加算合成 = true;
                //Effects_Hit_Great_Big[i].b加算合成 = true;
                //Effects_Hit_Good[i].b加算合成 = true;
                //Effects_Hit_Good_Big[i].b加算合成 = true;
            }
            Effects_Roll = new CTexture[4];
            for (int i = 0; i < 4; i++)
            {
                Effects_Roll[i] = TxC(GAME + EFFECTS + @"Roll\" + i.ToString() + ".png");
            }
            #endregion
            #region レーン
                Lane_Base = new CTexture[3];
            Lane_Text = new CTexture[3];
            foreach (var item in new string[] { "Normal", "Expert", "Master"})
            {
                int num = 0;
                Lane_Base[num] = TxC(GAME + LANE + "Base_" + item + ".png");
                Lane_Text[num] = TxC(GAME + LANE + "Text_" + item + ".png");
                num++;
            }
            Lane_Red = TxC(GAME + LANE + @"Red.png");
            Lane_Blue = TxC(GAME + LANE + @"Blue.png");
            Lane_Yellow = TxC(GAME + LANE + @"Yellow.png");
            Lane_Background_Main = TxC(GAME + LANE + @"Background_Main.png");
            Lane_Background_Sub = TxC(GAME + LANE + @"Background_Sub.png");
            Lane_Background_GoGo = TxC(GAME + LANE + @"Background_GoGo.png");

            #endregion
            #region 終了演出
            End_Clear_L = new CTexture[5];
            End_Clear_R = new CTexture[5];
            for (int i = 0; i < 5; i++)
            {
                End_Clear_L[i] = TxC(GAME + END + @"Clear_L_" + i.ToString() + ".png");
                End_Clear_R[i] = TxC(GAME + END + @"Clear_R_" + i.ToString() + ".png");
            }
            End_Clear_Text = TxC(GAME + END + @"Clear_Text.png");
            End_Clear_Text_Effect = TxC(GAME + END + @"Clear_Text_Effect.png");
            if (End_Clear_Text_Effect != null) End_Clear_Text_Effect.b加算合成 = true;
            #endregion
            #region
            GameMode_Timer_Tick = TxC(GAME + GAMEMODE + @"Timer_Tick.png");
            GameMode_Timer_Frame = TxC(GAME + GAMEMODE + @"Timer_Frame.png");
            #endregion
            #endregion

            #region 6_結果発表
            Result_Background = TxC(RESULT + @"Background.png");
            Result_FadeIn = TxC(RESULT + @"FadeIn.png");
            Result_Gauge = TxC(RESULT + @"Gauge.png");
            Result_Gauge_Base = TxC(RESULT + @"Gauge_Base.png");
            Result_Judge = TxC(RESULT + @"Judge.png");
            Result_Header = TxC(RESULT + @"Header.png");
            Result_Number = TxC(RESULT + @"Number.png");
            Result_Panel = TxC(RESULT + @"Panel.png");
            Result_Score_Text = TxC(RESULT + @"Score_Text.png");
            Result_Score_Number = TxC(RESULT + @"Score_Number.png");
            #endregion

        }

        public void DisposeTexture()
        {
            Title_Background.Dispose();
            Title_Menu.Dispose();
        }

        #region 共通
        public CTexture Tile_Black,
            Tile_White,
            Menu_Title;
        #endregion
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

        #region 4_読み込み画面
        public CTexture SongLoading_Plate,
            SongLoading_FadeIn,
            SongLoading_FadeOut;
        #endregion

        #region 5_演奏画面
        #region 共通
        public CTexture Notes,
            Judge_Frame,
            SenNotes,
            Notes_Arm,
            Judge;
        public CTexture Judge_Meter,
            Bar,
            Bar_Branch;
        #endregion
        #region キャラクター
        public CTexture[] Chara_Normal,
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
        public CTexture Chara_Balloon_Breaking,
            Chara_Balloon_Broken;
        #endregion
        #region 踊り子
        public CTexture[] Dancer_1,
            Dancer_2,
            Dancer_3,
            Dancer_4,
            Dancer_5;
        #endregion
        #region モブ
        public CTexture[] Mob;
        public CTexture Mob_Footer;
        #endregion
        #region 背景
        public CTexture Background_Up,
            Background_Up_Clear,
            Background_Down,
            Background_Down_Clear,
            Background_Down_Scroll;
        #endregion
        #region 太鼓
        public CTexture[] Taiko_Frame, // MTaiko下敷き
            Taiko_Background;
        public CTexture Taiko_Base,
            Taiko_Don_Left,
            Taiko_Don_Right,
            Taiko_Ka_Left,
            Taiko_Ka_Right,
            Taiko_LevelUp,
            Taiko_LevelDown,
            Taiko_Combo_Effect;
        public CTexture[] Couse_Symbol, // コースシンボル
            Taiko_PlayerNumber,
            Taiko_NamePlate; // ネームプレート
        public CTexture[] Taiko_Score,
            Taiko_Combo;
        #endregion
        #region ゲージ
        public CTexture[] Gauge,
            Gauge_Base,
            Gauge_Line,
            Gauge_Rainbow;
        public CTexture Gauge_Soul,
            Gauge_Soul_Fire,
            Gauge_Soul_Explosion;
        #endregion
        #region 吹き出し
        public CTexture[] Balloon_Combo;
        public CTexture Balloon_Roll,
            Balloon_Balloon,
            Balloon_Number_Roll,
            Balloon_Number_Combo/*,*/
            /*Balloon_Broken*/;
        public CTexture[] Balloon_Breaking;
        #endregion
        #region エフェクト
        public CTexture Effects_Hit_Explosion,
            Effects_Hit_Explosion_Big,
            Effects_Fire,
            Effects_Rainbow;
        public CTexture[] Effects_Splash;
        public CTextureAf[] Effects_Hit_FireWorks;
        public CTexture[] Effects_Hit_Great,
            Effects_Hit_Good,
            Effects_Hit_Great_Big,
            Effects_Hit_Good_Big;
        public CTexture[] Effects_Roll;
        #endregion
        #region レーン
        public CTexture[] Lane_Base,
            Lane_Text;
        public CTexture Lane_Red,
            Lane_Blue,
            Lane_Yellow;
        public CTexture Lane_Background_Main,
            Lane_Background_Sub,
            Lane_Background_GoGo;
        #endregion
        #region 終了演出
        public CTexture[] End_Clear_L,
            End_Clear_R;
        public CTexture End_Clear_Text,
            End_Clear_Text_Effect;
        #endregion
        #region ゲームモード
        public CTexture GameMode_Timer_Frame,
            GameMode_Timer_Tick;
        #endregion
        #endregion

        #region 6_結果発表
        public CTexture Result_Background,
            Result_FadeIn,
            Result_Gauge,
            Result_Gauge_Base,
            Result_Judge,
            Result_Header,
            Result_Number,
            Result_Panel,
            Result_Score_Text,
            Result_Score_Number;
        #endregion
    }
}
