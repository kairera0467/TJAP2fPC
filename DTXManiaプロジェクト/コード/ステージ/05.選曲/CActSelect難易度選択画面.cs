using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Drawing.Text;

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
        
        public void t次に移動()
		{
            if (this.n現在の選択行 < this.list難易度選択項目.Count - 1)
            {
                this.n現在の選択行 += 1;
            }

            this.ct移動 = new CCounter( 1, 710, 1, CSound管理.rc演奏用タイマ );
		}
		public void t前に移動()
		{
            if( this.n現在の選択行 > 0 )
            {
                this.n現在の選択行 -= 1;
            }

            this.ct移動 = new CCounter( 1, 710, 1, CSound管理.rc演奏用タイマ );
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

			// フォント作成。
			// 曲リスト文字は２倍（面積４倍）でテクスチャに描画してから縮小表示するので、フォントサイズは２倍とする。
            this.ct三角矢印アニメ = new CCounter();
            this.ct移動 = new CCounter();
            this.ct譜面分岐 = new CCounter();

			base.On活性化();
		}
		public override void On非活性化()
		{
			if( this.b活性化してない )
				return;

			for( int i = 0; i < 13; i++ )
				this.ct登場アニメ用[ i ] = null;

            this.ct移動 = null;
            this.ct三角矢印アニメ = null;
            this.ct譜面分岐 = null;

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
                            stDiffList.rectパネル位置 = new Rectangle( 0, 0, 0, 0 );
                            stDiffList.txパネル = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_item_back.png" ) );
                            this.list難易度選択項目.Add( stDiffList );

                            break;
                        case 1:
                            // オプション
                            stDiffList.b選択可 = false;
                            stDiffList.str項目名 = "option";
                            stDiffList.e項目種類 = E項目種類.オプション;
                            stDiffList.ptパネル座標 = new Point( 319, 114 );
                            stDiffList.rectパネル位置 = new Rectangle( 0, 0, 0, 0 );
                            stDiffList.txパネル = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_item_option.png" ) );
                            this.list難易度選択項目.Add( stDiffList );
                            break;
                        case 2:
                            // 音色
                            stDiffList.b選択可 = false;
                            stDiffList.str項目名 = "se";
                            stDiffList.e項目種類 = E項目種類.音色;
                            stDiffList.ptパネル座標 = new Point( 389, 114 );
                            stDiffList.rectパネル位置 = new Rectangle( 0, 0, 0, 0 );
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
				for( int i = 0; i < 13; i++ )
					this.ct登場アニメ用[ i ] = new CCounter( -i * 10, 100, 3, CDTXMania.Timer );
                this.ct譜面分岐.t開始( 1, 200, 10, CDTXMania.Timer );
				this.nスクロールタイマ = CSound管理.rc演奏用タイマ.n現在時刻;
				CDTXMania.stage選曲.t選択曲変更通知();

                this.n矢印スクロール用タイマ値 = CSound管理.rc演奏用タイマ.n現在時刻;
				this.ct三角矢印アニメ.t開始( 0, 19, 40, CDTXMania.Timer );

                // 現在位置をかんたん～おに(エディット)の間に移動させる
#if DEBUG
                this.n現在の選択行 = 0;
#else
                this.n現在の選択行 = 3 + CDTXMania.stage選曲.act曲リスト.n現在選択中の曲の現在の難易度レベル;
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
                    stDiffList.rectパネル位置 = new Rectangle( 0, 0, 0, 0 );
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
            if( CDTXMania.Input管理.Keyboard.bキーが押された( (int) SlimDX.DirectInput.Key.RightArrow ) )
            {
                CDTXMania.Skin.soundカーソル移動音.t再生する();
                this.t次に移動();
            }
            else if( CDTXMania.Input管理.Keyboard.bキーが押された( (int) SlimDX.DirectInput.Key.LeftArrow ) )
            {
                CDTXMania.Skin.soundカーソル移動音.t再生する();
                this.t前に移動();
            }
            else if ( ( CDTXMania.Pad.b押されたDGB( Eパッド.Decide ) ||
					( ( CDTXMania.ConfigIni.bEnterがキー割り当てのどこにも使用されていない && CDTXMania.Input管理.Keyboard.bキーが押された( (int) SlimDX.DirectInput.Key.Return ) ) ) ) )
            {
                if( this.list難易度選択項目[ this.n現在の選択行 ].b選択可 )
                {
                    //CDTXMania.stage選曲.actPresound.tサウンド停止();
                    switch( this.list難易度選択項目[ this.n現在の選択行 ].e項目種類 )
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
                                            CDTXMania.stage選曲.t曲を選択する( (int)this.list難易度選択項目[ this.n現在の選択行 ].e項目種類 );
                                        }
                                        break;
                                    case C曲リストノード.Eノード種別.RANDOM:
                                        {
                                            CDTXMania.Skin.sound曲決定音.t再生する();
                                            //CDTXMania.stage選曲.n確定された曲の難易度 = (int)this.list難易度選択項目[this.n現在の選択行].e項目種類;
                                            CDTXMania.stage選曲.act曲リスト.n現在のアンカ難易度レベル_渡( (int)this.list難易度選択項目[this.n現在の選択行].e項目種類 );
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

            switch( this.list難易度選択項目[ this.n現在の選択行 ].e項目種類 )
            {
                case E項目種類.かんたん:
                case E項目種類.ふつう:
                case E項目種類.むずかしい:
                case E項目種類.おに:
                case E項目種類.エディット:
                    if( this.txカーソル大 != null )
                        this.txカーソル大.t2D描画( CDTXMania.app.Device, this.list難易度選択項目[ this.n現在の選択行 ].ptパネル座標.X, this.list難易度選択項目[ this.n現在の選択行 ].ptパネル座標.Y, new Rectangle( 0, 0, 102, 530 ) );
                    break;
                case E項目種類.戻る:
                case E項目種類.オプション:
                case E項目種類.音色:
                    if( this.txカーソル小 != null )
                        this.txカーソル小.t2D描画( CDTXMania.app.Device, this.list難易度選択項目[ this.n現在の選択行 ].ptパネル座標.X, this.list難易度選択項目[ this.n現在の選択行 ].ptパネル座標.Y, new Rectangle( 0, 0, 70, 310 ) );
                    break;
            }


            #region[ デバッグ用表示 ]
            //-----------------
#if DEBUG
            int nバー基準Y = 64;
            CDTXMania.act文字コンソール.tPrint( 0, 32, C文字コンソール.Eフォント種別.白, this.n現在の選択行.ToString() );

            for( int i = 0; i < this.list難易度選択項目.Count; i++ )
            {
                C文字コンソール.Eフォント種別 bColorFlag = this.n現在の選択行 == i ? C文字コンソール.Eフォント種別.赤 : (this.list難易度選択項目[i].b選択可 ? C文字コンソール.Eフォント種別.白 : C文字コンソール.Eフォント種別.灰);

                nバー基準Y = nバー基準Y + 16;
                CDTXMania.act文字コンソール.tPrint( 0, nバー基準Y, bColorFlag, this.list難易度選択項目[ i ].str項目名 );
            }
            //-----------------
#endif
            #endregion

            if( this.txフッター != null )
                this.txフッター.t2D描画( CDTXMania.app.Device, 0, 720 - this.txフッター.sz画像サイズ.Height );

			return 0;
		}
		

		// その他

#region [ private ]
		//-----------------

		private bool b登場アニメ全部完了;
		private CCounter[] ct登場アニメ用 = new CCounter[ 13 ];
        private CCounter ct三角矢印アニメ;
        private CCounter ct移動;
        private CCounter ct譜面分岐;
		private long nスクロールタイマ;
		private int n現在のスクロールカウンタ;
		private int n現在の選択行;
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

        private int[] n描画順;
        private int[] n踏み台座標;
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
	}
}
