using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Diagnostics;
using SlimDX;
using FDK;

namespace DTXMania
{
	internal class CAct演奏Drumsゲージ : CAct演奏ゲージ共通
	{
		// プロパティ

//		public double db現在のゲージ値
//		{
//			get
//			{
//				return this.dbゲージ値;
//			}
//			set
//			{
//				this.dbゲージ値 = value;
//				if( this.dbゲージ値 > 1.0 )
//				{
//					this.dbゲージ値 = 1.0;
//				}
//			}
//		}

		
		// コンストラクタ
        /// <summary>
        /// ゲージの描画クラス。ドラム側。
        /// 
        /// 課題
        /// _ゲージの実装。
        /// _Danger時にゲージの色が変わる演出の実装。
        /// _Danger、MAX時のアニメーション実装。
        /// </summary>
		public CAct演奏Drumsゲージ()
		{
			base.b活性化してない = true;
		}

        public override void Start(int nLane, E判定 judge, int player)
        {
            for (int j = 0; j < 32; j++)
            {
                if( player == 0 )
                {
                    if( !this.st花火状態[ j ].b使用中 )
                    {
                        this.st花火状態[j].ct進行 = new CCounter(0, 10, 20, CDTXMania.Timer);
                        this.st花火状態[j].nPlayer = player;

                        switch (nLane)
                        {
                            case 0x11:
                            case 0x12:
                            case 0x15:
                                this.st花火状態[j].isBig = false;
                                break;
                            case 0x13:
                            case 0x14:
                            case 0x16:
                            case 0x17:
                                this.st花火状態[j].isBig = true;
                                break;
                        }

                        this.st花火状態[j].b使用中 = true;
                        break;
                    }
                }
                if( player == 1 )
                {
                    if( !this.st花火状態2P[ j ].b使用中 )
                    {
                        this.st花火状態2P[ j ].ct進行 = new CCounter(0, 10, 20, CDTXMania.Timer);
                        this.st花火状態2P[ j ].nPlayer = player;

                        switch (nLane)
                        {
                            case 0x11:
                            case 0x12:
                            case 0x15:
                                this.st花火状態2P[ j ].isBig = false;
                                break;
                            case 0x13:
                            case 0x14:
                            case 0x16:
                            case 0x17:
                                this.st花火状態2P[ j ].isBig = true;
                                break;
                        }

                        this.st花火状態2P[ j ].b使用中 = true;
                        break;
                    }
                }
            }
        }

		// CActivity 実装

