using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Drawing.Text;
using SharpDX.Animation;

using SlimDX;
using FDK;

namespace DTXMania
{
    /// <summary>
    /// 難易度選択画面。
    /// </summary>
	internal class CActSelect難易度選択画面 : CActivity
	{
		// プロパティ

		public bool bスクロール中
		{
			get
			{
				if( this.n目標のスクロールカウンタ == 0 )
				{
					return ( this.n現在のスクロールカウンタ != 0 );
				}
				return true;
			}
		}
        public bool bIsDifficltSelect;

		// コンストラクタ

        public CActSelect難易度選択画面()
        {
			base.b活性化してない = true;
		}


		// メソッド
        public int t指定した方向に近い難易度番号を返す( int nDIRECTION, int pos )
        {
            if( nDIRECTION == 0)
            {
                for( int i = pos; i < 5; i++ )
                {
                    if( i == pos ) continue;
                    if( CDTXMania.stage選曲.r現在選択中の曲.arスコア[ i ] != null ) return i;
                    if( i == 4 ) return this.t指定した方向に近い難易度番号を返す( 0, 0 );
                }
            }
            else
            {
                for( int i = pos; i > -1; i-- )
                {
                    if( pos == i ) continue;
                    if( CDTXMania.stage選曲.r現在選択中の曲.arスコア[ i ] != null ) return i;
                    if( i == 0 ) return this.t指定した方向に近い難易度番号を返す( 1, 4 );
                }
            }
            return pos;
        }
        
        public void t次に移動( int player )
		{
            if (this.n現在の選択行[ player ] < this.list難易度選択項目.Count - 1)
            {
                this.n現在の選択行[ player ] += 1;
            }

            this.ct移動 = new CCounter( 1, 710, 1, CSound管理.rc演奏用タイマ );

            #region[ ストーリーボード構築 ]
            float 速度倍率 = 1.0f; //1.0を基準とした速度。数値が1より小さくなると遅くなる。
            //double 秒( double v ) => ( v / 速度倍率 );
            //var animation = CDTXMania.AnimationManager;
            //var basetime = animation.Timer.Time;
            //var start = basetime;

            //// 表示中のスコアボード(カーソル種類)切り替え
            //// 0.1秒 カーソルを次の位置へ移動
            //var 直前の選択項目 = this.list難易度選択項目[ this.n現在の選択行[ player ] - 1 ];
            //var 次の選択項目 = this.list難易度選択項目[ this.n現在の選択行[ player ] ];

            //var 直前のパネル位置中心X = 直前の選択項目.ptパネル座標.X + 直前の選択項目.txパネル.szテクスチャサイズ.Width / 2;
            //var 次のパネル位置中心X = 次の選択項目.ptパネル座標.X + 次の選択項目.txパネル.szテクスチャサイズ.Width / 2;
            
            //var カーソル = this._プレイヤーカーソル[ player ];
            //var カーソルの幅 = カーソル.txカーソル.szテクスチャサイズ.Width / 2;

            //カーソル.Dispose();
            //カーソル.枠左上位置X = new Variable( animation.Manager, 0.0 );
            //カーソル.枠左上位置Y = new Variable( animation.Manager, 0.0 );
            //カーソル.枠不透明度 = new Variable( animation.Manager, 0.0 );
            //カーソル.吹き出し左上位置X = new Variable( animation.Manager, 0 );
            //カーソル.吹き出し左上位置Y = new Variable( animation.Manager, 4.0 );
            //カーソル.ストーリーボード = new Storyboard( animation.Manager );

            //Trace.WriteLine( "現在の選択行:" + this.n現在の選択行[ player ] );
            //Trace.WriteLine( "吹き出し位置初期値X:" + (直前のパネル位置中心X) );
            //Trace.WriteLine( "吹き出し位置移動先X:" + (次のパネル位置中心X) );

            //using (var 枠座標移動X = animation.TrasitionLibrary.Linear(秒(0.1), 次のパネル位置中心X - 直前のパネル位置中心X))
            //{
            //    カーソル.ストーリーボード.AddTransition(カーソル.吹き出し左上位置X, 枠座標移動X);
            //}
            //カーソル.ストーリーボード.Schedule( start );
            #endregion
        }
		public void t前に移動( int player )
		{
            if( this.n現在の選択行[ player ] > 0 )
            {
                this.n現在の選択行[ player ] -= 1;
            }

            this.ct移動 = new CCounter( 1, 710, 1, CSound管理.rc演奏用タイマ );

            #region[ ストーリーボード構築 ]
            #endregion
        }
        public void t曲を確定()
        {
            #region[ ストーリーボード構築 ]
            // ToDo:画像を白くする処理は別テクスチャ(自動生成)
            // 0.20 画像コントラストを最大まで上げる(白くする)
            // 0.20 画像コントラストを戻す、透明度を最小まで下げる
            #endregion
        }
        public void t選択画面初期化()
		{
            this.b初めての進行描画 = true;
		}

