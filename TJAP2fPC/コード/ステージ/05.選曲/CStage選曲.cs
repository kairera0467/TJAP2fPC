﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using SharpDX.Animation;
using FDK;

namespace DTXMania
{
	internal class CStage選曲 : CStage
	{
		// プロパティ
		public int nスクロールバー相対y座標
		{
			get
			{
				if ( act曲リスト != null )
				{
					return act曲リスト.nスクロールバー相対y座標;
				}
				else
				{
					return 0;
				}
			}
		}
		public bool bIsEnumeratingSongs
		{
			get
			{
				return act曲リスト.bIsEnumeratingSongs;
			}
			set
			{
				act曲リスト.bIsEnumeratingSongs = value;
			}
		}
		public bool bIsPlayingPremovie
		{
			get
			{
				return this.actPreimageパネル.bIsPlayingPremovie;
			}
		}
		public bool bスクロール中
		{
			get
			{
				return this.act曲リスト.bスクロール中;
			}
		}
        public bool bActivePopup // 2019.3.22 kairera0467
        {
            get
            {
                return this.actQuickConfig.bIsActivePopupMenu || this.actSortSongs.bIsActivePopupMenu;
            }
        }
		public int n確定された曲の難易度
		{
			get;
			set;
		}
		public Cスコア r確定されたスコア
		{
			get;
			private set;
		}
		public C曲リストノード r確定された曲 
		{
			get;
			private set;
		}
		public int n現在選択中の曲の難易度
		{
			get
			{
				return this.act曲リスト.n現在選択中の曲の現在の難易度レベル;
			}
		}
		public Cスコア r現在選択中のスコア
		{
			get
			{
				return this.act曲リスト.r現在選択中のスコア;
			}
		}
		public C曲リストノード r現在選択中の曲
		{
			get
			{
				return this.act曲リスト.r現在選択中の曲;
			}
		}

		// コンストラクタ
		public CStage選曲()
		{
			base.eステージID = CStage.Eステージ.選曲;
			base.eフェーズID = CStage.Eフェーズ.共通_通常状態;
			base.b活性化してない = true;
			base.list子Activities.Add( this.actオプションパネル = new CActオプションパネル() );
            base.list子Activities.Add( this.actFIFO = new CActFIFOBlack() );
			base.list子Activities.Add( this.actFIfrom結果画面 = new CActFIFOBlack() );
			//base.list子Activities.Add( this.actFOtoNowLoading = new CActFIFOBlack() );
            base.list子Activities.Add( this.actFOtoNowLoading = new CActFIFOStart() );
            base.list子Activities.Add( this.actFIFOタイトル画面 = new CActFIFOFace() );
			base.list子Activities.Add( this.act曲リスト = new CActSelect曲リスト() );
			base.list子Activities.Add( this.actステータスパネル = new CActSelectステータスパネル() );
			base.list子Activities.Add( this.act演奏履歴パネル = new CActSelect演奏履歴パネル() );
			base.list子Activities.Add( this.actPreimageパネル = new CActSelectPreimageパネル() );
			base.list子Activities.Add( this.actPresound = new CActSelectPresound() );
			base.list子Activities.Add( this.actArtistComment = new CActSelectArtistComment() );
			base.list子Activities.Add( this.actInformation = new CActSelectInformation() );
			base.list子Activities.Add( this.actSortSongs = new CActSortSongs() );
			base.list子Activities.Add( this.actShowCurrentPosition = new CActSelectShowCurrentPosition() );
			base.list子Activities.Add( this.actQuickConfig = new CActSelectQuickConfig() );
			base.list子Activities.Add( this.act難易度選択画面 = new CActSelect難易度選択画面() );

			this.CommandHistory = new CCommandHistory();		// #24063 2011.1.16 yyagi
		}
		
		
		// メソッド

		public void t選択曲変更通知()
		{
			this.actPreimageパネル.t選択曲が変更された();
			this.actPresound.t選択曲が変更された();
			this.act演奏履歴パネル.t選択曲が変更された();
			this.actステータスパネル.t選択曲が変更された();
			this.actArtistComment.t選択曲が変更された();

			#region [ プラグインにも通知する（BOX, RANDOM, BACK なら通知しない）]
			//---------------------
			if( CDTXMania.app != null )
			{
				var c曲リストノード = CDTXMania.stage選曲.r現在選択中の曲;
				var cスコア = CDTXMania.stage選曲.r現在選択中のスコア;

				if( c曲リストノード != null && cスコア != null && c曲リストノード.eノード種別 == C曲リストノード.Eノード種別.SCORE )
				{
					string str選択曲ファイル名 = cスコア.ファイル情報.ファイルの絶対パス;
					CSetDef setDef = null;
					int nブロック番号inSetDef = -1;
					int n曲番号inブロック = -1;

					if( !string.IsNullOrEmpty( c曲リストノード.pathSetDefの絶対パス ) && File.Exists( c曲リストノード.pathSetDefの絶対パス ) )
					{
						setDef = new CSetDef( c曲リストノード.pathSetDefの絶対パス );
						nブロック番号inSetDef = c曲リストノード.SetDefのブロック番号;
						n曲番号inブロック = CDTXMania.stage選曲.act曲リスト.n現在のアンカ難易度レベルに最も近い難易度レベルを返す( c曲リストノード );
					}

					foreach( CDTXMania.STPlugin stPlugin in CDTXMania.app.listプラグイン )
					{
						Directory.SetCurrentDirectory( stPlugin.strプラグインフォルダ );
						stPlugin.plugin.On選択曲変更( str選択曲ファイル名, setDef, nブロック番号inSetDef, n曲番号inブロック );
						Directory.SetCurrentDirectory( CDTXMania.strEXEのあるフォルダ );
					}
				}
			}
			//---------------------
			#endregion
		}

		// CStage 実装

		/// <summary>
		/// 曲リストをリセットする
		/// </summary>
		/// <param name="cs"></param>
		public void Refresh( CSongs管理 cs, bool bRemakeSongTitleBar)
		{
			this.act曲リスト.Refresh( cs, bRemakeSongTitleBar );
		}

