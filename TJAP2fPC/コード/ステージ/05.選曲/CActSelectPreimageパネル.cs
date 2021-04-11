using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using SlimDX;
using SlimDX.Direct3D9;
using FDK;

namespace DTXMania
{
	internal class CActSelectPreimageパネル : CActivity
	{
		// メソッド

		public CActSelectPreimageパネル()
		{
			base.b活性化してない = true;
		}
		public void t選択曲が変更された()
		{
			this.ct遅延表示 = new CCounter( -CDTXMania.ConfigIni.n曲が選択されてからプレビュー画像が表示開始されるまでのウェイトms, 100, 1, CDTXMania.Timer );
			this.b新しいプレビューファイルを読み込んだ = false;
		}

		public bool bIsPlayingPremovie		// #27060
		{
			get
			{
				return false;
			}
		}

		// CActivity 実装

		public override void On活性化()
		{
			this.n本体X = 8;
			this.n本体Y = 0x39;
			this.str現在のファイル名 = "";
			this.b新しいプレビューファイルを読み込んだ = false;
			base.On活性化();
		}
		public override void On非活性化()
		{
			this.ct登場アニメ用 = null;
			this.ct遅延表示 = null;
			base.On非活性化();
		}
		public override void OnManagedリソースの作成()
		{
			if( !base.b活性化してない )
			{
				this.nAVI再生開始時刻 = -1;
				this.n前回描画したフレーム番号 = -1;
				this.b動画フレームを作成した = false;
				this.pAVIBmp = IntPtr.Zero;
				this.tプレビュー画像_動画の変更();
				base.OnManagedリソースの作成();
			}
		}
		public override void OnManagedリソースの解放()
		{
			if( !base.b活性化してない )
			{
				base.OnManagedリソースの解放();
			}
		}
		public override int On進行描画()
		{
			if( !base.b活性化してない )
			{
				if( base.b初めての進行描画 )
				{
					this.ct登場アニメ用 = new CCounter( 0, 100, 5, CDTXMania.Timer );
					this.ctセンサ光 = new CCounter( 0, 100, 30, CDTXMania.Timer );
					this.ctセンサ光.n現在の値 = 70;
					base.b初めての進行描画 = false;
				}
				this.ct登場アニメ用.t進行();

				this.t描画処理_パネル本体();
				//this.t描画処理_ジャンル文字列();
				this.t描画処理_プレビュー画像();
				//this.t描画処理_センサ光();
				//this.t描画処理_センサ本体();
			}
			return 0;
		}


		// その他

		#region [ private ]
		//-----------------
		private bool b動画フレームを作成した;
		private CCounter ctセンサ光;
		private CCounter ct遅延表示;
		private CCounter ct登場アニメ用;
		private long nAVI再生開始時刻;
		private int n前回描画したフレーム番号;
		private int n本体X;
		private int n本体Y;
		private IntPtr pAVIBmp;
		private readonly Rectangle rcセンサ光 = new Rectangle( 0, 0xc0, 0x40, 0x40 );
		private readonly Rectangle rcセンサ本体下半分 = new Rectangle( 0x40, 0, 0x40, 0x80 );
		private readonly Rectangle rcセンサ本体上半分 = new Rectangle( 0, 0, 0x40, 0x80 );
		private string str現在のファイル名;
		private bool b新しいプレビューファイルを読み込んだ;
		private bool b新しいプレビューファイルをまだ読み込んでいない
		{
			get
			{
				return !this.b新しいプレビューファイルを読み込んだ;
			}
			set
			{
				this.b新しいプレビューファイルを読み込んだ = !value;
			}
		}

