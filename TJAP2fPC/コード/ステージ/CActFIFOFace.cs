using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using SlimDX;
using FDK;

namespace DTXMania
{
    /// <summary>
    /// 太鼓Wiiっぽいフェードイン・フェードアウト演出。
    /// 実装を見てわかるとおり、画像の変更は(ほぼ)想定していないので注意。
    /// </summary>
	internal class CActFIFOFace : CActivity
	{
		// メソッド

		public void tフェードアウト開始()
		{
			this.mode = EFIFOモード.フェードアウト;
			this.counter = new CCounter( 0, 100, 5, CDTXMania.Timer );
		}
		public void tフェードイン開始()
		{
			this.mode = EFIFOモード.フェードイン;
			this.counter = new CCounter( 0, 100, 5, CDTXMania.Timer );
		}
		public void tフェードイン完了()
		{
			this.counter.n現在の値 = this.counter.n終了値;
		}

		// CActivity 実装

		public override void On非活性化()
		{
			if( !base.b活性化してない )
			{
				CDTXMania.tテクスチャの解放( ref this.txタイル );
                CDTXMania.tテクスチャの解放( ref this.txタイル背景 );
				base.On非活性化();
			}
		}
		public override void OnManagedリソースの作成()
		{
			if( !base.b活性化してない )
			{
				this.txタイル = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\FIFO Face.png" ), false );
                this.txタイル背景 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\Tile red 64x64.png" ) );
				base.OnManagedリソースの作成();
			}
		}
		public override int On進行描画()
		{
			if( base.b活性化してない || ( this.counter == null ) )
			{
				return 0;
			}
			this.counter.t進行();
            
            if( CDTXMania.Input管理.Keyboard.bキーが押された( (int) SlimDX.DirectInput.Key.NumberPad5 ) )
            {
                this.pos = 50;
            }
            if( CDTXMania.Input管理.Keyboard.bキーが押された( (int) SlimDX.DirectInput.Key.NumberPad4 ) )
            {
                this.pos--;
            }
            if( CDTXMania.Input管理.Keyboard.bキーが押された( (int) SlimDX.DirectInput.Key.NumberPad6 ) )
            {
                this.pos++;
            }

            if( this.mode == EFIFOモード.フェードイン )
            {
			    if( this.txタイル != null && this.txタイル背景 != null && this.counter.n現在の値 < 100 )
			    {
                    float f拡大率 = ( ( this.counter.n現在の値 / 100.0f ) * 2.0f ) + 0.18f; // 0.1～2.1
                    Matrix mat = Matrix.Identity;
                    mat *= Matrix.Scaling( f拡大率, f拡大率, 1.0f );
                    mat *= Matrix.Translation( 0, 0, 0 );
				    this.txタイル.t3D描画( CDTXMania.app.Device, mat );

                    int n描画枚数x = 0;
                    int n描画枚数y = 0;

                    if( this.counter.n現在の値 < 1 ) n描画枚数x = 9;
                    else if( this.counter.n現在の値 < 7 ) n描画枚数x = 8;
                    else if( this.counter.n現在の値 < 12 ) n描画枚数x = 7;
                    else if( this.counter.n現在の値 < 17 ) n描画枚数x = 6;
                    else if( this.counter.n現在の値 < 21 ) n描画枚数x = 5;
                    else if( this.counter.n現在の値 < 26 ) n描画枚数x = 4;
                    else if( this.counter.n現在の値 < 31 ) n描画枚数x = 3;
                    else if( this.counter.n現在の値 < 36 ) n描画枚数x = 2;
                    else if( this.counter.n現在の値 < 41 ) n描画枚数x = 1;
                    else n描画枚数x = 0;

                    if( this.counter.n現在の値 < 7 ) n描画枚数y = 4;
                    else if( this.counter.n現在の値 < 12 ) n描画枚数y = 3;
                    else if( this.counter.n現在の値 < 15 ) n描画枚数y = 2;
                    else if( this.counter.n現在の値 < 20 ) n描画枚数y = 1;
                    else n描画枚数y = 0;

                    for( int h = 0; h < 21; h++ )
                    {
                        for ( int i = 0; i < n描画枚数y; i++ ) // 4:0-6, 3:0-11 2:0-14 1:0-19
                        {
                            this.txタイル背景.t2D描画( CDTXMania.app.Device, h * 64, i * 64 );
                            this.txタイル背景.t2D描画( CDTXMania.app.Device, h * 64, (720 - 64) - i * 64 );
                        }
                    }
                    for( int h = 0; h < 13; h++ )
                    {
                        for ( int i = 0; i < n描画枚数x; i++ ) // 9:0-1, 8:0-6, 7:0-11 6:0-16 5:0-20 4:0-25 3:0-30 2:0-35 1:0-40
                        {
                            this.txタイル背景.t2D描画( CDTXMania.app.Device, i * 64,  h * 64 );
                            this.txタイル背景.t2D描画( CDTXMania.app.Device, (1280 - 64) - i * 64, h * 64 );
                        }
                    }

                
                    //this.txタイル背景.t2D描画( CDTXMania.app.Device, 0, 0 );
			    }
            }
            else
            {
                //this.counter.n現在の値 = pos;
			    if( this.txタイル != null && this.txタイル背景 != null )
			    {
                    float f拡大率 = 2.18f - ( ( ( this.counter.n現在の値 / 100.0f ) * 2.0f ) + 0.18f ); // 0.1～2.1
                    Matrix mat = Matrix.Identity;
                    mat *= Matrix.Scaling( f拡大率, f拡大率, 1.0f );
                    mat *= Matrix.Translation( 0, 0, 0 );
				    this.txタイル.t3D描画( CDTXMania.app.Device, mat );

                    int n描画枚数x = 0;
                    int n描画枚数y = 0;

                    if( this.counter.n現在の値 < 51 ) n描画枚数x = 0;
                    else if( this.counter.n現在の値 < 56 ) n描画枚数x = 1;
                    else if( this.counter.n現在の値 < 61 ) n描画枚数x = 2;
                    else if( this.counter.n現在の値 < 66 ) n描画枚数x = 3;
                    else if( this.counter.n現在の値 < 71 ) n描画枚数x = 4;
                    else if( this.counter.n現在の値 < 76 ) n描画枚数x = 5;
                    else if( this.counter.n現在の値 < 81 ) n描画枚数x = 6;
                    else if( this.counter.n現在の値 < 86 ) n描画枚数x = 7;
                    else n描画枚数x = 8;

                    if( this.counter.n現在の値 < 72 ) n描画枚数y = 0;
                    else if( this.counter.n現在の値 < 77 ) n描画枚数y = 1;
                    else if( this.counter.n現在の値 < 82 ) n描画枚数y = 2;
                    else if( this.counter.n現在の値 < 87 ) n描画枚数y = 3;
                    else n描画枚数y = 4;

                    if( f拡大率 >= 0.20f )
                    {
                        for( int h = 0; h < 21; h++ )
                        {
                            for ( int i = 0; i < n描画枚数y; i++ ) // 4:0-6, 3:0-11 2:0-14 1:0-19
                            {
                                this.txタイル背景.t2D描画( CDTXMania.app.Device, h * 64, i * 64 );
                                this.txタイル背景.t2D描画( CDTXMania.app.Device, h * 64, (720 - 64) - i * 64 );
                            }
                        }
                        for( int h = 0; h < 13; h++ )
                        {
                            for ( int i = 0; i < n描画枚数x; i++ ) // 9:0-1, 8:0-6, 7:0-11 6:0-16 5:0-20 4:0-25 3:0-30 2:0-35 1:0-40
                            {
                                this.txタイル背景.t2D描画( CDTXMania.app.Device, i * 64,  h * 64 );
                                this.txタイル背景.t2D描画( CDTXMania.app.Device, (1280 - 64) - i * 64, h * 64 );
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i <= (1280 / 64); i++)		// #23510 2010.10.31 yyagi: change "clientSize.Width" to "640" to fix FIFO drawing size
				        {
					        for (int j = 0; j <= (720 / 64); j++)	// #23510 2010.10.31 yyagi: change "clientSize.Height" to "480" to fix FIFO drawing size
					        {
						        this.txタイル背景.t2D描画( CDTXMania.app.Device, i * 64, j * 64 );
					        }
				        }
                    }
			    }
            }

			if( this.counter.n現在の値 != 100 )
			{
				return 0;
			}
			return 1;
		}


		// その他

		#region [ private ]
		//-----------------
		private CCounter counter;
		private EFIFOモード mode;
		private CTexture txタイル;
        private CTexture txタイル背景;
        private int pos;
		//-----------------
		#endregion
	}
}
