using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using FDK;

namespace DTXMania
{
    internal class CAct演奏Drumsレーン : CActivity
    {
        public CAct演奏Drumsレーン()
        {
            base.b活性化してない = true;
        }

        public override void On活性化()
        {
            base.On活性化();
        }

        public override void On非活性化()
        {
            CDTXMania.t安全にDisposeする( ref this.ct分岐アニメ進行 );
            base.On非活性化();
        }

        public override void OnManagedリソースの作成()
        {
            //this.tx普通譜面[ 0 ] = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\7_field_normal_base.png"));
            //this.tx玄人譜面[ 0 ] = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\7_field_expert_base.png"));
            //this.tx達人譜面[ 0 ] = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\7_field_master_base.png"));
            //this.tx普通譜面[ 1 ] = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\7_field_normal.png"));
            //this.tx玄人譜面[ 1 ] = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\7_field_expert.png"));
            //this.tx達人譜面[ 1 ] = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\7_field_master.png"));
            this.ct分岐アニメ進行 = new CCounter[ 4 ];
            this.nBefore = new int[ 4 ];
            this.nAfter = new int[ 4 ];
            for( int i = 0; i < CDTXMania.ConfigIni.nPlayerCount; i++ )
            {
                this.ct分岐アニメ進行[ i ] = new CCounter();
                this.nBefore[ i ] = 0;
                this.nAfter[ i ] = 0;
                this.bState[ i ] = false;
            }
            CDTXMania.Tx.Lane_Base[0].n透明度 = 255;

            base.OnManagedリソースの作成();
        }

        public override void OnManagedリソースの解放()
        {
            //CDTXMania.tテクスチャの解放( ref this.tx普通譜面[ 0 ] );
            //CDTXMania.tテクスチャの解放( ref this.tx玄人譜面[ 0 ] );
            //CDTXMania.tテクスチャの解放( ref this.tx達人譜面[ 0 ] );
            //CDTXMania.tテクスチャの解放( ref this.tx普通譜面[ 1 ] );
            //CDTXMania.tテクスチャの解放( ref this.tx玄人譜面[ 1 ] );
            //CDTXMania.tテクスチャの解放( ref this.tx達人譜面[ 1 ] );

            base.OnManagedリソースの解放();
        }

