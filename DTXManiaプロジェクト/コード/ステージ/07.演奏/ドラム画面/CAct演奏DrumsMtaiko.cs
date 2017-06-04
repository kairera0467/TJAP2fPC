using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using FDK;

namespace DTXMania
{
    internal class CAct演奏DrumsMtaiko : CActivity
    {
        /// <summary>
        /// mtaiko部分を描画するクラス。左側だけ。
        /// 
        /// </summary>
        public CAct演奏DrumsMtaiko()
        {
            this.strCourseSymbolFileName = new string[]{
                @"Graphics\CourseSymbol\easy.png",
                @"Graphics\CourseSymbol\normal.png",
                @"Graphics\CourseSymbol\hard.png",
                @"Graphics\CourseSymbol\oni.png",
                @"Graphics\CourseSymbol\edit.png",
                @"Graphics\CourseSymbol\sinuchi.png",
            };
            base.b活性化してない = true;
        }

        public override void On活性化()
        {
			for( int i = 0; i < 4; i++ )
			{
				STパッド状態 stパッド状態 = new STパッド状態();
				stパッド状態.n明るさ = 0;
				this.stパッド状態[ i ] = stパッド状態;
			}
            base.On活性化();
        }

        public override void On非活性化()
        {
            base.On非活性化();
        }

        public override void OnManagedリソースの作成()
        {
            this.txMtaiko枠 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_mtaiko_A.png" ) );
            this.txMtaiko下敷き = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_mtaiko_C.png" ) );
            this.txオプションパネル_HS = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_HiSpeed.png" ) );
            this.txオプションパネル_RANMIR = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_RANMIR.png" ) );
            this.txオプションパネル_特殊 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_SpecialOption.png" ) );
            
            this.tx太鼓_土台 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_mtaiko_main.png" ) );
            this.tx太鼓_面L = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_mtaiko_red.png" ) );
            this.tx太鼓_ふちL = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_mtaiko_blue.png" ) );
            this.tx太鼓_面R = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_mtaiko_red.png" ) );
            this.tx太鼓_ふちR = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_mtaiko_blue.png" ) );

            this.txレベルアップ = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_LevelUp.png" ) );
            this.txレベルダウン = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_LevelDown.png" ) );

            this.txネームプレート = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_NamePlate.png" ) );
            this.txネームプレート = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_NamePlate2P.png" ) );
            
            for( int i = 0; i < 6; i++ )
            {
                this.txコースシンボル[ i ] = CDTXMania.tテクスチャの生成( CSkin.Path( this.strCourseSymbolFileName[ i ] ) );
            }

            //this.ctレベルアップダウン = new CCounter( 0, 1000, 1, CDTXMania.Timer );
            this.ctレベルアップダウン = new CCounter();

            base.OnManagedリソースの作成();
        }

        public override void OnManagedリソースの解放()
        {
            CDTXMania.tテクスチャの解放( ref this.txMtaiko枠 );
            CDTXMania.tテクスチャの解放( ref this.txMtaiko下敷き );
            
		    CDTXMania.tテクスチャの解放( ref this.tx太鼓_土台 );
            CDTXMania.tテクスチャの解放( ref this.txオプションパネル_HS );
            CDTXMania.tテクスチャの解放( ref this.txオプションパネル_RANMIR );
            CDTXMania.tテクスチャの解放( ref this.txオプションパネル_特殊 );

            CDTXMania.tテクスチャの解放( ref this.tx太鼓_面L );
            CDTXMania.tテクスチャの解放( ref this.tx太鼓_面R );
		    CDTXMania.tテクスチャの解放( ref this.tx太鼓_ふちL );
            CDTXMania.tテクスチャの解放( ref this.tx太鼓_ふちR );

		    CDTXMania.tテクスチャの解放( ref this.txレベルアップ );
            CDTXMania.tテクスチャの解放( ref this.txレベルダウン );

            CDTXMania.tテクスチャの解放( ref this.txネームプレート );
            CDTXMania.tテクスチャの解放( ref this.txネームプレート2P );

            for( int i = 0; i < 6; i++ )
            {
                CDTXMania.tテクスチャの解放( ref this.txコースシンボル[ i ] );
            }

            this.ctレベルアップダウン = null;

            base.OnManagedリソースの解放();
        }

