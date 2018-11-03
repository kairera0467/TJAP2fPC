using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using SlimDX;
using FDK;

namespace DTXMania
{
	internal class CAct演奏Drums飛んでいく音符 : CActivity
	{
		// コンストラクタ

		public CAct演奏Drums飛んでいく音符()
		{
			base.b活性化してない = true;
		}
		
        public virtual void t虹( int player )
		{
            if (CDTXMania.Tx.Effects_Rainbow != null)
            {
                for (int i = 0; i < 2; i++)
                {
                    if (!this.st虹[i].b使用中 && player == 0)
                    {
                        this.st虹[i].b使用中 = true;
                        this.st虹[i].ct進行 = new CCounter(0, 164, CDTXMania.Skin.Game_Effect_Rainbow_Timer, CDTXMania.Timer); // カウンタ
                        this.st虹[i].nPlayer = player;
                        break;
                    }
                    if (!this.st虹2[i].b使用中 && player == 1)
                    {
                        this.st虹2[i].b使用中 = true;
                        this.st虹2[i].ct進行 = new CCounter(0, 164, CDTXMania.Skin.Game_Effect_Rainbow_Timer, CDTXMania.Timer); // カウンタ
                        this.st虹2[i].nPlayer = player;
                        break;
                    }
                }
            }
		}


		// CActivity 実装

		public override void On活性化()
		{
            for( int i = 0; i < 2; i++ )
			{
				this.st虹[ i ].ct進行 = new CCounter();
				this.st虹2[ i ].ct進行 = new CCounter();
			}
            base.On活性化();
		}
		public override void On非活性化()
		{
            for( int i = 0; i < 2; i++ )
			{
				this.st虹[ i ].ct進行 = null;
				this.st虹2[ i ].ct進行 = null;
			}
			base.On非活性化();
		}
		public override void OnManagedリソースの作成()
		{
			if( !base.b活性化してない )
			{
				base.OnManagedリソースの作成();
			}
		}
		public override void OnManagedリソースの解放()
		{
			if( !base.b活性化してない )
			{
				base.OnManagedリソースの解放();
			}
		}
		public override int On進行描画()
		{
			if( !base.b活性化してない )
			{
                for (int f = 0; f < 2; f++)
                {
                    if (this.st虹[f].b使用中)
                    {
                        this.st虹[f].ct進行.t進行();
                        if (this.st虹[f].ct進行.b終了値に達した)
                        {
                            this.st虹[f].ct進行.t停止();
                            this.st虹[f].b使用中 = false;
                        }

                        if(CDTXMania.Tx.Effects_Rainbow != null && this.st虹[f].nPlayer == 0 ) //画像が出来るまで
                        {
                            //this.st虹[f].ct進行.n現在の値 = 164;

                            if (this.st虹[f].ct進行.n現在の値 < 82)
                            {
                                int nRectX = ((this.st虹[f].ct進行.n現在の値 * 920) / 85);
                                CDTXMania.Tx.Effects_Rainbow.t2D描画(CDTXMania.app.Device, 360, -100, new Rectangle(0, 0, nRectX, 410));
                            }
                            else if (this.st虹[f].ct進行.n現在の値 >= 82)
                            {
                                int nRectX = (((this.st虹[f].ct進行.n現在の値 - 82) * 920) / 85);
                                CDTXMania.Tx.Effects_Rainbow.t2D描画(CDTXMania.app.Device, 360 + nRectX, -100, new Rectangle(nRectX, 0, 920 - nRectX, 410));
                            }

                        }

                    }
                }
                for (int f = 0; f < 2; f++)
                {
                    if (this.st虹2[f].b使用中)
                    {
                        this.st虹2[f].ct進行.t進行();
                        if (this.st虹2[f].ct進行.b終了値に達した)
                        {
                            this.st虹2[f].ct進行.t停止();
                            this.st虹2[f].b使用中 = false;
                        }

                        if(CDTXMania.Tx.Effects_Rainbow != null && this.st虹2[f].nPlayer == 1 ) //画像が出来るまで
                        {
                            //this.st虹[f].ct進行.n現在の値 = 164;

                            if (this.st虹2[f].ct進行.n現在の値 < 82)
                            {
                                int nRectX = ((this.st虹2[f].ct進行.n現在の値 * 920) / 85);
                                CDTXMania.Tx.Effects_Rainbow.t2D上下反転描画(CDTXMania.app.Device, 360, 410, new Rectangle(0, 0, nRectX, 410));
                            }
                            else if (this.st虹2[f].ct進行.n現在の値 >= 82)
                            {
                                int nRectX = (((this.st虹2[f].ct進行.n現在の値 - 82) * 920) / 85);
                                CDTXMania.Tx.Effects_Rainbow.t2D上下反転描画(CDTXMania.app.Device, 360 + nRectX, 410, new Rectangle(nRectX, 0, 920 - nRectX, 410));
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

        [StructLayout(LayoutKind.Sequential)]
        private struct ST虹
        {
            public bool b使用中;
            public int nPlayer;
            public CCounter ct進行;
            public float fX;
        }

        private ST虹[] st虹 = new ST虹[2];
        private ST虹[] st虹2 = new ST虹[2];

		//-----------------
		#endregion
	}
}
