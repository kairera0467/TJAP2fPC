using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Reflection;
using System.Globalization;
using System.Threading;
using System.Text.RegularExpressions;
//using System.Windows.Forms;
using FDK;

namespace DTXMania
{
	internal class CDTX : CActivity
	{
		// 定数

		public enum E種別 { DTX, GDA, G2D, BMS, BME, SMF }

		// クラス

		public class CAVI : IDisposable
		{
			public CAvi avi;
			private bool bDispose済み;
			public int n番号;
			public string strコメント文 = "";
			public string strファイル名 = "";

			public void OnDeviceCreated()
			{
				#region [ strAVIファイル名の作成。]
				//-----------------
				string strAVIファイル名;
				if( !string.IsNullOrEmpty( CDTXMania.DTX.PATH_WAV ) )
					strAVIファイル名 = CDTXMania.DTX.PATH_WAV + this.strファイル名;
				else
					strAVIファイル名 = CDTXMania.DTX.strフォルダ名 + this.strファイル名;
				//-----------------
				#endregion

				if( !File.Exists( strAVIファイル名 ) )
				{
					Trace.TraceWarning( "ファイルが存在しません。({0})({1})", this.strコメント文, strAVIファイル名 );
					this.avi = null;
					return;
				}

				// AVI の生成。

				try
				{
					this.avi = new CAvi( strAVIファイル名 );
					Trace.TraceInformation( "動画を生成しました。({0})({1})({2}frames)", this.strコメント文, strAVIファイル名, this.avi.GetMaxFrameCount() );
				}
				catch( Exception e )
				{
					Trace.TraceError( e.Message );
					Trace.TraceError( "動画の生成に失敗しました。({0})({1})", this.strコメント文, strAVIファイル名 );
					this.avi = null;
				}
			}
			public override string ToString()
			{
				return string.Format( "CAVI{0}: File:{1}, Comment:{2}", CDTX.tZZ( this.n番号 ), this.strファイル名, this.strコメント文 );
			}

			#region [ IDisposable 実装 ]
			//-----------------
			public void Dispose()
			{
				if( this.bDispose済み )
					return;

				if( this.avi != null )
				{
					#region [ strAVIファイル名 の作成。 ]
					//-----------------
					string strAVIファイル名;
					if( !string.IsNullOrEmpty( CDTXMania.DTX.PATH_WAV ) )
						strAVIファイル名 = CDTXMania.DTX.PATH_WAV + this.strファイル名;
					else
						strAVIファイル名 = CDTXMania.DTX.strフォルダ名 + this.strファイル名;
					//-----------------
					#endregion

					this.avi.Dispose();
					this.avi = null;
					
					Trace.TraceInformation( "動画を解放しました。({0})({1})", this.strコメント文, strAVIファイル名 );
				}

				this.bDispose済み = true;
			}
			//-----------------
			#endregion
		}
        public class CAVIPAN
		{
			public int nAVI番号;
			public int n移動時間ct;
			public int n番号;
			public Point pt動画側開始位置 = new Point( 0, 0 );
			public Point pt動画側終了位置 = new Point( 0, 0 );
			public Point pt表示側開始位置 = new Point( 0, 0 );
			public Point pt表示側終了位置 = new Point( 0, 0 );
			public Size sz開始サイズ = new Size( 0, 0 );
			public Size sz終了サイズ = new Size( 0, 0 );

			public override string ToString()
			{
				return string.Format( "CAVIPAN{0}: AVI:{14}, 開始サイズ:{1}x{2}, 終了サイズ:{3}x{4}, 動画側開始位置:{5}x{6}, 動画側終了位置:{7}x{8}, 表示側開始位置:{9}x{10}, 表示側終了位置:{11}x{12}, 移動時間:{13}ct",
					CDTX.tZZ( this.n番号 ),
					this.sz開始サイズ.Width, this.sz開始サイズ.Height,
					this.sz終了サイズ.Width, this.sz終了サイズ.Height,
					this.pt動画側開始位置.X, this.pt動画側開始位置.Y,
					this.pt動画側終了位置.X, this.pt動画側終了位置.Y,
					this.pt表示側開始位置.X, this.pt表示側開始位置.Y,
					this.pt表示側終了位置.X, this.pt表示側終了位置.Y,
					this.n移動時間ct,
					CDTX.tZZ( this.nAVI番号 ) );
			}
		}
        public class CDirectShow : IDisposable
		{
			public FDK.CDirectShow dshow;
			private bool bDispose済み;
			public int n番号;
			public string strコメント文 = "";
			public string strファイル名 = "";

			public void OnDeviceCreated()
			{
				#region [ str動画ファイル名の作成。]
				//-----------------
				string str動画ファイル名;
				if( !string.IsNullOrEmpty( CDTXMania.DTX.PATH_WAV ) )
					str動画ファイル名 = CDTXMania.DTX.PATH_WAV + this.strファイル名;
				else
					str動画ファイル名 = CDTXMania.DTX.strフォルダ名 + this.strファイル名;
				//-----------------
				#endregion

				if( !File.Exists( str動画ファイル名 ) )
				{
					Trace.TraceWarning( "ファイルが存在しません。({0})({1})", this.strコメント文, str動画ファイル名 );
					this.dshow = null;
				}

				// AVI の生成。

				try
				{
                    this.dshow = new FDK.CDirectShow( CDTXMania.stage選曲.r確定されたスコア.ファイル情報.フォルダの絶対パス + this.strファイル名, CDTXMania.app.WindowHandle, true);
					Trace.TraceInformation( "DirectShowを生成しました。({0})({1})({2}byte)", this.strコメント文, str動画ファイル名, this.dshow.nデータサイズbyte );
				}
				catch( Exception e )
				{
					Trace.TraceError( e.Message );
					Trace.TraceError( "DirectShowの生成に失敗しました。({0})({1})", this.strコメント文, str動画ファイル名 );
					this.dshow= null;
				}
			}
			public override string ToString()
			{
				return string.Format( "CAVI{0}: File:{1}, Comment:{2}", CDTX.tZZ( this.n番号 ), this.strファイル名, this.strコメント文 );
			}

			#region [ IDisposable 実装 ]
			//-----------------
			public void Dispose()
			{
				if( this.bDispose済み )
					return;

				if( this.dshow != null )
				{
					#region [ strAVIファイル名 の作成。 ]
					//-----------------
					string str動画ファイル名;
					if( !string.IsNullOrEmpty( CDTXMania.DTX.PATH_WAV ) )
						str動画ファイル名 = CDTXMania.DTX.PATH_WAV + this.strファイル名;
					else
						str動画ファイル名 = CDTXMania.DTX.strフォルダ名 + this.strファイル名;
					//-----------------
					#endregion

					this.dshow.Dispose();
					this.dshow = null;
					
					Trace.TraceInformation( "動画を解放しました。({0})({1})", this.strコメント文, str動画ファイル名 );
				}

				this.bDispose済み = true;
			}
			//-----------------
			#endregion
		}

		public class CBPM
		{
			public double dbBPM値;
            public double bpm_change_time;
            public double bpm_change_bmscroll_time;
            public int bpm_change_course;
			public int n内部番号;
			public int n表記上の番号;

			public override string ToString()
			{
				StringBuilder builder = new StringBuilder( 0x80 );
				if( this.n内部番号 != this.n表記上の番号 )
				{
					builder.Append( string.Format( "CBPM{0}(内部{1})", CDTX.tZZ( this.n表記上の番号 ), this.n内部番号 ) );
				}
				else
				{
					builder.Append( string.Format( "CBPM{0}", CDTX.tZZ( this.n表記上の番号 ) ) );
				}
				builder.Append( string.Format( ", BPM:{0}", this.dbBPM値 ) );
				return builder.ToString();
			}
		}
		public class CSCROLL
		{
			public double dbSCROLL値;
            public double dbSCROLL値Y;
			public int n内部番号;
			public int n表記上の番号;

			public override string ToString()
			{
				StringBuilder builder = new StringBuilder( 0x80 );
				if( this.n内部番号 != this.n表記上の番号 )
				{
					builder.Append( string.Format( "CSCROLL{0}(内部{1})", CDTX.tZZ( this.n表記上の番号 ), this.n内部番号 ) );
				}
				else
				{
					builder.Append( string.Format( "CSCROLL{0}", CDTX.tZZ( this.n表記上の番号 ) ) );
				}
				builder.Append( string.Format( ", SCROLL:{0}", this.dbSCROLL値 ) );
				return builder.ToString();
			}
		}
        /// <summary>
        /// 判定ライン移動命令
        /// </summary>
		public class CJPOSSCROLL
		{
            public double db移動時間;
            public int n移動距離px;
            public int n移動方向; //移動方向は0(左)、1(右)の2つだけ。
			public int n内部番号;
			public int n表記上の番号;

			public override string ToString()
			{
				StringBuilder builder = new StringBuilder( 0x80 );
				if( this.n内部番号 != this.n表記上の番号 )
				{
					builder.Append( string.Format( "CJPOSSCROLL{0}(内部{1})", CDTX.tZZ( this.n表記上の番号 ), this.n内部番号 ) );
				}
				else
				{
					builder.Append( string.Format( "CJPOSSCROLL{0}", CDTX.tZZ( this.n表記上の番号 ) ) );
				}
				builder.Append( string.Format( ", JPOSSCROLL:{0}", this.db移動時間 ) );
				return builder.ToString();
			}
		}

        public class CDELAY
        {
			public int nDELAY値; //格納時にはmsになっているため、doubleにはしない。
			public int n内部番号;
			public int n表記上の番号;
            public double delay_time;
            public double delay_bmscroll_time;
            public double delay_delay;
            public double delay_bpm;
            public int delay_course;

			public override string ToString()
			{
				StringBuilder builder = new StringBuilder( 0x80 );
				if( this.n内部番号 != this.n表記上の番号 )
				{
					builder.Append( string.Format( "CDELAY{0}(内部{1})", CDTX.tZZ( this.n表記上の番号 ), this.n内部番号 ) );
				}
				else
				{
					builder.Append( string.Format( "CDELAY{0}", CDTX.tZZ( this.n表記上の番号 ) ) );
				}
				builder.Append( string.Format( ", DELAY:{0}", this.nDELAY値 ) );
				return builder.ToString();
			}
        }

        public class CBRANCH
        {
            public bool bGOGOTIME;
            public int n分岐の種類; //0:精度分岐 1:連打分岐 2:スコア分岐 3:大音符のみの精度分岐
            public double n条件数値A;
            public double n条件数値B;
            public double db分岐時間;
            public double db分岐時間ms;
            public double db判定時間;
            public double dbBMScrollTime;
            public double dbBPM;
            public double dbSCROLL;
            public int n現在の小節;
            public int n命令時のChipList番号;
            public float fMeasure_s = 4.0f;
            public float fMeasure_m = 4.0f;
            public int n表記上の番号;
            public int n内部番号;

			public override string ToString()
			{
				StringBuilder builder = new StringBuilder( 0x80 );
				if( this.n内部番号 != this.n表記上の番号 )
				{
					builder.Append( string.Format( "CBRANCH{0}(内部{1})", CDTX.tZZ( this.n表記上の番号 ), this.n内部番号 ) );
				}
				else
				{
					builder.Append( string.Format( "CBRANCH{0}", CDTX.tZZ( this.n表記上の番号 ) ) );
				}
				builder.Append( string.Format( ", BRANCH:{0}", this.n分岐の種類 ) );
				return builder.ToString();
			}
        }


		public class CChip : IComparable<CDTX.CChip>, ICloneable
		{
			public bool bHit; //プレイヤーの手によって叩かれた(判定イベントが発生した)か
			public bool b可視 = true; //falseにすると処理しない音符として扱う
            public bool bShow; //(特殊フラグ)SUDDEN命令を使用した時用
            public bool bBranchLine = false; //(特殊フラグ)分岐小節線であるか
            public bool bBranch = false; //譜面分岐対象のチップであるか
			public double dbチップサイズ倍率 = 1.0;
			public double db実数値;
            public double dbBPM;
            public double dbSCROLL;
            public double dbSCROLL_Y;
            public int nコース;
            public int nSenote;
            public int nState;
            public int nRollCount;
            public int nBalloon;
            public int nProcessTime;
            public int nスクロール方向;
            public int n描画優先度; //(特殊)現状連打との判断目的で使用
            public ENoteState eNoteState;
            public EAVI種別 eAVI種別;
			public E楽器パート e楽器パート = E楽器パート.UNKNOWN;
			public int nチャンネル番号;
			public STDGBVALUE<float> nバーからの距離dot;
			public STDGBVALUE<float> nバーからのノーツ末端距離dot;
			public int n整数値;
			public int n整数値_内部番号;
			public int n総移動時間;
			public int n透明度 = 0xff;
			public int n発声位置;
			public double db発声位置;
            public double fBMSCROLLTime;
            public double fBMSCROLLTime_end;
			public int n発声時刻ms;
            public double db発声時刻ms;
            public int nノーツ終了位置;
			public int nノーツ終了時刻ms;
            public int nノーツ出現時刻ms;
            public int nノーツ移動開始時刻ms;
            public int n分岐回数;
            public int n連打音符State;
			public int nLag;				// 2011.2.1 yyagi
			public CDTX.CAVI rAVI;
            public CDTX.CAVIPAN rAVIPan;
            public CDTX.CDirectShow rDShow;
            public double db判定終了時刻;//連打系音符で使用
            public double dbProcess_Time;
            public int nPlayerSide;
            public bool bGOGOTIME = false; //2018.03.11 kairera0467 ゴーゴータイム内のチップであるか
            public int nList上の位置; //2018.04.01 List上での位置
			public bool bBPMチップである
			{
				get
				{
					if (this.nチャンネル番号 == 3 || this.nチャンネル番号 == 8) {
						return true;
					} else {
						return false;
					}
				}
			}
			public bool bWAVを使うチャンネルである
			{
				get
				{
					switch( this.nチャンネル番号 )
					{
						case 0x01:
							return true;
					}
					return false;
				}
			}
			public bool b自動再生音チャンネルである
			{
				get
				{
					int num = this.nチャンネル番号;
					if( ( ( ( num != 1 ) && ( ( 0x61 > num ) || ( num > 0x69 ) ) ) && ( ( 0x70 > num ) || ( num > 0x79 ) ) ) && ( ( 0x80 > num ) || ( num > 0x89 ) ) )
					{
						return ( ( 0x90 <= num ) && ( num <= 0x92 ) );
					}
					return true;
				}
			}
			public bool bIsAutoPlayed;							// 2011.6.10 yyagi
			public bool b演奏終了後も再生が続くチップである;	// #32248 2013.10.14 yyagi

			public CChip()
			{
				this.nバーからの距離dot = new STDGBVALUE<float>() {
					Drums = 0,
					Guitar = 0,
					Bass = 0,
				};
			}
			public void t初期化()
			{
                this.bBranch = false;
                this.bBranchLine = false;
				this.nチャンネル番号 = 0;
				this.n整数値 = 0;
				this.n整数値_内部番号 = 0;
                this.nList上の位置 = 0;
				this.db実数値 = 0.0;
				this.n発声位置 = 0;
                this.db発声位置 = 0.0f;
				this.n発声時刻ms = 0;
                this.db発声時刻ms = 0.0f;
                this.fBMSCROLLTime = 0;
                this.nノーツ終了位置 = 0;
                this.nノーツ終了時刻ms = 0;
                this.n描画優先度 = 0;
				this.nLag = -999;
				this.bIsAutoPlayed = false;
				this.b演奏終了後も再生が続くチップである = false;
				this.dbチップサイズ倍率 = 1.0;
				this.bHit = false;
				this.b可視 = true;
				this.e楽器パート = E楽器パート.UNKNOWN;
				this.n透明度 = 0xff;
				this.nバーからの距離dot.Drums = 0;
				this.nバーからの距離dot.Guitar = 0;
				this.nバーからの距離dot.Bass = 0;
                this.nバーからの距離dot.Taiko = 0;
                this.nバーからのノーツ末端距離dot.Drums = 0;
                this.nバーからのノーツ末端距離dot.Guitar = 0;
                this.nバーからのノーツ末端距離dot.Bass = 0;
                this.nバーからのノーツ末端距離dot.Taiko = 0;
				this.n総移動時間 = 0;
                this.dbBPM = 120.0;
                this.nスクロール方向 = 0;
                this.dbSCROLL = 1.0;
                this.dbSCROLL_Y = 0.0f;
			}
			public override string ToString()
			{

                //2016.10.07 kairera0467 近日中に再編成予定
				string[] chToStr = 
				{
                    //システム
					"??", "バックコーラス", "小節長変更", "BPM変更", "??", "??", "??", "??",
					"BPM変更(拡張)", "??", "??", "??", "??", "??", "??", "??",

                    //太鼓1P(移動予定)
					"??", "ドン", "カツ", "ドン(大)", "カツ(大)", "連打", "連打(大)", "ふうせん連打",
					"連打終点", "芋", "ドン(手)", "カッ(手)", "??", "??", "??", "AD-LIB",

                    //太鼓予備
					"??", "??", "??", "??", "??", "??", "??", "??",
					"??", "??", "??", "??", "??", "??", "??", "??",

                    //太鼓予備
					"??", "??", "??", "??", "??", "??", "??", "??",
					"??", "??", "??", "??", "??", "??", "??", "??",

                    //太鼓予備
					"??", "??", "??", "??", "??", "??", "??", "??", 
					"??", "??", "??", "??", "??", "??", "??", "??", 

                    //システム
					"小節線", "拍線", "??", "??", "AVI", "??", "??", "??",
					"??", "??", "??", "??", "??", "??", "??", "??", 

                    //システム(移動予定)
					"SCROLL", "DELAY", "ゴーゴータイム開始", "ゴーゴータイム終了", "??", "??", "??", "??",
					"??", "??", "??", "??", "??", "??", "??", "??", 

					"??", "??", "??", "??", "??", "??", "??", "??",
					"??", "??", "??", "??", "??", "??", "??", "??", 

					"??", "??", "??", "??", "??", "??", "??", "??",
					"??", "??", "??", "??", "??", "??", "??", "??", 

                    //太鼓1P、システム(現行)
					"??", "??", "??", "太鼓_赤", "太鼓_青", "太鼓_赤(大)", "太鼓_青(大)", "太鼓_黄", 
					"太鼓_黄(大)", "太鼓_風船", "太鼓_連打末端", "太鼓_芋", "??", "SCROLL", "ゴーゴータイム開始", "ゴーゴータイム終了", 

					"??", "??", "??", "??", "??", "??", "??", "??",
					"??", "??", "??", "??", "??", "??", "??", "太鼓 AD-LIB",

					"??", "??", "??", "??", "??", "??", "??", "??",
					"??", "??", "??", "??", "??", "??", "??", "??", 

					"??", "??", "??", "??", "0xC4", "0xC5", "0xC6", "??",
					"??", "??", "0xCA", "??", "??", "??", "??", "0xCF", 

                    //システム(現行)
					"0xD0", "??", "??", "??", "??", "??", "??", "??",
					"??", "??", "ミキサー追加", "ミキサー削除", "DELAY", "譜面分岐リセット", "譜面分岐アニメ", "譜面分岐内部処理", 

                    //システム(現行)
					"小節線ON/OFF", "分岐固定", "判定枠移動", "", "", "", "", "",
                    "", "", "", "", "", "", "", "",

                    "0xF0", "歌詞", "??", "SUDDEN", "??", "??", "??", "??",
                    "??", "??", "??", "??", "??", "??", "??", "??", "譜面終了"
				};
				return string.Format( "CChip: 位置:{0:D4}.{1:D3}, 時刻{2:D6}, Ch:{3:X2}({4}), Pn:{5}({11})(内部{6}), Pd:{7}, Sz:{8}, BMScroll:{9}, Auto:{10}, コース:{11}",
					this.n発声位置 / 384, this.n発声位置 % 384,
					this.n発声時刻ms,
					this.nチャンネル番号, chToStr[ this.nチャンネル番号 ],
					this.n整数値, this.n整数値_内部番号,
					this.db実数値,
					this.dbチップサイズ倍率,
					this.fBMSCROLLTime,
					this.b自動再生音チャンネルである,
                    this.nコース,
					CDTX.tZZ( this.n整数値 ) );
			}
			/// <summary>
			/// チップの再生長を取得する。現状、WAVチップとBGAチップでのみ使用可能。
			/// </summary>
			/// <returns>再生長(ms)</returns>
			public int GetDuration()
			{
				int nDuration = 0;

				if ( this.bWAVを使うチャンネルである )		// WAV
				{
					CDTX.CWAV wc;
					CDTXMania.DTX.listWAV.TryGetValue( this.n整数値_内部番号, out wc );
					if ( wc == null )
					{
						nDuration = 0;
					}
					else
					{
						nDuration = ( wc.rSound[ 0 ] == null ) ? 0 : wc.rSound[ 0 ].n総演奏時間ms;
					}
				}
				else if ( this.nチャンネル番号 == 0x54 )	// AVI
				{
					if ( this.rAVI != null && this.rAVI.avi != null )
					{
						int dwRate = (int) this.rAVI.avi.dwレート;
						int dwScale = (int) this.rAVI.avi.dwスケール;
						nDuration = (int) ( 1000.0f * dwScale / dwRate * this.rAVI.avi.GetMaxFrameCount() );
					}
				}

				double _db再生速度 = ( CDTXMania.DTXVmode.Enabled ) ? CDTXMania.DTX.dbDTXVPlaySpeed : CDTXMania.DTX.db再生速度;
				return (int) ( nDuration / _db再生速度 );
			}

			#region [ IComparable 実装 ]
			//-----------------
			public int CompareTo( CDTX.CChip other )
			{
				byte[] n優先度 = new byte[] {
					5, 5, 3, 7, 5, 5, 5, 5, 3, 5, 5, 5, 5, 5, 5, 5, //0x00
					5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, //0x10
					7, 7, 7, 7, 7, 7, 7, 7, 5, 5, 5, 5, 5, 5, 5, 5, //0x20
					5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, //0x30
					5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, //0x40
					9, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, //0x50
					5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, //0x60
					5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, //0x70
					5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, //0x80
					5, 5, 5, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 9, 9, 9, //0x90
					5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, //0xA0
					5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, //0xB0
					5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, //0xC0
					5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 3, 4, 4, //0xD0
					5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, //0xE0
					5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, //0xF0
				};


				// まずは位置で比較。

                //BGMチップだけ発声位置
                //if( this.nチャンネル番号 == 0x01 || this.nチャンネル番号 == 0x02 )
                //{
                //    if( this.n発声位置 < other.n発声位置 )
                //        return -1;

                //    if( this.n発声位置 > other.n発声位置 )
                //        return 1;
                //}

                //if( this.n発声位置 < other.n発声位置 )
                //    return -1;

                //if( this.n発声位置 > other.n発声位置 )
                //    return 1;

                //譜面解析メソッドV4では発声時刻msで比較する。
                if( this.n発声時刻ms < other.n発声時刻ms )
                    return -1;

                if( this.n発声時刻ms > other.n発声時刻ms )
                    return 1;


				// 位置が同じなら優先度で比較。

				if( n優先度[ this.nチャンネル番号 ] < n優先度[ other.nチャンネル番号 ] )
					return -1;

				if( n優先度[ this.nチャンネル番号 ] > n優先度[ other.nチャンネル番号 ] )
					return 1;


				// 位置も優先度も同じなら同じと返す。

				return 0;
			}
			//-----------------
			#endregion
			/// <summary>
			/// shallow copyです。
			/// </summary>
			/// <returns></returns>
			public object Clone()
			{
				return MemberwiseClone();
			}
		}
		public class CWAV : IDisposable
		{
			public bool bBGMとして使う;
			public List<int> listこのWAVを使用するチャンネル番号の集合 = new List<int>( 16 );
			public int nチップサイズ = 100;
			public int n位置;
			public long[] n一時停止時刻 = new long[ CDTXMania.ConfigIni.nPoliphonicSounds ];	// 4
			public int n音量 = 100;
			public int n現在再生中のサウンド番号;
			public long[] n再生開始時刻 = new long[ CDTXMania.ConfigIni.nPoliphonicSounds ];	// 4
			public int n内部番号;
			public int n表記上の番号;
			public CSound[] rSound = new CSound[ CDTXMania.ConfigIni.nPoliphonicSounds ];		// 4
			public string strコメント文 = "";
			public string strファイル名 = "";
			public bool bBGMとして使わない
			{
				get
				{
					return !this.bBGMとして使う;
				}
				set
				{
					this.bBGMとして使う = !value;
				}
			}
			public bool bIsBassSound = false;
			public bool bIsGuitarSound = false;
			public bool bIsDrumsSound = false;
			public bool bIsSESound = false;
			public bool bIsBGMSound = false;

			public override string ToString()
			{
				var sb = new StringBuilder( 128 );
				
				if( this.n表記上の番号 == this.n内部番号 )
				{
					sb.Append( string.Format( "CWAV{0}: ", CDTX.tZZ( this.n表記上の番号 ) ) );
				}
				else
				{
					sb.Append( string.Format( "CWAV{0}(内部{1}): ", CDTX.tZZ( this.n表記上の番号 ), this.n内部番号 ) );
				}
				sb.Append( string.Format( "音量:{0}, 位置:{1}, サイズ:{2}, BGM:{3}, File:{4}, Comment:{5}", this.n音量, this.n位置, this.nチップサイズ, this.bBGMとして使う ? 'Y' : 'N', this.strファイル名, this.strコメント文 ) );
				
				return sb.ToString();
			}

			#region [ Dispose-Finalize パターン実装 ]
			//-----------------
			public void Dispose()
			{
				this.Dispose( true );
				GC.SuppressFinalize( this );
			}
			public void Dispose( bool bManagedリソースの解放も行う )
			{
				if( this.bDisposed済み )
					return;

				if( bManagedリソースの解放も行う )
				{
					for ( int i = 0; i < CDTXMania.ConfigIni.nPoliphonicSounds; i++ )	// 4
					{
						if( this.rSound[ i ] != null )
							CDTXMania.Sound管理.tサウンドを破棄する( this.rSound[ i ] );
						this.rSound[ i ] = null;

						if( ( i == 0 ) && CDTXMania.ConfigIni.bLog作成解放ログ出力 )
							Trace.TraceInformation( "サウンドを解放しました。({0})({1})", this.strコメント文, this.strファイル名 );
					}
				}

				this.bDisposed済み = true;
			}
			~CWAV()
			{
				this.Dispose( false );
			}
			//-----------------
			#endregion

			#region [ private ]
			//-----------------
			private bool bDisposed済み;
			//-----------------
			#endregion
		}
		

		// 構造体

		public struct STLANEINT
		{
			public int HH;
			public int SD;
			public int BD;
			public int HT;
			public int LT;
			public int CY;
			public int FT;
			public int HHO;
			public int RD;
			public int LC;
            public int LP;
            public int LBD;

			public int Drums
			{
				get
				{
					return this.HH + this.SD + this.BD + this.HT + this.LT + this.CY + this.FT + this.HHO + this.RD + this.LC + this.LP + this.LBD;
				}
			}
			public int Guitar;
			public int Bass;
            public int Taiko_Red;
            public int Taiko_Blue;

			public int this[ int index ]
			{
				get
				{
					switch( index )
					{
						case 0:
							return this.HH;

						case 1:
							return this.SD;

						case 2:
							return this.BD;

						case 3:
							return this.HT;

						case 4:
							return this.LT;

						case 5:
							return this.CY;

						case 6:
							return this.FT;

						case 7:
							return this.HHO;

						case 8:
							return this.RD;

						case 9:
							return this.LC;

						case 10:
                            return this.LP;

						case 11:
							return this.LBD;

						case 12:
							return this.Guitar;

						case 13:
							return this.Bass;

                        case 14:
                            return this.Taiko_Red;

                        case 15:
                            return this.Taiko_Blue;
					}
					throw new IndexOutOfRangeException();
				}
				set
				{
					if( value < 0 )
					{
						throw new ArgumentOutOfRangeException();
					}
					switch( index )
					{
						case 0:
							this.HH = value;
							return;

						case 1:
							this.SD = value;
							return;

						case 2:
							this.BD = value;
							return;

						case 3:
							this.HT = value;
							return;

						case 4:
							this.LT = value;
							return;

						case 5:
							this.CY = value;
							return;

						case 6:
							this.FT = value;
							return;

						case 7:
							this.HHO = value;
							return;

						case 8:
							this.RD = value;
							return;

						case 9:
							this.LC = value;
							return;

						case 10:
							this.LP = value;
							return;

						case 11:
							this.LBD = value;
							return;

						case 12:
							this.Guitar = value;
							return;

						case 13:
							this.Bass = value;
							return;

                        case 14:
                            this.Taiko_Red = value;
                            return;

                        case 15:
                            this.Taiko_Blue = value;
                            return;
					}
					throw new IndexOutOfRangeException();
				}
			}
		}
		public struct STRESULT
		{
			public string SS;
			public string S;
			public string A;
			public string B;
			public string C;
			public string D;
			public string E;

			public string this[ int index ]
			{
				get
				{
					switch( index )
					{
						case 0:
							return this.SS;

						case 1:
							return this.S;

						case 2:
							return this.A;

						case 3:
							return this.B;

						case 4:
							return this.C;

						case 5:
							return this.D;

						case 6:
							return this.E;
					}
					throw new IndexOutOfRangeException();
				}
				set
				{
					switch( index )
					{
						case 0:
							this.SS = value;
							return;

						case 1:
							this.S = value;
							return;

						case 2:
							this.A = value;
							return;

						case 3:
							this.B = value;
							return;

						case 4:
							this.C = value;
							return;

						case 5:
							this.D = value;
							return;

						case 6:
							this.E = value;
							return;
					}
					throw new IndexOutOfRangeException();
				}
			}
		}
		public struct STチップがある
		{
			public bool Drums;
			public bool Guitar;
			public bool Bass;

			public bool HHOpen;
			public bool Ride;
			public bool LeftCymbal;
			public bool OpenGuitar;
			public bool OpenBass;

            public bool Branch;
			
			public bool this[ int index ]
			{
				get
				{
					switch( index )
					{
						case 0:
							return this.Drums;

						case 1:
							return this.Guitar;

						case 2:
							return this.Bass;

						case 3:
							return this.HHOpen;

						case 4:
							return this.Ride;

						case 5:
							return this.LeftCymbal;

						case 6:
							return this.OpenGuitar;

						case 7:
							return this.OpenBass;

                        case 8:
                            return this.Branch;
					}
					throw new IndexOutOfRangeException();
				}
				set
				{
					switch( index )
					{
						case 0:
							this.Drums = value;
							return;

						case 1:
							this.Guitar = value;
							return;

						case 2:
							this.Bass = value;
							return;

						case 3:
							this.HHOpen = value;
							return;

						case 4:
							this.Ride = value;
							return;

						case 5:
							this.LeftCymbal = value;
							return;

						case 6:
							this.OpenGuitar = value;
							return;

						case 7:
							this.OpenBass = value;
							return;

                        case 8:
                            this.Branch = value;
                            return;
					}
					throw new IndexOutOfRangeException();
				}
			}
		}

        public class CLine
        {
            public int n小節番号;
            public int n文字数;
            public double db発声時刻;
            public double dbBMS時刻;
            public int nコース;
            public int nタイプ;
        }

		// プロパティ

		public int nBGMAdjust
		{
			get; 
			private set;
		}
        public int nPlayerSide; //2017.08.14 kairera0467 引数で指定する
        public bool bDP譜面が存在する;
        public bool bSession譜面を読み込む;

		public string ARTIST;
		public string BACKGROUND;
		public string BACKGROUND_GR;
		public double BASEBPM;
		public double BPM;
		public STチップがある bチップがある;
		public string COMMENT;
		public double db再生速度;
		public E種別 e種別;
		public string GENRE;
        public Eジャンル eジャンル;
		public bool HIDDENLEVEL;
		public STDGBVALUE<int> LEVEL;
        public int[] LEVELtaiko = new int[5];
		public Dictionary<int, CAVI> listAVI;
        public Dictionary<int, CAVIPAN> listAVIPAN;
        public Dictionary<int, CDirectShow> listDS;
		public Dictionary<int, CBPM> listBPM;
		public List<CChip> listChip;
		public Dictionary<int, CWAV> listWAV;
		public Dictionary<int, CSCROLL> listSCROLL;
		public Dictionary<int, CSCROLL> listSCROLL_Normal;
		public Dictionary<int, CSCROLL> listSCROLL_Expert;
		public Dictionary<int, CSCROLL> listSCROLL_Master;
		public Dictionary<int, CJPOSSCROLL> listJPOSSCROLL;

        private int listSCROLL_Normal_数値管理;
        private int listSCROLL_Expert_数値管理;
        private int listSCROLL_Master_数値管理;

        private double[] dbNowSCROLL_Normal;
        private double[] dbNowSCROLL_Expert;
        private double[] dbNowSCROLL_Master;


        public Dictionary<int, CDELAY> listDELAY;
        public Dictionary<int, CBRANCH> listBRANCH;
		public STLANEINT n可視チップ数;
		public const int n最大音数 = 4;
		public const int n小節の解像度 = 384;
		public string PANEL;
		public string PATH_WAV;
		public string PREIMAGE;
		public string PREVIEW;
		public string strハッシュofDTXファイル;
		public string strファイル名;
		public string strファイル名の絶対パス;
		public string strフォルダ名;
        public string SUBTITLE;
		public string TITLE;
		public double dbDTXVPlaySpeed;
        public double dbScrollSpeed;
        public int nデモBGMオフセット;

        private int n現在の小節数 = 1;
        private bool bBarLine = true;
        private int n命令数 = 0;
        private string[] strSplitした後の譜面; //0:ヘッダー情報 1:#START以降 となる。個数の定義は後からされるため、ここでは省略。
        private string[] strSplitしたヘッダ;
        private string[] strSplitした譜面;
        private string[] str命令消去譜面; //命令をすべて消去した譜面

        private int nNowRoll = 0;
        private int nNowRollCount = 0;

        private int[] n連打チップ_temp = new int[3];


        private int nCount = 0;

        public int nOFFSET = 0;
        private bool bOFFSETの値がマイナスである = false;
        private int nMOVIEOFFSET = 0;
        private bool bMOVIEOFFSETの値がマイナスである = false;
        private double dbNowBPM = 120.0;
        private int nDELAY = 0;
        public bool[] bHasBranch = new bool[]{ false, false, false, false, false };

        //分岐関連
        private int n現在の発声時刻;
        private int n現在の発声時刻ms;

        public int[] nノーツ数 = new int[ 4 ]; //0～2:各コース 3:共通
        public int[] n風船数 = new int[ 4 ]; //0～2:各コース 3:共通

        private List<CLine> listLine;
        private int nLineCountTemp; //分岐開始時の小節数を記録。
        private int nLineCountCourseTemp; //現在カウント中のコースを記録。

        public int n参照中の難易度 = 3;
        public int nScoreModeTmp = 99; //2017.01.28 DD
        public int[,] nScoreInit = new int[ 2, 5 ]; //[ x, y ] x=通常or真打 y=コース
        public int[] nScoreDiff = new int[ 5 ]; //[y]
        public bool[,] b配点が指定されている = new bool[ 3, 5 ]; //2017.06.04 kairera0467 [ x, y ] x=通常(Init)or真打orDiff y=コース

        private double dbBarLength;
        public float fNow_Measure_s = 4.0f;
        public float fNow_Measure_m = 4.0f;
        public float fMeasure_BranchStart_s = 4.0f; //2018.05.22 kairera0467 譜面分岐開始時の拍子情報を一時保存する変数
        public float fMeasure_BranchStart_m = 4.0f;
        public double dbNowTime = 0.0;
        public double dbNowBMScollTime = 0.0;
        public double dbNowScroll = 1.0;
        public double dbNowScrollY = 0.0; //2016.08.13 kairera0467 複素数スクロール
        public double dbLastTime = 0.0; //直前の小節の開始時間
        public double dbLastBMScrollTime = 0.0;

        public int[] bBARLINECUE = new int[ 2 ]; //命令を入れた次の小節の操作を実現するためのフラグ。0 = mainflag, 1 = cuetype
        public bool b小節線を挿入している = false;

        private List<int> listBalloon_Normal;
        private List<int> listBalloon_Expert;
        private List<int> listBalloon_Master;
        private List<int> listBalloon; //旧構文用
        private int listBalloon_Normal_数値管理;
        private int listBalloon_Expert_数値管理;
        private int listBalloon_Master_数値管理;

        public List<string> listLiryc; //歌詞を格納していくリスト。スペル忘れた(ぉい
        //譜面分岐関連
        #region[ 譜面分岐 ]
        private int n現在のコース;
        private bool b最初の分岐である;
        private bool b次の小節が分岐である;
        private bool b次の分岐で数値リセット; //2018.03.16 kairera0467 SECTION処理を分岐判定と同時に行う。
        private bool bBranch中である = false;
        public bool bHIDDENBRANCH; //2016.04.01 kairera0467 選曲画面上、譜面分岐開始前まで譜面分岐の表示を隠す

        private bool bAutoBranch = true; // 2019.8.19 kairera0467 譜面分岐の各コースで一番ノートが取れるコースに自動で分岐させる。(連打を考慮した実装は後々)
        private int[] n現在小節のノート数コース毎 = new int[ 3 ]; // 2019.8.19 kairera0467
        #endregion

        public bool[] b譜面が存在する = new bool[5];

        private string[] dlmtSpace = { " " };
        private string[] dlmtEnter = { "\n" };
        private string[] dlmtCOURSE = { "COURSE:" };

        private int nスクロール方向 = 0;
        //2015.09.18 kairera0467
        //バタフライスライドみたいなアレをやりたいがために実装。
        //次郎2みたいな複素数とかは意味不明なので、方向を指定してスクロールさせることにした。
        //0:通常
        //1:上
        //2:下
        //3:右上
        //4:右下
        //5:左
        //6:左上
        //7:左下

        public string strBGIMAGE_PATH;
        public string strBGVIDEO_PATH;

        public double db出現時刻;
        public double db移動待機時刻;

        public string strBGM_PATH;


        public bool bGOGOTIME; //2018.03.11 kairera0467

#if TEST_NOTEOFFMODE
		public STLANEVALUE<bool> b演奏で直前の音を消音する;
//		public bool bHH演奏で直前のHHを消音する;
//		public bool bGUITAR演奏で直前のGUITARを消音する;
//		public bool bBASS演奏で直前のBASSを消音する;
#endif
		// コンストラクタ