		// CActivity 実装

		public override void On活性化()
		{
			if( this.b活性化してる )
				return;

			this.b登場アニメ全部完了 = false;
			this.n目標のスクロールカウンタ = 0;
			this.n現在のスクロールカウンタ = 0;
			this.nスクロールタイマ = -1;
            
            this.ct三角矢印アニメ = new CCounter();
            this.ct移動 = new CCounter();
            this.ct譜面分岐 = new CCounter();

            this.n現在の選択行 = new int[ 2 ];

            this._プレイヤーカーソル = new プレイヤーカーソル[]
            {
                new プレイヤーカーソル(){ txカーソル = CDTXMania.tテクスチャの生成( CSkin.Path(@"Graphics\5_player cursor p1.png") ) },
                new プレイヤーカーソル(){ txカーソル = CDTXMania.tテクスチャの生成( CSkin.Path(@"Graphics\5_player cursor p2.png") ) }
            };

			base.On活性化();
		}
		public override void On非活性化()
		{
			if( this.b活性化してない )
				return;

            this.ct移動 = null;
            this.ct三角矢印アニメ = null;
            this.ct譜面分岐 = null;

            foreach( var p in this._プレイヤーカーソル )
            {
                CDTXMania.tテクスチャの解放( ref p.txカーソル );
                p.Dispose();
            }
            this._プレイヤーカーソル = null;

			base.On非活性化();
		}
		public override void OnManagedリソースの作成()
		{
			if( this.b活性化してない )
				return;

            this.tx背景 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_diffselect_background.png" ) );
            this.txヘッダー = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_diffselect_header_panel.png" ) );
            this.txフッター = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_footer panel.png" ) );

            this.txカーソル大 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_diff_coursol1.png" ) );
            this.txカーソル小 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_diff_coursol2.png" ) );

            this.tx説明背景 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_information_BG.png" ) );
            this.tx説明1 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_information.png" ) );

            this.txレベル星 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_diffboard_star.png" ) );
            this.tx譜面分岐 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_diffboard_branch.png" ) );

            this.soundSelectAnnounce = CDTXMania.Sound管理.tサウンドを生成する( CSkin.Path( @"Sounds\DiffSelect.ogg" ) );

            this.list難易度選択項目 = new List<ST難易度選択項目>();
            if( this.list難易度選択項目 != null )
            {
                for( int i = 0; i < 8; i++ )
                {
                    // 項目リストを作る
                    ST難易度選択項目 stDiffList = new ST難易度選択項目();

                    switch( i )
                    {
                        case 0:
                            // 戻る
                            stDiffList.b選択可 = true;
                            stDiffList.str項目名 = "back";
                            stDiffList.e項目種類 = E項目種類.戻る;
                            stDiffList.ptパネル座標 = new Point( 249, 114 );
                            stDiffList.rectパネル位置 = new Rectangle( 0, 0, 70, 310 );
                            stDiffList.txパネル = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_item_back.png" ) );
                            this.list難易度選択項目.Add( stDiffList );

                            break;
                        case 1:
                            // オプション
                            stDiffList.b選択可 = false;
                            stDiffList.str項目名 = "option";
                            stDiffList.e項目種類 = E項目種類.オプション;
                            stDiffList.ptパネル座標 = new Point( 319, 114 );
                            stDiffList.rectパネル位置 = new Rectangle( 0, 0, 70, 310 );
                            stDiffList.txパネル = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_item_option.png" ) );
                            this.list難易度選択項目.Add( stDiffList );
                            break;
                        case 2:
                            // 音色
                            stDiffList.b選択可 = false;
                            stDiffList.str項目名 = "se";
                            stDiffList.e項目種類 = E項目種類.音色;
                            stDiffList.ptパネル座標 = new Point( 389, 114 );
                            stDiffList.rectパネル位置 = new Rectangle( 0, 0, 70, 310 );
                            stDiffList.txパネル = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_item_se.png" ) );
                            this.list難易度選択項目.Add( stDiffList );
                            break;
                        case 3:
                            stDiffList.b選択可 = false;
                            stDiffList.e項目種類 = E項目種類.かんたん;
                            stDiffList.str項目名 = "Diff:0";
                            stDiffList.ptパネル座標 = new Point( 450, 80 );
                            stDiffList.rectパネル位置 = new Rectangle( 0, 0, 0, 0 );

                            this.list難易度選択項目.Add( stDiffList );
                            break;
                        case 4:
                            stDiffList.b選択可 = false;
                            stDiffList.e項目種類 = E項目種類.ふつう;
                            stDiffList.str項目名 = "Diff:1";
                            stDiffList.ptパネル座標 = new Point( 550, 80 );
                            stDiffList.rectパネル位置 = new Rectangle( 0, 0, 0, 0 );

                            this.list難易度選択項目.Add( stDiffList );
                            break;
                        case 5:
                            stDiffList.b選択可 = false;
                            stDiffList.e項目種類 = E項目種類.むずかしい;
                            stDiffList.str項目名 = "Diff:2";
                            stDiffList.ptパネル座標 = new Point( 650, 0 );
                            stDiffList.rectパネル位置 = new Rectangle( 0, 0, 0, 0 );

                            this.list難易度選択項目.Add( stDiffList );
                            break;
                        case 6:
                            stDiffList.b選択可 = false;
                            stDiffList.e項目種類 = E項目種類.おに;
                            stDiffList.str項目名 = "Diff:3";
                            stDiffList.ptパネル座標 = new Point( 750, 0 );
                            stDiffList.rectパネル位置 = new Rectangle( 0, 0, 0, 0 );

                            this.list難易度選択項目.Add( stDiffList );
                            break;
                        //case 7:
                        //    stDiffList.b選択可 = CDTXMania.stage選曲.act曲リスト.r現在選択中の曲.arスコア[ 4 ] != null ? true : false;
                        //    stDiffList.e項目種類 = E項目種類.エディット;
                        //    stDiffList.str項目名 = "Diff:4";
                        //    stDiffList.ptパネル座標 = new Point( 850, 0 );
                        //    stDiffList.rectパネル位置 = new Rectangle( 0, 0, 0, 0 );

                        //    this.list難易度選択項目.Add( stDiffList );
                        //    break;
                    }



                }
            }

			base.OnManagedリソースの作成();
		}
		public override void OnManagedリソースの解放()
		{
			if( this.b活性化してない )
				return;

            CDTXMania.tテクスチャの解放( ref this.tx背景 );
            CDTXMania.tテクスチャの解放( ref this.txヘッダー );
            CDTXMania.tテクスチャの解放( ref this.txフッター );

            CDTXMania.tテクスチャの解放( ref this.tx説明背景 );
            CDTXMania.tテクスチャの解放( ref this.tx説明1 );

            this.tx譜面分岐?.Dispose();
            this.txレベル星?.Dispose();

            this.txカーソル大?.Dispose();
            this.txカーソル小?.Dispose();

            CDTXMania.t安全にDisposeする( ref this.soundSelectAnnounce );

            foreach( var item in this.list難易度選択項目 )
            {
                item.txパネル?.Dispose();
            }

            this.list難易度選択項目?.Clear();

			base.OnManagedリソースの解放();
		}
		public override int On進行描画()
		{
			if( this.b活性化してない )
				return 0;

#region [ 初めての進行描画 ]
			//-----------------
			if( this.b初めての進行描画 )
			{
                this.ct譜面分岐.t開始( 1, 200, 10, CDTXMania.Timer );
				this.nスクロールタイマ = CSound管理.rc演奏用タイマ.n現在時刻;

                this.n矢印スクロール用タイマ値 = CSound管理.rc演奏用タイマ.n現在時刻;
				this.ct三角矢印アニメ.t開始( 0, 19, 40, CDTXMania.Timer );

                // 現在位置をかんたん～おに(エディット)の間に移動させる
#if DEBUG
                this.n現在の選択行[ 0 ] = 0;
#else
                this.n現在の選択行[ 0 ] = 3 + CDTXMania.stage選曲.act曲リスト.n現在選択中の曲の現在の難易度レベル;
#endif

                Point[] ptパネル座標 = new Point[]
                {
                    new Point( 450, 84 ),
                    new Point( 550, 84 ),
                    new Point( 650, 84 ),
                    new Point( 750, 84 ),
                    new Point( 850, 84 )
                };

                for( int j = 3; j < 7; j++ )
                {
                    ST難易度選択項目 stDiffList = new ST難易度選択項目();

                    stDiffList.b選択可 = CDTXMania.stage選曲.act曲リスト.r現在選択中の曲.arスコア[ j - 3 ] != null ? true : false;
                    stDiffList.b譜面分岐 = CDTXMania.stage選曲.act曲リスト.r現在選択中の曲.arスコア[ j - 3 ] != null ? CDTXMania.stage選曲.act曲リスト.r現在選択中の曲.arスコア[ j - 3 ].譜面情報.b譜面分岐[ j - 3 ] : false;
                    stDiffList.e項目種類 = (E項目種類)(j - 3);
                    stDiffList.str項目名 = "Diff:" + j;
                    stDiffList.ptパネル座標 = ptパネル座標[ j - 3 ];
                    stDiffList.rectパネル位置 = new Rectangle( 0, 0, 102, 530 );
                    stDiffList.txパネル = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_diffboard_"+ (j - 2) +".png" ) );

                    this.list難易度選択項目[j] = stDiffList;
                }

                this.soundSelectAnnounce?.tサウンドを再生する();

				base.b初めての進行描画 = false;
			}
            //-----------------
            #endregion


            // 進行。
            //this.ct三角矢印アニメ.t進行Loop();
            this.ct譜面分岐?.t進行Loop();

            //if( this.tx背景 != null )
            //    this.tx背景.t2D描画( CDTXMania.app.Device, 0, 0 );

            //キー操作
            if( !CDTXMania.stage選曲.bActivePopup )
            {
                #region[ 1P ]
                if( CDTXMania.Pad.b押された( E楽器パート.DRUMS, Eパッド.LBlue ) || CDTXMania.Input管理.Keyboard.bキーが押された( (int) SlimDX.DirectInput.Key.LeftArrow ) )
                {
                    CDTXMania.Skin.soundカーソル移動音.t再生する();
                    this.t前に移動( 0 );
                }
                else if( CDTXMania.Pad.b押された( E楽器パート.DRUMS, Eパッド.RBlue ) || CDTXMania.Input管理.Keyboard.bキーが押された( (int) SlimDX.DirectInput.Key.RightArrow ) )
                {
                    CDTXMania.Skin.soundカーソル移動音.t再生する();
                    this.t次に移動( 0 );
                }
                #endregion
                #region[ 2P ]
                if ( CDTXMania.Pad.b押された( E楽器パート.DRUMS, Eパッド.LBlue2P ) && CDTXMania.ConfigIni.nPlayerCount >= 2 )
                {
                    CDTXMania.Skin.soundカーソル移動音.t再生する();
                    this.t前に移動( 1 );
                }
                else if( CDTXMania.Pad.b押された( E楽器パート.DRUMS, Eパッド.RBlue2P ) && CDTXMania.ConfigIni.nPlayerCount >= 2 )
                {
                    CDTXMania.Skin.soundカーソル移動音.t再生する();
                    this.t次に移動( 1 );
                }
                #endregion
                if ( ( CDTXMania.Pad.b押されたDGB( Eパッド.Decide ) ||
					    ( ( CDTXMania.ConfigIni.bEnterがキー割り当てのどこにも使用されていない && CDTXMania.Input管理.Keyboard.bキーが押された( (int) SlimDX.DirectInput.Key.Return ) ) ) ) )
                {
                    // ToDo:2プレイヤー以上の場合は流れが別のものになる(1P決定→2P未決定なら1Pは待機...みたいな)

                    if( this.list難易度選択項目[ this.n現在の選択行[ 0 ] ].b選択可 )
                    {
                        //CDTXMania.stage選曲.actPresound.tサウンド停止();
                        switch( this.list難易度選択項目[ this.n現在の選択行[ 0 ] ].e項目種類 )
                        {
                            case E項目種類.かんたん:
                            case E項目種類.ふつう:
                            case E項目種類.むずかしい:
                            case E項目種類.おに:
                            case E項目種類.エディット:
                                {
                                    switch( CDTXMania.stage選曲.r現在選択中の曲.eノード種別 )
                                    {
                                        case C曲リストノード.Eノード種別.SCORE:
                                            {
                                                CDTXMania.Skin.sound決定音.t再生する();
                                                CDTXMania.stage選曲.t曲を選択する( (int)this.list難易度選択項目[ this.n現在の選択行[ 0 ] ].e項目種類 );
                                            }
                                            break;
                                        case C曲リストノード.Eノード種別.RANDOM:
                                            {
                                                CDTXMania.Skin.sound曲決定音.t再生する();
                                                //CDTXMania.stage選曲.n確定された曲の難易度 = (int)this.list難易度選択項目[this.n現在の選択行].e項目種類;
                                                CDTXMania.stage選曲.act曲リスト.n現在のアンカ難易度レベル_渡( 0, (int)this.list難易度選択項目[this.n現在の選択行[ 0 ]].e項目種類 );
                                                CDTXMania.stage選曲.t曲をランダム選択する();
                                            }
                                            break;
                                    }
                                }
                                break;
                            case E項目種類.戻る:
                                CDTXMania.stage選曲.t難易度選択画面を閉じる();
                                break;
                            case E項目種類.オプション:
                                break;
                            case E項目種類.音色:
                                break;
                        }

                    }
                    else
                    {
                        // 選択できない項目だった
                        CDTXMania.Skin.sound選択不可音.t再生する();
                    }
                }
                else if( CDTXMania.Input管理.Keyboard.bキーが押された( (int) SlimDX.DirectInput.Key.Escape ) )
                {
                    CDTXMania.stage選曲.t難易度選択画面を閉じる();
                }
			    #region [ F2 簡易オプション ]
			    if ( CDTXMania.Input管理.Keyboard.bキーが押された( (int) SlimDX.DirectInput.Key.F2 ) )
			    {
                    CDTXMania.Skin.sound変更音.t再生する();
                    CDTXMania.stage選曲.actQuickConfig.tActivatePopupMenu( E楽器パート.DRUMS );
			    }
			    #endregion
			    #region [ F3 オートON/OFF ]
			    if ( CDTXMania.Input管理.Keyboard.bキーが押された( (int) SlimDX.DirectInput.Key.F3 ) )
			    {	// [ESC]
				    CDTXMania.Skin.sound変更音.t再生する();
                    C共通.bToggleBoolian( ref CDTXMania.ConfigIni.b太鼓パートAutoPlay );
			    }
			    #endregion
			    #region [ F4 ゲージ ]
			    if ( CDTXMania.Input管理.Keyboard.bキーが押された( (int) SlimDX.DirectInput.Key.F4 ) )
			    {
				    CDTXMania.Skin.sound変更音.t再生する();
                    if( CDTXMania.ConfigIni.eGaugeMode == Eゲージモード.Normal )
                        CDTXMania.ConfigIni.eGaugeMode = Eゲージモード.IIDX;
                    else if( CDTXMania.ConfigIni.eGaugeMode == Eゲージモード.IIDX )
                        CDTXMania.ConfigIni.eGaugeMode = Eゲージモード.HARD;
                    else if( CDTXMania.ConfigIni.eGaugeMode == Eゲージモード.HARD )
                        CDTXMania.ConfigIni.eGaugeMode = Eゲージモード.EXHARD;
                    else if( CDTXMania.ConfigIni.eGaugeMode == Eゲージモード.EXHARD )
                        CDTXMania.ConfigIni.eGaugeMode = Eゲージモード.DEATH;
                    else if( CDTXMania.ConfigIni.eGaugeMode == Eゲージモード.DEATH )
                        CDTXMania.ConfigIni.eGaugeMode = Eゲージモード.Normal;
			    }
			    #endregion
            }
            

            foreach( var item in this.list難易度選択項目 )
            {
                if( item.txパネル != null )
                {
                    item.txパネル.n透明度 = item.b選択可 ? 255 : 127;
                    item.txパネル.t2D描画( CDTXMania.app.Device, item.ptパネル座標.X, item.ptパネル座標.Y );
                    if( item.b譜面 )
                    {
                        if( item.b譜面分岐 ? ( this.ct譜面分岐.n現在の値 >= 0 && this.ct譜面分岐.n現在の値 < 100 ) : false )
                        {
                            this.tx譜面分岐?.t2D描画( CDTXMania.app.Device, item.ptパネル座標.X, item.ptパネル座標.Y );
                            //CDTXMania.act文字コンソール.tPrint( CDTXMania.Skin.nSelectSongDiffIconX + (60 * i), 343, C文字コンソール.Eフォント種別.赤, "B\nr\na\nn\nc\nh" );
                        }
                        else
                        {
                            for ( int i = 0; i < CDTXMania.stage選曲.r現在選択中のスコア.譜面情報.nレベル[ (int)item.e項目種類 ]; i++ )
                            {
                                this.txレベル星.t2D描画( CDTXMania.app.Device, item.ptパネル座標.X + 40, (item.ptパネル座標.Y + 392) - (20 * i ) );
                            }
                        }
                    }
                }
            }

            for( int i = 0; i < CDTXMania.ConfigIni.nPlayerCount; i++ )
            {
                this.tカーソルを描画する( i );
            }

            #region[ デバッグ用表示 ]
            //-----------------
#if DEBUG
            int nバー基準Y = 64;
            CDTXMania.act文字コンソール.tPrint( 0, 32, C文字コンソール.Eフォント種別.白, "P1:" + this.n現在の選択行[ 0 ].ToString() );

            for( int i = 0; i < this.list難易度選択項目.Count; i++ )
            {
                C文字コンソール.Eフォント種別 bColorFlag = this.n現在の選択行[ 0 ] == i ? C文字コンソール.Eフォント種別.赤 : (this.list難易度選択項目[i].b選択可 ? C文字コンソール.Eフォント種別.白 : C文字コンソール.Eフォント種別.灰);

                nバー基準Y = nバー基準Y + 16;
                CDTXMania.act文字コンソール.tPrint( 0, nバー基準Y, bColorFlag, this.list難易度選択項目[ i ].str項目名 );
            }

            nバー基準Y += 32;
            CDTXMania.act文字コンソール.tPrint( 0, nバー基準Y, C文字コンソール.Eフォント種別.白, "P2:" + this.n現在の選択行[ 1 ].ToString() );
            nバー基準Y += 16;
            
            for( int i = 0; i < this.list難易度選択項目.Count; i++ )
            {
                C文字コンソール.Eフォント種別 bColorFlag = this.n現在の選択行[ 1 ] == i ? C文字コンソール.Eフォント種別.赤 : (this.list難易度選択項目[i].b選択可 ? C文字コンソール.Eフォント種別.白 : C文字コンソール.Eフォント種別.灰);

                nバー基準Y = nバー基準Y + 16;
                CDTXMania.act文字コンソール.tPrint( 0, nバー基準Y, bColorFlag, this.list難易度選択項目[ i ].str項目名 );
            }
            //-----------------
#endif
            #endregion

            // ToDo:フッター画像は曲選択と統一したほうがいい
            if( this.txフッター != null )
                this.txフッター.t2D描画( CDTXMania.app.Device, 0, 720 - this.txフッター.sz画像サイズ.Height );

			return 0;
		}
		