		public override void On活性化()
		{
			Trace.TraceInformation( "選曲ステージを活性化します。" );
			Trace.Indent();
			try
			{
				this.eフェードアウト完了時の戻り値 = E戻り値.継続;
				this.bBGM再生済み = false;
				this.ftフォント = new Font( "MS PGothic", 26f, GraphicsUnit.Pixel );
				for( int i = 0; i < 4; i++ )
					this.ctキー反復用[ i ] = new CCounter( 0, 0, 0, CDTXMania.Timer );

                this.act難易度選択画面.bIsDifficltSelect = false;
				base.On活性化();

				this.actステータスパネル.t選択曲が変更された();	// 最大ランクを更新

                this.ctDiffSelect移動待ち = new CCounter();
                this.ctDiffSelect戻り待ち = new CCounter();

                this._左上テキスト = new 左上テキスト();
			}
			finally
			{
                CDTXMania.ConfigIni.eScrollMode = EScrollMode.Normal;
                CDTXMania.ConfigIni.bスクロールモードを上書き = false;
                Trace.TraceInformation( "選曲ステージの活性化を完了しました。" );
				Trace.Unindent();
			}
		}
		public override void On非活性化()
		{
			Trace.TraceInformation( "選曲ステージを非活性化します。" );
			Trace.Indent();
			try
			{
				if( this.ftフォント != null )
				{
					this.ftフォント.Dispose();
					this.ftフォント = null;
				}
				for( int i = 0; i < 4; i++ )
				{
					this.ctキー反復用[ i ] = null;
				}
                this.ct背景スクロール = null;
				base.On非活性化();

                this._左上テキスト.Dispose();
			}
			finally
			{
				Trace.TraceInformation( "選曲ステージの非活性化を完了しました。" );
				Trace.Unindent();
			}
		}
		public override void OnManagedリソースの作成()
		{
			if( !base.b活性化してない )
			{
				this.tx背景 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_background.png" ) );
				this.tx上部パネル = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_header_panel.png" ), false );
				this.tx下部パネル = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_footer panel.png" ) );

				//this.txFLIP = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_skill number on gauge etc.png" ), false );
                this.tx難易度名 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_difficulty name.png" ) );
                this.txジャンル別背景[0] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_background_Anime.png" ) );
                this.txジャンル別背景[1] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_background_JPOP.png" ) );
                this.txジャンル別背景[2] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_background_Game.png" ) );
                this.txジャンル別背景[3] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_background_Namco.png" ) );
                this.txジャンル別背景[4] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_background_Classic.png" ) );
                this.txジャンル別背景[5] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_background_Child.png" ) );
                this.txジャンル別背景[6] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_background_Variety.png" ) );
                this.txジャンル別背景[7] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_background_VOCALID.png" ) );
                this.txジャンル別背景[8] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_background_Other.png" ) );

                this.tx難易度別背景[0] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_background_Easy.png" ) );
                this.tx難易度別背景[1] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_background_Normal.png" ) );
                this.tx難易度別背景[2] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_background_Hard.png" ) );
                this.tx難易度別背景[3] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_background_Master.png" ) );
                this.tx難易度別背景[4] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_background_Edit.png" ) );
                this.tx左上テキスト = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_header text.png" ) );
                this.tx下部テキスト = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_footer text.png" ) );


                //敷き詰める枚数を計算。
                if( this.n背景ループ幅 <= 0 )
                {
                    if( this.tx背景 != null )
                        this.n背景ループ幅 = this.tx背景.sz画像サイズ.Width;
                    else
                        this.n背景ループ幅 = 1; // ゼロ除算防止のため
                }

                int nTilingX = 0;
                for( int i = 0; ; i++ )
                {
                    this.n背景テクスチャ敷き詰め枚数++;
                    nTilingX += this.n背景ループ幅;
                    if( nTilingX > 1280 ) {
                        this.n背景テクスチャ敷き詰め枚数++;
                        break;
                    }
                }

                this.ct背景スクロール = new CCounter( 0, this.n背景ループ幅, 50, CDTXMania.Timer );
				base.OnManagedリソースの作成();
			}
		}
		public override void OnManagedリソースの解放()
		{
			if( !base.b活性化してない )
			{
				CDTXMania.tテクスチャの解放( ref this.tx背景 );
				CDTXMania.tテクスチャの解放( ref this.tx上部パネル );
				CDTXMania.tテクスチャの解放( ref this.tx下部パネル );
				//CDTXMania.tテクスチャの解放( ref this.txFLIP );
                CDTXMania.tテクスチャの解放( ref this.tx難易度名 );
                CDTXMania.tテクスチャの解放( ref this.tx左上テキスト );
                CDTXMania.tテクスチャの解放( ref this.tx下部テキスト );
                for( int j = 0; j < 9; j++ )
                {
                    CDTXMania.tテクスチャの解放( ref this.txジャンル別背景[ j ] );
                }
                for( int j = 0; j < 5; j++ )
                {
                    CDTXMania.tテクスチャの解放( ref this.tx難易度別背景[ j ] );
                }
				base.OnManagedリソースの解放();
			}
		}
		public override int On進行描画()
		{
			if( !base.b活性化してない )
			{
				#region [ 初めての進行描画 ]
				//---------------------
				if( base.b初めての進行描画 )
				{
					this.ct登場時アニメ用共通 = new CCounter( 0, 100, 3, CDTXMania.Timer );
					if( CDTXMania.r直前のステージ == CDTXMania.stage結果 )
					{
						this.actFIfrom結果画面.tフェードイン開始();
						base.eフェーズID = CStage.Eフェーズ.選曲_結果画面からのフェードイン;
					}
                    else if( CDTXMania.r直前のステージ == CDTXMania.stageタイトル )
                    {
						this.actFIFOタイトル画面.tフェードイン開始();
						base.eフェーズID = CStage.Eフェーズ.選曲_タイトル画面からのフェードイン;
                    }
					else
					{
						this.actFIFO.tフェードイン開始();
						base.eフェーズID = CStage.Eフェーズ.共通_フェードイン;
					}
					this.t選択曲変更通知();
                    this.t左上テキスト登場();
					base.b初めての進行描画 = false;
				}
				//---------------------
				#endregion

				this.ct登場時アニメ用共通.t進行();
                this.ct背景スクロール.t進行Loop();

				if( this.tx背景 != null )
                {
                    for( int i = 0; i < this.n背景テクスチャ敷き詰め枚数; i++ )
                    {
					    this.tx背景.t2D描画( CDTXMania.app.Device, ( i * this.n背景ループ幅 ) - this.ct背景スクロール.n現在の値, 0, new Rectangle( 0, 0, this.n背景ループ幅, 720 ) );
                    }

                }

                if( this.r現在選択中の曲 != null )
                {
                    if( CDTXMania.stage選曲.actSortSongs.e現在のソート != CActSortSongs.EOrder.Title )
                    {
                        for ( int i = 0; i < this.n背景テクスチャ敷き詰め枚数; i++ )
                        {
                            this.txジャンル別背景[ this.nStrジャンルtoNum( this.r現在選択中の曲.strジャンル ) ]?.t2D描画( CDTXMania.app.Device, ( i * this.n背景ループ幅 ) - this.ct背景スクロール.n現在の値, 0, new Rectangle( 0, 0, this.n背景ループ幅, this.txジャンル別背景[this.nStrジャンルtoNum( this.r現在選択中の曲.strジャンル )].sz画像サイズ.Height ) );
                        }
                    }
                    else
                    {
                        for ( int i = 0; i < this.n背景テクスチャ敷き詰め枚数; i++ )
                        {
                            this.tx難易度別背景[ this.n現在選択中の曲の難易度 ]?.t2D描画( CDTXMania.app.Device, ( i * this.n背景ループ幅 ) - this.ct背景スクロール.n現在の値, 0, new Rectangle( 0, 0, this.n背景ループ幅, 720 ) );
                        }
                    }
                }


				//this.actPreimageパネル.On進行描画();
			//	this.bIsEnumeratingSongs = !this.actPreimageパネル.bIsPlayingPremovie;				// #27060 2011.3.2 yyagi: #PREMOVIE再生中は曲検索を中断する

				this.act曲リスト.On進行描画();
                int x = 0;
				int y = 0;
				if( this.ct登場時アニメ用共通.b進行中 )
				{
					double db登場割合 = ( (double) this.ct登場時アニメ用共通.n現在の値 ) / 100.0;	// 100が最終値
					double dbY表示割合 = Math.Sin( Math.PI / 2 * db登場割合 );
                    if( this.tx上部パネル != null )
					    y = ( (int) ( this.tx上部パネル.sz画像サイズ.Height * dbY表示割合 ) ) - this.tx上部パネル.sz画像サイズ.Height;
                    if( this.tx左上テキスト != null )
                        x = ( (int)( this.tx左上テキスト.sz画像サイズ.Width * dbY表示割合 ) );
                        
				}

                this.tx上部パネル?.t2D描画( CDTXMania.app.Device, 0, 0 );

                //if( !this.act難易度選択画面.bIsDifficltSelect )
                //    this.tx左上テキスト?.t2D描画( CDTXMania.app.Device, -this.tx左上テキスト.sz画像サイズ.Width + x, 0, new Rectangle( 0, 0, this.tx左上テキスト.sz画像サイズ.Width, 90 ) );
                //else if( this.act難易度選択画面.bIsDifficltSelect && this.ctDiffSelect移動待ち.b終了値に達した )
                //    this.tx左上テキスト?.t2D描画( CDTXMania.app.Device, -this.tx左上テキスト.sz画像サイズ.Width + x, 0, new Rectangle( 0, 90, this.tx左上テキスト.sz画像サイズ.Width, 90 ) );
                if( this.tx左上テキスト != null )
                {
                    if( (int)this._左上テキスト.var表示内容.Value == 0 )
                    {
                        this.tx左上テキスト.n透明度 = (int)this._左上テキスト.var左上テキスト不透明度.Value;
                        this.tx左上テキスト?.t2D描画( CDTXMania.app.Device, (int)this._左上テキスト.var左上テキスト位置X.Value, 0, new Rectangle( 0, 0, this.tx左上テキスト.sz画像サイズ.Width, 90 ) );
                    }
                    else if( (int)this._左上テキスト.var表示内容.Value == 1 )
                    {
                        this.tx左上テキスト.n透明度 = (int)this._左上テキスト.var左上テキスト不透明度.Value;
                        this.tx左上テキスト?.t2D描画( CDTXMania.app.Device, (int)this._左上テキスト.var左上テキスト位置X.Value, 0, new Rectangle( 0, 90, this.tx左上テキスト.sz画像サイズ.Width, 90 ) );
                    }
                }
                


				this.actInformation.On進行描画();
				this.tx下部パネル?.t2D描画( CDTXMania.app.Device, 0, 720 - this.tx下部パネル.sz画像サイズ.Height );
                #region[ 上部テキスト ]
                if( CDTXMania.ConfigIni.eGameMode == EGame.完走叩ききりまショー )
                    CDTXMania.act文字コンソール.tPrint( 0, 0, C文字コンソール.Eフォント種別.白, "GAME: SURVIVAL" );
                if( CDTXMania.ConfigIni.eGameMode == EGame.完走叩ききりまショー激辛 )
                    CDTXMania.act文字コンソール.tPrint( 0, 0, C文字コンソール.Eフォント種別.白, "GAME: SURVIVAL HARD" );
                if( CDTXMania.ConfigIni.bSuperHard )
                    CDTXMania.act文字コンソール.tPrint( 0, 16, C文字コンソール.Eフォント種別.赤, "SUPER HARD MODE : ON" );
                if( CDTXMania.ConfigIni.eScrollMode == EScrollMode.BMSCROLL )
                    CDTXMania.act文字コンソール.tPrint( 0, 32, C文字コンソール.Eフォント種別.赤, "BMSCROLL : ON" );
                else if( CDTXMania.ConfigIni.eScrollMode == EScrollMode.HBSCROLL )
                    CDTXMania.act文字コンソール.tPrint( 0, 32, C文字コンソール.Eフォント種別.赤, "HBSCROLL : ON" );

                if( CDTXMania.ConfigIni.eGaugeMode == Eゲージモード.IIDX )
                    CDTXMania.act文字コンソール.tPrint( 240, 0, C文字コンソール.Eフォント種別.白, "GAUGE : IIDX" );
                else if( CDTXMania.ConfigIni.eGaugeMode == Eゲージモード.HARD )
                    CDTXMania.act文字コンソール.tPrint( 240, 0, C文字コンソール.Eフォント種別.白, "GAUGE : HARD" );
                else if( CDTXMania.ConfigIni.eGaugeMode == Eゲージモード.EXHARD )
                    CDTXMania.act文字コンソール.tPrint( 240, 0, C文字コンソール.Eフォント種別.白, "GAUGE : EX-HARD" );
                else if( CDTXMania.ConfigIni.eGaugeMode == Eゲージモード.DEATH )
                    CDTXMania.act文字コンソール.tPrint( 240, 0, C文字コンソール.Eフォント種別.赤, "GAUGE : DEATH" );

                if( CDTXMania.ConfigIni.nJustHIDDEN == 1 )
                    CDTXMania.act文字コンソール.tPrint( 240, 16, C文字コンソール.Eフォント種別.赤, "JUSTHIDDEN : TYPE-A" );
                else if( CDTXMania.ConfigIni.nJustHIDDEN == 2 )
                    CDTXMania.act文字コンソール.tPrint( 240, 16, C文字コンソール.Eフォント種別.赤, "JUSTHIDDEN : TYPE-B" );
                else if( CDTXMania.ConfigIni.nJustHIDDEN == 3 )
                    CDTXMania.act文字コンソール.tPrint( 240, 16, C文字コンソール.Eフォント種別.赤, "JUSTHIDDEN : TYPE-C" );

                if( CDTXMania.ConfigIni.bMonochlo )
                    CDTXMania.act文字コンソール.tPrint( 240, 32, C文字コンソール.Eフォント種別.赤, "NOTE : MONOCHRO" );

                if( CDTXMania.ConfigIni.bZeroSpeed )
                    CDTXMania.act文字コンソール.tPrint( 640, 0, C文字コンソール.Eフォント種別.赤, "ZERO-SPEED : ON" );
                #endregion
                #region[ 下部テキスト ]
                if( this.tx下部テキスト != null )
                {
                    if( CDTXMania.ConfigIni.b太鼓パートAutoPlay ) {
                        this.tx下部テキスト.t2D描画( CDTXMania.app.Device, 250 - ( 184 / 2 ), 660, new Rectangle( 0, 0, 184, 60 ) );
                    }
                    if( CDTXMania.ConfigIni.b太鼓パートAutoPlay2P ) {
                        this.tx下部テキスト.t2D描画( CDTXMania.app.Device, 1030 - ( 184 / 2 ), 660, new Rectangle( 0, 0, 184, 60 ) );
                    }
                }
                #endregion

                //this.actステータスパネル.On進行描画();

				this.actPresound.On進行描画();
				//if( this.txコメントバー != null )
				{
					//this.txコメントバー.t2D描画( CDTXMania.app.Device, 484, 314 );
				}
				//this.actArtistComment.On進行描画();
                if( !this.act難易度選択画面.bIsDifficltSelect )
                    this.act演奏履歴パネル.On進行描画();
				//this.actオプションパネル.On進行描画();
				this.actShowCurrentPosition.On進行描画();								// #27648 2011.3.28 yyagi

                //CDTXMania.act文字コンソール.tPrint( 0, 0, C文字コンソール.Eフォント種別.白, this.n現在選択中の曲の難易度.ToString() );
                if( CDTXMania.Skin.eDiffSelectMode == EDiffSelectMode.難易度から選ぶ )
                    this.tx難易度名?.t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nSelectDiffStringX, CDTXMania.Skin.nSelectDiffStringY, new Rectangle( 0, 70 * this.n現在選択中の曲の難易度, 260, 70 ) );

				if( !this.bBGM再生済み && ( base.eフェーズID == CStage.Eフェーズ.共通_通常状態 ) )
				{
					CDTXMania.Skin.bgm選曲画面.n音量_次に鳴るサウンド = 100;
					CDTXMania.Skin.bgm選曲画面.t再生する();
					this.bBGM再生済み = true;
				}


//Debug.WriteLine( "パンくず=" + this.r現在選択中の曲.strBreadcrumbs );
                if( this.ctDiffSelect移動待ち != null )
                    this.ctDiffSelect移動待ち.t進行();
                if( this.ctDiffSelect戻り待ち != null )
                    this.ctDiffSelect戻り待ち.t進行();

				// キー入力
				if( base.eフェーズID == CStage.Eフェーズ.共通_通常状態 
					&& CDTXMania.act現在入力を占有中のプラグイン == null )
				{
					#region [ 簡易CONFIGでMore、またはShift+F1: 詳細CONFIG呼び出し ]
					if (  actQuickConfig.bGotoDetailConfig )
					{	// 詳細CONFIG呼び出し
						actQuickConfig.tDeativatePopupMenu();
						this.actPresound.tサウンド停止();
						this.eフェードアウト完了時の戻り値 = E戻り値.コンフィグ呼び出し;	// #24525 2011.3.16 yyagi: [SHIFT]-[F1]でCONFIG呼び出し
						this.actFIFO.tフェードアウト開始();
						base.eフェーズID = CStage.Eフェーズ.共通_フェードアウト;
						CDTXMania.Skin.sound取消音.t再生する();
						return 0;
					}
					#endregion
					if ( !this.actSortSongs.bIsActivePopupMenu && !this.actQuickConfig.bIsActivePopupMenu && !this.act難易度選択画面.bIsDifficltSelect )
					{
						#region [ ESC ]
						if ( CDTXMania.Input管理.Keyboard.bキーが押された( (int) SlimDX.DirectInput.Key.Escape ) && ( this.act曲リスト.r現在選択中の曲.r親ノード == null ) )
						{	// [ESC]
							CDTXMania.Skin.sound取消音.t再生する();
							this.eフェードアウト完了時の戻り値 = E戻り値.タイトルに戻る;
                            //this.actFIFO.tフェードアウト開始();
                            this.actFIFOタイトル画面.tフェードアウト開始();
							base.eフェーズID = CStage.Eフェーズ.選曲_タイトル画面へのフェードアウト;
							return 0;
						}
						#endregion
						#region [ Shift-F1: CONFIG画面 ]
						if ( ( CDTXMania.Input管理.Keyboard.bキーが押されている( (int) SlimDX.DirectInput.Key.RightShift ) || CDTXMania.Input管理.Keyboard.bキーが押されている( (int) SlimDX.DirectInput.Key.LeftShift ) ) &&
							CDTXMania.Input管理.Keyboard.bキーが押された( (int) SlimDX.DirectInput.Key.F1 ) )
						{	// [SHIFT] + [F1] CONFIG
							this.actPresound.tサウンド停止();
							this.eフェードアウト完了時の戻り値 = E戻り値.コンフィグ呼び出し;	// #24525 2011.3.16 yyagi: [SHIFT]-[F1]でCONFIG呼び出し
							this.actFIFO.tフェードアウト開始();
							base.eフェーズID = CStage.Eフェーズ.共通_フェードアウト;
							CDTXMania.Skin.sound取消音.t再生する();
							return 0;
						}
						#endregion
						#region [ F2 簡易オプション ]
						if ( CDTXMania.Input管理.Keyboard.bキーが押された( (int) SlimDX.DirectInput.Key.F2 ) )
						{
                            CDTXMania.Skin.sound変更音.t再生する();
                            this.actQuickConfig.tActivatePopupMenu( E楽器パート.DRUMS );
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
						#region [ F5 スーパーハード ]
						if ( CDTXMania.Input管理.Keyboard.bキーが押された( (int) SlimDX.DirectInput.Key.F5 ) )
						{
							CDTXMania.Skin.sound変更音.t再生する();
                            C共通.bToggleBoolian( ref CDTXMania.ConfigIni.bSuperHard );
						}
						#endregion
						#region [ F6 SCROLL ]
						if ( CDTXMania.Input管理.Keyboard.bキーが押された( (int) SlimDX.DirectInput.Key.F6 ) )
						{
							CDTXMania.Skin.sound変更音.t再生する();
                            CDTXMania.ConfigIni.bスクロールモードを上書き = true;
                            switch( (int)CDTXMania.ConfigIni.eScrollMode )
                            {
                                case 0:
                                    CDTXMania.ConfigIni.eScrollMode = EScrollMode.BMSCROLL;
                                    break;
                                case 1:
                                    CDTXMania.ConfigIni.eScrollMode = EScrollMode.HBSCROLL;
                                    break;
                                case 2:
                                    CDTXMania.ConfigIni.eScrollMode = EScrollMode.Normal;
                                    CDTXMania.ConfigIni.bスクロールモードを上書き = false;
                                    break;
                            }
						}
						#endregion
						#region [ F7 JUST HIDDEN ]
						if ( CDTXMania.Input管理.Keyboard.bキーが押された( (int) SlimDX.DirectInput.Key.F7 ) )
						{
							CDTXMania.Skin.sound変更音.t再生する();
                            if( CDTXMania.ConfigIni.nJustHIDDEN < 3 )
                                CDTXMania.ConfigIni.nJustHIDDEN++;
                            else if( CDTXMania.ConfigIni.nJustHIDDEN == 3 )
                                CDTXMania.ConfigIni.nJustHIDDEN = 0;
						}
						#endregion
						#region [ F8 MONOCHRO ]
						if ( CDTXMania.Input管理.Keyboard.bキーが押された( (int) SlimDX.DirectInput.Key.F8 ) )
						{
							CDTXMania.Skin.sound変更音.t再生する();
                            C共通.bToggleBoolian( ref CDTXMania.ConfigIni.bMonochlo );
						}
						#endregion
						#region [ F9 ZERO-SPEED ]
						if ( CDTXMania.Input管理.Keyboard.bキーが押された( (int) SlimDX.DirectInput.Key.F9 ) )
						{
							CDTXMania.Skin.sound変更音.t再生する();
                            C共通.bToggleBoolian( ref CDTXMania.ConfigIni.bZeroSpeed );
						}
                        #endregion
                        #region [ TEST ]
                        if( CDTXMania.Input管理.Keyboard.bキーが押された( (int) SlimDX.DirectInput.Key.F11 ) )
                        {
                            //    CDTXMania.Skin.sound決定音.t再生する();
                            //    CDTXMania.Skin.sound曲読込開始音.t再生する();
                            //    if( !this.act難易度選択画面.bIsDifficltSelect )
                            //        this.ctDiffSelect移動待ち = new CCounter( 0, 1062, 1, CDTXMania.Timer );
                            //    this.act難易度選択画面.t選択画面初期化();
                            //    C共通.bToggleBoolian( ref this.act難易度選択画面.bIsDifficltSelect );
                            this.t左上テキスト登場();
                        }
                        //if( CDTXMania.Input管理.Keyboard.bキーが押された( (int) SlimDX.DirectInput.Key.NumberPad7 ) )
                        //{
                        //    this.t難易度選択画面を閉じる();
                        //}
						#endregion


						if( this.act曲リスト.r現在選択中の曲 != null && (!this.ctDiffSelect戻り待ち.b進行中 || this.ctDiffSelect戻り待ち.b終了値に達した) )
						{
							#region [ Decide ]
							if ( ( CDTXMania.Pad.b押されたDGB( Eパッド.Decide ) || ( CDTXMania.Pad.b押されたDGB( Eパッド.LRed ) || CDTXMania.Pad.b押されたDGB( Eパッド.RRed ) ) ||
								( ( CDTXMania.ConfigIni.bEnterがキー割り当てのどこにも使用されていない && CDTXMania.Input管理.Keyboard.bキーが押された( (int) SlimDX.DirectInput.Key.Return ) ) ) ) )
							{
								if ( this.act曲リスト.r現在選択中の曲 != null )
								{
									switch ( this.act曲リスト.r現在選択中の曲.eノード種別 )
									{
										case C曲リストノード.Eノード種別.SCORE:
                                            if( CDTXMania.Skin.eDiffSelectMode == EDiffSelectMode.曲から選ぶ )
                                            {
                                                CDTXMania.Skin.sound決定音.t再生する();
                                                CDTXMania.Skin.sound曲読込開始音.t再生する();
                                                this.t曲確定進行();
                                                if( !this.act難易度選択画面.bIsDifficltSelect )
                                                    this.ctDiffSelect移動待ち = new CCounter( 0, 1062, 1, CDTXMania.Timer );
                                                this.act難易度選択画面.t選択画面初期化();
                                                C共通.bToggleBoolian( ref this.act難易度選択画面.bIsDifficltSelect );
                                            }
                                            else
                                            {
                                                if( CDTXMania.Skin.sound曲決定音.b読み込み成功 )
                                                    CDTXMania.Skin.sound曲決定音.t再生する();
                                                else
                                                    CDTXMania.Skin.sound決定音.t再生する();
											    this.t曲を選択する();
                                            }
											break;

										case C曲リストノード.Eノード種別.BOX:
											{
                                                CDTXMania.Skin.sound決定音.t再生する();
												bool bNeedChangeSkin = this.act曲リスト.tBOXに入る();
												if ( bNeedChangeSkin )
												{
													this.eフェードアウト完了時の戻り値 = E戻り値.スキン変更; 
													base.eフェーズID = Eフェーズ.選曲_NowLoading画面へのフェードアウト;
												}
											}
											break;

										case C曲リストノード.Eノード種別.BACKBOX:
											{
                                                CDTXMania.Skin.sound決定音.t再生する();
												bool bNeedChangeSkin = this.act曲リスト.tBOXを出る();
												if ( bNeedChangeSkin )
												{
													this.eフェードアウト完了時の戻り値 = E戻り値.スキン変更; 
													base.eフェーズID = Eフェーズ.選曲_NowLoading画面へのフェードアウト;
												}
											}
											break;

										case C曲リストノード.Eノード種別.RANDOM:
                                            if( CDTXMania.Skin.eDiffSelectMode == EDiffSelectMode.曲から選ぶ )
                                            {
                                                CDTXMania.Skin.sound決定音.t再生する();
                                                CDTXMania.Skin.sound曲読込開始音.t再生する();
                                                if( !this.act難易度選択画面.bIsDifficltSelect )
                                                    this.ctDiffSelect移動待ち = new CCounter( 0, 1062, 1, CDTXMania.Timer );
                                                this.act難易度選択画面.t選択画面初期化();
                                                C共通.bToggleBoolian( ref this.act難易度選択画面.bIsDifficltSelect );
                                            }
                                            else
                                            {
                                                if( CDTXMania.Skin.sound曲決定音.b読み込み成功 )
                                                    CDTXMania.Skin.sound曲決定音.t再生する();
                                                else
                                                    CDTXMania.Skin.sound決定音.t再生する();
											    this.t曲をランダム選択する();
                                            }
											break;
									}
								}
							}
							#endregion
							#region [ Up ]
							this.ctキー反復用.Up.tキー反復( CDTXMania.Input管理.Keyboard.bキーが押されている( (int) SlimDX.DirectInput.Key.LeftArrow ), new CCounter.DGキー処理( this.tカーソルを上へ移動する ) );
							//this.ctキー反復用.Up.tキー反復( CDTXMania.Input管理.Keyboard.bキーが押されている( (int) SlimDX.DirectInput.Key.UpArrow ) || CDTXMania.Input管理.Keyboard.bキーが押されている( (int) SlimDX.DirectInput.Key.LeftArrow ), new CCounter.DGキー処理( this.tカーソルを上へ移動する ) );
							if ( CDTXMania.Pad.b押された( E楽器パート.DRUMS, Eパッド.LBlue ) )
							{
								this.tカーソルを上へ移動する();
							}
							#endregion
							#region [ Down ]
							this.ctキー反復用.Down.tキー反復( CDTXMania.Input管理.Keyboard.bキーが押されている( (int) SlimDX.DirectInput.Key.RightArrow ), new CCounter.DGキー処理( this.tカーソルを下へ移動する ) );
							//this.ctキー反復用.Down.tキー反復( CDTXMania.Input管理.Keyboard.bキーが押されている( (int) SlimDX.DirectInput.Key.DownArrow ) || CDTXMania.Input管理.Keyboard.bキーが押されている( (int) SlimDX.DirectInput.Key.RightArrow ), new CCounter.DGキー処理( this.tカーソルを下へ移動する ) );
							if ( CDTXMania.Pad.b押された( E楽器パート.DRUMS, Eパッド.RBlue ) )
							{
								this.tカーソルを下へ移動する();
							}
							#endregion
							#region [ Upstairs ]
							if ( ( ( this.act曲リスト.r現在選択中の曲 != null ) && ( this.act曲リスト.r現在選択中の曲.r親ノード != null ) ) && ( CDTXMania.Pad.b押された( E楽器パート.DRUMS, Eパッド.FT ) || CDTXMania.Pad.b押されたGB( Eパッド.Cancel ) ) )
							{
								this.actPresound.tサウンド停止();
								CDTXMania.Skin.sound取消音.t再生する();
								this.act曲リスト.tBOXを出る();
								this.t選択曲変更通知();
							}
							#endregion
							#region [ BDx2: 簡易CONFIG ]
                            if( CDTXMania.Input管理.Keyboard.bキーが押された( (int) SlimDX.DirectInput.Key.Space ) )
                            {
							    CDTXMania.Skin.sound変更音.t再生する();
								this.actSortSongs.tActivatePopupMenu( E楽器パート.DRUMS, ref this.act曲リスト );
                            }
							#endregion
							#region [ HHx2: 難易度変更 ]
							if ( CDTXMania.Pad.b押された( E楽器パート.DRUMS, Eパッド.HH ) || CDTXMania.Pad.b押された( E楽器パート.DRUMS, Eパッド.HHO ) )
							{	// [HH]x2 難易度変更
								CommandHistory.Add( E楽器パート.DRUMS, EパッドFlag.HH );
								EパッドFlag[] comChangeDifficulty = new EパッドFlag[] { EパッドFlag.HH, EパッドFlag.HH };
								if ( CommandHistory.CheckCommand( comChangeDifficulty, E楽器パート.DRUMS ) )
								{
									Debug.WriteLine( "ドラムス難易度変更" );
									this.act曲リスト.t難易度レベルをひとつ進める();
									CDTXMania.Skin.sound変更音.t再生する();
								}
							}
							#endregion
							#region [ 上: 難易度変更(上) ]
							if( CDTXMania.Input管理.Keyboard.bキーが押された( (int) SlimDX.DirectInput.Key.UpArrow ) )
							{
								//CommandHistory.Add( E楽器パート.DRUMS, EパッドFlag.HH );
								//EパッドFlag[] comChangeDifficulty = new EパッドFlag[] { EパッドFlag.HH, EパッドFlag.HH };
								//if ( CommandHistory.CheckCommand( comChangeDifficulty, E楽器パート.DRUMS ) )
								{
									Debug.WriteLine( "ドラムス難易度変更" );
                                    this.act曲リスト.t難易度レベルをひとつ進める();
									CDTXMania.Skin.sound変更音.t再生する();
								}
							}
							#endregion
							#region [ 下: 難易度変更(下) ]
							if( CDTXMania.Input管理.Keyboard.bキーが押された( (int) SlimDX.DirectInput.Key.DownArrow ) )
							{
								//CommandHistory.Add( E楽器パート.DRUMS, EパッドFlag.HH );
								//EパッドFlag[] comChangeDifficulty = new EパッドFlag[] { EパッドFlag.HH, EパッドFlag.HH };
								//if ( CommandHistory.CheckCommand( comChangeDifficulty, E楽器パート.DRUMS ) )
								{
									Debug.WriteLine( "ドラムス難易度変更" );
                                    this.act曲リスト.t難易度レベルをひとつ戻す();
									CDTXMania.Skin.sound変更音.t再生する();
								}
							}
							#endregion
						}
					}
					this.actSortSongs.t進行描画();
				}
                //------------------------------
                if (this.act難易度選択画面.bIsDifficltSelect)
                {
                    if (this.ctDiffSelect移動待ち?.n現在の値 == this.ctDiffSelect移動待ち?.n終了値)
                    {
                        this.act難易度選択画面.On進行描画();
                        //CDTXMania.act文字コンソール.tPrint(0, 0, C文字コンソール.Eフォント種別.赤, "NowStage:DifficltSelect");
                    }
                    //CDTXMania.act文字コンソール.tPrint(0, 16, C文字コンソール.Eフォント種別.赤, "Count:" + this.ctDiffSelect移動待ち?.n現在の値);

                }
                else if( this.ctDiffSelect戻り待ち.n現在の値 > 0 && this.ctDiffSelect戻り待ち.b終了値に達してない )
                {
                    if (this.ctDiffSelect戻り待ち?.n現在の値 == this.ctDiffSelect戻り待ち?.n終了値)
                    {
                        this.act難易度選択画面.On進行描画();
                        //CDTXMania.act文字コンソール.tPrint(0, 0, C文字コンソール.Eフォント種別.赤, "NowStage:DifficltSelect");
                    }
                    //CDTXMania.act文字コンソール.tPrint(0, 48, C文字コンソール.Eフォント種別.赤, "Count:" + this.ctDiffSelect戻り待ち?.n現在の値);
                }
                //------------------------------
                this.actQuickConfig.t進行描画(); // 2018.8.29 kairera0467 描画優先度が難易度選択より上になるよう修正
                switch ( base.eフェーズID )
				{
					case CStage.Eフェーズ.共通_フェードイン:
						if( this.actFIFO.On進行描画() != 0 )
						{
							base.eフェーズID = CStage.Eフェーズ.共通_通常状態;
						}
						break;

					case CStage.Eフェーズ.共通_フェードアウト:
						if( this.actFIFO.On進行描画() == 0 )
						{
							break;
						}
						return (int) this.eフェードアウト完了時の戻り値;

					case CStage.Eフェーズ.選曲_結果画面からのフェードイン:
						if( this.actFIfrom結果画面.On進行描画() != 0 )
						{
							base.eフェーズID = CStage.Eフェーズ.共通_通常状態;
						}
						break;

					case CStage.Eフェーズ.選曲_タイトル画面からのフェードイン:
						if( this.actFIFOタイトル画面.On進行描画() != 0 )
						{
							base.eフェーズID = CStage.Eフェーズ.共通_通常状態;
						}
						break;

                    case CStage.Eフェーズ.選曲_タイトル画面へのフェードアウト:
						if( this.actFIFOタイトル画面.On進行描画() == 0 )
						{
                            break;
						}
						return (int) this.eフェードアウト完了時の戻り値;

					case CStage.Eフェーズ.選曲_NowLoading画面へのフェードアウト:
						if( this.actFOtoNowLoading.On進行描画() == 0 )
						{
							break;
						}
						return (int) this.eフェードアウト完了時の戻り値;
				}
			}
			return 0;
		}
		public enum E戻り値 : int
		{
			継続,
			タイトルに戻る,
			選曲した,
			オプション呼び出し,
			コンフィグ呼び出し,
			スキン変更
		}
		

