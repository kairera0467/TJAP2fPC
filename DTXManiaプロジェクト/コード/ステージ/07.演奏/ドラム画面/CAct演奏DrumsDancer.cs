using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using FDK;

namespace DTXMania
{
    internal class CAct演奏DrumsDancer : CActivity
    {
        /// <summary>
        /// 踊り子
        /// </summary>
        public CAct演奏DrumsDancer()
        {
            base.b活性化してない = true;
        }

        public override void On活性化()
        {
            // ↓踊り子・モブ↓
            this.ct踊り子モーション = new CCounter();
            base.On活性化();
        }

        public override void On非活性化()
        {
            // ↓踊り子・モブ↓
            this.ct踊り子モーション = null;
            base.On非活性化();
        }

        public override void OnManagedリソースの作成()
        {
            //this.n踊り子モーション枚数 = CDTXMania.ConfigIni.nDancerMotionCount;
            //this.str踊り子リスト = CDTXMania.ConfigIni.strDancerMotionList;

            this.ar踊り子モーション番号 = C変換.ar配列形式のstringをint配列に変換して返す(CDTXMania.Skin.Game_Dancer_Motion);

            //this.tx踊り子_1 = new CTexture[this.n踊り子モーション枚数];
            //this.tx踊り子_2 = new CTexture[this.n踊り子モーション枚数];
            //this.tx踊り子_3 = new CTexture[this.n踊り子モーション枚数];
            //this.tx踊り子_4 = new CTexture[this.n踊り子モーション枚数];
            //this.tx踊り子_5 = new CTexture[this.n踊り子モーション枚数];
            //for (int i = 0; i < this.n踊り子モーション枚数; i++)
            //{
            //    this.tx踊り子_1[i] = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\Dancer\0\" + i.ToString() + ".png"));
            //    this.tx踊り子_2[i] = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\Dancer\1\" + i.ToString() + ".png"));
            //    this.tx踊り子_3[i] = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\Dancer\2\" + i.ToString() + ".png"));
            //    this.tx踊り子_4[i] = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\Dancer\3\" + i.ToString() + ".png"));
            //    this.tx踊り子_5[i] = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\Dancer\4\" + i.ToString() + ".png"));

            //}

            this.ct踊り子モーション = new CCounter(0, this.ar踊り子モーション番号.Length - 1, 0.01, CSound管理.rc演奏用タイマ);
            //this.strList = "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16";
            //this.strList_登場 = "0,1,2,3,4,5,6,7,8,8,8,8,8,8,8,8";
            //this.strList_退場 = "1,2,3,3,3,3,3,3,3,3,3,3,3,3,3,3";

            //this.arモーション番号_通常 = C変換.ar配列形式のstringをint配列に変換して返す( this.strList );
            //this.arモーション番号_登場 = C変換.ar配列形式のstringをint配列に変換して返す( this.strList_登場 );
            //this.arモーション番号_退場 = C変換.ar配列形式のstringをint配列に変換して返す( this.strList_退場 );

            //this.nテクスチャ枚数_通常 = 16;
            //this.nテクスチャ枚数_登場 = 10;
            //this.nテクスチャ枚数_退場 = 3;
            //this.n現在表示している踊り子数 = 1;

            //this.e現在のモーション = new EMotion[ 5 ];
            //for( int i = 0; i < 5; i++ )
            //{
            //    this.e現在のモーション[ i ] = EMotion.非表示;
            //}

            /*this.tx踊り子1_通常 = new CTexture[ this.nテクスチャ枚数_通常 ];
            for( int i = 0; i < this.nテクスチャ枚数_通常; i++ )
            {
                this.tx踊り子1_通常[ i ] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\Dancer\0\dance_1_" + i.ToString() + ".png" ) );
            }
            this.tx踊り子1_登場 = new CTexture[ this.nテクスチャ枚数_登場 ];
            for( int i = 0; i < this.nテクスチャ枚数_登場; i++ )
            {
                this.tx踊り子1_登場[ i ] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\Dancer\0\appear_1_" + i.ToString() + ".png" ) );
            }
            this.tx踊り子1_退場 = new CTexture[ this.nテクスチャ枚数_退場 ];
            for( int i = 0; i < this.nテクスチャ枚数_退場; i++ )
            {
                this.tx踊り子1_退場[ i ] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\Dancer\0\leave_1_" + i.ToString() + ".png" ) );
            } */

            // this.ct通常モーション = new CCounter( 0, this.arモーション番号_通常.Length - 1, 0.4, CSound管理.rc演奏用タイマ );
            //this.ct登場モーション = new CCounter( 0, this.arモーション番号_登場.Length - 1, 0.4, CSound管理.rc演奏用タイマ );

            //this.ctモブ = new CCounter( 1, 16, 0.025, CSound管理.rc演奏用タイマ );

            //this.txフッター = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\Dancer_BG\footer\01.png" ) );
            //this.txモブ = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\Dancer\mob\1.png" ) );

            //for( int i = 0; i < 5; i++ )
            //{
            //    this.st投げ上げ[ i ].ct進行 = new CCounter();
            //}
            base.OnManagedリソースの作成();
        }

