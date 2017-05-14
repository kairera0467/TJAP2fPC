using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using FDK;

namespace DTXMania
{
	internal class CAct演奏DrumsレーンフラッシュD : CActivity
	{
		// コンストラクタ

		public CAct演奏DrumsレーンフラッシュD()
		{
            STレーンサイズ[] stレーンサイズArray = new STレーンサイズ[ 11 ];
            STレーンサイズ stレーンサイズ = new STレーンサイズ();
            stレーンサイズ.x = 0x12a;
            stレーンサイズ.w = 0x40;
            stレーンサイズArray[ 0 ] = stレーンサイズ;
            STレーンサイズ stレーンサイズ2 = new STレーンサイズ();
            stレーンサイズ2.x = 370;
            stレーンサイズ2.w = 0x2e;
            stレーンサイズArray[ 1 ] = stレーンサイズ2;
            STレーンサイズ stレーンサイズ3 = new STレーンサイズ();
            stレーンサイズ3.x = 470;
            stレーンサイズ3.w = 0x36;
            stレーンサイズArray[ 2 ] = stレーンサイズ3;
            STレーンサイズ stレーンサイズ4 = new STレーンサイズ();
            stレーンサイズ4.x = 0x246;
            stレーンサイズ4.w = 60;
            stレーンサイズArray[ 3 ] = stレーンサイズ4;
            STレーンサイズ stレーンサイズ5 = new STレーンサイズ();
            stレーンサイズ5.x = 0x210;
            stレーンサイズ5.w = 0x2e;
            stレーンサイズArray[ 4 ] = stレーンサイズ5;
            STレーンサイズ stレーンサイズ6 = new STレーンサイズ();
            stレーンサイズ6.x = 0x285;
            stレーンサイズ6.w = 0x2e;
            stレーンサイズArray[ 5 ] = stレーンサイズ6;
            STレーンサイズ stレーンサイズ7 = new STレーンサイズ();
            stレーンサイズ7.x = 0x2b6;
            stレーンサイズ7.w = 0x2e;
            stレーンサイズArray[ 6 ] = stレーンサイズ7;
            STレーンサイズ stレーンサイズ8 = new STレーンサイズ();
            stレーンサイズ8.x = 0x2ec;
            stレーンサイズ8.w = 0x40;
            stレーンサイズArray[ 7 ] = stレーンサイズ8;
            STレーンサイズ stレーンサイズ9 = new STレーンサイズ();
            stレーンサイズ9.x = 0x1a3;
            stレーンサイズ9.w = 0x30;
            stレーンサイズArray[ 8 ] = stレーンサイズ9;
            STレーンサイズ stレーンサイズ10 = new STレーンサイズ();
            stレーンサイズ10.x = 0x32f;
            stレーンサイズ10.w = 0x26;
            stレーンサイズArray[ 9 ] = stレーンサイズ10;
            STレーンサイズ stレーンサイズ11 = new STレーンサイズ();
            stレーンサイズ11.x = 0x1a3;
            stレーンサイズ11.w = 0x30;
            stレーンサイズArray[ 10 ] = stレーンサイズ11;
            this.stレーンサイズ = stレーンサイズArray;
            this.strファイル名 = new string[] { 
        @"Graphics\ScreenPlayDrums lane flush leftcymbal.png",
        @"Graphics\ScreenPlayDrums lane flush hihat.png",
        @"Graphics\ScreenPlayDrums lane flush snare.png",
        @"Graphics\ScreenPlayDrums lane flush bass.png",
        @"Graphics\ScreenPlayDrums lane flush hitom.png",
        @"Graphics\ScreenPlayDrums lane flush lowtom.png",
        @"Graphics\ScreenPlayDrums lane flush floortom.png",
        @"Graphics\ScreenPlayDrums lane flush cymbal.png",
        @"Graphics\ScreenPlayDrums lane flush leftpedal.png",
        @"Graphics\ScreenPlayDrums lane flush hihat.png",
        @"Graphics\ScreenPlayDrums lane flush leftpedal.png",

        @"Graphics\ScreenPlayDrums lane flush leftcymbal reverse.png",
        @"Graphics\ScreenPlayDrums lane flush hihat reverse.png",
        @"Graphics\ScreenPlayDrums lane flush snare reverse.png",
        @"Graphics\ScreenPlayDrums lane flush bass reverse.png",
        @"Graphics\ScreenPlayDrums lane flush hitom reverse.png", 
        @"Graphics\ScreenPlayDrums lane flush lowtom reverse.png",
        @"Graphics\ScreenPlayDrums lane flush floortom reverse.png",
        @"Graphics\ScreenPlayDrums lane flush cymbal reverse.png",
        @"Graphics\ScreenPlayDrums lane flush leftpedal reverse.png",
        @"Graphics\ScreenPlayDrums lane flush hihat reverse.png",
        @"Graphics\ScreenPlayDrums lane flush leftpedal reverse.png"
     };
			base.b活性化してない = true;
		}
		
		
		// メソッド