		public CDTX()
		{
            this.nPlayerSide = 0;
			this.TITLE = "";
            this.SUBTITLE = "";
			this.ARTIST = "";
			this.COMMENT = "";
			this.PANEL = "";
			this.GENRE = "";
            this.eジャンル = Eジャンル.None;
			this.PREVIEW = "";
			this.PREIMAGE = "";
			this.BACKGROUND = "";
			this.BACKGROUND_GR = "";
			this.PATH_WAV = "";
			this.BPM = 120.0;
			STDGBVALUE<int> stdgbvalue = new STDGBVALUE<int>();
			stdgbvalue.Drums = 0;
			stdgbvalue.Guitar = 0;
			stdgbvalue.Bass = 0;
			this.LEVEL = stdgbvalue;
            this.bHIDDENBRANCH = false;
			this.db再生速度 = 1.0;
			this.strハッシュofDTXファイル = "";
			this.bチップがある = new STチップがある();
			this.bチップがある.Drums = false;
			this.bチップがある.Guitar = false;
			this.bチップがある.Bass = false;
			this.bチップがある.HHOpen = false;
			this.bチップがある.Ride = false;
			this.bチップがある.LeftCymbal = false;
			this.bチップがある.OpenGuitar = false;
			this.bチップがある.OpenBass = false;
			this.strファイル名 = "";
			this.strフォルダ名 = "";
			this.strファイル名の絶対パス = "";
			this.n無限管理WAV = new int[ 36 * 36 ];
			this.n無限管理BPM = new int[ 36 * 36 ];
            this.listBalloon_Normal_数値管理 = 0;
            this.listBalloon_Expert_数値管理 = 0;
            this.listBalloon_Master_数値管理 = 0;
			this.nRESULTIMAGE用優先順位 = new int[ 7 ];
			this.nRESULTMOVIE用優先順位 = new int[ 7 ];
			this.nRESULTSOUND用優先順位 = new int[ 7 ];

			this.nBGMAdjust = 0;
			this.nPolyphonicSounds = CDTXMania.ConfigIni.nPoliphonicSounds;
			this.dbDTXVPlaySpeed = 1.0f;

            //this.nScoreModeTmp = 1;
            for( int y = 0; y < 5; y++ )
            {
                this.nScoreInit[ 0, y ] = 300;
                this.nScoreInit[ 1, y ] = 1000;
                this.nScoreDiff[ y ] = 120;
                this.b配点が指定されている[ 0, y ] = false;
                this.b配点が指定されている[ 1, y ] = false;
                this.b配点が指定されている[ 2, y ] = false;
            }

            this.bBarLine = true;

            this.dbBarLength = 1.0;

            this.b最初の分岐である = true;
            this.b次の小節が分岐である = false;

#if TEST_NOTEOFFMODE
			this.bHH演奏で直前のHHを消音する = true;
			this.bGUITAR演奏で直前のGUITARを消音する = true;
			this.bBASS演奏で直前のBASSを消音する = true;
#endif
	
		}
		public CDTX( string str全入力文字列 )
			: this()
		{
			this.On活性化();
			this.t入力_全入力文字列から( str全入力文字列 );
		}
		public CDTX( string strファイル名, bool bヘッダのみ )
			: this()
		{
			this.On活性化();
			this.t入力( strファイル名, bヘッダのみ );
		}
		public CDTX( string str全入力文字列, double db再生速度, int nBGMAdjust )
			: this()
		{
			this.On活性化();
			this.t入力_全入力文字列から( str全入力文字列, str全入力文字列, db再生速度, nBGMAdjust );
		}
		public CDTX( string strファイル名, bool bヘッダのみ, double db再生速度, int nBGMAdjust )
			: this()
		{
			this.On活性化();
			this.t入力( strファイル名, bヘッダのみ, db再生速度, nBGMAdjust, 0, 0, false );
		}
		public CDTX( string strファイル名, bool bヘッダのみ, double db再生速度, int nBGMAdjust, int nReadVersion )
			: this()
		{
			this.On活性化();
			this.t入力( strファイル名, bヘッダのみ, db再生速度, nBGMAdjust, nReadVersion, 0, false );
		}
		public CDTX( string strファイル名, bool bヘッダのみ, double db再生速度, int nBGMAdjust, int nReadVersion, int nPlayerSide, bool bSession )
			: this()
		{
			this.On活性化();
			this.t入力( strファイル名, bヘッダのみ, db再生速度, nBGMAdjust, nReadVersion, nPlayerSide, bSession );
		}


		// メソッド

		public int nモニタを考慮した音量( E楽器パート part )
		{
			CConfigIni configIni = CDTXMania.ConfigIni;
			switch( part )
			{
				case E楽器パート.DRUMS:
					if( configIni.b演奏音を強調する.Drums )
					{
						return configIni.n自動再生音量;
					}
					return configIni.n手動再生音量;

				case E楽器パート.GUITAR:
					if( configIni.b演奏音を強調する.Guitar )
					{
						return configIni.n自動再生音量;
					}
					return configIni.n手動再生音量;

				case E楽器パート.BASS:
					if( configIni.b演奏音を強調する.Bass )
					{
						return configIni.n自動再生音量;
					}
					return configIni.n手動再生音量;
			}
			if( ( !configIni.b演奏音を強調する.Drums && !configIni.b演奏音を強調する.Guitar ) && !configIni.b演奏音を強調する.Bass )
			{
				return configIni.n手動再生音量;
			}
			return configIni.n自動再生音量;
		}
		public void tAVIの読み込み()
		{
			if( this.listAVI != null )
			{
				foreach( CAVI cavi in this.listAVI.Values )
				{
					cavi.OnDeviceCreated();
				}
			}
            if( this.listDS != null)
            {
                foreach( CDirectShow cds in this.listDS.Values)
                {
                    cds.OnDeviceCreated();
                }
            }
			if( !this.bヘッダのみ )//&& this.b動画読み込み )
			{
				foreach( CChip chip in this.listChip )
				{
					if( chip.nチャンネル番号 == 0x54 || chip.nチャンネル番号 == 0x5A )
					{
						chip.eAVI種別 = EAVI種別.Unknown;
						chip.rAVI = null;
                        chip.rDShow = null;
						chip.rAVIPan = null;
						if( this.listAVIPAN.ContainsKey( chip.n整数値 ) )
						{
							CAVIPAN cavipan = this.listAVIPAN[ chip.n整数値 ];
							if( this.listAVI.ContainsKey( cavipan.nAVI番号 ) && ( this.listAVI[ cavipan.nAVI番号 ].avi != null ) )
							{
								chip.eAVI種別 = EAVI種別.AVIPAN;
								chip.rAVI = this.listAVI[ cavipan.nAVI番号 ];
                                //if( CDTXMania.ConfigIni.bDirectShowMode == true )
                                    chip.rDShow = this.listDS[ cavipan.nAVI番号 ];
								chip.rAVIPan = cavipan;
								continue;
							}
						}
						if( this.listAVI.ContainsKey( chip.n整数値 ) && ( this.listAVI[ chip.n整数値 ].avi != null ) || ( this.listDS.ContainsKey( chip.n整数値 ) && ( this.listDS[ chip.n整数値 ].dshow != null ) ) )
						{
							chip.eAVI種別 = EAVI種別.AVI;
							chip.rAVI = this.listAVI[ chip.n整数値 ];
                            //if(CDTXMania.ConfigIni.bDirectShowMode == true)
                                chip.rDShow = this.listDS[ chip.n整数値 ];
						}
					}
				}
			}
		}

		public void tWave再生位置自動補正()
		{
			foreach( CWAV cwav in this.listWAV.Values )
			{
				this.tWave再生位置自動補正( cwav );
			}
		}
		public void tWave再生位置自動補正( CWAV wc )
		{
			if ( wc.rSound[ 0 ] != null && wc.rSound[ 0 ].n総演奏時間ms >= 5000 )
			{
				for ( int i = 0; i < nPolyphonicSounds; i++ )
				{
					if ( ( wc.rSound[ i ] != null ) && ( wc.rSound[ i ].b再生中 ) )
					{
						long nCurrentTime = CSound管理.rc演奏用タイマ.nシステム時刻ms;
						if ( nCurrentTime > wc.n再生開始時刻[ i ] )
						{
							long nAbsTimeFromStartPlaying = nCurrentTime - wc.n再生開始時刻[ i ];
							//Trace.TraceInformation( "再生位置自動補正: {0}, seek先={1}ms, 全音長={2}ms",
							//    Path.GetFileName( wc.rSound[ 0 ].strファイル名 ),
							//    nAbsTimeFromStartPlaying,
							//    wc.rSound[ 0 ].n総演奏時間ms
							//);
							// wc.rSound[ i ].t再生位置を変更する( wc.rSound[ i ].t時刻から位置を返す( nAbsTimeFromStartPlaying ) );
							wc.rSound[ i ].t再生位置を変更する( nAbsTimeFromStartPlaying );	// WASAPI/ASIO用
						}
					}
				}
			}
		}
		public void tWavの再生停止( int nWaveの内部番号 )
		{
			tWavの再生停止( nWaveの内部番号, false );
		}
		public void tWavの再生停止( int nWaveの内部番号, bool bミキサーからも削除する )
		{
			if( this.listWAV.ContainsKey( nWaveの内部番号 ) )
			{
				CWAV cwav = this.listWAV[ nWaveの内部番号 ];
				for ( int i = 0; i < nPolyphonicSounds; i++ )
				{
					if( cwav.rSound[ i ] != null && cwav.rSound[ i ].b再生中 )
					{
						if ( bミキサーからも削除する )
						{
							cwav.rSound[ i ].tサウンドを停止してMixerからも削除する();
						}
						else
						{
							cwav.rSound[ i ].t再生を停止する();
						}
					}
				}
			}
		}
		public void tWAVの読み込み( CWAV cwav )
		{
//			Trace.TraceInformation("WAV files={0}", this.listWAV.Count);
//			int count = 0;
//			foreach (CWAV cwav in this.listWAV.Values)
			{
//				string strCount = count.ToString() + " / " + this.listWAV.Count.ToString();
//				Debug.WriteLine(strCount);
//				CDTXMania.act文字コンソール.tPrint(0, 0, C文字コンソール.Eフォント種別.白, strCount);
//				count++;

				string str = string.IsNullOrEmpty(this.PATH_WAV) ? this.strフォルダ名 : this.PATH_WAV;
				str = str + cwav.strファイル名;
				bool bIsDirectSound = ( CDTXMania.Sound管理.GetCurrentSoundDeviceType() == "DirectSound" );
				try
				{
					//try
					//{
					//    cwav.rSound[ 0 ] = CDTXMania.Sound管理.tサウンドを生成する( str );
					//    cwav.rSound[ 0 ].n音量 = 100;
					//    if ( CDTXMania.ConfigIni.bLog作成解放ログ出力 )
					//    {
					//        Trace.TraceInformation( "サウンドを作成しました。({3})({0})({1})({2}bytes)", cwav.strコメント文, str, cwav.rSound[ 0 ].nサウンドバッファサイズ, cwav.rSound[ 0 ].bストリーム再生する ? "Stream" : "OnMemory" );
					//    }
					//}
					//catch
					//{
					//    cwav.rSound[ 0 ] = null;
					//    Trace.TraceError( "サウンドの作成に失敗しました。({0})({1})", cwav.strコメント文, str );
					//}
					//if ( cwav.rSound[ 0 ] == null )	// #xxxxx 2012.5.3 yyagi rSound[1-3]もClone()するようにし、これらのストリーム再生がおかしくなる問題を修正
					//{
					//    for ( int j = 1; j < nPolyphonicSounds; j++ )
					//    {
					//        cwav.rSound[ j ] = null;
					//    }
					//}
					//else
					//{
					//    for ( int j = 1; j < nPolyphonicSounds; j++ )
					//    {
					//        cwav.rSound[ j ] = (CSound) cwav.rSound[ 0 ].Clone();	// #24007 2011.9.5 yyagi add: to accelerate loading chip sounds
					//        CDTXMania.Sound管理.tサウンドを登録する( cwav.rSound[ j ] );
					//    }
					//}

					// まず1つめを登録する
					try
					{
						cwav.rSound[ 0 ] = CDTXMania.Sound管理.tサウンドを生成する( str );
						cwav.rSound[ 0 ].n音量 = 100;
						if ( !CDTXMania.ConfigIni.bDynamicBassMixerManagement )
						{
							cwav.rSound[ 0 ].tBASSサウンドをミキサーに追加する();
						}
						if ( CDTXMania.ConfigIni.bLog作成解放ログ出力 )
						{
							Trace.TraceInformation( "サウンドを作成しました。({3})({0})({1})({2}bytes)", cwav.strコメント文, str, cwav.rSound[ 0 ].nサウンドバッファサイズ, cwav.rSound[ 0 ].bストリーム再生する ? "Stream" : "OnMemory" );
						}
					}
					catch ( Exception e )
					{
						cwav.rSound[ 0 ] = null;
						Trace.TraceError( "サウンドの作成に失敗しました。({0})({1})", cwav.strコメント文, str );
						Trace.TraceError( "例外: " + e.Message );
					}

					#region [ 同時発音数を、チャンネルによって変える ]
					int nPoly = nPolyphonicSounds;
					if ( CDTXMania.Sound管理.GetCurrentSoundDeviceType() != "DirectSound" )	// DShowでの再生の場合はミキシング負荷が高くないため、
					{																		// チップのライフタイム管理を行わない
						if      ( cwav.bIsBassSound )   nPoly = (nPolyphonicSounds >= 2)? 2 : 1;
						else if ( cwav.bIsGuitarSound ) nPoly = (nPolyphonicSounds >= 2)? 2 : 1;
						else if ( cwav.bIsSESound )     nPoly = 1;
						else if ( cwav.bIsBGMSound)     nPoly = 1;
					}
					if ( cwav.bIsBGMSound ) nPoly = 1;
					#endregion

					// 残りはClone等で登録する
					//if ( bIsDirectSound )	// DShowでの再生の場合はCloneする
					//{
					//    for ( int i = 1; i < nPoly; i++ )
					//    {
					//        cwav.rSound[ i ] = (CSound) cwav.rSound[ 0 ].Clone();	// #24007 2011.9.5 yyagi add: to accelerate loading chip sounds
					//        // CDTXMania.Sound管理.tサウンドを登録する( cwav.rSound[ j ] );
					//    }
					//    for ( int i = nPoly; i < nPolyphonicSounds; i++ )
					//    {
					//        cwav.rSound[ i ] = null;
					//    }
					//}
					//else															// WASAPI/ASIO時は通常通り登録
					{
						for ( int i = 1; i < nPoly; i++ )
						{
							try
							{
								cwav.rSound[ i ] = CDTXMania.Sound管理.tサウンドを生成する( str );
								cwav.rSound[ i ].n音量 = 100;
								if ( !CDTXMania.ConfigIni.bDynamicBassMixerManagement )
								{
									cwav.rSound[ i ].tBASSサウンドをミキサーに追加する();
								}
								if ( CDTXMania.ConfigIni.bLog作成解放ログ出力 )
								{
									Trace.TraceInformation( "サウンドを作成しました。({3})({0})({1})({2}bytes)", cwav.strコメント文, str, cwav.rSound[ 0 ].nサウンドバッファサイズ, cwav.rSound[ 0 ].bストリーム再生する ? "Stream" : "OnMemory" );
								}
							}
							catch ( Exception e )
							{
								cwav.rSound[ i ] = null;
								Trace.TraceError( "サウンドの作成に失敗しました。({0})({1})", cwav.strコメント文, str );
								Trace.TraceError( "例外: " + e.Message );
							}
						}
					}
				}
				catch( Exception exception )
				{
					Trace.TraceError( "サウンドの生成に失敗しました。({0})({1})({2})", exception.Message, cwav.strコメント文, str );
					for ( int j = 0; j < nPolyphonicSounds; j++ )
					{
						cwav.rSound[ j ] = null;
					}
					//continue;
				}
			}
		}
		public static string tZZ( int n )
		{
			if( n < 0 || n >= 36 * 36 )
				return "!!";	// オーバー／アンダーフロー。

			// n を36進数2桁の文字列にして返す。

			string str = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
			return new string( new char[] { str[ n / 36 ], str[ n % 36 ] } );
		}
		public void tギターとベースのランダム化(E楽器パート part, Eランダムモード eRandom)
		{
		}
        public void t太鼓チップのランダム化( Eランダムモード eRandom )
        {
            //2016.02.11 kairera0467
            //なんだよこのクソ実装は(怒)
            Random rnd = new System.Random();

            switch( eRandom )
            {
                case Eランダムモード.MIRROR:
                    foreach( var chip in this.listChip )
                    {
                        switch( chip.nチャンネル番号 )
                        {
                            case 0x11:
                                chip.nチャンネル番号 = 0x12;
                                break;
                            case 0x12:
                                chip.nチャンネル番号 = 0x11;
                                break;
                            case 0x13:
                                chip.nチャンネル番号 = 0x14;
                                break;
                            case 0x14:
                                chip.nチャンネル番号 = 0x13;
                                break;
                        }
                    }
                    this.tSenotes_Core_V2( this.listChip );
                    break;
                case Eランダムモード.RANDOM:
                    foreach( var chip in this.listChip )
                    {
                        int n = rnd.Next( 50 );

                        if( n >= 5 && n <= 10 )
                        {
                            switch( chip.nチャンネル番号 )
                            {
                                case 0x11:
                                    chip.nチャンネル番号 = 0x12;
                                    break;
                                case 0x12:
                                    chip.nチャンネル番号 = 0x11;
                                    break;
                                case 0x13:
                                    chip.nチャンネル番号 = 0x14;
                                    break;
                                case 0x14:
                                    chip.nチャンネル番号 = 0x13;
                                    break;
                            }
                        }
                    }
                    this.tSenotes_Core_V2( this.listChip );
                    break;
                case Eランダムモード.SUPERRANDOM:
                    foreach( var chip in this.listChip )
                    {
                        int n = rnd.Next( 80 );

                        if( n >= 3 && n <= 43 )
                        {
                            switch( chip.nチャンネル番号 )
                            {
                                case 0x11:
                                    chip.nチャンネル番号 = 0x12;
                                    break;
                                case 0x12:
                                    chip.nチャンネル番号 = 0x11;
                                    break;
                                case 0x13:
                                    chip.nチャンネル番号 = 0x14;
                                    break;
                                case 0x14:
                                    chip.nチャンネル番号 = 0x13;
                                    break;
                            }
                        }
                    }
                    this.tSenotes_Core_V2( this.listChip );
                    break;
                case Eランダムモード.HYPERRANDOM:
                    foreach( var chip in this.listChip )
                    {
                        int n = rnd.Next( 100 );

                        if( n >= 20 && n <= 80 )
                        {
                            switch( chip.nチャンネル番号 )
                            {
                                case 0x11:
                                    chip.nチャンネル番号 = 0x12;
                                    break;
                                case 0x12:
                                    chip.nチャンネル番号 = 0x11;
                                    break;
                                case 0x13:
                                    chip.nチャンネル番号 = 0x14;
                                    break;
                                case 0x14:
                                    chip.nチャンネル番号 = 0x13;
                                    break;
                            }
                        }
                    }
                    this.tSenotes_Core_V2( this.listChip );
                    break;
                case Eランダムモード.OFF:
                default:
                    break;
            }
        }

		#region [ チップの再生と停止 ]
		public void tチップの再生( CChip rChip, long n再生開始システム時刻ms, int nLane )
		{
			this.tチップの再生( rChip, n再生開始システム時刻ms, nLane, CDTXMania.ConfigIni.n自動再生音量, false, false );
		}
		public void tチップの再生( CChip rChip, long n再生開始システム時刻ms, int nLane, int nVol )
		{
			this.tチップの再生( rChip, n再生開始システム時刻ms, nLane, nVol, false, false );
		}
		public void tチップの再生( CChip rChip, long n再生開始システム時刻ms, int nLane, int nVol, bool bMIDIMonitor )
		{
			this.tチップの再生( rChip, n再生開始システム時刻ms, nLane, nVol, bMIDIMonitor, false );
		}
		public void tチップの再生( CChip pChip, long n再生開始システム時刻ms, int nLane, int nVol, bool bMIDIMonitor, bool bBad )
		{
			if( pChip.n整数値_内部番号 >= 0 )
			{
				if( ( nLane < (int) Eレーン.LC ) || ( (int) Eレーン.BGM < nLane ) )
				{
					throw new ArgumentOutOfRangeException();
				}
				if( this.listWAV.ContainsKey( pChip.n整数値_内部番号 ) )
				{
					CWAV wc = this.listWAV[ pChip.n整数値_内部番号 ];
					int index = wc.n現在再生中のサウンド番号 = ( wc.n現在再生中のサウンド番号 + 1 ) % nPolyphonicSounds;
					if( ( wc.rSound[ 0 ] != null ) && 
						( wc.rSound[ 0 ].bストリーム再生する || wc.rSound[index] == null ) )
					{
						index = wc.n現在再生中のサウンド番号 = 0;
					}
					CSound sound = wc.rSound[ index ];
					if( sound != null )
					{
						if( bBad )
						{
							sound.db周波数倍率 = ( (float) ( 100 + ( ( ( CDTXMania.Random.Next( 3 ) + 1 ) * 7 ) * ( 1 - ( CDTXMania.Random.Next( 2 ) * 2 ) ) ) ) ) / 100f;
						}
						else
						{
							sound.db周波数倍率 = 1.0;
						}
						sound.db再生速度 = ( (double) CDTXMania.ConfigIni.n演奏速度 ) / 20.0;
						// 再生速度によって、WASAPI/ASIOで使う使用mixerが決まるため、付随情報の設定(音量/PAN)は、再生速度の設定後に行う
						sound.n音量 = (int) ( ( (double) ( nVol * wc.n音量 ) ) / 100.0 );
						sound.n位置 = wc.n位置;
						sound.t再生を開始する();
					}
					wc.n再生開始時刻[ wc.n現在再生中のサウンド番号 ] = n再生開始システム時刻ms;
					this.tWave再生位置自動補正( wc );
				}
			}
		}
		public void t各自動再生音チップの再生時刻を変更する( int nBGMAdjustの増減値 )
		{
			this.nBGMAdjust += nBGMAdjustの増減値;
			for( int i = 0; i < this.listChip.Count; i++ )
			{
				int nChannelNumber = this.listChip[ i ].nチャンネル番号;
				if( ( (
						( nChannelNumber == 1 ) ||
						( ( 0x61 <= nChannelNumber ) && ( nChannelNumber <= 0x69 ) )
					  ) ||
						( ( 0x70 <= nChannelNumber ) && ( nChannelNumber <= 0x79 ) )
					) ||
					( ( ( 0x80 <= nChannelNumber ) && ( nChannelNumber <= 0x89 ) ) || ( ( 0x90 <= nChannelNumber ) && ( nChannelNumber <= 0x92 ) ) )
				  )
				{
					this.listChip[ i ].n発声時刻ms += nBGMAdjustの増減値;
				}
			}
			foreach( CWAV cwav in this.listWAV.Values )
			{
				for ( int j = 0; j < nPolyphonicSounds; j++ )
				{
					if( ( cwav.rSound[ j ] != null ) && cwav.rSound[ j ].b再生中 )
					{
						cwav.n再生開始時刻[ j ] += nBGMAdjustの増減値;
					}
				}
			}
		}
		public void t全チップの再生一時停止()
		{
			foreach( CWAV cwav in this.listWAV.Values )
			{
				for ( int i = 0; i < nPolyphonicSounds; i++ )
				{
					if( ( cwav.rSound[ i ] != null ) && cwav.rSound[ i ].b再生中 )
					{
						cwav.rSound[ i ].t再生を一時停止する();
						cwav.n一時停止時刻[ i ] = CSound管理.rc演奏用タイマ.nシステム時刻ms;
					}
				}
			}
		}
		public void t全チップの再生再開()
		{
			foreach( CWAV cwav in this.listWAV.Values )
			{
				for ( int i = 0; i < nPolyphonicSounds; i++ )
				{
					if( ( cwav.rSound[ i ] != null ) && cwav.rSound[ i ].b一時停止中 )
					{
						//long num1 = cwav.n一時停止時刻[ i ];
						//long num2 = cwav.n再生開始時刻[ i ];
						cwav.rSound[ i ].t再生を再開する( cwav.n一時停止時刻[ i ] - cwav.n再生開始時刻[ i ] );
						cwav.n再生開始時刻[ i ] += CSound管理.rc演奏用タイマ.nシステム時刻ms - cwav.n一時停止時刻[ i ];
					}
				}
			}
		}
		public void t全チップの再生停止()
		{
			foreach( CWAV cwav in this.listWAV.Values )
			{
				this.tWavの再生停止( cwav.n内部番号 );
			}
		}
		public void t全チップの再生停止とミキサーからの削除()
		{
			foreach( CWAV cwav in this.listWAV.Values )
			{
				this.tWavの再生停止( cwav.n内部番号, true );
			}
		}
		#endregion

