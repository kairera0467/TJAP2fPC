using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using FDK;

namespace DTXMania
{
    internal class CAct演奏Drums演奏終了演出 : CActivity
    {
        /// <summary>
        /// 課題
        /// ・クリア失敗 →素材不足(確保はできる。切り出しと加工をしてないだけ。)
        /// ・
        /// </summary>
        public CAct演奏Drums演奏終了演出()
        {
            base.b活性化してない = true;
        }

        public void Start()
        {
            this.ct進行メイン = new CCounter( 0, 300, 22, CDTXMania.Timer );
        }

        public override void On活性化()
        {
            this.bリザルトボイス再生済み = false;
            base.On活性化();
        }

        public override void On非活性化()
        {
            this.ct進行メイン = null;
            base.On非活性化();
        }

        public override void OnManagedリソースの作成()
        {
            this.b再生済み = false;

            this.txバチお左_成功[ 0 ] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\End\Clear_L_00.png" ) );
            this.txバチお左_成功[ 1 ] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\End\Clear_L_01.png" ) );
            this.txバチお左_成功[ 2 ] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\End\Clear_L_02.png" ) );
            this.txバチお左_成功[ 3 ] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\End\Clear_L_03.png" ) );
            this.txバチお左_成功[ 4 ] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\End\Clear_L_04.png" ) );

            this.txバチお右_成功[ 0 ] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\End\Clear_R_00.png" ) );
            this.txバチお右_成功[ 1 ] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\End\Clear_R_01.png" ) );
            this.txバチお右_成功[ 2 ] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\End\Clear_R_02.png" ) );
            this.txバチお右_成功[ 3 ] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\End\Clear_R_03.png" ) );
            this.txバチお右_成功[ 4 ] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\End\Clear_R_04.png" ) );

            this.tx文字 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\End\Text.png" ) );
            this.tx文字マスク = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\End\Text_Effect.png" ) );
            if( this.tx文字マスク != null )
                this.tx文字マスク.b加算合成 = true;

            this.soundClear = CDTXMania.Sound管理.tサウンドを生成する( CSkin.Path( @"Sounds\Clear.ogg" ) );
            base.OnManagedリソースの作成();
        }

        public override void OnManagedリソースの解放()
        {
            for( int i = 0; i < 5; i++ )
            {
                CDTXMania.tテクスチャの解放( ref this.txバチお右_成功[ i ] );
                CDTXMania.tテクスチャの解放( ref this.txバチお左_成功[ i ] );
            }
            CDTXMania.tテクスチャの解放( ref this.tx文字 );
            CDTXMania.tテクスチャの解放( ref this.tx文字マスク );

            if( this.soundClear != null )
                this.soundClear.t解放する();
            base.OnManagedリソースの解放();
        }