		// その他

		#region [ private ]
		//-----------------
		[StructLayout( LayoutKind.Sequential )]
		private struct STキー反復用カウンタ
		{
			public CCounter Up;
			public CCounter Down;
			public CCounter R;
			public CCounter B;
			public CCounter this[ int index ]
			{
				get
				{
					switch( index )
					{
						case 0:
							return this.Up;

						case 1:
							return this.Down;

						case 2:
							return this.R;

						case 3:
							return this.B;
					}
					throw new IndexOutOfRangeException();
				}
				set
				{
					switch( index )
					{
						case 0:
							this.Up = value;
							return;

						case 1:
							this.Down = value;
							return;

						case 2:
							this.R = value;
							return;

						case 3:
							this.B = value;
							return;
					}
					throw new IndexOutOfRangeException();
				}
			}
		}
		private CActSelectArtistComment actArtistComment;
		private CActFIFOBlack actFIFO;
		private CActFIFOBlack actFIfrom結果画面;
        private CActFIFOFace actFIFOタイトル画面;
		//private CActFIFOBlack actFOtoNowLoading;
        private CActFIFOStart actFOtoNowLoading;
		private CActSelectInformation actInformation;
		private CActSelectPreimageパネル actPreimageパネル;
		public CActSelectPresound actPresound;
		private CActオプションパネル actオプションパネル;
		private CActSelectステータスパネル actステータスパネル;
		public CActSelect演奏履歴パネル act演奏履歴パネル;
		public CActSelect曲リスト act曲リスト;
		private CActSelectShowCurrentPosition actShowCurrentPosition;
        public CActSelect難易度選択画面 act難易度選択画面;