        public override int On進行描画()
        {
            if( base.b初めての進行描画 )
			{
				this.nフラッシュ制御タイマ = FDK.CSound管理.rc演奏用タイマ.n現在時刻;
				base.b初めての進行描画 = false;
            }
            
            long num = FDK.CSound管理.rc演奏用タイマ.n現在時刻;
			if( num < this.nフラッシュ制御タイマ )
			{
				this.nフラッシュ制御タイマ = num;
			}
			while( ( num - this.nフラッシュ制御タイマ ) >= 30 )
			{
				for( int j = 0; j < 4; j++ )
				{
					if( this.stパッド状態[ j ].n明るさ > 0 )
					{
						this.stパッド状態[ j ].n明るさ--;
					}
				}
				this.nフラッシュ制御タイマ += 30;
		    }


            this.nHS = CDTXMania.ConfigIni.n譜面スクロール速度.Drums < 8 ? CDTXMania.ConfigIni.n譜面スクロール速度.Drums : 7;

            if( this.txMtaiko下敷き != null )
                this.txMtaiko下敷き.t2D描画( CDTXMania.app.Device, 0, 184 );
            if( this.txMtaiko枠 != null )
                this.txMtaiko枠.t2D描画( CDTXMania.app.Device, 0, 184 );

            if( CDTXMania.stage演奏ドラム画面.bDoublePlay )
            {
                if( this.txMtaiko下敷き != null )
                    this.txMtaiko下敷き.t2D描画( CDTXMania.app.Device, 0, 360 );
                if( this.txMtaiko枠 != null )
                    this.txMtaiko枠.t2D上下反転描画( CDTXMania.app.Device, 0, 360 );
            }
            
            if( this.tx太鼓_土台 != null )
            {
                this.tx太鼓_土台.t2D描画( CDTXMania.app.Device, 169, 190 );
                if( CDTXMania.stage演奏ドラム画面.bDoublePlay )
                    this.tx太鼓_土台.t2D描画( CDTXMania.app.Device, 169, 366 );
            }
            if( this.tx太鼓_面L != null && this.tx太鼓_面R != null && this.tx太鼓_ふちL != null && this.tx太鼓_ふちR != null )
            {
                this.tx太鼓_ふちL.n透明度 = this.stパッド状態[0].n明るさ * 43;
                this.tx太鼓_ふちR.n透明度 = this.stパッド状態[1].n明るさ * 43;
                this.tx太鼓_面L.n透明度 = this.stパッド状態[2].n明るさ * 43;
                this.tx太鼓_面R.n透明度 = this.stパッド状態[3].n明るさ * 43;
            
                this.tx太鼓_ふちL.t2D描画( CDTXMania.app.Device, 169, 190, new Rectangle( 0, 0, 76, 164 ) );
                this.tx太鼓_ふちR.t2D描画( CDTXMania.app.Device, 169 + 76, 190, new Rectangle( 76, 0, 76, 164 ) );
                this.tx太鼓_面L.t2D描画( CDTXMania.app.Device, 169, 190, new Rectangle( 0, 0, 76, 164 ) );
                this.tx太鼓_面R.t2D描画( CDTXMania.app.Device, 169 + 76, 190, new Rectangle( 76, 0, 76, 164 ) );
            }

            //if( this.txネームプレート != null )
            //    this.txネームプレート.t2D描画( CDTXMania.app.Device, 314, 19 );
            
            if( !this.ctレベルアップダウン.b停止中 )
            {
                this.ctレベルアップダウン.t進行();
                if( this.ctレベルアップダウン.b終了値に達した )
                {
                    this.ctレベルアップダウン.t停止();
                }
            }
            if( ( this.ctレベルアップダウン.b進行中 && ( this.txレベルアップ != null && this.txレベルダウン != null ) ) && !CDTXMania.ConfigIni.bNoInfo )
            {
                //this.ctレベルアップダウン.n現在の値 = 110;

                if (this.After - this.Before >= 0)
                {
                    //レベルアップ
                    int x;
                    int y;

                    if( this.ctレベルアップダウン.n現在の値 >= 0 && this.ctレベルアップダウン.n現在の値 <= 20 )
                    {
                        this.txレベルアップ.n透明度 = 60;
                        x = 141;
                        y = 123;
                        this.txレベルアップ.vc拡大縮小倍率.X = 1.14f;
                        this.txレベルアップ.vc拡大縮小倍率.Y = 1.14f;
                    }
                    else if( this.ctレベルアップダウン.n現在の値 >= 21 && this.ctレベルアップダウン.n現在の値 <= 40 )
                    {
                        this.txレベルアップ.n透明度 = 180;
                        x = 136;
                        y = 122;
                        this.txレベルアップ.vc拡大縮小倍率.X = 1.19f;
                        this.txレベルアップ.vc拡大縮小倍率.Y = 1.19f;
                    }
                    else if( this.ctレベルアップダウン.n現在の値 >= 41 && this.ctレベルアップダウン.n現在の値 <= 60 )
                    {
                        this.txレベルアップ.n透明度 = 220;
                        x = 133;
                        y = 121;
                        this.txレベルアップ.vc拡大縮小倍率.X = 1.23f;
                        this.txレベルアップ.vc拡大縮小倍率.Y = 1.23f;
                    }
                    else if( this.ctレベルアップダウン.n現在の値 >= 61 && this.ctレベルアップダウン.n現在の値 <= 80 )
                    {
                        this.txレベルアップ.n透明度 = 230;
                        x = 136;
                        y = 122;
                        this.txレベルアップ.vc拡大縮小倍率.X = 1.19f;
                        this.txレベルアップ.vc拡大縮小倍率.Y = 1.19f;
                    }
                    else if( this.ctレベルアップダウン.n現在の値 >= 81 && this.ctレベルアップダウン.n現在の値 <= 100 )
                    {
                        this.txレベルアップ.n透明度 = 240;
                        x = 141;
                        y = 123;
                        this.txレベルアップ.vc拡大縮小倍率.X = 1.14f;
                        this.txレベルアップ.vc拡大縮小倍率.Y = 1.14f;
                    }
                    else if( this.ctレベルアップダウン.n現在の値 >= 101 && this.ctレベルアップダウン.n現在の値 <= 120 )
                    {
                        this.txレベルアップ.n透明度 = 255;
                        x = 150;
                        y = 126;
                        this.txレベルアップ.vc拡大縮小倍率.X = 1.04f;
                        this.txレベルアップ.vc拡大縮小倍率.Y = 1.04f;
                    }
                    else
                    {
                        this.txレベルアップ.n透明度 = 255;
                        x = 154;
                        y = 127;
                        this.txレベルアップ.vc拡大縮小倍率.X = 1f;
                        this.txレベルアップ.vc拡大縮小倍率.Y = 1f;
                    }

                    this.txレベルアップ.t2D描画( CDTXMania.app.Device, x, y );
                }
                else
                {
                    //レベルダウン
                    int x;
                    int y;

                    if( this.ctレベルアップダウン.n現在の値 >= 0 && this.ctレベルアップダウン.n現在の値 <= 20 )
                    {
                        this.txレベルダウン.n透明度 = 60;
                        x = 141;
                        y = 123;
                        this.txレベルダウン.vc拡大縮小倍率.X = 1.14f;
                        this.txレベルダウン.vc拡大縮小倍率.Y = 1.14f;
                    }
                    else if( this.ctレベルアップダウン.n現在の値 >= 21 && this.ctレベルアップダウン.n現在の値 <= 40 )
                    {
                        this.txレベルダウン.n透明度 = 180;
                        x = 136;
                        y = 122;
                        this.txレベルダウン.vc拡大縮小倍率.X = 1.19f;
                        this.txレベルダウン.vc拡大縮小倍率.Y = 1.19f;
                    }
                    else if( this.ctレベルアップダウン.n現在の値 >= 41 && this.ctレベルアップダウン.n現在の値 <= 60 )
                    {
                        this.txレベルダウン.n透明度 = 220;
                        x = 133;
                        y = 121;
                        this.txレベルダウン.vc拡大縮小倍率.X = 1.23f;
                        this.txレベルダウン.vc拡大縮小倍率.Y = 1.23f;
                    }
                    else if( this.ctレベルアップダウン.n現在の値 >= 61 && this.ctレベルアップダウン.n現在の値 <= 80 )
                    {
                        this.txレベルダウン.n透明度 = 230;
                        x = 136;
                        y = 122;
                        this.txレベルダウン.vc拡大縮小倍率.X = 1.19f;
                        this.txレベルダウン.vc拡大縮小倍率.Y = 1.19f;
                    }
                    else if( this.ctレベルアップダウン.n現在の値 >= 81 && this.ctレベルアップダウン.n現在の値 <= 100 )
                    {
                        this.txレベルアップ.n透明度 = 240;
                        x = 141;
                        y = 123;
                        this.txレベルダウン.vc拡大縮小倍率.X = 1.14f;
                        this.txレベルダウン.vc拡大縮小倍率.Y = 1.14f;
                    }
                    else if( this.ctレベルアップダウン.n現在の値 >= 101 && this.ctレベルアップダウン.n現在の値 <= 120 )
                    {
                        this.txレベルダウン.n透明度 = 255;
                        x = 150;
                        y = 126;
                        this.txレベルダウン.vc拡大縮小倍率.X = 1.04f;
                        this.txレベルダウン.vc拡大縮小倍率.Y = 1.04f;
                    }
                    else
                    {
                        this.txレベルダウン.n透明度 = 255;
                        x = 154;
                        y = 127;
                        this.txレベルダウン.vc拡大縮小倍率.X = 1f;
                        this.txレベルダウン.vc拡大縮小倍率.Y = 1f;
                    }

                    this.txレベルダウン.t2D描画(CDTXMania.app.Device, x, y);
                }
            }

            if( !CDTXMania.ConfigIni.bNoInfo && CDTXMania.Skin.eDiffDispMode != E難易度表示タイプ.mtaikoに画像で表示 )
            {
                this.txオプションパネル_HS.t2D描画( CDTXMania.app.Device, 0, 230, new Rectangle( 0, this.nHS * 44, 162, 44 ) );
                switch( CDTXMania.ConfigIni.eRandom.Taiko )
                {
                    case Eランダムモード.RANDOM:
                        if( this.txオプションパネル_RANMIR != null )
                            this.txオプションパネル_RANMIR.t2D描画( CDTXMania.app.Device, 0, 264, new Rectangle( 0, 0, 162, 44 ) );
                        break;
                    case Eランダムモード.HYPERRANDOM:
                        if( this.txオプションパネル_RANMIR != null )
                            this.txオプションパネル_RANMIR.t2D描画( CDTXMania.app.Device, 0, 264, new Rectangle( 0, 88, 162, 44 ) );
                        break;
                    case Eランダムモード.SUPERRANDOM:
                        if( this.txオプションパネル_RANMIR != null )
                            this.txオプションパネル_RANMIR.t2D描画( CDTXMania.app.Device, 0, 264, new Rectangle( 0, 132, 162, 44 ) );
                        break;
                    case Eランダムモード.MIRROR:
                        if( this.txオプションパネル_RANMIR != null )
                            this.txオプションパネル_RANMIR.t2D描画( CDTXMania.app.Device, 0, 264, new Rectangle( 0, 44, 162, 44 ) );
                        break;
                }

                if( CDTXMania.ConfigIni.eSTEALTH == Eステルスモード.STEALTH )
                    this.txオプションパネル_特殊.t2D描画( CDTXMania.app.Device, 0, 300, new Rectangle( 0, 0, 162, 44 ) );
                else if( CDTXMania.ConfigIni.eSTEALTH == Eステルスモード.DORON )
                    this.txオプションパネル_特殊.t2D描画( CDTXMania.app.Device, 0, 300, new Rectangle( 0, 44, 162, 44 ) );
            }
            else if( CDTXMania.Skin.eDiffDispMode == E難易度表示タイプ.mtaikoに画像で表示 )
            {
                if( this.txコースシンボル[ CDTXMania.stage選曲.n確定された曲の難易度 ] != null )
                {
                    this.txコースシンボル[ CDTXMania.stage選曲.n確定された曲の難易度 ].t2D描画( CDTXMania.app.Device, 
                        CDTXMania.Skin.nCourseSymbolP1X - ( this.txコースシンボル[ CDTXMania.stage選曲.n確定された曲の難易度 ].sz画像サイズ.Width / 2 ),
                        CDTXMania.Skin.nCourseSymbolP1Y - ( this.txコースシンボル[ CDTXMania.stage選曲.n確定された曲の難易度 ].sz画像サイズ.Height / 2 )
                        );
                }
                if( CDTXMania.DTX.nScoreModeTmp == 3 )
                {
                    if( this.txコースシンボル[ 5 ] != null )
                    {
                        this.txコースシンボル[ 5 ].t2D描画( CDTXMania.app.Device, 
                            CDTXMania.Skin.nCourseSymbolP1X - ( this.txコースシンボル[ 5 ].sz画像サイズ.Width / 2 ),
                            CDTXMania.Skin.nCourseSymbolP1Y - ( this.txコースシンボル[ 5 ].sz画像サイズ.Height / 2 )
                            );
                    }
                }
            }

            return base.On進行描画();
        }