        public override void OnManagedリソースの解放()
        {
            //CDTXMania.tテクスチャの解放( ref this.txフッター );
            //CDTXMania.tテクスチャの解放( ref this.txモブ );
            //if (this.n踊り子モーション枚数 != 0)
            //{
            //    for (int i = 0; i < this.n踊り子モーション枚数; i++)
            //    {
            //        CDTXMania.tテクスチャの解放(ref this.tx踊り子_1[i]);
            //        CDTXMania.tテクスチャの解放(ref this.tx踊り子_2[i]);
            //        CDTXMania.tテクスチャの解放(ref this.tx踊り子_3[i]);
            //        CDTXMania.tテクスチャの解放(ref this.tx踊り子_4[i]);
            //        CDTXMania.tテクスチャの解放(ref this.tx踊り子_5[i]);
            //    }
            //}
            base.OnManagedリソースの解放();
        }

        public override int On進行描画()
        {
            //設定
            if( this.b初めての進行描画 )
            {
                this.b初めての進行描画 = true;
            }

            //レイヤー位置を考えて移動。
            //if( this.txフッター != null )
            //{
            //    this.txフッター.t2D描画( CDTXMania.app.Device, 0, 676 );
            //}

            //return 0;

            //this.ct通常モーション.t進行LoopDb();
            //this.ct登場モーション.t進行db();
            //this.ctモブ.t進行LoopDb();

            if (this.ct踊り子モーション != null) this.ct踊り子モーション.t進行LoopDb();

            //CDTXMania.act文字コンソール.tPrint(0, 0, C文字コンソール.Eフォント種別.白, this.ct踊り子モーション.db現在の値.ToString());
            //CDTXMania.act文字コンソール.tPrint(0, 20, C文字コンソール.Eフォント種別.白, this.n踊り子モーション枚数.ToString());
            //CDTXMania.act文字コンソール.tPrint(0, 0, C文字コンソール.Eフォント種別.白, this.str踊り子リスト.ToString());

            

            if (CDTXMania.ConfigIni.bDancer)
            {
                for (int i = 0; i < 5; i++)
                {
                    if(CDTXMania.Tx.Dancer[i][this.ar踊り子モーション番号[(int)this.ct踊り子モーション.db現在の値]] != null)
                    {
                        if(CDTXMania.stage演奏ドラム画面.actGauge.db現在のゲージ値[0] >= CDTXMania.Skin.Game_Dancer_Gauge[i])
                            CDTXMania.Tx.Dancer[i][this.ar踊り子モーション番号[(int)this.ct踊り子モーション.db現在の値]].t2D中心基準描画(CDTXMania.app.Device, CDTXMania.Skin.Game_Dancer_X[i], CDTXMania.Skin.Game_Dancer_Y[i]);
                    }
                }
                //if (CDTXMania.Tx.Dancer_1[this.ar踊り子モーション番号[(int)this.ct踊り子モーション.db現在の値]] != null)
                //{
                //    CDTXMania.Tx.Dancer_1[this.ar踊り子モーション番号[(int)this.ct踊り子モーション.db現在の値]].t2D中心基準描画(CDTXMania.app.Device, 640, 500);
                //}
                //if (CDTXMania.stage演奏ドラム画面.actGauge.db現在のゲージ値[0] >= 20)
                //{
                //    if (CDTXMania.Tx.Dancer_2[this.ar踊り子モーション番号[(int)this.ct踊り子モーション.db現在の値]] != null)
                //    {
                //        CDTXMania.Tx.Dancer_2[this.ar踊り子モーション番号[(int)this.ct踊り子モーション.db現在の値]].t2D中心基準描画(CDTXMania.app.Device, 430, 500);
                //    }
                //}
                //if (CDTXMania.stage演奏ドラム画面.actGauge.db現在のゲージ値[0] >= 40)
                //{
                //    if (CDTXMania.Tx.Dancer_3[this.ar踊り子モーション番号[(int)this.ct踊り子モーション.db現在の値]]  != null)
                //    {
                //        CDTXMania.Tx.Dancer_3[this.ar踊り子モーション番号[(int)this.ct踊り子モーション.db現在の値]].t2D中心基準描画(CDTXMania.app.Device, 856, 500);
                //    }
                //}
                //if (CDTXMania.stage演奏ドラム画面.actGauge.db現在のゲージ値[0] >= 60)
                //{
                //    if (CDTXMania.Tx.Dancer_4[this.ar踊り子モーション番号[(int)this.ct踊り子モーション.db現在の値]] != null)
                //    {
                //        CDTXMania.Tx.Dancer_4[this.ar踊り子モーション番号[(int)this.ct踊り子モーション.db現在の値]].t2D中心基準描画(CDTXMania.app.Device, 215, 500);
                //    }
                //}
                //if (CDTXMania.stage演奏ドラム画面.actGauge.db現在のゲージ値[0] >= 80)
                //{
                //    if (CDTXMania.Tx.Dancer_5[this.ar踊り子モーション番号[(int)this.ct踊り子モーション.db現在の値]] != null)
                //    {
                //        CDTXMania.Tx.Dancer_5[this.ar踊り子モーション番号[(int)this.ct踊り子モーション.db現在の値]].t2D中心基準描画(CDTXMania.app.Device, 1070, 500);
                //    }
                //}

            }

            //double[] dbGauge = new double[] { 0, 0, 0, 0, 0 }; //出現タイミング
            //for( int i = 0; i < 5; i++ )
            //{
            //    dbGauge[ i ] = 80.0 / 4.0 * i;
            //}
            //int[] nX = new int[] { 640, 430, 856, 215, 1070 }; //出現位置
            //for( int i = 0; i < 5; i++ )
            //{
            //    switch( this.e現在のモーション[ i ] )
            //    {
            //        case EMotion.登場:
            //            {
            //                if( this.tx踊り子1_登場[ this.arモーション番号_登場[ (int)this.ct登場モーション.db現在の値 ] ] != null )
            //                {
            //                    for (int k = 0; k < 1; k++)
            //                    {
            //                        for (int j = 0; j < 5; j++)
            //                        {
            //                            CDTXMania.act文字コンソール.tPrint(0, 16, C文字コンソール.Eフォント種別.白, this.st投げ上げ[j].ct進行.n現在の値.ToString());
            //                            if (this.st投げ上げ[j].b使用中)
            //                            {
            //                                this.st投げ上げ[j].n前回のValue = this.st投げ上げ[j].ct進行.n現在の値;
            //                                this.st投げ上げ[j].ct進行.t進行();
            //                                if (this.st投げ上げ[j].ct進行.b終了値に達した)
            //                                {
            //                                    this.st投げ上げ[j].ct進行.t停止();
            //                                    this.st投げ上げ[j].b使用中 = false;
            //                                }
            //                                for (double n = this.st投げ上げ[j].n前回のValue; n < this.st投げ上げ[j].ct進行.n現在の値; n++)
            //                                {
            //                                    this.st投げ上げ[j].fY -= (float)((this.st投げ上げ[j].f加速度Y * Math.Sin((60.0 * Math.PI / 180.0))) * 10.0f - Math.Exp(this.st投げ上げ[j].f重力加速度 * 2.0f) / 2.0f);
            //                                    this.st投げ上げ[j].f加速度Y *= this.st投げ上げ[j].f加速度の加速度Y;
            //                                    this.st投げ上げ[j].f加速度Y -= this.st投げ上げ[j].f重力加速度;
            //                                }
            //                            }
            //                            int nTexSize = this.tx踊り子1_登場[this.arモーション番号_登場[(int)this.ct登場モーション.db現在の値]].szテクスチャサイズ.Width;
            //                            this.tx踊り子1_登場[this.arモーション番号_登場[(int)this.ct登場モーション.db現在の値]].t2D描画(CDTXMania.app.Device, nX[i] - (nTexSize / 2), 720 + this.st投げ上げ[j].fY); //Y:360
            //                        }
            //                    }
            //                }
            //                if( this.ct登場モーション.b終了値に達したdb )
            //                {
            //                    this.e現在のモーション[ i ] = EMotion.通常;
            //                }
            //            }
            //            break;
            //        case EMotion.通常:
            //            {
            //                if( this.tx踊り子1_通常[ (int)this.ct通常モーション.db現在の値 ] != null && CDTXMania.stage演奏ドラム画面.actGauge.db現在のゲージ値[ 0 ] >= dbGauge[ i ] )
            //                {
            //                    int nTexSize = this.tx踊り子1_通常[ (int)this.ct通常モーション.db現在の値 ].szテクスチャサイズ.Width;
            //                    this.tx踊り子1_通常[ (int)this.ct通常モーション.db現在の値 ].t2D描画( CDTXMania.app.Device, nX[ i ] - (nTexSize / 2), 360 );
            //                }
            //            }
            //            break;
            //    }

            //}

            //if( CDTXMania.stage演奏ドラム画面.actGauge.db現在のゲージ値[ 0 ] >= 100.0 )
            //{
            //    if( this.txモブ != null )
            //    {
            //        //float num1 = (float)this.ctモブ.db現在の値;
            //        //float fSpeed = ( 1.0f - ( 1.0f * (float)this.ctモブ.db現在の値 ) );

            //        //int mobY = -(int)(this.ctモブ.db現在の値) + 2;
            //        //mobY = (int)( 2.0 * this.ctモブ.db現在の値 + ( ( 1.0 * Math.Pow( this.ctモブ.db現在の値, 2.0 ) ) * 0.5 ) );
            //        //CDTXMania.act文字コンソール.tPrint( 0, 0, C文字コンソール.Eフォント種別.白, mobY.ToString() );
            //        //this.txモブ.t2D描画( CDTXMania.app.Device, 0, ( 720 - this.txモブ.szテクスチャサイズ.Height ) + mobY );
            //        //this.txモブ.t2D描画( CDTXMania.app.Device, 0, (( 780.0f - this.txモブ.szテクスチャサイズ.Height ) - (float)( 60.0f * Math.Sin( Math.PI * num1 / 14.0f ) )) );
            //    }
            //}
            return base.On進行描画();
        }