		public CActSortSongs actSortSongs;
		public CActSelectQuickConfig actQuickConfig;

		private bool bBGM再生済み;
		private STキー反復用カウンタ ctキー反復用;
		public CCounter ct登場時アニメ用共通;
		private E戻り値 eフェードアウト完了時の戻り値;
		private Font ftフォント;
		private CTexture tx下部パネル;
		private CTexture tx上部パネル;
		private CTexture tx背景;
        private CTexture[] txジャンル別背景 = new CTexture[9];
        private CTexture[] tx難易度別背景 = new CTexture[5];
        private CTexture tx難易度名;
        private CTexture tx左上テキスト;
        private CTexture tx下部テキスト;
        public CCounter ctDiffSelect移動待ち;
        public CCounter ctDiffSelect戻り待ち;
        private CCounter ct背景スクロール;
        private int n背景ループ幅;
        private int n背景テクスチャ敷き詰め枚数;

		private struct STCommandTime		// #24063 2011.1.16 yyagi コマンド入力時刻の記録用
		{
			public E楽器パート eInst;		// 使用楽器
			public EパッドFlag ePad;		// 押されたコマンド(同時押しはOR演算で列挙する)
			public long time;				// コマンド入力時刻
		}
		private class CCommandHistory		// #24063 2011.1.16 yyagi コマンド入力履歴を保持_確認するクラス
		{
			readonly int buffersize = 16;
			private List<STCommandTime> stct;

