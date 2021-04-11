﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.Runtime.InteropServices;
using SlimDX;
using FDK;

namespace DTXMania
{
    internal class CAct演奏Drums風船 : CActivity
    {
        // ToDo:WAM導入して簡易アニメ作成

        public CAct演奏Drums風船()
        {
            ST文字位置[] st文字位置Array = new ST文字位置[ 11 ];

			ST文字位置 st文字位置 = new ST文字位置();
			st文字位置.ch = '0';
			st文字位置.pt = new Point( 0, 0 );
			st文字位置Array[ 0 ] = st文字位置;
			ST文字位置 st文字位置2 = new ST文字位置();
			st文字位置2.ch = '1';
			st文字位置2.pt = new Point( 62, 0 );
			st文字位置Array[ 1 ] = st文字位置2;
			ST文字位置 st文字位置3 = new ST文字位置();
			st文字位置3.ch = '2';
			st文字位置3.pt = new Point( 124, 0 );
			st文字位置Array[ 2 ] = st文字位置3;
			ST文字位置 st文字位置4 = new ST文字位置();
			st文字位置4.ch = '3';
			st文字位置4.pt = new Point( 186, 0 );
			st文字位置Array[ 3 ] = st文字位置4;
			ST文字位置 st文字位置5 = new ST文字位置();
			st文字位置5.ch = '4';
			st文字位置5.pt = new Point( 248, 0 );
			st文字位置Array[ 4 ] = st文字位置5;
			ST文字位置 st文字位置6 = new ST文字位置();
			st文字位置6.ch = '5';
			st文字位置6.pt = new Point( 310, 0 );
			st文字位置Array[ 5 ] = st文字位置6;
			ST文字位置 st文字位置7 = new ST文字位置();
			st文字位置7.ch = '6';
			st文字位置7.pt = new Point( 372, 0 );
			st文字位置Array[ 6 ] = st文字位置7;
			ST文字位置 st文字位置8 = new ST文字位置();
			st文字位置8.ch = '7';
			st文字位置8.pt = new Point( 434, 0 );
			st文字位置Array[ 7 ] = st文字位置8;
			ST文字位置 st文字位置9 = new ST文字位置();
			st文字位置9.ch = '8';
			st文字位置9.pt = new Point( 496, 0 );
			st文字位置Array[ 8 ] = st文字位置9;
			ST文字位置 st文字位置10 = new ST文字位置();
			st文字位置10.ch = '9';
			st文字位置10.pt = new Point( 558, 0 );
			st文字位置Array[ 9 ] = st文字位置10;

			this.st文字位置 = st文字位置Array;

			base.b活性化してない = true;

        }

        public override void On活性化()
        {
            this.ct風船終了 = new CCounter();
            this.ct風船ふきだしアニメ = new CCounter();
            base.On活性化();
        }

        public override void On非活性化()
        {
            this.ct風船終了 = null;
            this.ct風船ふきだしアニメ = null;
            base.On非活性化();
        }

        public override void OnManagedリソースの作成()
        {
            if( !this.b活性化してない )
            {
                this.tx連打枠 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_balloon.png" ) );
                this.tx連打数字 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_RollNumber.png" ) );

                this.txキャラクター = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\Chara\balloon.png" ) );
                this.txキャラクター_風船終了 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\Chara\balloon_break_0.png" ) );

                for( int i = 0; i < 6; i++ )
                {
                    this.tx風船枠[ i ] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\Chara\balloon_" + i.ToString() +".png" ) );
                }

                this.ct風船ふきだしアニメ = new CCounter( 0, 1, 100, CDTXMania.Timer );
                base.OnManagedリソースの作成();
            }
        }

        public override void OnManagedリソースの解放()
        {
            if( !this.b活性化してない )
            {
                CDTXMania.tテクスチャの解放( ref this.tx連打枠 );
                CDTXMania.tテクスチャの解放( ref this.tx連打数字 );

                CDTXMania.tテクスチャの解放( ref this.txキャラクター );
                CDTXMania.tテクスチャの解放( ref this.txキャラクター_風船終了 );

                for( int i = 0; i < 6; i++ )
                {
                    CDTXMania.tテクスチャの解放( ref this.tx風船枠[ i ] );
                }

                base.OnManagedリソースの解放();
            }
        }

        public override int On進行描画()
        {
            return base.On進行描画();
        }