        public void tモブ_進行描画()
        {

        }

        /// <summary>
        /// 踊り子の入退場時に使います。
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="pos"></param>
        //public void t入退場( int mode, int pos, double dbUnit )
        //{
        //    switch( mode )
        //    {
        //        case 0:
        //            this.e現在のモーション[ pos ] = EMotion.登場;
        //            this.ct登場モーション.db現在の値 = 0;

        //            for( int j = 0; j < 1; j++ )
        //            {
        //                for( int i = 0; i < 5; i++ )
        //                {
        //                    if( !this.st投げ上げ[ i ].b使用中 )
        //                    {
        //                        int n回転初期値 = 1;
        //                        double num7 = 1.1 + (1 / 100.0); // 拡散の大きさ
        //                        this.st投げ上げ[ i ].ct進行 = new CCounter( 0, 99, (int)( ( ( 0.2 * this.arモーション番号_登場.Length ) / 100.0 ) * 1000 ), CDTXMania.Timer );
        //                        this.st投げ上げ[ i ].fY = 0; //Y座標
                                
        //                        this.st投げ上げ[ i ].f加速度Y = 1.515f;
        //                        this.st投げ上げ[ i ].f加速度の加速度Y = 1.0105f;
        //                        this.st投げ上げ[ i ].f重力加速度 = 0.03155f;
                            