		public override void On活性化()
		{
            this.ct炎 = new CCounter( 0, 6, 50, CDTXMania.Timer );

            for (int i = 0; i < 32; i++ )
            {
                this.st花火状態[i].ct進行 = new CCounter();
                this.st花火状態2P[i].ct進行 = new CCounter();
            }
			base.On活性化();
		}
		public override void On非活性化()
		{
            for (int i = 0; i < 32; i++ )
            {
                this.st花火状態[i].ct進行 = null;
                this.st花火状態2P[i].ct進行 = null;
            }
            this.ct炎 = null;
		}
		public override void OnManagedリソースの作成()
		{
			if( !base.b活性化してない )
			{
				this.txゲージ = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_Gauge.png" ) );
				this.txゲージ背景 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_Gauge_base.png" ) );
                if (CDTXMania.stage演奏ドラム画面.bDoublePlay)
                    this.txゲージ2P = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_Gauge_2P.png" ) );
                if (CDTXMania.stage演奏ドラム画面.bDoublePlay)
                    this.txゲージ背景2P = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_Gauge_base_2P.png" ) );
                this.txゲージ線 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_Gauge_line.png" ) );
                if (CDTXMania.stage演奏ドラム画面.bDoublePlay)
                    this.txゲージ線2P = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_Gauge_line_2P.png" ) );

                this.tx魂 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_Soul.png" ) );
                this.tx炎 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_Soul_fire.png" ) );

                this.tx魂花火 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_explosion_soul.png" ) );
                for( int i = 0; i < 12; i++ )
                {
                    this.txゲージ虹[ i ] = CDTXMania.tテクスチャの生成( CSkin.Path(@"Graphics\Gauge\Gauge_rainbow_" + i.ToString() + ".png") );
                }
                this.ct虹アニメ = new CCounter( 0, 11, 80, CDTXMania.Timer );
				base.OnManagedリソースの作成();
			}
		}
		public override void OnManagedリソースの解放()
		{
			if( !base.b活性化してない )
			{
				CDTXMania.tテクスチャの解放( ref this.txゲージ );
				CDTXMania.tテクスチャの解放( ref this.txゲージ背景 );
                if (CDTXMania.stage演奏ドラム画面.bDoublePlay)
                    CDTXMania.tテクスチャの解放( ref this.txゲージ2P );
                if (CDTXMania.stage演奏ドラム画面.bDoublePlay)
                    CDTXMania.tテクスチャの解放( ref this.txゲージ背景2P );
                CDTXMania.tテクスチャの解放( ref this.txゲージ線 );
                if (CDTXMania.stage演奏ドラム画面.bDoublePlay)
                    CDTXMania.tテクスチャの解放( ref this.txゲージ線2P );
                CDTXMania.tテクスチャの解放( ref this.tx魂 );
                CDTXMania.tテクスチャの解放( ref this.tx炎 );
                CDTXMania.tテクスチャの解放( ref this.tx魂花火 );


                for( int i = 0; i < 12; i++ )
                {
                    CDTXMania.tテクスチャの解放( ref this.txゲージ虹[ i ] );
                }
                this.ct虹アニメ = null;
				base.OnManagedリソースの解放();
			}
		}
		public override int On進行描画()
		{
			if ( !base.b活性化してない )
			{
                //CDTXMania.act文字コンソール.tPrint( 20, 150, C文字コンソール.Eフォント種別.白, this.db現在のゲージ値.Taiko.ToString() );

                #region [ 初めての進行描画 ]
				if ( base.b初めての進行描画 )
				{
					base.b初めての進行描画 = false;
                }
                #endregion


                int nRectX = (int)( this.db現在のゲージ値[ 0 ] / 2 ) * 14;
                int nRectX2P = (int)( this.db現在のゲージ値[ 1 ] / 2 ) * 14;

                if( this.txゲージ背景 != null )
                {
                    this.txゲージ背景.t2D描画( CDTXMania.app.Device, 492, 144, new Rectangle( 0, 0, 700, 44 ) );
                }
                if( CDTXMania.ConfigIni.nPlayerCount == 2 && this.txゲージ背景2P != null )
                {
                    this.txゲージ背景2P.t2D描画( CDTXMania.app.Device, 492, 532, new Rectangle( 0, 0, 700, 44 ) );
                }
                #region[ ゲージ1P ]
                if( this.txゲージ != null )
                {
                    this.txゲージ.t2D描画( CDTXMania.app.Device, 492, 144, new Rectangle( 0, 0, nRectX, 44 ) );

                    if( this.txゲージ線 != null )
                    {
                        if( this.db現在のゲージ値[ 0 ] >= 100.0 )
                        {
                            this.ct虹アニメ.t進行Loop();
                            if( this.txゲージ虹[ this.ct虹アニメ.n現在の値 ] != null )
                            {
                                this.txゲージ虹[ this.ct虹アニメ.n現在の値 ].t2D描画( CDTXMania.app.Device, 492, 144 );
                            }

                        }
                        this.txゲージ線.t2D描画( CDTXMania.app.Device, 492, 144 );
                    }
                    #region[ 「クリア」文字 ]
                    if( this.db現在のゲージ値[ 0 ] >= 80.0 )
                    {
                        this.txゲージ.t2D描画( CDTXMania.app.Device, 1038, 144, new Rectangle( 0, 44, 58, 24 ) );
                    }
                    else
                    {
                        this.txゲージ.t2D描画( CDTXMania.app.Device, 1038, 144, new Rectangle( 58, 44, 58, 24 ) );
                    }
                    #endregion
                }
                #endregion
                #region[ ゲージ2P ]
                if( CDTXMania.ConfigIni.nPlayerCount == 2 && this.txゲージ2P != null )
                {
                    this.txゲージ2P.t2D描画( CDTXMania.app.Device, 492, 532, new Rectangle( 0, 0, nRectX2P, 44 ) );

                    if( this.txゲージ線2P != null )
                    {
                        //if( this.db現在のゲージ値.Taiko >= 100.0 )
                        //{
                        //    this.ct虹アニメ.t進行Loop();
                        //    if( this.txゲージ虹[ this.ct虹アニメ.n現在の値 ] != null )
                        //    {
                        //        this.txゲージ虹[ this.ct虹アニメ.n現在の値 ].t2D描画( CDTXMania.app.Device, 492, 529 );
                        //    }

                        //}
                        this.txゲージ線2P.t2D描画( CDTXMania.app.Device, 492, 532 );
                    }
                    #region[ 「クリア」文字 ]
                    if( this.db現在のゲージ値[ 1 ] >= 80.0 )
                    {
                        this.txゲージ2P.t2D描画( CDTXMania.app.Device, 1038, 554, new Rectangle( 0, 44, 58, 24 ) );
                    }
                    else
                    {
                        this.txゲージ2P.t2D描画( CDTXMania.app.Device, 1038, 554, new Rectangle( 58, 44, 58, 24 ) );
                    }
                    #endregion
                }
                #endregion


                if( this.tx炎 != null )
                {
                    //仮置き
                    int[] nSoulFire = new int[] { 52, 443, 0, 0 };
                    for( int i = 0; i < CDTXMania.ConfigIni.nPlayerCount; i++ )
                    {
                        if( this.db現在のゲージ値[ i ] >= 100.0 )
                        {
                            this.ct炎.t進行Loop();
                            this.tx炎.t2D描画( CDTXMania.app.Device, 1112, nSoulFire[ i ], new Rectangle( 230 * ( this.ct炎.n現在の値 ), 0, 230, 230 ) );
                        }
                    }
                }
                if( this.tx魂 != null )
                {
                    //仮置き
                    int[] nSoulY = new int[] { 125, 516, 0, 0 };
                    for( int i = 0; i < CDTXMania.ConfigIni.nPlayerCount; i++ )
                    {
                        if( this.db現在のゲージ値[ i ] >= 80.0 )
                        {
                            this.tx魂.t2D描画( CDTXMania.app.Device, 1184, nSoulY[ i ], new Rectangle( 0, 0, 80, 80 ) );
                        }
                        else
                        {
                            this.tx魂.t2D描画( CDTXMania.app.Device, 1184, nSoulY[ i ], new Rectangle( 0, 80, 80, 80 ) );
                        }
                    }
                }

                //仮置き
                int[] nSoulExplosion = new int[] { 73, 468, 0, 0 };
                for( int d = 0; d < 32; d++ )
                {
                    if( this.st花火状態[d].b使用中 )
                    {
                        this.st花火状態[d].ct進行.t進行();
                        if (this.st花火状態[d].ct進行.b終了値に達した)
                        {
                            this.st花火状態[d].ct進行.t停止();
                            this.st花火状態[d].b使用中 = false;
                        }
                            
                            
                        if( this.tx魂花火 != null )
                        {
                            this.tx魂花火.t2D描画( CDTXMania.app.Device, 1140, 73, new Rectangle( this.st花火状態[d].ct進行.n現在の値 * 140, 0, 140, 180 ) );
                        }
                        break;
                    }
                }
                for( int d = 0; d < 32; d++ )
                {
                    if (this.st花火状態2P[d].b使用中)
                    {
                        this.st花火状態2P[d].ct進行.t進行();
                        if (this.st花火状態2P[d].ct進行.b終了値に達した)
                        {
                            this.st花火状態2P[d].ct進行.t停止();
                            this.st花火状態2P[d].b使用中 = false;
                        }
                            
                            
                        if( this.tx魂花火 != null )
                        {
                            this.tx魂花火.t2D描画( CDTXMania.app.Device, 1140, 468, new Rectangle( this.st花火状態2P[d].ct進行.n現在の値 * 140, 0, 140, 180 ) );
                        }
                        break;
                    }
                }
			}
			return 0;
		}


		// その他

		#region [ private ]
		//-----------------
        protected STSTATUS[] st花火状態 = new STSTATUS[ 32 ];
        protected STSTATUS[] st花火状態2P = new STSTATUS[ 32 ];
        [StructLayout(LayoutKind.Sequential)]
        protected struct STSTATUS
        {
            public CCounter ct進行;
            public bool isBig;
            public bool b使用中;
            public int nPlayer;
        }
		//-----------------
		#endregion
	}
}