        public override int On進行描画()
        {
            for( int i = 0; i < CDTXMania.ConfigIni.nPlayerCount; i++ )
            {
                if( !this.ct分岐アニメ進行[ i ].b停止中 )
                {
                    this.ct分岐アニメ進行[ i ].t進行();
                    if( this.ct分岐アニメ進行[ i ].b終了値に達した )
                    {
                        this.bState[ i ] = false;
                        this.ct分岐アニメ進行[ i ].t停止();
                    }
                }
            }


            //アニメーション中の分岐レイヤー(背景)の描画を行う。
            for( int i = 0; i < CDTXMania.ConfigIni.nPlayerCount; i++ )
            {
                if( CDTXMania.stage演奏ドラム画面.bUseBranch[ i ] == true )
                {
                    if( this.ct分岐アニメ進行[ i ].b進行中 )
                    {
                        #region[ 普通譜面_レベルアップ ]
                        //普通→玄人
                        if( nBefore[ i ] == 0 && nAfter[ i ] == 1 )
                        {
                            CDTXMania.Tx.Lane_Base[1].n透明度 = this.ct分岐アニメ進行[ i ].n現在の値 > 100 ? 255 : ( ( this.ct分岐アニメ進行[ i ].n現在の値 * 0xff ) / 100 );
                            CDTXMania.Tx.Lane_Base[0].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] );
                            CDTXMania.Tx.Lane_Base[1].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] );
                        }
                        //普通→達人
                        if( nBefore[ i ] == 0 && nAfter[ i ] == 2)
                        {
                            CDTXMania.Tx.Lane_Base[0].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] );
                            if( this.ct分岐アニメ進行[ i ].n現在の値 < 100 )
                            {
                                CDTXMania.Tx.Lane_Base[1].n透明度 = this.ct分岐アニメ進行[ i ].n現在の値 > 100 ? 255 : ( ( this.ct分岐アニメ進行[ i ].n現在の値 * 0xff ) / 100 );
                                CDTXMania.Tx.Lane_Base[1].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] );
                            }
                            else if( this.ct分岐アニメ進行[ i ].n現在の値 >= 100 && this.ct分岐アニメ進行[ i ].n現在の値 < 150 )
                            {
                                CDTXMania.Tx.Lane_Base[1].n透明度 = 255;
                                CDTXMania.Tx.Lane_Base[1].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] );
                            }
                            else if( this.ct分岐アニメ進行[ i ].n現在の値 >= 150 )
                            {
                                CDTXMania.Tx.Lane_Base[1].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] );
                                CDTXMania.Tx.Lane_Base[2].n透明度 = this.ct分岐アニメ進行[ i ].n現在の値 > 250 ? 255 : ( ( (this.ct分岐アニメ進行[ i ].n現在の値 - 150) * 0xff ) / 100 );
                                CDTXMania.Tx.Lane_Base[2].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] );
                            }
                        }
                        #endregion
                        #region[ 玄人譜面_レベルアップ ]
                        if( nBefore[ i ] == 1 && nAfter[ i ] == 2 )
                        {
                            CDTXMania.Tx.Lane_Base[1].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] );
                            CDTXMania.Tx.Lane_Base[2].n透明度 = this.ct分岐アニメ進行[ i ].n現在の値 > 100 ? 255 : ( ( this.ct分岐アニメ進行[ i ].n現在の値 * 0xff ) / 100 );
                            CDTXMania.Tx.Lane_Base[2].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] );
                        }
                        #endregion
                        #region[ 玄人譜面_レベルダウン ]
                        if( nBefore[ i ] == 1 && nAfter[ i ] == 0)
                        {
                            CDTXMania.Tx.Lane_Base[1].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] );
                            CDTXMania.Tx.Lane_Base[0].n透明度 = this.ct分岐アニメ進行[ i ].n現在の値 > 100 ? 255 : ( ( this.ct分岐アニメ進行[ i ].n現在の値 * 0xff ) / 100 );
                            CDTXMania.Tx.Lane_Base[0].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] );
                        }
                        #endregion
                        #region[ 達人譜面_レベルダウン ]
                        if( nBefore[ i ] == 2 && nAfter[ i ] == 0)
                        {
                            CDTXMania.Tx.Lane_Base[2].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] );
                            if( this.ct分岐アニメ進行[ i ].n現在の値 < 100 )
                            {
                                CDTXMania.Tx.Lane_Base[1].n透明度 = this.ct分岐アニメ進行[ i ].n現在の値 > 100 ? 255 : ( ( this.ct分岐アニメ進行[ i ].n現在の値 * 0xff ) / 100 );
                                CDTXMania.Tx.Lane_Base[1].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] );
                            }
                            else if( this.ct分岐アニメ進行[ i ].n現在の値 >= 100 && this.ct分岐アニメ進行[ i ].n現在の値 < 150 )
                            {
                                CDTXMania.Tx.Lane_Base[1].n透明度 = 255;
                                CDTXMania.Tx.Lane_Base[1].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] );
                            }
                            else if( this.ct分岐アニメ進行[ i ].n現在の値 >= 150 )
                            {
                                CDTXMania.Tx.Lane_Base[1].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] );
                                CDTXMania.Tx.Lane_Base[0].n透明度 = this.ct分岐アニメ進行[ i ].n現在の値 > 250 ? 255 : ( ( ( this.ct分岐アニメ進行[ i ].n現在の値 - 150 ) * 0xff ) / 100 );
                                CDTXMania.Tx.Lane_Base[0].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] );
                            }
                        }
                        if( nBefore[ i ] == 2 && nAfter[ i ] == 1 )
                        {
                            CDTXMania.Tx.Lane_Base[2].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] );
                            CDTXMania.Tx.Lane_Base[2].n透明度 = this.ct分岐アニメ進行[ i ].n現在の値 > 100 ? 255 : ( ( this.ct分岐アニメ進行[ i ].n現在の値 * 0xff ) / 100 );
                            CDTXMania.Tx.Lane_Base[2].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] );
                        }
                        #endregion
                    }
                }
            }
            return base.On進行描画();
        }

        public virtual void t分岐レイヤー_コース変化( int n現在, int n次回, int player )
        {
            if( n現在 == n次回 ) {
                return;
            }
            this.ct分岐アニメ進行[ player ] = new CCounter( 0, 300, 2, CDTXMania.Timer );
            this.bState[ player ] = true;

            this.nBefore[ player ] = n現在;
            this.nAfter[ player ] = n次回;

        }

        #region[ private ]
        //-----------------
        public bool[] bState = new bool[4];
        public CCounter[] ct分岐アニメ進行 = new CCounter[4];
        private int[] nBefore;
        private int[] nAfter;
        private int[] n透明度 = new int[4];
        //private CTexture[] tx普通譜面 = new CTexture[2];
        //private CTexture[] tx玄人譜面 = new CTexture[2];
        //private CTexture[] tx達人譜面 = new CTexture[2];
        //-----------------
        #endregion
    }
}