			public CCommandHistory()		// コンストラクタ
			{
				stct = new List<STCommandTime>( buffersize );
			}

			/// <summary>
			/// コマンド入力履歴へのコマンド追加
			/// </summary>
			/// <param name="_eInst">楽器の種類</param>
			/// <param name="_ePad">入力コマンド(同時押しはOR演算で列挙すること)</param>
			public void Add( E楽器パート _eInst, EパッドFlag _ePad )
			{
				STCommandTime _stct = new STCommandTime {
					eInst = _eInst,
					ePad = _ePad,
					time = CDTXMania.Timer.n現在時刻
				};

				if ( stct.Count >= buffersize )
				{
					stct.RemoveAt( 0 );
				}
				stct.Add(_stct);
//Debug.WriteLine( "CMDHIS: 楽器=" + _stct.eInst + ", CMD=" + _stct.ePad + ", time=" + _stct.time );
			}
			public void RemoveAt( int index )
			{
				stct.RemoveAt( index );
			}

			/// <summary>
			/// コマンド入力に成功しているか調べる
			/// </summary>
			/// <param name="_ePad">入力が成功したか調べたいコマンド</param>
			/// <param name="_eInst">対象楽器</param>
			/// <returns>コマンド入力成功時true</returns>
			public bool CheckCommand( EパッドFlag[] _ePad, E楽器パート _eInst)
			{
				int targetCount = _ePad.Length;
				int stciCount = stct.Count;
				if ( stciCount < targetCount )
				{
//Debug.WriteLine("NOT start checking...stciCount=" + stciCount + ", targetCount=" + targetCount);
					return false;
				}

				long curTime = CDTXMania.Timer.n現在時刻;
//Debug.WriteLine("Start checking...targetCount=" + targetCount);
				for ( int i = targetCount - 1, j = stciCount - 1; i >= 0; i--, j-- )
				{
					if ( _ePad[ i ] != stct[ j ].ePad )
					{
//Debug.WriteLine( "CMD解析: false targetCount=" + targetCount + ", i=" + i + ", j=" + j + ": ePad[]=" + _ePad[i] + ", stci[j] = " + stct[j].ePad );
						return false;
					}
					if ( stct[ j ].eInst != _eInst )
					{
//Debug.WriteLine( "CMD解析: false " + i );
						return false;
					}
					if ( curTime - stct[ j ].time > 500 )
					{
//Debug.WriteLine( "CMD解析: false " + i + "; over 500ms" );
						return false;
					}
					curTime = stct[ j ].time;
				}

//Debug.Write( "CMD解析: 成功!(" + _ePad.Length + ") " );
//for ( int i = 0; i < _ePad.Length; i++ ) Debug.Write( _ePad[ i ] + ", " );
//Debug.WriteLine( "" );
				//stct.RemoveRange( 0, targetCount );			// #24396 2011.2.13 yyagi 
				stct.Clear();									// #24396 2011.2.13 yyagi Clear all command input history in case you succeeded inputting some command

				return true;
			}
		}
		private CCommandHistory CommandHistory;

