using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using FDK;
using SharpDX.Animation;

namespace DTXMania
{
	internal class CAct演奏Drumsスコア : CAct演奏スコア共通
	{
		// CActivity 実装（共通クラスからの差分のみ）

		public unsafe override int On進行描画()
        {
            if (!base.b活性化してない)
            {
                if (base.b初めての進行描画)
                {
                    base.b初めての進行描画 = false;
                }
                long num = FDK.CSound管理.rc演奏用タイマ.n現在時刻;
                //if (num < base.n進行用タイマ)
                //{
                //    base.n進行用タイマ = num;
                //}
                //while ((num - base.n進行用タイマ) >= 10)
                //{
                //    for (int j = 0; j < 4; j++)
                //    {
                //        this.n現在表示中のスコア[j] += this.nスコアの増分[j];

                //        if (this.n現在表示中のスコア[j] > (long) this.n現在の本当のスコア[j])
                //            this.n現在表示中のスコア[j] = (long) this.n現在の本当のスコア[j];
                //    }
                //    base.n進行用タイマ += 10;

                //}
                if( !this.ctTimer.b停止中 )
                {
                    this.ctTimer.t進行();
                    if( this.ctTimer.b終了値に達した )
                    {
                        this.ctTimer.t停止();
                    }

                    //base.t小文字表示( 20, 150, string.Format( "{0,7:######0}", this.nスコアの増分.Guitar ) );
                }

                base.t小文字表示( 20, 190, string.Format( "{0,7:######0}", this.n現在表示中のスコア[ 0 ].Taiko ), 0 );
                if( CDTXMania.stage演奏ドラム画面.bDoublePlay ) base.t小文字表示( 20, CDTXMania.Skin.nScoreY[ 1 ], string.Format( "{0,7:######0}", this.n現在表示中のスコア[ 1 ].Taiko ), 0 );

                for( int i = 0; i < 256; i++ )
                {
                    if( this.stScore[ i ].b使用中 )
                    {
                        if( !this.stScore[ i ].ctTimer.b停止中 )
                        {
                            this.stScore[ i ].ctTimer.t進行();
                            if( this.stScore[ i ].ctTimer.b終了値に達した )
                            {
                                this.n現在表示中のスコア[ this.stScore[ i ].nPlayer ].Taiko += (long)this.stScore[ i ].nAddScore;
                                if( this.stScore[ i ].b表示中 == true )
                                    this.n現在表示中のAddScore--;
                                this.stScore[ i ].ctTimer.t停止();
                                this.stScore[ i ].b使用中 = false;
                            }

                            int xAdd = 0;
                            int yAdd = 0;

                            if( this.stScore[ i ].ctTimer.n現在の値 < 50 )
                            {
                                xAdd = 30 - this.stScore[i].ctTimer.n現在の値;
                            }
                            else
                            {
                                xAdd = 0;
                            }
                            if( this.stScore[ i ].ctTimer.n現在の値 >= 460 )
                            {
                                yAdd = 500 - this.stScore[i].ctTimer.n現在の値;
                            }

                            if( this.n現在表示中のAddScore < 10 && this.stScore[ i ].bBonusScore == false )
                                base.t小文字表示( 20 + xAdd, this.stScore[ i ].nPlayer == 0 ? CDTXMania.Skin.nScoreAddY[ this.stScore[ i ].nPlayer ] + yAdd : CDTXMania.Skin.nScoreAddY[ this.stScore[ i ].nPlayer ] - yAdd, string.Format( "{0,7:######0}", this.stScore[ i ].nAddScore ), this.stScore[ i ].nPlayer + 1 );
                            if( this.n現在表示中のAddScore < 10 && this.stScore[ i ].bBonusScore == true )
                                base.t小文字表示( 20 + xAdd, CDTXMania.Skin.nScoreAddBonusY[ this.stScore[ i ].nPlayer ], string.Format( "{0,7:######0}", this.stScore[ i ].nAddScore ), this.stScore[ i ].nPlayer + 1 );
                            else
                            {
                                this.n現在表示中のAddScore--;
                                this.stScore[ i ].b表示中 = false;
                            }
                        }
                    }
                }


                //this.n現在表示中のスコア.Taiko = (long)this.n現在の本当のスコア.Taiko;

                //string str = this.n現在表示中のスコア.Taiko.ToString( "0000000" );
                //for ( int i = 0; i < 7; i++ )
                //{
                //    Rectangle rectangle;
                //    char ch = str[i];
                //    if( ch.Equals(' ') )
                //    {
                //        rectangle = new Rectangle(0, 0, 24, 34);
                //    }
                //    else
                //    {
                //        int num4 = int.Parse(str.Substring(i, 1));
                //        rectangle = new Rectangle(num4 * 24, 0, 24, 34);
                //    }
                //    if( base.txScore != null )
                //    {
                //        base.txScore.t2D描画(CDTXMania.app.Device, 20 + (i * 20), 192, rectangle);
                //    }
                //}


                //CDTXMania.act文字コンソール.tPrint( 50, 200, C文字コンソール.Eフォント種別.白, str  );
            }
            return 0;
        }
    }
}
