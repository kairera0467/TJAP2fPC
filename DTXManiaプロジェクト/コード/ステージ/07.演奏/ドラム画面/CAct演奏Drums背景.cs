using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using FDK;
using System.Diagnostics;

namespace DTXMania
{
    internal class CAct演奏Drums背景 : CActivity
    {
        // 本家っぽい背景を表示させるメソッド。
        //
        // 拡張性とかないんで。はい、ヨロシクゥ!
        //
        public CAct演奏Drums背景()
        {
            base.b活性化してない = true;
        }

        public void tFadeIn()
        {
            this.ct上背景FIFOタイマー = new CCounter( 0, 100, 6, CDTXMania.Timer );
            this.eFadeMode = EFIFOモード.フェードイン;
        }

        public void tFadeOut()
        {
            this.ct上背景FIFOタイマー = new CCounter( 0, 100, 6, CDTXMania.Timer );
            this.eFadeMode = EFIFOモード.フェードアウト;
        }

        public override void On活性化()
        {
            base.On活性化();
        }

        public override void On非活性化()
        {
            CDTXMania.t安全にDisposeする( ref this.ct上背景FIFOタイマー );
            CDTXMania.t安全にDisposeする( ref this.ct上背景スクロール用タイマー );
            CDTXMania.t安全にDisposeする( ref this.ct下背景スクロール用タイマー1 );
            base.On非活性化();
        }

        public override void OnManagedリソースの作成()
        {
            //this.tx上背景メイン = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\Upper_BG\01\bg.png" ) );
            //this.tx上背景クリアメイン = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\Upper_BG\01\bg_clear.png" ) );
            //this.tx下背景メイン = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\Dancer_BG\01\bg.png" ) );
            //this.tx下背景クリアメイン = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\Dancer_BG\01\bg_clear.png" ) );
            //this.tx下背景クリアサブ1 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\Dancer_BG\01\bg_clear_01.png" ) );
            this.ct上背景スクロール用タイマー = new CCounter( 1, 328, 40, CDTXMania.Timer );
            this.ct下背景スクロール用タイマー1 = new CCounter( 1, 1257, 6, CDTXMania.Timer );
            this.ct上背景FIFOタイマー = new CCounter();
            base.OnManagedリソースの作成();
        }

        public override void OnManagedリソースの解放()
        {
            //CDTXMania.tテクスチャの解放( ref this.tx上背景メイン );
            //CDTXMania.tテクスチャの解放( ref this.tx上背景クリアメイン );
            //CDTXMania.tテクスチャの解放( ref this.tx下背景メイン );
            //CDTXMania.tテクスチャの解放( ref this.tx下背景クリアメイン );
            //CDTXMania.tテクスチャの解放( ref this.tx下背景クリアサブ1 );
            //Trace.TraceInformation("CActDrums背景 リソースの開放");
            base.OnManagedリソースの解放();
        }

