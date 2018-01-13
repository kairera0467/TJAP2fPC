using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using FDK;
using System.Diagnostics;

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
            this.ct通常モーション = new CCounter();
            this.ctゴーゴーモーション = new CCounter();
            this.ctクリア通常モーション = new CCounter();
            // ↓踊り子・モブ↓
            this.ct踊り子モーション = new CCounter();
            this.ctモブモーション = new CCounter();
            // ↑踊り子・モブ↑
            this.b風船連打中 = false;
            this.b演奏中 = false;

            base.On活性化();
        }

        public override void On非活性化()
        {
            this.ct通常モーション = null;
            this.ctゴーゴーモーション = null;
            this.ctクリア通常モーション = null;
            // ↓踊り子・モブ↓
            this.ct踊り子モーション = null;
            this.ctモブモーション = null;
            // ↑踊り子・モブ↑
            base.On非活性化();
        }

        public override void OnManagedリソースの作成()
        {
            //キャラクター画像を格納しているディレクトリに各モーションの画像がいくつあるかを検索。
            //現在は固定値。
            this.nキャラクター通常モーション枚数 = CDTXMania.ConfigIni.nCharaMotionCount;
            this.nキャラクターゴーゴーモーション枚数 = CDTXMania.ConfigIni.nCharaMotionCount_gogo;
            this.nキャラクタークリアモーション枚数 = CDTXMania.ConfigIni.nCharaMotionCount_clear;
            this.nキャラクターMAX通常モーション枚数 = CDTXMania.ConfigIni.nCharaMotionCount_max;
            this.nキャラクターMAXゴーゴーモーション枚数 = CDTXMania.ConfigIni.nCharaMotionCount_maxgogo;

            // ↓踊り子・モブ↓
            this.n踊り子モーション枚数 = 28;
            this.nモブモーション枚数 = 10;

            this.str踊り子リスト = "0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27";//,26,25,24,23,22,21,20,19,18,17,16,15,14,13,12,11,10,9,8,7,6,5,4,3,2,1";
            this.strモブ = "4,5,6,7,8,9,8,7,6,5,4,3,3,2,2,2,1,1,0,0,0,1,1,2,2,3,3";

            this.ar踊り子モーション番号 = C変換.ar配列形式のstringをint配列に変換して返す(str踊り子リスト);
            this.arモブモーション番号 = C変換.ar配列形式のstringをint配列に変換して返す(strモブ);

            this.tx踊り子_1 = new CTexture[this.n踊り子モーション枚数];
            for (int i = 0; i < this.n踊り子モーション枚数; i++)
            {
                this.tx踊り子_1[i] = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\Dancer\0\" + i.ToString() + ".png"));
            }
            this.tx踊り子_2 = new CTexture[this.n踊り子モーション枚数];
            for (int i = 0; i < this.n踊り子モーション枚数; i++)
            {
                this.tx踊り子_2[i] = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\Dancer\1\" + i.ToString() + ".png"));
            }
            this.tx踊り子_3 = new CTexture[this.n踊り子モーション枚数];
            for (int i = 0; i < this.n踊り子モーション枚数; i++)
            {
                this.tx踊り子_3[i] = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\Dancer\2\" + i.ToString() + ".png"));
            }
            this.tx踊り子_4 = new CTexture[this.n踊り子モーション枚数];
            for (int i = 0; i < this.n踊り子モーション枚数; i++)
            {
                this.tx踊り子_4[i] = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\Dancer\3\" + i.ToString() + ".png"));
            }
            this.tx踊り子_5 = new CTexture[this.n踊り子モーション枚数];
            for (int i = 0; i < this.n踊り子モーション枚数; i++)
            {
                this.tx踊り子_5[i] = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\Dancer\4\" + i.ToString() + ".png"));
            }
            this.txモブ = new CTexture[this.nモブモーション枚数];
            for (int i = 0; i < this.nモブモーション枚数; i++)
            {
                this.txモブ[i] = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\Dancer\mob\mob_" + i.ToString() + ".png"));
            }

            this.txフッター = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\Dancer_BG\footer\01.png"));
            // ↑踊り子・モブ↑

            this.txキャラクターNormal = new CTexture[ this.nキャラクター通常モーション枚数 ];
            for( int i = 0; i < this.nキャラクター通常モーション枚数; i++ )
            {
                this.txキャラクターNormal[ i ] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\Chara\Normal_" + i.ToString() + ".png" ) );
            }

            this.txキャラクターGogo = new CTexture[ this.nキャラクターゴーゴーモーション枚数 ];
            for( int i = 0; i < this.nキャラクターゴーゴーモーション枚数; i++ )
            {
                this.txキャラクターGogo[ i ] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\Chara\gogo_" + i.ToString() + ".png" ) );
            }

            if( this.nキャラクタークリアモーション枚数 != 0 )
            {
                this.txキャラクターClear_Normal = new CTexture[ this.nキャラクタークリアモーション枚数 ];
                for( int i = 0; i < this.nキャラクタークリアモーション枚数; i++ )
                {
                    this.txキャラクターClear_Normal[ i ] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\Chara\Clear_" + i.ToString() + ".png" ) );
                }
            }

            if( this.nキャラクターMAX通常モーション枚数 != 0 )
            {
                this.txキャラクターMax_Normal = new CTexture[ this.nキャラクターMAX通常モーション枚数 ];
                for( int i = 0; i < this.nキャラクターMAX通常モーション枚数; i++ )
                {
                    this.txキャラクターMax_Normal[ i ] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\Chara\Max_" + i.ToString() + ".png" ) );
                }
            }
            
            if( this.nキャラクターMAXゴーゴーモーション枚数 != 0 )
            {
                this.txキャラクターMax_Gogo = new CTexture[ this.nキャラクターMAXゴーゴーモーション枚数 ];
                for( int i = 0; i < this.nキャラクターMAXゴーゴーモーション枚数; i++ )
                {
                    this.txキャラクターMax_Gogo[ i ] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\Chara\Max_gogo_" + i.ToString() + ".png" ) );
                }
            }


            //固定値
            //this.arゴーゴーモーション番号 = new int[]{ 0, 1, 2, 3, 3, 3, 3, 3, 3, 2, 1, 0, 0, 0, 0, 0 };

            //2015.08.05 Config.iniから変更可能にするための実験
            this.strList = CDTXMania.ConfigIni.strCharaMotionList;
            this.strListGogo = CDTXMania.ConfigIni.strCharaMotionList_gogo;
            this.strListClear = CDTXMania.ConfigIni.strCharaMotionList_clear;
            this.strListMAX = CDTXMania.ConfigIni.strCharaMotionList_max;
            this.strListMAXGogo = CDTXMania.ConfigIni.strCharaMotionList_maxgogo;
            this.arモーション番号 = C変換.ar配列形式のstringをint配列に変換して返す( this.strList );
            this.arゴーゴーモーション番号 = C変換.ar配列形式のstringをint配列に変換して返す( this.strListGogo );
            this.arクリアモーション番号 = C変換.ar配列形式のstringをint配列に変換して返す( this.strListClear );
            this.ar黄色モーション番号 = C変換.ar配列形式のstringをint配列に変換して返す( this.strListGogo );
            this.ar黄色ゴーゴーモーション番号 = C変換.ar配列形式のstringをint配列に変換して返す( this.strListMAXGogo );

            this.ct通常モーション = new CCounter( 0, this.arモーション番号.Length - 1, 0.02, CSound管理.rc演奏用タイマ );
            this.ctゴーゴーモーション = new CCounter( 0, this.arゴーゴーモーション番号.Length - 1, 50, CDTXMania.Timer );
            if( this.nキャラクタークリアモーション枚数 != 0 )
                this.ctクリア通常モーション = new CCounter( 0, this.arクリアモーション番号.Length - 1, 50, CDTXMania.Timer );
            if( this.nキャラクターMAX通常モーション枚数 != 0 )
                this.ctMAX通常モーション = new CCounter( 0, this.ar黄色モーション番号.Length - 1, 50, CDTXMania.Timer );
            if( this.nキャラクターMAXゴーゴーモーション枚数 != 0 )
                this.ctMAXゴーゴーモーション = new CCounter( 0, this.ar黄色ゴーゴーモーション番号.Length - 1, 50, CDTXMania.Timer );
            this.ct踊り子モーション = new CCounter(0, this.ar踊り子モーション番号.Length - 1, 0.04, CSound管理.rc演奏用タイマ);
            this.ctモブモーション = new CCounter(0, this.arモブモーション番号.Length - 1, 0.04, CSound管理.rc演奏用タイマ);

            base.OnManagedリソースの作成();
        }

        public override void OnManagedリソースの解放()
        {
            for( int i = 0; i < this.nキャラクター通常モーション枚数; i++ )
            {
                CDTXMania.tテクスチャの解放( ref this.txキャラクターNormal[ i ] );
            }
            for( int i = 0; i < this.nキャラクターゴーゴーモーション枚数; i++ )
            {
                CDTXMania.tテクスチャの解放( ref this.txキャラクターGogo[ i ] );
            }
            if( this.nキャラクタークリアモーション枚数 != 0 )
            {
                for( int i = 0; i < this.nキャラクタークリアモーション枚数; i++ )
                {
                    CDTXMania.tテクスチャの解放( ref this.txキャラクターClear_Normal[ i ] );
                }
            }
            if( this.nキャラクターMAX通常モーション枚数 != 0 )
            {
                for( int i = 0; i < this.nキャラクターMAX通常モーション枚数; i++ )
                {
                    CDTXMania.tテクスチャの解放( ref this.txキャラクターMax_Normal[ i ] );
                }
            }
            if( this.nキャラクターMAXゴーゴーモーション枚数 != 0 )
            {
                for( int i = 0; i < this.nキャラクターMAXゴーゴーモーション枚数; i++ )
                {
                    CDTXMania.tテクスチャの解放( ref this.txキャラクターMax_Gogo[ i ] );
                }
            }

            // ↓踊り子・モブ↓
            if(this.n踊り子モーション枚数 != 0)
            {
                for(int i = 0; i < this.n踊り子モーション枚数; i++)
                {
                    CDTXMania.tテクスチャの解放(ref this.tx踊り子_1[i]);
                }
                for (int i = 0; i < this.n踊り子モーション枚数; i++)
                {
                    CDTXMania.tテクスチャの解放(ref this.tx踊り子_2[i]);
                }
                for (int i = 0; i < this.n踊り子モーション枚数; i++)
                {
                    CDTXMania.tテクスチャの解放(ref this.tx踊り子_3[i]);
                }
                for (int i = 0; i < this.n踊り子モーション枚数; i++)
                {
                    CDTXMania.tテクスチャの解放(ref this.tx踊り子_4[i]);
                }
                for (int i = 0; i < this.n踊り子モーション枚数; i++)
                {
                    CDTXMania.tテクスチャの解放(ref this.tx踊り子_5[i]);
                }
            }
            if(this.nモブモーション枚数 != 0)
            {
                for(int i = 0; i < this.nモブモーション枚数; i++)
                {
                    CDTXMania.tテクスチャの解放(ref this.txモブ[i]);
                }
            }
            // ↑踊り子・モブ↑

            CDTXMania.tテクスチャの解放(ref this.txフッター);

            Trace.TraceInformation("CA リソースの開放");
            base.OnManagedリソースの解放();
        }

        public override int On進行描画()
        {
            if( this.ct通常モーション != null ) this.ct通常モーション.t進行LoopDb();
            if( this.ctクリア通常モーション != null ) this.ctクリア通常モーション.t進行LoopDb();
            if( this.ctMAX通常モーション != null ) this.ctMAX通常モーション.t進行LoopDb();
            if( this.ctMAXゴーゴーモーション != null ) this.ctMAXゴーゴーモーション.t進行LoopDb();

            // ↓踊り子・モブ↓
            if (this.ct踊り子モーション != null) this.ct踊り子モーション.t進行LoopDb();
            if (this.ctモブモーション != null) this.ctモブモーション.t進行LoopDb();
            // ↑踊り子・モブ↑
            
            if( this.b風船連打中 != true )
            {
                if( !CDTXMania.stage演奏ドラム画面.bIsGOGOTIME[ 0 ] )
                {
                    if( CDTXMania.stage演奏ドラム画面.actGauge.db現在のゲージ値[ 0 ] >= 100.0 && this.nキャラクターMAX通常モーション枚数 != 0 )
                    {
                        if( this.txキャラクターMax_Normal[ 0 ] != null )
                            this.txキャラクターMax_Normal[ this.ar黄色モーション番号[ (int)this.ctMAX通常モーション.db現在の値 ] ].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nPlayerCharacterX[ 0 ], CDTXMania.Skin.nPlayerCharacterY[ 0 ] );
                    }
                    else if( CDTXMania.stage演奏ドラム画面.actGauge.db現在のゲージ値[ 0 ] >= 80.0 && this.nキャラクタークリアモーション枚数 != 0 )
                    {
                        if( this.txキャラクターClear_Normal[ 0 ] != null )
                        {
                            this.txキャラクターClear_Normal[ this.arクリアモーション番号[ (int)this.ctクリア通常モーション.db現在の値 ] ].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nPlayerCharacterX[ 0 ], CDTXMania.Skin.nPlayerCharacterY[ 0 ] );
                        }
                    }
                    else
                    {
                        if( this.txキャラクターNormal[ 0 ] != null )
                        {
                            this.txキャラクターNormal[ this.arモーション番号[ (int)this.ct通常モーション.db現在の値 ] ].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nPlayerCharacterX[0], CDTXMania.Skin.nPlayerCharacterY[0] );
                        }
                    }
                }
                else
                {
                    if( CDTXMania.stage演奏ドラム画面.actGauge.db現在のゲージ値[ 0 ] >= 100.0 && this.nキャラクターMAXゴーゴーモーション枚数 != 0 )
                    {
                        if( this.txキャラクターMax_Gogo[ 0 ] != null )
                            this.txキャラクターMax_Gogo[ this.ar黄色ゴーゴーモーション番号[ (int)this.ctMAXゴーゴーモーション.db現在の値 ] ].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nPlayerCharacterX[0], CDTXMania.Skin.nPlayerCharacterY[0] );
                    }
                    else
                    {
                        if( this.txキャラクターGogo[ this.arゴーゴーモーション番号[ (int)this.ctゴーゴーモーション.db現在の値 ] ] != null )
                            this.txキャラクターGogo[ this.arゴーゴーモーション番号[ (int)this.ctゴーゴーモーション.db現在の値 ] ].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nPlayerCharacterX[0], CDTXMania.Skin.nPlayerCharacterY[0] );
                    }
                }
            
            }

            // ↓踊り子・モブ↓
            // 踊り子の登場処理(魂ゲージ0,20,40,60,80のとき)
            // 640, 430, 856, 215, 1070
            if (this.n踊り子モーション枚数 != 0)
            {
                if(this.tx踊り子_1 != null)
                {
                    this.tx踊り子_1[this.ar踊り子モーション番号[(int)this.ct踊り子モーション.db現在の値]].t2D中心基準描画(CDTXMania.app.Device, 640, 500);
                }
                if (CDTXMania.stage演奏ドラム画面.actGauge.db現在のゲージ値[0] >= 20)
                {
                    if(this.tx踊り子_2 != null)
                    {
                        this.tx踊り子_2[this.ar踊り子モーション番号[(int)this.ct踊り子モーション.db現在の値]].t2D中心基準描画(CDTXMania.app.Device, 430, 500);
                    }
                }
                if (CDTXMania.stage演奏ドラム画面.actGauge.db現在のゲージ値[0] >= 40)
                {
                    if (this.tx踊り子_3 != null)
                    {
                        this.tx踊り子_3[this.ar踊り子モーション番号[(int)this.ct踊り子モーション.db現在の値]].t2D中心基準描画(CDTXMania.app.Device, 856, 500);
                    }
                }
                if (CDTXMania.stage演奏ドラム画面.actGauge.db現在のゲージ値[0] >= 60)
                {
                    if (this.tx踊り子_4 != null)
                    {
                        this.tx踊り子_4[this.ar踊り子モーション番号[(int)this.ct踊り子モーション.db現在の値]].t2D中心基準描画(CDTXMania.app.Device, 215, 500);
                    }
                }
                if (CDTXMania.stage演奏ドラム画面.actGauge.db現在のゲージ値[0] >= 80)
                {
                    if (this.tx踊り子_5 != null)
                    {
                        this.tx踊り子_5[this.ar踊り子モーション番号[(int)this.ct踊り子モーション.db現在の値]].t2D中心基準描画(CDTXMania.app.Device, 1070, 500);
                    }
                }
            }
            if (this.txフッター != null)
            {
                this.txフッター.t2D描画(CDTXMania.app.Device, 0, 676);
            }


            //モブの登場処理(魂ゲージMAX)
            if (this.nモブモーション枚数 != 0)
            {
                if (CDTXMania.stage演奏ドラム画面.actGauge.db現在のゲージ値[0] >= 100)
                {
                    this.txモブ[this.arモブモーション番号[(int)this.ctモブモーション.db現在の値]].t2D描画(CDTXMania.app.Device, 0, 0);
                }
            }


            // ↑踊り子・モブ↑


            return base.On進行描画();
        }

        private CTexture txフッター;

        //---------------------------
        private CTexture[] txキャラクターNormal; //キャラクター画像 通常モーション
        private CTexture[] txキャラクターClear_Normal;
        private CTexture[] txキャラクターMax_Normal;
        private CTexture[] txキャラクターGogo;
        private CTexture[] txキャラクターMax_Gogo;
        private CTexture[] txキャラクターJump; //まだ実装できてない
        private CTexture[] txキャラクターMax_Jump; //まだ実装できてない

        public int nキャラクター通常モーション枚数;
        public int nキャラクターコンボモーション枚数;
        public int nキャラクターゴーゴーモーション枚数;
        public int nキャラクタークリアモーション枚数;
        public int nキャラクターMAX通常モーション枚数;
        public int nキャラクターMAXゴーゴーモーション枚数;

        public int[] arモーション番号;
        public int[] arゴーゴーモーション番号;
        public int[] arクリアモーション番号;
        public int[] ar黄色モーション番号;
        public int[] ar黄色ゴーゴーモーション番号;

        public CCounter ct通常モーション;
        public CCounter ctクリア通常モーション;
        public CCounter ctゴーゴーモーション;
        public CCounter ctMAX通常モーション;
        public CCounter ctMAXゴーゴーモーション;

        public string strList;
        public string strListGogo;
        public string strListClear;
        public string strListMAX;
        public string strListMAXGogo;

        // ↓踊り子・モブ↓
        private CTexture[] tx踊り子_1;
        private CTexture[] tx踊り子_2;
        private CTexture[] tx踊り子_3;
        private CTexture[] tx踊り子_4;
        private CTexture[] tx踊り子_5;

        public int n踊り子モーション枚数;

        public int[] ar踊り子モーション番号;

        public CCounter ct踊り子モーション;

        public string str踊り子リスト;

        private CTexture[] txモブ;
        public int nモブモーション枚数;
        public int[] arモブモーション番号;
        public CCounter ctモブモーション;
        public string strモブ;
        // ↑踊り子・モブ↑


        public bool b風船連打中;
        public bool b演奏中;
    }
}