        public int On進行描画( int n連打ノルマ, int n連打数, int player )
        {
            this.ct風船ふきだしアニメ.t進行Loop();

            //CDTXMania.act文字コンソール.tPrint( 0, 16, C文字コンソール.Eフォント種別.赤, this.ct風船終了.n現在の値.ToString() );
            int[] n残り打数 = new int[] { 0, 0, 0, 0, 0 };
            #region[ 風船の画像を変える打数を風船の残り打数から算出 ]
            if ( n連打ノルマ > 0 )
            {
                if( n連打ノルマ < 5 )
                {
                    n残り打数 = new int[] { 99, 99, 99, 99, 99 };
                }
                else
                {
                    n残り打数[ 0 ] = ( n連打ノルマ / 5 ) * 4;
                    n残り打数[ 1 ] = ( n連打ノルマ / 5 ) * 3;
                    n残り打数[ 2 ] = ( n連打ノルマ / 5 ) * 2;
                    n残り打数[ 3 ] = ( n連打ノルマ / 5 ) * 1;
                }
            }
            #endregion

                if( n連打数 != 0 )
                {
                    //1P:0 2P:245
                    if( this.txキャラクター != null )
                        this.txキャラクター.t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nPlayerCharacterBalloonX[ player ], CDTXMania.Skin.nPlayerCharacterBalloonY[ player ] );
                    for( int j = 0; j < 5; j++ )
                    {
                        if( n残り打数[ j ] < n連打数 )
                        {
                            if( this.tx風船枠[ j ] != null )
                                this.tx風船枠[ j ].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nBurstBalloonX[ player ] + ( this.ct風船ふきだしアニメ.n現在の値 == 1 ? 3 : 0 ), CDTXMania.Skin.nBurstBalloonY[ player ] );
                            break;
                        }
                    }
                    //1P:31 2P:329
                    if( this.tx連打枠 != null )
                        this.tx連打枠.t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nBurstFrameX[ player ], CDTXMania.Skin.nBurstFrameY[ player ] );
                    this.t文字表示( CDTXMania.Skin.nBurstNumberX[ player ], CDTXMania.Skin.nBurstNumberY[ player ], n連打数.ToString(), n連打数 );
                    //CDTXMania.act文字コンソール.tPrint( 0, 0, C文字コンソール.Eフォント種別.白, n連打数.ToString() );
                }
                if( n連打数 == 0 && CDTXMania.stage演奏ドラム画面.actChara.b風船連打中 )
                {
                    //this.ct風船終了.n現在の値 = 0;
                    if( !this.ct風船終了.b停止中 )
                    {
                        this.ct風船終了.t進行();
                        if( this.ct風船終了.b終了値に達した )
                        {
                            this.ct風船終了.t停止();
                        }
                    }
                    else if( this.ct風船終了.b進行中 )
                    {
                        int nY = 0;
                        int nT = 255;


                        if( this.ct風船終了.n現在の値 <= 100 )
                        {
                            nY = this.ct風船終了.n現在の値;
                        }
                        else if( this.ct風船終了.n現在の値 > 100 )
                        {
                            nY = 100;
                        }
                        //else if( this.ct風船終了.n現在の値 > 800 )
                        {
                            //nY = 100;
                            //nT = 0;
                        }

                        if( this.txキャラクター != null )
                            this.txキャラクター_風船終了.t2D描画( CDTXMania.app.Device, 240, 140 - nY );
                        //this.txキャラクター_風船終了.n透明度 = nT;
                        //CDTXMania.act文字コンソール.tPrint( 0, 0, C文字コンソール.Eフォント種別.赤, this.ct風船終了.n現在の値.ToString() );
                    
                        if( this.ct風船終了.b終了値に達した )
                        {
                            CDTXMania.stage演奏ドラム画面.actChara.b風船連打中 = false;
                            CDTXMania.stage演奏ドラム画面.b連打中[ player ] = false;
                        }
                    }
                    CDTXMania.stage演奏ドラム画面.actChara.b風船連打中 = false;
 
                
                }




            return base.On進行描画();
        }



        private CTexture tx連打枠;
        private CTexture tx連打数字;
        private readonly ST文字位置[] st文字位置;

        private CTexture txキャラクター;
        private CTexture txキャラクター_風船終了;

        private CTexture[] tx風船枠 = new CTexture[ 6 ];

        private CCounter ct風船終了;
        private CCounter ct風船ふきだしアニメ;

        [StructLayout(LayoutKind.Sequential)]
        private struct ST文字位置
        {
            public char ch;
            public Point pt;
        }

        private void t文字表示( int x, int y, string str, int n連打 )
		{
            int n桁数 = n連打.ToString().Length;
			foreach( char ch in str )
			{
				for( int i = 0; i < this.st文字位置.Length; i++ )
				{
					if( this.st文字位置[ i ].ch == ch )
					{
						Rectangle rectangle = new Rectangle( this.st文字位置[ i ].pt.X, this.st文字位置[ i ].pt.Y, 62, 80 );

						if( this.tx連打数字 != null )
						{
							this.tx連打数字.t2D描画( CDTXMania.app.Device, x - ( ( 62 * n桁数 ) / 2 ), y, rectangle );
						}
						break;
					}
				}
				x += ( 60 - ( n桁数 > 2 ? n桁数 * 2 : 0 ) );
			}
		}

        public void tEnd()
        {
            this.ct風船終了 = new CCounter(0, 800, 1, CDTXMania.Timer);
        }
    }
}