        public override int On進行描画()
        {
            if( base.b初めての進行描画 )
			{
				base.b初めての進行描画 = false;
			}
            if( this.ct進行メイン != null && ( CDTXMania.stage演奏ドラム画面.eフェーズID == CStage.Eフェーズ.演奏_演奏終了演出 || CDTXMania.stage演奏ドラム画面.eフェーズID == CStage.Eフェーズ.演奏_STAGE_CLEAR_フェードアウト ) )
            {
                this.ct進行メイン.t進行();

                //CDTXMania.act文字コンソール.tPrint( 0, 0, C文字コンソール.Eフォント種別.灰, this.ct進行メイン.n現在の値.ToString() );

                if( CDTXMania.stage演奏ドラム画面.actGauge.db現在のゲージ値.Taiko >= 80 )
                {
                    //this.ct進行メイン.n現在の値 = 18;
                    if( this.soundClear != null && !this.b再生済み )
                    {
                        this.soundClear.t再生を開始する();
                        this.b再生済み = true;
                    }

                    #region[ 文字 ]
                    //登場アニメは20フレーム。うち最初の5フレームは半透過状態。
                    float[] f文字拡大率 = new float[] { 1.04f, 1.11f, 1.15f, 1.19f, 1.23f, 1.26f, 1.30f, 1.31f, 1.32f, 1.32f, 1.32f, 1.30f, 1.30f, 1.26f, 1.25f, 1.19f, 1.15f, 1.11f, 1.05f, 1.0f };
                    int[] n透明度 = new int[] { 43, 85, 128, 170, 213, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 };
                    if( this.ct進行メイン.n現在の値 >= 17 )
                    {
                        if( this.ct進行メイン.n現在の値 <= 36 )
                        {
                            this.tx文字.vc拡大縮小倍率.Y = f文字拡大率[ this.ct進行メイン.n現在の値 - 17 ];
                            this.tx文字.n透明度 = n透明度[ this.ct進行メイン.n現在の値 - 17 ];
                            this.tx文字.t2D描画( CDTXMania.app.Device, 634, (int)( 210 - ( ( 90 * f文字拡大率[ this.ct進行メイン.n現在の値 - 17 ] ) - 90 ) ), new Rectangle( 0, 0, 90, 90 ) );
                        }
                        else
                        {
                            this.tx文字.vc拡大縮小倍率.Y = 1.0f;
                            this.tx文字.t2D描画( CDTXMania.app.Device, 634, 210, new Rectangle( 0, 0, 90, 90 ) );
                        }
                    }
                    if( this.ct進行メイン.n現在の値 >= 19 )
                    {
                        if( this.ct進行メイン.n現在の値 <= 38 )
                        {
                            this.tx文字.vc拡大縮小倍率.Y = f文字拡大率[ this.ct進行メイン.n現在の値 - 19 ];
                            this.tx文字.n透明度 = n透明度[ this.ct進行メイン.n現在の値 - 19 ];
                            this.tx文字.t2D描画( CDTXMania.app.Device, 692, (int)( 210 - ( ( 90 * f文字拡大率[ this.ct進行メイン.n現在の値 - 19 ] ) - 90 ) ), new Rectangle( 90, 0, 90, 90 ) );
                        }
                        else
                        {
                            this.tx文字.vc拡大縮小倍率.Y = 1.0f;
                            this.tx文字.t2D描画( CDTXMania.app.Device, 692, 210, new Rectangle( 90, 0, 90, 90 ) );
                        }
                    }
                    this.tx文字.vc拡大縮小倍率.Y = 1.0f;
                    if( this.ct進行メイン.n現在の値 >= 21 )
                    {
                        if( this.ct進行メイン.n現在の値 <= 40 )
                        {
                            this.tx文字.vc拡大縮小倍率.Y = f文字拡大率[ this.ct進行メイン.n現在の値 - 21 ];
                            this.tx文字.n透明度 = n透明度[ this.ct進行メイン.n現在の値 - 21 ];
                            this.tx文字.t2D描画( CDTXMania.app.Device, 750, 210 - (int)( ( 90 * f文字拡大率[ this.ct進行メイン.n現在の値 - 21 ] ) - 90 ), new Rectangle( 180, 0, 90, 90 ) );
                        }
                        else
                        {
                            this.tx文字.vc拡大縮小倍率.Y = 1.0f;
                            this.tx文字.t2D描画( CDTXMania.app.Device, 750, 210, new Rectangle( 180, 0, 90, 90 ) );
                        }
                    }
                    if( this.ct進行メイン.n現在の値 >= 23 )
                    {
                        if( this.ct進行メイン.n現在の値 <= 42 )
                        {
                            this.tx文字.vc拡大縮小倍率.Y = f文字拡大率[ this.ct進行メイン.n現在の値 - 23 ];
                            this.tx文字.n透明度 = n透明度[ this.ct進行メイン.n現在の値 - 23 ];
                            this.tx文字.t2D描画( CDTXMania.app.Device, 819, 210 - (int)( ( 90 * f文字拡大率[ this.ct進行メイン.n現在の値 - 23 ] ) - 90 ), new Rectangle( 270, 0, 90, 90 ) );
                        }
                        else
                        {
                            this.tx文字.vc拡大縮小倍率.Y = 1.0f;
                            this.tx文字.t2D描画( CDTXMania.app.Device, 819, 210, new Rectangle( 270, 0, 90, 90 ) );
                        }
                    }
                    if( this.ct進行メイン.n現在の値 >= 25 )
                    {
                        if( this.ct進行メイン.n現在の値 <= 44 )
                        {
                            this.tx文字.vc拡大縮小倍率.Y = f文字拡大率[ this.ct進行メイン.n現在の値 - 25 ];
                            this.tx文字.n透明度 = n透明度[ this.ct進行メイン.n現在の値 - 25 ];
                            this.tx文字.t2D描画( CDTXMania.app.Device, 890, 212 - (int)( ( 90 * f文字拡大率[ this.ct進行メイン.n現在の値 - 25 ] ) - 90 ), new Rectangle( 360, 0, 90, 90 ) );
                        }
                        else
                        {
                            this.tx文字.vc拡大縮小倍率.Y = 1.0f;
                            this.tx文字.t2D描画( CDTXMania.app.Device, 890, 212, new Rectangle( 360, 0, 90, 90 ) );
                        }
                    }
                    if( this.ct進行メイン.n現在の値 >= 50 && this.ct進行メイン.n現在の値 < 90 )
                    {
                        if( this.ct進行メイン.n現在の値 < 70 )
                        {
                            this.tx文字マスク.n透明度 = ( this.ct進行メイン.n現在の値 - 50 ) * ( 255 / 20);
                            this.tx文字マスク.t2D描画( CDTXMania.app.Device, 634, 208 );
                        }
                        else
                        {
                            this.tx文字マスク.n透明度 = 255 - ( ( this.ct進行メイン.n現在の値 - 70 ) * ( 255 / 20) );
                            this.tx文字マスク.t2D描画( CDTXMania.app.Device, 634, 208 );
                        }
                    }
                    #endregion
                    #region[ バチお ]
                    if( this.ct進行メイン.n現在の値 <= 11 )
                    {
                        if( this.txバチお左_成功[ 1 ] != null )
                        {
                            this.txバチお左_成功[ 1 ].t2D描画( CDTXMania.app.Device, 697, 180 );
                            this.txバチお左_成功[ 1 ].n透明度 = (int)( 11.0 / this.ct進行メイン.n現在の値 ) * 255;
                        }
                        if( this.txバチお右_成功[ 1 ] != null )
                        {
                            this.txバチお右_成功[ 1 ].t2D描画( CDTXMania.app.Device, 738, 180 );
                            this.txバチお右_成功[ 1 ].n透明度 = (int)( 11.0 / this.ct進行メイン.n現在の値 ) * 255;
                        }
                    }
                    else if( this.ct進行メイン.n現在の値 <= 35 )
                    {
                        if( this.txバチお左_成功[ 0 ] != null )
                            this.txバチお左_成功[ 0 ].t2D描画( CDTXMania.app.Device, 697 - (int)( ( this.ct進行メイン.n現在の値 - 12 ) * 10 ), 180 );
                        if( this.txバチお右_成功[ 0 ] != null )
                            this.txバチお右_成功[ 0 ].t2D描画( CDTXMania.app.Device, 738 + (int)( ( this.ct進行メイン.n現在の値 - 12 ) * 10 ), 180 );
                    }
                    else if( this.ct進行メイン.n現在の値 <= 46 )
                    {
                        if( this.txバチお左_成功[ 0 ] != null )
                        {
                            //2016.07.16 kairera0467 またも原始的...
                            float[] fRet = new float[]{ 1.0f, 0.99f, 0.98f, 0.97f, 0.96f, 0.95f, 0.96f, 0.97f, 0.98f, 0.99f, 1.0f };
                            this.txバチお左_成功[ 0 ].t2D描画( CDTXMania.app.Device, 466, 180 );
                            this.txバチお左_成功[ 0 ].vc拡大縮小倍率 = new SlimDX.Vector3( fRet[ this.ct進行メイン.n現在の値 - 36 ], 1.0f, 1.0f );
                            //this.txバチお右_成功[ 0 ].t2D描画( CDTXMania.app.Device, 956 + (( this.ct進行メイン.n現在の値 - 36 ) / 2), 180 );
                            this.txバチお右_成功[ 0 ].t2D描画( CDTXMania.app.Device, 1136 - 180 * fRet[ this.ct進行メイン.n現在の値 - 36 ], 180 );
                            this.txバチお右_成功[ 0 ].vc拡大縮小倍率 = new SlimDX.Vector3( fRet[ this.ct進行メイン.n現在の値 - 36 ], 1.0f, 1.0f );
                        }
                    }
                    else if( this.ct進行メイン.n現在の値 <= 49 )
                    {
                        if( this.txバチお左_成功[ 1 ] != null )
                            this.txバチお左_成功[ 1 ].t2D描画( CDTXMania.app.Device, 466, 180 );
                        if( this.txバチお右_成功[ 1 ] != null )
                            this.txバチお右_成功[ 1 ].t2D描画( CDTXMania.app.Device, 956, 180 );
                    }
                    else if( this.ct進行メイン.n現在の値 <= 54 )
                    {
                        if( this.txバチお左_成功[ 2 ] != null )
                            this.txバチお左_成功[ 2 ].t2D描画( CDTXMania.app.Device, 466, 180 );
                        if( this.txバチお右_成功[ 2 ] != null )
                            this.txバチお右_成功[ 2 ].t2D描画( CDTXMania.app.Device, 956, 180 );
                    }
                    else if( this.ct進行メイン.n現在の値 <= 58 )
                    {
                        if( this.txバチお左_成功[ 3 ] != null )
                            this.txバチお左_成功[ 3 ].t2D描画( CDTXMania.app.Device, 466, 180 );
                        if( this.txバチお右_成功[ 3 ] != null )
                            this.txバチお右_成功[ 3 ].t2D描画( CDTXMania.app.Device, 956, 180 );
                    }
                    else
                    {
                        if( this.txバチお左_成功[ 4 ] != null )
                            this.txバチお左_成功[ 4 ].t2D描画( CDTXMania.app.Device, 466, 180 );
                        if( this.txバチお右_成功[ 4 ] != null )
                            this.txバチお右_成功[ 4 ].t2D描画( CDTXMania.app.Device, 956, 180 );
                    }
                    #endregion
                }

                if( this.ct進行メイン.b終了値に達した )
                {
                    if( !this.bリザルトボイス再生済み )
                    {
                        CDTXMania.Skin.sound成績発表.t再生する();
                        this.bリザルトボイス再生済み = true;
                    }
                    return 1;
                }
            }

            return 0;
        }

        #region[ private ]
        //-----------------
        bool b再生済み;
        bool bリザルトボイス再生済み;
        CCounter ct進行メイン;
        CTexture[] txバチお左_成功 = new CTexture[ 5 ];
        CTexture[] txバチお右_成功 = new CTexture[ 5 ];
        CTexture tx文字;
        CTexture tx文字マスク;
        CSound soundClear;
        //-----------------
        #endregion
    }
}