		private void tカーソルを下へ移動する()
		{
			CDTXMania.Skin.soundカーソル移動音.t再生する();
			this.act曲リスト.t次に移動();
		}
		private void tカーソルを上へ移動する()
		{
			CDTXMania.Skin.soundカーソル移動音.t再生する();
			this.act曲リスト.t前に移動();
		}
		public void t曲をランダム選択する()
		{
			C曲リストノード song = this.act曲リスト.r現在選択中の曲;
			if( ( song.stackランダム演奏番号.Count == 0 ) || ( song.listランダム用ノードリスト == null ) )
			{
				if( song.listランダム用ノードリスト == null )
				{
					song.listランダム用ノードリスト = this.t指定された曲が存在する場所の曲を列挙する_子リスト含む( song );
				}
				int count = song.listランダム用ノードリスト.Count;
				if( count == 0 )
				{
					return;
				}
				int[] numArray = new int[ count ];
				for( int i = 0; i < count; i++ )
				{
					numArray[ i ] = i;
				}
				for( int j = 0; j < ( count * 1.5 ); j++ )
				{
					int index = CDTXMania.Random.Next( count );
					int num5 = CDTXMania.Random.Next( count );
					int num6 = numArray[ num5 ];
					numArray[ num5 ] = numArray[ index ];
					numArray[ index ] = num6;
				}
				for( int k = 0; k < count; k++ )
				{
					song.stackランダム演奏番号.Push( numArray[ k ] );
				}
				if( CDTXMania.ConfigIni.bLogDTX詳細ログ出力 )
				{
					StringBuilder builder = new StringBuilder( 0x400 );
					builder.Append( string.Format( "ランダムインデックスリストを作成しました: {0}曲: ", song.stackランダム演奏番号.Count ) );
					for( int m = 0; m < count; m++ )
					{
						builder.Append( string.Format( "{0} ", numArray[ m ] ) );
					}
					Trace.TraceInformation( builder.ToString() );
				}
			}
			this.r確定された曲 = song.listランダム用ノードリスト[ song.stackランダム演奏番号.Pop() ];
			this.n確定された曲の難易度 = this.act曲リスト.n現在のアンカ難易度レベルに最も近い難易度レベルを返す( this.r確定された曲 );
			this.r確定されたスコア = this.r確定された曲.arスコア[ this.n確定された曲の難易度 ];
			this.eフェードアウト完了時の戻り値 = E戻り値.選曲した;
			this.actFOtoNowLoading.tフェードアウト開始();					// #27787 2012.3.10 yyagi 曲決定時の画面フェードアウトの省略
			base.eフェーズID = CStage.Eフェーズ.選曲_NowLoading画面へのフェードアウト;
			if( CDTXMania.ConfigIni.bLogDTX詳細ログ出力 )
			{
				int[] numArray2 = song.stackランダム演奏番号.ToArray();
				StringBuilder builder2 = new StringBuilder( 0x400 );
				builder2.Append( "ランダムインデックスリスト残り: " );
				if( numArray2.Length > 0 )
				{
					for( int n = 0; n < numArray2.Length; n++ )
					{
						builder2.Append( string.Format( "{0} ", numArray2[ n ] ) );
					}
				}
				else
				{
					builder2.Append( "(なし)" );
				}
				Trace.TraceInformation( builder2.ToString() );
			}
			CDTXMania.Skin.bgm選曲画面.t停止する();
		}
		private void t曲を選択する()
		{
			this.r確定された曲 = this.act曲リスト.r現在選択中の曲;
			this.r確定されたスコア = this.act曲リスト.r現在選択中のスコア;
			this.n確定された曲の難易度 = this.act曲リスト.n現在選択中の曲の現在の難易度レベル;
			if( ( this.r確定された曲 != null ) && ( this.r確定されたスコア != null ) )
			{
				this.eフェードアウト完了時の戻り値 = E戻り値.選曲した;
				this.actFOtoNowLoading.tフェードアウト開始();				// #27787 2012.3.10 yyagi 曲決定時の画面フェードアウトの省略
				base.eフェーズID = CStage.Eフェーズ.選曲_NowLoading画面へのフェードアウト;
			}
			CDTXMania.Skin.bgm選曲画面.t停止する();
		}
		public void t曲を選択する( int nCurrentLevel )
		{
			this.r確定された曲 = this.act曲リスト.r現在選択中の曲;
			this.r確定されたスコア = this.act曲リスト.r現在選択中のスコア;
			this.n確定された曲の難易度 = nCurrentLevel;
			if( ( this.r確定された曲 != null ) && ( this.r確定されたスコア != null ) )
			{
				this.eフェードアウト完了時の戻り値 = E戻り値.選曲した;
				this.actFOtoNowLoading.tフェードアウト開始();				// #27787 2012.3.10 yyagi 曲決定時の画面フェードアウトの省略
				base.eフェーズID = CStage.Eフェーズ.選曲_NowLoading画面へのフェードアウト;
			}

			CDTXMania.Skin.bgm選曲画面.t停止する();
		}
		private List<C曲リストノード> t指定された曲が存在する場所の曲を列挙する_子リスト含む( C曲リストノード song )
		{
			List<C曲リストノード> list = new List<C曲リストノード>();
			song = song.r親ノード;
			if( ( song == null ) && ( CDTXMania.Songs管理.list曲ルート.Count > 0 ) )
			{
				foreach( C曲リストノード c曲リストノード in CDTXMania.Songs管理.list曲ルート )
				{
					if( ( c曲リストノード.eノード種別 == C曲リストノード.Eノード種別.SCORE ) || ( c曲リストノード.eノード種別 == C曲リストノード.Eノード種別.SCORE_MIDI ) )
					{
						list.Add( c曲リストノード );
					}
					if( ( c曲リストノード.list子リスト != null ) && CDTXMania.ConfigIni.bランダムセレクトで子BOXを検索対象とする )
					{
						this.t指定された曲の子リストの曲を列挙する_孫リスト含む( c曲リストノード, ref list );
					}
				}
				return list;
			}
			this.t指定された曲の子リストの曲を列挙する_孫リスト含む( song, ref list );
			return list;
		}
		private void t指定された曲の子リストの曲を列挙する_孫リスト含む( C曲リストノード r親, ref List<C曲リストノード> list )
		{
			if( ( r親 != null ) && ( r親.list子リスト != null ) )
			{
				foreach( C曲リストノード c曲リストノード in r親.list子リスト )
				{
					if( ( c曲リストノード.eノード種別 == C曲リストノード.Eノード種別.SCORE ) || ( c曲リストノード.eノード種別 == C曲リストノード.Eノード種別.SCORE_MIDI ) )
					{
						list.Add( c曲リストノード );
					}
					if( ( c曲リストノード.list子リスト != null ) && CDTXMania.ConfigIni.bランダムセレクトで子BOXを検索対象とする )
					{
						this.t指定された曲の子リストの曲を列挙する_孫リスト含む( c曲リストノード, ref list );
					}
				}
			}
		}

