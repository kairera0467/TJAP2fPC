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
		
		
		// メソッド
        public virtual void Start( int nLane, int nPlayer )
		{
            if (CDTXMania.Tx.Notes != null)
            {
                for (int i = 0; i < 1; i++)
                {
                    for (int j = 0; j < 64; j++)
                    {
                        if (!this.st飛び散るチップ[j].b使用中)
                        {
                            this.st飛び散るチップ[j].b使用中 = true;
                            int n回転初期値 = 1;
                            double num7 = 1.115 + (1 / 100.0); // 拡散の大きさ
                            this.st飛び散るチップ[j].nLane = (int)nLane;
                            this.st飛び散るチップ[j].nPlayer = nPlayer;
                            this.st飛び散るチップ[j].ct進行 = new CCounter(0, 323/4, 5, CDTXMania.Timer); // カウンタ

                            this.st飛び散るチップ[j].fXL = (414 - 655) + (130 / 2); //X座標
                            this.st飛び散るチップ[j].fXR = (414 - 655) + (130 / 2); //X座標

                            this.st飛び散るチップ[j].fY = nPlayer == 0 ? (257 - 360) : ( 470 - 360 );
                            this.st飛び散るチップ[j].f加速度X = (float)(num7 * Math.Cos((Math.PI * 2 * n回転初期値) / 360.0) + 2.53);
                            this.st飛び散るチップ[j].f加速度Y = (float)(num7 * (Math.Sin((Math.PI * 2 * n回転初期値) / 360.0) - 1.3));
                            this.st飛び散るチップ[j].f加速度の加速度X = 1.0027f;
                            this.st飛び散るチップ[j].f加速度の加速度Y = 1.005f;
                            this.st飛び散るチップ[j].f重力加速度 = 0.0347f;
                            this.st飛び散るチップ[j].f半径 = (float)(0.6 + (((double)CDTXMania.Random.Next(30)) / 100.0));
                            break;
                        }

                        //if( !this.st飛んで行く音符[j].b使用中 )
                        //{
                        //    this.st飛んで行く音符[j].b使用中 = true;

                        //    this.st飛んで行く音符[j].nLane = (int)nLane;
                        //    this.st飛んで行く音符[j].ct進行 = new CCounter(0, 49, 12, CDTXMania.Timer); // カウンタ

                        //    this.st飛んで行く音符[j].fXL = CDTXMania.Skin.nScrollFieldX; //X座標
                        //    this.st飛んで行く音符[j].fY = 257 - 65;

                        //    int n頂点 = -18;
                        //    this.st飛んで行く音符[j].f加速度X = (float)( ( 1224 - CDTXMania.Skin.nScrollFieldX ) / 50.0f );
                        //    this.st飛んで行く音符[j].f加速度Y = (float)( ( 275 ) / ( 53.0f / 2.0f ) );// JLinePosY = 257
                        //    break;
                        //}
                    }
                }
            }
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
                        this.st虹[i].ct進行 = new CCounter(0, 164, 5, CDTXMania.Timer); // カウンタ
                        this.st虹[i].nPlayer = player;
                        break;
                    }
                    if (!this.st虹2[i].b使用中 && player == 1)
                    {
                        this.st虹2[i].b使用中 = true;
                        this.st虹2[i].ct進行 = new CCounter(0, 164, 5, CDTXMania.Timer); // カウンタ
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
				this.st状態[ i ].ct進行 = new CCounter();
				this.st虹[ i ].ct進行 = new CCounter();
				this.st虹2[ i ].ct進行 = new CCounter();
			}
            for (int i = 0; i < 64; i++)
            {
                this.st飛び散るチップ[i] = new ST飛び散るチップ();
                this.st飛び散るチップ[i].b使用中 = false;
                this.st飛び散るチップ[i].ct進行 = new CCounter();

                this.st飛んで行く音符[i] = new ST飛び散るチップ();
                this.st飛んで行く音符[i].b使用中 = false;
                this.st飛んで行く音符[i].ct進行 = new CCounter();
            }
            base.On活性化();
		}
		public override void On非活性化()
		{
            for( int i = 0; i < 2; i++ )
			{
				this.st状態[ i ].ct進行 = null;
				this.st虹[ i ].ct進行 = null;
				this.st虹2[ i ].ct進行 = null;
			}

            for (int i = 0; i < 64; i++)
            {
                this.st飛び散るチップ[i].ct進行 = null;

                this.st飛んで行く音符[i].ct進行 = null;
            }
			base.On非活性化();
		}
		public override void OnManagedリソースの作成()
		{
			if( !base.b活性化してない )
			{
                //this.tx音符 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_taiko_notes.png" ) );
                //this.tx虹 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_balloon_rainbow.png" ) );
				base.OnManagedリソースの作成();
			}
		}
		public override void OnManagedリソースの解放()
		{
			if( !base.b活性化してない )
			{
				//CDTXMania.tテクスチャの解放( ref this.tx音符 );
    //            CDTXMania.tテクスチャの解放( ref this.tx虹 );

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

                for (int i = 0; i < 64; i++)
                {
                    if( CDTXMania.Skin.nScrollFieldX[0] > 414 + 4 )
                        break;
                    if( CDTXMania.Skin.nScrollFieldX[0] < 414 - 4 )
                        break;

                    if (this.st飛び散るチップ[i].b使用中)
                    {
                        this.st飛び散るチップ[i].n前回のValue = this.st飛び散るチップ[i].ct進行.n現在の値;
                        this.st飛び散るチップ[i].ct進行.t進行();
                        if (this.st飛び散るチップ[i].ct進行.b終了値に達した)
                        {
                            this.st飛び散るチップ[i].ct進行.t停止();
                            this.st飛び散るチップ[i].b使用中 = false;
                        }
                        for (int n = this.st飛び散るチップ[i].n前回のValue; n < this.st飛び散るチップ[i].ct進行.n現在の値; n++)
                        {
                            if( this.st飛び散るチップ[i].nPlayer == 0 )
                            {
                                this.st飛び散るチップ[i].fXL += (float)((this.st飛び散るチップ[i].f加速度X * Math.Cos((120.0 * Math.PI / 180.0))) * 5);
                                this.st飛び散るチップ[i].fXR += (float)((this.st飛び散るチップ[i].f加速度X * Math.Cos((60.0 * Math.PI / 180.0))) * 5);

                                this.st飛び散るチップ[i].fY += (float)((this.st飛び散るチップ[i].f加速度Y * Math.Sin((60.0 * Math.PI / 180.0))) * 9.40f - Math.Exp(this.st飛び散るチップ[i].f重力加速度 * 2.0f) / 2.0f);
                                this.st飛び散るチップ[i].f加速度X *= this.st飛び散るチップ[i].f加速度の加速度X;
                                //this.st飛び散るチップ[i].fY *= this.st飛び散るチップ[i].f加速度Y;
                                this.st飛び散るチップ[i].f加速度Y += this.st飛び散るチップ[i].f重力加速度;
                            }
                            else if( this.st飛び散るチップ[i].nPlayer == 1 )
                            {
                                this.st飛び散るチップ[i].fXL += (float)((this.st飛び散るチップ[i].f加速度X * Math.Cos((120.0 * Math.PI / 180.0))) * 5);
                                this.st飛び散るチップ[i].fXR += (float)((this.st飛び散るチップ[i].f加速度X * Math.Cos((60.0 * Math.PI / 180.0))) * 5);

                                this.st飛び散るチップ[i].fY -= (float)((this.st飛び散るチップ[i].f加速度Y * Math.Sin((60.0 * Math.PI / 180.0))) * 9.40f - Math.Exp(this.st飛び散るチップ[i].f重力加速度 * 2.0f) / 2.0f);
                                this.st飛び散るチップ[i].f加速度X *= this.st飛び散るチップ[i].f加速度の加速度X;
                                //this.st飛び散るチップ[i].fY *= this.st飛び散るチップ[i].f加速度Y;
                                this.st飛び散るチップ[i].f加速度Y += this.st飛び散るチップ[i].f重力加速度;
                            }
                        }

                        //Matrix mat = Matrix.Identity;
                        Matrix mat2 = Matrix.Identity;

                        //mat *= Matrix.RotationZ(0.09f * this.st飛び散るチップ[i].ct進行.n現在の値);


                        //mat *= Matrix.Translation((this.st飛び散るチップ[i].fXL - 50f), -(this.st飛び散るチップ[i].fY + 257), 0f);
                        mat2 *= Matrix.Translation((this.st飛び散るチップ[i].fXR - 50f), -(this.st飛び散るチップ[i].fY + 0), 0f);

                        if (CDTXMania.Tx.Notes != null)
                        {
                            if( this.st飛び散るチップ[i].nLane == 0xA || this.st飛び散るチップ[i].nLane == 0xB )
                            {
                                CDTXMania.Tx.Notes.t3D描画(CDTXMania.app.Device, mat2, new Rectangle( ( this.st飛び散るチップ[i].nLane + 3 ) * 130, 0, 130, 130));
                            }
                            else
                            {
                                CDTXMania.Tx.Notes.t3D描画(CDTXMania.app.Device, mat2, new Rectangle( this.st飛び散るチップ[i].nLane * 130, 0, 130, 130));
                            }
                        }

                        if( this.st飛び散るチップ[ i ].ct進行.b終了値に達した )
                        {
                            CDTXMania.stage演奏ドラム画面.actGauge.Start( this.st飛び散るチップ[i].nLane, E判定.Perfect, this.st飛び散るチップ[i].nPlayer );
                            CDTXMania.stage演奏ドラム画面.actChipEffects.Start(st飛び散るチップ[i].nPlayer, st飛び散るチップ[i].nLane);
                        }
                    }


                    if (this.st飛んで行く音符[i].b使用中)
                    {
                        this.st飛んで行く音符[i].n前回のValue = this.st飛んで行く音符[i].ct進行.n現在の値;
                        this.st飛んで行く音符[i].ct進行.t進行();
                        if( this.st飛んで行く音符[i].ct進行.b終了値に達した )
                        {
                            this.st飛んで行く音符[i].ct進行.t停止();
                            this.st飛んで行く音符[i].b使用中 = false;
                        }
                        for (int n = this.st飛んで行く音符[i].n前回のValue; n < this.st飛んで行く音符[i].ct進行.n現在の値; n++)
                        {
                            this.st飛んで行く音符[ i ].fXL += this.st飛んで行く音符[i].f加速度X;
                            //if( this.st飛んで行く音符[ i ].ct進行.n現在の値 <= 17 )
                                this.st飛んで行く音符[ i ].fY -= this.st飛んで行く音符[ i ].f加速度Y;
                            //else
                                //this.st飛んで行く音符[ i ].fY += this.st飛んで行く音符[ i ].f加速度Y;
                        }

                        Matrix mat = Matrix.Identity;
                        mat *= Matrix.Translation( this.st飛んで行く音符[ i ].fXL - 640, -(this.st飛んで行く音符[i].fY - 320), 0f);
                        //mat *= Matrix.Translation( this.st飛んで行く音符[ i ].fXL - 640, 0, 0f);

                        if (CDTXMania.Tx.Notes != null)
                        {
                            //this.tx音符.t3D描画(CDTXMania.app.Device, mat, new Rectangle( this.st飛んで行く音符[i].nLane * 130, 0, 130, 130));
                        }

                        //if( this.st飛んで行く音符[ i ].ct進行.b終了値に達した )
                        {
                            //CDTXMania.stage演奏ドラム画面.actGauge.Start( this.st飛んで行く音符[i].nLane, E判定.Perfect );
                        }
                    }
                }
			}
			return 0;
		}
		

		// その他

		#region [ private ]
		//-----------------
        //private CTexture tx音符;
        //private CTexture tx虹;
        protected STSTATUS[] st状態 = new STSTATUS[2];

        [StructLayout(LayoutKind.Sequential)]
        protected struct STSTATUS
        {
            public CCounter ct進行;
            public E判定 judge;
            public int nIsBig;
            public int n透明度;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct ST飛び散るチップ
        {
            public int nLane;
            public int nPlayer;
            public bool b使用中;
            public CCounter ct進行;
            public int n前回のValue;
            public float fXL;
            public float fXR;
            public float fY;
            public float fチップの質量;
            public float f初速度X;
            public float f初速度Y;
            public float f加速度X;
            public float f加速度Y;
            public float f加速度の加速度X;
            public float f加速度の加速度Y;
            public float f重力加速度;
            public float f半径;
            public float f回転単位;
            public float f回転方向;
        }

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
        private ST飛び散るチップ[] st飛び散るチップ = new ST飛び散るチップ[64];
        private ST飛び散るチップ[] st飛んで行く音符 = new ST飛び散るチップ[64]; //2016.08.30 kairera0467 新メソッド

		//-----------------
		#endregion
	}
}
