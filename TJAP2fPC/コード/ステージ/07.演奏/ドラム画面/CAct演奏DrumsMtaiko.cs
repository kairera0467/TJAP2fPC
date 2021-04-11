﻿using System;
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
			for( int i = 0; i < 16; i++ )
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
            if( !this.b活性化してない )
            {
                this.txMtaiko枠 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_mtaiko_A.png" ) );
                this.txMtaiko下敷き[ 0 ] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_mtaiko_C.png" ) );
                this.txMtaiko下敷き[ 1 ] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_mtaiko_C_2P.png" ) );

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
                this.txネームプレート2P = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_NamePlate2P.png" ) );
            
                for( int i = 0; i < 6; i++ )
                {
                    this.txコースシンボル[ i ] = CDTXMania.tテクスチャの生成( CSkin.Path( this.strCourseSymbolFileName[ i ] ) );
                }
                this.ctレベルアップダウン = new CCounter[ 4 ];
                this.After = new int[ 4 ];
                this.Before = new int[ 4 ];
                for( int i = 0; i < 4; i++ )
                {
                    //this.ctレベルアップダウン = new CCounter( 0, 1000, 1, CDTXMania.Timer );
                    this.ctレベルアップダウン[ i ] = new CCounter();
                }


                base.OnManagedリソースの作成();
            }
        }

        public override void OnManagedリソースの解放()
        {
            CDTXMania.tテクスチャの解放( ref this.txMtaiko枠 );
            CDTXMania.tテクスチャの解放( ref this.txMtaiko下敷き[ 0 ] );
            CDTXMania.tテクスチャの解放( ref this.txMtaiko下敷き[ 1 ] );
            
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
				for( int j = 0; j < 16; j++ )
				{
					if( this.stパッド状態[ j ].n明るさ > 0 )
					{
						this.stパッド状態[ j ].n明るさ--;
					}
				}
				this.nフラッシュ制御タイマ += 30;
		    }


            this.nHS = CDTXMania.ConfigIni.n譜面スクロール速度.Drums < 8 ? CDTXMania.ConfigIni.n譜面スクロール速度.Drums : 7;

            if( this.txMtaiko下敷き[ 0 ] != null )
                this.txMtaiko下敷き[ 0 ].t2D描画( CDTXMania.app.Device, 0, 184 );
            if( this.txMtaiko枠 != null )
                this.txMtaiko枠.t2D描画( CDTXMania.app.Device, 0, 184 );

            if( CDTXMania.stage演奏ドラム画面.bDoublePlay )
            {
                if( this.txMtaiko下敷き[ 1 ] != null )
                    this.txMtaiko下敷き[ 1 ].t2D描画( CDTXMania.app.Device, 0, 360 );
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

            if( this.tx太鼓_面L != null && this.tx太鼓_面R != null && this.tx太鼓_ふちL != null && this.tx太鼓_ふちR != null )
            {
                this.tx太鼓_ふちL.n透明度 = this.stパッド状態[4].n明るさ * 43;
                this.tx太鼓_ふちR.n透明度 = this.stパッド状態[5].n明るさ * 43;
                this.tx太鼓_面L.n透明度 = this.stパッド状態[6].n明るさ * 43;
                this.tx太鼓_面R.n透明度 = this.stパッド状態[7].n明るさ * 43;
            
                this.tx太鼓_ふちL.t2D描画( CDTXMania.app.Device, 169, 366, new Rectangle( 0, 0, 76, 164 ) );
                this.tx太鼓_ふちR.t2D描画( CDTXMania.app.Device, 169 + 76, 366, new Rectangle( 76, 0, 76, 164 ) );
                this.tx太鼓_面L.t2D描画( CDTXMania.app.Device, 169, 366, new Rectangle( 0, 0, 76, 164 ) );
                this.tx太鼓_面R.t2D描画( CDTXMania.app.Device, 169 + 76, 366, new Rectangle( 76, 0, 76, 164 ) );
            }

            int[] nLVUPY = new int[] { 127, 127, 0, 0 };

            //if( this.txネームプレート != null )
            //    this.txネームプレート.t2D描画( CDTXMania.app.Device, 314, 19 );
            for( int i = 0; i < CDTXMania.ConfigIni.nPlayerCount; i++ )
            {
                if( !this.ctレベルアップダウン[ i ].b停止中 )
                {
                    this.ctレベルアップダウン[ i ].t進行();
                    if( this.ctレベルアップダウン[ i ].b終了値に達した ) {
                        this.ctレベルアップダウン[ i ].t停止();
                    }
                }
                if( ( this.ctレベルアップダウン[ i ].b進行中 && ( this.txレベルアップ != null && this.txレベルダウン != null ) ) && !CDTXMania.ConfigIni.bNoInfo )
                {
                    //this.ctレベルアップダウン[ i ].n現在の値 = 110;

                    //2017.08.21 kairera0467 t3D描画に変更。
                    float fScale = 1.0f;
                    int nAlpha = 255;
                    float[] fY = new float[] { 206, -206, 0, 0 };
                    if( this.ctレベルアップダウン[ i ].n現在の値 >= 0 && this.ctレベルアップダウン[ i ].n現在の値 <= 20 )
                    {
                        nAlpha = 60;
                        fScale = 1.14f;
                    }
                    else if( this.ctレベルアップダウン[ i ].n現在の値 >= 21 && this.ctレベルアップダウン[ i ].n現在の値 <= 40 )
                    {
                        nAlpha = 60;
                        fScale = 1.19f;
                    }
                    else if( this.ctレベルアップダウン[ i ].n現在の値 >= 41 && this.ctレベルアップダウン[ i ].n現在の値 <= 60 )
                    {
                        nAlpha = 220;
                        fScale = 1.23f;
                    }
                    else if( this.ctレベルアップダウン[ i ].n現在の値 >= 61 && this.ctレベルアップダウン[ i ].n現在の値 <= 80 )
                    {
                        nAlpha = 230;
                        fScale = 1.19f;
                    }
                    else if( this.ctレベルアップダウン[ i ].n現在の値 >= 81 && this.ctレベルアップダウン[ i ].n現在の値 <= 100 )
                    {
                        nAlpha = 240;
                        fScale = 1.14f;
                    }
                    else if( this.ctレベルアップダウン[ i ].n現在の値 >= 101 && this.ctレベルアップダウン[ i ].n現在の値 <= 120 )
                    {
                        nAlpha = 255;
                        fScale = 1.04f;
                    }
                    else
                    {
                        nAlpha = 255;
                        fScale = 1.0f;
                    }

                    SlimDX.Matrix mat = SlimDX.Matrix.Identity;
                    mat *= SlimDX.Matrix.Scaling( fScale, fScale, 1.0f );
                    mat *= SlimDX.Matrix.Translation( -329, fY[ i ], 0 );
                    if( this.After[ i ] - this.Before[ i ] >= 0 )
                    {
                        //レベルアップ
                        this.txレベルアップ.n透明度 = nAlpha;
                        this.txレベルアップ.t3D描画( CDTXMania.app.Device, mat );
                    }
                    else
                    {
                        this.txレベルダウン.n透明度 = nAlpha;
                        this.txレベルダウン.t3D描画( CDTXMania.app.Device, mat );
                    }
                }
            }

            for( int i = 0; i < CDTXMania.ConfigIni.nPlayerCount; i++ )
            {
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
                            CDTXMania.Skin.nCourseSymbolX[ i ] - ( this.txコースシンボル[ CDTXMania.stage選曲.n確定された曲の難易度 ].sz画像サイズ.Width / 2 ),
                            CDTXMania.Skin.nCourseSymbolY[ i ] - ( this.txコースシンボル[ CDTXMania.stage選曲.n確定された曲の難易度 ].sz画像サイズ.Height / 2 )
                            );
                    }
                    if( CDTXMania.DTX.nScoreModeTmp == 3 )
                    {
                        if( this.txコースシンボル[ 5 ] != null )
                        {
                            this.txコースシンボル[ 5 ].t2D描画( CDTXMania.app.Device, 
                                CDTXMania.Skin.nCourseSymbolX[ i ] - ( this.txコースシンボル[ 5 ].sz画像サイズ.Width / 2 ),
                                CDTXMania.Skin.nCourseSymbolY[ i ] - ( this.txコースシンボル[ 5 ].sz画像サイズ.Height / 2 )
                                );
                        }
                    }
                }
            }


            //if (CDTXMania.Input管理.Keyboard.bキーが押された((int)SlimDX.DirectInput.Key.V))
            //{
            //    this.tMtaikoEvent( 0x11, 0, 1 );
            //}
            //if (CDTXMania.Input管理.Keyboard.bキーが押された((int)SlimDX.DirectInput.Key.N))
            //{
            //    this.tMtaikoEvent( 0x11, 1, 1 );
            //}
            //if (CDTXMania.Input管理.Keyboard.bキーが押された((int)SlimDX.DirectInput.Key.C))
            //{
            //    this.tMtaikoEvent( 0x12, 0, 1 );
            //}
            //if (CDTXMania.Input管理.Keyboard.bキーが押された((int)SlimDX.DirectInput.Key.M))
            //{
            //    this.tMtaikoEvent( 0x12, 1, 1 );
            //}


            return base.On進行描画();
        }

        public void tMtaikoEvent( int nChannel, int nHand, int nPlayer )
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
                            this.stパッド状態[ 2 + nHand + ( 4 * nPlayer ) ].n明るさ = 6;
                        }
                        break;
                    case 0x12:
                    case 0x14:
                        {
                            this.stパッド状態[ nHand + ( 4 * nPlayer ) ].n明るさ = 6;
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
                            this.stパッド状態[ 2 + nHand + ( 4 * nPlayer ) ].n明るさ = 6;
                        }
                        break;
                            
                    case 0x13:
                        {
                            this.stパッド状態[ 2 + ( 4 * nPlayer ) ].n明るさ = 6;
                            this.stパッド状態[ 3 + ( 4 * nPlayer ) ].n明るさ = 6;
                        }
                        break;

                    case 0x12:
                        {
                            this.stパッド状態[ nHand + ( 4 * nPlayer ) ].n明るさ = 6;
                        }
                        break;

                    case 0x14:
                        {
                            this.stパッド状態[ 0 + ( 4 * nPlayer ) ].n明るさ = 6;
                            this.stパッド状態[ 1 + ( 4 * nPlayer ) ].n明るさ = 6;
                        }
                        break;
                }
            }

        }

        public void tBranchEvent( int Before, int After, int player )
        {
            if( After != Before )
                this.ctレベルアップダウン[ player ] = new CCounter( 0, 1000, 1, CDTXMania.Timer );

            this.After[ player ] = After;
            this.Before[ player ] = Before;
        }

        #region[ private ]
        //-----------------
        //構造体
        [StructLayout(LayoutKind.Sequential)]
        private struct STパッド状態
        {
            public int n明るさ;
        }

        //太鼓
        private CTexture txMtaiko枠;
        private CTexture[] txMtaiko下敷き = new CTexture[ 4 ];

        private CTexture tx太鼓_土台;
        private CTexture tx太鼓_面L;
        private CTexture tx太鼓_ふちL;
        private CTexture tx太鼓_面R;
        private CTexture tx太鼓_ふちR;

        private STパッド状態[] stパッド状態 = new STパッド状態[ 4 * 4 ];
        private long nフラッシュ制御タイマ;

        private CTexture[] txコースシンボル = new CTexture[ 6 ];
        private string[] strCourseSymbolFileName;

        //オプション
        private CTexture txオプションパネル_HS;
        private CTexture txオプションパネル_RANMIR;
        private CTexture txオプションパネル_特殊;
        private int nHS;

        //ネームプレート
        private CTexture txネームプレート;
        private CTexture txネームプレート2P;

        //譜面分岐
        private CCounter[] ctレベルアップダウン;
        private int[] After;
        private int[] Before;
        private CTexture txレベルアップ;
        private CTexture txレベルダウン;
        //-----------------
        #endregion
    }
}