        //                        this.st投げ上げ[ i ].b使用中 = true;
        //                    }
        //                }
        //            }
        //            break;
        //        case 1:
        //            this.e現在のモーション[ pos ] = EMotion.退場;
        //            break;
        //    }
        //}

        #region[ private ]
        //-----------------
        public enum EMotion
        {
            通常 = 0,
            登場 = 1,
            退場 = 2,
            非表示 = 99
        }

        ////画像、モーション番号一覧などはそれぞれ3つずつ用意する。
        //private CTexture[] tx踊り子1_通常;
        //private CTexture[] tx踊り子1_登場;
        //private CTexture[] tx踊り子1_退場;

        //private int nテクスチャ枚数_通常;
        //private int nテクスチャ枚数_登場;
        //private int nテクスチャ枚数_退場;

        //public CCounter ct通常モーション;
        //public CCounter ct登場モーション;
        //public CCounter ct登場Y座標;
        //public CCounter ct退場モーション;
        //public CCounter ctモブ;

        //public int[] arモーション番号_通常;
        //public int[] arモーション番号_登場;
        //public int[] arモーション番号_退場;

        //public string strList;
        //public string strList_登場;
        //public string strList_退場;

        //public bool b登場アニメタイプ; //0:そのまま 1:鉛直投げ上げ
        //public EMotion[] e現在のモーション;
        //public int n現在表示している踊り子数;

        //private CTexture txフッター;
        //private CTexture txモブ;


        //public ST投げ上げ[] st投げ上げ = new ST投げ上げ[ 5 ];
        //[StructLayout(LayoutKind.Sequential)]
        //public struct ST投げ上げ
        //{
        //    public bool b使用中;
        //    public CCounter ct進行;
        //    public int n前回のValue;
        //    public float fY;
        //    public float f加速度Y;
        //    public float f加速度の加速度Y;
        //    public float f重力加速度;
        //    public float f半径;
        //    public float f角度;
        //}
        // ↓踊り子・モブ↓
        //private CTexture[] tx踊り子_1;
        //private CTexture[] tx踊り子_2;
        //private CTexture[] tx踊り子_3;
        //private CTexture[] tx踊り子_4;
        //private CTexture[] tx踊り子_5;

        //public int n踊り子モーション枚数;

        public int[] ar踊り子モーション番号;

        public CCounter ct踊り子モーション;

        //public string str踊り子リスト;
        //-----------------
        #endregion
    }
}
