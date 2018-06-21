using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Diagnostics;
using FDK;


namespace DTXMania
{
    //クラスの設置位置は必ず演奏画面共通に置くこと。
    //そうしなければBPM変化に対応できません。

    //完成している部分は以下のとおり。(画像完成+動作確認完了で完成とする)
    //_通常モーション
    //_ゴーゴータイムモーション
    //_クリア時モーション
    //
    internal class CAct演奏Drumsキャラクター : CActivity
    {
        public CAct演奏Drumsキャラクター()
        {

        }

        public override void On活性化()
        {
            //this.ct通常モーション = new CCounter();
            //this.ctゴーゴーモーション = new CCounter();
            //this.ctクリア通常モーション = new CCounter();
            ctChara_Normal = new CCounter();
            ctChara_GoGo = new CCounter();
            ctChara_Clear = new CCounter();
            //// ↓踊り子・モブ↓
            //this.ct踊り子モーション = new CCounter();
            //this.ctモブモーション = new CCounter();
            // ↑踊り子・モブ↑

            //this.ctキャラクターアクションタイマ = new CCounter();
            this.ctキャラクターアクション_10コンボ = new CCounter();
            this.ctキャラクターアクション_10コンボMAX = new CCounter();
            this.ctキャラクターアクション_ゴーゴースタート = new CCounter();
            this.ctキャラクターアクション_ゴーゴースタートMAX = new CCounter();
            this.ctキャラクターアクション_ノルマ = new CCounter();
            this.ctキャラクターアクション_魂MAX = new CCounter();

            this.b風船連打中 = false;
            this.b演奏中 = false;

            this.bマイどんアクション中 = false;
            this.db前回のゲージ値 = 0D;

            base.On活性化();
        }

        public override void On非活性化()
        {
            //this.ct通常モーション = null;
            //this.ctゴーゴーモーション = null;
            //this.ctクリア通常モーション = null;
            ctChara_Normal = null;
            ctChara_GoGo = null;
            ctChara_Clear = null;
            //// ↓踊り子・モブ↓
            //this.ct踊り子モーション = null;
            //this.ctモブモーション = null;
            // ↑踊り子・モブ↑

            //this.ctキャラクターアクションタイマ = null;
            this.ctキャラクターアクション_10コンボ = null;
            this.ctキャラクターアクション_10コンボMAX = null;
            this.ctキャラクターアクション_ゴーゴースタート = null;
            this.ctキャラクターアクション_ゴーゴースタートMAX = null;
            this.ctキャラクターアクション_ノルマ = null;
            this.ctキャラクターアクション_魂MAX = null;
       
            base.On非活性化();
        }