        public override int On進行描画()
        {
            this.ct上背景FIFOタイマー.t進行();
            this.ct上背景スクロール用タイマー.t進行Loop();
            this.ct下背景スクロール用タイマー1.t進行Loop();

            int[] nBgY = new int[] { 0, 536 };
            for( int i = 0; i < 2; i++ )
            {
                if( CDTXMania.ConfigIni.nPlayerCount == 1 && i == 1 ) continue;
                
                if( CDTXMania.Tx.Background_Up != null )
                {
                    int nループ幅 = 328;
                    CDTXMania.Tx.Background_Up.t2D描画( CDTXMania.app.Device, 0 - this.ct上背景スクロール用タイマー.n現在の値, nBgY[ i ] );
                    CDTXMania.Tx.Background_Up.t2D描画( CDTXMania.app.Device, ( 1 * nループ幅 ) - this.ct上背景スクロール用タイマー.n現在の値, nBgY[ i ] );
                    CDTXMania.Tx.Background_Up.t2D描画( CDTXMania.app.Device, ( 2 * nループ幅 ) - this.ct上背景スクロール用タイマー.n現在の値, nBgY[ i ] );
                    CDTXMania.Tx.Background_Up.t2D描画( CDTXMania.app.Device, ( 3 * nループ幅 ) - this.ct上背景スクロール用タイマー.n現在の値, nBgY[ i ] );
                    CDTXMania.Tx.Background_Up.t2D描画( CDTXMania.app.Device, ( 4 * nループ幅 ) - this.ct上背景スクロール用タイマー.n現在の値, nBgY[ i ] );
                }
                if( CDTXMania.Tx.Background_Up_Clear != null )
                {
                    if( CDTXMania.stage演奏ドラム画面.actGauge.db現在のゲージ値[ 0 ] < 80.0 )
                        CDTXMania.Tx.Background_Up_Clear.n透明度 = 0;
                    else
                        CDTXMania.Tx.Background_Up_Clear.n透明度 = ( ( this.ct上背景FIFOタイマー.n現在の値 * 0xff ) / 100 );

                    int nループ幅 = 328;
                    CDTXMania.Tx.Background_Up_Clear.t2D描画( CDTXMania.app.Device, 0 - this.ct上背景スクロール用タイマー.n現在の値, nBgY[ i ] );
                    CDTXMania.Tx.Background_Up_Clear.t2D描画( CDTXMania.app.Device, ( 1 * nループ幅 ) - this.ct上背景スクロール用タイマー.n現在の値, nBgY[ i ] );
                    CDTXMania.Tx.Background_Up_Clear.t2D描画( CDTXMania.app.Device, ( 2 * nループ幅 ) - this.ct上背景スクロール用タイマー.n現在の値, nBgY[ i ] );
                    CDTXMania.Tx.Background_Up_Clear.t2D描画( CDTXMania.app.Device, ( 3 * nループ幅 ) - this.ct上背景スクロール用タイマー.n現在の値, nBgY[ i ] );
                    CDTXMania.Tx.Background_Up_Clear.t2D描画( CDTXMania.app.Device, ( 4 * nループ幅 ) - this.ct上背景スクロール用タイマー.n現在の値, nBgY[ i ] );
                }
            }




            if( CDTXMania.ConfigIni.nPlayerCount == 1 )
            {
                {
                    if( CDTXMania.Tx.Background_Down != null )
                    {
                        CDTXMania.Tx.Background_Down.t2D描画( CDTXMania.app.Device, 0, 360 );
                    }
                }
                if( CDTXMania.stage演奏ドラム画面.actGauge.db現在のゲージ値[ 1 ] >= 80.0 )
                {
                    if( CDTXMania.Tx.Background_Down_Clear != null && CDTXMania.Tx.Background_Down_Scroll != null )
                    {
                        CDTXMania.Tx.Background_Down_Clear.n透明度 = ( ( this.ct上背景FIFOタイマー.n現在の値 * 0xff ) / 100 );
                        CDTXMania.Tx.Background_Down_Scroll.n透明度 = ( ( this.ct上背景FIFOタイマー.n現在の値 * 0xff ) / 100 );
                    
                        CDTXMania.Tx.Background_Down_Clear.t2D描画( CDTXMania.app.Device, 0, 360 );

                        int nループ幅 = 1257;
                        CDTXMania.Tx.Background_Down_Scroll.t2D描画( CDTXMania.app.Device, 0 - this.ct下背景スクロール用タイマー1.n現在の値, 360 );
                        CDTXMania.Tx.Background_Down_Scroll.t2D描画( CDTXMania.app.Device, ( 1 * nループ幅 ) - this.ct下背景スクロール用タイマー1.n現在の値, 360 );
                    }
                }
            }

            return base.On進行描画();
        }

        #region[ private ]
        //-----------------
        private CCounter ct上背景スクロール用タイマー; //上背景のX方向スクロール用
        private CCounter ct下背景スクロール用タイマー1; //下背景パーツ1のX方向スクロール用
        private CCounter ct上背景FIFOタイマー;
        //private CTexture tx上背景メイン;
        //private CTexture tx上背景クリアメイン;
        //private CTexture tx下背景メイン;
        //private CTexture tx下背景クリアメイン;
        //private CTexture tx下背景クリアサブ1;
        private EFIFOモード eFadeMode;
        //-----------------
        #endregion
    }
}