        public void t難易度選択画面を閉じる()
        {
            CDTXMania.Skin.sound取消音.t再生する();
            if( this.act難易度選択画面.bIsDifficltSelect )
                this.ctDiffSelect戻り待ち = new CCounter( 0, 1062, 1, CDTXMania.Timer );
            //this.act難易度選択画面.t選択画面初期化();
            this.act難易度選択画面.bIsDifficltSelect = false;

            float f速度倍率 = 1.0f;
            double 秒( double v ) => ( v / f速度倍率 );
            var animation = CDTXMania.AnimationManager;
            var basetime = animation.Timer.Time;
            var start = basetime;

            var text = this._左上テキスト;
            text.Dispose();
            text.var左上テキスト位置X = new Variable( animation.Manager, 0 );
            text.var左上テキスト位置Y = new Variable( animation.Manager, 0 );
            text.var左上テキスト不透明度 = new Variable( animation.Manager, 255 );
            text.var表示内容 = new Variable( animation.Manager, 1 );
            text.sb左上テキスト = new Storyboard( animation.Manager );
            
            using ( var 透明度 = animation.TrasitionLibrary.Linear( 秒( 0.1 ), 0 ) )
            using ( var 内容 = animation.TrasitionLibrary.Constant( 秒( 0.1 ) ) )
            using ( var 移動X = animation.TrasitionLibrary.Constant(秒(0.1)))
            {
                text.sb左上テキスト.AddTransition( text.var左上テキスト不透明度, 透明度 );
                text.sb左上テキスト.AddTransition( text.var表示内容, 内容 );
                text.sb左上テキスト.AddTransition(text.var左上テキスト位置X, 移動X);
            }
            using (var 透明度 = animation.TrasitionLibrary.Constant(秒(0.5)))
            using (var 内容 = animation.TrasitionLibrary.Constant(秒(0.5)))
            using(var 移動X = animation.TrasitionLibrary.Constant(秒(0.5))) 
            {
                text.sb左上テキスト.AddTransition(text.var左上テキスト不透明度, 透明度);
                text.sb左上テキスト.AddTransition(text.var表示内容, 内容);
                text.sb左上テキスト.AddTransition(text.var左上テキスト位置X, 移動X);
            }
            using ( var 透明度 = animation.TrasitionLibrary.Constant( 秒( 0.7 ) ) )
            using ( var 内容 = animation.TrasitionLibrary.Linear( 秒( 0.7 ), 0 ) )
            using ( var 移動X = animation.TrasitionLibrary.Linear(秒(0.7), -120))
            {
                text.sb左上テキスト.AddTransition( text.var左上テキスト不透明度, 透明度 );
                text.sb左上テキスト.AddTransition( text.var表示内容, 内容 );
                text.sb左上テキスト.AddTransition(text.var左上テキスト位置X, 移動X);
            }
            using (var 移動X = animation.TrasitionLibrary.AccelerateDecelerate(秒(0.25), 20, 0.3, 0.7))
            using (var 透明度 = animation.TrasitionLibrary.Linear( 秒( 0.25 ), 255 ) )
            {
                text.sb左上テキスト.AddTransition(text.var左上テキスト位置X, 移動X);
                text.sb左上テキスト.AddTransition(text.var左上テキスト不透明度, 透明度);
            }
            using ( var 移動X = animation.TrasitionLibrary.AccelerateDecelerate( 秒(0.05), 0, 0.5, 0.5 ) )
            {
                text.sb左上テキスト.AddTransition( text.var左上テキスト位置X, 移動X );
            }
            text.sb左上テキスト.Schedule( start );
        }