        public override void OnManagedリソースの作成()
        {
            //キャラクター画像を格納しているディレクトリに各モーションの画像がいくつあるかを検索。
            //現在は固定値。
            //this.nキャラクター通常モーション枚数 = CDTXMania.ConfigIni.nCharaMotionCount;
            //this.nキャラクターゴーゴーモーション枚数 = CDTXMania.ConfigIni.nCharaMotionCount_gogo;
            //this.nキャラクタークリアモーション枚数 = CDTXMania.ConfigIni.nCharaMotionCount_clear;
            //this.nキャラクターMAX通常モーション枚数 = CDTXMania.ConfigIni.nCharaMotionCount_max;
            //this.nキャラクターMAXゴーゴーモーション枚数 = CDTXMania.ConfigIni.nCharaMotionCount_maxgogo;

            // ↓踊り子・モブ↓
            ////this.n踊り子モーション枚数 = 28;
            //this.nモブモーション枚数 = 30;

            ////this.str踊り子リスト = "0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27";//,26,25,24,23,22,21,20,19,18,17,16,15,14,13,12,11,10,9,8,7,6,5,4,3,2,1";
            //this.strモブ = "0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,28,27,26,25,24,23,22,21,20,19,18,17,16,15,14,13,12,11,10,9,8,7,6,5,4,3,2,1,0";

            ////this.ar踊り子モーション番号 = C変換.ar配列形式のstringをint配列に変換して返す(str踊り子リスト);
            //this.arモブモーション番号 = C変換.ar配列形式のstringをint配列に変換して返す(strモブ);

            //this.tx踊り子_1 = new CTexture[this.n踊り子モーション枚数];
            //for (int i = 0; i < this.n踊り子モーション枚数; i++)
            //{
            //    this.tx踊り子_1[i] = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\Dancer\0\" + i.ToString() + ".png"));
            //}
            //this.tx踊り子_2 = new CTexture[this.n踊り子モーション枚数];
            //for (int i = 0; i < this.n踊り子モーション枚数; i++)
            //{
            //    this.tx踊り子_2[i] = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\Dancer\1\" + i.ToString() + ".png"));
            //}
            //this.tx踊り子_3 = new CTexture[this.n踊り子モーション枚数];
            //for (int i = 0; i < this.n踊り子モーション枚数; i++)
            //{
            //    this.tx踊り子_3[i] = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\Dancer\2\" + i.ToString() + ".png"));
            //}
            //this.tx踊り子_4 = new CTexture[this.n踊り子モーション枚数];
            //for (int i = 0; i < this.n踊り子モーション枚数; i++)
            //{
            //    this.tx踊り子_4[i] = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\Dancer\3\" + i.ToString() + ".png"));
            //}
            //this.tx踊り子_5 = new CTexture[this.n踊り子モーション枚数];
            //for (int i = 0; i < this.n踊り子モーション枚数; i++)
            //{
            //    this.tx踊り子_5[i] = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\Dancer\4\" + i.ToString() + ".png"));
            //}
            //this.txモブ = new CTexture[this.nモブモーション枚数];
            //for (int i = 0; i < this.nモブモーション枚数; i++)
            //{
            //    this.txモブ[i] = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\Dancer\mob\" + i.ToString() + ".png"));
            //}

            //this.txフッター = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\Dancer_BG\footer\01.png"));
            //// ↑踊り子・モブ↑

            //this.txキャラクターNormal = new CTexture[ this.nキャラクター通常モーション枚数 ];
            //for( int i = 0; i < this.nキャラクター通常モーション枚数; i++ )
            //{
            //    this.txキャラクターNormal[ i ] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\Chara\Normal\" + i.ToString() + ".png" ) );
            //}

            //this.txキャラクターGogo = new CTexture[ this.nキャラクターゴーゴーモーション枚数 ];
            //for( int i = 0; i < this.nキャラクターゴーゴーモーション枚数; i++ )
            //{
            //    this.txキャラクターGogo[ i ] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\Chara\GoGo\" + i.ToString() + ".png" ) );
            //}

            //if( this.nキャラクタークリアモーション枚数 != 0 )
            //{
            //    this.txキャラクターClear_Normal = new CTexture[ this.nキャラクタークリアモーション枚数 ];
            //    for( int i = 0; i < this.nキャラクタークリアモーション枚数; i++ )
            //    {
            //        this.txキャラクターClear_Normal[ i ] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\Chara\Clear\" + i.ToString() + ".png" ) );
            //    }
            //}

            //if( this.nキャラクターMAX通常モーション枚数 != 0 )
            //{
            //    this.txキャラクターMax_Normal = new CTexture[ this.nキャラクターMAX通常モーション枚数 ];
            //    for( int i = 0; i < this.nキャラクターMAX通常モーション枚数; i++ )
            //    {
            //        this.txキャラクターMax_Normal[ i ] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\Chara\Clear_Max\" + i.ToString() + ".png" ) );
            //    }
            //}
            
            //if( this.nキャラクターMAXゴーゴーモーション枚数 != 0 )
            //{
            //    this.txキャラクターMax_Gogo = new CTexture[ this.nキャラクターMAXゴーゴーモーション枚数 ];
            //    for( int i = 0; i < this.nキャラクターMAXゴーゴーモーション枚数; i++ )
            //    {
            //        this.txキャラクターMax_Gogo[ i ] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\Chara\GoGo_Max\" + i.ToString() + ".png" ) );
            //    }
            //}

            // ↓マイどんアクション↓
            //this.nキャラクターアクション_10コンボ枚数 = CDTXMania.ConfigIni.nCharaAction_10combo;
            //this.nキャラクターアクション_10コンボMAX枚数 = CDTXMania.ConfigIni.nCharaAction_10combo_max;
            //this.nキャラクターアクション_ゴーゴースタート枚数 = CDTXMania.ConfigIni.nCharaAction_gogostart;
            //this.nキャラクターアクション_ゴーゴースタートMAX枚数 = CDTXMania.ConfigIni.nCharaAction_gogostart_max;
            //this.nキャラクターアクション_ノルマ枚数 = CDTXMania.ConfigIni.nCharaAction_clearstart;
            //this.nキャラクターアクション_魂MAX枚数 = CDTXMania.ConfigIni.nCharaAction_fullgauge;

            //if (this.nキャラクターアクション_10コンボ枚数 != 0)
            //{
            //    this.txキャラクターアクション_10コンボ = new CTexture[this.nキャラクターアクション_10コンボ枚数];
            //    for(int i = 0; i < this.nキャラクターアクション_10コンボ枚数; i++)
            //    {
            //        this.txキャラクターアクション_10コンボ[i] = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\Chara\10combo\" + i.ToString() + ".png"));
            //    }
            //}
            //if (this.nキャラクターアクション_10コンボMAX枚数 != 0)
            //{
            //    this.txキャラクターアクション_10コンボMAX = new CTexture[this.nキャラクターアクション_10コンボMAX枚数];
            //    for (int i = 0; i < this.nキャラクターアクション_10コンボMAX枚数; i++)
            //    {
            //        this.txキャラクターアクション_10コンボMAX[i] = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\Chara\10combo_Max\" + i.ToString() + ".png"));
            //    }
            //}
            //if (this.nキャラクターアクション_ゴーゴースタート枚数 != 0)
            //{
            //    this.txキャラクターアクション_ゴーゴースタート = new CTexture[this.nキャラクターアクション_ゴーゴースタート枚数];
            //    for (int i = 0; i < this.nキャラクターアクション_ゴーゴースタート枚数; i++)
            //    {
            //        this.txキャラクターアクション_ゴーゴースタート[i] = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\Chara\GoGoStart\" + i.ToString() + ".png"));
            //    }
            //}
            //if (this.nキャラクターアクション_ゴーゴースタートMAX枚数 != 0)
            //{
            //    this.txキャラクターアクション_ゴーゴースタートMAX = new CTexture[this.nキャラクターアクション_ゴーゴースタートMAX枚数];
            //    for (int i = 0; i < this.nキャラクターアクション_ゴーゴースタートMAX枚数; i++)
            //    {
            //        this.txキャラクターアクション_ゴーゴースタートMAX[i] = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\Chara\GoGoStart_Max\" + i.ToString() + ".png"));
            //    }
            //}
            //if (this.nキャラクターアクション_ノルマ枚数 != 0)
            //{
            //    this.txキャラクターアクション_ノルマ = new CTexture[this.nキャラクターアクション_ノルマ枚数];
            //    for (int i = 0; i < this.nキャラクターアクション_ノルマ枚数; i++)
            //    {
            //        this.txキャラクターアクション_ノルマ[i] = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\Chara\ClearStart\" + i.ToString() + ".png"));
            //    }
            //}
            //if (this.nキャラクターアクション_魂MAX枚数 != 0)
            //{
            //    this.txキャラクターアクション_魂MAX = new CTexture[this.nキャラクターアクション_魂MAX枚数];
            //    for (int i = 0; i < this.nキャラクターアクション_魂MAX枚数; i++)
            //    {
            //        this.txキャラクターアクション_魂MAX[i] = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\Chara\FullGauge\" + i.ToString() + ".png"));
            //    }
            //}
            // ↑マイどんアクション↑


            //固定値
            //this.arゴーゴーモーション番号 = new int[]{ 0, 1, 2, 3, 3, 3, 3, 3, 3, 2, 1, 0, 0, 0, 0, 0 };

            //2015.08.05 Config.iniから変更可能にするための実験
            this.arモーション番号 = C変換.ar配列形式のstringをint配列に変換して返す( CDTXMania.Skin.Game_Chara_Motion_Normal);
            this.arゴーゴーモーション番号 = C変換.ar配列形式のstringをint配列に変換して返す(CDTXMania.Skin.Game_Chara_Motion_GoGo);
            this.arクリアモーション番号 = C変換.ar配列形式のstringをint配列に変換して返す(CDTXMania.Skin.Game_Chara_Motion_Clear);
            //this.ar黄色モーション番号 = C変換.ar配列形式のstringをint配列に変換して返す(CDTXMania.Skin.Game_Chara_Motion_Clear);
            //this.ar黄色ゴーゴーモーション番号 = C変換.ar配列形式のstringをint配列に変換して返す(CDTXMania.Skin.Game_Chara_Motion_GoGo);

            //this.ct通常モーション = new CCounter( 0, this.arモーション番号.Length - 1, 0.02, CSound管理.rc演奏用タイマ );
            //this.ctゴーゴーモーション = new CCounter( 0, this.arゴーゴーモーション番号.Length - 1, 50, CDTXMania.Timer );
            //if( this.nキャラクタークリアモーション枚数 != 0 )
            //    this.ctクリア通常モーション = new CCounter( 0, this.arクリアモーション番号.Length - 1, 50, CDTXMania.Timer );
            //if( this.nキャラクターMAX通常モーション枚数 != 0 )
            //    this.ctMAX通常モーション = new CCounter( 0, this.ar黄色モーション番号.Length - 1, 50, CDTXMania.Timer );
            //if( this.nキャラクターMAXゴーゴーモーション枚数 != 0 )
            //    this.ctMAXゴーゴーモーション = new CCounter( 0, this.ar黄色ゴーゴーモーション番号.Length - 1, 50, CDTXMania.Timer );
            //this.ct踊り子モーション = new CCounter(0, this.ar踊り子モーション番号.Length - 1, 0.04, CSound管理.rc演奏用タイマ);
            //this.ctモブモーション = new CCounter(0, this.arモブモーション番号.Length - 1, 0.04, CSound管理.rc演奏用タイマ);
            if (arモーション番号 == null) this.arモーション番号 = C変換.ar配列形式のstringをint配列に変換して返す("0,0");
            if (arゴーゴーモーション番号 == null) this.arゴーゴーモーション番号 = C変換.ar配列形式のstringをint配列に変換して返す("0,0");
            if (arクリアモーション番号 == null) this.arクリアモーション番号 = C変換.ar配列形式のstringをint配列に変換して返す("0,0");
            ctChara_Normal = new CCounter(0, arモーション番号.Length - 1, 10, CSound管理.rc演奏用タイマ);
            ctChara_GoGo = new CCounter(0, arゴーゴーモーション番号.Length - 1, 10, CSound管理.rc演奏用タイマ);
            ctChara_Clear = new CCounter(0, arクリアモーション番号.Length - 1, 10, CSound管理.rc演奏用タイマ);
            base.OnManagedリソースの作成();
        }

