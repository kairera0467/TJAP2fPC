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
        /// ・ゲージの実装。
        /// ・Danger時にゲージの色が変わる演出の実装。
        /// ・Danger、MAX時のアニメーション実装。
        /// </summary>
		public CAct演奏Drumsゲージ()
		{
			base.b活性化してない = true;
		}

        public override void Start(int nLane, E判定 judge)
        {
            for (int j = 0; j < 16; j++)
            {
                for (int i = 0; i < 1; i++ )
                {
                    this.st花火状態[j].ct進行 = new CCounter(0, 10, 20, CDTXMania.Timer);

                    switch (nLane)
                    {
                        case 0x93:
                        case 0x94:
                        case 0x97:
                            this.st花火状態[j].isBig = false;
                            break;
                        case 3:
                        case 4:
                        case 0x98:
                        case 0x99:
                            this.st花火状態[j].isBig = true;
                            break;
                    }

                    this.st花火状態[j].b使用中 = true;
                }
            }
        }

		// CActivity 実装

		public override void On活性化()
		{
			// CAct演奏ゲージ共通.Init()に移動
			// this.dbゲージ値 = ( CDTXMania.ConfigIni.nRisky > 0 ) ? 1.0 : 0.66666666666666663;
            this.ctマスク透明度タイマー = new CCounter(0, 1500, 2, CDTXMania.Timer);
            this.ct炎 = new CCounter( 0, 6, 50, CDTXMania.Timer );

            for (int i = 0; i < 16; i++ )
            {
                this.st花火状態[i].ct進行 = new CCounter();
            }
			base.On活性化();
		}
		public override void On非活性化()
		{
            this.ct本体振動 = null;
            this.ct本体移動 = null;
            for (int i = 0; i < 16; i++ )
            {
                this.st花火状態[i].ct進行 = null;
            }
            this.ct炎 = null;
		}
		public override void OnManagedリソースの作成()
		{
			if( !base.b活性化してない )
			{
				this.txゲージ = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_Gauge.png" ) );
				this.txゲージ背景 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_Gauge_base.png" ) );

                this.tx魂 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_Soul.png" ) );
                this.tx炎 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_Soul_fire.png" ) );

                this.tx魂花火 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_explosion_soul.png" ) );

                this.txゲージ線 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_Gauge_line.png" ) );
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
                CDTXMania.tテクスチャの解放( ref this.txゲージマスクDANGER );
                CDTXMania.tテクスチャの解放( ref this.txゲージマスクMAX );
                CDTXMania.tテクスチャの解放( ref this.tx魂 );
                CDTXMania.tテクスチャの解放( ref this.tx炎 );
                CDTXMania.tテクスチャの解放( ref this.tx魂花火 );

                CDTXMania.tテクスチャの解放( ref this.txゲージ線 );
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


                int nRectX = (int)( this.db現在のゲージ値.Taiko / 2 ) * 14;

                if( this.txゲージ背景 != null )
                {
                    this.txゲージ背景.t2D描画( CDTXMania.app.Device, 492, 144, new Rectangle( 0, 0, 700, 44 ) );
                }

                if( this.txゲージ != null )
                {
                    this.txゲージ.t2D描画( CDTXMania.app.Device, 492, 144, new Rectangle( 0, 0, nRectX, 44 ) );

                    if( this.txゲージ線 != null )
                    {
                        if( this.db現在のゲージ値.Taiko >= 100.0 )
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
                    if( this.db現在のゲージ値.Taiko >= 80.0 )
                    {
                        this.txゲージ.t2D描画( CDTXMania.app.Device, 1038, 144, new Rectangle( 0, 44, 58, 24 ) );
                    }
                    else
                    {
                        this.txゲージ.t2D描画( CDTXMania.app.Device, 1038, 144, new Rectangle( 58, 44, 58, 24 ) );
                    }
                    #endregion
                }
                if( this.db現在のゲージ値.Taiko >= 100.0 )
                {
                    this.ct炎.t進行Loop();
                    if( this.tx炎 != null )
                    {
                        this.tx炎.t2D描画( CDTXMania.app.Device, 1112, 52, new Rectangle( 230 * ( this.ct炎.n現在の値 ), 0, 230, 230 ) );
                    }
                }
                if( this.tx魂 != null )
                {
                    if( this.db現在のゲージ値.Taiko >= 80.0 )
                    {
                        this.tx魂.t2D描画( CDTXMania.app.Device, 1184, 125, new Rectangle( 0, 0, 80, 80 ) );
                    }
                    else
                    {
                        this.tx魂.t2D描画( CDTXMania.app.Device, 1184, 125, new Rectangle( 0, 80, 80, 80 ) );
                    }
                }
                for( int i = 0; i < 1; i++ )
                {
                    for( int d = 0; d < 16; d++ )
                    {
                        if (this.st花火状態[i].b使用中)
                        {
                            this.st花火状態[i].ct進行.t進行();
                            if (this.st花火状態[i].ct進行.b終了値に達した)
                            {
                                this.st花火状態[i].ct進行.t停止();
                                this.st花火状態[i].b使用中 = false;
                            }
        
                            if( this.tx魂花火 != null )
                            {
                                this.tx魂花火.t2D描画( CDTXMania.app.Device, 1140, 73, new Rectangle( this.st花火状態[i].ct進行.n現在の値 * 140, 0, 140, 180 ) );
                            }
                        }
                    }
                }
			}
			return 0;
		}


		// その他

		#region [ private ]
		//-----------------
        protected STSTATUS[] st花火状態 = new STSTATUS[16];
        [StructLayout(LayoutKind.Sequential)]
        protected struct STSTATUS
        {
            public CCounter ct進行;
            public bool isBig;
            public bool b使用中;
        }
		//-----------------
		#endregion
	}
}
