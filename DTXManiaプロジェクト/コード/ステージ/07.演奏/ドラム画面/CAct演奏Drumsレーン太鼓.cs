using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Diagnostics;
using SlimDX;
using FDK;
using SharpDX.Animation;

namespace DTXMania
{
    internal class CAct演奏Drumsレーン太鼓 : CActivity
    {
        /// <summary>
        /// レーンを描画するクラス。
        /// 
        /// 
        /// </summary>
        public CAct演奏Drumsレーン太鼓()
        {
            base.b活性化してない = true;
        }

        public override void On活性化()
        {
            for( int i = 0; i < 4; i++ )
			{
				this.st状態[ i ].ct進行 = new CCounter();
                this.stBranch[ i ].ct分岐アニメ進行 = new CCounter();
                this.stBranch[ i ].nフラッシュ制御タイマ = -1;
                this.stBranch[ i ].nBranchレイヤー透明度 = 0;
                this.stBranch[ i ].nBranch文字透明度 = 0;
                this.stBranch[ i ].nY座標 = 0;
			}
            this.ctゴーゴー = new CCounter();


            this.n総移動時間  = -1;
            this.nDefaultJudgePos[ 0 ] = CDTXMania.Skin.nScrollFieldX[ 0 ];
            this.nDefaultJudgePos[ 1 ] = CDTXMania.Skin.nScrollFieldY[ 0 ];
            this.ctゴーゴー炎 = new CCounter( 0, 6, 50, CDTXMania.Timer );

            this._分岐文字 = new 分岐文字[3 * 4] { new 分岐文字(), new 分岐文字(), new 分岐文字(),
                                                   new 分岐文字(), new 分岐文字(), new 分岐文字(),
                                                   new 分岐文字(), new 分岐文字(), new 分岐文字(),
                                                   new 分岐文字(), new 分岐文字(), new 分岐文字()};

            this._分岐背景レイヤー = new 分岐背景レイヤー[4]{ new 分岐背景レイヤー(), new 分岐背景レイヤー(), new 分岐背景レイヤー(), new 分岐背景レイヤー()};
            base.On活性化();
        }

        public override void On非活性化()
        {
            for( int i = 0; i < 4; i++ )
			{
				this.st状態[ i ].ct進行 = null;
                this.stBranch[ i ].ct分岐アニメ進行 = null;
			}
            CDTXMania.Skin.nScrollFieldX[ 0 ] = this.nDefaultJudgePos[ 0 ];
            CDTXMania.Skin.nScrollFieldY[ 0 ] = this.nDefaultJudgePos[ 1 ];
            this.ctゴーゴー = null;

            foreach( 分岐文字 s in this._分岐文字 )
            {
                s.Dispose();
            }
            this._分岐文字 = null;

            foreach( 分岐背景レイヤー s in this._分岐背景レイヤー )
            {
                s.Dispose();
            }
            this._分岐背景レイヤー = null;
            base.On非活性化();
        }

        public override void OnManagedリソースの作成()
        {
            if( !this.b活性化してない )
            {
                this.txLane = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_fieldbgA.png" ) );
                this.txLaneB = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_fieldbgB.png" ) );
                this.txゴーゴー = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_fieldbgC.png" ) );
                this.tx普通譜面[ 0 ] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_field_normal_base.png" ) );
                this.tx玄人譜面[ 0 ] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_field_expert_base.png" ) );
                this.tx達人譜面[ 0 ] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_field_master_base.png" ) );
                this.tx普通譜面[ 1 ] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_field_normal.png" ) );
                this.tx玄人譜面[ 1 ] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_field_expert.png" ) );
                this.tx達人譜面[ 1 ] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_field_master.png" ) );
                this.tx枠線 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_mtaiko_B.png" ) );
                this.tx判定枠 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_taiko_notes.png" ) );
                //this.txアタックエフェクトLower = CDTXMania.tテクスチャの生成Af( CSkin.Path( @"Graphics\7_explosion_lower.png" ) );

                this.txゴーゴー炎 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_gogo_fire.png" ) );

                this.txArアタックエフェクトLower_A = new CTexture[ 15 ];
                this.txArアタックエフェクトLower_B = new CTexture[ 15 ];
                this.txArアタックエフェクトLower_C = new CTexture[ 15 ];
                this.txArアタックエフェクトLower_D = new CTexture[ 15 ];
                for( int i = 0; i < 15; i++ )
                {
                    this.txArアタックエフェクトLower_A[ i ] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\Effect\lower_1_" + i.ToString() + ".png" ) );
                    this.txArアタックエフェクトLower_B[ i ] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\Effect\lower_2_" + i.ToString() + ".png" ) );
                    this.txArアタックエフェクトLower_C[ i ] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\Effect\lower_3_" + i.ToString() + ".png" ) );
                    this.txArアタックエフェクトLower_D[ i ] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\Effect\lower_4_" + i.ToString() + ".png" ) );

                    //this.txArアタックエフェクトLower_A[ i ].b加算合成 = true;
                    //this.txArアタックエフェクトLower_B[ i ].b加算合成 = true;
                    //this.txArアタックエフェクトLower_C[ i ].b加算合成 = true;
                    //this.txArアタックエフェクトLower_D[ i ].b加算合成 = true;
                }
                base.OnManagedリソースの作成();
            }
        }

        public override void OnManagedリソースの解放()
        {
            if( !this.b活性化してない )
            {
                CDTXMania.tテクスチャの解放( ref this.txLane );
                CDTXMania.tテクスチャの解放( ref this.txLaneB );
                CDTXMania.tテクスチャの解放( ref this.txゴーゴー );
                CDTXMania.tテクスチャの解放( ref this.tx普通譜面[ 0 ] );
                CDTXMania.tテクスチャの解放( ref this.tx玄人譜面[ 0 ] );
                CDTXMania.tテクスチャの解放( ref this.tx達人譜面[ 0 ] );
                CDTXMania.tテクスチャの解放( ref this.tx普通譜面[ 1 ] );
                CDTXMania.tテクスチャの解放( ref this.tx玄人譜面[ 1 ] );
                CDTXMania.tテクスチャの解放( ref this.tx達人譜面[ 1 ] );

                CDTXMania.tテクスチャの解放( ref this.tx枠線 );
                CDTXMania.tテクスチャの解放( ref this.tx判定枠 );
                //CDTXMania.tテクスチャの解放( ref this.txアタックエフェクトLower );

                CDTXMania.tテクスチャの解放( ref this.txゴーゴー炎 );

                for( int i = 0; i < 15; i++ )
                {
                    CDTXMania.tテクスチャの解放( ref this.txArアタックエフェクトLower_A[ i ] );
                    CDTXMania.tテクスチャの解放( ref this.txArアタックエフェクトLower_B[ i ] );
                    CDTXMania.tテクスチャの解放( ref this.txArアタックエフェクトLower_C[ i ] );
                    CDTXMania.tテクスチャの解放( ref this.txArアタックエフェクトLower_D[ i ] );
                }

                base.OnManagedリソースの解放();
            }
        }

