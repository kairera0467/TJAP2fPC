using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.IO;
using SlimDX;
using FDK;

namespace DTXMania
{
	internal class CAct演奏Drumsグラフ : CActivity
	{
        // #24074 2011.01.23 ikanick グラフの描画
        // 実装内容
        // ・左を現在、右を目標
        // ・基準線(60,70,80,90,100%)を超えると線が黄色くなる（元は白）
        // ・目標を超えると現在が光る
        // ・オート時には描画しない
        // 要望・実装予定
        // ・グラフを波打たせるなどの視覚の向上→実装済
        // 修正等
        // ・画像がないと落ちる→修正済

		// プロパティ

        public double dbグラフ値現在_渡
        {
            get
            {
                return this.dbグラフ値現在;
            }
            set
            {
                this.dbグラフ値現在 = value;
            }
        }
        public double dbグラフ値目標_渡
        {
            get
            {
                return this.dbグラフ値目標;
            }
            set
            {
                this.dbグラフ値目標 = value;
            }
        }
		
		// コンストラクタ

		public CAct演奏Drumsグラフ()
		{
			base.b活性化してない = true;
		}


		// CActivity 実装

		public override void On活性化()
        {
            this.dbグラフ値目標 = 80f;
            this.dbグラフ値現在 = 0f;
			base.On活性化();
		}
		public override void On非活性化()
		{
			base.On非活性化();
		}
		public override void OnManagedリソースの作成()
		{
			if( !base.b活性化してない )
			{
				string pathグラフ = CSkin.Path( @"Graphics\ScreenPlay graph.png" );
				if ( File.Exists( pathグラフ ) )
				{
					this.txグラフ = CDTXMania.tテクスチャの生成( pathグラフ );
				}
				base.OnManagedリソースの作成();
			}
		}
		public override void OnManagedリソースの解放()
		{
			if( !base.b活性化してない )
			{
				CDTXMania.tテクスチャの解放( ref this.txグラフ );
				base.OnManagedリソースの解放();
			}
		}
		public override int On進行描画()
		{
			if( !base.b活性化してない )
			{
				if( base.b初めての進行描画 )
				{
					for( int k = 0; k < 64; k++ )
					{
						this.stキラキラ[ k ].x = 0 + CDTXMania.Random.Next( 8 );
						this.stキラキラ[ k ].fScale = 1f + ( CDTXMania.Random.Next( 9 ) * 0.2f );
						this.stキラキラ[ k ].Trans = 0 + CDTXMania.Random.Next( 32 ) ;
                        if (k < 32)
                        {
                            this.stキラキラ[ k ].ct進行 = new CCounter(0, 230, 10 + CDTXMania.Random.Next(20), CDTXMania.Timer);
                        }
                        else if (k < 64)
                        {
                            this.stキラキラ[ k ].ct進行 = new CCounter(0, 230, 20 + CDTXMania.Random.Next(50), CDTXMania.Timer);
                        }
                        this.stキラキラ[ k ].ct進行.n現在の値 = CDTXMania.Random.Next(230);
					}
					for( int k = 0; k < 16; k++ )
					{
						this.stフラッシュ[ k ].y = -1;
						this.stフラッシュ[ k ].Trans = 0;
					}
					base.b初めての進行描画 = false;
                }
                // 背景暗幕
                Rectangle rectangle = new Rectangle(22, 0, 1, 1);
                if (this.txグラフ != null)
                {
                    this.txグラフ.vc拡大縮小倍率 = new Vector3(38f, 230f, 1f);
                    this.txグラフ.n透明度 = 128;
                    this.txグラフ.t2D描画(CDTXMania.app.Device, 345, 88, rectangle);
                }
                
                // 基準線
                rectangle = new Rectangle(20, 0, 1, 1);
                if (this.txグラフ != null)
                {
                    this.txグラフ.n透明度 = 32;
                    this.txグラフ.vc拡大縮小倍率 = new Vector3(38f, 1f, 1f);
                    for (int i = 0; i < 20; i++)
                    {
                        this.txグラフ.t2D描画(CDTXMania.app.Device, 345, 88 + (int)(11.5 * i), rectangle);
                    }
                    this.txグラフ.vc拡大縮小倍率 = new Vector3(1f, 230f, 1f);
                    for (int i = 0; i < 2; i++)
                    {
                        this.txグラフ.t2D描画(CDTXMania.app.Device, 349 + i * 18, 88 , rectangle);
                        this.txグラフ.t2D描画(CDTXMania.app.Device, 360 + i * 18, 88 , rectangle);
                    }
                }
                if (this.txグラフ != null)
                {
                    this.txグラフ.vc拡大縮小倍率 = new Vector3(38f, 1f, 1f);
                }
                for (int i = 0; i < 5; i++)
                {
                    // 基準線を越えたら線が黄色くなる
                    if (this.dbグラフ値現在 >= (100 - i * 10))
                    {
                        rectangle = new Rectangle(21, 0, 1, 1);//黄色
                        if (this.txグラフ != null)
                        {
                            this.txグラフ.n透明度 = 224;
                        }
                    }
                    else
                    {
                        rectangle = new Rectangle(20, 0, 1, 1);
                        if (this.txグラフ != null)
                        {
                            this.txグラフ.n透明度 = 160;
                        }
                    }

                    if (this.txグラフ != null)
                    {
                        this.txグラフ.t2D描画(CDTXMania.app.Device, 345, 88 + i * 23, rectangle);
                    }
                }
                // グラフ
                // --現在値
                if (this.dbグラフ値現在_表示 < this.dbグラフ値現在)
                {
                    this.dbグラフ値現在_表示 += (this.dbグラフ値現在 - this.dbグラフ値現在_表示) / 5 + 0.01;
                }
                if (this.dbグラフ値現在_表示 >= this.dbグラフ値現在)
                {
                    this.dbグラフ値現在_表示 = this.dbグラフ値現在;
                }
                rectangle = new Rectangle(0, 0, 10, (int)(230f * this.dbグラフ値現在_表示 / 100));
                if (this.txグラフ != null)
                {
                    this.txグラフ.vc拡大縮小倍率 = new Vector3(1f, 1f, 1f);
                    this.txグラフ.n透明度 = 192;
                    this.txグラフ.t2D描画(CDTXMania.app.Device, 350, 318 - (int)(230f * this.dbグラフ値現在_表示 / 100), rectangle);
                }
				for( int k = 0; k < 32; k++ )
				{
                    rectangle = new Rectangle(20, 0, 1, 1);
                    if (this.txグラフ != null)
                    {
				    	this.stキラキラ[ k ].ct進行.t進行Loop();
                        int num1 = (int)this.stキラキラ[ k ].x;
                        int num2 = this.stキラキラ[ k ].ct進行.n現在の値;
                        this.txグラフ.vc拡大縮小倍率 = new Vector3(this.stキラキラ[ k ].fScale, this.stキラキラ[ k ].fScale, this.stキラキラ[ k ].fScale);
                        this.txグラフ.n透明度 = 138 - 2 * this.stキラキラ[ k ].Trans;
                        if ( num2 < (2.3f * this.dbグラフ値現在_表示) )
                        {
                            this.txグラフ.t2D描画(CDTXMania.app.Device, 350+num1, 318-num2, rectangle);
                        }
                    }
				}
                // --現在値_追加エフェクト
                if (this.dbグラフ値直前 != this.dbグラフ値現在)
                {
                    this.stフラッシュ[ nグラフフラッシュct ].y = 0;
                    this.stフラッシュ[ nグラフフラッシュct ].Trans = 224;
                    nグラフフラッシュct ++;
                    if (nグラフフラッシュct >= 16)
                    {
                        nグラフフラッシュct = 0;
                    }
                }
                this.dbグラフ値直前 = this.dbグラフ値現在;
                for (int m = 0; m < 16; m++)
                {
                    rectangle = new Rectangle(20, 0, 1, 1);
                    if ((this.stフラッシュ[ m ].y >= 0) && (this.stフラッシュ[ m ].y+3 < (int)(230f * this.dbグラフ値現在_表示 / 100)) && (this.txグラフ != null))
                    {
                        this.txグラフ.vc拡大縮小倍率 = new Vector3(10f, 1f, 1f);
                        this.txグラフ.n透明度 = this.stフラッシュ[ m ].Trans;
                        this.txグラフ.t2D描画(CDTXMania.app.Device, 350, this.stフラッシュ[ m ].y + (318 - (int)(230f * this.dbグラフ値現在_表示 / 100)), rectangle);
                        this.txグラフ.n透明度 = this.stフラッシュ[ m ].Trans;
                        this.txグラフ.t2D描画(CDTXMania.app.Device, 350, this.stフラッシュ[ m ].y + 2 + (318 - (int)(230f * this.dbグラフ値現在_表示 / 100)), rectangle);
                    }
                    this.stフラッシュ[ m ].y += 4;
                    this.stフラッシュ[ m ].Trans -= 4;
                }
                // --現在値_目標越
                rectangle = new Rectangle(0, 0, 10, (int)(230f * this.dbグラフ値現在_表示 / 100));
                if ((dbグラフ値現在 >= dbグラフ値目標) && (this.txグラフ != null))
                {
                    this.txグラフ.vc拡大縮小倍率 = new Vector3(1.4f, 1f, 1f);
                    this.txグラフ.n透明度 = 128;
                    this.txグラフ.b加算合成 = true;
                    this.txグラフ.t2D描画(CDTXMania.app.Device, 348, 318 - (int)(230f * this.dbグラフ値現在_表示 / 100), rectangle);
                    this.txグラフ.b加算合成 = false;
                }
                // --目標値
                if (this.dbグラフ値目標_表示 < this.dbグラフ値目標)
                {
                    this.dbグラフ値目標_表示 += (this.dbグラフ値目標 - this.dbグラフ値目標_表示) / 5 + 0.01;
                }
                if (this.dbグラフ値目標_表示 >= this.dbグラフ値目標)
                {
                    this.dbグラフ値目標_表示 = this.dbグラフ値目標;
                }
                rectangle = new Rectangle(10, 0, 10, (int)(230f * this.dbグラフ値目標_表示 / 100));
                if (this.txグラフ != null)
                {
                    this.txグラフ.vc拡大縮小倍率 = new Vector3(1f, 1f, 1f);
                    this.txグラフ.n透明度 = 192;
                    this.txグラフ.t2D描画(CDTXMania.app.Device, 368, 318 - (int)(230f * this.dbグラフ値目標_表示 / 100), rectangle);
                    this.txグラフ.vc拡大縮小倍率 = new Vector3(1.4f, 1f, 1f);
                    this.txグラフ.n透明度 = 48;
                    this.txグラフ.b加算合成 = true;
                    this.txグラフ.t2D描画(CDTXMania.app.Device, 366, 318 - (int)(230f * this.dbグラフ値目標_表示 / 100), rectangle);
                    this.txグラフ.b加算合成 = false;
                }
				for( int k = 32; k < 64; k++ )
				{
                    rectangle = new Rectangle(20, 0, 1, 1);
                    if (this.txグラフ != null)
                    {
				    	this.stキラキラ[ k ].ct進行.t進行Loop();
                        int num1 = (int)this.stキラキラ[ k ].x;
                        int num2 = this.stキラキラ[ k ].ct進行.n現在の値;
                        this.txグラフ.vc拡大縮小倍率 = new Vector3(this.stキラキラ[ k ].fScale, this.stキラキラ[ k ].fScale, this.stキラキラ[ k ].fScale);
                        this.txグラフ.n透明度 = 138 - 2 * this.stキラキラ[ k ].Trans;
                        if ( num2 < (2.3f * this.dbグラフ値目標_表示) )
                        {
                            this.txグラフ.t2D描画(CDTXMania.app.Device, 368+num1, 318-num2, rectangle);
                        }
                    }
				}
                
			}
			return 0;
		}


		// その他

		#region [ private ]
		//----------------
		[StructLayout( LayoutKind.Sequential )]
		private struct STキラキラ
		{
			public int x;
			public int y;
			public float fScale;
			public int Trans;
			public CCounter ct進行;
		}
        private STキラキラ[] stキラキラ = new STキラキラ[ 64 ];
        private STキラキラ[] stフラッシュ = new STキラキラ[ 16 ];

        private double dbグラフ値目標;
        private double dbグラフ値目標_表示;
        private double dbグラフ値現在;
        private double dbグラフ値現在_表示;
        private double dbグラフ値直前;
        private int nグラフフラッシュct;

		private CTexture txグラフ;
		//-----------------
		#endregion
	}
}