        public override void OnManagedリソースの解放()
        {
            //for( int i = 0; i < this.nキャラクター通常モーション枚数; i++ )
            //{
            //    CDTXMania.tテクスチャの解放( ref this.txキャラクターNormal[ i ] );
            //}
            //for( int i = 0; i < this.nキャラクターゴーゴーモーション枚数; i++ )
            //{
            //    CDTXMania.tテクスチャの解放( ref this.txキャラクターGogo[ i ] );
            //}
            //if( this.nキャラクタークリアモーション枚数 != 0 )
            //{
            //    for( int i = 0; i < this.nキャラクタークリアモーション枚数; i++ )
            //    {
            //        CDTXMania.tテクスチャの解放( ref this.txキャラクターClear_Normal[ i ] );
            //    }
            //}
            //if( this.nキャラクターMAX通常モーション枚数 != 0 )
            //{
            //    for( int i = 0; i < this.nキャラクターMAX通常モーション枚数; i++ )
            //    {
            //        CDTXMania.tテクスチャの解放( ref this.txキャラクターMax_Normal[ i ] );
            //    }
            //}
            //if( this.nキャラクターMAXゴーゴーモーション枚数 != 0 )
            //{
            //    for( int i = 0; i < this.nキャラクターMAXゴーゴーモーション枚数; i++ )
            //    {
            //        CDTXMania.tテクスチャの解放( ref this.txキャラクターMax_Gogo[ i ] );
            //    }
            //}

            // ↓踊り子・モブ↓
            //if(this.n踊り子モーション枚数 != 0)
            //{
            //    for(int i = 0; i < this.n踊り子モーション枚数; i++)
            //    {
            //        CDTXMania.tテクスチャの解放(ref this.tx踊り子_1[i]);
            //        CDTXMania.tテクスチャの解放(ref this.tx踊り子_2[i]);
            //        CDTXMania.tテクスチャの解放(ref this.tx踊り子_3[i]);
            //        CDTXMania.tテクスチャの解放(ref this.tx踊り子_4[i]);
            //        CDTXMania.tテクスチャの解放(ref this.tx踊り子_5[i]);
            //    }
            //}
            //if(this.nモブモーション枚数 != 0)
            //{
            //    for(int i = 0; i < this.nモブモーション枚数; i++)
            //    {
            //        CDTXMania.tテクスチャの解放(ref this.txモブ[i]);
            //    }
            //}
            //// ↑踊り子・モブ↑

            //// ↓マイどんアクション↓
            //if (this.nキャラクターアクション_10コンボ枚数 != 0)
            //{
            //    for (int i = 0; i < this.nキャラクターアクション_10コンボ枚数; i++)
            //    {
            //        CDTXMania.tテクスチャの解放(ref this.txキャラクターアクション_10コンボ[i]);
            //    }
            //}
            //if (this.nキャラクターアクション_10コンボMAX枚数 != 0)
            //{
            //    for (int i = 0; i < this.nキャラクターアクション_10コンボMAX枚数; i++)
            //    {
            //        CDTXMania.tテクスチャの解放(ref this.txキャラクターアクション_10コンボMAX[i]);
            //    }
            //}
            //if (this.nキャラクターアクション_ゴーゴースタート枚数 != 0)
            //{
            //    for (int i = 0; i < this.nキャラクターアクション_ゴーゴースタート枚数; i++)
            //    {
            //        CDTXMania.tテクスチャの解放(ref this.txキャラクターアクション_ゴーゴースタート[i]);
            //    }
            //}
            //if (this.nキャラクターアクション_ゴーゴースタートMAX枚数 != 0)
            //{
            //    for (int i = 0; i < this.nキャラクターアクション_ゴーゴースタートMAX枚数; i++)
            //    {
            //        CDTXMania.tテクスチャの解放(ref this.txキャラクターアクション_ゴーゴースタートMAX[i]);
            //    }
            //}
            //if(this.nキャラクターアクション_ノルマ枚数 != 0)
            //{
            //    for (int i = 0; i < this.nキャラクターアクション_ノルマ枚数; i++)
            //    {
            //        CDTXMania.tテクスチャの解放(ref this.txキャラクターアクション_ノルマ[i]);
            //    }
            //}
            //if(this.nキャラクターアクション_魂MAX枚数 != 0)
            //{
            //    for (int i = 0; i < this.nキャラクターアクション_魂MAX枚数; i++) {
            //        CDTXMania.tテクスチャの解放(ref this.txキャラクターアクション_魂MAX[i]);
            //    }
            //}
            //// ↑マイどんアクション↑

            //CDTXMania.tテクスチャの解放(ref this.txフッター);

            //Trace.TraceInformation("CA リソースの開放");
            base.OnManagedリソースの解放();
        }