		// その他

#region [ private ]
		//-----------------
		private bool b登場アニメ全部完了;
        private CCounter ct三角矢印アニメ;
        private CCounter ct移動;
        private CCounter ct譜面分岐;
		private long nスクロールタイマ;
		private int n現在のスクロールカウンタ;
		private int[] n現在の選択行;
		private int n目標のスクロールカウンタ;

        private CTexture tx背景;
        private CTexture txヘッダー;
        private CTexture txフッター;

        private CTexture tx説明背景;
        private CTexture tx説明1;

        private CTexture txカーソル大;
        private CTexture txカーソル小;
        private CTexture txレベル星;
        private CTexture tx譜面分岐;

        private CSound soundSelectAnnounce;


        private long n矢印スクロール用タイマ値;
        
        protected List<ST難易度選択項目> list難易度選択項目 = new List<ST難易度選択項目>();
		//-----------------
        
        //構造体
        protected struct ST難易度選択項目
        {
            public CTexture txパネル;
            public E項目種類 e項目種類;
            public Point ptパネル座標;
            public Rectangle rectパネル位置;
            public bool b選択可;
            public string str項目名;
            public bool b譜面
            {
                get
                {
                    return (int)e項目種類 < 5 ? true : false;
                }
            }
            public bool b譜面分岐;
        }

