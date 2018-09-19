using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using SlimDX;
using System.Drawing.Text;
using FDK;

namespace DTXMania
{
	internal class CStage曲読み込み : CStage
	{
		// コンストラクタ

		public CStage曲読み込み()
		{
			base.eステージID = CStage.Eステージ.曲読み込み;
			base.eフェーズID = CStage.Eフェーズ.共通_通常状態;
			base.b活性化してない = true;
			//base.list子Activities.Add( this.actFI = new CActFIFOBlack() );	// #27787 2012.3.10 yyagi 曲読み込み画面のフェードインの省略
			//base.list子Activities.Add( this.actFO = new CActFIFOBlack() );
		}


		// CStage 実装

		public override void On活性化()
		{
			Trace.TraceInformation( "曲読み込みステージを活性化します。" );
			Trace.Indent();
			try
			{
				this.str曲タイトル = "";
				this.strSTAGEFILE = "";
                if( !string.IsNullOrEmpty( CDTXMania.ConfigIni.FontName ) )
                {
                    this.pfTITLE = new CPrivateFastFont( new FontFamily( CDTXMania.ConfigIni.FontName ), 30 );
                    this.pfSUBTITLE = new CPrivateFastFont( new FontFamily( CDTXMania.ConfigIni.FontName ), 22 );
                }
                else
                {
                    this.pfTITLE = new CPrivateFastFont( new FontFamily("MS UI Gothic"), 30 );
                    this.pfSUBTITLE = new CPrivateFastFont( new FontFamily("MS UI Gothic" ), 22 );
                }
				this.nBGM再生開始時刻 = -1;
				this.nBGMの総再生時間ms = 0;
				if( this.sd読み込み音 != null )
				{
					CDTXMania.Sound管理.tサウンドを破棄する( this.sd読み込み音 );
					this.sd読み込み音 = null;
				}

				string strDTXファイルパス = ( CDTXMania.bコンパクトモード ) ?
					CDTXMania.strコンパクトモードファイル : CDTXMania.stage選曲.r確定されたスコア.ファイル情報.ファイルの絶対パス;
				
				CDTX cdtx = new CDTX( strDTXファイルパス, true, 1.0, 0, 0 );
                if( File.Exists( cdtx.strフォルダ名 + @"set.def" ) )
				    cdtx = new CDTX( strDTXファイルパス, true, 1.0, 0, 1 );

				this.str曲タイトル = cdtx.TITLE;
                this.strサブタイトル = cdtx.SUBTITLE;
				this.strSTAGEFILE = CSkin.Path(@"Graphics\4_SongLoading\Background.png");
				cdtx.On非活性化();
				base.On活性化();
			}
			finally
			{
				Trace.TraceInformation( "曲読み込みステージの活性化を完了しました。" );
				Trace.Unindent();
			}
		}
		public override void On非活性化()
		{
			Trace.TraceInformation( "曲読み込みステージを非活性化します。" );
			Trace.Indent();
			try
			{
                CDTXMania.t安全にDisposeする(ref this.pfTITLE);
                CDTXMania.t安全にDisposeする(ref this.pfSUBTITLE);
                base.On非活性化();
			}
			finally
			{
				Trace.TraceInformation( "曲読み込みステージの非活性化を完了しました。" );
				Trace.Unindent();
			}
		}
		public override void OnManagedリソースの作成()
		{
			if( !base.b活性化してない )
			{
				this.tx背景 = CDTXMania.tテクスチャの生成( this.strSTAGEFILE, false );
                //this.txSongnamePlate = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\6_SongnamePlate.png" ) );
                this.ct待機 = new CCounter( 0, 600, 5, CDTXMania.Timer );
                this.ct曲名表示 = new CCounter( 1, 30, 30, CDTXMania.Timer );
				try
				{
					if( ( this.str曲タイトル != null ) && ( this.str曲タイトル.Length > 0 ) )
					{
                        //this.txタイトル = new CTexture( CDTXMania.app.Device, image, CDTXMania.TextureFormat );
                        //this.txタイトル.vc拡大縮小倍率 = new Vector3( 0.5f, 0.5f, 1f );

                        Bitmap bmpSongTitle = new Bitmap(1, 1);
                        bmpSongTitle = this.pfTITLE.DrawPrivateFont( this.str曲タイトル, Color.White, Color.Black );
						this.txタイトル = new CTexture( CDTXMania.app.Device, bmpSongTitle, CDTXMania.TextureFormat, false );
                        txタイトル.vc拡大縮小倍率.X = CDTXMania.GetSongNameXScaling(ref txタイトル, 710);
                        Bitmap bmpSongSubTitle = new Bitmap(1, 1);
                        bmpSongSubTitle = this.pfSUBTITLE.DrawPrivateFont( this.strサブタイトル, Color.White, Color.Black );
						this.txサブタイトル = new CTexture( CDTXMania.app.Device, bmpSongSubTitle, CDTXMania.TextureFormat, false );

                        //image.Dispose();
                        CDTXMania.t安全にDisposeする( ref bmpSongTitle );
                        CDTXMania.t安全にDisposeする( ref bmpSongSubTitle );
                        
                    }
					else
					{
						this.txタイトル = null;
                        this.txサブタイトル = null;
                    }
				}
				catch( CTextureCreateFailedException e )
				{
					Trace.TraceError( e.ToString() );
					Trace.TraceError( "テクスチャの生成に失敗しました。({0})", new object[] { this.strSTAGEFILE } );
					this.txタイトル = null;
                    this.txサブタイトル = null;
                    this.tx背景 = null;
				}
				base.OnManagedリソースの作成();
			}
		}
		public override void OnManagedリソースの解放()
		{
			if( !base.b活性化してない )
			{
				CDTXMania.tテクスチャの解放( ref this.tx背景 );
				CDTXMania.tテクスチャの解放( ref this.txタイトル );
				//CDTXMania.tテクスチャの解放( ref this.txSongnamePlate );
                CDTXMania.tテクスチャの解放( ref this.txサブタイトル );
				base.OnManagedリソースの解放();
			}
		}
		public override int On進行描画()
		{
			string str;

			if( base.b活性化してない )
				return 0;

			#region [ 初めての進行描画 ]
			//-----------------------------
			if( base.b初めての進行描画 )
			{
				Cスコア cスコア1 = CDTXMania.stage選曲.r確定されたスコア;
				if( this.sd読み込み音 != null )
				{
					if( CDTXMania.Skin.sound曲読込開始音.b排他 && ( CSkin.Cシステムサウンド.r最後に再生した排他システムサウンド != null ) )
					{
						CSkin.Cシステムサウンド.r最後に再生した排他システムサウンド.t停止する();
					}
					this.sd読み込み音.t再生を開始する();
					this.nBGM再生開始時刻 = CSound管理.rc演奏用タイマ.n現在時刻;
					this.nBGMの総再生時間ms = this.sd読み込み音.n総演奏時間ms;
				}
				else
				{
					CDTXMania.Skin.sound曲読込開始音.t再生する();
					this.nBGM再生開始時刻 = CSound管理.rc演奏用タイマ.n現在時刻;
					this.nBGMの総再生時間ms = CDTXMania.Skin.sound曲読込開始音.n長さ_現在のサウンド;
				}
				//this.actFI.tフェードイン開始();							// #27787 2012.3.10 yyagi 曲読み込み画面のフェードインの省略
				base.eフェーズID = CStage.Eフェーズ.共通_フェードイン;
				base.b初めての進行描画 = false;

				nWAVcount = 1;
				bitmapFilename = new Bitmap( 640, 24 );
				graphicsFilename = Graphics.FromImage( bitmapFilename );
				graphicsFilename.TextRenderingHint = TextRenderingHint.AntiAlias;
				ftFilename = new Font("MS UI Gothic", 24f, FontStyle.Bold, GraphicsUnit.Pixel );
			}
			//-----------------------------
			#endregion
            this.ct待機.t進行();



			#region [ ESC押下時は選曲画面に戻る ]
			if ( tキー入力() )
			{
				if ( this.sd読み込み音 != null )
				{
					this.sd読み込み音.tサウンドを停止する();
					this.sd読み込み音.t解放する();
				}
				return (int) E曲読込画面の戻り値.読込中止;
			}
			#endregion

			#region [ 背景、音符＋タイトル表示 ]
			//-----------------------------
            this.ct曲名表示.t進行();
			if( this.tx背景 != null )
				this.tx背景.t2D描画( CDTXMania.app.Device, 0, 0 );
            //CDTXMania.act文字コンソール.tPrint( 0, 0, C文字コンソール.Eフォント種別.灰, this.ct曲名表示.n現在の値.ToString() );

            if( CDTXMania.Tx.SongLoading_Plate != null )
            {
                CDTXMania.Tx.SongLoading_Plate.bスクリーン合成 = true; //あまりにも出番が無い
                CDTXMania.Tx.SongLoading_Plate.n透明度 = C変換.nParsentTo255( ( this.ct曲名表示.n現在の値 / 30.0 ) );
                CDTXMania.Tx.SongLoading_Plate.t2D描画( CDTXMania.app.Device, 640 - (CDTXMania.Tx.SongLoading_Plate.sz画像サイズ.Width / 2 ), 360 - (CDTXMania.Tx.SongLoading_Plate.sz画像サイズ.Height / 2 ) );
            }
            //CDTXMania.act文字コンソール.tPrint( 0, 16, C文字コンソール.Eフォント種別.灰, C変換.nParsentTo255( ( this.ct曲名表示.n現在の値 / 30.0 ) ).ToString() );


			int y = 720 - 45;
			if( this.txタイトル != null )
			{
                int nサブタイトル補正 = string.IsNullOrEmpty(CDTXMania.stage選曲.r確定されたスコア.譜面情報.strサブタイトル) ? 15 : 0;

                this.txタイトル.n透明度 = C変換.nParsentTo255( ( this.ct曲名表示.n現在の値 / 30.0 ) );
				this.txタイトル.t2D描画( CDTXMania.app.Device, ( 640 - ( (this.txタイトル.sz画像サイズ.Width * txタイトル.vc拡大縮小倍率.X) / 2 ) ), 340 - ( this.txタイトル.sz画像サイズ.Height / 2 ) + nサブタイトル補正 );
			}
			if( this.txサブタイトル != null )
			{
                this.txサブタイトル.n透明度 = C変換.nParsentTo255( ( this.ct曲名表示.n現在の値 / 30.0 ) );
				this.txサブタイトル.t2D描画( CDTXMania.app.Device, ( 640 - ( this.txサブタイトル.sz画像サイズ.Width / 2 ) ), 390 - ( this.txサブタイトル.sz画像サイズ.Height / 2 ) );
			}
			//-----------------------------
			#endregion

			switch( base.eフェーズID )
			{
				case CStage.Eフェーズ.共通_フェードイン:
					//if( this.actFI.On進行描画() != 0 )			    // #27787 2012.3.10 yyagi 曲読み込み画面のフェードインの省略
																		// 必ず一度「CStaeg.Eフェーズ.共通_フェードイン」フェーズを経由させること。
																		// さもないと、曲読み込みが完了するまで、曲読み込み画面が描画されない。
						base.eフェーズID = CStage.Eフェーズ.NOWLOADING_DTXファイルを読み込む;
					return (int) E曲読込画面の戻り値.継続;

				case CStage.Eフェーズ.NOWLOADING_DTXファイルを読み込む:
					{
						timeBeginLoad = DateTime.Now;
						TimeSpan span;
						str = null;
						if( !CDTXMania.bコンパクトモード )
							str = CDTXMania.stage選曲.r確定されたスコア.ファイル情報.ファイルの絶対パス;
						else
							str = CDTXMania.strコンパクトモードファイル;

						CScoreIni ini = new CScoreIni( str + ".score.ini" );
						ini.t全演奏記録セクションの整合性をチェックし不整合があればリセットする();

						if( ( CDTXMania.DTX != null ) && CDTXMania.DTX.b活性化してる )
							CDTXMania.DTX.On非活性化();

                        //if( CDTXMania.DTX == null )
                        {
						    CDTXMania.DTX = new CDTX( str, false, ( (double) CDTXMania.ConfigIni.n演奏速度 ) / 20.0, ini.stファイル.BGMAdjust, 0, 0, true );
                            if( CDTXMania.ConfigIni.nPlayerCount == 2 )
						        CDTXMania.DTX_2P = new CDTX( str, false, ( (double) CDTXMania.ConfigIni.n演奏速度 ) / 20.0, ini.stファイル.BGMAdjust, 0, 1, true );
                            if( File.Exists( CDTXMania.DTX.strフォルダ名 + @"\\set.def" ) )
                            {
						        CDTXMania.DTX = new CDTX( str, false, ( (double) CDTXMania.ConfigIni.n演奏速度 ) / 20.0, ini.stファイル.BGMAdjust, 0, 1, true );
                                if( CDTXMania.ConfigIni.nPlayerCount == 2 )
						            CDTXMania.DTX_2P = new CDTX( str, false, ( (double) CDTXMania.ConfigIni.n演奏速度 ) / 20.0, ini.stファイル.BGMAdjust, 0, 1, true );
                            }


					    	Trace.TraceInformation( "----曲情報-----------------" );
				    		Trace.TraceInformation( "TITLE: {0}", CDTXMania.DTX.TITLE );
			    			Trace.TraceInformation( "FILE: {0}",  CDTXMania.DTX.strファイル名の絶対パス );
		    				Trace.TraceInformation( "---------------------------" );

	    					span = (TimeSpan) ( DateTime.Now - timeBeginLoad );
    						Trace.TraceInformation( "DTX読込所要時間:           {0}", span.ToString() );
                        }

                        //2017.01.28 DD Config.iniに反映しないように変更
                        /*
                        switch( CDTXMania.DTX.nScoreModeTmp )
                        {
                            case 0:
                                CDTXMania.ConfigIni.nScoreMode = 0;
                                break;
                            case 1:
                                CDTXMania.ConfigIni.nScoreMode = 1;
                                break;
                            case 2:
                                CDTXMania.ConfigIni.nScoreMode = 2;
                                break;
                            case -1:
                                CDTXMania.ConfigIni.nScoreMode = 1;
                                break;
                        }
                        */

						base.eフェーズID = CStage.Eフェーズ.NOWLOADING_WAV読み込み待機;
						timeBeginLoadWAV = DateTime.Now;
						return (int) E曲読込画面の戻り値.継続;
					}

                case CStage.Eフェーズ.NOWLOADING_WAV読み込み待機:
                    {
                        if( this.ct待機.n現在の値 > 260 )
                        {
						    base.eフェーズID = CStage.Eフェーズ.NOWLOADING_WAVファイルを読み込む;
                        }
						return (int) E曲読込画面の戻り値.継続;
                    }

				case CStage.Eフェーズ.NOWLOADING_WAVファイルを読み込む:
					{
						if ( nWAVcount == 1 && CDTXMania.DTX.listWAV.Count > 0 )			// #28934 2012.7.7 yyagi (added checking Count)
						{
							ShowProgressByFilename( CDTXMania.DTX.listWAV[ nWAVcount ].strファイル名 );
						}
						int looptime = (CDTXMania.ConfigIni.b垂直帰線待ちを行う)? 3 : 1;	// VSyncWait=ON時は1frame(1/60s)あたり3つ読むようにする
						for ( int i = 0; i < looptime && nWAVcount <= CDTXMania.DTX.listWAV.Count; i++ )
						{
							if ( CDTXMania.DTX.listWAV[ nWAVcount ].listこのWAVを使用するチャンネル番号の集合.Count > 0 )	// #28674 2012.5.8 yyagi
							{
								CDTXMania.DTX.tWAVの読み込み( CDTXMania.DTX.listWAV[ nWAVcount ] );
							}
							nWAVcount++;
						}
						if ( nWAVcount <= CDTXMania.DTX.listWAV.Count )
						{
							ShowProgressByFilename( CDTXMania.DTX.listWAV[ nWAVcount ].strファイル名 );
						}
						if ( nWAVcount > CDTXMania.DTX.listWAV.Count )
						{
							TimeSpan span = ( TimeSpan ) ( DateTime.Now - timeBeginLoadWAV );
							Trace.TraceInformation( "WAV読込所要時間({0,4}):     {1}", CDTXMania.DTX.listWAV.Count, span.ToString() );
							timeBeginLoadWAV = DateTime.Now;

							if ( CDTXMania.ConfigIni.bDynamicBassMixerManagement )
							{
								CDTXMania.DTX.PlanToAddMixerChannel();
							}
                            CDTXMania.DTX.t太鼓チップのランダム化( CDTXMania.ConfigIni.eRandom.Taiko );

							CDTXMania.stage演奏ドラム画面.On活性化();

							span = (TimeSpan) ( DateTime.Now - timeBeginLoadWAV );

							base.eフェーズID = CStage.Eフェーズ.NOWLOADING_BMPファイルを読み込む;
						}
						return (int) E曲読込画面の戻り値.継続;
					}

				case CStage.Eフェーズ.NOWLOADING_BMPファイルを読み込む:
					{
						TimeSpan span;
						DateTime timeBeginLoadBMPAVI = DateTime.Now;

						if ( CDTXMania.ConfigIni.bAVI有効 )
							CDTXMania.DTX.tAVIの読み込み();
						span = ( TimeSpan ) ( DateTime.Now - timeBeginLoadBMPAVI );

						span = ( TimeSpan ) ( DateTime.Now - timeBeginLoad );
						Trace.TraceInformation( "総読込時間:                {0}", span.ToString() );

						if ( bitmapFilename != null )
						{
							bitmapFilename.Dispose();
							bitmapFilename = null;
						}
						if ( graphicsFilename != null )
						{
							graphicsFilename.Dispose();
							graphicsFilename = null;
						}
						if ( ftFilename != null )
						{
							ftFilename.Dispose();
							ftFilename = null;
						}
						CDTXMania.Timer.t更新();
                        //CSound管理.rc演奏用タイマ.t更新();
						base.eフェーズID = CStage.Eフェーズ.NOWLOADING_システムサウンドBGMの完了を待つ;
						return (int) E曲読込画面の戻り値.継続;
					}

				case CStage.Eフェーズ.NOWLOADING_システムサウンドBGMの完了を待つ:
					{
						long nCurrentTime = CDTXMania.Timer.n現在時刻;
						if( nCurrentTime < this.nBGM再生開始時刻 )
							this.nBGM再生開始時刻 = nCurrentTime;

//						if ( ( nCurrentTime - this.nBGM再生開始時刻 ) > ( this.nBGMの総再生時間ms - 1000 ) )
						if ( ( nCurrentTime - this.nBGM再生開始時刻 ) >= ( this.nBGMの総再生時間ms ) )	// #27787 2012.3.10 yyagi 1000ms == フェードイン分の時間
						{
							if ( !CDTXMania.DTXVmode.Enabled )
							{
							}
							base.eフェーズID = CStage.Eフェーズ.共通_フェードアウト;
						}
						return (int) E曲読込画面の戻り値.継続;
					}

				case CStage.Eフェーズ.共通_フェードアウト:
					if ( this.ct待機.b終了値に達してない )		// DTXVモード時は、フェードアウト省略
						return (int)E曲読込画面の戻り値.継続;

					if ( txFilename != null )
					{
						txFilename.Dispose();
					}
					if ( this.sd読み込み音 != null )
					{
						this.sd読み込み音.t解放する();
					}
					return (int) E曲読込画面の戻り値.読込完了;
			}
			return (int) E曲読込画面の戻り値.継続;
		}