		public void t入力( string strファイル名, bool bヘッダのみ )
		{
			this.t入力( strファイル名, bヘッダのみ, 1.0, 0, 0, 0, false );
		}
		public void t入力( string strファイル名, bool bヘッダのみ, double db再生速度, int nBGMAdjust, int nReadVersion, int nPlayerSide, bool bSession )
		{
			this.bヘッダのみ = bヘッダのみ;
			this.strファイル名の絶対パス = Path.GetFullPath( strファイル名 );
			this.strファイル名 = Path.GetFileName( this.strファイル名の絶対パス );
			this.strフォルダ名 = Path.GetDirectoryName( this.strファイル名の絶対パス ) + @"\";
			//if ( this.e種別 != E種別.SMF )
			{
				try
				{
                    this.nPlayerSide = nPlayerSide;
                    this.bSession譜面を読み込む = bSession;
                    if( nReadVersion != 0 )
                    {
                        //DTX方式
                        
                        //DateTime timeBeginLoad = DateTime.Now;
					    //TimeSpan span;
    			        string[] files = Directory.GetFiles( this.strフォルダ名, "*.tja" );

	    				StreamReader reader = new StreamReader( strファイル名, Encoding.GetEncoding( "Shift_JIS" ) );
		    			string str2 = reader.ReadToEnd();
			    		reader.Close();

				    	//StreamReader reader2 = new StreamReader( this.strフォルダ名 + "test.tja", Encoding.GetEncoding( "Shift_JIS" ) );
                        StreamReader reader2 = new StreamReader( files[0], Encoding.GetEncoding( "Shift_JIS" ) );
    					string str3 = reader2.ReadToEnd();
	    				reader2.Close();

		    			//span = (TimeSpan) ( DateTime.Now - timeBeginLoad );
			    		//Trace.TraceInformation( "DTXfileload時間:          {0}", span.ToString() );

					    this.t入力_全入力文字列から( str2, str3, db再生速度, nBGMAdjust );
                    }
                    else
                    {
                        //次郎方式

                        //DateTime timeBeginLoad = DateTime.Now;
					    //TimeSpan span;

                        StreamReader reader = new StreamReader( strファイル名, Encoding.GetEncoding( "Shift_JIS" ) );
		    			string str2 = reader.ReadToEnd();
			    		reader.Close();

				    	//StreamReader reader2 = new StreamReader( this.strフォルダ名 + "test.tja", Encoding.GetEncoding( "Shift_JIS" ) );
                        StreamReader reader2 = new StreamReader( strファイル名, Encoding.GetEncoding( "Shift_JIS" ) );
    					string str3 = reader2.ReadToEnd();
	    				reader2.Close();

		    			//span = (TimeSpan) ( DateTime.Now - timeBeginLoad );
			    		//Trace.TraceInformation( "DTXfileload時間:          {0}", span.ToString() );
                        
                        this.t入力_全入力文字列から( str2, str3, db再生速度, nBGMAdjust );
                    }
				}
				catch( Exception ex )
				{
                    //MessageBox.Show( "おや?エラーが出たようです。お兄様。" );
                    Trace.TraceError( "おや?エラーが出たようです。お兄様。" );
                    Trace.TraceError( "エラー:{0}", ex.StackTrace );
				}
			}
		}
		public void t入力_全入力文字列から( string str全入力文字列 )
		{
			this.t入力_全入力文字列から( str全入力文字列, str全入力文字列, 1.0, 0 );
		}
		public void t入力_全入力文字列から( string str全入力文字列, string str1, double db再生速度, int nBGMAdjust )
		{
			//DateTime timeBeginLoad = DateTime.Now;
			//TimeSpan span;

			if ( !string.IsNullOrEmpty( str全入力文字列 ) )
			{
				#region [ 改行カット ]
				this.db再生速度 = db再生速度;
				str全入力文字列 = str全入力文字列.Replace( Environment.NewLine, "\n" );
				str全入力文字列 = str全入力文字列.Replace( '\t', ' ' );
				str全入力文字列 = str全入力文字列 + "\n";
				#endregion
				//span = (TimeSpan) ( DateTime.Now - timeBeginLoad );
				//Trace.TraceInformation( "改行カット時間:           {0}", span.ToString() );
				//timeBeginLoad = DateTime.Now;
				#region [ 初期化 ]
				for ( int j = 0; j < 36 * 36; j++ )
				{
					this.n無限管理WAV[ j ] = -j;
					this.n無限管理BPM[ j ] = -j;
				}
				this.n内部番号WAV1to = 1;
				this.n内部番号BPM1to = 1;
				this.bstackIFからENDIFをスキップする = new Stack<bool>();
				this.bstackIFからENDIFをスキップする.Push( false );
				this.n現在の乱数 = 0;
				for ( int k = 0; k < 7; k++ )
				{
					this.nRESULTIMAGE用優先順位[ k ] = 0;
					this.nRESULTMOVIE用優先順位[ k ] = 0;
					this.nRESULTSOUND用優先順位[ k ] = 0;
				}
				#endregion
				#region [ 入力/行解析 ]
                #region[初期化]
                this.dbNowScroll = 1.0;
                this.dbNowSCROLL_Normal = new double[]{ 1.0, 0.0 };
                this.dbNowSCROLL_Expert = new double[]{ 1.0, 0.0 };
                this.dbNowSCROLL_Master = new double[]{ 1.0, 0.0 };
                this.n現在のコース = 0;
                #endregion
				CharEnumerator ce = str全入力文字列.GetEnumerator();
				if ( ce.MoveNext() )
				{
					this.n現在の行数 = 1;
					do
					{
						if ( !this.t入力_空白と改行をスキップする( ref ce ) )
						{
							break;
						}
                        if (this.listChip.Count == 0)
                        {
                            //this.t入力(str1);
                            //this.t入力_V3( str1, 3 );
                            this.t入力_V4( str1 );
                        }
						if ( ce.Current == '#' )
						{
							if ( ce.MoveNext() )
							{
								StringBuilder builder = new StringBuilder( 0x20 );
								if ( this.t入力_コマンド文字列を抜き出す( ref ce, ref builder ) )
								{
									StringBuilder builder2 = new StringBuilder( 0x400 );
									if ( this.t入力_パラメータ文字列を抜き出す( ref ce, ref builder2 ) )
									{
										StringBuilder builder3 = new StringBuilder( 0x400 );
										if ( this.t入力_コメント文字列を抜き出す( ref ce, ref builder3 ) )
										{
											this.t入力_行解析( ref builder, ref builder2, ref builder3 );

											this.n現在の行数++;
											continue;
										}
									}
								}
							}
							break;
						}
                        //this.t入力(str1);
					}
					while ( this.t入力_コメントをスキップする( ref ce ) );

				#endregion
					//span = (TimeSpan) ( DateTime.Now - timeBeginLoad );
					//Trace.TraceInformation( "抜き出し時間:             {0}", span.ToString() );
					//timeBeginLoad = DateTime.Now;
					this.n無限管理WAV = null;
					this.n無限管理BPM = null;
                    //this.t入力_行解析ヘッダ( str1 );
					if ( !this.bヘッダのみ )
					{
						#region [ BPM/BMP初期化 ]
						int ch;
						CBPM cbpm = null;
						foreach ( CBPM cbpm2 in this.listBPM.Values )
						{
							if ( cbpm2.n表記上の番号 == 0 )
							{
								cbpm = cbpm2;
								break;
							}
						}
						if ( cbpm == null )
						{
							cbpm = new CBPM();
							cbpm.n内部番号 = this.n内部番号BPM1to++;
							cbpm.n表記上の番号 = 0;
							cbpm.dbBPM値 = 120.0;
							this.listBPM.Add( cbpm.n内部番号, cbpm );
							CChip chip = new CChip();
							chip.n発声位置 = 0;
							chip.nチャンネル番号 = 8;		// 拡張BPM
							chip.n整数値 = 0;
							chip.n整数値_内部番号 = cbpm.n内部番号;
							this.listChip.Insert( 0, chip );
						}
						else
						{
							CChip chip = new CChip();
							chip.n発声位置 = 0;
							chip.nチャンネル番号 = 8;		// 拡張BPM
							chip.n整数値 = 0;
							chip.n整数値_内部番号 = cbpm.n内部番号;
							this.listChip.Insert( 0, chip );
						}
						#endregion
						//span = (TimeSpan) ( DateTime.Now - timeBeginLoad );
						//Trace.TraceInformation( "前準備完了時間:           {0}", span.ToString() );
						//timeBeginLoad = DateTime.Now;
						#region [ CWAV初期化 ]
						foreach ( CWAV cwav in this.listWAV.Values )
						{
							if ( cwav.nチップサイズ < 0 )
							{
								cwav.nチップサイズ = 100;
							}
							if ( cwav.n位置 <= -10000 )
							{
								cwav.n位置 = 0;
							}
							if ( cwav.n音量 < 0 )
							{
								cwav.n音量 = 100;
							}
						}
						#endregion
						//span = (TimeSpan) ( DateTime.Now - timeBeginLoad );
						//Trace.TraceInformation( "CWAV前準備時間:           {0}", span.ToString() );
						//timeBeginLoad = DateTime.Now;
						#region [ チップ倍率設定 ]						// #28145 2012.4.22 yyagi 二重ループを1重ループに変更して高速化)
						//foreach ( CWAV cwav in this.listWAV.Values )
						//{
						//    foreach( CChip chip in this.listChip )
						//    {
						//        if( chip.n整数値_内部番号 == cwav.n内部番号 )
						//        {
						//            chip.dbチップサイズ倍率 = ( (double) cwav.nチップサイズ ) / 100.0;
						//            if (chip.nチャンネル番号 == 0x01 )	// BGMだったら
						//            {
						//                cwav.bIsOnBGMLane = true;
						//            }
						//        }
						//    }
						//}
						foreach ( CChip chip in this.listChip )
						{
							if ( this.listWAV.ContainsKey( chip.n整数値_内部番号 ) )
							//foreach ( CWAV cwav in this.listWAV.Values )
							{
								CWAV cwav = this.listWAV[ chip.n整数値_内部番号 ];
								//	if ( chip.n整数値_内部番号 == cwav.n内部番号 )
								//	{
								chip.dbチップサイズ倍率 = ( (double) cwav.nチップサイズ ) / 100.0;
								//if ( chip.nチャンネル番号 == 0x01 )	// BGMだったら
								//{
								//	cwav.bIsOnBGMLane = true;
								//}
								//	}
							}
						}
						#endregion
						//span = (TimeSpan) ( DateTime.Now - timeBeginLoad );
						//Trace.TraceInformation( "CWAV全準備時間:           {0}", span.ToString() );
						//timeBeginLoad = DateTime.Now;
						#region [ 必要に応じて空打ち音を0小節に定義する ]
						//for ( int m = 0xb1; m <= 0xbc; m++ )			// #28146 2012.4.21 yyagi; bb -> bc
						//{
						//    foreach ( CChip chip in this.listChip )
						//    {
						//        if ( chip.nチャンネル番号 == m )
						//        {
						//            CChip c = new CChip();
						//            c.n発声位置 = 0;
						//            c.nチャンネル番号 = chip.nチャンネル番号;
						//            c.n整数値 = chip.n整数値;
						//            c.n整数値_内部番号 = chip.n整数値_内部番号;
						//            this.listChip.Insert( 0, c );
						//            break;
						//        }
						//    }
						//}
						#endregion
						//span = (TimeSpan) ( DateTime.Now - timeBeginLoad );
						//Trace.TraceInformation( "空打確認時間:             {0}", span.ToString() );
						//timeBeginLoad = DateTime.Now;
						#region [ 拍子_拍線の挿入 ]
						if ( this.listChip.Count > 0 )
						{
							this.listChip.Sort();		// 高速化のためにはこれを削りたいが、listChipの最後がn発声位置の終端である必要があるので、
							// 保守性確保を優先してここでのソートは残しておく
							// なお、093時点では、このソートを削除しても動作するようにはしてある。
							// (ここまでの一部チップ登録を、listChip.Add(c)から同Insert(0,c)に変更してある)
							// これにより、数ms程度ながらここでのソートも高速化されている。

                            //double barlength = 1.0;
                            //int nEndOfSong = ( this.listChip[ this.listChip.Count - 1 ].n発声位置 + 384 ) - ( this.listChip[ this.listChip.Count - 1 ].n発声位置 % 384 );
                            //for ( int tick384 = 0; tick384 <= nEndOfSong; tick384 += 384 )	// 小節線の挿入　(後に出てくる拍子線とループをまとめようとするなら、forループの終了条件の微妙な違いに注意が必要)
                            //{
                            //    CChip chip = new CChip();
                            //    chip.n発声位置 = tick384;
                            //    chip.nチャンネル番号 = 0x50;	// 小節線
                            //    chip.n整数値 = 36 * 36 - 1;
                            //    chip.dbSCROLL = 1.0;
                            //    this.listChip.Add( chip );
                            //}
                            ////this.listChip.Sort();				// ここでのソートは不要。ただし最後にソートすること
                            //int nChipNo_BarLength = 0;
                            //int nChipNo_C1 = 0;

							//this.listChip.Sort();
						}
						#endregion
						//span = (TimeSpan) ( DateTime.Now - timeBeginLoad );
						//Trace.TraceInformation( "拍子_拍線挿入時間:       {0}", span.ToString() );
						//timeBeginLoad = DateTime.Now;
						#region [ C2 [拍線_小節線表示指定] の処理 ]		// #28145 2012.4.21 yyagi; 2重ループをほぼ1重にして高速化
						bool bShowBeatBarLine = true;
						for ( int i = 0; i < this.listChip.Count; i++ )
						{
							bool bChangedBeatBarStatus = false;
							if ( ( this.listChip[ i ].nチャンネル番号 == 0xc2 ) )
							{
								if ( this.listChip[ i ].n整数値 == 1 )				// BAR/BEAT LINE = ON
								{
									bShowBeatBarLine = true;
									bChangedBeatBarStatus = true;
								}
								else if ( this.listChip[ i ].n整数値 == 2 )			// BAR/BEAT LINE = OFF
								{
									bShowBeatBarLine = false;
									bChangedBeatBarStatus = true;
								}
							}
							int startIndex = i;
							if ( bChangedBeatBarStatus )							// C2チップの前に50/51チップが来ている可能性に配慮
							{
								while ( startIndex > 0 && this.listChip[ startIndex ].n発声位置 == this.listChip[ i ].n発声位置 )
								{
									startIndex--;
								}
								startIndex++;	// 1つ小さく過ぎているので、戻す
							}
							for ( int j = startIndex; j <= i; j++ )
							{
								if ( ( ( this.listChip[ j ].nチャンネル番号 == 0x50 ) || ( this.listChip[ j ].nチャンネル番号 == 0x51 ) ) &&
									( this.listChip[ j ].n整数値 == ( 36 * 36 - 1 ) ) )
								{
									this.listChip[ j ].b可視 = bShowBeatBarLine;
								}
							}
						}
						#endregion
						//span = (TimeSpan) ( DateTime.Now - timeBeginLoad );
						//Trace.TraceInformation( "C2 [拍線_小節線表示指定]:  {0}", span.ToString() );
						//timeBeginLoad = DateTime.Now;
                        this.n内部番号BRANCH1to = 0;
                        this.n内部番号JSCROLL1to = 0;
						#region [ 発声時刻の計算 ]
						double bpm = 120.0;
						//double dbBarLength = 1.0;
						int n発声位置 = 0;
						int ms = 0;
						int nBar = 0;
                        int nCount = 0;
                        this.nNowRollCount = 0;

                        foreach ( CChip chip in this.listChip )
                        {
                            if( chip.nチャンネル番号 == 0x02 ){}
                            //else if( chip.nチャンネル番号 == 0x03 ){}
                            else if( chip.nチャンネル番号 == 0x01 ){}
                            else if( chip.nチャンネル番号 == 0x08 ){}
                            else if( chip.nチャンネル番号 >= 0x11 && chip.nチャンネル番号 <= 0x1F ){}
                            else if( chip.nチャンネル番号 == 0x50 ){}
                            else if( chip.nチャンネル番号 == 0x51 ){}
                            else if( chip.nチャンネル番号 == 0x54 ){}
                            else if( chip.nチャンネル番号 == 0x08 ){}
                            else if( chip.nチャンネル番号 == 0xF1 ){}
                            else if( chip.nチャンネル番号 == 0xF2 ){}
                            else if( chip.nチャンネル番号 == 0xFF ){}
                            else if( chip.nチャンネル番号 == 0xDD ){ chip.n発声時刻ms = ms + ( (int) ( ( ( 625 * ( chip.n発声位置 - n発声位置 ) ) * this.dbBarLength ) / bpm ) ); }
                            else if( chip.nチャンネル番号 == 0xDF ){ chip.n発声時刻ms = ms + ( (int) ( ( ( 625 * ( chip.n発声位置 - n発声位置 ) ) * this.dbBarLength ) / bpm ) ); }
                            else if( chip.nチャンネル番号 < 0x93 )
                                chip.n発声時刻ms = ms + ( (int) ( ( ( 625 * ( chip.n発声位置 - n発声位置 ) ) * this.dbBarLength ) / bpm ) );
                            else if( ( chip.nチャンネル番号 > 0x9F && chip.nチャンネル番号 < 0xA0 ) || ( chip.nチャンネル番号 >= 0xF0 && chip.nチャンネル番号 < 0xFE ) )
                                chip.n発声時刻ms = ms + ( (int) ( ( ( 625 * ( chip.n発声位置 - n発声位置 ) ) * this.dbBarLength ) / bpm ) );
                            //else if( chip.nチャンネル番号 > 0xDF )
                            //    chip.n発声時刻ms = ms + ( (int) ( ( ( 625 * ( chip.n発声位置 - n発声位置 ) ) * this.dbBarLength ) / bpm ) );

                            //chip.n発声時刻ms += nDELAY;
                            //chip.nノーツ終了時刻ms += nDELAY;
                            if ( ( ( this.e種別 == E種別.BMS ) || ( this.e種別 == E種別.BME ) ) && ( ( this.dbBarLength != 1.0 ) && ( ( chip.n発声位置 / 384 ) != nBar ) ) )
							{
								n発声位置 = chip.n発声位置;
								ms = chip.n発声時刻ms;
								this.dbBarLength = 1.0;
							}
							nBar = chip.n発声位置 / 384;
							ch = chip.nチャンネル番号;

                            nCount++;
                            this.nNowRollCount++;
                            
                            switch ( ch )
							{
                                case 0x01:
									{
										n発声位置 = chip.n発声位置;

                                        if( this.bOFFSETの値がマイナスである == false )
                                            chip.n発声時刻ms += this.nOFFSET;
										ms = chip.n発声時刻ms;
										continue;
									}
								case 0x02:	// BarLength
									{
										n発声位置 = chip.n発声位置;
                                        if( this.bOFFSETの値がマイナスである == false )
                                            chip.n発声時刻ms += this.nOFFSET;
										ms = chip.n発声時刻ms;
										dbBarLength = chip.db実数値;
										continue;
									}
								case 0x03:	// BPM
									{
										n発声位置 = chip.n発声位置;
                                        if( this.bOFFSETの値がマイナスである == false )
										    chip.n発声時刻ms += this.nOFFSET;
                                        ms = chip.n発声時刻ms;
										bpm = this.BASEBPM + chip.n整数値;
                                        this.dbNowBPM = bpm;
										continue;
									}
								case 0x04:	// BGA (レイヤBGA1)
								case 0x07:	// レイヤBGA2
                                    break;

                                case 0x15:
                                case 0x16:
                                case 0x17:
                                    {
                                        if( this.bOFFSETの値がマイナスである )
                                        {
                                            chip.n発声時刻ms += this.nOFFSET;
                                            chip.nノーツ終了時刻ms += this.nOFFSET;
                                        }

                                        this.nNowRoll = this.nNowRollCount - 1;
                                        continue;
                                    }
                                case 0x18:
                                    {
                                        if( this.bOFFSETの値がマイナスである )
                                        {
                                            chip.n発声時刻ms += this.nOFFSET;
                                        }
                                        continue;
                                    }

								case 0x55:
								case 0x56:
								case 0x57:
								case 0x58:
								case 0x59:
								case 0x60:
									break;

                                case 0x50:
                                    {
                                        if( this.bOFFSETの値がマイナスである )
                                            chip.n発声時刻ms += this.nOFFSET;
                                        //chip.n発声時刻ms += this.nDELAY;
                                        //chip.dbBPM = this.dbNowBPM;
                                        //chip.dbSCROLL = this.dbNowSCROLL;

                                        if( this.n内部番号BRANCH1to + 1 > this.listBRANCH.Count )
                                            continue;

                                        if( this.listBRANCH[ this.n内部番号BRANCH1to ].n現在の小節 == nBar )
                                        {
                                            chip.bBranchLine = true;
                                            this.b次の小節が分岐である = false;
                                            this.n内部番号BRANCH1to++;
                                        }

                                        //switch (this.n現在のコース)
                                        //{
                                        //    case 0:
                                        //        chip.dbSCROLL = this.dbNowSCROLL_Normal;
                                        //        break;
                                        //    case 1:
                                        //        chip.dbSCROLL = this.dbNowSCROLL_Expert;
                                        //        break;
                                        //    case 2:
                                        //        chip.dbSCROLL = this.dbNowSCROLL_Master;
                                        //        break;
                                        //}

                                        //if( this.bBarLine == true )
                                        //    chip.b可視 = true;
                                        //else
                                        //    chip.b可視 = false;

                                        //if( this.b次の小節が分岐である )
                                        //{
                                        //    chip.bBranch = true;
                                        //    this.b次の小節が分岐である = false;
                                        //}
                                        continue;
                                    }

								case 0x05:	// Extended Object (非対応)
								case 0x06:	// Missアニメ (非対応)
								case 0x5A:	// 未定義
								case 0x5b:	// 未定義
								case 0x5c:	// 未定義
								case 0x5d:	// 未定義
								case 0x5e:	// 未定義
								case 0x5f:	// 未定義
									{
										continue;
									}
								case 0x08:	// 拡張BPM
									{
										n発声位置 = chip.n発声位置;
                                        if( this.bOFFSETの値がマイナスである == false )
                                            chip.n発声時刻ms += this.nOFFSET;
										ms = chip.n発声時刻ms;
										if ( this.listBPM.ContainsKey( chip.n整数値_内部番号 ) )
										{
											bpm = ( ( this.listBPM[ chip.n整数値_内部番号 ].n表記上の番号 == 0 ) ? 0.0 : this.BASEBPM ) + this.listBPM[ chip.n整数値_内部番号 ].dbBPM値;
                                            this.dbNowBPM = bpm;
										}
										continue;
									}
								case 0x54:	// 動画再生
									{
                                        if( this.bOFFSETの値がマイナスである == false )
                                            chip.n発声時刻ms += this.nOFFSET;
                                        if( this.bMOVIEOFFSETの値がマイナスである == false )
                                            chip.n発声時刻ms += this.nMOVIEOFFSET;
                                        else
                                            chip.n発声時刻ms -= this.nMOVIEOFFSET;
										if ( this.listAVIPAN.ContainsKey( chip.n整数値 ) )
										{
											int num21 = ms + ( (int) ( ( ( 0x271 * ( chip.n発声位置 - n発声位置 ) ) * this.dbBarLength ) / bpm ) );
											int num22 = ms + ( (int) ( ( ( 0x271 * ( ( chip.n発声位置 + this.listAVIPAN[ chip.n整数値 ].n移動時間ct ) - n発声位置 ) ) * this.dbBarLength ) / bpm ) );
											chip.n総移動時間 = num22 - num21;
										}
										continue;
									}
                                case 0x97:
                                case 0x98:
                                case 0x99:
                                    {
                                        if( this.bOFFSETの値がマイナスである )
                                        {
                                            chip.n発声時刻ms += this.nOFFSET;
                                            chip.nノーツ終了時刻ms += this.nOFFSET;
                                        }

                                        //chip.dbBPM = this.dbNowBPM;
                                        //chip.dbSCROLL = this.dbNowSCROLL;
                                        this.nNowRoll = this.nNowRollCount - 1;

                                        //chip.nノーツ終了時刻ms = ms + ( (int) ( ( ( 0x271 * ( chip.nノーツ終了位置 - n発声位置 ) ) * dbBarLength ) / bpm ) );

                                        #region[チップ番号を記録]
                                        //switch(chip.nコース)
                                        //{
                                        //    case 0:
                                        //        this.n連打チップ_temp[0] = this.nNowRoll;
                                        //        this.dbSCROLL_temp[0] = this.dbNowSCROLL;
                                        //        break;
                                        //    case 1:
                                        //        this.n連打チップ_temp[1] = this.nNowRoll;
                                        //        this.dbSCROLL_temp[1] = this.dbNowSCROLL;
                                        //        break;
                                        //    case 2:
                                        //        this.n連打チップ_temp[2] = this.nNowRoll;
                                        //        this.dbSCROLL_temp[2] = this.dbNowSCROLL;
                                        //        break;
                                        //}

                                        #endregion

                                        continue;
                                    }
                                case 0x9A:
                                    {

                                        if( this.bOFFSETの値がマイナスである )
                                        {
                                            chip.n発声時刻ms += this.nOFFSET;
                                        }
                                        //chip.n発声時刻ms += this.nDELAY;
                                        //chip.dbBPM = this.dbNowBPM;
                                        //chip.dbSCROLL = this.dbNowSCROLL;

                                        #region[チップ番号を記録]
                                        //風船は現時点では未実装のため処理しない。


                                        //switch (chip.nコース)
                                        //{
                                        //    case 0:
                                        //        if (this.listChip[this.n連打チップ_temp[0]].nチャンネル番号 == 0x99) break;
                                        //        this.listChip[this.n連打チップ_temp[0]].nノーツ終了時刻ms = chip.n発声時刻ms;
                                        //        this.listChip[this.n連打チップ_temp[0]].dbSCROLL = this.dbSCROLL_temp[0];
                                        //        break;
                                        //    case 1:
                                        //        if (this.listChip[this.n連打チップ_temp[1]].nチャンネル番号 == 0x99) break;
                                        //        this.listChip[this.n連打チップ_temp[1]].nノーツ終了時刻ms = chip.n発声時刻ms;
                                        //        this.listChip[this.n連打チップ_temp[1]].dbSCROLL = this.dbSCROLL_temp[1];
                                        //        break;
                                        //    case 2:
                                        //        if (this.listChip[this.n連打チップ_temp[2]].nチャンネル番号 == 0x99) break;
                                        //        this.listChip[this.n連打チップ_temp[2]].nノーツ終了時刻ms = chip.n発声時刻ms;
                                        //        this.listChip[this.n連打チップ_temp[2]].dbSCROLL = this.dbSCROLL_temp[2];
                                        //        break;
                                        //}

                                        #endregion

                                        //this.listChip[this.nNowRoll].nノーツ終了時刻ms = chip.n発声時刻ms;
                                        //this.listChip[this.nNowRoll].dbSCROLL = this.dbNowSCROLL;
                                        //this.listChip[this.nNowRoll].dbBPM = this.dbNowBPM;
                                        continue;
                                    }
                                case 0x9D:
                                    {
										if ( this.listSCROLL.ContainsKey( chip.n整数値_内部番号 ) )
										{
                                            //this.dbNowSCROLL = ( ( this.listSCROLL[ chip.n整数値_内部番号 ].n表記上の番号 == 0 ) ? 0.0 : 1.0 ) + this.listSCROLL[ chip.n整数値_内部番号 ].dbSCROLL値;
										}

                                        //switch (chip.nコース)
                                        //{
                                        //    case 0:
                                        //        this.dbNowSCROLL_Normal = this.dbNowSCROLL;
                                        //        this.n現在のコース = 0;
                                        //        break;
                                        //    case 1:
                                        //        this.dbNowSCROLL_Expert = this.dbNowSCROLL;
                                        //        this.n現在のコース = 1;
                                        //        break;
                                        //    case 2:
                                        //        this.dbNowSCROLL_Master = this.dbNowSCROLL;
                                        //        this.n現在のコース = 2;
                                        //        break;
                                        //}

										continue;
                                    }
                                case 0xDC:
                                    {
                                        if (this.bOFFSETの値がマイナスである)
                                            chip.n発声時刻ms += this.nOFFSET;
                                        if( this.listDELAY.ContainsKey( chip.n整数値_内部番号 ) )
                                        {
                                            //this.nDELAY = ( ( this.listDELAY[ chip.n整数値_内部番号 ].n表記上の番号 == 0 ) ? 0 : 0 ) + this.listDELAY[ chip.n整数値_内部番号 ].nDELAY値;
                                        }
										continue;
                                    }
                                case 0xDE:
                                    {
                                        if (this.bOFFSETの値がマイナスである)
                                            chip.n発声時刻ms += this.nOFFSET;
                                        //chip.n発声時刻ms += this.nDELAY;
                                        //chip.dbBPM = this.dbNowBPM;
                                        //chip.dbSCROLL = this.dbNowSCROLL;
                                        this.b次の小節が分岐である = true;
                                        this.n現在のコース = chip.nコース;
                                        continue;
                                    }
                                case 0xDF:
                                    {
                                        if (this.bOFFSETの値がマイナスである)
                                            chip.n発声時刻ms += this.nOFFSET;
                                        if ( this.listBRANCH.ContainsKey( chip.n整数値_内部番号 ) )
                                        {
                                            //this.listBRANCH[chip.n整数値_内部番号].db分岐時間ms = chip.n発声時刻ms + ( this.bOFFSETの値がマイナスである ? this.nOFFSET : 0 );
                                        }

                                        continue;
                                    }
                                case 0xE0:
                                    {
                                        //if (this.bOFFSETの値がマイナスである)
                                        //    chip.n発声時刻ms += this.nOFFSET;

                                        //chip.dbBPM = this.dbNowBPM;
                                        //chip.dbSCROLL = this.dbNowSCROLL;
                                        //if( chip.n整数値_内部番号 == 1 )
                                        //    this.bBarLine = false;
                                        //else
                                        //    this.bBarLine = true;
                                        continue;
                                    }
								default:
									{
                                        if( this.bOFFSETの値がマイナスである )
                                            chip.n発声時刻ms += this.nOFFSET;
                                        //chip.n発声時刻ms += this.nDELAY;
                                        chip.dbBPM = this.dbNowBPM;
                                        //chip.dbSCROLL = this.dbNowSCROLL;
										continue;
									}
							}
						}
						if ( this.db再生速度 > 0.0 )
						{
							double _db再生速度 = ( CDTXMania.DTXVmode.Enabled ) ? this.dbDTXVPlaySpeed : this.db再生速度;
							foreach ( CChip chip in this.listChip )
							{
								chip.n発声時刻ms = (int) ( ( (double) chip.n発声時刻ms ) / _db再生速度 );
                                chip.db発声時刻ms = (float)( ( (double) chip.n発声時刻ms ) / _db再生速度 );
                                chip.nノーツ終了時刻ms = (int) ( ( (double) chip.nノーツ終了時刻ms ) / _db再生速度 );
							}
						}
                        this.listChip.Sort();
						#endregion
						//span = (TimeSpan) ( DateTime.Now - timeBeginLoad );
						//Trace.TraceInformation( "発声時刻計算:             {0}", span.ToString() );
						//timeBeginLoad = DateTime.Now;
						this.nBGMAdjust = 0;
						this.t各自動再生音チップの再生時刻を変更する( nBGMAdjust );
						//span = (TimeSpan) ( DateTime.Now - timeBeginLoad );
						//Trace.TraceInformation( "再生時刻変更:             {0}", span.ToString() );
						//timeBeginLoad = DateTime.Now;
						#region [ 可視チップ数カウント ]
						for ( int n = 0; n < 14; n++ )
						{
							this.n可視チップ数[ n ] = 0;
						}
						foreach ( CChip chip in this.listChip )
						{
							int c = chip.nチャンネル番号;
                            if( ( 0x11 <= c ) && ( c <= 0x14 ) )
                            {
                                if( c == 0x11 || c == 0x13 )
                                    this.n可視チップ数.Taiko_Red++;
                                else if( c == 0x12 || c == 0x14 )
                                    this.n可視チップ数.Taiko_Blue++;
                            }
						}
						#endregion
						//span = (TimeSpan) ( DateTime.Now - timeBeginLoad );
						//Trace.TraceInformation( "可視チップ数カウント      {0}", span.ToString() );
						//timeBeginLoad = DateTime.Now;
						#region [ チップの種類を分類し、対応するフラグを立てる ]
						foreach ( CChip chip in this.listChip )
						{
							if ( ( chip.bWAVを使うチャンネルである && this.listWAV.ContainsKey( chip.n整数値_内部番号 ) ) && !this.listWAV[ chip.n整数値_内部番号 ].listこのWAVを使用するチャンネル番号の集合.Contains( chip.nチャンネル番号 ) )
							{
								this.listWAV[ chip.n整数値_内部番号 ].listこのWAVを使用するチャンネル番号の集合.Add( chip.nチャンネル番号 );

								int c = chip.nチャンネル番号 >> 4;
								switch ( c )
								{
									case 0x01:
										this.listWAV[ chip.n整数値_内部番号 ].bIsDrumsSound = true; break;
									case 0x02:
										this.listWAV[ chip.n整数値_内部番号 ].bIsGuitarSound = true; break;
									case 0x0A:
										this.listWAV[ chip.n整数値_内部番号 ].bIsBassSound = true; break;
									case 0x06:
									case 0x07:
									case 0x08:
									case 0x09:
										this.listWAV[ chip.n整数値_内部番号 ].bIsSESound = true; break;
									case 0x00:
										if ( chip.nチャンネル番号 == 0x01 )
										{
											this.listWAV[ chip.n整数値_内部番号 ].bIsBGMSound = true; break;
										}
										break;
								}
							}
						}
						#endregion
						//span = (TimeSpan) ( DateTime.Now - timeBeginLoad );
						//Trace.TraceInformation( "ch番号集合確認:           {0}", span.ToString() );
                        //timeBeginLoad = DateTime.Now;
                        #region[ seNotes計算 ]
                        if( this.listBRANCH.Count != 0 )
                            this.tSetSenotes_branch();
                        else
                            this.tSetSenotes();

                        #endregion
                        #region [ bLogDTX詳細ログ出力 ]
                        if ( CDTXMania.ConfigIni.bLogDTX詳細ログ出力 )
						{
							foreach ( CWAV cwav in this.listWAV.Values )
							{
								Trace.TraceInformation( cwav.ToString() );
							}
							foreach ( CAVI cavi in this.listAVI.Values )
							{
								Trace.TraceInformation( cavi.ToString() );
							}
							foreach ( CBPM cbpm3 in this.listBPM.Values )
							{
								Trace.TraceInformation( cbpm3.ToString() );
							}
							foreach ( CChip chip in this.listChip )
							{
								Trace.TraceInformation( chip.ToString() );
							}
						}
						#endregion

                        //ソートっぽい
                        //this.listChip.Sort(delegate(CChip pchipA, CChip pchipB) { return pchipA.n発声時刻ms - pchipB.n発声時刻ms; } );
                        //Random ran1 = new Random();
                        //for (int n = 0; n < this.listChip.Count; n++ )
                        //{

                        //    if (CDTXMania.ConfigIni.bHispeedRandom)
                        //    {

                        //        int nRan = ran1.Next(5, 40);
                        //        this.listChip[n].dbSCROLL = nRan / 10.0;
                        //    }
                        //}
                        this.listChip.Sort();
                        // 2020.3.20 kairera0467 WASAPI/ASIO時のみこの後でさらにSortされてたらしく、ここで指定した値がズレていたため廃止。
                        //int n整数値管理 = 0;
                        //foreach( CChip chip in this.listChip )
                        //{
                        //    if( chip.nチャンネル番号 != 0x54 )
                        //        chip.n整数値 = n整数値管理;
                        //    n整数値管理++;
                        //}

                        #region[ ログ ]
                        int nlinecount = 0;
                        if( !CDTXMania.EnumSongs.IsEnumerating )
                        {
                            // 2020.3.20 kairera0467
                            // 譜面分岐の動作確認のため、以下のチャンネルに限定。
                            //  0x11～0x1A 通常ノーツ
                            //  0x50       小節線
                            //  0x9E～0x9F ゴーゴー
                            //  0xDD～0xDF 譜面分岐
                            Trace.TraceInformation("//--------------------------------------------------");
                            Trace.TraceInformation( "//TJAログ(譜面分岐確認用)" );
                            foreach ( CChip chip in this.listChip )
                            {
                                if (chip.nチャンネル番号 < 0x11)
                                    continue;
                                else if (chip.nチャンネル番号 > 0x1A && chip.nチャンネル番号 < 0x50)
                                    continue;
                                else if (chip.nチャンネル番号 > 0x50 && chip.nチャンネル番号 < 0x9E)
                                    continue;
                                else if (chip.nチャンネル番号 > 0x9F && chip.nチャンネル番号 < 0xDD)
                                    continue;
                                else if (chip.nチャンネル番号 > 0xDF)
                                    continue;

                                if (chip.nチャンネル番号 == 0x50 && chip.nコース == 0)
                                    nlinecount++;

                                // 出力内容
                                //  チャンネル番号(hex)、発声時刻ms、発声位置
                                Trace.WriteLine(string.Format("{4:00}| Ch:{0:00} Time:{1:00000}ms {2:00000} COURSE:{3}", chip.nチャンネル番号.ToString("x"), chip.n発声時刻ms, chip.n発声位置, chip.nコース, nlinecount));
                            
                            }
                            Trace.TraceInformation("//--------------------------------------------------");

                        }

                        #endregion
                    }
                }
			}
		}

        private string tコメントを削除する( string input )
        {
            //string strOutput = Regex.Replace( input, @" *//.*", "" ); //2017.01.28 DD コメント前のスペースも削除するように修正
            //コメント以降を削除→頭、末尾についてる空白をトリミング
            string strOutput = Regex.Replace( input, @" *//.*", "" );
            if( this.strファイル名 == "sample2.tja" )
            {
                if( input == "//-------------むずかしい-------------" )
                {

                }

                if( input.StartsWith( " " ) && strOutput.Length > 1 )
                {
                    strOutput = strOutput.Remove( 0, 1 );
                }

            }


            return strOutput;
        }

        private string[] tコマンド行を削除したTJAを返す( string[] input )
        {
            return this.tコマンド行を削除したTJAを返す( input, 0 );
        }

        private string[] tコマンド行を削除したTJAを返す( string[] input, int nMode )
        {
            string strTemp = "";

            for( int n = 0; n < input.Length; n++ )
            {
                if( nMode == 0 )
                {
                    if( !string.IsNullOrEmpty( input[ n ] ) && this.CharConvertNote( input[ n ].Substring( 0, 1 ) ) != -1 )
                    {
                        strTemp += ( input[ n ] + "\n" );
                    }
                }
                else if( nMode == 1 )
                {
                    if( !string.IsNullOrEmpty( input[ n ] ) && ( input[ n ].Substring( 0, 1 ) == "#" || this.CharConvertNote( input[ n ].Substring( 0, 1 ) ) != -1 ) )
                    {
                        if( input[ n ].StartsWith( "BALLOON" ) || input[ n ].StartsWith( "BPM" ) )
                        {
                            //A～Fで始まる命令が削除されない不具合の対策
                        }
                        else
                        {
                            strTemp += ( input[ n ] + "\n" );
                        }
                    }
                }
                else if( nMode == 2 )
                {
                    if( !string.IsNullOrEmpty( input[ n ] ) && this.CharConvertNote( input[ n ].Substring( 0, 1 ) ) != -1 )
                    {
                        if( input[ n ].StartsWith( "BALLOON" ) || input[ n ].StartsWith( "BPM" ) )
                        {
                            //A～Fで始まる命令が削除されない不具合の対策
                        }
                        else
                        {
                            strTemp += ( input[ n ] + "\n" );
                        }
                    }
                    else
                    {
                        if( input[ n ].StartsWith( "#BRANCHSTART" ) || input[ n ] == "#N" || input[ n ] == "#E" || input[ n ] == "#M"  )
                        {
                            strTemp += ( input[ n ] + "\n" );
                        }

                    }
                }
            }

            string[] strOutput = strTemp.Split( this.dlmtEnter, StringSplitOptions.None );

            return strOutput;
        }

        private string[] t空のstring配列を詰めたstring配列を返す( string[] input )
        {
            string strTemp = "";

            for( int n = 0; n < input.Length; n++ )
            {
                if( !string.IsNullOrEmpty( input[ n ] ) )
                {
                    strTemp += ( input[ n ] + "\n" );
                }
            }

            string[] strOutput = strTemp.Split( this.dlmtEnter, StringSplitOptions.None );

            return strOutput;
        }