        public void tMtaikoEvent( int nChannel, int nHand )
        {
            if( !CDTXMania.ConfigIni.b太鼓パートAutoPlay )
            {
                switch( nChannel )
                {
                    case 0x11:
                    case 0x13:
                    case 0x15:
                    case 0x16:
                    case 0x17:
                        {
                            this.stパッド状態[ 2 + nHand ].n明るさ = 6;
                        }
                        break;
                    case 0x12:
                    case 0x14:
                        {
                            this.stパッド状態[ nHand ].n明るさ = 6;
                        }
                        break;

                }
            }
            else
            {
                switch( nChannel )
                {
                    case 0x11:
                    case 0x15:
                    case 0x16:
                    case 0x17:
                        {
                            this.stパッド状態[ 2 + nHand ].n明るさ = 6;
                        }
                        break;
                            
                    case 0x13:
                        {
                            this.stパッド状態[ 2 ].n明るさ = 6;
                            this.stパッド状態[ 3 ].n明るさ = 6;
                        }
                        break;

                    case 0x12:
                        {
                            this.stパッド状態[ nHand ].n明るさ = 6;
                        }
                        break;

                    case 0x14:
                        {
                            this.stパッド状態[ 0 ].n明るさ = 6;
                            this.stパッド状態[ 1 ].n明るさ = 6;
                        }
                        break;
                }
            }

        }