		/// <summary>
		/// ESC押下時、trueを返す
		/// </summary>
		/// <returns></returns>
		protected bool tキー入力()
		{
			IInputDevice keyboard = CDTXMania.Input管理.Keyboard;
			if 	( keyboard.bキーが押された( (int) SlimDX.DirectInput.Key.Escape ) )		// escape (exit)
			{
				return true;
			}
			return false;
		}


		private void ShowProgressByFilename(string strファイル名 )
		{
			if ( graphicsFilename != null && ftFilename != null )
			{
				graphicsFilename.Clear( Color.Transparent );
				graphicsFilename.DrawString( strファイル名, ftFilename, Brushes.White, new RectangleF( 0, 0, 640, 24 ) );
				if ( txFilename != null )
				{
					txFilename.Dispose();
				}
				txFilename = new CTexture( CDTXMania.app.Device, bitmapFilename, CDTXMania.TextureFormat );
				txFilename.vc拡大縮小倍率 = new Vector3( 0.5f, 0.5f, 1f );
				txFilename.t2D描画( CDTXMania.app.Device, 0, 720 - 16 );
			}
		}

		// その他

		#region [ private ]
		//-----------------
		//private CActFIFOBlack actFI;
		//private CActFIFOBlack actFO;
		private long nBGMの総再生時間ms;
		private long nBGM再生開始時刻;
		private CSound sd読み込み音;
		private string strSTAGEFILE;
		private string str曲タイトル;
        private string strサブタイトル;
		private CTexture txタイトル;
        private CTexture txサブタイトル;
		private CTexture tx背景;
        //private CTexture txSongnamePlate;
		private DateTime timeBeginLoad;
		private DateTime timeBeginLoadWAV;
		private int nWAVcount;
		private CTexture txFilename;
		private Bitmap bitmapFilename;
		private Graphics graphicsFilename;
		private Font ftFilename;
        private CCounter ct待機;
        private CCounter ct曲名表示;

        private CPrivateFastFont pfTITLE;
        private CPrivateFastFont pfSUBTITLE;
		//-----------------
		#endregion
	}
}