		public void Start( int nLane, float f強弱度合い )
		{
			int num = (int) ( ( 1f - f強弱度合い ) * 55f );
            if( nLane == 0 )
            {
			    this.ct進行[ 0 ] = new CCounter( 0, 100, 2, CDTXMania.Timer );
            }
            else if( nLane == 1 )
            {
			    this.ct進行[ 1 ] = new CCounter( 0, 100, 2, CDTXMania.Timer );
            }
            else if( nLane == 2 )
            {
			    this.ct進行[ 2 ] = new CCounter( 0, 100, 2, CDTXMania.Timer );
            }
		}


		// CActivity 実装

		public override void On活性化()
		{
			for( int i = 0; i < 11; i++ )
			{
				this.ct進行[ i ] = new CCounter();
			}
			base.On活性化();
		}
		public override void On非活性化()
		{
			for( int i = 0; i < 11; i++ )
			{
				this.ct進行[ i ] = null;
			}
			base.On非活性化();
		}
		public override void OnManagedリソースの作成()
		{
			if( !base.b活性化してない )
			{
                this.txRed = CDTXMania.tテクスチャの生成( CSkin.Path(@"Graphics\7_LaneFlush_Red.png") );
                this.txBlue = CDTXMania.tテクスチャの生成( CSkin.Path(@"Graphics\7_LaneFlush_Blue.png") );
                this.txYellow = CDTXMania.tテクスチャの生成( CSkin.Path(@"Graphics\7_LaneFlush_Yellow.png") );
				base.OnManagedリソースの作成();
			}
		}
		public override void OnManagedリソースの解放()
		{
			if( !base.b活性化してない )
			{
                CDTXMania.tテクスチャの解放( ref this.txRed );
                CDTXMania.tテクスチャの解放( ref this.txBlue );
                CDTXMania.tテクスチャの解放( ref this.txYellow );
				base.OnManagedリソースの解放();
			}
		}
		public override int On進行描画()
		{
			if( !base.b活性化してない )
			{
				for( int i = 0; i < 11; i++ )
				{
					if( !this.ct進行[ i ].b停止中 )
					{
						this.ct進行[ i ].t進行();
						if( this.ct進行[ i ].b終了値に達した )
						{
							this.ct進行[ i ].t停止();
						}
					}
				}

                if( this.ct進行[0].b進行中 )
                {
	                int num8 = ( ( ( 100 - this.ct進行[0].n現在の値 ) * 0xff ) / 100 );
					if( this.txRed != null && this.txYellow != null )
					{
                        this.txRed.n透明度 = ( num8 );
						this.txRed.t2D描画( CDTXMania.app.Device, 333, 192 );
                        this.txYellow.n透明度 = ( num8 );
						this.txYellow.t2D描画( CDTXMania.app.Device, 333, 192 );
					}
                }
                if( this.ct進行[1].b進行中 )
                {
	                int num8 = ( ( ( 100 - this.ct進行[1].n現在の値 ) * 0xff ) / 100 );
					if( this.txBlue != null && this.txYellow != null )
					{
                        this.txBlue.n透明度 = ( num8 );
						this.txBlue.t2D描画( CDTXMania.app.Device, 333, 192 );
                        this.txYellow.n透明度 = ( num8 );
						this.txYellow.t2D描画( CDTXMania.app.Device, 333, 192 );
					}
                }
                if( this.ct進行[2].b進行中 )
                {
	                int num8 = ( ( ( 100 - this.ct進行[2].n現在の値 ) * 0xff ) / 100 );
					if( this.txYellow != null )
					{
                        this.txYellow.n透明度 = ( num8 );
						this.txYellow.t2D描画( CDTXMania.app.Device, 333, 192 );
					}
                }
			}
			return 0;
		}

		
		// その他

		#region [ private ]
		//-----------------
		[StructLayout( LayoutKind.Sequential )]
		private struct STレーンサイズ
		{
			public int x;
			public int w;
		}

		private CCounter[] ct進行 = new CCounter[ 11 ];
		private readonly string[] strファイル名;
		private readonly STレーンサイズ[] stレーンサイズ;
		private CTexture[] txFlush = new CTexture[ 0x16 ];

        private CTexture txRed;
        private CTexture txBlue;
        private CTexture txYellow;
		//-----------------
		#endregion
	}
}