        private int nStrジャンルtoNum( string strジャンル )
        {
            int nGenre = 8;
            switch( strジャンル )
            {
                case "アニメ":
                    nGenre = 0;
                    break;
                case "J-POP":
                    nGenre = 1;
                    break;
                case "ゲームミュージック":
                    nGenre = 2;
                    break;
                case "ナムコオリジナル":
                    nGenre = 3;
                    break;
                case "クラシック":
                    nGenre = 4;
                    break;
                case "どうよう":
                    nGenre = 5;
                    break;
                case "バラエティ":
                    nGenre = 6;
                    break;
                case "ボーカロイド":
                case "VOCALOID":
                    nGenre = 7;
                    break;
                default:
                    nGenre = 8;
                    break;

            }

            return nGenre;
        }

        private void t左上テキスト登場()
        {
            float f速度倍率 = 1.0f;
            double 秒( double v ) => ( v / f速度倍率 );
            var animation = CDTXMania.AnimationManager;
            var basetime = animation.Timer.Time;
            var start = basetime;

            var text = this._左上テキスト;
            text.Dispose();
            text.var左上テキスト位置X = new Variable( animation.Manager, -140 );
            text.var左上テキスト位置Y = new Variable( animation.Manager, 0 );
            text.var左上テキスト不透明度 = new Variable( animation.Manager, 0 );
            text.var表示内容 = new Variable( animation.Manager, 0 );
            text.sb左上テキスト = new Storyboard( animation.Manager );

            using (var 移動X = animation.TrasitionLibrary.AccelerateDecelerate(秒(0.25), 20, 0.3, 0.7))
            using (var 透明度 = animation.TrasitionLibrary.Linear( 秒( 0.25 ), 255 ) )
            {
                text.sb左上テキスト.AddTransition(text.var左上テキスト位置X, 移動X);
                text.sb左上テキスト.AddTransition(text.var左上テキスト不透明度, 透明度);
            }
            using ( var 移動X = animation.TrasitionLibrary.AccelerateDecelerate( 秒(0.05), 0, 0.5, 0.5 ) )
            {
                text.sb左上テキスト.AddTransition( text.var左上テキスト位置X, 移動X );
            }
            text.sb左上テキスト.Schedule( start );
        }

        private void t曲確定進行()
        {
            float f速度倍率 = 1.0f;
            double 秒( double v ) => ( v / f速度倍率 );
            var animation = CDTXMania.AnimationManager;
            var basetime = animation.Timer.Time;
            var start = basetime;

            var text = this._左上テキスト;
            text.Dispose();
            text.var左上テキスト位置X = new Variable( animation.Manager, 0 );
            text.var左上テキスト位置Y = new Variable( animation.Manager, 0 );
            text.var左上テキスト不透明度 = new Variable( animation.Manager, 255 );
            text.var表示内容 = new Variable( animation.Manager, 0 );
            text.sb左上テキスト = new Storyboard( animation.Manager );

            using (var 透明度 = animation.TrasitionLibrary.Constant(秒(0.5)))
            using (var 内容 = animation.TrasitionLibrary.Constant(秒(0.5)))
            using(var 移動X = animation.TrasitionLibrary.Constant(秒(0.5))) 
            {
                text.sb左上テキスト.AddTransition(text.var左上テキスト不透明度, 透明度);
                text.sb左上テキスト.AddTransition(text.var表示内容, 内容);
                text.sb左上テキスト.AddTransition(text.var左上テキスト位置X, 移動X);
            }
            using ( var 透明度 = animation.TrasitionLibrary.Linear( 秒( 0.1 ), 0 ) )
            using ( var 内容 = animation.TrasitionLibrary.Constant( 秒( 0.1 ) ) )
            using ( var 移動X = animation.TrasitionLibrary.Constant(秒(0.1)))
            {
                text.sb左上テキスト.AddTransition( text.var左上テキスト不透明度, 透明度 );
                text.sb左上テキスト.AddTransition( text.var表示内容, 内容 );
                text.sb左上テキスト.AddTransition(text.var左上テキスト位置X, 移動X);
            }
            using ( var 透明度 = animation.TrasitionLibrary.Constant( 秒( 0.7 ) ) )
            using ( var 内容 = animation.TrasitionLibrary.Linear( 秒( 0.7 ), 1 ) )
            using ( var 移動X = animation.TrasitionLibrary.Linear(秒(0.7), -120))
            {
                text.sb左上テキスト.AddTransition( text.var左上テキスト不透明度, 透明度 );
                text.sb左上テキスト.AddTransition( text.var表示内容, 内容 );
                text.sb左上テキスト.AddTransition(text.var左上テキスト位置X, 移動X);
            }
            using (var 移動X = animation.TrasitionLibrary.AccelerateDecelerate(秒(0.25), 20, 0.3, 0.7))
            using (var 透明度 = animation.TrasitionLibrary.Linear( 秒( 0.25 ), 255 ) )
            {
                text.sb左上テキスト.AddTransition(text.var左上テキスト位置X, 移動X);
                text.sb左上テキスト.AddTransition(text.var左上テキスト不透明度, 透明度);
            }
            using ( var 移動X = animation.TrasitionLibrary.AccelerateDecelerate( 秒(0.05), 0, 0.5, 0.5 ) )
            {
                text.sb左上テキスト.AddTransition( text.var左上テキスト位置X, 移動X );
            }
            text.sb左上テキスト.Schedule( start );
        }

        protected class 左上テキスト : IDisposable
        {
            public Storyboard sb左上テキスト;
            public Variable var左上テキスト位置X;
            public Variable var左上テキスト位置Y;
            public Variable var左上テキスト不透明度;
            /// <summary>
            /// 0:曲を選ぶ 1:難易度を選ぶ
            /// </summary>
            public Variable var表示内容;
            public void Dispose()
            {
                this.sb左上テキスト?.Abandon();
                this.sb左上テキスト = null;

                this.var左上テキスト位置X?.Dispose();
                this.var左上テキスト位置X = null;

                this.var左上テキスト位置Y?.Dispose();
                this.var左上テキスト位置Y = null;

                this.var左上テキスト不透明度?.Dispose();
                this.var左上テキスト不透明度 = null;

                this.var表示内容?.Dispose();
                this.var表示内容 = null;
            }
        }
        protected 左上テキスト _左上テキスト;
		//-----------------
		#endregion
	}
}