        public void tBranchEvent( int Before, int After )
        {
            if( After != Before )
                this.ctレベルアップダウン = new CCounter( 0, 1000, 1, CDTXMania.Timer );

            this.After = After;
            this.Before = Before;
        }

        #region[ private ]
        //-----------------
        private int nHS;

        private CTexture txMtaiko枠;
        private CTexture txMtaiko下敷き;

        private CTexture tx太鼓_土台;
        private CTexture tx太鼓_面L;
        private CTexture tx太鼓_ふちL;
        private CTexture tx太鼓_面R;
        private CTexture tx太鼓_ふちR;

        private CTexture txオプションパネル_HS;
        private CTexture txオプションパネル_RANMIR;
        private CTexture txオプションパネル_特殊;

        private CTexture txレベルアップ;
        private CTexture txレベルダウン;

        private CTexture txネームプレート;
        private CTexture txネームプレート2P;

        private CTexture[] txコースシンボル = new CTexture[ 6 ];
        private string[] strCourseSymbolFileName;

        [StructLayout(LayoutKind.Sequential)]
        private struct STパッド状態
        {
            public int n明るさ;
        }

        private int After;
        private int Before;

        private STパッド状態[] stパッド状態 = new STパッド状態[4];
        private long nフラッシュ制御タイマ;
        private CCounter ctレベルアップダウン;
        //-----------------
        #endregion
    }
}