        public override int On進行描画()
        {
            //if( this.ct通常モーション != null ) this.ct通常モーション.t進行LoopDb();
            //if( this.ctクリア通常モーション != null ) this.ctクリア通常モーション.t進行LoopDb();
            //if( this.ctMAX通常モーション != null ) this.ctMAX通常モーション.t進行LoopDb();
            //if( this.ctMAXゴーゴーモーション != null ) this.ctMAXゴーゴーモーション.t進行LoopDb();
            //if( this.ctキャラクターアクションタイマ != null) this.ctキャラクターアクションタイマ.t進行db();
            if (ctChara_Normal != null || CDTXMania.Skin.Game_Chara_Ptn_Normal != 0) ctChara_Normal.t進行LoopDb();
            if (ctChara_GoGo != null || CDTXMania.Skin.Game_Chara_Ptn_GoGo != 0) ctChara_GoGo.t進行LoopDb();
            if (ctChara_Clear != null || CDTXMania.Skin.Game_Chara_Ptn_Clear != 0) ctChara_Clear.t進行LoopDb();
            if (this.ctキャラクターアクション_10コンボ != null || CDTXMania.Skin.Game_Chara_Ptn_10combo != 0) this.ctキャラクターアクション_10コンボ.t進行db();
            if (this.ctキャラクターアクション_10コンボMAX != null || CDTXMania.Skin.Game_Chara_Ptn_10combo_Max != 0) this.ctキャラクターアクション_10コンボMAX.t進行db();
            if (this.ctキャラクターアクション_ゴーゴースタート != null || CDTXMania.Skin.Game_Chara_Ptn_GoGoStart != 0) this.ctキャラクターアクション_ゴーゴースタート.t進行db();
            if (this.ctキャラクターアクション_ゴーゴースタートMAX != null || CDTXMania.Skin.Game_Chara_Ptn_GoGoStart_Max != 0) this.ctキャラクターアクション_ゴーゴースタートMAX.t進行db();
            if (this.ctキャラクターアクション_ノルマ != null || CDTXMania.Skin.Game_Chara_Ptn_ClearIn != 0) this.ctキャラクターアクション_ノルマ.t進行db();
            if (this.ctキャラクターアクション_魂MAX != null || CDTXMania.Skin.Game_Chara_Ptn_SoulIn != 0) this.ctキャラクターアクション_魂MAX.t進行db();

            // ↓踊り子・モブ↓
            //if (this.ct踊り子モーション != null) this.ct踊り子モーション.t進行LoopDb();
            //if (this.ctモブモーション != null) this.ctモブモーション.t進行LoopDb();
            // ↑踊り子・モブ↑

            if ( this.b風船連打中 != true && this.bマイどんアクション中 != true)
            {
                if ( !CDTXMania.stage演奏ドラム画面.bIsGOGOTIME[ 0 ] )
                {
                    if( CDTXMania.stage演奏ドラム画面.actGauge.db現在のゲージ値[ 0 ] >= 100.0 && CDTXMania.Skin.Game_Chara_Ptn_Clear != 0 )
                    {
                        if(CDTXMania.Skin.Game_Chara_Ptn_Clear != 0)
                            CDTXMania.Tx.Chara_Normal_Maxed[ this.arクリアモーション番号[(int)this.ctChara_Clear.db現在の値] ].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.Game_Chara_X[0], CDTXMania.Skin.Game_Chara_Y[0] );
                    }
                    else if( CDTXMania.stage演奏ドラム画面.actGauge.db現在のゲージ値[ 0 ] >= 80.0 && CDTXMania.Skin.Game_Chara_Ptn_Clear != 0 )
                    {
                        if(CDTXMania.Skin.Game_Chara_Ptn_Clear != 0)
                        {
                            CDTXMania.Tx.Chara_Normal_Cleared[ this.arクリアモーション番号[ (int)this.ctChara_Clear.db現在の値 ] ].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.Game_Chara_X[0], CDTXMania.Skin.Game_Chara_Y[0] );
                        }
                    }
                    else
                    {
                        if (CDTXMania.Skin.Game_Chara_Ptn_Normal != 0)
                        {
                            CDTXMania.Tx.Chara_Normal[ this.arモーション番号[ (int)this.ctChara_Normal.db現在の値 ] ].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.Game_Chara_X[0], CDTXMania.Skin.Game_Chara_Y[0] );
                        }
                    }
                }
                else
                {
                    if( CDTXMania.stage演奏ドラム画面.actGauge.db現在のゲージ値[ 0 ] >= 100.0 && CDTXMania.Skin.Game_Chara_Ptn_GoGo != 0 )
                    {
                        if(CDTXMania.Skin.Game_Chara_Ptn_GoGo != 0)
                            CDTXMania.Tx.Chara_GoGoTime_Maxed[this.arゴーゴーモーション番号[(int)this.ctChara_GoGo.db現在の値] ].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.Game_Chara_X[0], CDTXMania.Skin.Game_Chara_Y[0] );
                    }
                    else
                    {
                        if(CDTXMania.Skin.Game_Chara_Ptn_GoGo != 0)
                            CDTXMania.Tx.Chara_GoGoTime[ this.arゴーゴーモーション番号[ (int)this.ctChara_GoGo.db現在の値 ] ].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.Game_Chara_X[0], CDTXMania.Skin.Game_Chara_Y[0] );
                    }
                }
            
            }
            if (this.b風船連打中 != true && bマイどんアクション中 == true)
            {

                if (this.ctキャラクターアクション_10コンボ.b進行中db)
                {
                    if(CDTXMania.Tx.Chara_10Combo[0] != null && CDTXMania.Skin.Game_Chara_Ptn_10combo != 0)
                    {
                        CDTXMania.Tx.Chara_10Combo[(int)this.ctキャラクターアクション_10コンボ.db現在の値].t2D描画(CDTXMania.app.Device, CDTXMania.Skin.Game_Chara_X[0], CDTXMania.Skin.Game_Chara_Y[0] );
                    }
                    if (this.ctキャラクターアクション_10コンボ.b終了値に達したdb)
                    {
                        this.bマイどんアクション中 = false;
                        this.ctキャラクターアクション_10コンボ.t停止();
                        //this.ctキャラクターアクション_10コンボ.t停止();
                        this.ctキャラクターアクション_10コンボ.db現在の値 = 0D;
                    }
                }
                

                if (this.ctキャラクターアクション_10コンボMAX.b進行中db)
                {
                    if (CDTXMania.Tx.Chara_10Combo_Maxed[0] != null && CDTXMania.Skin.Game_Chara_Ptn_10combo_Max != 0)
                    {
                        CDTXMania.Tx.Chara_10Combo_Maxed[(int)this.ctキャラクターアクション_10コンボMAX.db現在の値].t2D描画(CDTXMania.app.Device, CDTXMania.Skin.Game_Chara_X[0], CDTXMania.Skin.Game_Chara_Y[0]);
                    }
                    if (this.ctキャラクターアクション_10コンボMAX.b終了値に達したdb)
                    {
                        this.bマイどんアクション中 = false;
                        this.ctキャラクターアクション_10コンボMAX.t停止();
                        //this.ctキャラクターアクション_10コンボ.t停止();
                        this.ctキャラクターアクション_10コンボMAX.db現在の値 = 0D;
                    }

                }

                if (this.ctキャラクターアクション_ゴーゴースタート.b進行中db)
                {
                    if (CDTXMania.Tx.Chara_GoGoStart[0] != null && CDTXMania.Skin.Game_Chara_Ptn_GoGoStart != 0)
                    {
                        CDTXMania.Tx.Chara_GoGoStart[(int)this.ctキャラクターアクション_ゴーゴースタート.db現在の値].t2D描画(CDTXMania.app.Device, CDTXMania.Skin.Game_Chara_X[0], CDTXMania.Skin.Game_Chara_Y[0]);
                    }
                    if (this.ctキャラクターアクション_ゴーゴースタート.b終了値に達したdb)
                    {
                        this.bマイどんアクション中 = false;
                        this.ctキャラクターアクション_ゴーゴースタート.t停止();
                        this.ctキャラクターアクション_ゴーゴースタート.db現在の値 = 0D;
                        this.ctChara_GoGo.db現在の値 = CDTXMania.Skin.Game_Chara_Ptn_GoGo / 2;
                    }
                }

                if (this.ctキャラクターアクション_ゴーゴースタートMAX.b進行中db)
                {
                    if (CDTXMania.Tx.Chara_GoGoStart_Maxed[0] != null && CDTXMania.Skin.Game_Chara_Ptn_GoGoStart_Max != 0)
                    {
                        CDTXMania.Tx.Chara_GoGoStart_Maxed[(int)this.ctキャラクターアクション_ゴーゴースタートMAX.db現在の値].t2D描画(CDTXMania.app.Device, CDTXMania.Skin.Game_Chara_X[0], CDTXMania.Skin.Game_Chara_Y[0]);
                    }
                    if (this.ctキャラクターアクション_ゴーゴースタートMAX.b終了値に達したdb)
                    {
                        this.bマイどんアクション中 = false;
                        this.ctキャラクターアクション_ゴーゴースタートMAX.t停止();
                        this.ctキャラクターアクション_ゴーゴースタートMAX.db現在の値 = 0D;
                        this.ctChara_GoGo.db現在の値 = CDTXMania.Skin.Game_Chara_Ptn_GoGo / 2;
                    }
                }

                if (this.ctキャラクターアクション_ノルマ.b進行中db)
                {
                    if (CDTXMania.Tx.Chara_Become_Cleared[0] != null && CDTXMania.Skin.Game_Chara_Ptn_ClearIn != 0)
                    {
                        CDTXMania.Tx.Chara_Become_Cleared[(int)this.ctキャラクターアクション_ノルマ.db現在の値].t2D描画(CDTXMania.app.Device, CDTXMania.Skin.Game_Chara_X[0], CDTXMania.Skin.Game_Chara_Y[0]);
                    }
                    if (this.ctキャラクターアクション_ノルマ.b終了値に達したdb)
                    {
                        this.bマイどんアクション中 = false;
                        this.ctキャラクターアクション_ノルマ.t停止();
                        this.ctキャラクターアクション_ノルマ.db現在の値 = 0D;
                    }
                }

                if (this.ctキャラクターアクション_魂MAX.b進行中db)
                {
                    if (CDTXMania.Tx.Chara_Become_Maxed[0] != null && CDTXMania.Skin.Game_Chara_Ptn_SoulIn != 0)
                    {
                        CDTXMania.Tx.Chara_Become_Maxed[(int)this.ctキャラクターアクション_魂MAX.db現在の値].t2D描画(CDTXMania.app.Device, CDTXMania.Skin.Game_Chara_X[0], CDTXMania.Skin.Game_Chara_Y[0]);
                    }
                    if (this.ctキャラクターアクション_魂MAX.b終了値に達したdb)
                    {
                        this.bマイどんアクション中 = false;
                        this.ctキャラクターアクション_魂MAX.t停止();
                        this.ctキャラクターアクション_魂MAX.db現在の値 = 0D;
                    }
                }



                //this.bマイどんアクション中 = this.ctキャラクターアクション_10コンボ.b終了値に達してないdb;
            }


            // ↓踊り子・モブ↓
            // 踊り子の登場処理(魂ゲージ0,20,40,60,80のとき)
            // 640, 430, 856, 215, 1070
            if (CDTXMania.ConfigIni.nPlayerCount == 1)
            {
                if (!CDTXMania.ConfigIni.bAVI有効)
                {
                    //if (this.n踊り子モーション枚数 != 0)
                    //{
                    //    if (this.tx踊り子_1 != null)
                    //    {
                    //        this.tx踊り子_1[this.ar踊り子モーション番号[(int)this.ct踊り子モーション.db現在の値]].t2D中心基準描画(CDTXMania.app.Device, 640, 500);
                    //    }
                    //    if (CDTXMania.stage演奏ドラム画面.actGauge.db現在のゲージ値[0] >= 20)
                    //    {
                    //        if (this.tx踊り子_2 != null)
                    //        {
                    //            this.tx踊り子_2[this.ar踊り子モーション番号[(int)this.ct踊り子モーション.db現在の値]].t2D中心基準描画(CDTXMania.app.Device, 430, 500);
                    //        }
                    //    }
                    //    if (CDTXMania.stage演奏ドラム画面.actGauge.db現在のゲージ値[0] >= 40)
                    //    {
                    //        if (this.tx踊り子_3 != null)
                    //        {
                    //            this.tx踊り子_3[this.ar踊り子モーション番号[(int)this.ct踊り子モーション.db現在の値]].t2D中心基準描画(CDTXMania.app.Device, 856, 500);
                    //        }
                    //    }
                    //    if (CDTXMania.stage演奏ドラム画面.actGauge.db現在のゲージ値[0] >= 60)
                    //    {
                    //        if (this.tx踊り子_4 != null)
                    //        {
                    //            this.tx踊り子_4[this.ar踊り子モーション番号[(int)this.ct踊り子モーション.db現在の値]].t2D中心基準描画(CDTXMania.app.Device, 215, 500);
                    //        }
                    //    }
                    //    if (CDTXMania.stage演奏ドラム画面.actGauge.db現在のゲージ値[0] >= 80)
                    //    {
                    //        if (this.tx踊り子_5 != null)
                    //        {
                    //            this.tx踊り子_5[this.ar踊り子モーション番号[(int)this.ct踊り子モーション.db現在の値]].t2D中心基準描画(CDTXMania.app.Device, 1070, 500);
                    //        }
                    //    }
                    //}
                    //if (CDTXMania.Tx.Mob_Footer != null)
                    //{
                    //    CDTXMania.Tx.Mob_Footer.t2D描画(CDTXMania.app.Device, 0, 676);
                    //}


                    //モブの登場処理(魂ゲージMAX)
                    //if (CDTXMania.Tx.Mob[0] != null)
                    //{
                    //    if (CDTXMania.stage演奏ドラム画面.actGauge.db現在のゲージ値[0] >= 100)
                    //    {
                    //        CDTXMania.Tx.Mob[this.arモブモーション番号[(int)this.ctモブモーション.db現在の値]].t2D描画(CDTXMania.app.Device, 0, 720 - CDTXMania.Tx.Mob[this.arモブモーション番号[(int)this.ctモブモーション.db現在の値]].szテクスチャサイズ.Height);
                    //    }
                    //}
                }
            }


            // ↑踊り子・モブ↑


            return base.On進行描画();
        }

        public void アクションタイマーリセット()
        {
            ctキャラクターアクション_10コンボ.t停止();
            ctキャラクターアクション_10コンボMAX.t停止();
            ctキャラクターアクション_ゴーゴースタート.t停止();
            ctキャラクターアクション_ゴーゴースタートMAX.t停止();
            ctキャラクターアクション_ノルマ.t停止();
            ctキャラクターアクション_魂MAX.t停止();
            ctキャラクターアクション_10コンボ.db現在の値 = 0D;
            ctキャラクターアクション_10コンボMAX.db現在の値 = 0D;
            ctキャラクターアクション_ゴーゴースタート.db現在の値 = 0D;
            ctキャラクターアクション_ゴーゴースタートMAX.db現在の値 = 0D;
            ctキャラクターアクション_ノルマ.db現在の値 = 0D;
            ctキャラクターアクション_魂MAX.db現在の値 = 0D;
        }

        public int[] arモーション番号;
        public int[] arゴーゴーモーション番号;
        public int[] arクリアモーション番号;
        //private CTexture[] txモブ;
        //public int nモブモーション枚数;
        //public int[] arモブモーション番号;
        //public CCounter ctモブモーション;
        //public string strモブ;
        // ↑踊り子・モブ↑

        // ↓マイどん追加アクション↓
        public CCounter ctキャラクターアクション_10コンボ;
        public CCounter ctキャラクターアクション_10コンボMAX;
        public CCounter ctキャラクターアクション_ゴーゴースタート;
        public CCounter ctキャラクターアクション_ゴーゴースタートMAX;
        public CCounter ctキャラクターアクション_ノルマ;
        public CCounter ctキャラクターアクション_魂MAX;

        public CCounter ctChara_Normal;
        public CCounter ctChara_GoGo;
        public CCounter ctChara_Clear;
        

        //public int n前回のコンボ数;
        public double db前回のゲージ値;
        public bool bマイどんアクション中;
        //public int nマイどんアクション番号;
        // ↑マイどん追加アクション↑


        public bool b風船連打中;
        public bool b演奏中;
    }
}