        public override int On進行描画()
        {
            if( base.b初めての進行描画 )
            {
                for( int i = 0; i < 4; i++ )
                    this.stBranch[ i ].nフラッシュ制御タイマ = FDK.CSound管理.rc演奏用タイマ.n現在時刻;
                base.b初めての進行描画 = false;
            }

            //それぞれが独立したレイヤーでないといけないのでforループはパーツごとに分離すること。

            #region[ レーン本体 ]
            if( this.txLane != null )
            {
                for( int i = 0; i < CDTXMania.ConfigIni.nPlayerCount; i++ )
                {
                    this.txLane.t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] );
                }
            }
            #endregion
            #region[ 分岐アニメ制御タイマー ]
            for( int i = 0; i < CDTXMania.ConfigIni.nPlayerCount; i++ )
            {
                long num = FDK.CSound管理.rc演奏用タイマ.n現在時刻;
                if( num < this.stBranch[ i ].nフラッシュ制御タイマ )
                {
                    this.stBranch[ i ].nフラッシュ制御タイマ = num;
                }
                while( ( num - this.stBranch[ i ].nフラッシュ制御タイマ ) >= 30 )
                {
                    if( this.stBranch[ i ].nBranchレイヤー透明度 <= 255 )
                    {
                        this.stBranch[ i ].nBranchレイヤー透明度 += 10;
                    }

                    if( this.stBranch[ i ].nBranch文字透明度 >= 0 )
                    {
                        this.stBranch[ i ].nBranch文字透明度 -= 10;
                    }

                    if( this.stBranch[ i ].nY座標 != 0 && this.stBranch[ i ].nY座標 <= 20 )
                    {
                        this.stBranch[ i ].nY座標++;
                    }

                    this.stBranch[ i ].nフラッシュ制御タイマ += 8;
                }

                if( !this.stBranch[ i ].ct分岐アニメ進行.b停止中 )
                {
                    this.stBranch[ i ].ct分岐アニメ進行.t進行();
                    if( this.stBranch[ i ].ct分岐アニメ進行.b終了値に達した )
                    {
                        this.stBranch[ i ].ct分岐アニメ進行.t停止();
                    }
                }
            }
            #endregion
            #region[ 分岐レイヤー ]
            for( int i = 0; i < CDTXMania.ConfigIni.nPlayerCount; i++ )
            {
                if( CDTXMania.stage演奏ドラム画面.bUseBranch[ i ] == true )
                {
                    #region[ 動いていない ]
                    switch( CDTXMania.stage演奏ドラム画面.n次回のコース[ i ] )
                    {
                        case E分岐コース.普通:
                            if( this.tx普通譜面[ 0 ] != null ) {
                                this.tx普通譜面[ 0 ].n透明度 = 255;
                                this.tx普通譜面[ 0 ].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] );
                            }
                            break;
                        case E分岐コース.玄人:
                            if( this.tx玄人譜面[ 0 ] != null ) {
                                this.tx玄人譜面[ 0 ].n透明度 = 255;
                                this.tx玄人譜面[ 0 ].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] );
                            }
                            break;
                        case E分岐コース.達人:
                            if( this.tx達人譜面[ 0 ] != null ) {
                                this.tx達人譜面[ 0 ].n透明度 = 255;
                                this.tx達人譜面[ 0 ].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] );
                            }
                            break;
                    }
                    
                    #endregion

                    if( CDTXMania.ConfigIni.nBranchAnime == 1 )
                    {
                        #region[ AC7～14風の背後レイヤー ]
                        if( this.stBranch[ i ].ct分岐アニメ進行.b進行中 )
                        {
                            int n透明度 = ( ( 100 - this.stBranch[ i ].ct分岐アニメ進行.n現在の値 ) * 0xff ) / 100;

                            if( this.stBranch[ i ].ct分岐アニメ進行.b終了値に達した )
                            {
                                n透明度 = 255;
                                this.stBranch[ i ].ct分岐アニメ進行.t停止();
                            }

                            #region[ 普通譜面_レベルアップ ]
                            //普通→玄人
                            if( this.stBranch[ i ].nBefore == 0 && this.stBranch[ i ].nAfter == 1 )
                            {
                                if( this.tx普通譜面[ 0 ] != null && this.tx玄人譜面[ 0 ] != null )
                                {
                                    this.tx普通譜面[ 0 ].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] );
                                    this.tx玄人譜面[ 0 ].n透明度 = this.stBranch[ i ].nBranchレイヤー透明度;
                                    this.tx玄人譜面[ 0 ].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] );
                                }
                            }
                            //普通→達人
                            if( this.stBranch[ i ].nBefore == 0 && this.stBranch[ i ].nAfter == 2 )
                            {
                                //if( this.stBranch[ i ].ct分岐アニメ進行.n現在の値 < 100 )
                                //{
                                //    n透明度 = ( ( 100 - this.stBranch[ i ].ct分岐アニメ進行.n現在の値 ) * 0xff ) / 100;
                                //}
                                if( this.tx普通譜面[ 0 ] != null && this.tx達人譜面[ 0 ] != null )
                                {
                                    this.tx普通譜面[ 0 ].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] );
                                    this.tx達人譜面[ 0 ].n透明度 = this.stBranch[ i ].nBranchレイヤー透明度;
                                    this.tx達人譜面[ 0 ].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] );
                                }
                            }
                            #endregion
                            #region[ 玄人譜面_レベルアップ ]
                            if( this.stBranch[ i ].nBefore == 1 && this.stBranch[ i ].nAfter == 2 )
                            {
                                if( this.tx玄人譜面[ 0 ] != null && this.tx達人譜面[ 0 ] != null )
                                {
                                    this.tx玄人譜面[ 0 ].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] );
                                    this.tx達人譜面[ 0 ].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] );
                                    this.tx達人譜面[ 0 ].n透明度 = this.stBranch[ i ].nBranchレイヤー透明度;
                                }
                            }
                            #endregion
                            #region[ 玄人譜面_レベルダウン ]
                            if( this.stBranch[ i ].nBefore == 1 && this.stBranch[ i ].nAfter == 0 )
                            {
                                if( this.tx玄人譜面[ 0 ] != null && this.tx普通譜面[ 0 ] != null )
                                {
                                    this.tx玄人譜面[ 0 ].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] );
                                    this.tx普通譜面[ 0 ].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] );
                                    this.tx普通譜面[ 0 ].n透明度 = this.stBranch[ i ].nBranchレイヤー透明度;
                                }
                            }
                            #endregion
                            #region[ 達人譜面_レベルダウン ]
                            if( this.stBranch[ i ].nBefore == 2 && this.stBranch[ i ].nAfter == 0 )
                            {
                                if( this.tx達人譜面[ 0 ] != null && this.tx普通譜面[ 0 ] != null )
                                {
                                    this.tx達人譜面[ 0 ].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] );
                                    this.tx普通譜面[ 0 ].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] );
                                    this.tx普通譜面[ 0 ].n透明度 = this.stBranch[ i ].nBranchレイヤー透明度;
                                }
                            }
                            #endregion
                        }
                        #endregion
                    }
                    else if( CDTXMania.ConfigIni.nBranchAnime == 0 )
                    {
                        if( this._分岐背景レイヤー[ 0 ].ストーリーボード != null )
                        {
                            #region[ 普通譜面_レベルアップ ]
                            if( this.stBranch[ i ].nBefore == (int)E分岐コース.普通 && this.stBranch[ i ].nAfter == (int)E分岐コース.玄人 )
                            {
                                if( this.tx普通譜面[ 0 ] != null && this.tx玄人譜面[ 0 ] != null )
                                {
                                    this.tx玄人譜面[ 0 ].n透明度 = (int)( this._分岐背景レイヤー[ i ].不透明度_次回.Value * 255 );
                                    this.tx普通譜面[ 0 ].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] );
                                    this.tx玄人譜面[ 0 ].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] );
                                }
                            }
                            if( this.stBranch[ i ].nBefore == (int)E分岐コース.普通 && this.stBranch[ i ].nAfter == (int)E分岐コース.達人 )
                            {
                                if( this.tx普通譜面[ 0 ] != null && this.tx玄人譜面[ 0 ] != null && this.tx達人譜面[ 0 ] != null )
                                {
                                    this.tx玄人譜面[ 0 ].n透明度 = (int)( this._分岐背景レイヤー[ i ].不透明度_次回.Value * 255 );
                                    this.tx達人譜面[ 0 ].n透明度 = (int)( this._分岐背景レイヤー[ i ].不透明度_次回2.Value * 255 );
                                    this.tx普通譜面[ 0 ].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] );
                                    this.tx玄人譜面[ 0 ].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] );
                                    this.tx達人譜面[ 0 ].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] );
                                }
                            }
                            #endregion
                            #region[ 玄人譜面_レベルアップ ]
                            if( this.stBranch[ i ].nBefore == (int)E分岐コース.玄人 && this.stBranch[ i ].nAfter == (int)E分岐コース.達人 )
                            {
                                if( this.tx達人譜面[ 0 ] != null && this.tx玄人譜面[ 0 ] != null )
                                {
                                    this.tx達人譜面[ 0 ].n透明度 = (int)( this._分岐背景レイヤー[ i ].不透明度_次回.Value * 255 );
                                    this.tx玄人譜面[ 0 ].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] );
                                    this.tx達人譜面[ 0 ].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] );
                                }
                            }
                            #endregion
                            #region[ 玄人譜面_レベルダウン ]
                            if( this.stBranch[ i ].nBefore == (int)E分岐コース.玄人 && this.stBranch[ i ].nAfter == (int)E分岐コース.普通 )
                            {
                                if( this.tx玄人譜面[ 0 ] != null && this.tx普通譜面[ 0 ] != null )
                                {
                                    this.tx玄人譜面[ 0 ].n透明度 = (int)( this._分岐背景レイヤー[ i ].不透明度_現在.Value * 255 );
                                    this.tx普通譜面[ 0 ].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] );
                                    this.tx玄人譜面[ 0 ].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] );                                    
                                }
                            }
                            #endregion
                            #region[ 達人譜面_レベルダウン ]
                            if( this.stBranch[ i ].nBefore == (int)E分岐コース.達人 && this.stBranch[ i ].nAfter == (int)E分岐コース.玄人 )
                            {
                                if( this.tx達人譜面[ 0 ] != null && this.tx玄人譜面[ 0 ] != null )
                                {
                                    this.tx達人譜面[ 0 ].n透明度 = (int)( this._分岐背景レイヤー[ i ].不透明度_現在.Value * 255 );
                                    this.tx玄人譜面[ 0 ].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] );
                                    this.tx達人譜面[ 0 ].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] );                                    
                                }
                            }      
                            if( this.stBranch[ i ].nBefore == (int)E分岐コース.達人 && this.stBranch[ i ].nAfter == (int)E分岐コース.普通 )
                            {
                                if( this.tx普通譜面[ 0 ] != null && this.tx玄人譜面[ 0 ] != null && this.tx達人譜面[ 0 ] != null )
                                {
                                    this.tx玄人譜面[ 0 ].n透明度 = (int)( this._分岐背景レイヤー[ i ].不透明度_次回.Value * 255 );
                                    this.tx達人譜面[ 0 ].n透明度 = (int)( this._分岐背景レイヤー[ i ].不透明度_現在.Value * 255 );
                                    this.tx普通譜面[ 0 ].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] );
                                    this.tx玄人譜面[ 0 ].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] );
                                    this.tx達人譜面[ 0 ].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] );
                                }
                            }
                            #endregion
                        }
                    }
                }
            }
            #endregion
            for( int i = 0; i < CDTXMania.ConfigIni.nPlayerCount; i++ )
            {
                #region[ ゴーゴータイムレーン背景レイヤー ]
                if( this.txゴーゴー != null && CDTXMania.stage演奏ドラム画面.bIsGOGOTIME[ i ] )
                {
                    if( !this.ctゴーゴー.b停止中 )
                    {
                        this.ctゴーゴー.t進行();
                    }

                    if( this.ctゴーゴー.n現在の値 <= 4 )
                    {
                        this.txゴーゴー.vc拡大縮小倍率.Y = 0.2f;
                        this.txゴーゴー.t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] + 54 );
                    }
                    else if( this.ctゴーゴー.n現在の値 <= 5 )
                    {
                        this.txゴーゴー.vc拡大縮小倍率.Y = 0.4f;
                        this.txゴーゴー.t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] + 40 );
                    }
                    else if( this.ctゴーゴー.n現在の値 <= 6 )
                    {
                        this.txゴーゴー.vc拡大縮小倍率.Y = 0.6f;
                        this.txゴーゴー.t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] + 26 );
                    }
                    else if( this.ctゴーゴー.n現在の値 <= 8 )
                    {
                        this.txゴーゴー.vc拡大縮小倍率.Y = 0.8f;
                        this.txゴーゴー.t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] + 13 );
                    }
                    else if( this.ctゴーゴー.n現在の値 >= 9 )
                    {
                        this.txゴーゴー.vc拡大縮小倍率.Y = 1.0f;
                        this.txゴーゴー.t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] );
                    }
                }
                #endregion
            }
            
            for( int i = 0; i < CDTXMania.ConfigIni.nPlayerCount; i++ )
            {
                if( CDTXMania.stage演奏ドラム画面.bUseBranch[ i ] == true )
                {
                    if( CDTXMania.ConfigIni.nBranchAnime == 0 )
                    {
                        if( !this.b分岐アニメ実行中( i ) )
                        {
                            switch( CDTXMania.stage演奏ドラム画面.n次回のコース[ i ] )
                            {
                                case E分岐コース.普通:
                                    this.tx普通譜面[ 1 ].n透明度 = 255;
                                    this.tx普通譜面[ 1 ].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] );
                                    break;
                                case E分岐コース.玄人:
                                    this.tx玄人譜面[ 1 ].n透明度 = 255;
                                    this.tx玄人譜面[ 1 ].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] );
                                    break;
                                case E分岐コース.達人:
                                    this.tx達人譜面[ 1 ].n透明度 = 255;
                                    this.tx達人譜面[ 1 ].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] );
                                    break;
                            }
                        }
                        if( this._分岐文字[0 + (i * 3)].ストーリーボード != null )
                        {
                            #region[ 普通譜面_レベルアップ ]
                            //普通→玄人
                            if( this.stBranch[ i ].nBefore == (int)E分岐コース.普通 && this.stBranch[ i ].nAfter == (int)E分岐コース.玄人)
                            {
                                this.tx普通譜面[1].n透明度 = (int)(255 * this._分岐文字[ 0 + (i * 3) ].不透明度.Value);
                                this.tx玄人譜面[1].n透明度 = (int)(255 * this._分岐文字[ 1 + (i * 3) ].不透明度.Value);
                                this.tx達人譜面[1].n透明度 = 0;
                                this.tx玄人譜面[1].t2D描画(CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], (float)this._分岐文字[ 1 + (i * 3) ].左上位置Y.Value );
                                this.tx普通譜面[1].t2D描画(CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], (float)this._分岐文字[ 0 + (i * 3) ].左上位置Y.Value );
                            }

                            //普通→達人
                            if( this.stBranch[ i ].nBefore == (int)E分岐コース.普通 && this.stBranch[ i ].nAfter == (int)E分岐コース.達人)
                            {
                                this.tx普通譜面[1].n透明度 = (int)(255 * this._分岐文字[ 0 + (i * 3) ].不透明度.Value);
                                this.tx玄人譜面[1].n透明度 = (int)(255 * this._分岐文字[ 1 + (i * 3) ].不透明度.Value);
                                this.tx達人譜面[1].n透明度 = (int)(255 * this._分岐文字[ 2 + (i * 3) ].不透明度.Value);
                                this.tx達人譜面[1].t2D描画(CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], (float)this._分岐文字[ 2 + (i * 3) ].左上位置Y.Value );
                                this.tx玄人譜面[1].t2D描画(CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], (float)this._分岐文字[ 1 + (i * 3) ].左上位置Y.Value );
                                this.tx普通譜面[1].t2D描画(CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], (float)this._分岐文字[ 0 + (i * 3) ].左上位置Y.Value );
                            }
                            #endregion
                            #region[ 玄人譜面_レベルアップ ]
                            if( this.stBranch[ i ].nBefore == (int)E分岐コース.玄人 && this.stBranch[ i ].nAfter == (int)E分岐コース.達人)
                            {
                                this.tx玄人譜面[1].n透明度 = (int)(255 * this._分岐文字[ 0 + (i * 3) ].不透明度.Value);
                                this.tx達人譜面[1].n透明度 = (int)(255 * this._分岐文字[ 1 + (i * 3) ].不透明度.Value);
                                this.tx普通譜面[1].n透明度 = 0;
                                this.tx達人譜面[1].t2D描画(CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], (float)this._分岐文字[ 1 + (i * 3) ].左上位置Y.Value );
                                this.tx玄人譜面[1].t2D描画(CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], (float)this._分岐文字[ 0 + (i * 3) ].左上位置Y.Value );
                            }
                            #endregion
                            #region[ 玄人譜面_レベルダウン ]
                            if( this.stBranch[ i ].nBefore == (int)E分岐コース.玄人 && this.stBranch[ i ].nAfter == (int)E分岐コース.普通)
                            {
                                this.tx玄人譜面[1].n透明度 = (int)(255 * this._分岐文字[ 0 + (i * 3) ].不透明度.Value);
                                this.tx普通譜面[1].n透明度 = (int)(255 * this._分岐文字[ 1 + (i * 3) ].不透明度.Value);
                                this.tx達人譜面[1].n透明度 = 0;
                                this.tx普通譜面[1].t2D描画(CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], (float)this._分岐文字[ 1 + (i * 3) ].左上位置Y.Value );
                                this.tx玄人譜面[1].t2D描画(CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], (float)this._分岐文字[ 0 + (i * 3) ].左上位置Y.Value );
                            }
                            #endregion
                            #region[ 達人譜面_レベルダウン ]
                            if( this.stBranch[ i ].nBefore == (int)E分岐コース.達人 && this.stBranch[ i ].nAfter == (int)E分岐コース.玄人)
                            {
                                this.tx達人譜面[1].n透明度 = (int)(255 * this._分岐文字[ 0 + (i * 3) ].不透明度.Value);
                                this.tx玄人譜面[1].n透明度 = (int)(255 * this._分岐文字[ 1 + (i * 3) ].不透明度.Value);
                                this.tx普通譜面[1].n透明度 = 0;
                                this.tx玄人譜面[1].t2D描画(CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], (float)this._分岐文字[ 1 + (i * 3) ].左上位置Y.Value );
                                this.tx達人譜面[1].t2D描画(CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], (float)this._分岐文字[ 0 + (i * 3) ].左上位置Y.Value );
                            }
                            if( this.stBranch[ i ].nBefore == (int)E分岐コース.達人 && this.stBranch[ i ].nAfter == (int)E分岐コース.普通)
                            {
                                this.tx達人譜面[1].n透明度 = (int)(255 * this._分岐文字[ 0 + (i * 3) ].不透明度.Value);
                                this.tx玄人譜面[1].n透明度 = (int)(255 * this._分岐文字[ 1 + (i * 3) ].不透明度.Value);
                                this.tx普通譜面[1].n透明度 = (int)(255 * this._分岐文字[ 2 + (i * 3) ].不透明度.Value);
                                this.tx普通譜面[1].t2D描画(CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], (float)this._分岐文字[ 2 + (i * 3) ].左上位置Y.Value );
                                this.tx玄人譜面[1].t2D描画(CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], (float)this._分岐文字[ 1 + (i * 3) ].左上位置Y.Value );
                                this.tx達人譜面[1].t2D描画(CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], (float)this._分岐文字[ 0 + (i * 3) ].左上位置Y.Value );
                            }
                            #endregion
                        }
                    }
                    else if( CDTXMania.ConfigIni.nBranchAnime == 1 )
                    {
                        //if( this.stBranch[ i ].nY座標 == 21 )
                        if( this.stBranch[ i ].ct分岐アニメ進行.b停止中 )
                        {
                            this.stBranch[ i ].nY座標 = 0;
                        }

                        if( this.stBranch[ i ].nY座標 == 0 )
                        {
                            switch( CDTXMania.stage演奏ドラム画面.n次回のコース[ i ] )
                            {
                                case E分岐コース.普通:
                                    this.tx普通譜面[ 1 ].n透明度 = 255;
                                    this.tx普通譜面[ 1 ].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] );
                                    break;
                                case E分岐コース.玄人:
                                    this.tx玄人譜面[ 1 ].n透明度 = 255;
                                    this.tx玄人譜面[ 1 ].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] );
                                    break;
                                case E分岐コース.達人:
                                    this.tx達人譜面[ 1 ].n透明度 = 255;
                                    this.tx達人譜面[ 1 ].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] );
                                    break;
                            }
                        }

                        float[] fLVUpA = new float[] { 0, +3, +5.2f, +6, +5.9f, +5.5f, +5, +4.2f, +3.2f, +2, +0.5f, -1, -2.8f, -4.7f, -6.8f, -9, -11.2f, -13.6f, -16, -18.5f, -21, 720, 720, 720, 720, 720, 720 };
                        float[] fLVUpB = new float[] { -999, -999, -999, -999, -999, 21, 18.5f, 16, 13.6f, 11.2f, 9, 6.8f, 4.7f, 2.8f, 1, 0.5f, -2, -3.2f, -4.2f, -5, -5.5f, -5.9f, -6, -5.2f, -3, 0, 0 };
                        float[] fLVDownA = new float[] { 0, -3, -5.2f, -6, -5.9f, -5.5f, -5, -4.2f, -3.2f, -2, -0.5f, 1, 2.8f, 4.7f, 6.8f, 9, 11.2f, 13.6f, 16, 18.5f, 21, 720, 720, 720, 720, 720, 720 };
                        float[] fLVDownB = new float[] { -999, -999, -999, -999, -999, -21, -18.5f, -16, -13.6f, -11.2f, -9, -6.8f, -4.7f, -2.8f, -1, -0.5f, 2, 3.2f, 4.2f, 5, 5.5f, 5.9f, 6, 5.2f, 3, 0, 0 };
                        int[] nLVDownA_T = new int[] { 255, 255, 255, 255, 242, 232, 219, 206, 193, 183, 170, 158, 147, 135, 122, 112, 99, 86, 73, 63, 51, 0, 0, 0, 0, 0, 0, 0 };
                        int[] nLVDownB_T = new int[] { 0, 0, 0, 0, 0, 51, 63, 73, 86, 99, 112, 122, 135, 147, 158, 170, 183, 193, 206, 219, 232, 242, 255, 255, 255, 255, 255 };

                        if( this.stBranch[ i ].nY座標 != 0 )
                        {
                            #region[ 普通譜面_レベルアップ ]
                            //普通→玄人
                            if( this.stBranch[ i ].nBefore == 0 && this.stBranch[ i ].nAfter == 1)
                            {
                                //this.tx玄人譜面[1].t2D描画(CDTXMania.app.Device, 333, 212 - this.stBranch[ i ].nY座標);
                                //this.tx普通譜面[1].t2D描画(CDTXMania.app.Device, 333, 192 - this.stBranch[ i ].nY座標);
                                //this.tx玄人譜面[1].n透明度 = this.stBranch[ i ].nBranchレイヤー透明度;
                                //this.tx普通譜面[1].n透明度 = 255 - this.stBranch[ i ].nBranchレイヤー透明度;
                                this.tx玄人譜面[1].t2D描画(CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] + fLVUpB[ this.stBranch[i].ct分岐アニメ進行.n現在の値 / 10 ] );
                                this.tx普通譜面[1].t2D描画(CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] + fLVUpA[ this.stBranch[i].ct分岐アニメ進行.n現在の値 / 10 ] );
                                this.tx玄人譜面[1].n透明度 = nLVDownB_T[ this.stBranch[i].ct分岐アニメ進行.n現在の値 / 10 ];
                                this.tx普通譜面[1].n透明度 = nLVDownA_T[ this.stBranch[i].ct分岐アニメ進行.n現在の値 / 10 ];
                            }
                            //普通→達人
                            if( this.stBranch[ i ].nBefore == 0 && this.stBranch[ i ].nAfter == 2)
                            {
                                //this.tx達人譜面[1].t2D描画(CDTXMania.app.Device, 333, 212 - this.stBranch[ i ].nY座標);
                                //this.tx普通譜面[1].t2D描画(CDTXMania.app.Device, 333, 192 - this.stBranch[ i ].nY座標);
                                //this.tx達人譜面[1].n透明度 = this.stBranch[ i ].nBranchレイヤー透明度;
                                //this.tx普通譜面[1].n透明度 = 255 - this.stBranch[ i ].nBranchレイヤー透明度;
                                this.tx達人譜面[1].t2D描画(CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] + fLVUpB[ this.stBranch[i].ct分岐アニメ進行.n現在の値 / 10 ] );
                                this.tx普通譜面[1].t2D描画(CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] + fLVUpA[ this.stBranch[i].ct分岐アニメ進行.n現在の値 / 10 ] );
                                this.tx達人譜面[1].n透明度 = nLVDownB_T[ this.stBranch[i].ct分岐アニメ進行.n現在の値 / 10 ];
                                this.tx普通譜面[1].n透明度 = nLVDownA_T[ this.stBranch[i].ct分岐アニメ進行.n現在の値 / 10 ];
                            }
                            #endregion
                            #region[ 玄人譜面_レベルアップ ]
                            //玄人→達人
                            if( this.stBranch[ i ].nBefore == 1 && this.stBranch[ i ].nAfter == 2)
                            {
                                //this.tx玄人譜面[1].t2D描画(CDTXMania.app.Device, 333, 192 - this.stBranch[ i ].nY座標);
                                //this.tx達人譜面[1].t2D描画(CDTXMania.app.Device, 333, 212 - this.stBranch[ i ].nY座標);
                                //this.tx玄人譜面[1].n透明度 = 255 - this.stBranch[ i ].nBranchレイヤー透明度;
                                //this.tx達人譜面[1].n透明度 = this.stBranch[ i ].nBranchレイヤー透明度;
                                this.tx達人譜面[1].t2D描画(CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] + fLVUpB[ this.stBranch[i].ct分岐アニメ進行.n現在の値 / 10 ] );
                                this.tx玄人譜面[1].t2D描画(CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] + fLVUpA[ this.stBranch[i].ct分岐アニメ進行.n現在の値 / 10 ] );
                                this.tx達人譜面[1].n透明度 = nLVDownB_T[ this.stBranch[i].ct分岐アニメ進行.n現在の値 / 10 ];
                                this.tx玄人譜面[1].n透明度 = nLVDownA_T[ this.stBranch[i].ct分岐アニメ進行.n現在の値 / 10 ];
                            }
                            #endregion
                            #region[ 玄人譜面_レベルダウン ]
                            if( this.stBranch[ i ].nBefore == 1 && this.stBranch[ i ].nAfter == 0)
                            {
                                //this.tx普通譜面[1].t2D描画(CDTXMania.app.Device, 333, 168 + this.stBranch[ i ].nY座標);
                                //this.tx玄人譜面[1].t2D描画(CDTXMania.app.Device, 333, 192 + this.stBranch[ i ].nY座標);
                                //this.tx普通譜面[1].n透明度 = this.stBranch[ i ].nBranchレイヤー透明度;
                                //this.tx玄人譜面[1].n透明度 = 255 - this.stBranch[ i ].nBranchレイヤー透明度;
                                this.tx普通譜面[1].t2D描画(CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] + fLVDownB[ this.stBranch[i].ct分岐アニメ進行.n現在の値 / 10 ] );
                                this.tx玄人譜面[1].t2D描画(CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] + fLVDownA[ this.stBranch[i].ct分岐アニメ進行.n現在の値 / 10 ] );
                                this.tx普通譜面[1].n透明度 = nLVDownB_T[ this.stBranch[i].ct分岐アニメ進行.n現在の値 / 10 ];
                                this.tx玄人譜面[1].n透明度 = nLVDownA_T[ this.stBranch[i].ct分岐アニメ進行.n現在の値 / 10 ];
                            }
                            #endregion
                            #region[ 達人譜面_レベルダウン ]
                            if( this.stBranch[ i ].nBefore == 2 && this.stBranch[ i ].nAfter == 0 )
                            {
                                //this.tx普通譜面[1].t2D描画(CDTXMania.app.Device, 333, 168 + this.stBranch[ i ].nY座標);
                                //this.tx達人譜面[1].t2D描画(CDTXMania.app.Device, 333, 192 + this.stBranch[ i ].nY座標);
                                //this.tx普通譜面[1].n透明度 = this.stBranch[i].nBranchレイヤー透明度;
                                //this.tx達人譜面[1].n透明度 = 255 - this.stBranch[ i ].nBranchレイヤー透明度;
                                this.tx普通譜面[1].t2D描画(CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] + fLVDownB[ this.stBranch[i].ct分岐アニメ進行.n現在の値 / 10 ] );
                                this.tx達人譜面[1].t2D描画(CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] + fLVDownA[ this.stBranch[i].ct分岐アニメ進行.n現在の値 / 10 ] );
                                this.tx普通譜面[1].n透明度 = nLVDownB_T[ this.stBranch[i].ct分岐アニメ進行.n現在の値 / 10 ];
                                this.tx達人譜面[1].n透明度 = nLVDownA_T[ this.stBranch[i].ct分岐アニメ進行.n現在の値 / 10 ];
                            }
                            if( this.stBranch[ i ].nBefore == 2 && this.stBranch[ i ].nAfter == 1 )
                            {
                                //this.tx玄人譜面[ 1 ].t2D描画( CDTXMania.app.Device, 333, 168 + this.stBranch[ i ].nY座標 );
                                //this.tx達人譜面[ 1 ].t2D描画( CDTXMania.app.Device, 333, 192 + this.stBranch[ i ].nY座標 );
                                //this.tx玄人譜面[ 1 ].n透明度 = this.stBranch[ i ].nBranchレイヤー透明度;
                                //this.tx達人譜面[ 1 ].n透明度 = 255 - this.stBranch[ i ].nBranchレイヤー透明度;
                                this.tx玄人譜面[1].t2D描画(CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] + fLVDownB[ this.stBranch[i].ct分岐アニメ進行.n現在の値 / 10 ] );
                                this.tx達人譜面[1].t2D描画(CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ i ], CDTXMania.Skin.nScrollFieldY[ i ] + fLVDownA[ this.stBranch[i].ct分岐アニメ進行.n現在の値 / 10 ] );
                                this.tx玄人譜面[1].n透明度 = nLVDownB_T[ this.stBranch[i].ct分岐アニメ進行.n現在の値 / 10 ];
                                this.tx達人譜面[1].n透明度 = nLVDownA_T[ this.stBranch[i].ct分岐アニメ進行.n現在の値 / 10 ];
                            }
                            #endregion
                        }
                    }
                }
            }

            if( this.txLaneB != null )
            {
                this.txLaneB.t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ 0 ], 326 );
                if( CDTXMania.stage演奏ドラム画面.bDoublePlay )
                {
                    this.txLaneB.t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[ 1 ], 502 );
                }
            }


            CDTXMania.stage演奏ドラム画面.actLaneFlushD.On進行描画();



            if( this.tx枠線 != null )
            {
                this.tx枠線.t2D描画( CDTXMania.app.Device, 1280 - ( this.tx枠線.szテクスチャサイズ.Width ), 136, new Rectangle( 0, 0, this.tx枠線.szテクスチャサイズ.Width, 224 ) );

                if( CDTXMania.stage演奏ドラム画面.bDoublePlay )
                {
                    this.tx枠線.t2D描画( CDTXMania.app.Device, 1280 - ( this.tx枠線.szテクスチャサイズ.Width ), 360, new Rectangle( 0, 224, this.tx枠線.szテクスチャサイズ.Width, 224 ) );
                }
            }

            #region[ ゴーゴー炎 ]
            for( int i = 0; i < CDTXMania.ConfigIni.nPlayerCount; i++ )
            {
                if( CDTXMania.stage演奏ドラム画面.bIsGOGOTIME[ i ] )
                {
                    this.ctゴーゴー炎.t進行Loop();

                    if( this.txゴーゴー炎 != null )
                    {
                        float f倍率 = 1.0f;

                        float[] ar倍率 = new float[] { 0.8f, 1.2f, 1.7f, 2.5f, 2.3f, 2.2f, 2.0f, 1.8f, 1.7f, 1.6f, 1.6f, 1.5f, 1.5f, 1.4f, 1.3f, 1.2f, 1.1f, 1.0f };

                        f倍率 = ar倍率[ this.ctゴーゴー.n現在の値 ];

                        Matrix mat = Matrix.Identity;
                        mat *= Matrix.Scaling(f倍率, f倍率, 1.0f);
                        mat *= Matrix.Translation( CDTXMania.Skin.nScrollFieldX[ i ] - SampleFramework.GameWindowSize.Width / 2.0f, -( CDTXMania.Skin.nJudgePointY[ i ] - SampleFramework.GameWindowSize.Height / 2.0f), 0f);

                        this.txゴーゴー炎.b加算合成 = true;

                        //this.ctゴーゴー.n現在の値 = 6;
                        if( this.ctゴーゴー.b終了値に達した )
                        {
                            this.txゴーゴー炎.t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldX[ i ] - 180, CDTXMania.Skin.nJudgePointY[ i ] - (this.txゴーゴー炎.szテクスチャサイズ.Height / 2), new Rectangle( 360 * ( this.ctゴーゴー炎.n現在の値 ), 0, 360, 370 ) );
                        }
                        else
                        {
                            this.txゴーゴー炎.t3D描画( CDTXMania.app.Device, mat, new Rectangle( 360 * ( this.ctゴーゴー炎.n現在の値 ), 0, 360, 370 ) );
                        }
                    }
                }
            }
            #endregion

            if( this.n総移動時間 != -1 )
            {
                if( n移動方向 == 1 )
                    CDTXMania.Skin.nScrollFieldX[0] = this.n移動開始X + (int)( ( ( (int)CSound管理.rc演奏用タイマ.n現在時刻ms - this.n移動開始時刻 ) / (double)( this.n総移動時間 ) ) * this.n移動距離px );
                else
                    CDTXMania.Skin.nScrollFieldX[0] = this.n移動開始X - (int)( ( ( (int)CSound管理.rc演奏用タイマ.n現在時刻ms - this.n移動開始時刻 ) / (double)( this.n総移動時間 ) ) * this.n移動距離px );

                if( ( (int)CSound管理.rc演奏用タイマ.n現在時刻ms ) > this.n移動開始時刻 + this.n総移動時間 )
                {
                    this.n総移動時間 = -1;
                }
            }


            if( this.tx判定枠 != null )
            {
                int nJudgeX = CDTXMania.Skin.nScrollFieldX[0] - ( 130 / 2 ); //元の値は349なんだけど...
                int nJudgeY = CDTXMania.Skin.nScrollFieldY[0]; //元の値は349なんだけど...
                this.tx判定枠.b加算合成 = true;
                this.tx判定枠.t2D描画( CDTXMania.app.Device, nJudgeX, nJudgeY, new Rectangle( 0, 0, 130, 130 ) );

                if( CDTXMania.stage演奏ドラム画面.bDoublePlay )
                    this.tx判定枠.t2D描画( CDTXMania.app.Device, nJudgeX, nJudgeY + 176, new Rectangle( 0, 0, 130, 130 ) );
            }

            for( int i = 0; i < CDTXMania.ConfigIni.nPlayerCount; i++ )
			{
				if( !this.st状態[ i ].ct進行.b停止中 )
				{
                    this.st状態[ i ].ct進行.t進行();
					if( this.st状態[ i ].ct進行.b終了値に達した )
					{
						this.st状態[ i ].ct進行.t停止();
					}
					//if( this.txアタックエフェクトLower != null )
					{
                        //this.txアタックエフェクトLower.b加算合成 = true;
                        int n = this.st状態[ i ].nIsBig == 1 ? 520 : 0;

                        switch( st状態[ i ].judge )
                        {
                            case E判定.Perfect:
                            case E判定.Great:
                            case E判定.Auto:
						        //this.txアタックエフェクトLower.t2D描画( CDTXMania.app.Device, 285, 127, new Rectangle( this.st状態[ i ].ct進行.n現在の値 * 260, n, 260, 260 ) );
                                if( this.st状態[ i ].nIsBig == 1 )
                                    this.txArアタックエフェクトLower_C[ this.st状態[ i ].ct進行.n現在の値 ].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldX[0] - this.txArアタックエフェクトLower_C[0].szテクスチャサイズ.Width / 2, CDTXMania.Skin.nJudgePointY[ i ] - this.txArアタックエフェクトLower_C[ 0 ].szテクスチャサイズ.Width / 2 );
                                else
                                    this.txArアタックエフェクトLower_A[ this.st状態[ i ].ct進行.n現在の値 ].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldX[0] - this.txArアタックエフェクトLower_A[0].szテクスチャサイズ.Width / 2, CDTXMania.Skin.nJudgePointY[ i ] - this.txArアタックエフェクトLower_A[ 0 ].szテクスチャサイズ.Width / 2 );
                                break;

                            case E判定.Good:
						        //this.txアタックエフェクトLower.t2D描画( CDTXMania.app.Device, 285, 127, new Rectangle( this.st状態[ i ].ct進行.n現在の値 * 260, n + 260, 260, 260 ) );
                                if( this.st状態[ i ].nIsBig == 1 )
                                    this.txArアタックエフェクトLower_D[ this.st状態[ i ].ct進行.n現在の値 ].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldX[0] - this.txArアタックエフェクトLower_D[0].szテクスチャサイズ.Width / 2, CDTXMania.Skin.nJudgePointY[ i ] - this.txArアタックエフェクトLower_D[ 0 ].szテクスチャサイズ.Width / 2 );
                                else
                                    this.txArアタックエフェクトLower_B[ this.st状態[ i ].ct進行.n現在の値 ].t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldX[0] - this.txArアタックエフェクトLower_B[0].szテクスチャサイズ.Width / 2, CDTXMania.Skin.nJudgePointY[ i ] - this.txArアタックエフェクトLower_B[ 0 ].szテクスチャサイズ.Width / 2 );
                                break;

                            case E判定.Miss:
                            case E判定.Bad:
                                break;
                        }
					}
				}
            }


            if( CDTXMania.ConfigIni.bAVI有効 )
            {
                this.txLane.n透明度 = 240;
                this.txLaneB.n透明度 = 240;
                this.txゴーゴー.n透明度 = 240;
            }

            //CDTXMania.act文字コンソール.tPrint(0, 0, C文字コンソール.Eフォント種別.白, this.nBranchレイヤー透明度.ToString());
            //CDTXMania.act文字コンソール.tPrint(0, 16, C文字コンソール.Eフォント種別.白, this.ct分岐アニメ進行.n現在の値.ToString());
            //CDTXMania.act文字コンソール.tPrint(0, 32, C文字コンソール.Eフォント種別.白, this.ct分岐アニメ進行.n終了値.ToString());
            return base.On進行描画();
        }


        public virtual void Start( int nLane, E判定 judge, bool b両手入力, int nPlayer )
		{
            //2017.08.15 kairera0467 排他なので番地をそのまま各レーンの状態として扱う

			//for( int n = 0; n < 1; n++ )
			{
				this.st状態[ nPlayer ].ct進行 = new CCounter( 0, 14, 10, CDTXMania.Timer );
				this.st状態[ nPlayer ].judge = judge;
                this.st状態[ nPlayer ].nPlayer = nPlayer;

                switch( nLane )
                {
                    case 0x11:
                    case 0x12:
                        this.st状態[ nPlayer ].nIsBig = 0;
                        break;
                    case 0x13:
                    case 0x14:
                    case 0x1A:
                    case 0x1B:
                        {
                            if( b両手入力 )
                                this.st状態[ nPlayer ].nIsBig = 1;
                            else
                                this.st状態[ nPlayer ].nIsBig = 0;
                        }
                        break;
                }
			}
		}

        public void GOGOSTART()
        {
            this.ctゴーゴー = new CCounter( 0, 17, 18, CDTXMania.Timer );
            //if( this.ctゴーゴー.b停止中 )
                //this.ctゴーゴー.t進行();
        }


        //public void t分岐レイヤー_コース変化( int n現在, int n次回, int nPlayer )
        //{
        //    if( n現在 == n次回 )
        //    {
        //        return;
        //    }
        //    if( CDTXMania.ConfigIni.nBranchAnime == 0 ) {
        //        this.stBranch[ nPlayer ].ct分岐アニメ進行 = new CCounter( 0, 300, 2, CDTXMania.Timer );
        //    } else if( CDTXMania.ConfigIni.nBranchAnime == 1 ) {
        //        this.stBranch[ nPlayer ].ct分岐アニメ進行 = new CCounter( 0, 260, 1, CDTXMania.Timer );
        //    }

        //    this.stBranch[ nPlayer ].nBranchレイヤー透明度 = 6;
        //    this.stBranch[ nPlayer ].nY座標 = 1;

        //    this.stBranch[ nPlayer ].nBefore = n現在;
        //    this.stBranch[ nPlayer ].nAfter = n次回;

        //    CDTXMania.stage演奏ドラム画面.actLane.t分岐レイヤー_コース変化( n現在, n次回, nPlayer );
        //}

        public void t分岐レイヤー_コース変化( int n現在, int n次回, int nPlayer )
        {
            if( n現在 == n次回 )
            {
                return;
            }

            float 速度倍率 = 1.0f; //1.0を基準とした速度。数値が1より小さくなると遅くなる。
            double 秒( double v ) => ( v / 速度倍率 );
            var animation = CDTXMania.AnimationManager;
            var basetime = animation.Timer.Time;
            var start = basetime;

            var 分岐文字現在 = this._分岐文字[ 0 + (nPlayer * 3) ];
            var 分岐文字次回 = this._分岐文字[ 1 + (nPlayer * 3) ];
            var 分岐文字次回2 = this._分岐文字[ 2 + (nPlayer * 3) ]; // 2段階アニメーションする場合に使う
            var 分岐背景 = this._分岐背景レイヤー[ nPlayer ];

            #region[ 背景レイヤーのアニメーション構築 ]
            分岐背景.Dispose();
            分岐背景.不透明度_現在 = new Variable( animation.Manager, 1.0 );
            分岐背景.不透明度_次回 = new Variable( animation.Manager, 0.0 );
            分岐背景.不透明度_次回2 = new Variable( animation.Manager, 0.0 );
            分岐背景.ストーリーボード = new Storyboard( animation.Manager );

            if( CDTXMania.ConfigIni.nBranchAnime == 0 )
            {
                // 0.3秒周期で切り替え
                switch( Math.Abs(n次回 - n現在) )
                {
                    case 1:
                        using (var 透明度変化 = animation.TrasitionLibrary.Linear(秒(0.3), 0.0))
                        using (var 透明度変化_次回 = animation.TrasitionLibrary.Linear(秒(0.3), 1.0))
                        {
                            分岐背景.ストーリーボード.AddTransition(分岐背景.不透明度_現在, 透明度変化);
                            分岐背景.ストーリーボード.AddTransition(分岐背景.不透明度_次回, 透明度変化_次回);
                        }
                        break;
                    case 2:
                        using (var 透明度変化 = animation.TrasitionLibrary.Linear(秒(0.3), 0.0))
                        using (var 透明度変化_次回 = animation.TrasitionLibrary.Linear(秒(0.3), 1.0))
                        using (var 透明度変化_次回2 = animation.TrasitionLibrary.Constant(秒(0.3)))
                        {
                            分岐背景.ストーリーボード.AddTransition(分岐背景.不透明度_現在, 透明度変化);
                            分岐背景.ストーリーボード.AddTransition(分岐背景.不透明度_次回, 透明度変化_次回);
                            分岐背景.ストーリーボード.AddTransition(分岐背景.不透明度_次回2, 透明度変化_次回2);
                        }
                        using (var 透明度変化_次回 = animation.TrasitionLibrary.Linear(秒(0.3), 0.0))
                        using (var 透明度変化_次回2 = animation.TrasitionLibrary.Linear(秒(0.3), 1.0))
                        {
                            分岐背景.ストーリーボード.AddTransition(分岐背景.不透明度_次回, 透明度変化_次回);
                            分岐背景.ストーリーボード.AddTransition(分岐背景.不透明度_次回2, 透明度変化_次回2);
                        }
                        break;
                }
            }
            分岐背景.ストーリーボード.Schedule( start );
            #endregion
            #region[ 現在の分岐文字アニメーションを構築 ]
            int n分岐文字Y基準 = CDTXMania.Skin.nScrollFieldY[ nPlayer ];

            分岐文字現在.Dispose();
            分岐文字現在.左上位置X = new Variable( animation.Manager, 0 );
            分岐文字現在.左上位置Y = new Variable( animation.Manager, n分岐文字Y基準 );
            分岐文字現在.不透明度 = new Variable( animation.Manager, 1.0 );

            分岐文字現在.ストーリーボード = new Storyboard( animation.Manager );

            if( CDTXMania.ConfigIni.nBranchAnime == 0 )
            {
                switch( n次回 - n現在 )
                {
                    case 1:
                    case 2:
                        // 1段階、2段階レベルアップ
                        using (var 文字Y移動 = animation.TrasitionLibrary.AccelerateDecelerate(秒(0.07), n分岐文字Y基準 - 26, 0.7, 0.3))
                        using (var 透明度変化 = animation.TrasitionLibrary.Linear(秒(0.07), 1.0))
                        {
                            分岐文字現在.ストーリーボード.AddTransition(分岐文字現在.左上位置Y, 文字Y移動);
                            分岐文字現在.ストーリーボード.AddTransition(分岐文字現在.不透明度, 透明度変化);
                        }

                        using (var 文字Y移動 = animation.TrasitionLibrary.AccelerateDecelerate(秒(0.14), n分岐文字Y基準 + 34, 0.2, 0.8))
                        using (var 透明度変化 = animation.TrasitionLibrary.Linear(秒(0.14), 0.0))
                        {
                            分岐文字現在.ストーリーボード.AddTransition(分岐文字現在.左上位置Y, 文字Y移動);
                            分岐文字現在.ストーリーボード.AddTransition(分岐文字現在.不透明度, 透明度変化);
                        }
                        break;
                    case -1:
                    case -2:
                        // 1段階、2段階レベルダウン
                        using (var 文字Y移動 = animation.TrasitionLibrary.AccelerateDecelerate(秒(0.07), n分岐文字Y基準 + 26, 0.7, 0.3))
                        using (var 透明度変化 = animation.TrasitionLibrary.Linear(秒(0.07), 1.0))
                        {
                            分岐文字現在.ストーリーボード.AddTransition(分岐文字現在.左上位置Y, 文字Y移動);
                            分岐文字現在.ストーリーボード.AddTransition(分岐文字現在.不透明度, 透明度変化);
                        }

                        using (var 文字Y移動 = animation.TrasitionLibrary.AccelerateDecelerate(秒(0.14), n分岐文字Y基準 - 34, 0.2, 0.8))
                        using (var 透明度変化 = animation.TrasitionLibrary.Linear(秒(0.14), 0.0))
                        {
                            分岐文字現在.ストーリーボード.AddTransition(分岐文字現在.左上位置Y, 文字Y移動);
                            分岐文字現在.ストーリーボード.AddTransition(分岐文字現在.不透明度, 透明度変化);
                        }
                        break;
                    default:
                        break;
                }
                    
            }
            else if( CDTXMania.ConfigIni.nBranchAnime == 1 )
            {
                switch( n次回 - n現在 )
                {
                    case 1:
                    case 2:
                        // レベルアップ
                        using (var 文字Y移動 = animation.TrasitionLibrary.AccelerateDecelerate(秒(0.1), n分岐文字Y基準 + 12, 0.7, 0.3))
                        using (var 透明度変化 = animation.TrasitionLibrary.Constant(秒(0.1)))
                        {
                            分岐文字現在.ストーリーボード.AddTransition(分岐文字現在.左上位置Y, 文字Y移動);
                            分岐文字現在.ストーリーボード.AddTransition(分岐文字現在.不透明度, 透明度変化);
                        }

                        using (var 文字Y移動 = animation.TrasitionLibrary.AccelerateDecelerate(秒(0.12), n分岐文字Y基準 - 36, 0.2, 0.8))
                        using (var 透明度変化 = animation.TrasitionLibrary.Linear(秒(0.12), 0.0))
                        {
                            分岐文字現在.ストーリーボード.AddTransition(分岐文字現在.左上位置Y, 文字Y移動);
                            分岐文字現在.ストーリーボード.AddTransition(分岐文字現在.不透明度, 透明度変化);
                        }
                        break;
                    case -1:
                    case -2:
                        // レベルダウン
                        using (var 文字Y移動 = animation.TrasitionLibrary.AccelerateDecelerate(秒(0.07), n分岐文字Y基準 + 26, 0.7, 0.3))
                        using (var 透明度変化 = animation.TrasitionLibrary.Linear(秒(0.07), 1.0))
                        {
                            分岐文字現在.ストーリーボード.AddTransition(分岐文字現在.左上位置Y, 文字Y移動);
                            分岐文字現在.ストーリーボード.AddTransition(分岐文字現在.不透明度, 透明度変化);
                        }

                        using (var 文字Y移動 = animation.TrasitionLibrary.AccelerateDecelerate(秒(0.14), n分岐文字Y基準 - 34, 0.2, 0.8))
                        using (var 透明度変化 = animation.TrasitionLibrary.Linear(秒(0.14), 0.0))
                        {
                            分岐文字現在.ストーリーボード.AddTransition(分岐文字現在.左上位置Y, 文字Y移動);
                            分岐文字現在.ストーリーボード.AddTransition(分岐文字現在.不透明度, 透明度変化);
                        }
                        break;
                    default:
                        break;
                }
            }
            分岐文字現在.ストーリーボード.Schedule( start );
            #endregion

            #region[ 次回の分岐文字アニメーションを構築 ]

            分岐文字次回.Dispose();
            分岐文字次回.左上位置X = new Variable( animation.Manager, 0 );
            //分岐文字次回.左上位置Y = new Variable( animation.Manager, n分岐文字Y基準 - 40 );
            分岐文字次回.不透明度 = new Variable( animation.Manager, 0.0 );

            分岐文字次回.ストーリーボード = new Storyboard( animation.Manager );

            if( CDTXMania.ConfigIni.nBranchAnime == 0 )
            {
                start = basetime + 秒(0.10);
                switch( n次回 - n現在 )
                {
                    case 1:
                        // 1段階レベルアップ
                        分岐文字次回.左上位置Y = new Variable( animation.Manager, n分岐文字Y基準 - 40 );
                        using (var 文字Y移動 = animation.TrasitionLibrary.AccelerateDecelerate(秒(0.12), n分岐文字Y基準, 0.3, 0.7))
                        using (var 透明度変化 = animation.TrasitionLibrary.Linear(秒(0.12), 1.0))
                        {
                            分岐文字次回.ストーリーボード.AddTransition(分岐文字次回.左上位置Y, 文字Y移動);
                            分岐文字次回.ストーリーボード.AddTransition(分岐文字次回.不透明度, 透明度変化);
                        }

                        using (var 文字Y移動 = animation.TrasitionLibrary.Linear(秒(0.04), n分岐文字Y基準 - 4))
                            分岐文字次回.ストーリーボード.AddTransition(分岐文字次回.左上位置Y, 文字Y移動);

                        using (var 文字Y移動 = animation.TrasitionLibrary.Linear(秒(0.04), n分岐文字Y基準))
                            分岐文字次回.ストーリーボード.AddTransition(分岐文字次回.左上位置Y, 文字Y移動);
                        break;
                    case 2:
                        // 2段階レベルアップ
                        分岐文字次回.左上位置Y = new Variable( animation.Manager, n分岐文字Y基準 - 40 );
                        using (var 文字Y移動 = animation.TrasitionLibrary.AccelerateDecelerate(秒(0.12), n分岐文字Y基準, 0.3, 0.7))
                        using (var 透明度変化 = animation.TrasitionLibrary.Linear(秒(0.12), 1.0))
                        {
                            分岐文字次回.ストーリーボード.AddTransition(分岐文字次回.左上位置Y, 文字Y移動);
                            分岐文字次回.ストーリーボード.AddTransition(分岐文字次回.不透明度, 透明度変化);
                        }

                        using (var 文字Y移動 = animation.TrasitionLibrary.AccelerateDecelerate(秒(0.07), n分岐文字Y基準 - 26, 0.7, 0.3))
                        using (var 透明度変化 = animation.TrasitionLibrary.Linear(秒(0.07), 1.0))
                        {
                            分岐文字次回.ストーリーボード.AddTransition(分岐文字次回.左上位置Y, 文字Y移動);
                            分岐文字次回.ストーリーボード.AddTransition(分岐文字次回.不透明度, 透明度変化);
                        }

                        using (var 文字Y移動 = animation.TrasitionLibrary.AccelerateDecelerate(秒(0.14), n分岐文字Y基準 + 34, 0.2, 0.8))
                        using (var 透明度変化 = animation.TrasitionLibrary.Linear(秒(0.14), 0.0))
                        {
                            分岐文字次回.ストーリーボード.AddTransition(分岐文字次回.左上位置Y, 文字Y移動);
                            分岐文字次回.ストーリーボード.AddTransition(分岐文字次回.不透明度, 透明度変化);
                        }
                        break;
                    case -1:
                        // 1段階レベルダウン
                        分岐文字次回.左上位置Y = new Variable( animation.Manager, n分岐文字Y基準 + 40 );
                        using (var 文字Y移動 = animation.TrasitionLibrary.AccelerateDecelerate(秒(0.12), n分岐文字Y基準, 0.3, 0.7))
                        using (var 透明度変化 = animation.TrasitionLibrary.Linear(秒(0.12), 1.0))
                        {
                            分岐文字次回.ストーリーボード.AddTransition(分岐文字次回.左上位置Y, 文字Y移動);
                            分岐文字次回.ストーリーボード.AddTransition(分岐文字次回.不透明度, 透明度変化);
                        }

                        using (var 文字Y移動 = animation.TrasitionLibrary.Linear(秒(0.04), n分岐文字Y基準 - 4))
                            分岐文字次回.ストーリーボード.AddTransition(分岐文字次回.左上位置Y, 文字Y移動);

                        using (var 文字Y移動 = animation.TrasitionLibrary.Linear(秒(0.04), n分岐文字Y基準))
                            分岐文字次回.ストーリーボード.AddTransition(分岐文字次回.左上位置Y, 文字Y移動);
                        break;
                    case -2:
                        // 2段階レベルダウン
                        分岐文字次回.左上位置Y = new Variable( animation.Manager, n分岐文字Y基準 + 40 );
                        using (var 文字Y移動 = animation.TrasitionLibrary.AccelerateDecelerate(秒(0.12), n分岐文字Y基準, 0.3, 0.7))
                        using (var 透明度変化 = animation.TrasitionLibrary.Linear(秒(0.12), 1.0))
                        {
                            分岐文字次回.ストーリーボード.AddTransition(分岐文字次回.左上位置Y, 文字Y移動);
                            分岐文字次回.ストーリーボード.AddTransition(分岐文字次回.不透明度, 透明度変化);
                        }
                        
                        using (var 文字Y移動 = animation.TrasitionLibrary.AccelerateDecelerate(秒(0.07), n分岐文字Y基準 + 26, 0.7, 0.3))
                        using (var 透明度変化 = animation.TrasitionLibrary.Linear(秒(0.07), 1.0))
                        {
                            分岐文字次回.ストーリーボード.AddTransition(分岐文字次回.左上位置Y, 文字Y移動);
                            分岐文字次回.ストーリーボード.AddTransition(分岐文字次回.不透明度, 透明度変化);
                        }

                        using (var 文字Y移動 = animation.TrasitionLibrary.AccelerateDecelerate(秒(0.14), n分岐文字Y基準 - 34, 0.2, 0.8))
                        using (var 透明度変化 = animation.TrasitionLibrary.Linear(秒(0.14), 0.0))
                        {
                            分岐文字次回.ストーリーボード.AddTransition(分岐文字次回.左上位置Y, 文字Y移動);
                            分岐文字次回.ストーリーボード.AddTransition(分岐文字次回.不透明度, 透明度変化);
                        }
                        break;
                    default:
                        分岐文字次回.左上位置Y = new Variable(animation.Manager, n分岐文字Y基準);
                        break;
                }
                    
            }
            else if( CDTXMania.ConfigIni.nBranchAnime == 1 )
            {
                start = basetime + 秒(0.10);
                switch( n次回 - n現在 )
                {
                    case 1:
                        // 1段階レベルアップ
                        分岐文字次回.左上位置Y = new Variable( animation.Manager, n分岐文字Y基準 - 40 );
                        using (var 文字Y移動 = animation.TrasitionLibrary.AccelerateDecelerate(秒(0.12), n分岐文字Y基準, 0.3, 0.7))
                        using (var 透明度変化 = animation.TrasitionLibrary.Linear(秒(0.12), 1.0))
                        {
                            分岐文字次回.ストーリーボード.AddTransition(分岐文字次回.左上位置Y, 文字Y移動);
                            分岐文字次回.ストーリーボード.AddTransition(分岐文字次回.不透明度, 透明度変化);
                        }

                        using (var 文字Y移動 = animation.TrasitionLibrary.Linear(秒(0.04), n分岐文字Y基準 - 4))
                            分岐文字次回.ストーリーボード.AddTransition(分岐文字次回.左上位置Y, 文字Y移動);

                        using (var 文字Y移動 = animation.TrasitionLibrary.Linear(秒(0.04), n分岐文字Y基準))
                            分岐文字次回.ストーリーボード.AddTransition(分岐文字次回.左上位置Y, 文字Y移動);
                        break;
                    case -1:
                        // 1段階レベルダウン
                        分岐文字次回.左上位置Y = new Variable( animation.Manager, n分岐文字Y基準 + 40 );
                        using (var 文字Y移動 = animation.TrasitionLibrary.AccelerateDecelerate(秒(0.12), n分岐文字Y基準, 0.3, 0.7))
                        using (var 透明度変化 = animation.TrasitionLibrary.Linear(秒(0.12), 1.0))
                        {
                            分岐文字次回.ストーリーボード.AddTransition(分岐文字次回.左上位置Y, 文字Y移動);
                            分岐文字次回.ストーリーボード.AddTransition(分岐文字次回.不透明度, 透明度変化);
                        }

                        using (var 文字Y移動 = animation.TrasitionLibrary.Linear(秒(0.04), n分岐文字Y基準 - 4))
                            分岐文字次回.ストーリーボード.AddTransition(分岐文字次回.左上位置Y, 文字Y移動);

                        using (var 文字Y移動 = animation.TrasitionLibrary.Linear(秒(0.04), n分岐文字Y基準))
                            分岐文字次回.ストーリーボード.AddTransition(分岐文字次回.左上位置Y, 文字Y移動);
                        break;
                    case -2:
                        // 2段階レベルダウン
                        分岐文字次回.左上位置Y = new Variable( animation.Manager, n分岐文字Y基準 + 40 );
                        using (var 文字Y移動 = animation.TrasitionLibrary.AccelerateDecelerate(秒(0.12), n分岐文字Y基準, 0.3, 0.7))
                        using (var 透明度変化 = animation.TrasitionLibrary.Linear(秒(0.12), 1.0))
                        {
                            分岐文字次回.ストーリーボード.AddTransition(分岐文字次回.左上位置Y, 文字Y移動);
                            分岐文字次回.ストーリーボード.AddTransition(分岐文字次回.不透明度, 透明度変化);
                        }
                        
                        using (var 文字Y移動 = animation.TrasitionLibrary.AccelerateDecelerate(秒(0.07), n分岐文字Y基準 + 26, 0.7, 0.3))
                        using (var 透明度変化 = animation.TrasitionLibrary.Linear(秒(0.07), 1.0))
                        {
                            分岐文字次回.ストーリーボード.AddTransition(分岐文字次回.左上位置Y, 文字Y移動);
                            分岐文字次回.ストーリーボード.AddTransition(分岐文字次回.不透明度, 透明度変化);
                        }

                        using (var 文字Y移動 = animation.TrasitionLibrary.AccelerateDecelerate(秒(0.14), n分岐文字Y基準 - 34, 0.2, 0.8))
                        using (var 透明度変化 = animation.TrasitionLibrary.Linear(秒(0.14), 0.0))
                        {
                            分岐文字次回.ストーリーボード.AddTransition(分岐文字次回.左上位置Y, 文字Y移動);
                            分岐文字次回.ストーリーボード.AddTransition(分岐文字次回.不透明度, 透明度変化);
                        }
                        break;
                    default:
                        分岐文字次回.左上位置Y = new Variable(animation.Manager, n分岐文字Y基準);
                        break;
                }
            }
            分岐文字次回.ストーリーボード.Schedule( start );
            #endregion
            #region[ 2つ先の分岐文字アニメーションを構築 ]

            分岐文字次回2.Dispose();
            分岐文字次回2.左上位置X = new Variable( animation.Manager, 0 );
            分岐文字次回2.不透明度 = new Variable( animation.Manager, 0.0 );
            分岐文字次回2.ストーリーボード = new Storyboard( animation.Manager );

            if( CDTXMania.ConfigIni.nBranchAnime == 0 )
            {
                start = basetime + 秒(0.30);
                switch( n次回 - n現在 )
                {
                    case 2:
                        // 2段階レベルアップ
                        分岐文字次回2.左上位置Y = new Variable( animation.Manager, n分岐文字Y基準 - 40 );

                        using (var 文字Y移動 = animation.TrasitionLibrary.AccelerateDecelerate(秒(0.12), n分岐文字Y基準, 0.3, 0.7))
                        using (var 透明度変化 = animation.TrasitionLibrary.Linear(秒(0.12), 1.0))
                        {
                            分岐文字次回2.ストーリーボード.AddTransition(分岐文字次回2.左上位置Y, 文字Y移動);
                            分岐文字次回2.ストーリーボード.AddTransition(分岐文字次回2.不透明度, 透明度変化);
                        }

                        using (var 文字Y移動 = animation.TrasitionLibrary.Linear(秒(0.04), n分岐文字Y基準 - 4))
                            分岐文字次回2.ストーリーボード.AddTransition(分岐文字次回2.左上位置Y, 文字Y移動);

                        using (var 文字Y移動 = animation.TrasitionLibrary.Linear(秒(0.04), n分岐文字Y基準))
                            分岐文字次回2.ストーリーボード.AddTransition(分岐文字次回2.左上位置Y, 文字Y移動);
                        break;
                    case -2:
                        // 2段階レベルダウン
                        分岐文字次回2.左上位置Y = new Variable( animation.Manager, n分岐文字Y基準 + 40 );

                        using (var 文字Y移動 = animation.TrasitionLibrary.AccelerateDecelerate(秒(0.12), n分岐文字Y基準, 0.3, 0.7))
                        using (var 透明度変化 = animation.TrasitionLibrary.Linear(秒(0.12), 1.0))
                        {
                            分岐文字次回2.ストーリーボード.AddTransition(分岐文字次回2.左上位置Y, 文字Y移動);
                            分岐文字次回2.ストーリーボード.AddTransition(分岐文字次回2.不透明度, 透明度変化);
                        }

                        using (var 文字Y移動 = animation.TrasitionLibrary.Linear(秒(0.04), n分岐文字Y基準 - 4))
                            分岐文字次回2.ストーリーボード.AddTransition(分岐文字次回2.左上位置Y, 文字Y移動);

                        using (var 文字Y移動 = animation.TrasitionLibrary.Linear(秒(0.04), n分岐文字Y基準))
                            分岐文字次回2.ストーリーボード.AddTransition(分岐文字次回2.左上位置Y, 文字Y移動);
                        break;
                    case 1:
                    case -1:
                        // 1段階レベルアップまたは1段階レベルダウンの場合は最小限のエラー防止処理だけを行う
                    default:
                        分岐文字次回2.左上位置Y = new Variable( animation.Manager, n分岐文字Y基準 );
                        break;
                }
                    
            }
            分岐文字次回2.ストーリーボード.Schedule( start );
            #endregion

            if( CDTXMania.ConfigIni.nBranchAnime == 0 ) {
                //this.stBranch[ nPlayer ].ct分岐アニメ進行 = new CCounter( 0, 230, 2, CDTXMania.Timer );
            } else if( CDTXMania.ConfigIni.nBranchAnime == 1 ) {
                this.stBranch[ nPlayer ].ct分岐アニメ進行 = new CCounter( 0, 260, 1, CDTXMania.Timer );
            }

            this.stBranch[ nPlayer ].nBranchレイヤー透明度 = 6;
            this.stBranch[ nPlayer ].nY座標 = 1;

            this.stBranch[ nPlayer ].nBefore = n現在;
            this.stBranch[ nPlayer ].nAfter = n次回;

            CDTXMania.stage演奏ドラム画面.actLane.t分岐レイヤー_コース変化( n現在, n次回, nPlayer );
        }

        public void t判定枠移動( double db移動時間, int n移動px, int n移動方向 )
        {
            this.n移動開始時刻 = (int)CSound管理.rc演奏用タイマ.n現在時刻ms;
            this.n移動開始X = CDTXMania.Skin.nScrollFieldX[0];
            this.n総移動時間 = (int)( db移動時間 * 1000 );
            this.n移動方向 = n移動方向;
            this.n移動距離px = n移動px;
        }


        #region[ private ]
        //-----------------
        private CTexture txLane;
        private CTexture txLaneB;
        private CTexture tx枠線;
        private CTexture tx判定枠;
        private CTexture txゴーゴー;
        private CTexture txゴーゴー炎;
        private CTexture[] txArゴーゴー炎;
        private CTexture[] txArアタックエフェクトLower_A;
        private CTexture[] txArアタックエフェクトLower_B;
        private CTexture[] txArアタックエフェクトLower_C;
        private CTexture[] txArアタックエフェクトLower_D;

        private CTexture[] txLaneFlush = new CTexture[3];

        private CTexture[] tx普通譜面 = new CTexture[2];
        private CTexture[] tx玄人譜面 = new CTexture[2];
        private CTexture[] tx達人譜面 = new CTexture[2];

        private CTextureAf txアタックエフェクトLower;

        protected STSTATUS[] st状態 = new STSTATUS[4];

        [StructLayout(LayoutKind.Sequential)]
        protected struct STSTATUS
        {
            public bool b使用中;
            public CCounter ct進行;
            public E判定 judge;
            public int nIsBig;
            public int n透明度;
            public int nPlayer;
        }
        private CCounter ctゴーゴー;
        private CCounter ctゴーゴー炎;


        protected STBRANCH[] stBranch = new STBRANCH[ 4 ];
        [StructLayout(LayoutKind.Sequential)]
        protected struct STBRANCH
        {
            public CCounter ct分岐アニメ進行;
            public int nBefore;
            public int nAfter;

            public long nフラッシュ制御タイマ;
            public int nBranchレイヤー透明度;
            public int nBranch文字透明度;
            public int nY座標;
            public int nY;
        }


        private int n総移動時間;
        private int n移動開始X;
        private int n移動開始Y;
        private int n移動開始時刻;
        private int n移動距離px;
        private int n移動方向;

        private int[] nDefaultJudgePos = new int[ 2 ];

        // 2019.2.11 kairera0467
        protected class 分岐文字 : IDisposable
        {
            public Variable 左上位置X;
            public Variable 左上位置Y;
            public Variable 不透明度;
            public Storyboard ストーリーボード;

            public void Dispose()
            {
                this.ストーリーボード?.Abandon();
                this.ストーリーボード = null;

                this.左上位置X?.Dispose();
                this.左上位置X = null;

                this.左上位置Y?.Dispose();
                this.左上位置Y = null;

                this.不透明度?.Dispose();
                this.不透明度 = null;
            }
        }
        protected 分岐文字[] _分岐文字 = null;
        protected class 分岐背景レイヤー : IDisposable
        {
            public Variable 左上位置X;
            public Variable 左上位置Y;
            public Variable 不透明度_現在;
            public Variable 不透明度_次回;
            public Variable 不透明度_次回2;
            public Storyboard ストーリーボード;

            public void Dispose()
            {
                this.ストーリーボード?.Abandon();
                this.ストーリーボード = null;

                this.左上位置X?.Dispose();
                this.左上位置X = null;

                this.左上位置Y?.Dispose();
                this.左上位置Y = null;

                this.不透明度_現在?.Dispose();
                this.不透明度_現在 = null;

                this.不透明度_次回?.Dispose();
                this.不透明度_次回 = null;

                this.不透明度_次回2?.Dispose();
                this.不透明度_次回2 = null;
            }
        }
        protected 分岐背景レイヤー[] _分岐背景レイヤー = null;
        protected bool b分岐アニメ実行中( int player )
        {
            if( this._分岐背景レイヤー[ player ].ストーリーボード != null )
            {
                if( this._分岐背景レイヤー[ player ].ストーリーボード.Status == StoryboardStatus.Playing ) return true;
                else return false;
            }
            else
            {
                return false;
            }
        }
        //-----------------
        #endregion
    }
}