		private unsafe void tサーフェイスをクリアする( Surface sf )
		{
			DataRectangle rectangle = sf.LockRectangle( LockFlags.None );
			DataStream data = rectangle.Data;
			switch( ( rectangle.Pitch / sf.Description.Width ) )
			{
				case 4:
					{
						uint* numPtr = (uint*) data.DataPointer.ToPointer();
						for( int i = 0; i < sf.Description.Height; i++ )
						{
							for( int j = 0; j < sf.Description.Width; j++ )
							{
								( numPtr + ( i * sf.Description.Width ) )[ j ] = 0;
							}
						}
						break;
					}
				case 2:
					{
						ushort* numPtr2 = (ushort*) data.DataPointer.ToPointer();
						for( int k = 0; k < sf.Description.Height; k++ )
						{
							for( int m = 0; m < sf.Description.Width; m++ )
							{
								( numPtr2 + ( k * sf.Description.Width ) )[ m ] = 0;
							}
						}
						break;
					}
			}
			sf.UnlockRectangle();
		}
		private void tプレビュー画像_動画の変更()
		{
			this.pAVIBmp = IntPtr.Zero;
			this.nAVI再生開始時刻 = -1;
			if( !CDTXMania.ConfigIni.bストイックモード )
			{
				if( this.tプレビュー動画の指定があれば構築する() )
				{
					return;
				}
				if( this.tプレビュー画像の指定があれば構築する() )
				{
					return;
				}
				if( this.t背景画像があればその一部からプレビュー画像を構築する() )
				{
					return;
				}
			}
			this.str現在のファイル名 = "";
		}
		private bool tプレビュー画像の指定があれば構築する()
		{

			return true;
		}
		private bool tプレビュー動画の指定があれば構築する()
		{
			return false;
		}
		private bool t背景画像があればその一部からプレビュー画像を構築する()
		{
			return true;
		}
        /// <summary>
        /// 一時的に使用禁止。
        /// </summary>
		private void t描画処理_ジャンル文字列()
		{
			C曲リストノード c曲リストノード = CDTXMania.stage選曲.r現在選択中の曲;
			Cスコア cスコア = CDTXMania.stage選曲.r現在選択中のスコア;
			if( ( c曲リストノード != null ) && ( cスコア != null ) )
			{
				string str = "";
				switch( c曲リストノード.eノード種別 )
				{
					case C曲リストノード.Eノード種別.SCORE:
						if( ( c曲リストノード.strジャンル == null ) || ( c曲リストノード.strジャンル.Length <= 0 ) )
						{
							if( ( cスコア.譜面情報.ジャンル != null ) && ( cスコア.譜面情報.ジャンル.Length > 0 ) )
							{
								str = cスコア.譜面情報.ジャンル;
							}
#if false	// #32644 2013.12.21 yyagi "Unknown"なジャンル表示を削除。DTX/BMSなどの種別表示もしない。
							else
							{
								switch( cスコア.譜面情報.曲種別 )
								{
									case CDTX.E種別.DTX:
										str = "DTX";
										break;

									case CDTX.E種別.GDA:
										str = "GDA";
										break;

									case CDTX.E種別.G2D:
										str = "G2D";
										break;

									case CDTX.E種別.BMS:
										str = "BMS";
										break;

									case CDTX.E種別.BME:
										str = "BME";
										break;
								}
								str = "Unknown";
							}
#endif
							break;
						}
						str = c曲リストノード.strジャンル;
						break;

					case C曲リストノード.Eノード種別.SCORE_MIDI:
						str = "MIDI";
						break;

					case C曲リストノード.Eノード種別.BOX:
						str = "MusicBox";
						break;

					case C曲リストノード.Eノード種別.BACKBOX:
						str = "BackBox";
						break;

					case C曲リストノード.Eノード種別.RANDOM:
						str = "Random";
						break;

					default:
						str = "Unknown";
						break;
				}
				CDTXMania.act文字コンソール.tPrint( this.n本体X + 0x12, this.n本体Y - 1, C文字コンソール.Eフォント種別.赤細, str );
			}
		}
		private void t描画処理_センサ光()
		{

		}
		private void t描画処理_センサ本体()
		{

		}
		private void t描画処理_パネル本体()
		{

		}
		private unsafe void t描画処理_プレビュー画像()
		{
            
		}
		//-----------------
		#endregion
	}
}