        public enum E項目種類
        {
            かんたん = 0,
            ふつう = 1,
            むずかしい = 2,
            おに = 3,
            エディット = 4,
            戻る = 5,
            オプション = 6,
            音色 = 7,
            未定義 = 99
        }


#endregion


        private void tカーソルを描画する( int player )
        {
            if (this.list難易度選択項目 != null && this.list難易度選択項目.Count > 1 )
            {
                switch( this.list難易度選択項目[ this.n現在の選択行[ player ] ].e項目種類 )
                {
                    case E項目種類.かんたん:
                    case E項目種類.ふつう:
                    case E項目種類.むずかしい:
                    case E項目種類.おに:
                    case E項目種類.エディット:
                        if( this.txカーソル大 != null )
                            this.txカーソル大.t2D描画( CDTXMania.app.Device, this.list難易度選択項目[ this.n現在の選択行[ player ] ].ptパネル座標.X, this.list難易度選択項目[ this.n現在の選択行[ player ] ].ptパネル座標.Y, new Rectangle( 0, 0, 102, 530 ) );
                        break;
                    case E項目種類.戻る:
                    case E項目種類.オプション:
                    case E項目種類.音色:
                        if( this.txカーソル小 != null )
                            this.txカーソル小.t2D描画( CDTXMania.app.Device, this.list難易度選択項目[ this.n現在の選択行[ player ] ].ptパネル座標.X, this.list難易度選択項目[ this.n現在の選択行[ player ] ].ptパネル座標.Y, new Rectangle( 0, 0, 70, 310 ) );
                        break;
                }

                //if( this._プレイヤーカーソル[ player ].txカーソル != null )
                //{
                //    this._プレイヤーカーソル[ player ].txカーソル.t2D描画(CDTXMania.app.Device, this.list難易度選択項目[ this.n現在の選択行[ player ] ].ptパネル座標.X + this.list難易度選択項目[ this.n現在の選択行[ player ] ].rectパネル位置.Width / 2 - (this._プレイヤーカーソル[ player ].txカーソル.szテクスチャサイズ.Width / 2), 4);
                //}
                if( this._プレイヤーカーソル[ player ].txカーソル != null )
                {
                    int nY移動 = 0;
                    //if( CAnimationManager.b進行中( this._プレイヤーカーソル[ player ].ストーリーボード ) ) {
                    //    nY移動 = (int)this._プレイヤーカーソル[ player ].吹き出し左上位置X.Value;
                    //}
                    this._プレイヤーカーソル[ player ].txカーソル.t2D描画( CDTXMania.app.Device, this.list難易度選択項目[ this.n現在の選択行[ player ] ].ptパネル座標.X + this.list難易度選択項目[this.n現在の選択行[player]].rectパネル位置.Width / 2 - (this._プレイヤーカーソル[player].txカーソル.szテクスチャサイズ.Width / 2) + nY移動, 4 );
                    
                }
            }
        }

        /// <summary>
        /// プレイヤーのカーソル(吹き出し/枠)のテクスチャとStoryboardを管理するためのクラス
        /// 使用する際はこのクラスのオブジェクトを複数作成して管理すること。
        /// </summary>
        protected class プレイヤーカーソル : IDisposable
        {
            public CTexture txカーソル;
            public Variable 吹き出し左上位置X;
            public Variable 吹き出し左上位置Y;
            public Variable 枠左上位置X;
            public Variable 枠左上位置Y;
            public Variable 枠不透明度;
            public Storyboard ストーリーボード;

            public void Dispose()
            {
                this.ストーリーボード?.Abandon();
                this.ストーリーボード = null;

                this.吹き出し左上位置X?.Dispose();
                this.吹き出し左上位置X = null;

                this.吹き出し左上位置Y?.Dispose();
                this.吹き出し左上位置X = null;

                this.枠左上位置X?.Dispose();
                this.枠左上位置X = null;

                this.枠左上位置Y?.Dispose();
                this.枠左上位置X = null;

                this.枠不透明度?.Dispose();
                this.枠不透明度 = null;
            }
        }
        protected プレイヤーカーソル[] _プレイヤーカーソル;
	}
}