        private string StringArrayToString( string[] input )
        {
            return this.StringArrayToString( input, "" );
        }
        private string StringArrayToString( string[] input, string strデリミタ文字 )
        {
            string strTemp = "";

            for( int n = 0; n < input.Length; n++ )
            {
                strTemp += ( input[ n ] + strデリミタ文字 );
            }

            return strTemp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InputText"></param>
        /// <returns>1小節内の文字数</returns>
        private int t1小節の文字数をカウントする(string InputText)
        {
            return InputText.Length - 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InputText"></param>
        /// <returns>1小節内の文字数</returns>
        private void t1小節の文字数をカウントしてリストに追加する(string InputText)
        {
            int nCount = 0;

            if( InputText.StartsWith( "#BRANCHSTART" ) )
            {
                this.nLineCountTemp = this.n現在の小節数;
                return;
            }
            else if( InputText.StartsWith( "#N" ) )
            {
                this.nLineCountCourseTemp = 0;
                this.n現在の小節数 = this.nLineCountTemp;
                return;
            }
            else if( InputText.StartsWith( "#E" ) )
            {
                this.nLineCountCourseTemp = 1;
                this.n現在の小節数 = this.nLineCountTemp;
                return;
            }
            else if( InputText.StartsWith( "#M" ) )
            {
                this.nLineCountCourseTemp = 2;
                this.n現在の小節数 = this.nLineCountTemp;
                return;
            }


            var line = new CLine();
            line.nコース = this.nLineCountCourseTemp;
            line.n文字数 = InputText.Length - 1;
            line.n小節番号 = this.n現在の小節数;

            this.listLine.Add( line );

            this.n現在の小節数++;

        }

        /// <summary>
        /// 0:改行文字を削除して、デリミタとしてスペースを入れる。(返り値:string)
        /// 1:改行文字を削除、さらにSplitして返す(返り値:string[n])
        /// </summary>
        /// <param name="strInput"></param>
        /// <param name="nMode"></param>
        /// <returns></returns>
        private object str改行文字を削除する( string strInput, int nMode )
        {
            string str = "";
            str = strInput.Replace( Environment.NewLine, "\n" );
            str = str.Replace( '\t', ' ' );

            if( nMode == 0 )
            {
                str = str.Replace( "\n", " " );
            }
            else if( nMode == 1 )
            {
                str = str + "\n";

                string[] strArray;
                strArray = str.Split( this.dlmtEnter, StringSplitOptions.RemoveEmptyEntries );

                return strArray;
            }

            return str;
        }

        /// <summary>
        /// コースごとに譜面を分割する。
        /// </summary>
        /// <param name="strTJA"></param>
        /// <returns>各コースの譜面(string[5])</returns>
        private string[] tコースで譜面を分割する( string strTJA )
        {
            string[] strCourseTJA = new string[5];

            if( strTJA.IndexOf("COURSE", 0) != -1 )
            {
                //tja内に「COURSE」があればここを使う。
                string[] strTemp = strTJA.Split( this.dlmtCOURSE, StringSplitOptions.RemoveEmptyEntries );
                
                for( int n = 1; n < strTemp.Length; n++ )
                {
                    int nCourse = 0;
                    string nNC = "";
                    while (strTemp[n].Substring(0, 1) != "\n") //2017.01.29 DD COURSE単語表記に対応
                    {
                        nNC += strTemp[n].Substring(0, 1);
                        strTemp[n] = strTemp[n].Remove(0, 1);
                    }

                    if( this.strConvertCourse( nNC ) != -1 )
                    {
                        nCourse = this.strConvertCourse( nNC );
                        strCourseTJA[ nCourse ] = strTemp[ n ];
                    }
                    else
                    {

                    }
                    //strCourseTJA[ ];

                }
            }
            else
            {
                strCourseTJA[3] = strTJA;
            }

            return strCourseTJA;
        }

        /// <summary>
        /// セッション譜面があるかどうかを判別、あった場合は指定したプレイヤーサイドに従ったrefで切り抜いた譜面を渡す
        /// </summary>
        /// <param name="strTJA"></param>
        /// <param name="strTJA2"></param>
        /// <returns></returns>
        private bool tセッション譜面がある( string strTJA, ref string strTJA2, int seqNo )
        {
            bool bIsSessionNotes = false;
            //入力された譜面がnullでないかチェック。
            if( string.IsNullOrEmpty( strTJA ) ) return false;

            //とりあえず腑分けしてやる
            //一旦STYLEで分ける
            string[] str1 = strTJA.Split( new string[]{"STYLE"}, StringSplitOptions.RemoveEmptyEntries );
            if( str1.Length < 2 ) return false;
            //さらにDPと思われる譜面をSTARTで腑分け
            string[] str2 = str1[ 1 ].Split( new string[]{"#START"}, StringSplitOptions.RemoveEmptyEntries );
            //正常なDP譜面ならLength:2になる。
            string strSingle = "";
            string strDoubleP1 = "";
            string strDoubleP2 = "";

            for( int i = 1; i < str2.Length; i++ )
            {
                //腑分けした時に「#START」が消えてBGMが再生できなくなってしまうので、strDoublePnに代入する時に頭に「#START」をつけておく。
                if( str2[ i ].IndexOf( "P1" ) != -1 )
                {
                    strDoubleP1 = ( "#START" + str2[ i ] );
                    bIsSessionNotes = true;
                }
                else if( str2[ i ].IndexOf( "P2" ) != -1 )
                {
                    strDoubleP2 = ( "#START" + str2[ i ]);
                    bIsSessionNotes = true;
                }
                else
                {
                    if( str1.Length == 2 && str1[ 1 ].IndexOf( "#START" ) != -1 ) {
                        strSingle = str1[ 1 ];
                    } else {
                        strSingle = str1[ 0 ];
                    }
                }
            }
            //strSingle = str1[ 0 ];
            if( !bIsSessionNotes ) seqNo = 99;

            switch( seqNo )
            {
                case 0:
                case 99:
                    strTJA2 = strSingle;
                    break;
                case 1:
                    strTJA2 = strDoubleP1;
                    break;
                case 2:
                    strTJA2 = strDoubleP2;
                    break;
            }

            return bIsSessionNotes;
        }

        /// <summary>
        /// 新型。
        /// ○未実装
        /// _「COURSE」定義が無い譜面は未対応
        /// 　→ver2015082200で対応完了。
        /// 
        /// </summary>
        /// <param name="strInput">譜面のデータ</param>
        private void t入力_V4( string strInput )
        {
            if( !String.IsNullOrEmpty( strInput ) ) //空なら通さない
            {
                //StreamWriter stream = null;
                //bool bLog = true;
                //try
                //{
                //    stream = new StreamWriter("noteTest.txt", false);
                //}
                //catch (Exception ex)
                //{
                //    Trace.TraceError( ex.StackTrace );
                //}

                //2017.01.31 DD カンマのみの行を0,に置き換え
                strInput = Regex.Replace( strInput, @"^,", "0,", RegexOptions.Multiline );

                //2017.02.03 DD ヘッダ内にある命令以外の文字列を削除
                string strInputHeader = strInput.Remove( strInput.IndexOf( "#START" ) );
                strInput = strInput.Remove(0, strInput.IndexOf( "#START" ) );
                strInputHeader = Regex.Replace( strInputHeader,
                    @"^(?!(TITLE|LEVEL|BPM|WAVE|OFFSET|BALLOON|BALLOONNOR|BALLOONEXP|BALLOONMAS|SONGVOL|SEVOL|SCOREINIT|SCOREDIFF|COURSE|STYLE|GAME|LIFE|DEMOSTART|SIDE|SUBTITLE|SCOREMODE|GENRE|MOVIEOFFSET|BGIMAGE|BGMOVIE|HIDDENBRANCH|#HBSCROLL|#BMSCROLL)).+\n",
                "", RegexOptions.Multiline );
                strInput = strInputHeader + "\n" + strInput;

                //どうせ使わないので先にSplitしてコメントを削除。
                this.strSplitした譜面 = (string[])this.str改行文字を削除する( strInput, 1 );
                for (int i = 0; this.strSplitした譜面.Length > i; i++)
                {
                    this.strSplitした譜面[i] = this.tコメントを削除する( this.strSplitした譜面[i] );
                }
                //空のstring配列を詰める
                this.strSplitした譜面 = this.t空のstring配列を詰めたstring配列を返す( this.strSplitした譜面 );

                #region[ヘッダ]

                //2015.05.21 kairera0467
                //ヘッダの読み込みは譜面全体から該当する命令を探す。
                //少し処理が遅くなる可能性はあるが、ここは正確性を重視する。
                //点数などの指定は後から各コースで行うので問題は無いだろう。

                //SplitしたヘッダのLengthの回数だけ、forで回して各種情報を読み取っていく。
                for (int i = 0; this.strSplitした譜面.Length > i; i++)
                {
                    this.t入力_行解析ヘッダ( this.strSplitした譜面[i] );
                }
                #endregion

                #region[譜面]

                int n読み込むコース = 3;
                int n読み込む譜面 = CDTXMania.ConfigIni.nPlayerCount; // 1ならSingle、2ならDouble、Coupleを読むようにする
                int n譜面数 = 0; //2017.07.22 kairera0467 tjaに含まれる譜面の数


                bool b新処理 = false;

                //まずはコースごとに譜面を分割。
                this.strSplitした譜面 = this.tコースで譜面を分割する( this.StringArrayToString( this.strSplitした譜面, "\n" ) );
                string strTest = "";
                //存在するかのフラグ作成。
                for( int i = 0; i < this.strSplitした譜面.Length; i++ )
                {
                    if( !String.IsNullOrEmpty( this.strSplitした譜面[ i ] ) )
                    {
                        this.b譜面が存在する[ i ] = true;
                        n譜面数++;
                    }
                    else
                        this.b譜面が存在する[ i ] = false;
                }
                #region[ 読み込ませるコースを決定 ]
                if( this.b譜面が存在する[CDTXMania.stage選曲.n確定された曲の難易度] == false )
                {
                    n読み込むコース = CDTXMania.stage選曲.n確定された曲の難易度;
                    n読み込むコース++;
                    for (int n = 1; n < 5; n++)
                    {
                        if (this.b譜面が存在する[n読み込むコース] == false)
                        {
                            n読み込むコース++;
                            if (n読み込むコース > 4)
                                n読み込むコース = 0;
                        }
                        else
                            break;
                    }
                }
                else
                    n読み込むコース = CDTXMania.stage選曲.n確定された曲の難易度;
                #endregion

                //指定したコースの譜面の命令を消去する。
                this.tセッション譜面がある( this.strSplitした譜面[ n読み込むコース ], ref this.strSplitした譜面[ n読み込むコース ], CDTXMania.ConfigIni.nPlayerCount > 1 ? ( this.nPlayerSide + 1 ) : 0 );
                this.str命令消去譜面 = this.strSplitした譜面[ n読み込むコース ].Split( this.dlmtEnter, StringSplitOptions.RemoveEmptyEntries );
                //if( bLog && stream != null )
                //{
                //    stream.WriteLine( "-------------------------------------------------" );
                //    stream.WriteLine( ">>this.str命令消去譜面(コマンド削除前)" );
                //    for( int i = 0; i < this.str命令消去譜面.Length; i++ )
                //    {
                //        stream.WriteLine( this.str命令消去譜面[ i ] );
                //    }
                //    stream.WriteLine( "-------------------------------------------------" );
                //}
                this.str命令消去譜面 = this.tコマンド行を削除したTJAを返す( this.str命令消去譜面, 2 );

                //if( bLog && stream != null )
                //{
                //    stream.WriteLine( "-------------------------------------------------" );
                //    stream.WriteLine( ">>this.str命令消去譜面" );
                //    for( int i = 0; i < this.str命令消去譜面.Length; i++ )
                //    {
                //        stream.WriteLine( this.str命令消去譜面[ i ] );
                //    }
                //    stream.WriteLine( "-------------------------------------------------" );
                //}


                //ここで1行の文字数をカウント。配列にして返す。
                string str = "";
                try
                {
                    if( n譜面数 > 1 )
                    {
                        //2017.07.22 kairera0467 譜面が2つ以上ある場合はCOURSE以下のBALLOON命令を使う
                        this.listBalloon.Clear();
                        this.listBalloon_Normal.Clear();
                        this.listBalloon_Expert.Clear();
                        this.listBalloon_Master.Clear();
                        this.listBalloon_Normal_数値管理 = 0;
                        this.listBalloon_Expert_数値管理 = 0;
                        this.listBalloon_Master_数値管理 = 0;
                    }

                    for( int i = 0; i < this.strSplitした譜面[ n読み込むコース ].Split( this.dlmtEnter, StringSplitOptions.RemoveEmptyEntries ).Length; i++ )
                    {
                        if( !String.IsNullOrEmpty( this.strSplitした譜面[ n読み込むコース ].Split( this.dlmtEnter, StringSplitOptions.RemoveEmptyEntries )[ i ] ) )
                        {
                            this.t難易度別ヘッダ( this.strSplitした譜面[ n読み込むコース ].Split( this.dlmtEnter, StringSplitOptions.RemoveEmptyEntries )[ i ] );
                        }
                    }
                    for( int i = 0; i < this.str命令消去譜面.Length; i++ )
                    {
                        if( this.str命令消去譜面[ i ].IndexOf( ',', 0 ) == -1 && !String.IsNullOrEmpty( this.str命令消去譜面[ i ] ) )
                        {
                            if(  this.str命令消去譜面[ i ].Substring( 0, 1 ) == "#" )
                            {
                                this.t1小節の文字数をカウントしてリストに追加する( str + this.str命令消去譜面[ i ] );
                            }

                            if( this.CharConvertNote( this.str命令消去譜面[ i ].Substring( 0, 1 ) ) != -1 )
                                str += this.str命令消去譜面[ i ];
                        }
                        else
                        {
                            this.t1小節の文字数をカウントしてリストに追加する( str + this.str命令消去譜面[ i ] );
                            str = "";
                        }
                    }
                }
                catch( Exception ex )
                {
                    Trace.TraceError( ex.StackTrace );
                }

                //if( bLog && stream != null )
                //{
                //    stream.WriteLine( "-------------------------------------------------" );
                //    stream.WriteLine( ">>this.str命令消去譜面 (命令消去した後)" );
                //    for( int i = 0; i < this.str命令消去譜面.Length; i++ )
                //    {
                //        stream.WriteLine( this.str命令消去譜面[ i ] );
                //    }
                //    stream.WriteLine( "-------------------------------------------------" );
                //}

                //読み込み部分本体に渡す譜面を作成。
                this.strSplitした後の譜面 = this.strSplitした譜面[ n読み込むコース ].Split( this.dlmtEnter, StringSplitOptions.RemoveEmptyEntries );
                this.strSplitした後の譜面 = this.tコマンド行を削除したTJAを返す( this.strSplitした後の譜面, 1 );
                string str命令消去譜面temp = this.StringArrayToString( this.str命令消去譜面 );
                //string[] strDelimiter = { "," };
                //this.str命令消去譜面 = str命令消去譜面temp.Split( strDelimiter, StringSplitOptions.RemoveEmptyEntries );

                //if( bLog && stream != null )
                //{
                //    stream.WriteLine( "-------------------------------------------------" );
                //    stream.WriteLine( ">>this.str命令消去譜面 (Splitした後)" );
                //    for( int i = 0; i < this.str命令消去譜面.Length; i++ )
                //    {
                //        stream.WriteLine( this.str命令消去譜面[ i ] );
                //    }
                //    stream.WriteLine( "-------------------------------------------------" );
                //}

                this.n現在の小節数 = 1;
                try
                {
                    #region[ 最初の処理 ]
                    //1小節の時間を挿入して開始時間を調節。
                    this.dbNowTime += ((15000.0 / 120.0 * ( 4.0 / 4.0 )) * 16.0 );
                    //this.dbNowBMScollTime += (( this.dbBarLength ) * 16.0 );
                    #endregion
                    //string strWrite = "";
                    for( int i = 0; this.strSplitした後の譜面.Length > i; i++ )
                    {
                        str = this.strSplitした後の譜面[ i ];
                        //strWrite += str;
                        //if( !str.StartsWith( "#" ) && !string.IsNullOrEmpty( this.strTemp ) )
                        //{
                        //    str = this.strTemp + str;
                        //}

                        this.t入力_行解析譜面_V4( str );
                    }
                }
                catch( Exception ex )
                {
                    Trace.TraceError( ex.StackTrace );
                }
                //if( stream != null )
                //{
                //    stream.Flush();
                //    stream.Close();
                //}
                #endregion
            }
        }

        private CChip t発声位置から過去方向で一番近くにある指定チャンネルのチップを返す( int n発声時刻, int nチャンネル番号 )
        {
            //過去方向への検索
            for( int i = this.listChip.Count - 1; i >= 0; i-- )
            {
                if( this.listChip[ i ].nチャンネル番号 == nチャンネル番号 )
                {
                    return this.listChip[ i ];
                }
            }

            return null;
        }

		//現在、以下のような行には対応できていません。
		//_パラメータを持つ命令がある
		//_行の途中に命令がある
        private int t文字数解析( string InputText )
        {
            int n文字数 = 0;

            for( int i = 0; i < InputText.Length; i++ )
            {
                if( this.CharConvertNote( InputText.Substring( i, 1 ) ) != -1 )
                {
                    n文字数++;
                }
            }


            return n文字数;
        }

        /// <summary>
        /// 譜面読み込みメソッドV4で使用。
        /// </summary>
        /// <param name="InputText"></param>
        private void t命令を挿入する(string InputText)
        {
            char[] chDelimiter = new char[] { ' ' };
            string[] strArray = null;

            if (InputText.StartsWith("#START"))
            {
                //#STARTと同時に鳴らすのはどうかと思うけどしゃーなしだな。

                var chip = new CChip
                {
                    nチャンネル番号 = 0x01,
                    n発声位置 = 384,
                    n発声時刻ms = (int)this.dbNowTime,
                    n整数値 = 0x01,
                    n整数値_内部番号 = 1
                };

                // チップを配置。
                this.listChip.Add(chip);

                var chip1 = new CChip
                {
                    nチャンネル番号 = 0x54,
                    dbBPM = this.dbNowBPM,
                    dbSCROLL = this.dbNowScroll,
                    n整数値 = 0x01,
                    n整数値_内部番号 = 1,
                    eAVI種別 = EAVI種別.AVI
                };

                //chip1.n発声位置 = 384;
                //chip1.n発声時刻ms = (int)this.dbNowTime;
                if ( this.nMOVIEOFFSET == 0 )
                    chip1.n発声時刻ms = (int)this.dbNowTime;
                else
                    chip1.n発声時刻ms = (int)this.nMOVIEOFFSET;


                // チップを配置。

                this.listChip.Add(chip1);
            }
            else if (InputText.StartsWith("#END"))
            {
                //ためしに割り込む。
                var chip = new CChip
                {
                    nチャンネル番号 = 0xFF,
                    n発声位置 = ((this.n現在の小節数 + 2) * 384),
                    //n発声時刻ms = (int)( this.dbNowTime + ((15000.0 / this.dbNowBPM * ( 4.0 / 4.0 )) * 16.0) * 2  ),
                    n発声時刻ms = (int)( this.dbNowTime + 1000 ), //2016.07.16 kairera0467 終了時から1秒後に設置するよう変更。
                    n整数値 = 0xFF,
                    n整数値_内部番号 = 1
                };

                // チップを配置。

                this.listChip.Add(chip);

                if (this.bチップがある.Branch)
                {
                    for (int f = 0; f <= 2; f++)
                    {
                        this.nノーツ数[f] = this.nノーツ数[f] + this.nノーツ数[3];
                    }
                }
            }

            else if (InputText.StartsWith("#BPMCHANGE"))
            {
                //strArray = InputText.Split(chDelimiter);
                this.SplitOrder( InputText, out strArray, "#BPMCHANGE" );
                if( InputText.IndexOf( "," ) != -1 )
                    InputText = InputText.Replace( ',', '.' );

                double dbBPM = Convert.ToDouble(strArray[1]);
                this.dbNowBPM = dbBPM;

                this.listBPM.Add(this.n内部番号BPM1to - 1, new CBPM() { n内部番号 = this.n内部番号BPM1to - 1, n表記上の番号 = 0, dbBPM値 = dbBPM, bpm_change_time = this.dbNowTime, bpm_change_bmscroll_time = this.dbNowBMScollTime, bpm_change_course = this.n現在のコース });


                //チップ追加して割り込んでみる。
                var chip = new CChip
                {
                    nチャンネル番号 = 0x08,
                    n発声位置 = ((this.n現在の小節数) * 384),
                    n発声時刻ms = (int)this.dbNowTime,
                    fBMSCROLLTime = (float)this.dbNowBMScollTime,
                    dbBPM = dbBPM,
                    n整数値_内部番号 = this.n内部番号BPM1to - 1
                };

                // チップを配置。

                this.listChip.Add(chip);

                var chip1 = new CChip
                {
                    nチャンネル番号 = 0x9C,
                    n発声位置 = ((this.n現在の小節数) * 384),
                    n発声時刻ms = (int)this.dbNowTime,
                    fBMSCROLLTime = (float)this.dbNowBMScollTime,
                    dbBPM = dbBPM,
                    dbSCROLL = this.dbNowScroll,
                    n整数値_内部番号 = this.n内部番号BPM1to - 1
                };

                // チップを配置。

                this.listChip.Add(chip1);

                this.n内部番号BPM1to++;
            }
            else if (InputText.StartsWith("#SCROLL"))
            {
                //2016.08.13 kairera0467 複素数スクロールもどきのテスト
                if( InputText.IndexOf( 'i' ) != -1 )
                {
                    //iが入っていた場合、複素数スクロールとみなす。

                    //strArray = InputText.Split(chDelimiter);
                    this.SplitOrder( InputText, out strArray, "#SCROLL" );

                    double[] dbComplexNum = new double[ 2 ];
                    this.tParsedComplexNumber( strArray[ 1 ], ref dbComplexNum );

                    this.dbNowScroll = dbComplexNum[ 0 ];
                    this.dbNowScrollY = dbComplexNum[ 1 ];

                    this.listSCROLL.Add(this.n内部番号SCROLL1to, new CSCROLL() { n内部番号 = this.n内部番号SCROLL1to, n表記上の番号 = 0, dbSCROLL値 = dbComplexNum[ 0 ], dbSCROLL値Y = dbComplexNum[ 1 ] });

                    switch (this.n現在のコース)
                    {
                        case 0:
                            this.dbNowSCROLL_Normal[ 0 ] = dbComplexNum[ 0 ];
                            this.dbNowSCROLL_Normal[ 1 ] = dbComplexNum[ 1 ];
                            break;
                        case 1:
                            this.dbNowSCROLL_Expert[ 0 ] = dbComplexNum[ 0 ];
                            this.dbNowSCROLL_Expert[ 1 ] = dbComplexNum[ 1 ];
                            break;
                        case 2:
                            this.dbNowSCROLL_Master[ 0 ] = dbComplexNum[ 0 ];
                            this.dbNowSCROLL_Master[ 1 ] = dbComplexNum[ 1 ];
                            break;
                    }

                    //チップ追加して割り込んでみる。
                    var chip = new CChip
                    {
                        nチャンネル番号 = 0x9D,
                        n発声位置 = ((this.n現在の小節数) * 384) - 1,
                        n発声時刻ms = (int)this.dbNowTime,
                        n整数値_内部番号 = this.n内部番号SCROLL1to,
                        dbSCROLL = dbComplexNum[0],
                        dbSCROLL_Y = dbComplexNum[1],
                        nコース = this.n現在のコース
                    };

                    // チップを配置。

                    this.listChip.Add(chip);
                }
                else
                {
                    strArray = InputText.Split(chDelimiter);
                    if( InputText.IndexOf( "," ) != -1 )
                        InputText = InputText.Replace( ',', '.' );
                    double dbSCROLL = Convert.ToDouble(strArray[1]);
                    this.dbNowScroll = dbSCROLL;
                    this.dbNowScrollY = 0.0;

                    this.listSCROLL.Add(this.n内部番号SCROLL1to, new CSCROLL() { n内部番号 = this.n内部番号SCROLL1to, n表記上の番号 = 0, dbSCROLL値 = dbSCROLL, dbSCROLL値Y = 0.0 });

                    switch (this.n現在のコース)
                    {
                        case 0:
                            this.dbNowSCROLL_Normal[ 0 ] = dbSCROLL;
                            break;
                        case 1:
                            this.dbNowSCROLL_Expert[ 0 ] = dbSCROLL;
                            break;
                        case 2:
                            this.dbNowSCROLL_Master[ 0 ] = dbSCROLL;
                            break;
                    }

                    //チップ追加して割り込んでみる。
                    var chip = new CChip
                    {
                        nチャンネル番号 = 0x9D,
                        n発声位置 = ((this.n現在の小節数) * 384) - 1,
                        n発声時刻ms = (int)this.dbNowTime,
                        n整数値_内部番号 = this.n内部番号SCROLL1to,
                        dbSCROLL = dbSCROLL,
                        dbSCROLL_Y = 0.0,
                        nコース = this.n現在のコース
                    };

                    // チップを配置。

                    this.listChip.Add(chip);
                }

                this.n内部番号SCROLL1to++;
            }
            else if (InputText.StartsWith("#MEASURE"))
            {
                //strArray = InputText.Split(chDelimiter);
                this.SplitOrder( InputText, out strArray, "#MEASURE" );
                strArray = strArray[1].Split(new char[] { '/' });

                double[] dbLength = new double[2];
                dbLength[0] = Convert.ToDouble(strArray[0]);
                dbLength[1] = Convert.ToDouble(strArray[1]);

                double db小節長倍率 = dbLength[0] / dbLength[1];
                this.dbBarLength = db小節長倍率;
                this.fNow_Measure_m = (float)dbLength[1];
                this.fNow_Measure_s = (float)dbLength[0];

                var chip = new CChip
                {
                    nチャンネル番号 = 0x02,
                    n発声位置 = ((this.n現在の小節数) * 384),
                    n発声時刻ms = (int)this.dbNowTime,
                    dbSCROLL = this.dbNowScroll,
                    db実数値 = db小節長倍率,
                    n整数値_内部番号 = 1
                };
                // チップを配置。

                this.listChip.Add(chip);

                //lbMaster.Items.Add( ";拍子変更 " + strArray[0] + "/" + strArray[1] );
            }
            else if( InputText.StartsWith( "#DELAY" ) )
            {
                //strArray = InputText.Split( chDelimiter );
                this.SplitOrder( InputText, out strArray, "#DELAY" );
                float nDELAY = (float)( Convert.ToDouble( strArray[ 1 ] ) * 1000.0 );


                this.listDELAY.Add( this.n内部番号DELAY1to, new CDELAY() { n内部番号 = this.n内部番号DELAY1to, n表記上の番号 = 0, nDELAY値 = (int)nDELAY, delay_bmscroll_time = this.dbLastBMScrollTime, delay_bpm = this.dbNowBPM, delay_course = this.n現在のコース, delay_time = this.dbLastTime, delay_delay = Convert.ToDouble(strArray[1]) });


                //チップ追加して割り込んでみる。
                var chip = new CChip
                {
                    nチャンネル番号 = 0xDC,
                    n発声位置 = ((this.n現在の小節数) * 384),
                    n発声時刻ms = (int)this.dbNowTime,
                    db発声時刻ms = this.dbNowTime,
                    n整数値_内部番号 = this.n内部番号DELAY1to,
                    fBMSCROLLTime = this.dbNowBMScollTime,
                    nコース = this.n現在のコース
                };

                // チップを配置。

                this.dbNowTime += nDELAY;
                this.dbNowBMScollTime += nDELAY * this.dbNowBPM / 15000.0;

                this.listChip.Add( chip );
                this.n内部番号DELAY1to++;
            }

            else if (InputText.StartsWith("#GOGOSTART"))
            {
                var chip = new CChip
                {
                    nチャンネル番号 = 0x9E,
                    n発声位置 = ((this.n現在の小節数) * 384),
                    dbBPM = this.dbNowBPM,
                    n発声時刻ms = (int)this.dbNowTime,
                    n整数値_内部番号 = 1
                };
                this.bGOGOTIME = true;

                // チップを配置。
                this.listChip.Add(chip);
            }
            else if (InputText.StartsWith("#GOGOEND"))
            {
                var chip = new CChip
                {
                    nチャンネル番号 = 0x9F,
                    n発声位置 = ((this.n現在の小節数) * 384),
                    n発声時刻ms = (int)this.dbNowTime,
                    dbBPM = this.dbNowBPM,
                    n整数値_内部番号 = 1
                };
                this.bGOGOTIME = false;

                // チップを配置。
                this.listChip.Add(chip);
            }
            else if (InputText.StartsWith("#SECTION"))
            {
                //分岐:条件リセット
                var chip = new CChip
                {
                    nチャンネル番号 = 0xDD,
                    n発声位置 = ((this.n現在の小節数 - 1) * 384),
                    n発声時刻ms = (int)this.dbNowTime,
                    db発声時刻ms = this.dbNowTime,
                    n整数値_内部番号 = 1
                };

                // チップを配置。
                this.listChip.Add(chip);
            }
            else if (InputText.StartsWith("#BRANCHSTART"))
            {
                this.bチップがある.Branch = true;
                this.b最初の分岐である = false;

                //分岐:分岐スタート
                int n条件 = 0;
                //strArray = InputText.Split(chDelimiter);
                this.SplitOrder( InputText, out strArray, "#BRANCHSTART" );
                strArray = strArray[1].Split(',');

                //条件数値。めちゃくちゃ無理やりな実装でスマン。
                double[] nNum = new double[2];
                string strNumA;
                string strNumB;

                if (strArray.Length == 3)
                {
                    strNumA = strArray[1];
                    strNumB = strArray[2];

                    nNum[0] = Convert.ToDouble(strNumA);
                    nNum[1] = Convert.ToDouble(strNumB);
                    switch (strArray[0])
                    {
                        case "p":
                            n条件 = 0;
                            break;
                        case "r":
                            n条件 = 1;
                            break;
                        case "s":
                            n条件 = 2;
                            break;
                        case "d":
                            n条件 = 3;
                            break;
                        default:
                            n条件 = 0;
                            break;
                    }
                }

                if (strArray.Length == 2)
                {
                    strArray = InputText.Split(chDelimiter);
                    strNumA = strArray[2].Split(',')[0];
                    strNumB = strArray[3].Split(',')[0];

                    nNum[0] = Convert.ToDouble(strNumA);
                    nNum[1] = Convert.ToDouble(strNumB);
                    switch (strArray[1])
                    {
                        case "p,":
                            n条件 = 0;
                            break;
                        case "r,":
                            n条件 = 1;
                            break;
                        case "s,":
                            n条件 = 2;
                            break;
                        case "d,":
                            n条件 = 3;
                            break;
                        default:
                            n条件 = 0;
                            break;
                    }
                }

                // 2018.05.22 kairera0467 開始時の拍子情報を保存しておく
                this.fMeasure_BranchStart_m = this.fNow_Measure_m;
                this.fMeasure_BranchStart_s = this.fNow_Measure_s;

                this.bBranch中である = true; // 2018.08.22 kairera0467 譜面分岐中かのフラグ

                //まずはリストに現在の小節、発声位置、分岐条件を追加。
                var branch = new CBRANCH
                {
                    db判定時間 = this.dbNowTime,
                    db分岐時間 = ((this.n現在の小節数 + 1) * 384),
                    db分岐時間ms = this.dbNowTime, //ここがうまく計算できてないので後からバグが出る。
                    //db分岐時間ms = this.dbNowTime + ((((60.0 / this.dbNowBPM) / 4.0 ) * 16.0) * 1000.0);
                    dbBPM = this.dbNowBPM,
                    dbSCROLL = this.dbNowScroll,
                    dbBMScrollTime = this.dbNowBMScollTime,
                    n現在の小節 = this.n現在の小節数,
                    n条件数値A = nNum[0],
                    n条件数値B = nNum[1],
                    n内部番号 = this.n内部番号BRANCH1to,
                    n表記上の番号 = 0,
                    n分岐の種類 = n条件,
                    n命令時のChipList番号 = this.listChip.Count,
                    fMeasure_m = this.fNow_Measure_m,
                    fMeasure_s = this.fNow_Measure_s,
                    bGOGOTIME = this.bGOGOTIME
                };

                this.listBRANCH.Add(this.n内部番号BRANCH1to, branch);


                //分岐アニメ開始時(分岐の1小節前)に設置。
                var chip = new CChip
                {
                    nチャンネル番号 = 0xDE,
                    n発声位置 = ((this.n現在の小節数 - 1) * 384),
                    n発声時刻ms = (int)(this.dbNowTime - ((15000.0 / this.dbNowBPM * (this.fNow_Measure_s / this.fNow_Measure_m)) * 16.0)), //ここの時間設定は前の小節の開始時刻である必要があるのだが...
                    db発声時刻ms = this.dbNowTime,
                    //n発声時刻ms = (int)this.dbLastTime;
                    dbSCROLL = this.dbNowScroll,
                    dbBPM = this.dbNowBPM,
                    n整数値_内部番号 = this.n内部番号BRANCH1to
                };

                // チップを配置。
                this.listChip.Add(chip);

                //実質的な位置に配置
                var chip2 = new CChip
                {
                    nチャンネル番号 = 0xDF,
                    n発声位置 = ((this.n現在の小節数) * 384),
                    db発声時刻ms = this.dbNowTime,
                    n発声時刻ms = (int)this.dbNowTime,
                    dbSCROLL = this.dbNowScroll,
                    dbBPM = this.dbNowBPM,

                    n整数値_内部番号 = this.n内部番号BRANCH1to
                };

                this.listChip.Add(chip2);

                this.n内部番号BRANCH1to++;
            }
            else if (InputText.StartsWith("#N"))
            {
                //分岐:普通譜面
                this.n現在のコース = 0;
                this.n現在の小節数 = this.listBRANCH[this.n内部番号BRANCH1to - 1].n現在の小節;
                this.dbNowTime = this.listBRANCH[this.n内部番号BRANCH1to - 1].db分岐時間ms;
                this.dbNowBPM = this.listBRANCH[this.n内部番号BRANCH1to - 1].dbBPM;
                this.dbNowScroll = this.listBRANCH[this.n内部番号BRANCH1to - 1].dbSCROLL;
                this.dbNowBMScollTime = this.listBRANCH[ this.n内部番号BRANCH1to - 1 ].dbBMScrollTime;

                this.bGOGOTIME = this.listBRANCH[ this.n内部番号BRANCH1to - 1 ].bGOGOTIME;
                this.fNow_Measure_m = this.listBRANCH[ this.n内部番号BRANCH1to - 1 ].fMeasure_m;
                this.fNow_Measure_s = this.listBRANCH[ this.n内部番号BRANCH1to - 1 ].fMeasure_s;
            }
            else if (InputText.StartsWith("#E"))
            {
                //分岐:玄人譜面
                this.n現在のコース = 1;
                this.n現在の小節数 = this.listBRANCH[this.n内部番号BRANCH1to - 1].n現在の小節;
                this.dbNowTime = this.listBRANCH[this.n内部番号BRANCH1to - 1].db分岐時間ms;
                this.dbNowBPM = this.listBRANCH[this.n内部番号BRANCH1to - 1].dbBPM;
                this.dbNowScroll = this.listBRANCH[this.n内部番号BRANCH1to - 1].dbSCROLL;
                this.dbNowBMScollTime = this.listBRANCH[ this.n内部番号BRANCH1to - 1 ].dbBMScrollTime;

                this.bGOGOTIME = this.listBRANCH[ this.n内部番号BRANCH1to - 1 ].bGOGOTIME;
                this.fNow_Measure_m = this.listBRANCH[ this.n内部番号BRANCH1to - 1 ].fMeasure_m;
                this.fNow_Measure_s = this.listBRANCH[ this.n内部番号BRANCH1to - 1 ].fMeasure_s;
            }
            else if (InputText.StartsWith("#M"))
            {
                //分岐:達人譜面
                this.n現在のコース = 2;
                this.n現在の小節数 = this.listBRANCH[this.n内部番号BRANCH1to - 1].n現在の小節;
                this.dbNowTime = this.listBRANCH[this.n内部番号BRANCH1to - 1].db分岐時間ms;
                this.dbNowBPM = this.listBRANCH[this.n内部番号BRANCH1to - 1].dbBPM;
                this.dbNowScroll = this.listBRANCH[this.n内部番号BRANCH1to - 1].dbSCROLL;
                this.dbNowBMScollTime = this.listBRANCH[ this.n内部番号BRANCH1to - 1 ].dbBMScrollTime;

                this.bGOGOTIME = this.listBRANCH[ this.n内部番号BRANCH1to - 1 ].bGOGOTIME;
                this.fNow_Measure_m = this.listBRANCH[ this.n内部番号BRANCH1to - 1 ].fMeasure_m;
                this.fNow_Measure_s = this.listBRANCH[ this.n内部番号BRANCH1to - 1 ].fMeasure_s;
            }
            else if( InputText.StartsWith( "#LEVELHOLD" ) )
            {
                var chip = new CChip
                {
                    nチャンネル番号 = 0xE1,
                    n発声位置 = ((this.n現在の小節数) * 384) - 1,
                    nコース = this.n現在のコース,
                    n発声時刻ms = (int)this.dbNowTime,
                    n整数値_内部番号 = 1
                };

                this.listChip.Add(chip);
            }
            else if( InputText.StartsWith( "#BRANCHEND" ) )
            {
                // 2018.08.22 kairera0467 譜面分岐終了後は現在のコースを普通譜面に戻し、他のコースには配置しない。
                this.bBranch中である = false;
                this.n現在のコース = 0;
            }
            else if( InputText.StartsWith( "#BARLINEOFF" ) )
            {
                var chip = new CChip
                {
                    nチャンネル番号 = 0xE0,
                    n発声位置 = ((this.n現在の小節数) * 384) - 1,
                    n発声時刻ms = (int)this.dbNowTime + 1,
                    n整数値_内部番号 = 1,
                    nコース = this.n現在のコース
                };
                this.bBARLINECUE[ 0 ] = 1;

                this.listChip.Add(chip);
            }
            else if( InputText.StartsWith( "#BARLINEON" ) )
            {
                var chip = new CChip
                {
                    nチャンネル番号 = 0xE0,
                    n発声位置 = ((this.n現在の小節数) * 384) - 1,
                    n発声時刻ms = (int)this.dbNowTime + 1,
                    n整数値_内部番号 = 2,
                    nコース = this.n現在のコース
                };
                this.bBARLINECUE[ 0 ] = 0;

                this.listChip.Add(chip);
            }
            else if( InputText.StartsWith("#LYRIC") )
            {
                strArray = InputText.Split(chDelimiter);

                this.listLiryc.Add(strArray[1]);

                var chip = new CChip
                {
                    nチャンネル番号 = 0xF1,
                    n発声時刻ms = (int)this.dbNowTime,
                    n整数値_内部番号 = 0,
                    nコース = this.n現在のコース,
                    nPlayerSide = this.nPlayerSide  // Issue#11 DP時に歌詞が2つ進んでしまうのを修正
                };

                // チップを配置。

                this.listChip.Add(chip);
            }
            else if( InputText.StartsWith( "#DIRECTION" ) )
            {
                strArray = InputText.Split(chDelimiter);
                double dbSCROLL = Convert.ToDouble(strArray[1]);
                this.nスクロール方向 = (int)dbSCROLL;

                //チップ追加して割り込んでみる。
                var chip = new CChip
                {
                    nチャンネル番号 = 0xF2,
                    n発声位置 = ((this.n現在の小節数) * 384) - 1,
                    n発声時刻ms = (int)this.dbNowTime,
                    n整数値_内部番号 = 0,
                    nスクロール方向 = (int)dbSCROLL,
                    nコース = this.n現在のコース
                };

                // チップを配置。

                this.listChip.Add(chip);
            }
            else if( InputText.StartsWith( "#SUDDEN" ) )
            {
                strArray = InputText.Split(chDelimiter);
                double db出現時刻 = Convert.ToDouble(strArray[1]);
                double db移動待機時刻 = Convert.ToDouble(strArray[2]);
                this.db出現時刻 = db出現時刻;
                this.db移動待機時刻 = db移動待機時刻;

                //チップ追加して割り込んでみる。
                var chip = new CChip
                {
                    nチャンネル番号 = 0xF3,
                    n発声位置 = ((this.n現在の小節数) * 384) - 1,
                    n発声時刻ms = (int)this.dbNowTime,
                    n整数値_内部番号 = 0,
                    nノーツ出現時刻ms = (int)this.db出現時刻,
                    nノーツ移動開始時刻ms = (int)this.db移動待機時刻,
                    nコース = this.n現在のコース
                };

                // チップを配置。

                this.listChip.Add(chip);
            }
            else if( InputText.StartsWith( "#JPOSSCROLL" ) )
            {
                strArray = InputText.Split(chDelimiter);
                double db移動時刻 = Convert.ToDouble(strArray[1]);
                int n移動px = Convert.ToInt32(strArray[2]);
                int n移動方向 = Convert.ToInt32(strArray[3]);

                //チップ追加して割り込んでみる。
                var chip = new CChip
                {
                    nチャンネル番号 = 0xE2,
                    n発声位置 = ((this.n現在の小節数) * 384) - 1,
                    n発声時刻ms = (int)this.dbNowTime,
                    n整数値_内部番号 = 0,
                    nコース = this.n現在のコース
                };

                // チップを配置。

                this.listJPOSSCROLL.Add( this.n内部番号JSCROLL1to, new CJPOSSCROLL() { n内部番号 = this.n内部番号JSCROLL1to, n表記上の番号 = 0, db移動時間 = db移動時刻, n移動距離px = n移動px, n移動方向 = n移動方向 });
                this.listChip.Add( chip );
                this.n内部番号JSCROLL1to++;
            }

        }

        private void t入力_行解析譜面_V4(string InputText)
        {
            if( !String.IsNullOrEmpty( InputText ) )
            {
                int n文字数 = 16;
                
                //現在のコース、小節に当てはまるものをリストから探して文字数を返す。
                for( int i = 0; i < this.listLine.Count; i++ )
                {
                    if( this.listLine[ i ].n小節番号 == this.n現在の小節数 && this.listLine[ i ].nコース == this.n現在のコース )
                    {
                        n文字数 = this.listLine[ i ].n文字数;
                    }

                }

                if( InputText.Substring(0, 1) == "#" )
                {
                    this.t命令を挿入する(InputText);
                    return;
                }
                else
                {
                    if( this.b小節線を挿入している == false )
                    {
                        CChip chip = new CChip
                        {
                            bBranch = this.bBranch中である,
                            nチャンネル番号 = 0x50,
                            n発声位置 = ((this.n現在の小節数) * 384),
                            n発声時刻ms = (int)this.dbNowTime,
                            db発声時刻ms = this.dbNowTime,
                            n整数値_内部番号 = this.n現在の小節数,
                            dbBPM = this.dbNowBPM,
                            dbSCROLL = this.dbNowScroll,
                            dbSCROLL_Y = this.dbNowScrollY,
                            fBMSCROLLTime = (float)this.dbNowBMScollTime,
                            nコース = this.n現在のコース
                        };

                        if ( this.bBARLINECUE[ 0 ] == 1 )
                        {
                            chip.b可視 = false;
                        }


                        if( this.listBRANCH.Count != 0 )
                        {
                            if (this.listBRANCH[ this.n内部番号BRANCH1to - 1 ].n現在の小節 == this.n現在の小節数)
                            {
                                chip.bBranchLine = true;
                            }
                        }
                        this.listChip.Add(chip);

                        this.dbLastTime = this.dbNowTime;
                        this.b小節線を挿入している = true;

                        #region[ 拍線チップテスト ]
                        //1拍の時間を計算
                        double db1拍 = ( 60.0 / this.dbNowBPM ) / 4.0;
                        //forループ(拍数)
                        for( int measure = 1; measure < this.fNow_Measure_s; measure++ )
                        {
                            CChip hakusen = new CChip
                            {
                                n発声位置 = ((this.n現在の小節数) * 384),
                                n発声時刻ms = (int)(this.dbNowTime + (((db1拍 * 4.0)) * measure) * 1000.0),
                                nチャンネル番号 = 0x51,
                                //n発声時刻ms = (int)this.dbNowTime;
                                fBMSCROLLTime = this.dbNowBMScollTime,
                                n整数値_内部番号 = this.n現在の小節数,
                                dbBPM = this.dbNowBPM,
                                dbSCROLL = this.dbNowScroll,
                                dbSCROLL_Y = this.dbNowScrollY,
                                nコース = this.n現在のコース
                            };

                            this.listChip.Add( hakusen );
//--全ての拍線の時間を出力する--
    //Trace.WriteLine( string.Format( "|| {0,3:##0} Time:{1} Beat:{2}", this.n現在の小節数, hakusen.n発声時刻ms, measure ) );
//--------------------------------
                        }

                        #endregion
                    }

                    for (int n = 0; n < InputText.Length; n++)
                    {
                        if (InputText.Substring(n, 1) == ",")
                        {
                            this.n現在の小節数++;
                            this.b小節線を挿入している = false;
                            return;
                        }

                        if( InputText.Substring(0, 1) == "F" )
                        {
                            bool bTest = true;
                        }


                        int nObjectNum = this.CharConvertNote(InputText.Substring(n, 1));

                        if (nObjectNum != 0)
                        {
                            // 連打音符
                            if (( nObjectNum >= 5 && nObjectNum <= 7 ) || nObjectNum == 9 )
                            {
                                if (nNowRoll != 0)
                                {
                                    this.dbNowTime += (15000.0 / this.dbNowBPM * (this.fNow_Measure_s / this.fNow_Measure_m) * (16.0 / n文字数));
                                    this.dbNowBMScollTime += (double)((this.dbBarLength) * (16.0 / n文字数));
                                    continue;
                                }
                                else
                                {
                                    this.nNowRollCount = listChip.Count;
                                    nNowRoll = nObjectNum;
                                }
                            }

                            var chip = new CChip
                            {
                                bBranch = this.bBranch中である,
                                bHit = false,
                                b可視 = true,
                                bShow = true,
                                nチャンネル番号 = 0x10 + nObjectNum,
                                n発声位置 = (int)((this.n現在の小節数 * 384.0) + ((384.0 * n) / n文字数)),
                                db発声位置 = this.dbNowTime,
                                n発声時刻ms = (int)this.dbNowTime,
                                db発声時刻ms = this.dbNowTime,
                                //fBMSCROLLTime = (float)(( this.dbBarLength ) * (16.0f / this.n各小節の文字数[this.n現在の小節数]));
                                fBMSCROLLTime = (float)this.dbNowBMScollTime,
                                n整数値 = nObjectNum,
                                n整数値_内部番号 = 1,
                                dbBPM = this.dbNowBPM,
                                dbSCROLL = this.dbNowScroll,
                                dbSCROLL_Y = this.dbNowScrollY,
                                nスクロール方向 = this.nスクロール方向,
                                nコース = this.n現在のコース,
                                n分岐回数 = this.n内部番号BRANCH1to,
                                e楽器パート = E楽器パート.TAIKO,
                                nノーツ出現時刻ms = (int)(this.db出現時刻 * 1000.0),
                                nノーツ移動開始時刻ms = (int)(this.db移動待機時刻 * 1000.0),
                                nPlayerSide = this.nPlayerSide,
                                bGOGOTIME = this.bGOGOTIME
                            };

                            // 風船連打
                            if ( nObjectNum == 7 || nObjectNum == 9 )
                            {
                                switch( this.n現在のコース )
                                {
                                    case 0:
                                        if( this.listBalloon_Normal.Count == 0  )
                                        {
                                            chip.nBalloon = 5;
                                            break;
                                        }

                                        if( this.listBalloon_Normal.Count > this.listBalloon_Normal_数値管理 )
                                        {
                                            chip.nBalloon = this.listBalloon_Normal[this.listBalloon_Normal_数値管理];
                                            this.listBalloon_Normal_数値管理++;
                                            break;
                                        }
                                        //else if( this.listBalloon.Count != 0 )
                                        //{
                                        //    chip.nBalloon = this.listBalloon[ this.listBalloon_Normal_数値管理 ];
                                        //    this.listBalloon_Normal_数値管理++;
                                        //    break;
                                        //}
                                        break;
                                    case 1:
                                        if (this.listBalloon_Expert.Count == 0)
                                        {
                                            chip.nBalloon = 5;
                                            break;
                                        }

                                        if( this.listBalloon_Expert.Count > this.listBalloon_Expert_数値管理 )
                                        {
                                            chip.nBalloon = this.listBalloon_Expert[this.listBalloon_Expert_数値管理];
                                            this.listBalloon_Expert_数値管理++;
                                            break;
                                        }
                                        //else if( this.listBalloon.Count != 0 )
                                        //{
                                        //    chip.nBalloon = this.listBalloon[ this.listBalloon_Normal_数値管理 ];
                                        //    this.listBalloon_Normal_数値管理++;
                                        //    break;
                                        //}
                                        break;
                                    case 2:
                                        if (this.listBalloon_Master.Count == 0)
                                        {
                                            chip.nBalloon = 5;
                                            break;
                                        }

                                        if( this.listBalloon_Master.Count > this.listBalloon_Master_数値管理 )
                                        {
                                            chip.nBalloon = this.listBalloon_Master[this.listBalloon_Master_数値管理];
                                            this.listBalloon_Master_数値管理++;
                                            break;
                                        }
                                        //else if( this.listBalloon.Count != 0 )
                                        //{
                                        //    chip.nBalloon = this.listBalloon[ this.listBalloon_Normal_数値管理 ];
                                        //    this.listBalloon_Normal_数値管理++;
                                        //    break;
                                        //}
                                        break;
                                }
                            }
                            if (nObjectNum == 8)
                            {
                                chip.nノーツ終了位置 = (this.n現在の小節数 * 384) + ((384 * n) / n文字数);
                                chip.nノーツ終了時刻ms = (int)this.dbNowTime;
                                chip.fBMSCROLLTime_end = (float)this.dbNowBMScollTime;

                                chip.nノーツ出現時刻ms = listChip[ nNowRollCount ].nノーツ出現時刻ms;
                                chip.nノーツ移動開始時刻ms = listChip[ nNowRollCount ].nノーツ移動開始時刻ms;

                                chip.n連打音符State = nNowRoll;
                                listChip[ nNowRollCount ].nノーツ終了位置 = (this.n現在の小節数 * 384) + ((384 * n) / n文字数);
                                listChip[ nNowRollCount ].nノーツ終了時刻ms = (int)this.dbNowTime;
                                listChip[ nNowRollCount ].fBMSCROLLTime_end = (int)this.dbNowBMScollTime;
                                //listChip[ nNowRollCount ].dbBPM = this.dbNowBPM;
                                //listChip[ nNowRollCount ].dbSCROLL = this.dbNowSCROLL;
                                nNowRoll = 0;
                                //continue;
                            }

                            #region[ 固定される種類のsenotesはここで設定しておく。 ]
                                switch ( nObjectNum )
                                {
                                    case 3:
                                        chip.nSenote = 5;
                                        break;
                                    case 4:
                                        chip.nSenote = 6;
                                        break;
                                    case 5:
                                        chip.nSenote = 7;
                                        break;
                                    case 6:
                                        chip.nSenote = 0xA;
                                        break;
                                    case 7:
                                        chip.nSenote = 0xB;
                                        break;
                                    case 8:
                                        chip.nSenote = 0xC;
                                        break;
                                    case 9:
                                        chip.nSenote = 0xD;
                                        break;
                                    case 10:
                                        chip.nSenote = 0xE;
                                        break;
                                    case 11:
                                        chip.nSenote = 0xF;
                                        break;
                                }
                                #endregion

                            if( nObjectNum < 5 )
                            {
                                if( this.b最初の分岐である == false )
                                {
                                    this.nノーツ数[ this.n現在のコース ]++;
                                    this.n現在小節のノート数コース毎[ this.n現在のコース ]++;
                                }
                                else
                                    this.nノーツ数[ 3 ]++;
                            }
                            else if( nObjectNum == 7 )
                            {
                                if( this.b最初の分岐である == false )
                                    this.n風船数[ this.n現在のコース ]++;
                                else
                                    this.n風船数[ 3 ]++;
                            }


                            this.listChip.Add(chip);
                        }

                        this.dbLastTime = this.dbNowTime;
                        this.dbLastBMScrollTime = this.dbNowBMScollTime;
                        this.dbNowTime += ( 15000.0 / this.dbNowBPM * (this.fNow_Measure_s / this.fNow_Measure_m) * ( 16.0 / n文字数 ) );
                        this.dbNowBMScollTime += ( ( ( this.fNow_Measure_s / this.fNow_Measure_m ) ) * ( 16.0 / (double)n文字数 ) );
                    }
                }
            }
        }

        /// <summary>
        /// 難易度ごとによって変わるヘッダ値を読み込む。
        /// (BALLOONなど。)
        /// </summary>
        /// <param name="InputText"></param>
        private void t難易度別ヘッダ( string InputText )
        {
            if( InputText.Equals( "#HBSCROLL" ) && CDTXMania.ConfigIni.bスクロールモードを上書き == false )
            {
                CDTXMania.ConfigIni.eScrollMode = EScrollMode.HBSCROLL;
            }
            if( InputText.Equals( "#BMSCROLL" ) && CDTXMania.ConfigIni.bスクロールモードを上書き == false )
            {
                CDTXMania.ConfigIni.eScrollMode = EScrollMode.BMSCROLL;
            }


            string[] strArray = InputText.Split( new char[] { ':' } );
            string strCommandName = "";
            string strCommandParam = "";

            if( strArray.Length == 2 )
            {
                strCommandName = strArray[0].Trim();
                strCommandParam = strArray[1].Trim();
            }

            if( strCommandName.Equals( "BALLOON" ) )
            {
                string[] strParam = strCommandParam.Split( ',' );
                for( int n = 0; n < strParam.Length; n++ )
                {
                    int n打数;
                    try
                    {
                        if (strParam[n] == null || strParam[n] == "")
                            break;

                        n打数 = Convert.ToInt32( strParam[ n ] );
                    }
                    catch(Exception ex)
                    {
                        Trace.TraceError( "おや?エラーが出たようです。お兄様。" );
                        Trace.TraceError( "エラー:{0}", ex.StackTrace );
                        break;
                    }
                    this.listBalloon_Normal.Add( n打数 );
                }
            }
            else if( strCommandName.Equals( "BALLOONNOR" ) )
            {
                string[] strParam = strCommandParam.Split( ',' );
                for( int n = 0; n < strParam.Length; n++ )
                {
                    int n打数;
                    try
                    {
                        if (strParam[n] == null || strParam[n] == "")
                            break;

                        n打数 = Convert.ToInt32( strParam[ n ] );
                    }
                    catch(Exception ex)
                    {
                        Trace.TraceError( "おや?エラーが出たようです。お兄様。" );
                        Trace.TraceError( "エラー:{0}", ex.StackTrace );
                        break;
                    }
                    this.listBalloon_Normal.Add( n打数 );
                }
            }
            else if( strCommandName.Equals( "BALLOONEXP" ) )
            {
                string[] strParam = strCommandParam.Split( ',' );
                for( int n = 0; n < strParam.Length; n++ )
                {
                    int n打数;
                    try
                    {
                        if (strParam[n] == null || strParam[n] == "")
                            break;

                        n打数 = Convert.ToInt32( strParam[ n ] );
                    }
                    catch(Exception ex)
                    {
                        Trace.TraceError( "おや?エラーが出たようです。お兄様。" );
                        Trace.TraceError( "エラー:{0}", ex.StackTrace );
                        break;
                    }
                    this.listBalloon_Expert.Add( n打数 );
                }
                //tbBALLOON.Text = strCommandParam;
            }
            else if( strCommandName.Equals( "BALLOONMAS" ) )
            {
                string[] strParam = strCommandParam.Split( ',' );
                for( int n = 0; n < strParam.Length; n++ )
                {
                    int n打数;
                    try
                    {
                        if (strParam[n] == null || strParam[n] == "")
                            break;

                        n打数 = Convert.ToInt32( strParam[ n ] );
                    }
                    catch(Exception ex)
                    {
                        Trace.TraceError( "おや?エラーが出たようです。お兄様。" );
                        Trace.TraceError( "エラー:{0}", ex.StackTrace );
                        break;
                    }
                    this.listBalloon_Master.Add( n打数 );
                }
                //tbBALLOON.Text = strCommandParam;
            }
            else if( strCommandName.Equals( "SCOREMODE" ) )
            {
                if( !string.IsNullOrEmpty( strCommandParam ) )
                {
                    this.nScoreModeTmp = Convert.ToInt16( strCommandParam );
                }
            }
            else if( strCommandName.Equals( "SCOREINIT" ) )
            {
                if( !string.IsNullOrEmpty( strCommandParam ) )
                {
                    string[] scoreinit = strCommandParam.Split(',');

                    this.nScoreInit[ 0, this.n参照中の難易度 ] = Convert.ToInt16( scoreinit[ 0 ] );
                    this.b配点が指定されている[ 0, this.n参照中の難易度 ] = true;
                    if( scoreinit.Length == 2 ){
                        this.nScoreInit[ 1, this.n参照中の難易度 ] = Convert.ToInt16( scoreinit[ 1 ] );
                        this.b配点が指定されている[ 2, this.n参照中の難易度 ] = true;
                    }
                }
            }
            else if( strCommandName.Equals( "SCOREDIFF" ) )
            {
                if( !string.IsNullOrEmpty( strCommandParam ) )
                {
                    this.nScoreDiff[ this.n参照中の難易度 ] = Convert.ToInt16( strCommandParam );
                    this.b配点が指定されている[ 1, this.n参照中の難易度 ] = true;
                }
            }

            if( this.nScoreModeTmp == 99 ) //2017.01.28 DD SCOREMODEを入力していない場合のみConfigで設定したモードにする
            {
                this.nScoreModeTmp = CDTXMania.ConfigIni.nScoreMode;
            }
            if( CDTXMania.ConfigIni.nScoreMode == 3 && !this.b配点が指定されている[ 2, this.n参照中の難易度 ] ){ //2017.06.04 kairera0467
                this.nScoreModeTmp = 3;
            }
        }

        private void t入力_行解析ヘッダ( string InputText )
		{
            //やべー。先頭にコメント行あったらやばいやん。
            string[] strArray = InputText.Split( new char[] { ':' } );
            string strCommandName = "";
            string strCommandParam = "";

            if( InputText.StartsWith( "#BRANCHSTART" ) )
            {
                //2015.08.18 kairera0467
                //本来はヘッダ命令ではありませんが、難易度ごとに違う項目なのでここで読み込ませます。
                //Lengthのチェックをされる前ににif文を入れています。
                this.bHasBranch[ this.n参照中の難易度 ] = true;
            }

            //まずは「:」でSplitして割り当てる。
            if( strArray.Length == 2 )
            {
                strCommandName = strArray[0].Trim();
                strCommandParam = strArray[1].Trim();
            }
            else if( strArray.Length > 2 )
            {
                //strArrayが2じゃない場合、ヘッダのSplitを通していない可能性がある。
                //この処理自体は「t入力」を改造したもの。STARTでSplitしていない等、一部の処理が異なる。

                #region[ヘッダ]
                InputText = InputText.Replace( Environment.NewLine, "\n" ); //改行文字を別の文字列に差し替え。
                InputText = InputText.Replace( '\t', ' ' ); //何の文字か知らないけどスペースに差し替え。
                InputText = InputText + "\n";

                string[] strDelimiter = { "#START" };
                strArray = InputText.Split( strDelimiter, StringSplitOptions.RemoveEmptyEntries );

                string[] strDelimiter2 = { "\n" };
                strArray = InputText.Split( strDelimiter2, StringSplitOptions.RemoveEmptyEntries );


                strArray = strArray[ 0 ].Split( new char[] { ':' } );

                strCommandName = strArray[0].Trim();
                strCommandParam = strArray[1].Trim();

                #endregion
                //lblMessage.Text = "おや?strArrayのLengthが2じゃないようですね。お兄様。";
            }

            //パラメータを分別、そこから割り当てていきます。
            // 2018.6.14 kairera0467 デリミタ文字入りのタイトルが正しく読み込めないので「TITLE」「SUBTITLE」での処理を変更
            if( strCommandName.Equals( "TITLE" ) )
            {
                this.TITLE = InputText.Substring( InputText.IndexOf(":") + 1 );
            }
            if( strCommandName.Equals( "SUBTITLE" ) )
            {
                if( strCommandParam.StartsWith("--") )
                {
                    this.SUBTITLE = InputText.Substring( InputText.IndexOf(":") + 3 );
                }
                else if( strCommandParam.StartsWith("++") )
                {
                //    //this.TITLE += strCommandParam.Remove( 0, 2 ); //このままだと選曲画面の表示がうまくいかない。
                    this.SUBTITLE = InputText.Substring( InputText.IndexOf(":") + 3 );
                }
            }
            else if( strCommandName.Equals( "LEVEL" ) )
            {
                this.LEVEL.Drums = (int)Convert.ToDouble( strCommandParam );
                this.LEVEL.Taiko = (int)Convert.ToDouble( strCommandParam );
                this.LEVELtaiko[ this.n参照中の難易度 ] = (int)Convert.ToDouble( strCommandParam );
            }
            else if( strCommandName.Equals( "BPM" ) )
            {
                if( strCommandParam.IndexOf( "," ) != -1 )
                    strCommandParam = strCommandParam.Replace( ',', '.' );

                this.BPM = Convert.ToDouble( strCommandParam );
                this.BASEBPM = Convert.ToDouble( strCommandParam );
                this.dbNowBPM = Convert.ToDouble( strCommandParam );

                double dbBPM = Convert.ToDouble( strCommandParam );

                this.listBPM.Add( this.n内部番号BPM1to - 1, new CBPM() { n内部番号 = this.n内部番号BPM1to - 1, n表記上の番号 = this.n内部番号BPM1to - 1, dbBPM値 = dbBPM, } );
                this.n内部番号BPM1to++;


                //チップ追加して割り込んでみる。
                var chip = new CChip();

				chip.nチャンネル番号 = 0x03;
				chip.n発声位置 = ( ( this.n現在の小節数 - 1 ) * 384 );
				chip.n整数値 = 0x00;
				chip.n整数値_内部番号 = 1;
                    
				this.listChip.Add( chip );
                //tbBPM.Text = strCommandParam;
            }
            else if( strCommandName.Equals( "WAVE" ) )
            {
                this.strBGM_PATH = strCommandParam;
                //tbWave.Text = strCommandParam;
                if( this.listWAV != null )
                {
                    var wav = new CWAV() {
				        n内部番号 = this.n内部番号WAV1to,
				        n表記上の番号 = 1,
        				strファイル名 = this.strBGM_PATH,
    				    strコメント文 = "TJA BGM",
                    };

                    this.listWAV.Add( this.n内部番号WAV1to, wav );
                    this.n内部番号WAV1to++;
                }
            }
            else if( strCommandName.Equals( "OFFSET" ) )
            {
                this.nOFFSET = (int)( Convert.ToDouble( strCommandParam ) * 1000 );
                this.bOFFSETの値がマイナスである = this.nOFFSET < 0 ? true : false;

                this.listBPM[ 0 ].bpm_change_bmscroll_time = -2000 * this.dbNowBPM / 15000;
                if (this.bOFFSETの値がマイナスである == true)
                    this.nOFFSET = this.nOFFSET * -1; //OFFSETは秒を加算するので、必ず正の数にすること。
                //tbOFFSET.Text = strCommandParam;
            }
            else if( strCommandName.Equals( "MOVIEOFFSET" ) )
            {
                this.nMOVIEOFFSET = (int)( Convert.ToDouble( strCommandParam ) * 1000 );
                this.bMOVIEOFFSETの値がマイナスである = this.nMOVIEOFFSET < 0 ? true : false;

                if (this.bMOVIEOFFSETの値がマイナスである == true)
                    this.nMOVIEOFFSET = this.nMOVIEOFFSET * -1; //OFFSETは秒を加算するので、必ず正の数にすること。
                //tbOFFSET.Text = strCommandParam;
            }
            #region[移動→不具合が起こるのでここも一応復活させておく]
            else if( strCommandName.Equals( "BALLOON" ) )
            {
                string[] strParam = strCommandParam.Split( ',' );
                for( int n = 0; n < strParam.Length; n++ )
                {
                    int n打数;
                    try
                    {
                        if (strParam[n] == null || strParam[n] == "")
                            break;

                        n打数 = Convert.ToInt32( strParam[ n ] );
                    }
                    catch(Exception ex)
                    {
                        Trace.TraceError( "おや?エラーが出たようです。お兄様。" );
                        Trace.TraceError( "エラー:{0}", ex.StackTrace );
                        break;
                    }
                    this.listBalloon_Normal.Add( n打数 );
                }
            }
            else if( strCommandName.Equals( "BALLOONNOR" ) )
            {
                string[] strParam = strCommandParam.Split( ',' );
                for( int n = 0; n < strParam.Length; n++ )
                {
                    int n打数;
                    try
                    {
                        if (strParam[n] == null || strParam[n] == "")
                            break;

                        n打数 = Convert.ToInt32( strParam[ n ] );
                    }
                    catch(Exception ex)
                    {
                        Trace.TraceError( "おや?エラーが出たようです。お兄様。" );
                        Trace.TraceError( "エラー:{0}", ex.StackTrace );
                        break;
                    }
                    this.listBalloon_Normal.Add( n打数 );
                }
            }
            else if( strCommandName.Equals( "BALLOONEXP" ) )
            {
                string[] strParam = strCommandParam.Split( ',' );
                for( int n = 0; n < strParam.Length; n++ )
                {
                    int n打数;
                    try
                    {
                        if (strParam[n] == null || strParam[n] == "")
                            break;

                        n打数 = Convert.ToInt32( strParam[ n ] );
                    }
                    catch(Exception ex)
                    {
                        Trace.TraceError( "おや?エラーが出たようです。お兄様。" );
                        Trace.TraceError( "エラー:{0}", ex.StackTrace );
                        break;
                    }
                    this.listBalloon_Expert.Add( n打数 );
                }
                //tbBALLOON.Text = strCommandParam;
            }
            else if( strCommandName.Equals( "BALLOONMAS" ) )
            {
                string[] strParam = strCommandParam.Split( ',' );
                for( int n = 0; n < strParam.Length; n++ )
                {
                    int n打数;
                    try
                    {
                        if (strParam[n] == null || strParam[n] == "")
                            break;

                        n打数 = Convert.ToInt32( strParam[ n ] );
                    }
                    catch(Exception ex)
                    {
                        Trace.TraceError( "おや?エラーが出たようです。お兄様。" );
                        Trace.TraceError( "エラー:{0}", ex.StackTrace );
                        break;
                    }
                    this.listBalloon_Master.Add( n打数 );
                }
                //tbBALLOON.Text = strCommandParam;
            }
            else if( strCommandName.Equals( "SCOREMODE" ) )
            {
                if( !string.IsNullOrEmpty( strCommandParam ) )
                {
                    this.nScoreModeTmp = Convert.ToInt16( strCommandParam );
                }
            }
            else if( strCommandName.Equals( "SCOREINIT" ) )
            {
                if( !string.IsNullOrEmpty( strCommandParam ) )
                {
                    string[] scoreinit = strCommandParam.Split(',');

                    this.nScoreInit[ 0, this.n参照中の難易度 ] = Convert.ToInt16( scoreinit[ 0 ] );
                    if( scoreinit.Length == 2 )
                        this.nScoreInit[ 1, this.n参照中の難易度 ] = Convert.ToInt16( scoreinit[ 1 ] );
                }
            }
            else if( strCommandName.Equals( "SCOREDIFF" ) )
            {
                if( !string.IsNullOrEmpty( strCommandParam ) )
                {
                    this.nScoreDiff[ this.n参照中の難易度 ] = Convert.ToInt16( strCommandParam );
                }
            }
            #endregion
            else if( strCommandName.Equals( "SONGVOL" ) )
            {
                //tbSongVol.Text = strCommandParam;
            }
            else if( strCommandName.Equals( "SEVOL" ) )
            {
                //tbSeVol.Text = strCommandParam;
            }
            else if( strCommandName.Equals( "COURSE" ) )
            {
                if( !string.IsNullOrEmpty( strCommandParam ) )
                {
                    //this.n参照中の難易度 = Convert.ToInt16( strCommandParam );
                    this.n参照中の難易度 = this.strConvertCourse( strCommandParam );
                }
            }

            else if( strCommandName.Equals( "HEADSCROLL" ) )
            {
                //新定義:初期スクロール速度設定(というよりこのシステムに合わせるには必須。)
                //どうしても一番最初に1小節挿入されるから、こうするしかなかったんだ___

                this.dbScrollSpeed = Convert.ToDouble( strCommandParam );

                this.listSCROLL.Add( this.n内部番号SCROLL1to, new CSCROLL() { n内部番号 = this.n内部番号SCROLL1to, n表記上の番号 = 0, dbSCROLL値 = this.dbScrollSpeed, } );


                //チップ追加して割り込んでみる。
                var chip = new CChip();

				chip.nチャンネル番号 = 0x9D;
                chip.n発声位置 = ((this.n現在の小節数 - 2) * 384);
				chip.n整数値 = 0x00;
				chip.n整数値_内部番号 = this.n内部番号SCROLL1to;
                chip.dbSCROLL = this.dbScrollSpeed;

    	        // チップを配置。
                    
				this.listChip.Add( chip );
                this.n内部番号SCROLL1to++;

                //this.nScoreDiff = Convert.ToInt16( strCommandParam );
                //tbScoreDiff.Text = strCommandParam;
            }
            else if( strCommandName.Equals( "GENRE" ) )
            {
                //2015.03.28 kairera0467
                //ジャンルの定義。DTXから入力もできるが、tjaからも入力できるようにする。
                //日本語名だと選曲画面でバグが出るので、そこもどうにかしていく予定。

                if( !string.IsNullOrEmpty( strCommandParam ) )
                {
                    this.GENRE = strCommandParam;
                }
            }
            else if( strCommandName.Equals( "DEMOSTART" ) )
            {
                //2015.04.10 kairera0467

                if( !string.IsNullOrEmpty( strCommandParam ) )
                {
                    int nOFFSETms;
                    try
                    {
                        nOFFSETms = (int)( Convert.ToDouble( strCommandParam ) * 1000.0 );
                    }
                    catch
                    {
                        nOFFSETms = 0;
                    }


                    this.nデモBGMオフセット = nOFFSETms;
                }
            }
            else if( strCommandName.Equals( "BGMOVIE" ) )
            {
                //2016.02.02 kairera0467
                //背景動画の定義。DTXから入力もできるが、tjaからも入力できるようにする。

                if( !string.IsNullOrEmpty( strCommandParam ) )
                {
                    this.strBGVIDEO_PATH = strCommandParam;
                }

                var avi = new CAVI()
                {
                    n番号 = 1,
                    strファイル名 = this.strBGVIDEO_PATH,
                    strコメント文 = "BGMOVIE命令",
                };

                if( this.listAVI.ContainsKey(1) )	// 既にリスト中に存在しているなら削除。後のものが有効。
                    this.listAVI.Remove(1);

                this.listAVI.Add(1, avi);

                var ds = new CDirectShow()
                {
                    n番号 = 1,
                    strファイル名 = this.strBGVIDEO_PATH,
                    strコメント文 = "BGMOVIE命令",
                };

                if (this.listDS.ContainsKey(1))	// 既にリスト中に存在しているなら削除。後のものが有効。
                    this.listDS.Remove(1);

                this.listDS.Add(1, ds);
            }
            else if( strCommandName.Equals( "BGIMAGE" ) )
            {
                //2016.02.02 kairera0467
                if( !string.IsNullOrEmpty( strCommandParam ) )
                {
                    this.strBGIMAGE_PATH = strCommandParam;
                }
            }
            else if( strCommandName.Equals( "HIDDENBRANCH" ) )
            {
                //2016.04.01 kairera0467 パラメーターは
                if( !string.IsNullOrEmpty( strCommandParam ) )
                {
                    this.bHIDDENBRANCH = true;
                }
            }
            if( this.nScoreModeTmp == 99 )
            {
                //2017.01.28 DD 
                this.nScoreModeTmp = CDTXMania.ConfigIni.nScoreMode;
            }
        }
        /// <summary>
        /// string型からint型に変換する。
        /// TJAP2から持ってきた。
        /// </summary>
        private int CharConvertNote( string str )
        {
        	switch( str )
        	{
        		case "0":
        			return 0;
        		case "1":
        			return 1;
        		case "2":
        			return 2;
        		case "3":
        			return 3;
        		case "4":
        			return 4;
        		case "5":
        			return 5;
        		case "6":
        			return 6;
        		case "7":
        			return 7;
        		case "8":
        			return 8;
        		case "9":
        			return 7; //2017.01.30 DD 芋連打を風船連打扱いに
                case "A": //2017.08.22 kairera0467 手つなぎ
                    return 10;
                case "B":
                    return 11;
                case "F":
                    return 15;
        		default:
        			return -1;
        	}
        }

        private int strConvertCourse( string str )
        {
        	switch( str )
        	{
        		case "0":
        			return 0;
        		case "1":
        			return 1;
        		case "2":
        			return 2;
        		case "3":
        			return 3;
        		case "4":
        			return 4;
        		case "5":
        			return 5;
        	}

            // 2018.05.12 Code by.AioiLight
            // https://github.com/kairera0467/TJAP2fPC/issues/20
            string[] Matchptn = new string[6] { "easy", "normal", "hard", "oni", "edit", "tower" };
            for( int i = 0; i < Matchptn.Length; i++ )
            {
                if( Regex.IsMatch( str, Matchptn[ i ], RegexOptions.IgnoreCase ) )
                {
                    return i;
                }
            }

            return 3;
        }

        /// <summary>
        /// 複素数のパースもどき
        /// </summary>
        private void tParsedComplexNumber( string strScroll, ref double[] dbScroll )
        {
            bool bFirst = true; //最初の数値か
            bool bUse = false; //数値扱い中
            string[] arScroll = new string[ 2 ];
            char[] c = strScroll.ToCharArray();
            //1.0-1.0i
            for( int i = 0; i < strScroll.Length; i++ )
            {
                if( bFirst )
                    arScroll[ 0 ] += c[ i ];
                else
                    arScroll[ 1 ] += c[ i ];

                //次の文字が'i'なら脱出。
                if( c[ i + 1 ] == 'i' )
                    break;
                else if( c[ i + 1 ] == '-' || c[ i + 1 ] == '+' )
                    bFirst = false;

            }

            dbScroll[ 0 ] = Convert.ToDouble( arScroll[ 0 ] );
            dbScroll[ 1 ] = Convert.ToDouble( arScroll[ 1 ] );
            return;
        }

        private void tSetSenotes()
        {
            #region[ list作成 ]
            //ひとまずチップだけのリストを作成しておく。
            List<CDTX.CChip> list音符のみのリスト;
            list音符のみのリスト = new List<CChip>();
            int nCount = 0;
            int dkdkCount = 0;

            foreach( CChip chip in this.listChip )
            {
                if( chip.nチャンネル番号 >= 0x11 && chip.nチャンネル番号 < 0x18 )
                {
                    list音符のみのリスト.Add( chip );
                }
            }
            #endregion

            //時間判定は、「次のチップの発声時刻」から「現在(過去)のチップの発声時刻」で引く必要がある。
            //逆にしてしまうと計算がとてつもないことになるので注意。

            try
            {
                //this.tSenotes_Core( list音符のみのリスト );
                this.tSenotes_Core_V2( list音符のみのリスト );
            }
            catch( Exception ex )
            {
                Trace.TraceError( ex.StackTrace );
            }


            #region[統合前]
            //foreach( CChip pChip in list音符のみのリスト )
            //{
            //    int dbUnitTime = ( int )( ( ( 60.0 / this.dbNowBPM ) / 4.0 ) * 1000.0 );
            //    int nUnit4 = dbUnitTime * 4;
            //    int nUnit8 = dbUnitTime * 2;
            //    int nUnit16 = dbUnitTime;

            //    if( nCount == 0  )
            //    {
            //        nCount++;
            //        continue;
            //    }

            //    double db1個前の発生時刻ms = list音符のみのリスト[nCount - 1].n発声時刻ms * 1;

            //    if( nCount == 1 )
            //    {
            //        //nCount - 1は一番最初のノーツになる。

            //        if( pChip.n発声時刻ms - list音符のみのリスト[ nCount - 1 ].n発声時刻ms >= nUnit4 )
            //        {
            //            if( list音符のみのリスト[ nCount - 1 ].nチャンネル番号 == 0x93 )
            //                list音符のみのリスト[ nCount - 1 ].nSenote = 0;
            //            else if( list音符のみのリスト[ nCount - 1 ].nチャンネル番号 == 0x94 )
            //                list音符のみのリスト[ nCount - 1 ].nSenote = 3;

            //            if( list音符のみのリスト[ nCount + 1 ].n発声時刻ms - pChip.n発声時刻ms < nUnit4 )
            //            {
            //                if( list音符のみのリスト[ nCount + 1 ].n発声時刻ms - pChip.n発声時刻ms < nUnit8 )
            //                {
            //                    //16分なら「ド」
            //                    pChip.nSenote = 1;
            //                }
            //                else
            //                {
            //                    if( dkdkCount == 0 )
            //                    {
            //                        pChip.nSenote = 1;
            //                        dkdkCount++;
            //                    }
            //                    else if( dkdkCount == 1 )
            //                    {
            //                        pChip.nSenote = 2;
            //                        dkdkCount = 0;
            //                    }
                                    
            //                }
            //            }
            //            else
            //            {
            //                //次も4分なら「ドン」か「カッ」
            //                if( pChip.nチャンネル番号 == 0x93 )
            //                {
            //                    pChip.nSenote = 0;
            //                }
            //                else if( pChip.nチャンネル番号 == 0x94 )
            //                {
            //                    pChip.nSenote = 3;
            //                }
            //            }
            //        }
            //        else if( pChip.n発声時刻ms - list音符のみのリスト[ nCount - 1 ].n発声時刻ms <= nUnit4 && pChip.n発声時刻ms - list音符のみのリスト[ nCount - 1 ].n発声時刻ms >= nUnit8 )
            //        {
            //            if( list音符のみのリスト[ nCount - 1 ].nチャンネル番号 == 0x93 )
            //                list音符のみのリスト[ nCount - 1 ].nSenote = 1;
            //            else if( list音符のみのリスト[ nCount - 1 ].nチャンネル番号 == 0x94 )
            //                list音符のみのリスト[ nCount - 1 ].nSenote = 4;

            //            if( pChip.nチャンネル番号 == 0x93 )
            //            {
            //                pChip.nSenote = 1;
            //            }
            //            else if( pChip.nチャンネル番号 == 0x94 )
            //            {
            //                pChip.nSenote = 4;
            //            }
            //        }
            //        else if( pChip.n発声時刻ms - list音符のみのリスト[ nCount - 1 ].n発声時刻ms < nUnit8 )
            //        {
            //            if( list音符のみのリスト[ nCount - 1 ].nチャンネル番号 == 0x93 )
            //                list音符のみのリスト[ nCount - 1 ].nSenote = 1;
            //            else if( list音符のみのリスト[ nCount - 1 ].nチャンネル番号 == 0x94 )
            //                list音符のみのリスト[ nCount - 1 ].nSenote = 4;

            //            if( pChip.nチャンネル番号 == 0x93 )
            //            {
            //                pChip.nSenote = 1;
            //            }
            //            else if( pChip.nチャンネル番号 == 0x94 )
            //            {
            //                pChip.nSenote = 4;
            //            }
            //        }

            //        nCount++;
            //        continue;
            //    }

            //    double db2個前の発声時刻ms = list音符のみのリスト[ nCount - 2 ].n発声時刻ms * 1;

            //    #region[新しいやつ]
            //    if( nCount + 1 >= list音符のみのリスト.Count )
            //        break;

            //    if( pChip.n発声時刻ms - list音符のみのリスト[ nCount - 1 ].n発声時刻ms >= nUnit4 )
            //    {
            //        if( pChip.nチャンネル番号 == 0x93 )
            //        {
            //            pChip.nSenote = 0;
            //        }
            //        else if( pChip.nチャンネル番号 == 0x94 )
            //        {
            //            pChip.nSenote = 3;
            //        }

            //        if( list音符のみのリスト[ nCount + 1 ].n発声時刻ms - pChip.n発声時刻ms <= nUnit4 )
            //        {
            //            if( pChip.nチャンネル番号 == 0x93 )
            //                pChip.nSenote = 1;
            //            else if( pChip.nチャンネル番号 == 0x94 )
            //                pChip.nSenote = 4;
            //        }
            //    }
            //    else if( pChip.n発声時刻ms - list音符のみのリスト[ nCount - 1 ].n発声時刻ms < nUnit4 && pChip.n発声時刻ms - list音符のみのリスト[ nCount - 1 ].n発声時刻ms >= nUnit8 )
            //    {
            //        if( pChip.nチャンネル番号 == 0x93 )
            //        {
            //            pChip.nSenote = 1;
            //        }
            //        else if( pChip.nチャンネル番号 == 0x94 )
            //        {
            //            pChip.nSenote = 4;
            //        }

            //        if( list音符のみのリスト[ nCount + 1 ].n発声時刻ms - pChip.n発声時刻ms <= nUnit4 )
            //        {
            //            if( pChip.nチャンネル番号 == 0x93 )
            //                pChip.nSenote = 0;
            //            else if( pChip.nチャンネル番号 == 0x94 )
            //                pChip.nSenote = 3;

            //            if( list音符のみのリスト[ nCount + 2 ].n発声時刻ms - list音符のみのリスト[ nCount + 1 ].n発声時刻ms >= nUnit8 )
            //            {
            //                if( pChip.nチャンネル番号 == 0x93 )
            //                    pChip.nSenote = 1;
            //                else if( pChip.nチャンネル番号 == 0x94 )
            //                    pChip.nSenote = 4;
            //            }
            //            else if( list音符のみのリスト[ nCount + 2 ].n発声時刻ms - list音符のみのリスト[ nCount + 1 ].n発声時刻ms < nUnit8 )
            //            {
            //                if( pChip.nチャンネル番号 == 0x93 )
            //                    pChip.nSenote = 1;
            //                else if( pChip.nチャンネル番号 == 0x94 )
            //                    pChip.nSenote = 4;
            //            }

            //        }
            //        else
            //        {
            //            if( pChip.nチャンネル番号 == 0x93 )
            //                pChip.nSenote = 0;
            //            else if( pChip.nチャンネル番号 == 0x94 )
            //                pChip.nSenote = 3;
            //        }
            //    }
            //    else if( pChip.n発声時刻ms - list音符のみのリスト[ nCount - 1 ].n発声時刻ms < nUnit8 ) //8分以下
            //    {
            //        if( pChip.nチャンネル番号 == 0x93 )
            //        {
            //            pChip.nSenote = 1;
            //        }
            //        else if( pChip.nチャンネル番号 == 0x94 )
            //        {
            //            pChip.nSenote = 4;
            //        }

            //        //後ろが4分
            //        try
            //        {
            //            if( nCount + 1 >= list音符のみのリスト.Count )
            //                break;

            //            if( list音符のみのリスト[ nCount + 1 ].n発声時刻ms - pChip.n発声時刻ms >= nUnit8 ) //分岐があるとここがバグるっぽい?(Indexエラー)
            //            {
            //                if( pChip.nチャンネル番号 == 0x93 )
            //                {
            //                    pChip.nSenote = 0;
            //                }
            //                else if( pChip.nチャンネル番号 == 0x94 )
            //                {
            //                    pChip.nSenote = 3;
            //                }
            //            }
            //        }
            //        catch( Exception ex )
            //        {

            //        }


            //    }
            //    #endregion

            //    #region[古いやつ]
            //    ////2つ前と1つ前のチップのSenoteを決めていく。
            //    ////連打、大音符などはチップ配置の際に決めます。
            //    //if (( db1個前の発生時刻ms - db2個前の発声時刻ms ) >= nUnit4)
            //    //{
            //    //    //2つ前の音符と1つ前の音符の間が4分以上でかつ、その音符がドンなら2つ前のSenoteは「ドン」で確定。
            //    //    //同時にdkdkをリセット
            //    //    dkdkCount = false;
            //    //    if( list音符のみのリスト[nCount - 2].nチャンネル番号 == 0x93 )
            //    //        list音符のみのリスト[nCount - 2].nSenote = 0;
            //    //    else if( list音符のみのリスト[nCount - 2].nチャンネル番号 == 0x94 )
            //    //        list音符のみのリスト[nCount - 2].nSenote = 3;

            //    //    if( ( pChip.n発声時刻ms - db1個前の発生時刻ms ) >= nUnit4 )
            //    //    {
            //    //        //1つ前の音符と現在の音符の間が4分以上かつ、その音符がドンなら1つ前の音符は「ドン」で確定。
            //    //        if( list音符のみのリスト[nCount - 1].nチャンネル番号 == 0x93 )
            //    //            list音符のみのリスト[nCount - 1].nSenote = 0;
            //    //        else if( list音符のみのリスト[nCount - 1].nチャンネル番号 == 0x94 )
            //    //            list音符のみのリスト[nCount - 1].nSenote = 3;
            //    //    }
            //    //    else if( ( pChip.n発声時刻ms - db1個前の発生時刻ms ) <= nUnit4 )
            //    //    {
            //    //        //4分
            //    //        if( ( pChip.n発声時刻ms - db1個前の発生時刻ms ) >= nUnit8 )
            //    //        {
            //    //            dkdkCount = false;
            //    //            //1つ前の音符と現在の音符の間が8分以内で16分以上でかつ、その音符が赤なら1つ前の音符は「ド」で確定。
            //    //            if( list音符のみのリスト[ nCount - 1 ].nチャンネル番号 == 0x94 )
            //    //                list音符のみのリスト[ nCount - 1 ].nSenote = 2;
            //    //            else if( list音符のみのリスト[ nCount - 1 ].nチャンネル番号 == 0x94 )
            //    //                list音符のみのリスト[ nCount - 1 ].nSenote = 4;
            //    //        }
            //    //        else if( ( db1個前の発生時刻ms - db2個前の発声時刻ms ) <= nUnit8 )
            //    //        {
            //    //            dkdkCount = false;
            //    //            if( list音符のみのリスト[ nCount - 2 ].nチャンネル番号 == 0x93 )
            //    //            {
            //    //                list音符のみのリスト[ nCount - 2 ].nSenote = 1;
                                
            //    //                //ドコドン
            //    //                if( list音符のみのリスト[ nCount - 1 ].nチャンネル番号 == 0x93 )
            //    //                {
            //    //                    if( pChip.nチャンネル番号 == 0x93 )
            //    //                        pChip.nSenote = dkdkCount ? 2 : 1;
            //    //                    if( dkdkCount == false )
            //    //                        dkdkCount = true;
            //    //                    else
            //    //                        dkdkCount = false;
            //    //                }
            //    //            }
            //    //            else if( list音符のみのリスト[ nCount - 2 ].nチャンネル番号 == 0x94 )
            //    //                list音符のみのリスト[ nCount - 2 ].nSenote = 4;
            //    //        }

            //    //    }
            //    //}
            //    //else if ( ( db1個前の発生時刻ms - db2個前の発声時刻ms ) <= nUnit4 && ( db1個前の発生時刻ms - db2個前の発声時刻ms ) >= nUnit8)
            //    //{
            //    //    //2つ前の音符と1つ前の音符の間が8分以上でかつ、16分以内

            //    //    if( ( db1個前の発生時刻ms - db2個前の発声時刻ms ) >= nUnit8 && ( db1個前の発生時刻ms - db2個前の発声時刻ms ) > nUnit16 )
            //    //    {
            //    //        //2つ前の音符と1つ前の音符の間が8分以上でかつ、16分以内なら「ド」
            //    //        if( list音符のみのリスト[ nCount - 2 ].nチャンネル番号 == 0x93 )
            //    //        {
            //    //            list音符のみのリスト[ nCount - 2 ].nSenote = 1;
            //    //        }
            //    //        else if( list音符のみのリスト[ nCount - 2 ].nチャンネル番号 == 0x94 )
            //    //        {
            //    //            list音符のみのリスト[ nCount - 2 ].nSenote = 4;
            //    //        }
            //    //    }
            //    //    else if( ( db1個前の発生時刻ms - db2個前の発声時刻ms ) < nUnit8 )
            //    //    {
            //    //        //2つ前の音符と1つ前の音符の間が16分以内なら「ド」で確定
            //    //        if( list音符のみのリスト[ nCount - 2 ].nチャンネル番号 == 0x93 )
            //    //        {
            //    //            list音符のみのリスト[ nCount - 2 ].nSenote = 1;
            //    //        }
            //    //        else if( list音符のみのリスト[ nCount - 2 ].nチャンネル番号 == 0x94 )
            //    //            list音符のみのリスト[ nCount - 2 ].nSenote = 4;
            //    //    }

            //    //    if( ( pChip.n発声時刻ms - db1個前の発生時刻ms ) >= nUnit16 )
            //    //    {
            //    //        if( ( pChip.n発声時刻ms - db1個前の発生時刻ms ) >= nUnit8 )
            //    //        {
            //    //            if( list音符のみのリスト[ nCount - 2 ].nチャンネル番号 == 0x93 )
            //    //            {
            //    //                list音符のみのリスト[ nCount - 1 ].nSenote = 0;
            //    //            }
            //    //            else if( list音符のみのリスト[ nCount - 2 ].nチャンネル番号 == 0x94 )
            //    //                list音符のみのリスト[ nCount - 1 ].nSenote = 3;
            //    //        }
            //    //    }
            //    //}
            //    //else if ( ( db1個前の発生時刻ms - db2個前の発声時刻ms ) >= nUnit16 && ( db1個前の発生時刻ms - db2個前の発声時刻ms ) <= nUnit8 )
            //    //{
            //    //    //2つ前の音符と1つ前の音符の間が16分以上
            //    //    if( list音符のみのリスト[ nCount - 2 ].nチャンネル番号 == 0x93 )
            //    //    {
            //    //        list音符のみのリスト[ nCount - 2 ].nSenote = 1;
            //    //    }
            //    //    else if( list音符のみのリスト[ nCount - 2 ].nチャンネル番号 == 0x94 )
            //    //        list音符のみのリスト[ nCount - 2 ].nSenote = 4;

            //    //    if( ( pChip.n発声時刻ms - db1個前の発生時刻ms ) >= nUnit8 )
            //    //    {
            //    //        if( list音符のみのリスト[ nCount - 2 ].nチャンネル番号 == 0x93 )
            //    //        {
            //    //            list音符のみのリスト[ nCount - 1 ].nSenote = 0;
            //    //        }
            //    //        else if( list音符のみのリスト[ nCount - 2 ].nチャンネル番号 == 0x94 )
            //    //            list音符のみのリスト[ nCount - 1 ].nSenote = 3;
            //    //    }


            //    //}
            //    #endregion

            //    nCount++;
            //}
            #endregion


        }

        /// <summary>
        /// 譜面分岐がある場合はこちらを使う
        /// </summary>
        private void tSetSenotes_branch()
        {
            #region[ list作成 ]
            //ひとまずチップだけのリストを作成しておく。
            List<CDTX.CChip> list音符のみのリスト;
            List<CDTX.CChip> list普通譜面のみのリスト;
            List<CDTX.CChip> list玄人譜面のみのリスト;
            List<CDTX.CChip> list達人譜面のみのリスト;
            list音符のみのリスト = new List<CChip>();
            list普通譜面のみのリスト = new List<CChip>();
            list玄人譜面のみのリスト = new List<CChip>();
            list達人譜面のみのリスト = new List<CChip>();
            int nCount = 0;
            int dkdkCount = 0;

            foreach( CChip chip in this.listChip )
            {
                if( chip.nチャンネル番号 >= 0x11 && chip.nチャンネル番号 < 0x18 )
                {
                    list音符のみのリスト.Add( chip );

                    switch( chip.nコース )
                    {
                        case 0:
                            list普通譜面のみのリスト.Add( chip );
                            break;
                        case 1:
                            list玄人譜面のみのリスト.Add( chip );
                            break;
                        case 2:
                            list達人譜面のみのリスト.Add( chip );
                            break;
                    }
                }
            }
            #endregion

            //forで処理。
            for( int n = 0; n < 3; n++ )
            {
                switch( n )
                {
                    case 0:
                        list音符のみのリスト = list普通譜面のみのリスト;
                        break;
                    case 1:
                        list音符のみのリスト =  list玄人譜面のみのリスト;
                        break;
                    case 2:
                        list音符のみのリスト =  list達人譜面のみのリスト;
                        break;
                }

                //this.tSenotes_Core( list音符のみのリスト );
                this.tSenotes_Core_V2( list音符のみのリスト );
            }

        }

        /// <summary>
        /// コア部分。譜面分岐時の処理実装にあたって分離。
        /// </summary>
        private void tSenotes_Core( List<CChip> list音符のみのリスト )
        {
            int nCount = 0;
            int dkdkCount = 0;

            foreach( CChip pChip in list音符のみのリスト )
            {
                int dbUnitTime = (int)(((60.0 / pChip.dbBPM) / 4.0) * 1000.0);
                int nUnit4 = dbUnitTime * 4;
                int nUnit6 = dbUnitTime * 3;
                int nUnit8 = dbUnitTime * 2;
                int nUnit16 = dbUnitTime;

                float fUnitTime = ( ( ( 60.0f / (float)pChip.dbBPM ) / 4.0f ) * 1000.0f );
                //float fUnitTime = (float)Math.Round( ( ( ( 60.0f / (float)pChip.dbBPM ) / 4.0f ) * 1000.0f ), 0 );
                float fUnit4 = fUnitTime * 4.0f;
                float fUnit8 = fUnitTime * 2.0f;
                float fUnit16 = fUnitTime;

                if( pChip.dbBPM < 120 )
                {
                    nUnit4 = ( dbUnitTime * 4 ) / 2;
                    nUnit8 = ( dbUnitTime * 2 ) / 2;
                    nUnit16 = dbUnitTime / 2;

                    fUnit4 = fUnitTime * 4.0f;
                    fUnit8 = fUnitTime * 2.0f;
                    fUnit16 = fUnitTime;
                }

                if( nCount == 0  )
                {
                    nCount++;
                    continue;
                }

                double db1個前の発生時刻ms = list音符のみのリスト[nCount - 1].n発声時刻ms * 1;

                #region[ float ]
                /*
                if( nCount == 1 )
                {
                    //nCount - 1は一番最初のノーツになる。

                    if( pChip.f発声時刻ms - list音符のみのリスト[ nCount - 1 ].f発声時刻ms >= fUnit4 )
                    {
                        //2番目のノーツと1番目のノーツの間隔が4分かそれ以上
                        if( list音符のみのリスト[ nCount - 1 ].nチャンネル番号 == 0x93 )
                            list音符のみのリスト[ nCount - 1 ].nSenote = 0;
                        else if( list音符のみのリスト[ nCount - 1 ].nチャンネル番号 == 0x94 )
                            list音符のみのリスト[ nCount - 1 ].nSenote = 3;

                        if( list音符のみのリスト[ nCount + 1 ].f発声時刻ms - pChip.f発声時刻ms < fUnit4 )
                        {
                            if( list音符のみのリスト[ nCount + 1 ].f発声時刻ms - pChip.f発声時刻ms < fUnit8 )
                            {
                                //16分なら「ド」
                                if( pChip.nチャンネル番号 == 0x93 )
                                    pChip.nSenote = 1;
                            }
                            else
                            {
                                if (pChip.nチャンネル番号 != 0x93)
                                    break;

                                if( dkdkCount == 0 )
                                {
                                    pChip.nSenote = 1;
                                    dkdkCount++;
                                }
                                else if( dkdkCount == 1 )
                                {
                                    pChip.nSenote = 2;
                                    dkdkCount = 0;
                                }
                                    
                            }
                        }
                        else
                        {
                            //次も4分なら「ドン」か「カッ」
                            if( pChip.nチャンネル番号 == 0x93 )
                            {
                                pChip.nSenote = 0;
                            }
                            else if( pChip.nチャンネル番号 == 0x94 )
                            {
                                pChip.nSenote = 3;
                            }
                        }
                    }
                    else if( pChip.f発声時刻ms - list音符のみのリスト[ nCount - 1 ].f発声時刻ms < fUnit4 && pChip.f発声時刻ms - list音符のみのリスト[ nCount - 1 ].f発声時刻ms > fUnit16 )
                    {
                        //2番目のチップと1番目のチップの間隔が4分以下でかつ16分以上
                        if( list音符のみのリスト[ nCount - 1 ].nチャンネル番号 == 0x93 )
                            list音符のみのリスト[ nCount - 1 ].nSenote = 1;
                        else if( list音符のみのリスト[ nCount - 1 ].nチャンネル番号 == 0x94 )
                            list音符のみのリスト[ nCount - 1 ].nSenote = 4;



                        if( pChip.nチャンネル番号 == 0x93 )
                        {
                            pChip.nSenote = 1;
                        }
                        else if( pChip.nチャンネル番号 == 0x94 )
                        {
                            pChip.nSenote = 4;
                        }
                            //
                            //if (list音符のみのリスト[nCount - 1].nチャンネル番号 == 0x93)
                            //    list音符のみのリスト[nCount - 1].nSenote = 0;
                            //else if (list音符のみのリスト[nCount - 1].nチャンネル番号 == 0x94)
                            //    list音符のみのリスト[nCount - 1].nSenote = 3;

                        if (list音符のみのリスト[ nCount + 1 ].f発声時刻ms - pChip.f発声時刻ms >= fUnit4)
                        {
                            //3番目のチップと2番目のチップの間隔が4分以上
                            if( pChip.nチャンネル番号 == 0x93 )
                            {
                                pChip.nSenote = 0;
                            }
                            else if( pChip.nチャンネル番号 == 0x94 )
                            {
                                pChip.nSenote = 3;
                            }
                        }
                    }
                    else if( pChip.f発声時刻ms - list音符のみのリスト[ nCount - 1 ].f発声時刻ms <= fUnit16 )
                    {
                        //2番目のチップと1番目のチップの間隔が16分かそれ以下
                        if( list音符のみのリスト[ nCount - 1 ].nチャンネル番号 == 0x93 )
                            list音符のみのリスト[ nCount - 1 ].nSenote = 1;
                        else if( list音符のみのリスト[ nCount - 1 ].nチャンネル番号 == 0x94 )
                            list音符のみのリスト[ nCount - 1 ].nSenote = 4;

                        if (pChip.nチャンネル番号 == 0x93)
                        {
                            pChip.nSenote = 1;
                        }
                        else if (pChip.nチャンネル番号 == 0x94)
                        {
                            pChip.nSenote = 4;
                        }

                        //1番目のチップが0x93
                        if( list音符のみのリスト[ nCount - 1 ].nチャンネル番号 == 0x93 )
                        {
                            //3番目のチップと2番目のチップの間隔が16分
                            if( list音符のみのリスト[ nCount + 1 ].f発声時刻ms - pChip.f発声時刻ms == fUnit16 )
                            {
                                if( list音符のみのリスト[ nCount + 1 ].nチャンネル番号 == 0x93 )
                                {
                                    pChip.nSenote = 2;
                                }
                            }
                        }


                    }

                    nCount++;
                    continue;
                }

                double db2個前の発声時刻ms = list音符のみのリスト[ nCount - 2 ].n発声時刻ms * 1;

                #region[新しいやつ]
                if( pChip.f発声時刻ms - list音符のみのリスト[ nCount - 1 ].f発声時刻ms >= fUnit4 )
                {
                    //現在のチップと1つ前のチップの間隔が4分以上
                    dkdkCount = 0;
                    if( pChip.nチャンネル番号 == 0x93 )
                    {
                        pChip.nSenote = 0;
                        //break;
                    }
                    else if( pChip.nチャンネル番号 == 0x94 )
                    {
                        pChip.nSenote = 3;
                    }

                    if( nCount + 1 >= list音符のみのリスト.Count )
                        break;

                    //次のチップと現在のチップの間が4分以下
                    if( list音符のみのリスト[ nCount + 1 ].f発声時刻ms - pChip.f発声時刻ms < fUnit4 )
                    {
                        if( pChip.nチャンネル番号 == 0x93 )
                            pChip.nSenote = 1;
                        else if( pChip.nチャンネル番号 == 0x94 )
                            pChip.nSenote = 4;
                    }
                }
                else if( pChip.f発声時刻ms - list音符のみのリスト[ nCount - 1 ].f発声時刻ms < fUnit4 && pChip.f発声時刻ms - list音符のみのリスト[ nCount - 1 ].f発声時刻ms > fUnit16 )
                {
                    //現在のチップと1つ前のチップの間隔が4分以下かつ16分以上
                    dkdkCount = 0;
                    if( pChip.nチャンネル番号 == 0x93 )
                    {
                        pChip.nSenote = 1;
                    }
                    else if( pChip.nチャンネル番号 == 0x94 )
                    {
                        pChip.nSenote = 4;
                    }

                    if( nCount + 1 >= list音符のみのリスト.Count )
                        break;


                    if( list音符のみのリスト[ nCount + 1 ].f発声時刻ms - pChip.f発声時刻ms >= fUnit4 )
                    {
                        //次のチップと現在のチップの間隔が4分以上
                        if( pChip.nチャンネル番号 == 0x93 )
                            pChip.nSenote = 0;
                        else if( pChip.nチャンネル番号 == 0x94 )
                            pChip.nSenote = 3;
                    }
                    else if( list音符のみのリスト[ nCount + 1 ].f発声時刻ms - pChip.f発声時刻ms < fUnit4 && list音符のみのリスト[ nCount + 1 ].f発声時刻ms - pChip.f発声時刻ms > fUnit16 )
                    {
                        //次のチップと現在のチップの間隔が4分以下
                        if( pChip.nチャンネル番号 == 0x93 )
                            pChip.nSenote = 1;
                        else if( pChip.nチャンネル番号 == 0x94 )
                            pChip.nSenote = 4;

                        if( nCount + 2 >= list音符のみのリスト.Count )
                            break;

                        //次のチップが0x93
                        //if( list音符のみのリスト[ nCount + 1 ].nチャンネル番号 == 0x93 )
                        //{
                        //    if( list音符のみのリスト[ nCount + 2 ].n発声時刻ms - list音符のみのリスト[ nCount + 1 ].n発声時刻ms < nUnit8 )
                        //    {
                        //        list音符のみのリスト[ nCount + 1 ].nSenote = 2;
                        //    }
                        //}
                    }
                    else if( list音符のみのリスト[ nCount + 1 ].f発声時刻ms - pChip.f発声時刻ms <= fUnit16 )
                    {
                        //次のチップと現在のチップの間隔が16分以下
                        //そうなると1つ前のチップは「ドン」か「カッ」になる
                        //if( list音符のみのリスト[ nCount - 1 ].nチャンネル番号 == 0x93 )
                        //    list音符のみのリスト[ nCount - 1 ].nSenote = 0;
                        //else if( list音符のみのリスト[ nCount - 1 ].nチャンネル番号 == 0x94 )
                        //    list音符のみのリスト[ nCount - 1 ].nSenote = 0;
                    }
                }
                else if( pChip.f発声時刻ms - list音符のみのリスト[ nCount - 1 ].f発声時刻ms <= fUnit16 )
                {
                    //現在のノーツと1つ前のノーツの間隔が16分かそれ以下
                    if( pChip.nチャンネル番号 == 0x93 )
                    {
                        pChip.nSenote = 1;
                    }
                    else if( pChip.nチャンネル番号 == 0x94 )
                    {
                        pChip.nSenote = 4;
                    }

                    
                    try
                    {
                        if( nCount + 1 >= list音符のみのリスト.Count ) //一番最後のノーツだった時のエラー対策。
                            break;

                        //後ろが4分
                        if( list音符のみのリスト[ nCount + 1 ].f発声時刻ms - pChip.f発声時刻ms >= fUnit4 )
                        {
                            dkdkCount = 0;
                            if( pChip.nチャンネル番号 == 0x93 )
                            {
                                pChip.nSenote = 0;
                            }
                            else if( pChip.nチャンネル番号 == 0x94 )
                            {
                                pChip.nSenote = 3;
                            }
                        }
                        else if( list音符のみのリスト[ nCount + 1 ].f発声時刻ms - pChip.f発声時刻ms <= fUnit8 && list音符のみのリスト[ nCount + 1 ].f発声時刻ms - pChip.f発声時刻ms > fUnit16 )
                        {
                            //次のノーツと現在のノーツの間隔が8分かそれ以下でかつ16分以上
                            dkdkCount = 0;
                            if( pChip.nチャンネル番号 == 0x93 )
                            {
                                pChip.nSenote = 0;
                            }
                            else if( pChip.nチャンネル番号 == 0x94 )
                            {
                                pChip.nSenote = 3;
                            }
                        }
                        else if( list音符のみのリスト[ nCount + 1 ].f発声時刻ms - pChip.f発声時刻ms <= fUnit16 )
                        {
                            if( pChip.nチャンネル番号 == 0x93 )
                            {
                                pChip.nSenote = 1;

                                if( nCount + 2 >= list音符のみのリスト.Count )
                                    break;

                                if( pChip.f発声時刻ms - list音符のみのリスト[ nCount - 1 ].f発声時刻ms == fUnit16 )
                                {
                                    //1つ前のノーツと現在のノーツの間隔が16分
                                    if( list音符のみのリスト[ nCount - 1 ].nチャンネル番号 == 0x93 )
                                    {
                                        //チャンネル番号が0x93
                                        if( dkdkCount == 0 )
                                        {
                                            pChip.nSenote = 2;
                                            dkdkCount = 1;
                                        }
                                        else
                                        {
                                            pChip.nSenote = 1;
                                            dkdkCount = 0;
                                        }
                                    }
                                    
                                }
                                else
                                {
                                    if (pChip.nチャンネル番号 == 0x93)
                                        pChip.nSenote = 1;
                                    else
                                        pChip.nSenote = 4;
                                }
                            }
                            else if( pChip.nチャンネル番号 == 0x94 )
                            {
                                pChip.nSenote = 4;
                            }
                        }
                    }
                    catch( Exception ex )
                    {

                    }


                }
                #endregion
                */
                #endregion

                #region[ ミリ秒 ]
                
                if( nCount == 1 )
                {
                    #region[ 一番最初 ]
                    //nCount - 1は一番最初のノーツになる。

                    if( pChip.n発声時刻ms - list音符のみのリスト[ 0 ].n発声時刻ms >= nUnit4 )
                    {
                        //2番目のノーツと1番目のノーツの間隔が4分かそれ以上
                        if( list音符のみのリスト[ 0 ].nチャンネル番号 == 0x93 )
                            list音符のみのリスト[ 0 ].nSenote = 0;
                        else if( list音符のみのリスト[ 0 ].nチャンネル番号 == 0x94 )
                            list音符のみのリスト[ 0 ].nSenote = 3;

                        if( list音符のみのリスト[ nCount + 1 ].n発声時刻ms - pChip.n発声時刻ms < nUnit4 )
                        {
                            if( list音符のみのリスト[ nCount + 1 ].n発声時刻ms - pChip.n発声時刻ms < nUnit8 )
                            {



                                //16分なら「ド」
                                if( pChip.nチャンネル番号 == 0x93 )
                                    pChip.nSenote = 1;
                            }
                            else
                            {
                                if (pChip.nチャンネル番号 == 0x93)
                                {
                                    pChip.nSenote = 0;
                                }
                                else if (pChip.nチャンネル番号 == 0x94)
                                {
                                    pChip.nSenote = 3;
                                }
                            }
                        }
                        else
                        {
                            //次も4分なら「ドン」か「カッ」
                            if( pChip.nチャンネル番号 == 0x93 )
                            {
                                pChip.nSenote = 0;
                            }
                            else if( pChip.nチャンネル番号 == 0x94 )
                            {
                                pChip.nSenote = 3;
                            }
                        }
                    }
                    else if( pChip.n発声時刻ms - list音符のみのリスト[ nCount - 1 ].n発声時刻ms < nUnit4 && pChip.n発声時刻ms - list音符のみのリスト[ nCount - 1 ].n発声時刻ms > nUnit16 )
                    {
                        //2番目のチップと1番目のチップの間隔が4分以下でかつ16分以上
                        if( list音符のみのリスト[ 0 ].nチャンネル番号 == 0x93 )
                            list音符のみのリスト[ 0 ].nSenote = 1;
                        else if( list音符のみのリスト[ 0 ].nチャンネル番号 == 0x94 )
                            list音符のみのリスト[ 0 ].nSenote = 4;



                        if( pChip.nチャンネル番号 == 0x93 )
                        {
                            pChip.nSenote = 1;
                        }
                        else if( pChip.nチャンネル番号 == 0x94 )
                        {
                            pChip.nSenote = 4;
                        }

                        if (list音符のみのリスト[ nCount + 1 ].n発声時刻ms - pChip.n発声時刻ms >= nUnit4)
                        {
                            //3番目のチップと2番目のチップの間隔が4分以上
                            if( pChip.nチャンネル番号 == 0x93 )
                            {
                                pChip.nSenote = 0;
                            }
                            else if( pChip.nチャンネル番号 == 0x94 )
                            {
                                pChip.nSenote = 3;
                            }
                        }
                        if( list音符のみのリスト[ nCount + 1 ].n発声時刻ms - pChip.n発声時刻ms < nUnit4 )
                        {
                            //3番目のチップと2番目のチップの間隔が4分以下
                            if( list音符のみのリスト[ nCount + 1 ].n発声時刻ms - pChip.n発声時刻ms <= nUnit8 )
                            {

                                if( list音符のみのリスト[ nCount + 1 ].n発声時刻ms - pChip.n発声時刻ms < nUnit8 )
                                {
                                    if( list音符のみのリスト[ 0 ].nチャンネル番号 == 0x93 )
                                        list音符のみのリスト[ 0 ].nSenote = 0;
                                    else if( list音符のみのリスト[ 0 ].nチャンネル番号 == 0x94 )
                                        list音符のみのリスト[ 0 ].nSenote = 3;
                                }




                                //16分なら「ド」
                                if( pChip.nチャンネル番号 == 0x93 )
                                    pChip.nSenote = 1;
                            }
                            else
                            {
                                if (pChip.nチャンネル番号 == 0x93)
                                {
                                    pChip.nSenote = 1;
                                }
                                else if (pChip.nチャンネル番号 == 0x94)
                                {
                                    pChip.nSenote = 4;
                                }
                            }
                        }
                    }
                    else if( pChip.n発声時刻ms - list音符のみのリスト[ nCount - 1 ].n発声時刻ms <= nUnit16 )
                    {
                        //2番目のチップと1番目のチップの間隔が16分かそれ以下
                        if( list音符のみのリスト[ nCount - 1 ].nチャンネル番号 == 0x93 )
                            list音符のみのリスト[ nCount - 1 ].nSenote = 1;
                        else if( list音符のみのリスト[ nCount - 1 ].nチャンネル番号 == 0x94 )
                            list音符のみのリスト[ nCount - 1 ].nSenote = 4;

                        if (pChip.nチャンネル番号 == 0x93)
                        {
                            pChip.nSenote = 1;
                        }
                        else if (pChip.nチャンネル番号 == 0x94)
                        {
                            pChip.nSenote = 4;
                        }

                        //1番目のチップが0x93
                        if( list音符のみのリスト[ nCount - 1 ].nチャンネル番号 == 0x93 )
                        {
                            //3番目のチップと2番目のチップの間隔が16分
                            if( list音符のみのリスト[ nCount + 1 ].n発声時刻ms - pChip.n発声時刻ms == nUnit16 )
                            {
                                if( list音符のみのリスト[ nCount + 1 ].nチャンネル番号 == 0x93 )
                                {
                                    pChip.nSenote = 2;
                                }
                            }
                        }


                    }

                    nCount++;
                    continue;

                    #endregion
                }

                double db2個前の発声時刻ms = list音符のみのリスト[ nCount - 2 ].n発声時刻ms * 1;

                #region[新しいやつ]
                if( pChip.n発声時刻ms - list音符のみのリスト[ nCount - 1 ].n発声時刻ms >= nUnit4 )
                {
                    //現在のチップと1つ前のチップの間隔が4分以上
                    dkdkCount = 0;
                    if( pChip.nチャンネル番号 == 0x93 )
                    {
                        pChip.nSenote = 0;
                    }
                    else if( pChip.nチャンネル番号 == 0x94 )
                    {
                        pChip.nSenote = 3;
                    }

                    if( nCount + 1 >= list音符のみのリスト.Count )
                        break;

                    //次のチップと現在のチップの間が4分以下
                    if( list音符のみのリスト[ nCount + 1 ].n発声時刻ms - pChip.n発声時刻ms < nUnit6 )
                    {
                        if( pChip.nチャンネル番号 == 0x93 )
                            pChip.nSenote = 1;
                        else if( pChip.nチャンネル番号 == 0x94 )
                            pChip.nSenote = 4;

                        //12、16分があるなら「ドン」か「カッ」に変える

                        if( list音符のみのリスト[ nCount + 2 ].n発声時刻ms - list音符のみのリスト[ nCount + 1 ].n発声時刻ms <= nUnit16 )
                        {
                            if( pChip.nチャンネル番号 == 0x93 )
                                pChip.nSenote = 1;
                            else if( pChip.nチャンネル番号 == 0x94 )
                                pChip.nSenote = 4;
                        }
                        if( list音符のみのリスト[ nCount + 2 ].n発声時刻ms - list音符のみのリスト[ nCount + 1 ].n発声時刻ms < nUnit8 )
                        {
                            if( pChip.nチャンネル番号 == 0x93 )
                                pChip.nSenote = 0;
                            else if( pChip.nチャンネル番号 == 0x94 )
                                pChip.nSenote = 3;
                        }
                    }
                    //else
                    //{
                    //    if( pChip.nチャンネル番号 == 0x93 )
                    //    {
                    //        pChip.nSenote = 0;
                    //    }
                    //    else if( pChip.nチャンネル番号 == 0x94 )
                    //    {
                    //        pChip.nSenote = 3;
                    //    }
                    //}
                }
                else if( pChip.n発声時刻ms - list音符のみのリスト[ nCount - 1 ].n発声時刻ms < nUnit4 && pChip.n発声時刻ms - list音符のみのリスト[ nCount - 1 ].n発声時刻ms > nUnit16 )
                {
                    //現在のチップと1つ前のチップの間隔が4分以下かつ16分以上
                    dkdkCount = 0;
                    if( pChip.nチャンネル番号 == 0x93 )
                    {
                        pChip.nSenote = 1;
                    }
                    else if( pChip.nチャンネル番号 == 0x94 )
                    {
                        pChip.nSenote = 4;
                    }

                    if( nCount + 1 >= list音符のみのリスト.Count )
                        break;


                    if( list音符のみのリスト[ nCount + 1 ].n発声時刻ms - pChip.n発声時刻ms >= nUnit4 )
                    {
                        //次のチップと現在のチップの間隔が4分以上
                        if( pChip.nチャンネル番号 == 0x93 )
                            pChip.nSenote = 0;
                        else if( pChip.nチャンネル番号 == 0x94 )
                            pChip.nSenote = 3;
                    }
                    else if( list音符のみのリスト[ nCount + 1 ].n発声時刻ms - pChip.n発声時刻ms < nUnit4 && list音符のみのリスト[ nCount + 1 ].n発声時刻ms - pChip.n発声時刻ms > nUnit16 )
                    {
                        //次のチップと現在のチップの間隔が4分以下
                        if( pChip.nチャンネル番号 == 0x93 )
                            pChip.nSenote = 1;
                        else if( pChip.nチャンネル番号 == 0x94 )
                            pChip.nSenote = 4;

                        if( nCount + 2 >= list音符のみのリスト.Count )
                            break;

                        //次のチップが0x93
                        //if( list音符のみのリスト[ nCount + 1 ].nチャンネル番号 == 0x93 )
                        //{
                        //    if( list音符のみのリスト[ nCount + 2 ].n発声時刻ms - list音符のみのリスト[ nCount + 1 ].n発声時刻ms < nUnit8 )
                        //    {
                        //        list音符のみのリスト[ nCount + 1 ].nSenote = 2;
                        //    }
                        //}

                        //12、16分があるなら「ドン」か「カッ」に変える
                        if (list音符のみのリスト[nCount + 2].n発声時刻ms - list音符のみのリスト[nCount + 1].n発声時刻ms < nUnit8)
                        {
                            if (pChip.nチャンネル番号 == 0x93)
                                pChip.nSenote = 0;
                            else if (pChip.nチャンネル番号 == 0x94)
                                pChip.nSenote = 3;

                            if( list音符のみのリスト[nCount - 1].n発声時刻ms - list音符のみのリスト[nCount - 2].n発声時刻ms >= nUnit8 )
                            {
                        if( pChip.nチャンネル番号 == 0x93 )
                            pChip.nSenote = 1;
                        else if( pChip.nチャンネル番号 == 0x94 )
                            pChip.nSenote = 4;
                            }


                        }
                    }
                    else if( list音符のみのリスト[ nCount + 1 ].n発声時刻ms - pChip.n発声時刻ms <= nUnit16 )
                    {
                        //次のチップと現在のチップの間隔が16分以下
                        //そうなると1つ前のチップは「ドン」か「カッ」になる
                        //if( list音符のみのリスト[ nCount - 1 ].nチャンネル番号 == 0x93 )
                        //    list音符のみのリスト[ nCount - 1 ].nSenote = 0;
                        //else if( list音符のみのリスト[ nCount - 1 ].nチャンネル番号 == 0x94 )
                        //    list音符のみのリスト[ nCount - 1 ].nSenote = 3;
                    }
                    else
                    {
                        if( pChip.nチャンネル番号 == 0x93 )
                            pChip.nSenote = 0;
                        else if( pChip.nチャンネル番号 == 0x94 )
                            pChip.nSenote = 3;
                    }
                }
                else if( pChip.n発声時刻ms - list音符のみのリスト[ nCount - 1 ].n発声時刻ms <= nUnit16 )
                {
                    //現在のノーツと1つ前のノーツの間隔が16分かそれ以下
                    if( pChip.nチャンネル番号 == 0x93 )
                    {
                        pChip.nSenote = 1;
                    }
                    else if( pChip.nチャンネル番号 == 0x94 )
                    {
                        pChip.nSenote = 4;
                    }

                    
                    try
                    {
                        if( nCount + 1 >= list音符のみのリスト.Count ) //一番最後のノーツだった時のエラー対策。
                            break;

                        //後ろが4分
                        if( list音符のみのリスト[ nCount + 1 ].n発声時刻ms - pChip.n発声時刻ms >= nUnit4 )
                        {
                            dkdkCount = 0;
                            if( pChip.nチャンネル番号 == 0x93 )
                            {
                                pChip.nSenote = 0;
                            }
                            else if( pChip.nチャンネル番号 == 0x94 )
                            {
                                pChip.nSenote = 3;
                            }
                        }
                        else if( list音符のみのリスト[ nCount + 1 ].n発声時刻ms - pChip.n発声時刻ms <= nUnit4 && list音符のみのリスト[ nCount + 1 ].n発声時刻ms - pChip.n発声時刻ms >= nUnit8 )
                        {
                            //次のノーツと現在のノーツの間隔が8分かそれ以下でかつ16分以上
                            dkdkCount = 0;
                            if( pChip.nチャンネル番号 == 0x93 )
                            {
                                pChip.nSenote = 0;
                            }
                            else if( pChip.nチャンネル番号 == 0x94 )
                            {
                                pChip.nSenote = 3;
                            }
                        }
                        else if( list音符のみのリスト[ nCount + 1 ].n発声時刻ms - pChip.n発声時刻ms < nUnit8 )
                        {
                            if( pChip.nチャンネル番号 == 0x93 )
                            {
                                pChip.nSenote = 1;

                                if( nCount + 2 >= list音符のみのリスト.Count )
                                    break;


                                //2015.03.31 kairera0467　「コ」を調節する部分。ただし動作があやしすぎるため、いったん封印。
                                if( pChip.n発声時刻ms - list音符のみのリスト[ nCount - 1 ].n発声時刻ms <= nUnit16 )
                                {
                                    //1つ前のノーツと現在のノーツの間隔が16分
                                    if( list音符のみのリスト[ nCount - 1 ].nチャンネル番号 == 0x93 )
                                    {
                                        //チャンネル番号が0x93
                                        //if( dkdkCount == 0 )
                                        //{
                                        //    pChip.nSenote = 2;
                                        //    dkdkCount = 1;
                                        //}
                                        //else
                                        //{
                                        //    pChip.nSenote = 1;
                                        //    dkdkCount = 0;
                                        //}
                                    }
                                    
                                }
                            }
                            else if( pChip.nチャンネル番号 == 0x94 )
                            {
                                pChip.nSenote = 4;
                            }
                        }
                    }
                    catch( Exception ex )
                    {
                        Trace.TraceError( ex.StackTrace );
                    }
                }
                else
                {
                    if( pChip.nチャンネル番号 == 0x93 )
                    {
                        pChip.nSenote = 0;
                    }
                    else if( pChip.nチャンネル番号 == 0x94 )
                    {
                        pChip.nSenote = 3;
                    }
                }
                #endregion
                
                #endregion

                #region[古いやつ]
                ////2つ前と1つ前のチップのSenoteを決めていく。
                ////連打、大音符などはチップ配置の際に決めます。
                //if (( db1個前の発生時刻ms - db2個前の発声時刻ms ) >= nUnit4)
                //{
                //    //2つ前の音符と1つ前の音符の間が4分以上でかつ、その音符がドンなら2つ前のSenoteは「ドン」で確定。
                //    //同時にdkdkをリセット
                //    dkdkCount = false;
                //    if( list音符のみのリスト[nCount - 2].nチャンネル番号 == 0x93 )
                //        list音符のみのリスト[nCount - 2].nSenote = 0;
                //    else if( list音符のみのリスト[nCount - 2].nチャンネル番号 == 0x94 )
                //        list音符のみのリスト[nCount - 2].nSenote = 3;

                //    if( ( pChip.n発声時刻ms - db1個前の発生時刻ms ) >= nUnit4 )
                //    {
                //        //1つ前の音符と現在の音符の間が4分以上かつ、その音符がドンなら1つ前の音符は「ドン」で確定。
                //        if( list音符のみのリスト[nCount - 1].nチャンネル番号 == 0x93 )
                //            list音符のみのリスト[nCount - 1].nSenote = 0;
                //        else if( list音符のみのリスト[nCount - 1].nチャンネル番号 == 0x94 )
                //            list音符のみのリスト[nCount - 1].nSenote = 3;
                //    }
                //    else if( ( pChip.n発声時刻ms - db1個前の発生時刻ms ) <= nUnit4 )
                //    {
                //        //4分
                //        if( ( pChip.n発声時刻ms - db1個前の発生時刻ms ) >= nUnit8 )
                //        {
                //            dkdkCount = false;
                //            //1つ前の音符と現在の音符の間が8分以内で16分以上でかつ、その音符が赤なら1つ前の音符は「ド」で確定。
                //            if( list音符のみのリスト[ nCount - 1 ].nチャンネル番号 == 0x94 )
                //                list音符のみのリスト[ nCount - 1 ].nSenote = 2;
                //            else if( list音符のみのリスト[ nCount - 1 ].nチャンネル番号 == 0x94 )
                //                list音符のみのリスト[ nCount - 1 ].nSenote = 4;
                //        }
                //        else if( ( db1個前の発生時刻ms - db2個前の発声時刻ms ) <= nUnit8 )
                //        {
                //            dkdkCount = false;
                //            if( list音符のみのリスト[ nCount - 2 ].nチャンネル番号 == 0x93 )
                //            {
                //                list音符のみのリスト[ nCount - 2 ].nSenote = 1;
                                
                //                //ドコドン
                //                if( list音符のみのリスト[ nCount - 1 ].nチャンネル番号 == 0x93 )
                //                {
                //                    if( pChip.nチャンネル番号 == 0x93 )
                //                        pChip.nSenote = dkdkCount ? 2 : 1;
                //                    if( dkdkCount == false )
                //                        dkdkCount = true;
                //                    else
                //                        dkdkCount = false;
                //                }
                //            }
                //            else if( list音符のみのリスト[ nCount - 2 ].nチャンネル番号 == 0x94 )
                //                list音符のみのリスト[ nCount - 2 ].nSenote = 4;
                //        }

                //    }
                //}
                //else if ( ( db1個前の発生時刻ms - db2個前の発声時刻ms ) <= nUnit4 && ( db1個前の発生時刻ms - db2個前の発声時刻ms ) >= nUnit8)
                //{
                //    //2つ前の音符と1つ前の音符の間が8分以上でかつ、16分以内

                //    if( ( db1個前の発生時刻ms - db2個前の発声時刻ms ) >= nUnit8 && ( db1個前の発生時刻ms - db2個前の発声時刻ms ) > nUnit16 )
                //    {
                //        //2つ前の音符と1つ前の音符の間が8分以上でかつ、16分以内なら「ド」
                //        if( list音符のみのリスト[ nCount - 2 ].nチャンネル番号 == 0x93 )
                //        {
                //            list音符のみのリスト[ nCount - 2 ].nSenote = 1;
                //        }
                //        else if( list音符のみのリスト[ nCount - 2 ].nチャンネル番号 == 0x94 )
                //        {
                //            list音符のみのリスト[ nCount - 2 ].nSenote = 4;
                //        }
                //    }
                //    else if( ( db1個前の発生時刻ms - db2個前の発声時刻ms ) < nUnit8 )
                //    {
                //        //2つ前の音符と1つ前の音符の間が16分以内なら「ド」で確定
                //        if( list音符のみのリスト[ nCount - 2 ].nチャンネル番号 == 0x93 )
                //        {
                //            list音符のみのリスト[ nCount - 2 ].nSenote = 1;
                //        }
                //        else if( list音符のみのリスト[ nCount - 2 ].nチャンネル番号 == 0x94 )
                //            list音符のみのリスト[ nCount - 2 ].nSenote = 4;
                //    }

                //    if( ( pChip.n発声時刻ms - db1個前の発生時刻ms ) >= nUnit16 )
                //    {
                //        if( ( pChip.n発声時刻ms - db1個前の発生時刻ms ) >= nUnit8 )
                //        {
                //            if( list音符のみのリスト[ nCount - 2 ].nチャンネル番号 == 0x93 )
                //            {
                //                list音符のみのリスト[ nCount - 1 ].nSenote = 0;
                //            }
                //            else if( list音符のみのリスト[ nCount - 2 ].nチャンネル番号 == 0x94 )
                //                list音符のみのリスト[ nCount - 1 ].nSenote = 3;
                //        }
                //    }
                //}
                //else if ( ( db1個前の発生時刻ms - db2個前の発声時刻ms ) >= nUnit16 && ( db1個前の発生時刻ms - db2個前の発声時刻ms ) <= nUnit8 )
                //{
                //    //2つ前の音符と1つ前の音符の間が16分以上
                //    if( list音符のみのリスト[ nCount - 2 ].nチャンネル番号 == 0x93 )
                //    {
                //        list音符のみのリスト[ nCount - 2 ].nSenote = 1;
                //    }
                //    else if( list音符のみのリスト[ nCount - 2 ].nチャンネル番号 == 0x94 )
                //        list音符のみのリスト[ nCount - 2 ].nSenote = 4;

                //    if( ( pChip.n発声時刻ms - db1個前の発生時刻ms ) >= nUnit8 )
                //    {
                //        if( list音符のみのリスト[ nCount - 2 ].nチャンネル番号 == 0x93 )
                //        {
                //            list音符のみのリスト[ nCount - 1 ].nSenote = 0;
                //        }
                //        else if( list音符のみのリスト[ nCount - 2 ].nチャンネル番号 == 0x94 )
                //            list音符のみのリスト[ nCount - 1 ].nSenote = 3;
                //    }


                //}
                #endregion

                nCount++;
            }
        }

        /// <summary>
        /// コア部分Ver2。TJAP2から移植しただけ。
        /// </summary>
        /// <param name="list音符のみのリスト"></param>
        private void tSenotes_Core_V2( List<CChip> list音符のみのリスト )
        {
	        const int DATA = 3;
	        int doco_count = 0;
	        int[] sort = new int [7];
	        double[] time = new double[7];
	        double[] scroll = new double [7];
	        double time_tmp;

          	for(int i = 0; i < list音符のみのリスト.Count; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    if (i + (j - 3) < 0)
                    {
                        sort[j] = -1;
                        time[j] = -1000000000;
                        scroll[j] = 1.0;
                    }
                    else if (i + (j - 3) >= list音符のみのリスト.Count)
                    {
                        sort[j] = -1;
                        time[j] = 1000000000;
                        scroll[j] = 1.0;
                    }
                    else
                    {
                        sort[j] = list音符のみのリスト[i + (j - 3)].nチャンネル番号;
                        time[j] = list音符のみのリスト[i + (j - 3)].fBMSCROLLTime;
                        scroll[j] = list音符のみのリスト[i + (j - 3)].dbSCROLL;
                    }
                }
                time_tmp = time[DATA];
                for (int j = 0; j < 7; j++)
                {
                    time[j] = (time[j] - time_tmp) * scroll[j];
                    if (time[j] < 0) time[j] *= -1;
                }

                switch( list音符のみのリスト[i].nチャンネル番号 )
                {
                    case 0x11:

                        //（左2より離れている｜）_右2_右ドン_右右4_右右ドン…
                        if ((time[DATA - 1] > 2/* || (sort[DATA-1] != 1 && time[DATA-1] >= 2 && time[DATA-2] >= 4 && time[DATA-3] <= 5)*/) && time[DATA + 1] == 2 && sort[DATA + 1] == 1 && time[DATA + 2] == 4 && sort[DATA + 2] == 0x11 && time[DATA + 3] == 6 && sort[DATA + 3] == 0x11)
                        {
                            list音符のみのリスト[i].nSenote = 1;
                            doco_count = 1;
                            break;
                        }
                        //ドコドコ中_左2_右2_右ドン
                        else if (doco_count != 0 && time[DATA - 1] == 2 && time[DATA + 1] == 2 && (sort[DATA + 1] == 0x11 || sort[DATA + 1] == 0x11))
                        {
                            if (doco_count % 2 == 0)
                                list音符のみのリスト[i].nSenote = 1;
                            else
                                list音符のみのリスト[i].nSenote = 2;
                            doco_count++;
                            break;
                        }
                        else
                        {
                            doco_count = 0;
                        }

                        //8分ドコドン
                        if( ( time[ DATA - 2 ] >= 4.1 && time[ DATA - 1 ] == 2 && time[ DATA + 1 ] == 2 && time[ DATA + 2 ] >= 4.1 ) && ( sort[ DATA - 1 ] == 0x11 && sort[ DATA + 1 ] == 0x11 ) )
                        {
                            if( list音符のみのリスト[ i ].dbBPM >= 120.0 )
                            {
                                list音符のみのリスト[ i - 1 ].nSenote = 1;
                                list音符のみのリスト[ i ].nSenote = 2;
                                list音符のみのリスト[ i + 1 ].nSenote = 0;
                                break;
                            }
                            else if( list音符のみのリスト[ i ].dbBPM < 120.0 )
                            {
                                list音符のみのリスト[ i - 1 ].nSenote = 0;
                                list音符のみのリスト[ i ].nSenote = 0;
                                list音符のみのリスト[ i + 1 ].nSenote = 0;
                                break;
                            }
                        }

                        //BPM120以下のみ
                        //8分間隔の「ドドド」→「ドンドンドン」

                        if( time[ DATA - 1 ] >= 2 && time[ DATA + 1 ] >= 2 )
                        {
                            if( list音符のみのリスト[ i ].dbBPM < 120.0 )
                            {
                                list音符のみのリスト[ i ].nSenote = 0;
                                break;
                            }
                        }

                        //ドコドコドン
                        if (time[DATA - 3] >= 3.4 && time[DATA - 2] == 2 && time[DATA - 1] == 1 && time[DATA + 1] == 1 && time[DATA + 2] == 2 && time[DATA + 3] >= 3.4 && sort[DATA - 2] == 0x93 && sort[DATA - 1] == 0x11 && sort[DATA + 1] == 0x11 && sort[DATA + 2] == 0x11)
                        {
                            list音符のみのリスト[i - 2].nSenote = 1;
                            list音符のみのリスト[i - 1].nSenote = 2;
                            list音符のみのリスト[i + 0].nSenote = 1;
                            list音符のみのリスト[i + 1].nSenote = 2;
                            list音符のみのリスト[i + 2].nSenote = 0;
                            i += 2;
                            //break;
                        }
                        //ドコドン
                        else if (time[DATA - 2] >= 2.4 && time[DATA - 1] == 1 && time[DATA + 1] == 1 && time[DATA + 2] >= 2.4 && sort[DATA - 1] == 0x11 && sort[DATA + 1] == 0x11)
                        {
                            list音符のみのリスト[i].nSenote = 2;
                        }
                        //右の音符が2以上離れている
                        else if (time[DATA + 1] > 2)
                        {
                            list音符のみのリスト[i].nSenote = 0;
                        }
                        //右の音符が1.4以上_左の音符が1.4以内
                        else if (time[DATA + 1] >= 1.4 && time[DATA - 1] <= 1.4)
                        {
                            list音符のみのリスト[i].nSenote = 0;
                        }
                        //右の音符が2以上_右右の音符が3以内
                        else if (time[DATA + 1] >= 2 && time[DATA + 2] <= 3)
                        {
                            list音符のみのリスト[i].nSenote = 0;
                        }
                        //右の音符が2以上_大音符
                        else if (time[DATA + 1] >= 2 && (sort[DATA + 1] == 0x13 || sort[DATA + 1] == 0x14))
                        {
                            list音符のみのリスト[i].nSenote = 0;
                        }
                        else
                        {
                            list音符のみのリスト[i].nSenote = 1;
                        }
                        break;
                    case 0x12:
                        doco_count = 0;

                        //BPM120以下のみ
                        //8分間隔の「ドドド」→「ドンドンドン」
                        if (time[DATA - 1] == 2 && time[DATA + 1] == 2)
                        {
                            if (list音符のみのリスト[i - 1].dbBPM < 120.0 && list音符のみのリスト[i].dbBPM < 120.0 && list音符のみのリスト[i + 1].dbBPM < 120.0)
                            {
                                list音符のみのリスト[i].nSenote = 3;
                                break;
                            }
                        }

                        //右の音符が2以上離れている
                        if (time[DATA + 1] > 2)
                        {
                            list音符のみのリスト[i].nSenote = 3;
                        }
                        //右の音符が1.4以上_左の音符が1.4以内
                        else if (time[DATA + 1] >= 1.4 && time[DATA - 1] <= 1.4)
                        {
                            list音符のみのリスト[i].nSenote = 3;
                        }
                        //右の音符が2以上_右右の音符が3以内
                        else if (time[DATA + 1] >= 2 && time[DATA + 2] <= 3)
                        {
                            list音符のみのリスト[i].nSenote = 3;
                        }
                        //右の音符が2以上_大音符
                        else if (time[DATA + 1] >= 2 && (sort[DATA + 1] == 0x13 || sort[DATA + 1] == 0x14))
                        {
                            list音符のみのリスト[i].nSenote = 3;
                        }
                        else
                        {
                            list音符のみのリスト[i].nSenote = 4;
                        }
                        break;
                    default:
                        doco_count = 0;
                        break;
                }
            }
        }

        //2017.01.31 DD
        //命令と値を分割して配列に格納 (命令と値の間にスペースが無くてもOK) {入力テキスト, 対象配列, 対象命令}
        private void SplitOrder( string argText, out string[] argArray, string argOrder )
        {
            string regStr;
            string replStr;
            if( argOrder == "#BRANCHSTART")
            {
                regStr = argOrder + "[^0-9rpsd]+";
                replStr = argOrder;
                argText = Regex.Replace(argText, regStr, replStr);
            }
            else
            {
                regStr = argOrder + "[^0-9-]+";
                replStr = argOrder;
                argText = Regex.Replace(argText, regStr, replStr);
            }
            argArray = argText.Split(new string[] { argOrder }, StringSplitOptions.RemoveEmptyEntries);
            List<string> stringList = new List<string>(argArray);
            stringList.Insert(0, argOrder);
            argArray = stringList.ToArray();
        }

		/// <summary>
		/// サウンドミキサーにサウンドを登録_削除する時刻を事前に算出する
		/// </summary>
		public void PlanToAddMixerChannel()
		{
			if ( CDTXMania.Sound管理.GetCurrentSoundDeviceType() == "DirectSound" )	// DShowでの再生の場合はミキシング負荷が高くないため、
			{																		// チップのライフタイム管理を行わない
				return;
			}

			List<CChip> listAddMixerChannel = new List<CChip>( 128 ); ;
			List<CChip> listRemoveMixerChannel = new List<CChip>( 128 );
			List<CChip> listRemoveTiming = new List<CChip>( 128 );

			foreach ( CChip pChip in listChip )
			{
				switch ( pChip.nチャンネル番号 )
				{
					// BGM, 演奏チャネル, 不可視サウンド, フィルインサウンド, 空打ち音はミキサー管理の対象
					// BGM:
					case 0x01:
					// Dr演奏チャネル
                    //case 0x11:	case 0x12:	case 0x13:	case 0x14:	case 0x15:	case 0x16:	case 0x17:	case 0x18:	case 0x19:	case 0x1A:  case 0x1B:  case 0x1C:
					// Gt演奏チャネル
                    //case 0x20:	case 0x21:	case 0x22:	case 0x23:	case 0x24:	case 0x25:	case 0x26:	case 0x27:	case 0x28:
					// Bs演奏チャネル
                    //case 0xA0:	case 0xA1:	case 0xA2:	case 0xA3:	case 0xA4:	case 0xA5:	case 0xA6:	case 0xA7:	case 0xA8:
					// Dr不可視チップ
                    //case 0x31:	case 0x32:	case 0x33:	case 0x34:	case 0x35:	case 0x36:	case 0x37:
                    //case 0x38:	case 0x39:	case 0x3A:
					// Dr/Gt/Bs空打ち
                    //case 0xB1:	case 0xB2:	case 0xB3:	case 0xB4:	case 0xB5:	case 0xB6:	case 0xB7:	case 0xB8:
                    //case 0xB9:	case 0xBA:	case 0xBB:	case 0xBC:
					// フィルインサウンド
                    //case 0x1F:	case 0x2F:	case 0xAF:
					// 自動演奏チップ
                    //case 0x61:	case 0x62:	case 0x63:	case 0x64:	case 0x65:	case 0x66:	case 0x67:	case 0x68:	case 0x69:
                    //case 0x70:	case 0x71:	case 0x72:	case 0x73:	case 0x74:	case 0x75:	case 0x76:	case 0x77:	case 0x78:	case 0x79:
                    //case 0x80:	case 0x81:	case 0x82:	case 0x83:	case 0x84:	case 0x85:	case 0x86:	case 0x87:	case 0x88:	case 0x89:
                    //case 0x90:	case 0x91:	case 0x92:

						#region [ 発音1秒前のタイミングを算出 ]
						int n発音前余裕ms = 1000, n発音後余裕ms = 800;
						{
							int ch = pChip.nチャンネル番号 >> 4;
							if ( ch == 0x02 || ch == 0x0A )
							{
								n発音前余裕ms = 800;
								n発音前余裕ms = 500;
							}
							if ( ch == 0x06 || ch == 0x07 || ch == 0x08 || ch == 0x09 )
							{
								n発音前余裕ms = 200;
								n発音前余裕ms = 500;
							}
						}
						#endregion
						#region [ BGMチップならば即ミキサーに追加 ]
						//if ( pChip.nチャンネル番号 == 0x01 )	// BGMチップは即ミキサーに追加
						//{
						//    if ( listWAV.ContainsKey( pChip.n整数値_内部番号 ) )
						//    {
						//        CDTX.CWAV wc = CDTXMania.DTX.listWAV[ pChip.n整数値_内部番号 ];
						//        if ( wc.rSound[ 0 ] != null )
						//        {
						//            CDTXMania.Sound管理.AddMixer( wc.rSound[ 0 ] );	// BGMは多重再生しない仕様としているので、1個目だけミキサーに登録すればよい
						//        }
						//    }
						//}
						#endregion
						#region [ 発音1秒前のタイミングを算出 ]
						int nAddMixer時刻ms, nAddMixer位置 = 0;
//Debug.WriteLine("==================================================================");
//Debug.WriteLine( "Start: ch=" + pChip.nチャンネル番号.ToString("x2") + ", nWAV番号=" + pChip.n整数値 + ", time=" + pChip.n発声時刻ms + ", lasttime=" + listChip[ listChip.Count - 1 ].n発声時刻ms );
						t発声時刻msと発声位置を取得する( pChip.n発声時刻ms - n発音前余裕ms, out nAddMixer時刻ms, out nAddMixer位置 );
//Debug.WriteLine( "nAddMixer時刻ms=" + nAddMixer時刻ms + ",nAddMixer位置=" + nAddMixer位置 );

						CChip c_AddMixer = new CChip()
						{
							nチャンネル番号 = 0xDA,
							n整数値 = pChip.n整数値,
							n整数値_内部番号 = pChip.n整数値_内部番号,
							n発声時刻ms = nAddMixer時刻ms,
							n発声位置 = nAddMixer位置,
							b演奏終了後も再生が続くチップである = false
						};
						listAddMixerChannel.Add( c_AddMixer );
//Debug.WriteLine("listAddMixerChannel:" );
//DebugOut_CChipList( listAddMixerChannel );
						#endregion

						int duration = 0;
						if ( listWAV.ContainsKey( pChip.n整数値_内部番号 ) )
						{
							CDTX.CWAV wc = CDTXMania.DTX.listWAV[ pChip.n整数値_内部番号 ];
							double _db再生速度 = ( CDTXMania.DTXVmode.Enabled ) ? this.dbDTXVPlaySpeed : this.db再生速度;
							duration = ( wc.rSound[ 0 ] == null ) ? 0 : (int) ( wc.rSound[ 0 ].n総演奏時間ms / _db再生速度 );	// #23664 durationに再生速度が加味されておらず、低速再生でBGMが途切れる問題を修正 (発声時刻msは、DTX読み込み時に再生速度加味済)
						}
//Debug.WriteLine("duration=" + duration );
						int n新RemoveMixer時刻ms, n新RemoveMixer位置;
						t発声時刻msと発声位置を取得する( pChip.n発声時刻ms + duration + n発音後余裕ms, out n新RemoveMixer時刻ms, out n新RemoveMixer位置 );
//Debug.WriteLine( "n新RemoveMixer時刻ms=" + n新RemoveMixer時刻ms + ",n新RemoveMixer位置=" + n新RemoveMixer位置 );
						if ( n新RemoveMixer時刻ms < pChip.n発声時刻ms + duration )	// 曲の最後でサウンドが切れるような場合は
						{
							CChip c_AddMixer_noremove = c_AddMixer;
							c_AddMixer_noremove.b演奏終了後も再生が続くチップである = true;
							listAddMixerChannel[ listAddMixerChannel.Count - 1 ] = c_AddMixer_noremove;
							//continue;												// 発声位置の計算ができないので、Mixer削除をあきらめる___のではなく
																					// #32248 2013.10.15 yyagi 演奏終了後も再生を続けるチップであるというフラグをpChip内に立てる
							break;
						}
						#region [ 未使用コード ]
						//if ( n新RemoveMixer時刻ms < pChip.n発声時刻ms + duration )	// 曲の最後でサウンドが切れるような場合
						//{
						//    n新RemoveMixer時刻ms = pChip.n発声時刻ms + duration;
						//    // 「位置」は比例計算で求めてお茶を濁す...このやり方だと誤動作したため対応中止
						//    n新RemoveMixer位置 = listChip[ listChip.Count - 1 ].n発声位置 * n新RemoveMixer時刻ms / listChip[ listChip.Count - 1 ].n発声時刻ms;
						//}
						#endregion

						#region [ 発音終了2秒後にmixerから削除するが、その前に再発音することになるのかを確認(再発音ならmixer削除タイミングを延期) ]
						int n整数値 = pChip.n整数値;
						int index = listRemoveTiming.FindIndex(
							delegate( CChip cchip ) { return cchip.n整数値 == n整数値; }
						);
//Debug.WriteLine( "index=" + index );
						if ( index >= 0 )													// 過去に同じチップで発音中のものが見つかった場合
						{																	// 過去の発音のmixer削除を確定させるか、延期するかの2択。
							int n旧RemoveMixer時刻ms = listRemoveTiming[ index ].n発声時刻ms;
							int n旧RemoveMixer位置 = listRemoveTiming[ index ].n発声位置;

//Debug.WriteLine( "n旧RemoveMixer時刻ms=" + n旧RemoveMixer時刻ms + ",n旧RemoveMixer位置=" + n旧RemoveMixer位置 );
							if ( pChip.n発声時刻ms - n発音前余裕ms <= n旧RemoveMixer時刻ms )	// mixer削除前に、同じ音の再発音がある場合は、
							{																	// mixer削除時刻を遅延させる(if-else後に行う)
//Debug.WriteLine( "remove TAIL of listAddMixerChannel. TAIL INDEX=" + listAddMixerChannel.Count );
//DebugOut_CChipList( listAddMixerChannel );
								listAddMixerChannel.RemoveAt( listAddMixerChannel.Count - 1 );	// また、同じチップ音の「mixerへの再追加」は削除する
//Debug.WriteLine( "removed result:" );
//DebugOut_CChipList( listAddMixerChannel );
							}
							else															// 逆に、時間軸上、mixer削除後に再発音するような流れの場合は
							{
//Debug.WriteLine( "Publish the value(listRemoveTiming[index] to listRemoveMixerChannel." );
								listRemoveMixerChannel.Add( listRemoveTiming[ index ] );	// mixer削除を確定させる
//Debug.WriteLine( "listRemoveMixerChannel:" );
//DebugOut_CChipList( listRemoveMixerChannel );
								//listRemoveTiming.RemoveAt( index );
							}
							CChip c = new CChip()											// mixer削除時刻を更新(遅延)する
							{
								nチャンネル番号 = 0xDB,
								n整数値 = listRemoveTiming[ index ].n整数値,
								n整数値_内部番号 = listRemoveTiming[ index ].n整数値_内部番号,
								n発声時刻ms = n新RemoveMixer時刻ms,
								n発声位置 = n新RemoveMixer位置
							};
							listRemoveTiming[ index ] = c;
							//listRemoveTiming[ index ].n発声時刻ms = n新RemoveMixer時刻ms;	// mixer削除時刻を更新(遅延)する
							//listRemoveTiming[ index ].n発声位置 = n新RemoveMixer位置;
//Debug.WriteLine( "listRemoveTiming: modified" );
//DebugOut_CChipList( listRemoveTiming );
						}
						else																// 過去に同じチップを発音していないor
						{																	// 発音していたが既にmixer削除確定していたなら
							CChip c = new CChip()											// 新しくmixer削除候補として追加する
							{
								nチャンネル番号 = 0xDB,
								n整数値 = pChip.n整数値,
								n整数値_内部番号 = pChip.n整数値_内部番号,
								n発声時刻ms = n新RemoveMixer時刻ms,
								n発声位置 = n新RemoveMixer位置
							};
//Debug.WriteLine( "Add new chip to listRemoveMixerTiming: " );
//Debug.WriteLine( "ch=" + c.nチャンネル番号.ToString( "x2" ) + ", nWAV番号=" + c.n整数値 + ", time=" + c.n発声時刻ms + ", lasttime=" + listChip[ listChip.Count - 1 ].n発声時刻ms );
							listRemoveTiming.Add( c );
//Debug.WriteLine( "listRemoveTiming:" );
//DebugOut_CChipList( listRemoveTiming );
						}
						#endregion
						break;
				}
			}
//Debug.WriteLine("==================================================================");
//Debug.WriteLine( "Result:" );
//Debug.WriteLine( "listAddMixerChannel:" );
//DebugOut_CChipList( listAddMixerChannel );
//Debug.WriteLine( "listRemoveMixerChannel:" );
//DebugOut_CChipList( listRemoveMixerChannel );
//Debug.WriteLine( "listRemoveTiming:" );
//DebugOut_CChipList( listRemoveTiming );
//Debug.WriteLine( "==================================================================" );

			listChip.AddRange( listAddMixerChannel );
			listChip.AddRange( listRemoveMixerChannel );
			listChip.AddRange( listRemoveTiming );
			listChip.Sort();
		}
		private void DebugOut_CChipList( List<CChip> c )
		{
//Debug.WriteLine( "Count=" + c.Count );
			for ( int i = 0; i < c.Count; i++ )
			{
				Debug.WriteLine( i + ": ch=" + c[ i ].nチャンネル番号.ToString("x2") + ", WAV番号=" + c[ i ].n整数値 + ", time=" + c[ i ].n発声時刻ms );
			}
		}
		private bool t発声時刻msと発声位置を取得する( int n希望発声時刻ms, out int n新発声時刻ms, out int n新発声位置 )
		{
			// 発声時刻msから発声位置を逆算することはできないため、近似計算する。
			// 具体的には、希望発声位置前後の2つのチップの発声位置の中間を取る。

			if ( n希望発声時刻ms < 0 )
			{
				n希望発声時刻ms = 0;
			}
			//else if ( n希望発声時刻ms > listChip[ listChip.Count - 1 ].n発声時刻ms )		// BGMの最後の余韻を殺してしまうので、この条件は外す
			//{
			//    n希望発声時刻ms = listChip[ listChip.Count - 1 ].n発声時刻ms;
			//}

			int index_min = -1, index_max = -1;
			for ( int i = 0; i < listChip.Count; i++ )		// 希望発声位置前後の「前」の方のチップを検索
			{
				if ( listChip[ i ].n発声時刻ms >= n希望発声時刻ms )
				{
					index_min = i;
					break;
				}
			}
			if ( index_min < 0 )	// 希望発声時刻に至らずに曲が終了してしまう場合
			{
				// listの最終項目の時刻をそのまま使用する
								//___のではダメ。BGMが尻切れになる。
								// そこで、listの最終項目の発声時刻msと発生位置から、希望発声時刻に相当する希望発声位置を比例計算して求める。
				//n新発声時刻ms = n希望発声時刻ms;
				//n新発声位置 = listChip[ listChip.Count - 1 ].n発声位置 * n希望発声時刻ms / listChip[ listChip.Count - 1 ].n発声時刻ms;
				n新発声時刻ms = listChip[ listChip.Count - 1 ].n発声時刻ms;
				n新発声位置   = listChip[ listChip.Count - 1 ].n発声位置;
				return false;
			}
			index_max = index_min + 1;
			if ( index_max >= listChip.Count )
			{
				index_max = index_min;
			}
			n新発声時刻ms = ( listChip[ index_max ].n発声時刻ms + listChip[ index_min ].n発声時刻ms ) / 2;
			n新発声位置   = ( listChip[ index_max ].n発声位置   + listChip[ index_min ].n発声位置   ) / 2;

			return true;
		}

		public void SwapGuitarBassInfos()
		{
		}

		// SwapGuitarBassInfos_AutoFlags()は、CDTXからCConfigIniに移動。

		// CActivity 実装

		public override void On活性化()
		{
			this.listWAV = new Dictionary<int, CWAV>();
			this.listBPM = new Dictionary<int, CBPM>();
            this.listSCROLL = new Dictionary<int, CSCROLL>();
            this.listSCROLL_Normal = new Dictionary<int, CSCROLL>();
            this.listSCROLL_Expert = new Dictionary<int, CSCROLL>();
            this.listSCROLL_Master = new Dictionary<int, CSCROLL>();
            this.listJPOSSCROLL = new Dictionary<int,CJPOSSCROLL>();
            this.listDELAY = new Dictionary<int, CDELAY>();
            this.listBRANCH = new Dictionary<int, CBRANCH>();
			this.listAVI = new Dictionary<int, CAVI>();
			this.listAVIPAN = new Dictionary<int, CAVIPAN>();
            this.listDS = new Dictionary<int, CDirectShow>();
			this.listChip = new List<CChip>();
            this.listBalloon = new List<int>();
            this.listBalloon_Normal = new List<int>();
            this.listBalloon_Expert = new List<int>();
            this.listBalloon_Master = new List<int>();
            this.listLine = new List<CLine>();
            this.listLiryc = new List<string>();
			base.On活性化();
		}
		public override void On非活性化()
		{
			if( this.listWAV != null )
			{
				foreach( CWAV cwav in this.listWAV.Values )
				{
					cwav.Dispose();
				}
				this.listWAV = null;
			}
			if( this.listAVI != null )
			{
				foreach( CAVI cavi in this.listAVI.Values )
				{
					cavi.Dispose();
				}
				this.listAVI = null;
			}
			if( this.listAVIPAN != null )
			{
				this.listAVIPAN.Clear();
				this.listAVIPAN = null;
			}
            if( this.listDS != null )
            {
                foreach ( CDirectShow cds in this.listDS.Values )
                {
                    cds.Dispose();
                }
                this.listDS = null;
            }
			if( this.listBPM != null )
			{
				this.listBPM.Clear();
				this.listBPM = null;
			}
			if( this.listDELAY != null )
			{
				this.listDELAY.Clear();
				this.listDELAY = null;
			}
			if( this.listBRANCH != null )
			{
				this.listBRANCH.Clear();
				this.listBRANCH = null;
			}
			if( this.listSCROLL != null )
			{
				this.listSCROLL.Clear();
				this.listSCROLL = null;
			}

			if( this.listSCROLL_Normal != null )
			{
				this.listSCROLL_Normal.Clear();
				this.listSCROLL_Normal = null;
			}
			if( this.listSCROLL_Expert != null )
			{
				this.listSCROLL_Expert.Clear();
				this.listSCROLL_Expert = null;
			}
			if( this.listSCROLL_Master != null )
			{
				this.listSCROLL_Master.Clear();
				this.listSCROLL_Master = null;
			}
			if( this.listJPOSSCROLL != null )
			{
				this.listJPOSSCROLL.Clear();
				this.listJPOSSCROLL = null;
			}

			if( this.listChip != null )
			{
				this.listChip.Clear();
			}

			if( this.listBalloon != null )
			{
				this.listBalloon.Clear();
			}
			if( this.listBalloon_Normal != null )
			{
				this.listBalloon_Normal.Clear();
			}
			if( this.listBalloon_Expert != null )
			{
				this.listBalloon_Expert.Clear();
			}
			if( this.listBalloon_Master != null )
			{
				this.listBalloon_Master.Clear();
			}
			if( this.listLiryc != null )
			{
				this.listLiryc.Clear();
			}

			base.On非活性化();
		}
		public override void OnManagedリソースの作成()
		{
			if( !base.b活性化してない )
			{
				this.tAVIの読み込み();
				base.OnManagedリソースの作成();
			}
		}
		public override void OnManagedリソースの解放()
		{
			if( !base.b活性化してない )
			{
				if( this.listAVI != null )
				{
					foreach( CAVI cavi in this.listAVI.Values )
					{
						cavi.Dispose();
					}
				}
                if( this.listDS != null )
                {
                    foreach( CDirectShow cds in this.listDS.Values )
                    {
                        cds.Dispose();
                    }
                }
				base.OnManagedリソースの解放();
			}
		}

		// その他
		
		#region [ private ]
		//-----------------
		private bool bヘッダのみ;
		private Stack<bool> bstackIFからENDIFをスキップする;
	
		private int n現在の行数;
		private int n現在の乱数;

		private int nPolyphonicSounds = 4;							// #28228 2012.5.1 yyagi

		private int n内部番号BPM1to;
		private int n内部番号SCROLL1to;
		private int n内部番号JSCROLL1to;
		private int n内部番号DELAY1to;
		private int n内部番号BRANCH1to;
		private int n内部番号WAV1to;
		private int[] n無限管理BPM;
		private int[] n無限管理WAV;
		private int[] nRESULTIMAGE用優先順位;
		private int[] nRESULTMOVIE用優先順位;
		private int[] nRESULTSOUND用優先順位;

        private void t行のコメント処理( ref string strText )
        {
            int nCommentPos = strText.IndexOf("//");
            if( nCommentPos != -1 )
                strText = strText.Remove( nCommentPos );
        }

		private bool t入力_コマンド文字列を抜き出す( ref CharEnumerator ce, ref StringBuilder sb文字列 )
		{
			if( !this.t入力_空白をスキップする( ref ce ) )
				return false;	// 文字が尽きた

			#region [ コマンド終端文字(':')、半角空白、コメント開始文字(';')、改行のいずれかが出現するまでをコマンド文字列と見なし、sb文字列 にコピーする。]
			//-----------------
			while( ce.Current != ':' && ce.Current != ' ' && ce.Current != ';' && ce.Current != '\n' )
			{
				sb文字列.Append( ce.Current );

				if( !ce.MoveNext() )
					return false;	// 文字が尽きた
			}
			//-----------------
			#endregion

			#region [ コマンド終端文字(':')で終端したなら、その次から空白をスキップしておく。]
			//-----------------
			if( ce.Current == ':' )
			{
				if( !ce.MoveNext() )
					return false;	// 文字が尽きた

				if( !this.t入力_空白をスキップする( ref ce ) )
					return false;	// 文字が尽きた
			}
			//-----------------
			#endregion

			return true;
		}
		private bool t入力_コメントをスキップする( ref CharEnumerator ce )
		{
			// 改行が現れるまでをコメントと見なしてスキップする。

			while( ce.Current != '\n' )
			{
				if( !ce.MoveNext() )
					return false;	// 文字が尽きた
			}

			// 改行の次の文字へ移動した結果を返す。

			return ce.MoveNext();
		}
		private bool t入力_コメント文字列を抜き出す( ref CharEnumerator ce, ref StringBuilder sb文字列 )
		{
			if( ce.Current != ';' )		// コメント開始文字(';')じゃなければ正常帰還。
				return true;

			if( !ce.MoveNext() )		// ';' の次で文字列が終わってたら終了帰還。
				return false;

			#region [ ';' の次の文字から '\n' の１つ前までをコメント文字列と見なし、sb文字列にコピーする。]
			//-----------------
			while( ce.Current != '\n' )
			{
				sb文字列.Append( ce.Current );

				if( !ce.MoveNext() )
					return false;
			}
			//-----------------
			#endregion

			return true;
		}
		private void t入力_パラメータ食い込みチェック( string strコマンド名, ref string strコマンド, ref string strパラメータ )
		{
			if( ( strコマンド.Length > strコマンド名.Length ) && strコマンド.StartsWith( strコマンド名, StringComparison.OrdinalIgnoreCase ) )
			{
				strパラメータ = strコマンド.Substring( strコマンド名.Length ).Trim();
				strコマンド = strコマンド.Substring( 0, strコマンド名.Length );
			}
		}
		private bool t入力_パラメータ文字列を抜き出す( ref CharEnumerator ce, ref StringBuilder sb文字列 )
		{
			if( !this.t入力_空白をスキップする( ref ce ) )
				return false;	// 文字が尽きた

			#region [ 改行またはコメント開始文字(';')が出現するまでをパラメータ文字列と見なし、sb文字列 にコピーする。]
			//-----------------
			while( ce.Current != '\n' && ce.Current != ';' )
			{
				sb文字列.Append( ce.Current );

				if( !ce.MoveNext() )
					return false;
			}
			//-----------------
			#endregion

			return true;
		}
		private bool t入力_空白と改行をスキップする( ref CharEnumerator ce )
		{
			// 空白と改行が続く間はこれらをスキップする。

			while( ce.Current == ' ' || ce.Current == '\n' )
			{
				if( ce.Current == '\n' )
					this.n現在の行数++;		// 改行文字では行番号が増える。

				if( !ce.MoveNext() )
					return false;	// 文字が尽きた
			}

			return true;
		}
		private bool t入力_空白をスキップする( ref CharEnumerator ce )
		{
			// 空白が続く間はこれをスキップする。

			while( ce.Current == ' ' )
			{
				if( !ce.MoveNext() )
					return false;	// 文字が尽きた
			}

			return true;
		}
		private void t入力_行解析( ref StringBuilder sbコマンド, ref StringBuilder sbパラメータ, ref StringBuilder sbコメント )
		{
			string strコマンド = sbコマンド.ToString();
			string strパラメータ = sbパラメータ.ToString().Trim();
			string strコメント = sbコメント.ToString();

			// 行頭コマンドの処理

			#region [ IF ]
			//-----------------
			if( strコマンド.StartsWith( "IF", StringComparison.OrdinalIgnoreCase ) )
			{
				this.t入力_パラメータ食い込みチェック( "IF", ref strコマンド, ref strパラメータ );

				if( this.bstackIFからENDIFをスキップする.Count == 255 )
				{
					Trace.TraceWarning( "#IF の入れ子の数が 255 を超えました。この #IF を無視します。[{0}: {1}行]", this.strファイル名の絶対パス, this.n現在の行数 );
				}
				else if( this.bstackIFからENDIFをスキップする.Peek() )
				{
					this.bstackIFからENDIFをスキップする.Push( true );	// 親が true ならその入れ子も問答無用で true 。
				}
				else													// 親が false なら入れ子はパラメータと乱数を比較して結果を判断する。
				{
					int n数値 = 0;

					if( !int.TryParse( strパラメータ, out n数値 ) )
						n数値 = 1;

					this.bstackIFからENDIFをスキップする.Push( n数値 != this.n現在の乱数 );		// 乱数と数値が一致したら true 。
				}
			}
			//-----------------
			#endregion
			#region [ ENDIF ]
			//-----------------
			else if( strコマンド.StartsWith( "ENDIF", StringComparison.OrdinalIgnoreCase ) )
			{
				this.t入力_パラメータ食い込みチェック( "ENDIF", ref strコマンド, ref strパラメータ );

				if( this.bstackIFからENDIFをスキップする.Count > 1 )
				{
					this.bstackIFからENDIFをスキップする.Pop();		// 入れ子を１つ脱出。
				}
				else
				{
					Trace.TraceWarning( "#ENDIF に対応する #IF がありません。この #ENDIF を無視します。[{0}: {1}行]", this.strファイル名の絶対パス, this.n現在の行数 );
				}
			}
			//-----------------
			#endregion

			else if( !this.bstackIFからENDIFをスキップする.Peek() )		// IF～ENDIF をスキップするなら以下はすべて無視。
			{
				#region [ PATH_WAV ]
				//-----------------
				if( strコマンド.StartsWith( "PATH_WAV", StringComparison.OrdinalIgnoreCase ) )
				{
					this.t入力_パラメータ食い込みチェック( "PATH_WAV", ref strコマンド, ref strパラメータ );
					this.PATH_WAV = strパラメータ;
				}
				//-----------------
				#endregion
				#region [ TITLE ]
				//-----------------
				else if( strコマンド.StartsWith( "TITLE", StringComparison.OrdinalIgnoreCase ) )
				{
					//this.t入力_パラメータ食い込みチェック( "TITLE", ref strコマンド, ref strパラメータ );
					//this.TITLE = strパラメータ;
				}
				//-----------------
				#endregion
				#region [ ARTIST ]
				//-----------------
				else if( strコマンド.StartsWith( "ARTIST", StringComparison.OrdinalIgnoreCase ) )
				{
					this.t入力_パラメータ食い込みチェック( "ARTIST", ref strコマンド, ref strパラメータ );
					this.ARTIST = strパラメータ;
				}
				//-----------------
				#endregion
				#region [ COMMENT ]
				//-----------------
				else if( strコマンド.StartsWith( "COMMENT", StringComparison.OrdinalIgnoreCase ) )
				{
					this.t入力_パラメータ食い込みチェック( "COMMENT", ref strコマンド, ref strパラメータ );
					this.COMMENT = strパラメータ;
				}
				//-----------------
				#endregion
				#region [ GENRE ]
				//-----------------
				else if( strコマンド.StartsWith( "GENRE", StringComparison.OrdinalIgnoreCase ) )
				{
					this.t入力_パラメータ食い込みチェック( "GENRE", ref strコマンド, ref strパラメータ );
					this.GENRE = strパラメータ;
				}
				//-----------------
				#endregion
				#region [ HIDDENLEVEL ]
				//-----------------
				else if( strコマンド.StartsWith( "HIDDENLEVEL", StringComparison.OrdinalIgnoreCase ) )
				{
					this.t入力_パラメータ食い込みチェック( "HIDDENLEVEL", ref strコマンド, ref strパラメータ );
					this.HIDDENLEVEL = strパラメータ.ToLower().Equals( "on" );
				}
				//-----------------
				#endregion
				#region [ PREVIEW ]
				//-----------------
				else if( strコマンド.StartsWith( "PREVIEW", StringComparison.OrdinalIgnoreCase ) )
				{
					this.t入力_パラメータ食い込みチェック( "PREVIEW", ref strコマンド, ref strパラメータ );
					this.PREVIEW = strパラメータ;
				}
				//-----------------
				#endregion
				#region [ PREIMAGE ]
				//-----------------
				else if( strコマンド.StartsWith( "PREIMAGE", StringComparison.OrdinalIgnoreCase ) )
				{
					this.t入力_パラメータ食い込みチェック( "PREIMAGE", ref strコマンド, ref strパラメータ );
					this.PREIMAGE = strパラメータ;
				}
				//-----------------
				#endregion
				#region [ RANDOM ]
				//-----------------
				else if( strコマンド.StartsWith( "RANDOM", StringComparison.OrdinalIgnoreCase ) )
				{
					this.t入力_パラメータ食い込みチェック( "RANDOM", ref strコマンド, ref strパラメータ );

					int n数値 = 1;
					if( !int.TryParse( strパラメータ, out n数値 ) )
						n数値 = 1;

					this.n現在の乱数 = CDTXMania.Random.Next( n数値 ) + 1;		// 1～数値 までの乱数を生成。
				}
				//-----------------
				#endregion
				#region [ BPM ]
				//-----------------
				else if( strコマンド.StartsWith( "BPM", StringComparison.OrdinalIgnoreCase ) )
				{
					//this.t入力_行解析_BPM_BPMzz( strコマンド, strパラメータ, strコメント );
				}
				//-----------------
				#endregion
				#region [ DTXVPLAYSPEED ]
				//-----------------
				else if ( strコマンド.StartsWith( "DTXVPLAYSPEED", StringComparison.OrdinalIgnoreCase ) )
				{
					this.t入力_パラメータ食い込みチェック( "DTXVPLAYSPEED", ref strコマンド, ref strパラメータ );

					double dtxvplayspeed = 0.0;
					if ( TryParse( strパラメータ, out dtxvplayspeed ) && dtxvplayspeed > 0.0 )
					{
						this.dbDTXVPlaySpeed = dtxvplayspeed;
					}
				}
				//-----------------
				#endregion
				else if( !this.bヘッダのみ )		// ヘッダのみの解析の場合、以下は無視。
				{
					#region [ PANEL ]
					//-----------------
					if( strコマンド.StartsWith( "PANEL", StringComparison.OrdinalIgnoreCase ) )
					{
						this.t入力_パラメータ食い込みチェック( "PANEL", ref strコマンド, ref strパラメータ );

						int dummyResult;								// #23885 2010.12.12 yyagi: not to confuse "#PANEL strings (panel)" and "#PANEL int (panpot of EL)"
						if( !int.TryParse( strパラメータ, out dummyResult ) )
						{		// 数値じゃないならPANELとみなす
							this.PANEL = strパラメータ;							//
							goto EOL;									//
						}												// 数値ならPAN ELとみなす

					}
					//-----------------
					#endregion
					#region [ BASEBPM ]
					//-----------------
					else if( strコマンド.StartsWith( "BASEBPM", StringComparison.OrdinalIgnoreCase ) )
					{
						this.t入力_パラメータ食い込みチェック( "BASEBPM", ref strコマンド, ref strパラメータ );

						double basebpm = 0.0;
						//if( double.TryParse( str2, out num6 ) && ( num6 > 0.0 ) )
						if( TryParse( strパラメータ, out basebpm ) && basebpm > 0.0 )	// #23880 2010.12.30 yyagi: alternative TryParse to permit both '.' and ',' for decimal point
						{													// #24204 2011.01.21 yyagi: Fix the condition correctly
							this.BASEBPM = basebpm;
						}
					}
					//-----------------
					#endregion

					// オブジェクト記述コマンドの処理。

					else if( !this.t入力_行解析_WAVVOL_VOLUME( strコマンド, strパラメータ, strコメント ) &&
						!this.t入力_行解析_WAVPAN_PAN( strコマンド, strパラメータ, strコメント ) &&
						!this.t入力_行解析_WAV( strコマンド, strBGM_PATH != null ? strBGM_PATH : strパラメータ, strコメント ) &&
                        !this.t入力_行解析_AVIPAN( strコマンド, strパラメータ, strコメント ) &&
						!this.t入力_行解析_AVI_VIDEO( strコマンド, strパラメータ, strコメント ) &&
					//	!this.t入力_行解析_BPM_BPMzz( strコマンド, strパラメータ, strコメント ) &&	// bヘッダのみ==trueの場合でもチェックするよう変更
						!this.t入力_行解析_SIZE( strコマンド, strパラメータ, strコメント ) )
					{
						this.t入力_行解析_チップ配置( strコマンド, strパラメータ, strコメント );
					}
				EOL:
					Debug.Assert( true );		// #23885 2010.12.12 yyagi: dummy line to exit parsing the line
												// 2011.8.17 from: "int xx=0;" から変更。毎回警告が出るので。
				}
				//else
				//{	// Duration測定のため、bヘッダのみ==trueでも、チップ配置は行う
				//	this.t入力_行解析_チップ配置( strコマンド, strパラメータ, strコメント );
				//}
			}
		}
		private bool t入力_行解析_AVI_VIDEO( string strコマンド, string strパラメータ, string strコメント )
		{
			// (1) コマンドを処理。

			#region [ "AVI" or "VIDEO" で始まらないコマンドは無効。]
			//-----------------
			if( strコマンド.StartsWith( "AVI", StringComparison.OrdinalIgnoreCase ) )
				strコマンド = strコマンド.Substring( 3 );		// strコマンド から先頭の"AVI"文字を除去。

			else if( strコマンド.StartsWith( "VIDEO", StringComparison.OrdinalIgnoreCase ) )
				strコマンド = strコマンド.Substring( 5 );		// strコマンド から先頭の"VIDEO"文字を除去。

			else
				return false;
			//-----------------
			#endregion

			// (2) パラメータを処理。

			if( strコマンド.Length < 2 )
				return false;	// AVI番号 zz がないなら無効。

			#region [ AVI番号 zz を取得する。]
			//-----------------
			int zz = C変換.n36進数2桁の文字列を数値に変換して返す( strコマンド.Substring( 0, 2 ) );
			if( zz < 0 || zz >= 36 * 36 )
			{
				Trace.TraceError( "AVI(VIDEO)番号に 00～ZZ 以外の値または不正な文字列が指定されました。[{0}: {1}行]", this.strファイル名の絶対パス, this.n現在の行数 );
				return false;
			}
			//-----------------
			#endregion

			#region [ AVIリストに {zz, avi} の組を登録する。 ]
			//-----------------
			var avi = new CAVI() {
				n番号 = zz,
				strファイル名 = strパラメータ,
				strコメント文 = strコメント,
			};

			if( this.listAVI.ContainsKey( zz ) )	// 既にリスト中に存在しているなら削除。後のものが有効。
				this.listAVI.Remove( zz );

			this.listAVI.Add( zz, avi );

            var ds = new CDirectShow()
            {
                n番号 = zz,
                strファイル名 = strパラメータ,
                strコメント文 = strコメント,
            };

            if (this.listDS.ContainsKey(zz))	// 既にリスト中に存在しているなら削除。後のものが有効。
                this.listDS.Remove(zz);

            this.listDS.Add(zz, ds);
			//-----------------
			#endregion

			return true;
		}
		private bool t入力_行解析_AVIPAN( string strコマンド, string strパラメータ, string strコメント )
		{
			// (1) コマンドを処理。

			#region [ "AVIPAN" で始まらないコマンドは無効。]
			//-----------------
			if( !strコマンド.StartsWith( "AVIPAN", StringComparison.OrdinalIgnoreCase ) )
				return false;

			strコマンド = strコマンド.Substring( 6 );	// strコマンド から先頭の"AVIPAN"文字を除去。
			//-----------------
			#endregion

			// (2) パラメータを処理。

			if( strコマンド.Length < 2 )
				return false;	// AVIPAN番号 zz がないなら無効。

			#region [ AVIPAN番号 zz を取得する。]
			//-----------------
			int zz = C変換.n36進数2桁の文字列を数値に変換して返す( strコマンド.Substring( 0, 2 ) );
			if( zz < 0 || zz >= 36 * 36 )
			{
				Trace.TraceError( "AVIPAN番号に 00～ZZ 以外の値または不正な文字列が指定されました。[{0}: {1}行]", this.strファイル名の絶対パス, this.n現在の行数 );
				return false;
			}
			//-----------------
			#endregion

			var avipan = new CAVIPAN() {
				n番号 = zz,
			};

			// パラメータ引数（14個）を取得し、avipan に登録していく。

			string[] strParams = strパラメータ.Split( new char[] { ' ', ',', '(', ')', '[', ']', 'x', '|' }, StringSplitOptions.RemoveEmptyEntries );

			#region [ パラメータ引数は全14個ないと無効。]
			//-----------------
			if( strParams.Length < 14 )
			{
				Trace.TraceError( "AVIPAN: 引数が足りません。[{0}: {1}行]", this.strファイル名の絶対パス, this.n現在の行数 );
				return false;
			}
			//-----------------
			#endregion

			int i = 0;
			int n値 = 0;

			#region [ 1. AVI番号 ]
			//-----------------
			if( string.IsNullOrEmpty( strParams[ i ] ) || strParams[ i ].Length > 2 )
			{
				Trace.TraceError( "AVIPAN: {2}番目の数（AVI番号）が異常です。[{0}: {1}行]", this.strファイル名の絶対パス, this.n現在の行数, i + 1 );
				return false;
			}
			avipan.nAVI番号 = C変換.n36進数2桁の文字列を数値に変換して返す( strParams[ i ] );
			if( avipan.nAVI番号 < 1 || avipan.nAVI番号 >= 36 * 36 )
			{
				Trace.TraceError( "AVIPAN: {2}番目の数（AVI番号）が異常です。[{0}: {1}行]", this.strファイル名の絶対パス, this.n現在の行数, i + 1 );
				return false;
			}
			i++;
			//-----------------
			#endregion
			#region [ 2. 開始転送サイズ_幅 ]
			//-----------------
			n値 = 0;
			if( !int.TryParse( strParams[ i ], out n値 ) )
			{
				Trace.TraceError( "AVIPAN: {2}番目の引数（開始転送サイズ_幅）が異常です。[{0}: {1}行]", this.strファイル名の絶対パス, this.n現在の行数, i + 1 );
				return false;
			}
			avipan.sz開始サイズ.Width = n値;
			i++;
			//-----------------
			#endregion
			#region [ 3. 転送サイズ_高さ ]
			//-----------------
			n値 = 0;
			if( !int.TryParse( strParams[ i ], out n値 ) )
			{
				Trace.TraceError( "AVIPAN: {2}番目の引数（開始転送サイズ_高さ）が異常です。[{0}: {1}行]", this.strファイル名の絶対パス, this.n現在の行数, i + 1 );
				return false;
			}
			avipan.sz開始サイズ.Height = n値;
			i++;
			//-----------------
			#endregion
			#region [ 4. 終了転送サイズ_幅 ]
			//-----------------
			n値 = 0;
			if( !int.TryParse( strParams[ i ], out n値 ) )
			{
				Trace.TraceError( "AVIPAN: {2}番目の引数（終了転送サイズ_幅）が異常です。[{0}: {1}行]", this.strファイル名の絶対パス, this.n現在の行数, i + 1 );
				return false;
			}
			avipan.sz終了サイズ.Width = n値;
			i++;
			//-----------------
			#endregion
			#region [ 5. 終了転送サイズ_高さ ]
			//-----------------
			n値 = 0;
			if( !int.TryParse( strParams[ i ], out n値 ) )
			{
				Trace.TraceError( "AVIPAN: {2}番目の引数（終了転送サイズ_高さ）が異常です。[{0}: {1}行]", this.strファイル名の絶対パス, this.n現在の行数, i + 1 );
				return false;
			}
			avipan.sz終了サイズ.Height = n値;
			i++;
			//-----------------
			#endregion
			#region [ 6. 動画側開始位置_X ]
			//-----------------
			n値 = 0;
			if( !int.TryParse( strParams[ i ], out n値 ) )
			{
				Trace.TraceError( "AVIPAN: {2}番目の引数（動画側開始位置_X）が異常です。[{0}: {1}行]", this.strファイル名の絶対パス, this.n現在の行数, i + 1 );
				return false;
			}
			avipan.pt動画側開始位置.X = n値;
			i++;
			//-----------------
			#endregion
			#region [ 7. 動画側開始位置_Y ]
			//-----------------
			n値 = 0;
			if( !int.TryParse( strParams[ i ], out n値 ) )
			{
				Trace.TraceError( "AVIPAN: {2}番目の引数（動画側開始位置_Y）が異常です。[{0}: {1}行]", this.strファイル名の絶対パス, this.n現在の行数, i + 1 );
				return false;
			}
			avipan.pt動画側開始位置.Y = n値;
			i++;
			//-----------------
			#endregion
			#region [ 8. 動画側終了位置_X ]
			//-----------------
			n値 = 0;
			if( !int.TryParse( strParams[ i ], out n値 ) )
			{
				Trace.TraceError( "AVIPAN: {2}番目の引数（動画側終了位置_X）が異常です。[{0}: {1}行]", this.strファイル名の絶対パス, this.n現在の行数, i + 1 );
				return false;
			}
			avipan.pt動画側終了位置.X = n値;
			i++;
			//-----------------
			#endregion
			#region [ 9. 動画側終了位置_Y ]
			//-----------------
			n値 = 0;
			if( !int.TryParse( strParams[ i ], out n値 ) )
			{
				Trace.TraceError( "AVIPAN: {2}番目の引数（動画側終了位置_Y）が異常です。[{0}: {1}行]", this.strファイル名の絶対パス, this.n現在の行数, i + 1 );
				return false;
			}
			avipan.pt動画側終了位置.Y = n値;
			i++;
			//-----------------
			#endregion
			#region [ 10.表示側開始位置_X ]
			//-----------------
			n値 = 0;
			if( !int.TryParse( strParams[ i ], out n値 ) )
			{
				Trace.TraceError( "AVIPAN: {2}番目の引数（表示側開始位置_X）が異常です。[{0}: {1}行]", this.strファイル名の絶対パス, this.n現在の行数, i + 1 );
				return false;
			}
			avipan.pt表示側開始位置.X = n値;
			i++;
			//-----------------
			#endregion
			#region [ 11.表示側開始位置_Y ]
			//-----------------
			n値 = 0;
			if( !int.TryParse( strParams[ i ], out n値 ) )
			{
				Trace.TraceError( "AVIPAN: {2}番目の引数（表示側開始位置_Y）が異常です。[{0}: {1}行]", this.strファイル名の絶対パス, this.n現在の行数, i + 1 );
				return false;
			}
			avipan.pt表示側開始位置.Y = n値;
			i++;
			//-----------------
			#endregion
			#region [ 12.表示側終了位置_X ]
			//-----------------
			n値 = 0;
			if( !int.TryParse( strParams[ i ], out n値 ) )
			{
				Trace.TraceError( "AVIPAN: {2}番目の引数（表示側終了位置_X）が異常です。[{0}: {1}行]", this.strファイル名の絶対パス, this.n現在の行数, i + 1 );
				return false;
			}
			avipan.pt表示側終了位置.X = n値;
			i++;
			//-----------------
			#endregion
			#region [ 13.表示側終了位置_Y ]
			//-----------------
			n値 = 0;
			if( !int.TryParse( strParams[ i ], out n値 ) )
			{
				Trace.TraceError( "AVIPAN: {2}番目の引数（表示側終了位置_Y）が異常です。[{0}: {1}行]", this.strファイル名の絶対パス, this.n現在の行数, i + 1 );
				return false;
			}
			avipan.pt表示側終了位置.Y = n値;
			i++;
			//-----------------
			#endregion
			#region [ 14.移動時間 ]
			//-----------------
			n値 = 0;
			if( !int.TryParse( strParams[ i ], out n値 ) )
			{
				Trace.TraceError( "AVIPAN: {2}番目の引数（移動時間）が異常です。[{0}: {1}行]", this.strファイル名の絶対パス, this.n現在の行数, i + 1 );
				return false;
			}

			if( n値 < 0 )
				n値 = 0;

			avipan.n移動時間ct = n値;
			i++;
			//-----------------
			#endregion

			#region [ AVIPANリストに {zz, avipan} の組を登録する。]
			//-----------------
			if( this.listAVIPAN.ContainsKey( zz ) )	// 既にリスト中に存在しているなら削除。後のものが有効。
				this.listAVIPAN.Remove( zz );

			this.listAVIPAN.Add( zz, avipan );
			//-----------------
			#endregion

			return true;
		}
		private bool t入力_行解析_BPM_BPMzz( string strコマンド, string strパラメータ, string strコメント )
		{
			// (1) コマンドを処理。

			#region [ "BPM" で始まらないコマンドは無効。]
			//-----------------
			if( !strコマンド.StartsWith( "BPM", StringComparison.OrdinalIgnoreCase ) )
				return false;

			strコマンド = strコマンド.Substring( 3 );	// strコマンド から先頭の"BPM"文字を除去。
			//-----------------
			#endregion

			// (2) パラメータを処理。

			int zz = 0;

			#region [ BPM番号 zz を取得する。]
			//-----------------
			if( strコマンド.Length < 2 )
			{
				#region [ (A) "#BPM:" の場合 → zz = 00 ]
				//-----------------
				zz = 0;
				//-----------------
				#endregion
			}
			else
			{
				#region [ (B) "#BPMzz:" の場合 → zz = 00 ～ ZZ ]
				//-----------------
				zz = C変換.n36進数2桁の文字列を数値に変換して返す( strコマンド.Substring( 0, 2 ) );
				if( zz < 0 || zz >= 36 * 36 )
				{
					Trace.TraceError( "BPM番号に 00～ZZ 以外の値または不正な文字列が指定されました。[{0}: {1}行]", this.strファイル名の絶対パス, this.n現在の行数 );
					return false;
				}
				//-----------------
				#endregion
			}
			//-----------------
			#endregion

			double dbBPM = 0.0;

			#region [ BPM値を取得する。]
			//-----------------
			//if( !double.TryParse( strパラメータ, out result ) )
			if( !TryParse( strパラメータ, out dbBPM ) )			// #23880 2010.12.30 yyagi: alternative TryParse to permit both '.' and ',' for decimal point
				return false;

			if( dbBPM <= 0.0 )
				return false;
			//-----------------
			#endregion

			if( zz == 0 )			// "#BPM00:" と "#BPM:" は等価。
				this.BPM = dbBPM;	// この曲の代表 BPM に格納する。

			#region [ BPMリストに {内部番号, zz, dbBPM} の組を登録。]
			//-----------------
			this.listBPM.Add(
				this.n内部番号BPM1to,
				new CBPM() {
					n内部番号 = this.n内部番号BPM1to,
					n表記上の番号 = zz,
					dbBPM値 = dbBPM,
				} );
			//-----------------
			#endregion

			#region [ BPM番号が zz であるBPM未設定のBPMチップがあれば、そのサイズを変更する。無限管理に対応。]
			//-----------------
			if( this.n無限管理BPM[ zz ] == -zz )	// 初期状態では n無限管理BPM[zz] = -zz である。この場合、#BPMzz がまだ出現していないことを意味する。
			{
				for( int i = 0; i < this.listChip.Count; i++ )	// これまでに出てきたチップのうち、該当する（BPM値が未設定の）BPMチップの値を変更する（仕組み上、必ず後方参照となる）。
				{
					var chip = this.listChip[ i ];

					if( chip.bBPMチップである && chip.n整数値_内部番号 == -zz )	// #BPMzz 行より前の行に出現した #BPMzz では、整数値_内部番号は -zz に初期化されている。
						chip.n整数値_内部番号 = this.n内部番号BPM1to;
				}
			}
			this.n無限管理BPM[ zz ] = this.n内部番号BPM1to;			// 次にこの BPM番号 zz を使うBPMチップが現れたら、このBPM値が格納されることになる。
			this.n内部番号BPM1to++;		// 内部番号は単純増加連番。
			//-----------------
			#endregion

			return true;
		}

		private bool t入力_行解析_SIZE( string strコマンド, string strパラメータ, string strコメント )
		{
			// (1) コマンドを処理。

			#region [ "SIZE" で始まらないコマンドや、その後ろに2文字（番号）が付随してないコマンドは無効。]
			//-----------------
			if( !strコマンド.StartsWith( "SIZE", StringComparison.OrdinalIgnoreCase ) )
				return false;

			strコマンド = strコマンド.Substring( 4 );	// strコマンド から先頭の"SIZE"文字を除去。

			if( strコマンド.Length < 2 )	// サイズ番号の指定がない場合は無効。
				return false;
			//-----------------
			#endregion

			#region [ nWAV番号（36進数2桁）を取得。]
			//-----------------
			int nWAV番号 = C変換.n36進数2桁の文字列を数値に変換して返す( strコマンド.Substring( 0, 2 ) );

			if( nWAV番号 < 0 || nWAV番号 >= 36 * 36 )
			{
				Trace.TraceError( "SIZEのWAV番号に 00～ZZ 以外の値または不正な文字列が指定されました。[{0}: {1}行]", this.strファイル名の絶対パス, this.n現在の行数 );
				return false;
			}
			//-----------------
			#endregion


			// (2) パラメータを処理。

			#region [ nサイズ値 を取得する。値は 0～100 に収める。]
			//-----------------
			int nサイズ値;

			if( !int.TryParse( strパラメータ, out nサイズ値 ) )
				return true;	// int変換に失敗しても、この行自体の処理は終えたのでtrueを返す。

			nサイズ値 = Math.Min( Math.Max( nサイズ値, 0 ), 100 );	// 0未満は0、100超えは100に強制変換。
			//-----------------
			#endregion

			#region [ nWAV番号で示されるサイズ未設定のWAVチップがあれば、そのサイズを変更する。無限管理に対応。]
			//-----------------
			//-----------------
			#endregion

			return true;
		}
		private bool t入力_行解析_WAV( string strコマンド, string strパラメータ, string strコメント )
		{
			// (1) コマンドを処理。

			#region [ "WAV" で始まらないコマンドは無効。]
			//-----------------
			if( !strコマンド.StartsWith( "WAV", StringComparison.OrdinalIgnoreCase ) )
				return false;

			strコマンド = strコマンド.Substring( 3 );	// strコマンド から先頭の"WAV"文字を除去。
			//-----------------
			#endregion

			// (2) パラメータを処理。

			if( strコマンド.Length < 2 )
				return false;	// WAV番号 zz がないなら無効。

			#region [ WAV番号 zz を取得する。]
			//-----------------
			int zz = C変換.n36進数2桁の文字列を数値に変換して返す( strコマンド.Substring( 0, 2 ) );
			if( zz < 0 || zz >= 36 * 36 )
			{
				Trace.TraceError( "WAV番号に 00～ZZ 以外の値または不正な文字列が指定されました。[{0}: {1}行]", this.strファイル名の絶対パス, this.n現在の行数 );
				return false;
			}
			//-----------------
			#endregion

			var wav = new CWAV() {
				n内部番号 = this.n内部番号WAV1to,
				n表記上の番号 = zz,
				strファイル名 = strパラメータ,
				strコメント文 = strコメント,
			};

			#region [ WAVリストに {内部番号, wav} の組を登録。]
			//-----------------
			this.listWAV.Add( this.n内部番号WAV1to, wav );
			//-----------------
			#endregion

			#region [ WAV番号が zz である内部番号未設定のWAVチップがあれば、その内部番号を変更する。無限管理対応。]
			//-----------------
			if( this.n無限管理WAV[ zz ] == -zz )	// 初期状態では n無限管理WAV[zz] = -zz である。この場合、#WAVzz がまだ出現していないことを意味する。
			{
				for( int i = 0; i < this.listChip.Count; i++ )	// これまでに出てきたチップのうち、該当する（内部番号が未設定の）WAVチップの値を変更する（仕組み上、必ず後方参照となる）。
				{
					var chip = this.listChip[ i ];

					if( chip.bWAVを使うチャンネルである && ( chip.n整数値_内部番号 == -zz ) )	// この #WAVzz 行より前の行に出現した #WAVzz では、整数値_内部番号は -zz に初期化されている。
						chip.n整数値_内部番号 = this.n内部番号WAV1to;
				}
			}
			this.n無限管理WAV[ zz ] = this.n内部番号WAV1to;			// 次にこの WAV番号 zz を使うWAVチップが現れたら、この内部番号が格納されることになる。
			this.n内部番号WAV1to++;		// 内部番号は単純増加連番。
			//-----------------
			#endregion

			return true;
		}
		private bool t入力_行解析_WAVPAN_PAN( string strコマンド, string strパラメータ, string strコメント )
		{
			// (1) コマンドを処理。

			#region [ "WAVPAN" or "PAN" で始まらないコマンドは無効。]
			//-----------------
			if( strコマンド.StartsWith( "WAVPAN", StringComparison.OrdinalIgnoreCase ) )
				strコマンド = strコマンド.Substring( 6 );		// strコマンド から先頭の"WAVPAN"文字を除去。

			else if( strコマンド.StartsWith( "PAN", StringComparison.OrdinalIgnoreCase ) )
				strコマンド = strコマンド.Substring( 3 );		// strコマンド から先頭の"PAN"文字を除去。

			else
				return false;
			//-----------------
			#endregion

			// (2) パラメータを処理。

			if( strコマンド.Length < 2 )
				return false;	// WAV番号 zz がないなら無効。

			#region [ WAV番号 zz を取得する。]
			//-----------------
			int zz = C変換.n36進数2桁の文字列を数値に変換して返す( strコマンド.Substring( 0, 2 ) );
			if( zz < 0 || zz >= 36 * 36 )
			{
				Trace.TraceError( "WAVPAN(PAN)のWAV番号に 00～ZZ 以外の値または不正な文字列が指定されました。[{0}: {1}行]", this.strファイル名の絶対パス, this.n現在の行数 );
				return false;
			}
			//-----------------
			#endregion

			#region [ WAV番号 zz を持つWAVチップの位置を変更する。無限定義対応。]
			//-----------------
			int n位置;
			if( int.TryParse( strパラメータ, out n位置 ) )
			{
				n位置 = Math.Min( Math.Max( n位置, -100 ), 100 );	// -100～+100 に丸める
			}
			//-----------------
			#endregion

			return true;
		}
		private bool t入力_行解析_WAVVOL_VOLUME( string strコマンド, string strパラメータ, string strコメント )
		{
			// (1) コマンドを処理。

			#region [ "WAVCOL" or "VOLUME" で始まらないコマンドは無効。]
			//-----------------
			if( strコマンド.StartsWith( "WAVVOL", StringComparison.OrdinalIgnoreCase ) )
				strコマンド = strコマンド.Substring( 6 );		// strコマンド から先頭の"WAVVOL"文字を除去。

			else if( strコマンド.StartsWith( "VOLUME", StringComparison.OrdinalIgnoreCase ) )
				strコマンド = strコマンド.Substring( 6 );		// strコマンド から先頭の"VOLUME"文字を除去。

			else
				return false;
			//-----------------
			#endregion

			// (2) パラメータを処理。

			if( strコマンド.Length < 2 )
				return false;	// WAV番号 zz がないなら無効。

			#region [ WAV番号 zz を取得する。]
			//-----------------
			int zz = C変換.n36進数2桁の文字列を数値に変換して返す( strコマンド.Substring( 0, 2 ) );
			if( zz < 0 || zz >= 36 * 36 )
			{
				Trace.TraceError( "WAV番号に 00～ZZ 以外の値または不正な文字列が指定されました。[{0}: {1}行]", this.strファイル名の絶対パス, this.n現在の行数 );
				return false;
			}
			//-----------------
			#endregion

			#region [ WAV番号 zz を持つWAVチップの音量を変更する。無限定義対応。]
			//-----------------
			int n音量;
			if( int.TryParse( strパラメータ, out n音量 ) )
			{
				n音量 = Math.Min( Math.Max( n音量, 0 ), 100 );	// 0～100に丸める。
			}
			//-----------------
			#endregion

			return true;
		}
		private bool t入力_行解析_チップ配置( string strコマンド, string strパラメータ, string strコメント )
		{
			// (1) コマンドを処理。

			if( strコマンド.Length != 5 )	// コマンドは必ず5文字であること。
				return false;

			#region [ n小節番号 を取得する。]
			//-----------------
			int n小節番号 = C変換.n小節番号の文字列3桁を数値に変換して返す( strコマンド.Substring( 0, 3 ) );
			if( n小節番号 < 0 )
				return false;

			n小節番号++;	// 先頭に空の1小節を設ける。
			//-----------------
			#endregion

			#region [ nチャンネル番号 を取得する。]
			//-----------------
			int nチャンネル番号 = -1;

			
			nチャンネル番号 = C変換.n16進数2桁の文字列を数値に変換して返す( strコマンド.Substring( 3, 2 ) );

			if( nチャンネル番号 < 0 )
				return false;
			//-----------------
			#endregion
			#region [ 取得したチャンネル番号で、this.bチップがある に該当があれば設定する。]
			//-----------------
			if( ( nチャンネル番号 >= 0x11 ) && ( nチャンネル番号 <= 0x1a ) )
			{
				this.bチップがある.Drums = true;
			}
			else if( ( nチャンネル番号 >= 0x20 ) && ( nチャンネル番号 <= 0x27 ) )
			{
				this.bチップがある.Guitar = true;
			}
			else if( ( nチャンネル番号 >= 0xA0 ) && ( nチャンネル番号 <= 0xa7 ) )
			{
				this.bチップがある.Bass = true;
			}
			switch( nチャンネル番号 )
			{
				case 0x18:
					this.bチップがある.HHOpen = true;
					break;

				case 0x19:
					this.bチップがある.Ride = true;
					break;

				case 0x1a:
					this.bチップがある.LeftCymbal = true;
					break;

				case 0x20:
					this.bチップがある.OpenGuitar = true;
					break;

				case 0xA0:
					this.bチップがある.OpenBass = true;
					break;
			}
			//-----------------
			#endregion


			// (2) Ch.02を処理。

			#region [ 小節長変更(Ch.02)は他のチャンネルとはパラメータが特殊なので、先にとっとと終わらせる。 ]
			//-----------------
			if( nチャンネル番号 == 0x02 )
			{
				// 小節長倍率を取得する。

				double db小節長倍率 = 1.0;
				//if( !double.TryParse( strパラメータ, out result ) )
				if( !this.TryParse( strパラメータ, out db小節長倍率 ) )			// #23880 2010.12.30 yyagi: alternative TryParse to permit both '.' and ',' for decimal point
				{
					Trace.TraceError( "小節長倍率に不正な値を指定しました。[{0}: {1}行]", this.strファイル名の絶対パス, this.n現在の行数 );
					return false;
				}

				// 小節長倍率チップを配置する。

				this.listChip.Insert(
					0,
					new CChip() {
						nチャンネル番号 = nチャンネル番号,
						db実数値 = db小節長倍率,
						n発声位置 = n小節番号 * 384,
					} );

				return true;	// 配置終了。
			}
			//-----------------
			#endregion


			// (3) パラメータを処理。

			if( string.IsNullOrEmpty( strパラメータ ) )		// パラメータはnullまたは空文字列ではないこと。
				return false;

			#region [ strパラメータ にオブジェクト記述を格納し、その n文字数 をカウントする。]
			//-----------------
			int n文字数 = 0;

			var sb = new StringBuilder( strパラメータ.Length );

			// strパラメータを先頭から1文字ずつ見ながら正規化（無効文字('_')を飛ばしたり不正な文字でエラーを出したり）し、sb へ格納する。

			CharEnumerator ce = strパラメータ.GetEnumerator();
			while( ce.MoveNext() )
			{
				if( ce.Current == '_' )		// '_' は無視。
					continue;

				if( C変換.str36進数文字.IndexOf( ce.Current ) < 0 )	// オブジェクト記述は36進数文字であること。
				{
					Trace.TraceError( "不正なオブジェクト指定があります。[{0}: {1}行]", this.strファイル名の絶対パス, this.n現在の行数 );
					return false;
				}

				sb.Append( ce.Current );
				n文字数++;
			}

			strパラメータ = sb.ToString();	// 正規化された文字列になりました。

			if( ( n文字数 % 2 ) != 0 )		// パラメータの文字数が奇数の場合、最後の1文字を無視する。
				n文字数--;
			//-----------------
			#endregion


			// (4) パラメータをオブジェクト数値に分解して配置する。

			for( int i = 0; i < ( n文字数 / 2 ); i++ )	// 2文字で1オブジェクト数値
			{
				#region [ nオブジェクト数値 を１つ取得する。'00' なら無視。]
				//-----------------
				int nオブジェクト数値 = 0;

				if( nチャンネル番号 == 0x03 )
				{
					// Ch.03 のみ 16進数2桁。
					nオブジェクト数値 = C変換.n16進数2桁の文字列を数値に変換して返す( strパラメータ.Substring( i * 2, 2 ) );
				}
				else
				{
					// その他のチャンネルは36進数2桁。
					nオブジェクト数値 = C変換.n36進数2桁の文字列を数値に変換して返す( strパラメータ.Substring( i * 2, 2 ) );
				}

				if( nオブジェクト数値 == 0x00 )
					continue;
				//-----------------
				#endregion

				// オブジェクト数値に対応するチップを生成。

				var chip = new CChip();

				chip.nチャンネル番号 = nチャンネル番号;
				chip.n発声位置 = ( n小節番号 * 384 ) + ( ( 384 * i ) / ( n文字数 / 2 ) );
				chip.n整数値 = nオブジェクト数値;
				chip.n整数値_内部番号 = nオブジェクト数値;

				#region [ chip.e楽器パート = ... ]
				//-----------------
				if( ( nチャンネル番号 >= 0x11 ) && ( nチャンネル番号 <= 0x1C ) )
				{
					chip.e楽器パート = E楽器パート.DRUMS;
				}
				if( ( nチャンネル番号 >= 0x20 ) && ( nチャンネル番号 <= 0x27 ) )
				{
					chip.e楽器パート = E楽器パート.GUITAR;
				}
				if( ( nチャンネル番号 >= 160 ) && ( nチャンネル番号 <= 0xA7 ) )
				{
					chip.e楽器パート = E楽器パート.BASS;
				}
				//-----------------
				#endregion

				#region [ 無限定義への対応 → 内部番号の取得。]
				//-----------------
				if( chip.bWAVを使うチャンネルである )
				{
					chip.n整数値_内部番号 = this.n無限管理WAV[ nオブジェクト数値 ];	// これが本当に一意なWAV番号となる。（無限定義の場合、chip.n整数値 は一意である保証がない。）
				}
				else if( chip.bBPMチップである )
				{
					chip.n整数値_内部番号 = this.n無限管理BPM[ nオブジェクト数値 ];	// これが本当に一意なBPM番号となる。（同上。）
				}
				//-----------------
				#endregion

				#region [ フィルインON/OFFチャンネル(Ch.53)の場合、発声位置を少し前後にずらす。]
				//-----------------
				if( nチャンネル番号 == 0x53 )
				{
					// ずらすのは、フィルインONチップと同じ位置にいるチップでも確実にフィルインが発動し、
					// 同様に、フィルインOFFチップと同じ位置にいるチップでも確実にフィルインが終了するようにするため。

					if( ( nオブジェクト数値 > 0 ) && ( nオブジェクト数値 != 2 ) )
					{
						chip.n発声位置 -= 32;	// 384÷32＝12 ということで、フィルインONチップは12分音符ほど前へ移動。
					}
					else if( nオブジェクト数値 == 2 )
					{
						chip.n発声位置 += 32;	// 同じく、フィルインOFFチップは12分音符ほど後ろへ移動。
					}
				}
				//-----------------
				#endregion

				// チップを配置。

				this.listChip.Add( chip );
			}
			return true;
		}
		#region [#23880 2010.12.30 yyagi: コンマとスペースの両方を小数点として扱うTryParse]
		/// <summary>
		/// 小数点としてコンマとピリオドの両方を受け付けるTryParse()
		/// </summary>
		/// <param name="s">strings convert to double</param>
		/// <param name="result">parsed double value</param>
		/// <returns>s が正常に変換された場合は true。それ以外の場合は false。</returns>
		/// <exception cref="ArgumentException">style が NumberStyles 値でないか、style に NumberStyles.AllowHexSpecifier 値が含まれている</exception>
		private bool TryParse(string s, out double result) {	// #23880 2010.12.30 yyagi: alternative TryParse to permit both '.' and ',' for decimal point
																// EU諸国での #BPM 123,45 のような記述に対応するため、
																// 小数点の最終位置を検出して、それをlocaleにあった
																// 文字に置き換えてからTryParse()する
																// 桁区切りの文字はスキップする

			const string DecimalSeparators = ".,";				// 小数点文字
			const string GroupSeparators = ".,' ";				// 桁区切り文字
			const string NumberSymbols = "0123456789";			// 数値文字

			int len = s.Length;									// 文字列長
			int decimalPosition = len;							// 真の小数点の位置 最初は文字列終端位置に仮置きする

			for (int i = 0; i < len; i++) {							// まず、真の小数点(一番最後に現れる小数点)の位置を求める
				char c = s[i];
				if (NumberSymbols.IndexOf(c) >= 0) {				// 数値だったらスキップ
					continue;
				} else if (DecimalSeparators.IndexOf(c) >= 0) {		// 小数点文字だったら、その都度位置を上書き記憶
					decimalPosition = i;
				} else if (GroupSeparators.IndexOf(c) >= 0) {		// 桁区切り文字の場合もスキップ
					continue;
				} else {											// 数値_小数点_区切り文字以外がきたらループ終了
					break;
				}
			}

			StringBuilder decimalStr = new StringBuilder(16);
			for (int i = 0; i < len; i++) {							// 次に、localeにあった数値文字列を生成する
				char c = s[i];
				if (NumberSymbols.IndexOf(c) >= 0) {				// 数値だったら
					decimalStr.Append(c);							// そのままコピー
				} else if (DecimalSeparators.IndexOf(c) >= 0) {		// 小数点文字だったら
					if (i == decimalPosition) {						// 最後に出現した小数点文字なら、localeに合った小数点を出力する
						decimalStr.Append(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
					}
				} else if (GroupSeparators.IndexOf(c) >= 0) {		// 桁区切り文字だったら
					continue;										// 何もしない(スキップ)
				} else {
					break;
				}
			}
			return double.TryParse(decimalStr.ToString(), out result);	// 最後に、自分のlocale向けの文字列に対してTryParse実行
		}
        #endregion
        //-----------------
        #endregion

        #region[ tja譜面読み込みエンジンV5(WIP) ]
        // CDTX→CTJA[5]
        // CDTXMania→CTJA[2]
        // 基本的にCTJAはCDTXで生成したものをCDTXMania直下のCTJAに複製するような仕組みになる予定
        // 構造的に継承が一切不可能である命令についてはCDTX、各難易度ごとの命令はCTJA側に持たせる。


        #endregion
    }
}
