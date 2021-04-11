﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;
using System.Web;
using FDK;

namespace DTXMania
{
	internal class CConfigIni
	{
		// クラス

		#region [ CKeyAssign ]
		public class CKeyAssign
		{
			public class CKeyAssignPad
			{
				public CConfigIni.CKeyAssign.STKEYASSIGN[] HH
				{
					get
					{
						return this.padHH_R;
					}
					set
					{
						this.padHH_R = value;
					}
				}
				public CConfigIni.CKeyAssign.STKEYASSIGN[] R
				{
					get
					{
						return this.padHH_R;
					}
					set
					{
						this.padHH_R = value;
					}
				}
				public CConfigIni.CKeyAssign.STKEYASSIGN[] SD
				{
					get
					{
						return this.padSD_G;
					}
					set
					{
						this.padSD_G = value;
					}
				}
				public CConfigIni.CKeyAssign.STKEYASSIGN[] G
				{
					get
					{
						return this.padSD_G;
					}
					set
					{
						this.padSD_G = value;
					}
				}
				public CConfigIni.CKeyAssign.STKEYASSIGN[] BD
				{
					get
					{
						return this.padBD_B;
					}
					set
					{
						this.padBD_B = value;
					}
				}
				public CConfigIni.CKeyAssign.STKEYASSIGN[] B
				{
					get
					{
						return this.padBD_B;
					}
					set
					{
						this.padBD_B = value;
					}
				}
				public CConfigIni.CKeyAssign.STKEYASSIGN[] HT
				{
					get
					{
						return this.padHT_Pick;
					}
					set
					{
						this.padHT_Pick = value;
					}
				}
				public CConfigIni.CKeyAssign.STKEYASSIGN[] Pick
				{
					get
					{
						return this.padHT_Pick;
					}
					set
					{
						this.padHT_Pick = value;
					}
				}
				public CConfigIni.CKeyAssign.STKEYASSIGN[] LT
				{
					get
					{
						return this.padLT_Wail;
					}
					set
					{
						this.padLT_Wail = value;
					}
				}
				public CConfigIni.CKeyAssign.STKEYASSIGN[] Wail
				{
					get
					{
						return this.padLT_Wail;
					}
					set
					{
						this.padLT_Wail = value;
					}
				}
				public CConfigIni.CKeyAssign.STKEYASSIGN[] FT
				{
					get
					{
						return this.padFT_Cancel;
					}
					set
					{
						this.padFT_Cancel = value;
					}
				}
				public CConfigIni.CKeyAssign.STKEYASSIGN[] Cancel
				{
					get
					{
						return this.padFT_Cancel;
					}
					set
					{
						this.padFT_Cancel = value;
					}
				}
				public CConfigIni.CKeyAssign.STKEYASSIGN[] CY
				{
					get
					{
						return this.padCY_Decide;
					}
					set
					{
						this.padCY_Decide = value;
					}
				}
				public CConfigIni.CKeyAssign.STKEYASSIGN[] Decide
				{
					get
					{
						return this.padCY_Decide;
					}
					set
					{
						this.padCY_Decide = value;
					}
				}
				public CConfigIni.CKeyAssign.STKEYASSIGN[] HHO
				{
					get
					{
						return this.padHHO;
					}
					set
					{
						this.padHHO = value;
					}
				}
				public CConfigIni.CKeyAssign.STKEYASSIGN[] RD
				{
					get
					{
						return this.padRD;
					}
					set
					{
						this.padRD = value;
					}
				}
				public CConfigIni.CKeyAssign.STKEYASSIGN[] LC
				{
					get
					{
						return this.padLC;
					}
					set
					{
						this.padLC = value;
					}
				}
				public CConfigIni.CKeyAssign.STKEYASSIGN[] LP
				{
					get
					{
						return this.padLP;
					}
					set
					{
						this.padLP = value;
					}
				}
				public CConfigIni.CKeyAssign.STKEYASSIGN[] LBD
				{
					get
					{
						return this.padLBD;
					}
					set
					{
						this.padLBD = value;
					}
				}
				public CConfigIni.CKeyAssign.STKEYASSIGN[] Capture
				{
					get
					{
						return this.padCapture;
					}
					set
					{
						this.padCapture = value;
					}
				}
				public CConfigIni.CKeyAssign.STKEYASSIGN[] LeftRed
				{
					get
					{
						return this.padLRed;
					}
					set
					{
						this.padLRed = value;
					}
				}
				public CConfigIni.CKeyAssign.STKEYASSIGN[] RightRed
				{
					get
					{
						return this.padRRed;
					}
					set
					{
						this.padRRed = value;
					}
				}
				public CConfigIni.CKeyAssign.STKEYASSIGN[] LeftBlue
				{
					get
					{
						return this.padLBlue;
					}
					set
					{
						this.padLBlue = value;
					}
				}
				public CConfigIni.CKeyAssign.STKEYASSIGN[] RightBlue
				{
					get
					{
						return this.padRBlue;
					}
					set
					{
						this.padRBlue = value;
					}
                }
				public CConfigIni.CKeyAssign.STKEYASSIGN[] LeftRed2P
				{
					get
					{
						return this.padLRed2P;
					}
					set
					{
						this.padLRed2P = value;
					}
				}
				public CConfigIni.CKeyAssign.STKEYASSIGN[] RightRed2P
				{
					get
					{
						return this.padRRed2P;
					}
					set
					{
						this.padRRed2P = value;
					}
				}
				public CConfigIni.CKeyAssign.STKEYASSIGN[] LeftBlue2P
				{
					get
					{
						return this.padLBlue2P;
					}
					set
					{
						this.padLBlue2P = value;
					}
				}
				public CConfigIni.CKeyAssign.STKEYASSIGN[] RightBlue2P
				{
					get
					{
						return this.padRBlue2P;
					}
					set
					{
						this.padRBlue2P = value;
					}
                }
				public CConfigIni.CKeyAssign.STKEYASSIGN[] this[ int index ]
				{
					get
					{
						switch ( index )
						{
							case (int) EKeyConfigPad.HH:
								return this.padHH_R;

							case (int) EKeyConfigPad.SD:
								return this.padSD_G;

							case (int) EKeyConfigPad.BD:
								return this.padBD_B;

							case (int) EKeyConfigPad.HT:
								return this.padHT_Pick;

							case (int) EKeyConfigPad.LT:
								return this.padLT_Wail;

							case (int) EKeyConfigPad.FT:
								return this.padFT_Cancel;

							case (int) EKeyConfigPad.CY:
								return this.padCY_Decide;

							case (int) EKeyConfigPad.HHO:
								return this.padHHO;

							case (int) EKeyConfigPad.RD:
								return this.padRD;

							case (int) EKeyConfigPad.LC:
								return this.padLC;

							case (int) EKeyConfigPad.LP:	// #27029 2012.1.4 from
								return this.padLP;			//

							case (int) EKeyConfigPad.LBD:	// #27029 2012.1.4 from
								return this.padLBD;			//

							case (int) EKeyConfigPad.LRed:
								return this.padLRed;

							case (int) EKeyConfigPad.RRed:
								return this.padRRed;

							case (int) EKeyConfigPad.LBlue:
								return this.padLBlue;

							case (int) EKeyConfigPad.RBlue:
								return this.padRBlue;

							case (int) EKeyConfigPad.LRed2P:
								return this.padLRed2P;

							case (int) EKeyConfigPad.RRed2P:
								return this.padRRed2P;

							case (int) EKeyConfigPad.LBlue2P:
								return this.padLBlue2P;

							case (int) EKeyConfigPad.RBlue2P:
								return this.padRBlue2P;

							case (int) EKeyConfigPad.Capture:
								return this.padCapture;
						}
						throw new IndexOutOfRangeException();
					}
					set
					{
						switch ( index )
						{
							case (int) EKeyConfigPad.HH:
								this.padHH_R = value;
								return;

							case (int) EKeyConfigPad.SD:
								this.padSD_G = value;
								return;

							case (int) EKeyConfigPad.BD:
								this.padBD_B = value;
								return;

							case (int) EKeyConfigPad.Pick:
								this.padHT_Pick = value;
								return;

							case (int) EKeyConfigPad.LT:
								this.padLT_Wail = value;
								return;

							case (int) EKeyConfigPad.FT:
								this.padFT_Cancel = value;
								return;

							case (int) EKeyConfigPad.CY:
								this.padCY_Decide = value;
								return;

							case (int) EKeyConfigPad.HHO:
								this.padHHO = value;
								return;

							case (int) EKeyConfigPad.RD:
								this.padRD = value;
								return;

							case (int) EKeyConfigPad.LC:
								this.padLC = value;
								return;

							case (int) EKeyConfigPad.LP:
								this.padLP = value;
								return;

							case (int) EKeyConfigPad.LBD:
								this.padLBD = value;
								return;

                            case (int) EKeyConfigPad.LRed:
                                this.padLRed = value;
                                return;

                            case (int) EKeyConfigPad.RRed:
                                this.padRRed = value;
                                return;
                                
                            case (int) EKeyConfigPad.LBlue:
                                this.padLBlue = value;
                                return;

                            case (int) EKeyConfigPad.RBlue:
                                this.padRBlue = value;
                                return;

                            case (int) EKeyConfigPad.LRed2P:
                                this.padLRed2P = value;
                                return;

                            case (int) EKeyConfigPad.RRed2P:
                                this.padRRed2P = value;
                                return;
                                
                            case (int) EKeyConfigPad.LBlue2P:
                                this.padLBlue2P = value;
                                return;

                            case (int) EKeyConfigPad.RBlue2P:
                                this.padRBlue2P = value;
                                return;

							case (int) EKeyConfigPad.Capture:
								this.padCapture = value;
								return;
						}
						throw new IndexOutOfRangeException();
					}
				}

				#region [ private ]
				//-----------------
				private CConfigIni.CKeyAssign.STKEYASSIGN[] padBD_B;
				private CConfigIni.CKeyAssign.STKEYASSIGN[] padCY_Decide;
				private CConfigIni.CKeyAssign.STKEYASSIGN[] padFT_Cancel;
				private CConfigIni.CKeyAssign.STKEYASSIGN[] padHH_R;
				private CConfigIni.CKeyAssign.STKEYASSIGN[] padHHO;
				private CConfigIni.CKeyAssign.STKEYASSIGN[] padHT_Pick;
				private CConfigIni.CKeyAssign.STKEYASSIGN[] padLC;
				private CConfigIni.CKeyAssign.STKEYASSIGN[] padLT_Wail;
				private CConfigIni.CKeyAssign.STKEYASSIGN[] padRD;
				private CConfigIni.CKeyAssign.STKEYASSIGN[] padSD_G;
				private CConfigIni.CKeyAssign.STKEYASSIGN[] padLP;
				private CConfigIni.CKeyAssign.STKEYASSIGN[] padLBD;
				private CConfigIni.CKeyAssign.STKEYASSIGN[] padLRed;
				private CConfigIni.CKeyAssign.STKEYASSIGN[] padLBlue;
				private CConfigIni.CKeyAssign.STKEYASSIGN[] padRRed;
				private CConfigIni.CKeyAssign.STKEYASSIGN[] padRBlue;
				private CConfigIni.CKeyAssign.STKEYASSIGN[] padLRed2P;
				private CConfigIni.CKeyAssign.STKEYASSIGN[] padLBlue2P;
				private CConfigIni.CKeyAssign.STKEYASSIGN[] padRRed2P;
				private CConfigIni.CKeyAssign.STKEYASSIGN[] padRBlue2P;

				private CConfigIni.CKeyAssign.STKEYASSIGN[] padCapture;
				//-----------------
				#endregion
			}

			[StructLayout( LayoutKind.Sequential )]
			public struct STKEYASSIGN
			{
				public E入力デバイス 入力デバイス;
				public int ID;
				public int コード;
				public STKEYASSIGN( E入力デバイス DeviceType, int nID, int nCode )
				{
					this.入力デバイス = DeviceType;
					this.ID = nID;
					this.コード = nCode;
				}
			}

			public CKeyAssignPad Bass = new CKeyAssignPad();
			public CKeyAssignPad Drums = new CKeyAssignPad();
			public CKeyAssignPad Guitar = new CKeyAssignPad();
			public CKeyAssignPad Taiko = new CKeyAssignPad();
			public CKeyAssignPad System = new CKeyAssignPad();
			public CKeyAssignPad this[ int index ]
			{
				get
				{
					switch( index )
					{
						case (int) EKeyConfigPart.DRUMS:
							return this.Drums;

						case (int) EKeyConfigPart.GUITAR:
							return this.Guitar;

						case (int) EKeyConfigPart.BASS:
							return this.Bass;

                        case (int) EKeyConfigPart.TAIKO:
                            return this.Taiko;

						case (int) EKeyConfigPart.SYSTEM:
							return this.System;
					}
					throw new IndexOutOfRangeException();
				}
				set
				{
					switch( index )
					{
						case (int) EKeyConfigPart.DRUMS:
							this.Drums = value;
							return;

						case (int) EKeyConfigPart.GUITAR:
							this.Guitar = value;
							return;

						case (int) EKeyConfigPart.BASS:
							this.Bass = value;
							return;

                        case (int) EKeyConfigPart.TAIKO:
                            this.Taiko = value;
                            return;

						case (int) EKeyConfigPart.SYSTEM:
							this.System = value;
							return;
					}
					throw new IndexOutOfRangeException();
				}
			}
		}
		#endregion

		//
		public enum ESoundDeviceTypeForConfig
		{
			ACM = 0,
			// DirectSound,
			ASIO,
			WASAPI,
			Unknown=99
		}
		// プロパティ

#if false		// #23625 2011.1.11 Config.iniからダメージ/回復値の定数変更を行う場合はここを有効にする 087リリースに合わせ機能無効化
		//----------------------------------------
		public float[,] fGaugeFactor = new float[5,2];
		public float[] fDamageLevelFactor = new float[3];
		//----------------------------------------
#endif
		public int nBGAlpha;
		public bool bAVI有効;
		public bool bBGA有効;
		public bool bBGM音を発声する;
		public STDGBVALUE<bool> bHidden;
		public STDGBVALUE<bool> bLeft;
		public STDGBVALUE<bool> bLight;
		public bool bLogDTX詳細ログ出力;
		public bool bLog曲検索ログ出力;
		public bool bLog作成解放ログ出力;
		public STDGBVALUE<bool> bReverse;
		//public STDGBVALUE<E判定表示優先度> e判定表示優先度;
		public E判定表示優先度 e判定表示優先度;
		public STDGBVALUE<E判定位置> e判定位置;			// #33891 2014.6.26 yyagi
		public bool bScoreIniを出力する;
		public bool bSTAGEFAILED有効;
		public STDGBVALUE<bool> bSudden;
		public bool bTight;
		public bool bTight2P; // 2018.12.15 kairera0467
		public STDGBVALUE<bool> bGraph;     // #24074 2011.01.23 add ikanick
		public bool bWave再生位置自動調整機能有効;
		public bool bストイックモード;
		public bool bフィルイン有効;
		public bool bランダムセレクトで子BOXを検索対象とする;
		public bool bログ出力;
		public STDGBVALUE<bool> b演奏音を強調する;
		public bool b演奏情報を表示する;
		public bool b歓声を発声する;
		public bool b垂直帰線待ちを行う;
		public bool b選曲リストフォントを斜体にする;
		public bool b選曲リストフォントを太字にする;
		public bool b全画面モード;
		public int n初期ウィンドウ開始位置X; // #30675 2013.02.04 ikanick add
		public int n初期ウィンドウ開始位置Y;  
		public int nウインドウwidth;				// #23510 2010.10.31 yyagi add
		public int nウインドウheight;				// #23510 2010.10.31 yyagi add
		public Dictionary<int, string> dicJoystick;
		public Eダークモード eDark;
		public STDGBVALUE<Eランダムモード> eRandom;
		public Eダメージレベル eダメージレベル;
        public CKeyAssign KeyAssign;
		public int n非フォーカス時スリープms;       // #23568 2010.11.04 ikanick add
		public int nフレーム毎スリープms;			// #xxxxx 2011.11.27 yyagi add
		public int n演奏速度;
		public int n曲が選択されてからプレビュー音が鳴るまでのウェイトms;
		public int n曲が選択されてからプレビュー画像が表示開始されるまでのウェイトms;
		public int n自動再生音量;
		public int n手動再生音量;
		public int n選曲リストフォントのサイズdot;
		public STDGBVALUE<int> n表示可能な最小コンボ数;
		public STDGBVALUE<int> n譜面スクロール速度;
		public string strDTXManiaのバージョン;
		public string str曲データ検索パス;
		public string str選曲リストフォント;
        public string strPrivateFontで使うフォント名;
        public bool bドラムコンボ表示;
        public bool bBranchGuide;
        public int nScoreMode;
        public int nDefaultCourse; //2017.01.30 DD デフォルトでカーソルをあわせる難易度


        public int nPlayerCount; //2017.08.18 kairera0467 マルチプレイ対応
        public bool b太鼓パートAutoPlay;
        public bool b太鼓パートAutoPlay2P; //2017.08.16 kairera0467 マルチプレイ対応
        public bool bAuto先生の連打;
        public bool b大音符判定;
        public int n両手判定の待ち時間;
        public int nBranchAnime;

        public bool bJudgeCountDisplay;

        public bool bChara;

        public int nCharaMotionCount;
        public int nCharaMotionCount_gogo;
        public int nCharaMotionCount_clear;
        public int nCharaMotionCount_max;
        public int nCharaMotionCount_maxgogo;
        public int nCharaMotionCount_jump;
        public int nCharaMotionLoopBeats;
        public int nCharaMotionLoopBeats_gogo;
        public int nCharaMotionLoopBeats_clear;
        public int nCharaMotionLoopBeats_max;
        public int nCharaMotionLoopBeats_maxgogo;

        public string strCharaMotionList;
        public string strCharaMotionList_gogo;
        public string strCharaMotionList_clear;
        public string strCharaMotionList_max;
        public string strCharaMotionList_maxgogo;
        public string strCharaMotionList_jump;

        public E難易度表示タイプ eDiffShowType;
        public EScrollMode eScrollMode = EScrollMode.Normal;
        public bool bスクロールモードを上書き = false;

        public bool bHispeedRandom;
        public Eステルスモード eSTEALTH;
        public bool bNoInfo;
        public bool bMonochlo;
        public int nJustHIDDEN; //2018.03.30 kairera0467
        public bool bZeroSpeed;
        public bool bAutoRetry; //2018.05.03 kairera0467

        public int nDefaultSongSort;

        public EGame eGameMode;
        public bool bSuperHard = false;
        public bool bJust;

        public bool bEndingAnime = false;   // 2017.01.27 DD 「また遊んでね」画面の有効/無効オプション追加
        
        public Eゲージモード eGaugeMode; //2018.03.26 kairera0467

		public STDGBVALUE<int> nInputAdjustTimeMs;	// #23580 2011.1.3 yyagi タイミングアジャスト機能
		public STDGBVALUE<int> nJudgeLinePosOffset;	// #31602 2013.6.23 yyagi 判定ライン表示位置のオフセット
		public int	nShowLagType;					// #25370 2011.6.5 yyagi ズレ時間表示機能
		public bool bIsAutoResultCapture;			// #25399 2011.6.9 yyagi リザルト画像自動保存機能のON/OFF制御
		public int nPoliphonicSounds;				// #28228 2012.5.1 yyagi レーン毎の最大同時発音数
		public bool bバッファ入力を行う;
		public bool bIsEnabledSystemMenu;			// #28200 2012.5.1 yyagi System Menuの使用可否切替
		public string strSystemSkinSubfolderFullName;	// #28195 2012.5.2 yyagi Skin切替用 System/以下のサブフォルダ名
		public bool bUseBoxDefSkin;						// #28195 2012.5.6 yyagi Skin切替用 box.defによるスキン変更機能を使用するか否か
		public bool bConfigIniがないかDTXManiaのバージョンが異なる
		{
			get
			{
				return ( !this.bConfigIniが存在している || !CDTXMania.VERSION.Equals( this.strDTXManiaのバージョン ) );
			}
		}
		public bool bEnterがキー割り当てのどこにも使用されていない
		{
			get
			{
				for( int i = 0; i <= (int)EKeyConfigPart.SYSTEM; i++ )
				{
					for( int j = 0; j <= (int)EKeyConfigPad.Capture; j++ )
					{
						for( int k = 0; k < 0x10; k++ )
						{
							if( ( this.KeyAssign[ i ][ j ][ k ].入力デバイス == E入力デバイス.キーボード ) && ( this.KeyAssign[ i ][ j ][ k ].コード == (int) SlimDX.DirectInput.Key.Return ) )
							{
								return false;
							}
						}
					}
				}
				return true;
			}
		}
		public bool bウィンドウモード
		{
			get
			{
				return !this.b全画面モード;
			}
			set
			{
				this.b全画面モード = !value;
			}
		}
		public bool b演奏情報を表示しない
		{
			get
			{
				return !this.b演奏情報を表示する;
			}
			set
			{
				this.b演奏情報を表示する = !value;
			}
		}
		public int n背景の透過度
		{
			get
			{
				return this.nBGAlpha;
			}
			set
			{
				if( value < 0 )
				{
					this.nBGAlpha = 0;
				}
				else if( value > 0xff )
				{
					this.nBGAlpha = 0xff;
				}
				else
				{
					this.nBGAlpha = value;
				}
			}
		}
		public int nRisky;						// #23559 2011.6.20 yyagi Riskyでの残ミス数。0で閉店
		public bool bIsAllowedDoubleClickFullscreen;	// #26752 2011.11.27 yyagi ダブルクリックしてもフルスクリーンに移行しない
		public STAUTOPLAY bAutoPlay;
		public int nSoundDeviceType;				// #24820 2012.12.23 yyagi 出力サウンドデバイス(0=ACM(にしたいが設計がきつそうならDirectShow), 1=ASIO, 2=WASAPI)
		public int nWASAPIBufferSizeMs;				// #24820 2013.1.15 yyagi WASAPIのバッファサイズ
//		public int nASIOBufferSizeMs;				// #24820 2012.12.28 yyagi ASIOのバッファサイズ
		public int nASIODevice;						// #24820 2013.1.17 yyagi ASIOデバイス
		public bool bUseOSTimer;					// #33689 2014.6.6 yyagi 演奏タイマーの種類
		public bool bDynamicBassMixerManagement;	// #24820
		public bool bTimeStretch;					// #23664 2013.2.24 yyagi ピッチ変更無しで再生速度を変更するかどうか
		public STDGBVALUE<EInvisible> eInvisible;	// #32072 2013.9.20 yyagi チップを非表示にする
		public int nDisplayTimesMs, nFadeoutTimeMs;

		public STDGBVALUE<int> nViewerScrollSpeed;
		public bool bViewerVSyncWait;
		public bool bViewerShowDebugStatus;
		public bool bViewerTimeStretch;
		public bool bViewerDrums有効, bViewerGuitar有効;
		//public bool bNoMP3Streaming;				// 2014.4.14 yyagi; mp3のシーク位置がおかしくなる場合は、これをtrueにすることで、wavにデコードしてからオンメモリ再生する
		public int nMasterVolume;
#if false
		[StructLayout( LayoutKind.Sequential )]
		public struct STAUTOPLAY								// C定数のEレーンとindexを一致させること
		{
			public bool LC;			// 0
			public bool HH;			// 1
			public bool SD;			// 2
			public bool BD;			// 3
			public bool HT;			// 4
			public bool LT;			// 5
			public bool FT;			// 6
			public bool CY;			// 7
			public bool RD;			// 8
			public bool Guitar;		// 9	(not used)
			public bool Bass;		// 10	(not used)
			public bool GtR;		// 11
			public bool GtG;		// 12
			public bool GtB;		// 13
			public bool GtPick;		// 14
			public bool GtW;		// 15
			public bool BsR;		// 16
			public bool BsG;		// 17
			public bool BsB;		// 18
			public bool BsPick;		// 19
			public bool BsW;		// 20
			public bool this[ int index ]
			{
				get
				{
					switch ( index )
					{
						case (int) Eレーン.LC:
							return this.LC;
						case (int) Eレーン.HH:
							return this.HH;
						case (int) Eレーン.SD:
							return this.SD;
						case (int) Eレーン.BD:
							return this.BD;
						case (int) Eレーン.HT:
							return this.HT;
						case (int) Eレーン.LT:
							return this.LT;
						case (int) Eレーン.FT:
							return this.FT;
						case (int) Eレーン.CY:
							return this.CY;
						case (int) Eレーン.RD:
							return this.RD;
						case (int) Eレーン.Guitar:
							return this.Guitar;
						case (int) Eレーン.Bass:
							return this.Bass;
						case (int) Eレーン.GtR:
							return this.GtR;
						case (int) Eレーン.GtG:
							return this.GtG;
						case (int) Eレーン.GtB:
							return this.GtB;
						case (int) Eレーン.GtPick:
							return this.GtPick;
						case (int) Eレーン.GtW:
							return this.GtW;
						case (int) Eレーン.BsR:
							return this.BsR;
						case (int) Eレーン.BsG:
							return this.BsG;
						case (int) Eレーン.BsB:
							return this.BsB;
						case (int) Eレーン.BsPick:
							return this.BsPick;
						case (int) Eレーン.BsW:
							return this.BsW;
					}
					throw new IndexOutOfRangeException();
				}
				set
				{
					switch ( index )
					{
						case (int) Eレーン.LC:
							this.LC = value;
							return;
						case (int) Eレーン.HH:
							this.HH = value;
							return;
						case (int) Eレーン.SD:
							this.SD = value;
							return;
						case (int) Eレーン.BD:
							this.BD = value;
							return;
						case (int) Eレーン.HT:
							this.HT = value;
							return;
						case (int) Eレーン.LT:
							this.LT = value;
							return;
						case (int) Eレーン.FT:
							this.FT = value;
							return;
						case (int) Eレーン.CY:
							this.CY = value;
							return;
						case (int) Eレーン.RD:
							this.RD = value;
							return;
						case (int) Eレーン.Guitar:
							this.Guitar = value;
							return;
						case (int) Eレーン.Bass:
							this.Bass = value;
							return;
						case (int) Eレーン.GtR:
							this.GtR = value;
							return;
						case (int) Eレーン.GtG:
							this.GtG = value;
							return;
						case (int) Eレーン.GtB:
							this.GtB = value;
							return;
						case (int) Eレーン.GtPick:
							this.GtPick = value;
							return;
						case (int) Eレーン.GtW:
							this.GtW = value;
							return;
						case (int) Eレーン.BsR:
							this.BsR = value;
							return;
						case (int) Eレーン.BsG:
							this.BsG = value;
							return;
						case (int) Eレーン.BsB:
							this.BsB = value;
							return;
						case (int) Eレーン.BsPick:
							this.BsPick = value;
							return;
						case (int) Eレーン.BsW:
							this.BsW = value;
							return;
					}
					throw new IndexOutOfRangeException();
				}
			}
		}
#endif
		#region [ STRANGE ]
		public STRANGE nヒット範囲ms;
		[StructLayout( LayoutKind.Sequential )]
		public struct STRANGE
		{
			public int Perfect;
			public int Great;
			public int Good;
			public int Poor;
			public int this[ int index ]
			{
				get
				{
					switch( index )
					{
						case 0:
							return this.Perfect;

						case 1:
							return this.Great;

						case 2:
							return this.Good;

						case 3:
							return this.Poor;
					}
					throw new IndexOutOfRangeException();
				}
				set
				{
					switch( index )
					{
						case 0:
							this.Perfect = value;
							return;

						case 1:
							this.Great = value;
							return;

						case 2:
							this.Good = value;
							return;

						case 3:
							this.Poor = value;
							return;
					}
					throw new IndexOutOfRangeException();
				}
			}
		}
		#endregion
		#region [ STLANEVALUE ]
		public STLANEVALUE nVelocityMin;
		[StructLayout( LayoutKind.Sequential )]
		public struct STLANEVALUE
		{
			public int LC;
			public int HH;
			public int SD;
			public int BD;
			public int HT;
			public int LT;
			public int FT;
			public int CY;
			public int RD;
            public int LP;
            public int LBD;
			public int Guitar;
			public int Bass;
			public int this[ int index ]
			{
				get
				{
					switch( index )
					{
						case 0:
							return this.LC;

						case 1:
							return this.HH;

						case 2:
							return this.SD;

						case 3:
							return this.BD;

						case 4:
							return this.HT;

						case 5:
							return this.LT;

						case 6:
							return this.FT;

						case 7:
							return this.CY;

						case 8:
							return this.RD;

						case 9:
							return this.LP;

						case 10:
							return this.LBD;

						case 11:
							return this.Guitar;

						case 12:
							return this.Bass;
					}
					throw new IndexOutOfRangeException();
				}
				set
				{
					switch( index )
					{
						case 0:
							this.LC = value;
							return;

						case 1:
							this.HH = value;
							return;

						case 2:
							this.SD = value;
							return;

						case 3:
							this.BD = value;
							return;

						case 4:
							this.HT = value;
							return;

						case 5:
							this.LT = value;
							return;

						case 6:
							this.FT = value;
							return;

						case 7:
							this.CY = value;
							return;

						case 8:
							this.RD = value;
							return;

						case 9:
							this.LP = value;
							return;

						case 10:
							this.LBD = value;
							return;

						case 11:
							this.Guitar = value;
							return;

						case 12:
							this.Bass = value;
							return;
					}
					throw new IndexOutOfRangeException();
				}
			}
		}
		#endregion


        #region[Ver.K追加オプション]
        //--------------------------
        //ゲーム内のオプションに加えて、
        //システム周りのオプションもこのブロックで記述している。
        #region[Display]
        //--------------------------
        public EClipDispType eClipDispType;
        #endregion

        #region[Position]
        //public Eレーンタイプ eLaneType;
        //public Eミラー eMirror;

        #endregion
        #region[System]
        public bool bDirectShowMode;
        #endregion

        //--------------------------
        #endregion


        // コンストラクタ

		public CConfigIni()
		{
#if false		// #23625 2011.1.11 Config.iniからダメージ/回復値の定数変更を行う場合はここを有効にする 087リリースに合わせ機能無効化
			//----------------------------------------
			this.fGaugeFactor[0,0] =  0.004f;
			this.fGaugeFactor[0,1] =  0.006f;
			this.fGaugeFactor[1,0] =  0.002f;
			this.fGaugeFactor[1,1] =  0.003f;
			this.fGaugeFactor[2,0] =  0.000f;
			this.fGaugeFactor[2,1] =  0.000f;
			this.fGaugeFactor[3,0] = -0.020f;
			this.fGaugeFactor[3,1] = -0.030f;
			this.fGaugeFactor[4,0] = -0.050f;
			this.fGaugeFactor[4,1] = -0.050f;

			this.fDamageLevelFactor[0] = 0.5f;
			this.fDamageLevelFactor[1] = 1.0f;
			this.fDamageLevelFactor[2] = 1.5f;
			//----------------------------------------
#endif
			this.strDTXManiaのバージョン = "Unknown";
			this.str曲データ検索パス = @".\";
			this.b全画面モード = false;
			this.b垂直帰線待ちを行う = true;
			this.n初期ウィンドウ開始位置X = 0; // #30675 2013.02.04 ikanick add
			this.n初期ウィンドウ開始位置Y = 0;  
			this.nウインドウwidth = SampleFramework.GameWindowSize.Width;			// #23510 2010.10.31 yyagi add
			this.nウインドウheight = SampleFramework.GameWindowSize.Height;			// 
			this.nフレーム毎スリープms = -1;			// #xxxxx 2011.11.27 yyagi add
			this.n非フォーカス時スリープms = 1;			// #23568 2010.11.04 ikanick add
			this._bGuitar有効 = true;
			this._bDrums有効 = true;
			this.nBGAlpha = 100;
			this.eダメージレベル = Eダメージレベル.普通;
			this.bSTAGEFAILED有効 = true;
			this.bAVI有効 = true;
			this.bBGA有効 = true;
			this.bフィルイン有効 = true;
			this.n曲が選択されてからプレビュー音が鳴るまでのウェイトms = 1000;
			this.n曲が選択されてからプレビュー画像が表示開始されるまでのウェイトms = 100;
			//this.bWave再生位置自動調整機能有効 = true;
			this.bWave再生位置自動調整機能有効 = false;
			this.bBGM音を発声する = true;
			this.b歓声を発声する = true;
			this.bScoreIniを出力する = true;
			this.bランダムセレクトで子BOXを検索対象とする = true;
			this.n表示可能な最小コンボ数 = new STDGBVALUE<int>();
			this.n表示可能な最小コンボ数.Drums = 10;
			this.n表示可能な最小コンボ数.Guitar = 2;
			this.n表示可能な最小コンボ数.Bass = 2;
			this.n表示可能な最小コンボ数.Taiko = 10;
			this.str選曲リストフォント = "MS PGothic";
			this.n選曲リストフォントのサイズdot = 20;
			this.b選曲リストフォントを太字にする = true;
			this.n自動再生音量 = 80;
			this.n手動再生音量 = 100;
			this.bログ出力 = true;
			this.b演奏音を強調する = new STDGBVALUE<bool>();
			this.bSudden = new STDGBVALUE<bool>();
			this.bHidden = new STDGBVALUE<bool>();
			this.bReverse = new STDGBVALUE<bool>();
			this.eRandom = new STDGBVALUE<Eランダムモード>();
			this.bLight = new STDGBVALUE<bool>();
			this.bLeft = new STDGBVALUE<bool>();
			this.e判定位置 = new STDGBVALUE<E判定位置>();		// #33891 2014.6.26 yyagi
			this.n譜面スクロール速度 = new STDGBVALUE<int>();
			this.nInputAdjustTimeMs = new STDGBVALUE<int>();	// #23580 2011.1.3 yyagi
			this.nJudgeLinePosOffset = new STDGBVALUE<int>();	// #31602 2013.6.23 yyagi
			this.e判定表示優先度 = E判定表示優先度.Chipより下;
			for ( int i = 0; i < 3; i++ )
			{
				this.b演奏音を強調する[ i ] = true;
				this.bSudden[ i ] = false;
				this.bHidden[ i ] = false;
				this.bReverse[ i ] = false;
				this.eRandom[ i ] = Eランダムモード.OFF;
				this.bLight[ i ] = false;
				this.bLeft[ i ] = false;
				this.n譜面スクロール速度[ i ] = 1;
				this.nInputAdjustTimeMs[ i ] = 0;
				this.nJudgeLinePosOffset[ i ] = 0;
				this.eInvisible[ i ] = EInvisible.OFF;
				this.nViewerScrollSpeed[ i ] = 1;
				this.e判定位置[ i ] = E判定位置.標準;
				//this.e判定表示優先度[ i ] = E判定表示優先度.Chipより下;
			}
			this.n演奏速度 = 20;
			#region [ AutoPlay ]
			this.bAutoPlay = new STAUTOPLAY();

            this.b太鼓パートAutoPlay = true;
            this.b太鼓パートAutoPlay2P = true;
            this.bAuto先生の連打 = true;
			#endregion
			this.nヒット範囲ms = new STRANGE();
			this.nヒット範囲ms.Perfect = 25;
			this.nヒット範囲ms.Great = -1; //使用しません。
			this.nヒット範囲ms.Good = 75;
			this.nヒット範囲ms.Poor = 108;
			this.ConfigIniファイル名 = "";
			this.dicJoystick = new Dictionary<int, string>( 10 );
			this.tデフォルトのキーアサインに設定する();
			#region [ velocityMin ]
			this.nVelocityMin.LC = 0;					// #23857 2011.1.31 yyagi VelocityMin
			this.nVelocityMin.HH = 20;
			this.nVelocityMin.SD = 0;
			this.nVelocityMin.BD = 0;
			this.nVelocityMin.HT = 0;
			this.nVelocityMin.LT = 0;
			this.nVelocityMin.FT = 0;
			this.nVelocityMin.CY = 0;
			this.nVelocityMin.RD = 0;
            this.nVelocityMin.LP = 0;
            this.nVelocityMin.LBD = 0;
			#endregion
			this.nRisky = 0;							// #23539 2011.7.26 yyagi RISKYモード
			this.nShowLagType = (int) EShowLagType.OFF;	// #25370 2011.6.3 yyagi ズレ時間表示
			this.bIsAutoResultCapture = false;			// #25399 2011.6.9 yyagi リザルト画像自動保存機能ON/OFF

			this.bバッファ入力を行う = true;
			this.bIsAllowedDoubleClickFullscreen = true;	// #26752 2011.11.26 ダブルクリックでのフルスクリーンモード移行を許可
			this.nPoliphonicSounds = 4;					// #28228 2012.5.1 yyagi レーン毎の最大同時発音数
														// #24820 2013.1.15 yyagi 初期値を4から2に変更。BASS.net使用時の負荷軽減のため。
														// #24820 2013.1.17 yyagi 初期値を4に戻した。動的なミキサー制御がうまく動作しているため。
			this.bIsEnabledSystemMenu = true;			// #28200 2012.5.1 yyagi System Menuの利用可否切替(使用可)
			this.strSystemSkinSubfolderFullName = "";	// #28195 2012.5.2 yyagi 使用中のSkinサブフォルダ名
			this.bUseBoxDefSkin = true;					// #28195 2012.5.6 yyagi box.defによるスキン切替機能を使用するか否か
			this.bTight = false;                        // #29500 2012.9.11 kairera0467 TIGHTモード
			#region [ WASAPI/ASIO ]
			this.nSoundDeviceType = FDK.COS.bIsVistaOrLater ?
				(int) ESoundDeviceTypeForConfig.WASAPI : (int) ESoundDeviceTypeForConfig.ACM;	// #24820 2012.12.23 yyagi 初期値はACM | #31927 2013.8.25 yyagi OSにより初期値変更
			this.nWASAPIBufferSizeMs = 50;				// #24820 2013.1.15 yyagi 初期値は50(0で自動設定)
			this.nASIODevice = 0;						// #24820 2013.1.17 yyagi
//			this.nASIOBufferSizeMs = 0;					// #24820 2012.12.25 yyagi 初期値は0(自動設定)
			#endregion
			this.bUseOSTimer = false;;					// #33689 2014.6.6 yyagi 初期値はfalse (FDKのタイマー。ＦＲＯＭ氏考案の独自タイマー)
			this.bDynamicBassMixerManagement = true;	//
			this.bTimeStretch = false;					// #23664 2013.2.24 yyagi 初期値はfalse (再生速度変更を、ピッチ変更にて行う)
			this.nDisplayTimesMs = 3000;				// #32072 2013.10.24 yyagi Semi-Invisibleでの、チップ再表示期間
			this.nFadeoutTimeMs = 2000;					// #32072 2013.10.24 yyagi Semi-Invisibleでの、チップフェードアウト時間

			bViewerVSyncWait = true;
			bViewerShowDebugStatus = true;
			bViewerTimeStretch = false;
			bViewerDrums有効 = true;
			bViewerGuitar有効 = true;
            
            

            this.bBranchGuide = false;
            this.nScoreMode = 2;
            this.nDefaultCourse = 3;
            this.nBranchAnime = 1;

            this.b大音符判定 = true;
            this.n両手判定の待ち時間 = 50;

            this.bJudgeCountDisplay = false;
            this.bChara = true;

            this.nCharaMotionCount = 6;
            this.nCharaMotionCount_clear = 0;
            this.nCharaMotionCount_gogo = 22;
            this.nCharaMotionCount_max = 0;
            this.nCharaMotionCount_maxgogo = 0;

            this.strCharaMotionList = "5,4,3,2,1,0,0,0,0,0,0,1,2,3,4,5";
            this.strCharaMotionList_gogo = "0,1,2,3,4,5,6,7,8,9,10,10,11,12,13,14,15,16,17,18,19,20,21";
            this.strCharaMotionList_clear = "0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22";
            this.strCharaMotionList_max = "0";
            this.strCharaMotionList_maxgogo = "0";

            this.nCharaMotionLoopBeats_clear = 2;

            this.eSTEALTH = Eステルスモード.OFF;
            this.bNoInfo = false;
            this.eGaugeMode = Eゲージモード.Normal;
            
            //this.bNoMP3Streaming = false;
			this.nMasterVolume = 100;					// #33700 2014.4.26 yyagi マスターボリュームの設定(WASAPI/ASIO用)

            this.bHispeedRandom = false;
            this.nDefaultSongSort = 0;
            this.eGameMode = EGame.OFF;
            this.bEndingAnime = false;
            this.nPlayerCount = 1; //2017.08.18 kairera0467 マルチプレイ対応
            this.eGaugeMode = Eゲージモード.Normal;

            #region[ Ver.K追加 ]
            //this.eLaneType = Eレーンタイプ.TypeA;
            this.bDirectShowMode = false;
            this.bドラムコンボ表示 = true;
            #endregion
        }
		public CConfigIni( string iniファイル名 )
			: this()
		{
			this.tファイルから読み込み( iniファイル名 );
		}


		// メソッド

		public void t指定した入力が既にアサイン済みである場合はそれを全削除する( E入力デバイス DeviceType, int nID, int nCode )
		{
			for( int i = 0; i <= (int)EKeyConfigPart.SYSTEM; i++ )
			{
				for( int j = 0; j <= (int)EKeyConfigPad.Capture; j++ )
				{
					for( int k = 0; k < 0x10; k++ )
					{
						if( ( ( this.KeyAssign[ i ][ j ][ k ].入力デバイス == DeviceType ) && ( this.KeyAssign[ i ][ j ][ k ].ID == nID ) ) && ( this.KeyAssign[ i ][ j ][ k ].コード == nCode ) )
						{
							for( int m = k; m < 15; m++ )
							{
								this.KeyAssign[ i ][ j ][ m ] = this.KeyAssign[ i ][ j ][ m + 1 ];
							}
							this.KeyAssign[ i ][ j ][ 15 ].入力デバイス = E入力デバイス.不明;
							this.KeyAssign[ i ][ j ][ 15 ].ID = 0;
							this.KeyAssign[ i ][ j ][ 15 ].コード = 0;
							k--;
						}
					}
				}
			}
		}
		public void t書き出し( string iniファイル名 )
		{
			StreamWriter sw = new StreamWriter( iniファイル名, false, Encoding.GetEncoding( "Shift_JIS" ) );
			sw.WriteLine( ";-------------------" );
			
			#region [ System ]
			sw.WriteLine( "[System]" );
			sw.WriteLine();

#if false		// #23625 2011.1.11 Config.iniからダメージ/回復値の定数変更を行う場合はここを有効にする 087リリースに合わせ機能無効化
	//------------------------------
			sw.WriteLine("; ライフゲージのパラメータ調整(調整完了後削除予定)");
			sw.WriteLine("; GaugeFactorD: ドラムのPerfect, Great,... の回復量(ライフMAXを1.0としたときの値を指定)");
			sw.WriteLine("; GaugeFactorG:  Gt/BsのPerfect, Great,... の回復量(ライフMAXを1.0としたときの値を指定)");
			sw.WriteLine("; DamageFactorD: DamageLevelがSmall, Normal, Largeの時に対するダメージ係数");
			sw.WriteLine("GaugeFactorD={0}, {1}, {2}, {3}, {4}", this.fGaugeFactor[0, 0], this.fGaugeFactor[1, 0], this.fGaugeFactor[2, 0], this.fGaugeFactor[3, 0], this.fGaugeFactor[4, 0]);
			sw.WriteLine("GaugeFactorG={0}, {1}, {2}, {3}, {4}", this.fGaugeFactor[0, 1], this.fGaugeFactor[1, 1], this.fGaugeFactor[2, 1], this.fGaugeFactor[3, 1], this.fGaugeFactor[4, 1]);
			sw.WriteLine("DamageFactor={0}, {1}, {2}", this.fDamageLevelFactor[0], this.fDamageLevelFactor[1], fDamageLevelFactor[2]);
			sw.WriteLine();
	//------------------------------
#endif
			#region [ Version ]
			sw.WriteLine( "; リリースバージョン" );
			sw.WriteLine( "; Release Version." );
			sw.WriteLine( "Version={0}", CDTXMania.VERSION );
			sw.WriteLine();
			#endregion
			#region [ DTXPath ]
			sw.WriteLine( "; 演奏データの格納されているフォルダへのパス。" );
			sw.WriteLine( @"; セミコロン(;)で区切ることにより複数のパスを指定できます。（例: d:\DTXFiles1\;e:\DTXFiles2\）" );
			sw.WriteLine( "; Pathes for DTX data." );
			sw.WriteLine( @"; You can specify many pathes separated with semicolon(;). (e.g. d:\DTXFiles1\;e:\DTXFiles2\)" );
			sw.WriteLine( "DTXPath={0}", this.str曲データ検索パス );
			sw.WriteLine();
			#endregion
			#region [ スキン関連 ]
			#region [ Skinパスの絶対パス→相対パス変換 ]
			Uri uriRoot = new Uri( System.IO.Path.Combine( CDTXMania.strEXEのあるフォルダ, "System" + System.IO.Path.DirectorySeparatorChar ) );
			if ( strSystemSkinSubfolderFullName != null && strSystemSkinSubfolderFullName.Length == 0 )
			{
				// Config.iniが空の状態でDTXManiaをViewerとして起動_終了すると、strSystemSkinSubfolderFullName が空の状態でここに来る。
				// → 初期値として Default/ を設定する。
				strSystemSkinSubfolderFullName = System.IO.Path.Combine( CDTXMania.strEXEのあるフォルダ, "System" + System.IO.Path.DirectorySeparatorChar + "Default" + System.IO.Path.DirectorySeparatorChar );
			}
			Uri uriPath = new Uri( System.IO.Path.Combine( this.strSystemSkinSubfolderFullName, "." + System.IO.Path.DirectorySeparatorChar ) );
			string relPath = uriRoot.MakeRelativeUri( uriPath ).ToString();				// 相対パスを取得
			relPath = System.Web.HttpUtility.UrlDecode( relPath );						// デコードする
			relPath = relPath.Replace( '/', System.IO.Path.DirectorySeparatorChar );	// 区切り文字が\ではなく/なので置換する
			#endregion
			sw.WriteLine( "; 使用するSkinのフォルダ名。" );
			sw.WriteLine( "; 例えば System\\Default\\Graphics\\... などの場合は、SkinPath=.\\Default\\ を指定します。" );
			sw.WriteLine( "; Skin folder path." );
			sw.WriteLine( "; e.g. System\\Default\\Graphics\\... -> Set SkinPath=.\\Default\\" );
			sw.WriteLine( "SkinPath={0}", relPath );
			sw.WriteLine();
			sw.WriteLine( "; box.defが指定するSkinに自動で切り替えるかどうか (0=切り替えない、1=切り替える)" );
			sw.WriteLine( "; Automatically change skin specified in box.def. (0=No 1=Yes)" );
			sw.WriteLine( "SkinChangeByBoxDef={0}", this.bUseBoxDefSkin? 1 : 0 );
			sw.WriteLine();
			#endregion
			#region [ Window関連 ]
			sw.WriteLine( "; 画面モード(0:ウィンドウ, 1:全画面)" );
			sw.WriteLine( "; Screen mode. (0:Window, 1:Fullscreen)" );
			sw.WriteLine( "FullScreen={0}", this.b全画面モード ? 1 : 0 );
            sw.WriteLine();
			sw.WriteLine("; ウインドウモード時の画面幅");				// #23510 2010.10.31 yyagi add
			sw.WriteLine("; A width size in the window mode.");			//
			sw.WriteLine("WindowWidth={0}", this.nウインドウwidth);		//
			sw.WriteLine();												//
			sw.WriteLine("; ウインドウモード時の画面高さ");				//
			sw.WriteLine("; A height size in the window mode.");		//
			sw.WriteLine("WindowHeight={0}", this.nウインドウheight);	//
			sw.WriteLine();												//
			sw.WriteLine( "; ウィンドウモード時の位置X" );				            // #30675 2013.02.04 ikanick add
			sw.WriteLine( "; X position in the window mode." );			            //
			sw.WriteLine( "WindowX={0}", this.n初期ウィンドウ開始位置X );			//
			sw.WriteLine();											            	//
			sw.WriteLine( "; ウィンドウモード時の位置Y" );			            	//
			sw.WriteLine( "; Y position in the window mode." );	            	    //
			sw.WriteLine( "WindowY={0}", this.n初期ウィンドウ開始位置Y );   		//
			sw.WriteLine();												            //

			sw.WriteLine( "; ウインドウをダブルクリックした時にフルスクリーンに移行するか(0:移行しない,1:移行する)" );	// #26752 2011.11.27 yyagi
			sw.WriteLine( "; Whether double click to go full screen mode or not.(0:No, 1:Yes)" );		//
			sw.WriteLine( "DoubleClickFullScreen={0}", this.bIsAllowedDoubleClickFullscreen? 1 : 0);	//
			sw.WriteLine();																				//
			sw.WriteLine( "; ALT+SPACEのメニュー表示を抑制するかどうか(0:抑制する 1:抑制しない)" );		// #28200 2012.5.1 yyagi
			sw.WriteLine( "; Whether ALT+SPACE menu would be masked or not.(0=masked 1=not masked)" );	//
			sw.WriteLine( "EnableSystemMenu={0}", this.bIsEnabledSystemMenu? 1 : 0 );					//
			sw.WriteLine();																				//
			sw.WriteLine( "; 非フォーカス時のsleep値[ms]" );	    			    // #23568 2011.11.04 ikanick add
			sw.WriteLine( "; A sleep time[ms] while the window is inactive." );	//
			sw.WriteLine( "BackSleep={0}", this.n非フォーカス時スリープms );		// そのまま引用（苦笑）
			sw.WriteLine();											        			//
			#endregion
			#region [ フレーム処理関連(VSync, フレーム毎のsleep) ]
			sw.WriteLine("; 垂直帰線同期(0:OFF,1:ON)");
			sw.WriteLine( "VSyncWait={0}", this.b垂直帰線待ちを行う ? 1 : 0 );
            sw.WriteLine();
			sw.WriteLine( "; フレーム毎のsleep値[ms] (-1でスリープ無し, 0以上で毎フレームスリープ。動画キャプチャ等で活用下さい)" );	// #xxxxx 2011.11.27 yyagi add
			sw.WriteLine( "; A sleep time[ms] per frame." );							//
			sw.WriteLine( "SleepTimePerFrame={0}", this.nフレーム毎スリープms );		//
			sw.WriteLine();											        			//
			#endregion
			#region [ WASAPI/ASIO関連 ]
			sw.WriteLine( "; サウンド出力方式(0=ACM(って今はまだDirectSoundですが), 1=ASIO, 2=WASAPI)" );
			sw.WriteLine( "; WASAPIはVista以降のOSで使用可能。推奨方式はWASAPI。" );
			sw.WriteLine( "; なお、WASAPIが使用不可ならASIOを、ASIOが使用不可ならACMを使用します。" );
			sw.WriteLine( "; Sound device type(0=ACM, 1=ASIO, 2=WASAPI)" );
			sw.WriteLine( "; WASAPI can use on Vista or later OSs." );
			sw.WriteLine( "; If WASAPI is not available, DTXMania try to use ASIO. If ASIO can't be used, ACM is used." );
			sw.WriteLine( "SoundDeviceType={0}", (int) this.nSoundDeviceType );
			sw.WriteLine();

			sw.WriteLine( "; WASAPI使用時のサウンドバッファサイズ" );
			sw.WriteLine( "; (0=デバイスに設定されている値を使用, 1～9999=バッファサイズ(単位:ms)の手動指定" );
			sw.WriteLine( "; WASAPI Sound Buffer Size." );
			sw.WriteLine( "; (0=Use system default buffer size, 1-9999=specify the buffer size(ms) by yourself)" );
			sw.WriteLine( "WASAPIBufferSizeMs={0}", (int) this.nWASAPIBufferSizeMs );
			sw.WriteLine();

			sw.WriteLine( "; ASIO使用時のサウンドデバイス" );
			sw.WriteLine( "; 存在しないデバイスを指定すると、DTXManiaが起動しないことがあります。" );
			sw.WriteLine( "; Sound device used by ASIO." );
			sw.WriteLine( "; Don't specify unconnected device, as the DTXMania may not bootup." );
			string[] asiodev = CEnumerateAllAsioDevices.GetAllASIODevices();
			for ( int i = 0; i < asiodev.Length; i++ )
			{
				sw.WriteLine( "; {0}: {1}", i, asiodev[ i ] );
			}
			sw.WriteLine( "ASIODevice={0}", (int) this.nASIODevice );
			sw.WriteLine();

			//sw.WriteLine( "; ASIO使用時のサウンドバッファサイズ" );
			//sw.WriteLine( "; (0=デバイスに設定されている値を使用, 1～9999=バッファサイズ(単位:ms)の手動指定" );
			//sw.WriteLine( "; ASIO Sound Buffer Size." );
			//sw.WriteLine( "; (0=Use the value specified to the device, 1-9999=specify the buffer size(ms) by yourself)" );
			//sw.WriteLine( "ASIOBufferSizeMs={0}", (int) this.nASIOBufferSizeMs );
			//sw.WriteLine();

			//sw.WriteLine( "; Bass.Mixの制御を動的に行うか否か。" );
			//sw.WriteLine( "; ONにすると、ギター曲などチップ音の多い曲も再生できますが、画面が少しがたつきます。" );
			//sw.WriteLine( "; (0=行わない, 1=行う)" );
			//sw.WriteLine( "DynamicBassMixerManagement={0}", this.bDynamicBassMixerManagement ? 1 : 0 );
			//sw.WriteLine();

			sw.WriteLine( "; WASAPI/ASIO時に使用する演奏タイマーの種類" );
			sw.WriteLine( "; Playback timer used for WASAPI/ASIO" );
			sw.WriteLine( "; (0=FDK Timer, 1=System Timer)" );
			sw.WriteLine( "SoundTimerType={0}", this.bUseOSTimer ? 1 : 0 );
			sw.WriteLine();

			//sw.WriteLine( "; 全体ボリュームの設定" );
			//sw.WriteLine( "; (0=無音 ～ 100=最大。WASAPI/ASIO時のみ有効)" );
			//sw.WriteLine( "; Master volume settings" );
			//sw.WriteLine( "; (0=Silent - 100=Max)" );
			//sw.WriteLine( "MasterVolume={0}", this.nMasterVolume );
			//sw.WriteLine();

			#endregion
			sw.WriteLine( "; 背景画像の半透明割合(0:透明～255:不透明)" );
			sw.WriteLine( "; Transparency for background image in playing screen.(0:tranaparent - 255:no transparent)" );
			sw.WriteLine( "BGAlpha={0}", this.nBGAlpha );
			sw.WriteLine();
			sw.WriteLine( "; Missヒット時のゲージ減少割合(0:少, 1:普通, 2:大)" );
			sw.WriteLine( "DamageLevel={0}", (int) this.eダメージレベル );
			sw.WriteLine();
			sw.WriteLine( "; ゲージゼロでSTAGE FAILED (0:OFF, 1:ON)" );
			sw.WriteLine( "StageFailed={0}", this.bSTAGEFAILED有効 ? 1 : 0 );
			sw.WriteLine();
			#region [ AVI/BGA ]
			sw.WriteLine( "; AVIの表示(0:OFF, 1:ON)" );
			sw.WriteLine( "AVI={0}", this.bAVI有効 ? 1 : 0 );
			sw.WriteLine();
			sw.WriteLine( "; BGAの表示(0:OFF, 1:ON)" );
			sw.WriteLine( "BGA={0}", this.bBGA有効 ? 1 : 0 );
			sw.WriteLine();
			sw.WriteLine( "; 動画表示モード( 0:表示しない, 1:背景のみ, 2:窓表示のみ, 3:両方)" );
			sw.WriteLine( "ClipDispType={0}", (int) this.eClipDispType );
			sw.WriteLine();
			#endregion
			#region [ フィルイン ]
			sw.WriteLine( "; フィルイン効果(0:OFF, 1:ON)" );
			sw.WriteLine( "FillInEffect={0}", this.bフィルイン有効 ? 1 : 0 );
			sw.WriteLine();
			sw.WriteLine( "; フィルイン達成時の歓声の再生(0:OFF, 1:ON)" );
			sw.WriteLine( "AudienceSound={0}", this.b歓声を発声する ? 1 : 0 );
			sw.WriteLine();
			#endregion
			#region [ プレビュー音 ]
			sw.WriteLine( "; 曲選択からプレビュー音の再生までのウェイト[ms]" );
			sw.WriteLine( "PreviewSoundWait={0}", this.n曲が選択されてからプレビュー音が鳴るまでのウェイトms );
			sw.WriteLine();
			sw.WriteLine( "; 曲選択からプレビュー画像表示までのウェイト[ms]" );
			sw.WriteLine( "PreviewImageWait={0}", this.n曲が選択されてからプレビュー画像が表示開始されるまでのウェイトms );
			sw.WriteLine();
			#endregion
			//sw.WriteLine( "; Waveの再生位置自動補正(0:OFF, 1:ON)" );
			//sw.WriteLine( "AdjustWaves={0}", this.bWave再生位置自動調整機能有効 ? 1 : 0 );
			sw.WriteLine();
			#region [ BGM/ドラムヒット音の再生 ]
			sw.WriteLine( "; BGM の再生(0:OFF, 1:ON)" );
			sw.WriteLine( "BGMSound={0}", this.bBGM音を発声する ? 1 : 0 );
			sw.WriteLine();
			#endregion
			sw.WriteLine( "; 演奏記録（～.score.ini）の出力 (0:OFF, 1:ON)" );
			sw.WriteLine( "SaveScoreIni={0}", this.bScoreIniを出力する ? 1 : 0 );
			sw.WriteLine();
			sw.WriteLine( "; RANDOM SELECT で子BOXを検索対象に含める (0:OFF, 1:ON)" );
			sw.WriteLine( "RandomFromSubBox={0}", this.bランダムセレクトで子BOXを検索対象とする ? 1 : 0 );
			sw.WriteLine();
			#region [ モニターサウンド(ヒット音の再生音量アップ) ]
			sw.WriteLine( "; ドラム演奏時にドラム音を強調する (0:OFF, 1:ON)" );
			sw.WriteLine( "SoundMonitorDrums={0}", this.b演奏音を強調する.Drums ? 1 : 0 );
			sw.WriteLine();
			sw.WriteLine( "; ギター演奏時にギター音を強調する (0:OFF, 1:ON)" );
			sw.WriteLine( "SoundMonitorGuitar={0}", this.b演奏音を強調する.Guitar ? 1 : 0 );
			sw.WriteLine();
			sw.WriteLine( "; ベース演奏時にベース音を強調する (0:OFF, 1:ON)" );
			sw.WriteLine( "SoundMonitorBass={0}", this.b演奏音を強調する.Bass ? 1 : 0 );
			sw.WriteLine();
			#endregion
			sw.WriteLine( "; 演奏情報を表示する (0:OFF, 1:ON)" );
			sw.WriteLine( "; Showing playing info on the playing screen. (0:OFF, 1:ON)" );
			sw.WriteLine( "ShowDebugStatus={0}", this.b演奏情報を表示する ? 1 : 0 );
			sw.WriteLine();
			#region [ 選曲リストのフォント ]
			sw.WriteLine( "; 選曲リストのフォント名" );
			sw.WriteLine( "; Font name for select song item." );
			sw.WriteLine( "SelectListFontName={0}", this.str選曲リストフォント );
			sw.WriteLine();
			sw.WriteLine( "; 選曲リストのフォントのサイズ[dot]" );
			sw.WriteLine( "; Font size[dot] for select song item." );
			sw.WriteLine( "SelectListFontSize={0}", this.n選曲リストフォントのサイズdot );
			sw.WriteLine();
			sw.WriteLine( "; 選曲リストのフォントを斜体にする (0:OFF, 1:ON)" );
			sw.WriteLine( "; Using italic font style select song list. (0:OFF, 1:ON)" );
			sw.WriteLine( "SelectListFontItalic={0}", this.b選曲リストフォントを斜体にする ? 1 : 0 );
			sw.WriteLine();
			sw.WriteLine( "; 選曲リストのフォントを太字にする (0:OFF, 1:ON)" );
			sw.WriteLine( "; Using bold font style select song list. (0:OFF, 1:ON)" );
			sw.WriteLine( "SelectListFontBold={0}", this.b選曲リストフォントを太字にする ? 1 : 0 );
			sw.WriteLine();
			sw.WriteLine( "; PrivateFontのフォント名" );
			sw.WriteLine( "; Font name for select song item." );
			sw.WriteLine( "PrivateFontFontName={0}", this.strPrivateFontで使うフォント名 );
			sw.WriteLine();
			#endregion
			sw.WriteLine( "; 打音の音量(0～100%)" );
			sw.WriteLine( "; Sound volume (you're playing) (0-100%)" );
			sw.WriteLine( "ChipVolume={0}", this.n手動再生音量 );
			sw.WriteLine();
			sw.WriteLine( "; 自動再生音の音量(0～100%)" );
			sw.WriteLine( "; Sound volume (auto playing) (0-100%)" );
			sw.WriteLine( "AutoChipVolume={0}", this.n自動再生音量 );
			sw.WriteLine();
			sw.WriteLine( "; ストイックモード(0:OFF, 1:ON)" );
			sw.WriteLine( "; Stoic mode. (0:OFF, 1:ON)" );
			sw.WriteLine( "StoicMode={0}", this.bストイックモード ? 1 : 0 );
			sw.WriteLine();
			sw.WriteLine( "; バッファ入力モード(0:OFF, 1:ON)" );
			sw.WriteLine( "; Using Buffered input (0:OFF, 1:ON)" );
			sw.WriteLine( "BufferedInput={0}", this.bバッファ入力を行う ? 1 : 0 );
			sw.WriteLine();
			sw.WriteLine( "; 判定ズレ時間表示(0:OFF, 1:ON, 2=GREAT-POOR)" );				// #25370 2011.6.3 yyagi
			sw.WriteLine( "; Whether displaying the lag times from the just timing or not." );	//
			sw.WriteLine( "ShowLagTime={0}", this.nShowLagType );							//
			sw.WriteLine();
			sw.WriteLine( "; リザルト画像自動保存機能(0:OFF, 1:ON)" );						// #25399 2011.6.9 yyagi
			sw.WriteLine( "; Set \"1\" if you'd like to save result screen image automatically");	//
			sw.WriteLine( "; when you get hiscore/hiskill.");								//
			sw.WriteLine( "AutoResultCapture={0}", this.bIsAutoResultCapture? 1 : 0 );		//
			sw.WriteLine();
			sw.WriteLine( "; 再生速度変更を、ピッチ変更で行うかどうか(0:ピッチ変更, 1:タイムストレッチ" );	// #23664 2013.2.24 yyagi
			sw.WriteLine( "; (WASAPI/ASIO使用時のみ有効) " );
			sw.WriteLine( "; Set \"0\" if you'd like to use pitch shift with PlaySpeed." );	//
			sw.WriteLine( "; Set \"1\" for time stretch." );								//
			sw.WriteLine( "; (Only available when you're using using WASAPI or ASIO)" );	//
			sw.WriteLine( "TimeStretch={0}", this.bTimeStretch ? 1 : 0 );					//
			sw.WriteLine();
			//sw.WriteLine( "; WASAPI/ASIO使用時に、MP3をストリーム再生するかどうか(0:ストリーム再生する, 1:しない)" );			//
			//sw.WriteLine( "; (mp3のシークがおかしくなる場合は、これを1にしてください) " );	//
			//sw.WriteLine( "; Set \"0\" if you'd like to use mp3 streaming playback on WASAPI/ASIO." );		//
			//sw.WriteLine( "; Set \"1\" not to use streaming playback for mp3." );			//
			//sw.WriteLine( "; (If you feel illegal seek with mp3, please set it to 1.)" );	//
			//sw.WriteLine( "NoMP3Streaming={0}", this.bNoMP3Streaming ? 1 : 0 );				//
			//sw.WriteLine();
            sw.WriteLine( "; 動画再生にDirectShowを使用する(0:OFF, 1:ON)" );
			sw.WriteLine( "; 動画再生にDirectShowを使うことによって、再生時の負担を軽減できます。");
			sw.WriteLine( "; ただし使用時にはセットアップが必要になるのでご注意ください。");
			sw.WriteLine( "DirectShowMode={0}", this.bDirectShowMode ? 1 : 0 );

			#region [ Adjust ]
			sw.WriteLine( "; 判定タイミング調整(ドラム, ギター, ベース)(-99～99)[ms]" );		// #23580 2011.1.3 yyagi
			sw.WriteLine("; Revision value to adjust judgement timing for the drums, guitar and bass.");	//
			sw.WriteLine("InputAdjustTimeDrums={0}", this.nInputAdjustTimeMs.Drums);		//
			sw.WriteLine("InputAdjustTimeGuitar={0}", this.nInputAdjustTimeMs.Guitar);		//
			sw.WriteLine("InputAdjustTimeBass={0}", this.nInputAdjustTimeMs.Bass);			//
			sw.WriteLine();

			sw.WriteLine( "; 判定ラインの表示位置調整(ドラム, ギター, ベース)(-99～99)[px]" );	// #31602 2013.6.23 yyagi 判定ラインの表示位置オフセット
			sw.WriteLine( "; Offset value to adjust displaying judgement line for the drums, guitar and bass." );	//
			sw.WriteLine( "JudgeLinePosOffsetDrums={0}",  this.nJudgeLinePosOffset.Drums );		//
			sw.WriteLine( "JudgeLinePosOffsetGuitar={0}", this.nJudgeLinePosOffset.Guitar );	//
			sw.WriteLine( "JudgeLinePosOffsetBass={0}",   this.nJudgeLinePosOffset.Bass );		//

			sw.WriteLine( "; 判定ラインの表示位置(ギター, ベース)" );	// #33891 2014.6.26 yyagi
			sw.WriteLine( "; 0=Normal, 1=Lower" );
			sw.WriteLine( "; Position of the Judgement line and RGB button; Vseries compatible(1) or not(0)." );	//
			sw.WriteLine( "JudgeLinePosModeGuitar={0}", (int) this.e判定位置.Guitar );	//
			sw.WriteLine( "JudgeLinePosModeBass={0}  ", (int) this.e判定位置.Bass );	//
			
			sw.WriteLine();
			#endregion
            sw.WriteLine( "; 「また遊んでね」画面(0:OFF, 1:ON)" );
            sw.WriteLine( "EndingAnime={0}", this.bEndingAnime ? 1 : 0 );
            sw.WriteLine();
			sw.WriteLine( ";-------------------" );
			#endregion
			#region [ Log ]
			sw.WriteLine( "[Log]" );
			sw.WriteLine();
			sw.WriteLine( "; Log出力(0:OFF, 1:ON)" );
			sw.WriteLine( "OutputLog={0}", this.bログ出力 ? 1 : 0 );
			sw.WriteLine();
			sw.WriteLine( "; 曲データ検索に関するLog出力(0:OFF, 1:ON)" );
			sw.WriteLine( "TraceSongSearch={0}", this.bLog曲検索ログ出力 ? 1 : 0 );
			sw.WriteLine();
			sw.WriteLine( "; 画像やサウンドの作成_解放に関するLog出力(0:OFF, 1:ON)" );
			sw.WriteLine( "TraceCreatedDisposed={0}", this.bLog作成解放ログ出力 ? 1 : 0 );
			sw.WriteLine();
			sw.WriteLine( "; DTX読み込み詳細に関するLog出力(0:OFF, 1:ON)" );
			sw.WriteLine( "TraceDTXDetails={0}", this.bLogDTX詳細ログ出力 ? 1 : 0 );
			sw.WriteLine();
			sw.WriteLine( ";-------------------" );
			#endregion

			#region [ PlayOption ]
			sw.WriteLine( "[PlayOption]" );
			sw.WriteLine();
			sw.WriteLine( "; DARKモード(0:OFF, 1:HALF, 2:FULL)" );
			sw.WriteLine( "Dark={0}", (int) this.eDark );
			sw.WriteLine();
            /*
            sw.WriteLine( "; スクロール方法(※β版)" );
            sw.WriteLine( "; (0:通常, 1:BMSCROLL, 2:HSSCROLL)" );
            sw.WriteLine( "ScrollMode={0}", (int)this.eScrollMode );
            sw.WriteLine();
            */
			#region [ SUDDEN ]
			sw.WriteLine( "; ドラムSUDDENモード(0:OFF, 1:ON)" );
			sw.WriteLine( "DrumsSudden={0}", this.bSudden.Drums ? 1 : 0 );
			sw.WriteLine();
			sw.WriteLine( "; ギターSUDDENモード(0:OFF, 1:ON)" );
			sw.WriteLine( "GuitarSudden={0}", this.bSudden.Guitar ? 1 : 0 );
			sw.WriteLine();
			sw.WriteLine( "; ベースSUDDENモード(0:OFF, 1:ON)" );
			sw.WriteLine( "BassSudden={0}", this.bSudden.Bass ? 1 : 0 );
			sw.WriteLine();
			#endregion
			#region [ HIDDEN ]
			sw.WriteLine( "; ドラムHIDDENモード(0:OFF, 1:ON)" );
			sw.WriteLine( "DrumsHidden={0}", this.bHidden.Drums ? 1 : 0 );
			sw.WriteLine();
			sw.WriteLine( "; ギターHIDDENモード(0:OFF, 1:ON)" );
			sw.WriteLine( "GuitarHidden={0}", this.bHidden.Guitar ? 1 : 0 );
			sw.WriteLine();
			sw.WriteLine( "; ベースHIDDENモード(0:OFF, 1:ON)" );
			sw.WriteLine( "BassHidden={0}", this.bHidden.Bass ? 1 : 0 );
			sw.WriteLine();
			#endregion
			#region [ Invisible ]
			sw.WriteLine( "; ドラムチップ非表示モード (0:OFF, 1=SEMI, 2:FULL)" );
			sw.WriteLine( "; Drums chip invisible mode" );
			sw.WriteLine( "DrumsInvisible={0}", (int) this.eInvisible.Drums );
			sw.WriteLine();
			sw.WriteLine( "; ギターチップ非表示モード (0:OFF, 1=SEMI, 2:FULL)" );
			sw.WriteLine( "; Guitar chip invisible mode" );
			sw.WriteLine( "GuitarInvisible={0}", (int) this.eInvisible.Guitar );
			sw.WriteLine();
			sw.WriteLine( "; ベースチップ非表示モード (0:OFF, 1=SEMI, 2:FULL)" );
			sw.WriteLine( "; Bbass chip invisible mode" );
			sw.WriteLine( "BassInvisible={0}", (int) this.eInvisible.Bass );
			sw.WriteLine();
			//sw.WriteLine( "; Semi-InvisibleでMissった時のチップ再表示時間(ms)" );
			//sw.WriteLine( "InvisibleDisplayTimeMs={0}", (int) this.nDisplayTimesMs );
			//sw.WriteLine();
			//sw.WriteLine( "; Semi-InvisibleでMissってチップ再表示時間終了後のフェードアウト時間(ms)" );
			//sw.WriteLine( "InvisibleFadeoutTimeMs={0}", (int) this.nFadeoutTimeMs );
			//sw.WriteLine();
			#endregion
			sw.WriteLine( "; ドラムREVERSEモード(0:OFF, 1:ON)" );
			sw.WriteLine( "DrumsReverse={0}", this.bReverse.Drums ? 1 : 0 );
			sw.WriteLine();
			sw.WriteLine( "; ギターREVERSEモード(0:OFF, 1:ON)" );
			sw.WriteLine( "GuitarReverse={0}", this.bReverse.Guitar ? 1 : 0 );
			sw.WriteLine();
			sw.WriteLine( "; ベースREVERSEモード(0:OFF, 1:ON)" );
			sw.WriteLine( "BassReverse={0}", this.bReverse.Bass ? 1 : 0 );
			sw.WriteLine();
			sw.WriteLine( "; ギターRANDOMモード(0:OFF, 1:Random, 2:SuperRandom, 3:HyperRandom)" );
			sw.WriteLine( "GuitarRandom={0}", (int) this.eRandom.Guitar );
			sw.WriteLine();
			sw.WriteLine( "; ベースRANDOMモード(0:OFF, 1:Random, 2:SuperRandom, 3:HyperRandom)" );
			sw.WriteLine( "BassRandom={0}", (int) this.eRandom.Bass );
			sw.WriteLine();
			sw.WriteLine( "; RISKYモード(0:OFF, 1-10)" );									// #23559 2011.6.23 yyagi
			sw.WriteLine( "; RISKY mode. 0=OFF, 1-10 is the times of misses to be Failed." );	//
			sw.WriteLine( "Risky={0}", this.nRisky );			//
			sw.WriteLine();
			sw.WriteLine( "; TIGHTモード(0:OFF, 1:ON)" );									// #29500 2012.9.11 kairera0467
			sw.WriteLine( "; TIGHT mode. 0=OFF, 1=ON " );
			sw.WriteLine( "TaikoTight={0}", this.bTight ? 1 : 0 );
			sw.WriteLine( "TaikoTight2P={0}", this.bTight2P ? 1 : 0 );
			sw.WriteLine();
			sw.WriteLine( "; ドラム譜面スクロール速度(0:x0.5, 1:x1.0, 2:x1.5,…,1999:x1000.0)" );
			sw.WriteLine( "DrumsScrollSpeed={0}", this.n譜面スクロール速度.Drums );
			sw.WriteLine();
			sw.WriteLine( "; ギター譜面スクロール速度(0:x0.5, 1:x1.0, 2:x1.5,…,1999:x1000.0)" );
			sw.WriteLine( "GuitarScrollSpeed={0}", this.n譜面スクロール速度.Guitar );
			sw.WriteLine();
			sw.WriteLine( "; ベース譜面スクロール速度(0:x0.5, 1:x1.0, 2:x1.5,…,1999:x1000.0)" );
			sw.WriteLine( "BassScrollSpeed={0}", this.n譜面スクロール速度.Bass );
			sw.WriteLine();
			sw.WriteLine( "; 演奏速度(5～40)(→x5/20～x40/20)" );
			sw.WriteLine( "PlaySpeed={0}", this.n演奏速度 );
			sw.WriteLine();

            sw.WriteLine("; デフォルトで選択される難易度");
            sw.WriteLine("DefaultCourse={0}", this.nDefaultCourse);
            sw.WriteLine();
            sw.WriteLine( "; 譜面分岐のガイド表示(0:OFF, 1:ON)" );
			sw.WriteLine( "BranchGuide={0}", this.bGraph.Drums ? 1 : 0 );
			sw.WriteLine();
			sw.WriteLine( "; スコア計算方法(0:1～7, 1:8～14, 2:15以降, 3:真打)" );
			sw.WriteLine( "ScoreMode={0}", this.nScoreMode );
			sw.WriteLine();
			//sw.WriteLine( "; 1ノーツごとのスクロール速度をランダムで変更します。(0:OFF, 1:ON)" );
			//sw.WriteLine( "HispeedRandom={0}", this.bHispeedRandom ? 1 : 0 );
			//sw.WriteLine();
			sw.WriteLine( "; 大音符の両手入力待機時間(ms)" );
			sw.WriteLine( "BigNotesWaitTime={0}", this.n両手判定の待ち時間 );
			sw.WriteLine();
			sw.WriteLine( "; 大音符の両手判定(0:OFF, 1:ON)" );
			sw.WriteLine( "BigNotesJudge={0}", this.b大音符判定 ? 1 : 0 );
			sw.WriteLine();
			sw.WriteLine( "; NoInfo(0:OFF, 1:ON)" );
			sw.WriteLine( "NoInfo={0}", this.bNoInfo ? 1 : 0 );
			sw.WriteLine();
			sw.WriteLine( "; 譜面分岐のアニメーション(0:7～14, 1:15)" );
			sw.WriteLine( "BranchAnime={0}", this.nBranchAnime );
			sw.WriteLine();
            sw.WriteLine( "; デフォルトの曲ソート(0:絶対パス順, 1:ジャンル名ソートOLD, 2:ジャンル名ソートNEW )" );
            sw.WriteLine( "0:Path, 1:GenreName(AC8～AC14), 2GenreName(AC15～)" );
            sw.WriteLine( "DefaultSongSort={0}", this.nDefaultSongSort );
            sw.WriteLine();

			sw.WriteLine( "; キャラクター画像(β版)を有効にする(ズレがあるのでまだβ)" );
			sw.WriteLine( "Chara={0}", this.bChara ? 1 : 0 );

			sw.WriteLine( "; キャラクターの画像数" );
			sw.WriteLine( "CharaMotionCount={0}", this.nCharaMotionCount );
            sw.WriteLine( "CharaMotionCountGogo={0}", this.nCharaMotionCount_gogo );
			sw.WriteLine( "CharaMotionCountClear={0}", this.nCharaMotionCount_clear );
            sw.WriteLine( "CharaMotionCountMax={0}", this.nCharaMotionCount_max );
            sw.WriteLine( "CharaMotionCountMaxGogo={0}", this.nCharaMotionCount_maxgogo );
			sw.WriteLine();

			sw.WriteLine( "; キャラクターのコマパターン" );
			sw.WriteLine( "CharaMotionList={0}", this.strCharaMotionList );
            sw.WriteLine( "CharaMotionListGogo={0}", this.strCharaMotionList_gogo );
            sw.WriteLine( "CharaMotionListClear={0}", this.strCharaMotionList_clear );
            sw.WriteLine( "CharaMotionListMax={0}", this.strCharaMotionList_max );
            sw.WriteLine( "CharaMotionListMaxGogo={0}", this.strCharaMotionList_maxgogo );
			sw.WriteLine();

			sw.WriteLine( "; キャラクターのモーション周期(β版)" );
            sw.WriteLine( "; 1拍単位で指定できます。" );
			sw.WriteLine( "CharaMotionLoopBeats={0}", this.nCharaMotionLoopBeats );
			sw.WriteLine( "CharaMotionLoopBeatsClear={0}", this.nCharaMotionLoopBeats_clear );

			sw.WriteLine( "; RANDOMモード(0:OFF, 1:Random, 2:Mirorr 3:SuperRandom, 4:HyperRandom)" );
			sw.WriteLine( "TaikoRandom={0}", (int) this.eRandom.Taiko );
			sw.WriteLine();
            sw.WriteLine( "; STEALTHモード(0:OFF, 1:ドロン, 2:ステルス)" );
			sw.WriteLine( "TaikoStealth={0}", (int) this.eSTEALTH );
			sw.WriteLine();
            sw.WriteLine( "; ゲーム(0:OFF, 1:完走!叩ききりまショー!, 2:完走!叩ききりまショー!(激辛) )" );
			sw.WriteLine( "GameMode={0}", (int) this.eGameMode );
			sw.WriteLine();
            sw.WriteLine( "; JUST(0:OFF, 1:ON)" );
			sw.WriteLine( "Just={0}", this.bJust ? 1 : 0 );
			sw.WriteLine();
            sw.WriteLine( "; 判定数の表示(0:OFF, 1:ON)" );
			sw.WriteLine( "JudgeCountDisplay={0}", this.bJudgeCountDisplay ? 1 : 0 );
			sw.WriteLine();
            sw.WriteLine( "; プレイ人数" );
            sw.WriteLine( "PlayerCount={0}", this.nPlayerCount );

			sw.WriteLine( ";-------------------" );
			#endregion

			#region [ ViewerOption ]
			sw.WriteLine( "[ViewerOption]" );
			sw.WriteLine();
			sw.WriteLine( "; Viewerモード時専用 ドラム譜面スクロール速度(0:x0.5, 1:x1.0, 2:x1.5,…,1999:x1000.0)" );
			sw.WriteLine( "; for viewer mode; Drums Scroll Speed" );
			sw.WriteLine( "ViewerDrumsScrollSpeed={0}", this.nViewerScrollSpeed.Drums );
			sw.WriteLine();
			sw.WriteLine( "; Viewerモード時専用 ギター譜面スクロール速度(0:x0.5, 1:x1.0, 2:x1.5,…,1999:x1000.0)");
			sw.WriteLine( "; for viewer mode; Guitar Scroll Speed" );
			sw.WriteLine( "ViewerGuitarScrollSpeed={0}", this.nViewerScrollSpeed.Guitar );
			sw.WriteLine();
			sw.WriteLine( "; Viewerモード時専用 ベース譜面スクロール速度(0:x0.5, 1:x1.0, 2:x1.5,…,1999:x1000.0)");
			sw.WriteLine( "; for viewer mode; Bass Scroll Speed" );
			sw.WriteLine( "ViewerBassScrollSpeed={0}", this.nViewerScrollSpeed.Bass );
			sw.WriteLine();
			sw.WriteLine( "; Viewerモード時専用 垂直帰線同期(0:OFF,1:ON)" );
			sw.WriteLine( "; for viewer mode; Use whether Vertical Sync or not." );
			sw.WriteLine( "ViewerVSyncWait={0}", this.bViewerVSyncWait ? 1 : 0 );
			sw.WriteLine();
			sw.WriteLine( "; Viewerモード時専用 演奏情報を表示する (0:OFF, 1:ON) ");
			sw.WriteLine( "; for viewer mode;" );
			sw.WriteLine( "; Showing playing info on the playing screen. (0:OFF, 1:ON) " );
			sw.WriteLine( "ViewerShowDebugStatus={0}", this.bViewerShowDebugStatus? 1 : 0 );
			sw.WriteLine();
			sw.WriteLine( "; Viewerモード時専用 再生速度変更を、ピッチ変更で行うかどうか(0:ピッチ変更, 1:タイムストレッチ ");
			sw.WriteLine( "; (WASAPI/ASIO使用時のみ有効)  ");
			sw.WriteLine( "; for viewer mode;" );
			sw.WriteLine( "; Set \"0\" if you'd like to use pitch shift with PlaySpeed. " );
			sw.WriteLine( "; Set \"1\" for time stretch. " );
			sw.WriteLine( "; (Only available when you're using using WASAPI or ASIO) ");
			sw.WriteLine( "ViewerTimeStretch={0}", this.bViewerTimeStretch? 1 : 0 );
			sw.WriteLine();
			sw.WriteLine( "; Viewerモード時専用 ギター/ベース有効(0:OFF,1:ON) ");
			sw.WriteLine( "; for viewer mode;" );
			sw.WriteLine( "; Enable Guitar/Bass or not.(0:OFF,1:ON) " );
			sw.WriteLine( "ViewerGuitar={0}", this.bViewerGuitar有効? 1 : 0 );
			sw.WriteLine();
			sw.WriteLine( "; Viewerモード時専用 ドラム有効(0:OFF,1:ON) ");
			sw.WriteLine( "; for viewer mode;" );
			sw.WriteLine( "; Enable Drums or not.(0:OFF,1:ON) " );
			sw.WriteLine( "ViewerDrums={0}", this.bViewerDrums有効? 1 : 0 );
			sw.WriteLine();
			sw.WriteLine( ";-------------------" );
			#endregion

			#region [ AutoPlay ]
			sw.WriteLine( "[AutoPlay]" );
			sw.WriteLine();
			sw.WriteLine( "; 自動演奏(0:OFF, 1:ON)" );
            sw.WriteLine( "Taiko={0}", this.b太鼓パートAutoPlay ? 1 : 0 );
            sw.WriteLine( "Taiko2P={0}", this.b太鼓パートAutoPlay2P ? 1 : 0 );
            sw.WriteLine( "TaikoAutoRoll={0}", this.bAuto先生の連打 ? 1 : 0 );
            sw.WriteLine();
			sw.WriteLine( ";-------------------" );
			#endregion

			#region [ HitRange ]
			sw.WriteLine( "[HitRange]" );
			sw.WriteLine();
			sw.WriteLine( "; Perfect～Poor とみなされる範囲[ms]" );
			sw.WriteLine( "Perfect={0}", this.nヒット範囲ms.Perfect );
			sw.WriteLine( "Good={0}", this.nヒット範囲ms.Good );
			sw.WriteLine( "Poor={0}", this.nヒット範囲ms.Poor );
			sw.WriteLine();
			sw.WriteLine( ";-------------------" );
			#endregion
			#region [ GUID ]
			sw.WriteLine( "[GUID]" );
			sw.WriteLine();
			foreach( KeyValuePair<int, string> pair in this.dicJoystick )
			{
				sw.WriteLine( "JoystickID={0},{1}", pair.Key, pair.Value );
			}
			#endregion
			#region [ DrumsKeyAssign ]
			sw.WriteLine();
			sw.WriteLine( ";-------------------" );
			sw.WriteLine( "; キーアサイン" );
			sw.WriteLine( ";   項　目：Keyboard → 'K'＋'0'＋キーコード(10進数)" );
			sw.WriteLine( ";           Mouse    → 'N'＋'0'＋ボタン番号(0～7)" );
			sw.WriteLine( ";           MIDI In  → 'M'＋デバイス番号1桁(0～9,A～Z)＋ノート番号(10進数)" );
			sw.WriteLine( ";           Joystick → 'J'＋デバイス番号1桁(0～9,A～Z)＋ 0 ...... Ｘ減少(左)ボタン" );
			sw.WriteLine( ";                                                         1 ...... Ｘ増加(右)ボタン" );
			sw.WriteLine( ";                                                         2 ...... Ｙ減少(上)ボタン" );
			sw.WriteLine( ";                                                         3 ...... Ｙ増加(下)ボタン" );
			sw.WriteLine( ";                                                         4 ...... Ｚ減少(前)ボタン" );
			sw.WriteLine( ";                                                         5 ...... Ｚ増加(後)ボタン" );
			sw.WriteLine( ";                                                         6～133.. ボタン1～128" );
			sw.WriteLine( ";           これらの項目を 16 個まで指定可能(',' で区切って記述）。" );
			sw.WriteLine( ";" );
			sw.WriteLine( ";   表記例：HH=K044,M042,J16" );
			sw.WriteLine( ";           → HiHat を Keyboard の 44 ('Z'), MidiIn#0 の 42, JoyPad#1 の 6(ボタン1) に割当て" );
			sw.WriteLine( ";" );
			sw.WriteLine( ";   ※Joystick のデバイス番号とデバイスとの関係は [GUID] セクションに記してあるものが有効。" );
			sw.WriteLine( ";" );
			sw.WriteLine();
			sw.WriteLine( "[DrumsKeyAssign]" );
			sw.WriteLine();
			sw.Write( "HH=" );
			this.tキーの書き出し( sw, this.KeyAssign.Drums.HH );
			sw.WriteLine();
			sw.Write( "SD=" );
			this.tキーの書き出し( sw, this.KeyAssign.Drums.SD );
			sw.WriteLine();
			sw.Write( "BD=" );
			this.tキーの書き出し( sw, this.KeyAssign.Drums.BD );
			sw.WriteLine();
			sw.Write( "HT=" );
			this.tキーの書き出し( sw, this.KeyAssign.Drums.HT );
			sw.WriteLine();
			sw.Write( "LT=" );
			this.tキーの書き出し( sw, this.KeyAssign.Drums.LT );
			sw.WriteLine();
			sw.Write( "FT=" );
			this.tキーの書き出し( sw, this.KeyAssign.Drums.FT );
			sw.WriteLine();
			sw.Write( "CY=" );
			this.tキーの書き出し( sw, this.KeyAssign.Drums.CY );
			sw.WriteLine();
			sw.Write( "HO=" );
			this.tキーの書き出し( sw, this.KeyAssign.Drums.HHO );
			sw.WriteLine();
			sw.Write( "RD=" );
			this.tキーの書き出し( sw, this.KeyAssign.Drums.RD );
			sw.WriteLine();
			sw.Write( "LC=" );
			this.tキーの書き出し( sw, this.KeyAssign.Drums.LC );
			sw.WriteLine();
			sw.Write( "LP=" );										// #27029 2012.1.4 from
			this.tキーの書き出し( sw, this.KeyAssign.Drums.LP );	//
			sw.WriteLine();											//
			sw.Write( "LBD=" );										// #27029 2012.1.4 from
			this.tキーの書き出し( sw, this.KeyAssign.Drums.LBD );	//
			sw.WriteLine();
			sw.Write( "LeftRed=" );
			this.tキーの書き出し( sw, this.KeyAssign.Drums.LeftRed );
			sw.WriteLine();
			sw.Write( "RightRed=" );
			this.tキーの書き出し( sw, this.KeyAssign.Drums.RightRed );
			sw.WriteLine();
			sw.Write( "LeftBlue=" );										// #27029 2012.1.4 from
			this.tキーの書き出し( sw, this.KeyAssign.Drums.LeftBlue );	//
			sw.WriteLine();											//
			sw.Write( "RightBlue=" );										// #27029 2012.1.4 from
			this.tキーの書き出し( sw, this.KeyAssign.Drums.RightBlue );	//
			sw.WriteLine();
			sw.Write( "LeftRed2P=" );
			this.tキーの書き出し( sw, this.KeyAssign.Drums.LeftRed2P );
			sw.WriteLine();
			sw.Write( "RightRed2P=" );
			this.tキーの書き出し( sw, this.KeyAssign.Drums.RightRed2P );
			sw.WriteLine();
			sw.Write( "LeftBlue2P=" );										// #27029 2012.1.4 from
			this.tキーの書き出し( sw, this.KeyAssign.Drums.LeftBlue2P );	//
			sw.WriteLine();											        //
			sw.Write( "RightBlue2P=" );										// #27029 2012.1.4 from
			this.tキーの書き出し( sw, this.KeyAssign.Drums.RightBlue2P );	//
			sw.WriteLine();
			sw.WriteLine();
			#endregion
			#region [ GuitarKeyAssign ]
			sw.WriteLine( "[GuitarKeyAssign]" );
			sw.WriteLine();
			sw.Write( "R=" );
			this.tキーの書き出し( sw, this.KeyAssign.Guitar.R );
			sw.WriteLine();
			sw.Write( "G=" );
			this.tキーの書き出し( sw, this.KeyAssign.Guitar.G );
			sw.WriteLine();
			sw.Write( "B=" );
			this.tキーの書き出し( sw, this.KeyAssign.Guitar.B );
			sw.WriteLine();
			sw.Write( "Pick=" );
			this.tキーの書き出し( sw, this.KeyAssign.Guitar.Pick );
			sw.WriteLine();
			sw.Write( "Wail=" );
			this.tキーの書き出し( sw, this.KeyAssign.Guitar.Wail );
			sw.WriteLine();
			sw.Write( "Decide=" );
			this.tキーの書き出し( sw, this.KeyAssign.Guitar.Decide );
			sw.WriteLine();
			sw.Write( "Cancel=" );
			this.tキーの書き出し( sw, this.KeyAssign.Guitar.Cancel );
			sw.WriteLine();
			sw.WriteLine();
			#endregion
			#region [ BassKeyAssign ]
			sw.WriteLine( "[BassKeyAssign]" );
			sw.WriteLine();
			sw.Write( "R=" );
			this.tキーの書き出し( sw, this.KeyAssign.Bass.R );
			sw.WriteLine();
			sw.Write( "G=" );
			this.tキーの書き出し( sw, this.KeyAssign.Bass.G );
			sw.WriteLine();
			sw.Write( "B=" );
			this.tキーの書き出し( sw, this.KeyAssign.Bass.B );
			sw.WriteLine();
			sw.Write( "Pick=" );
			this.tキーの書き出し( sw, this.KeyAssign.Bass.Pick );
			sw.WriteLine();
			sw.Write( "Wail=" );
			this.tキーの書き出し( sw, this.KeyAssign.Bass.Wail );
			sw.WriteLine();
			sw.Write( "Decide=" );
			this.tキーの書き出し( sw, this.KeyAssign.Bass.Decide );
			sw.WriteLine();
			sw.Write( "Cancel=" );
			this.tキーの書き出し( sw, this.KeyAssign.Bass.Cancel );
			sw.WriteLine();
			sw.WriteLine();
			#endregion
			#region [ SystemkeyAssign ]
			sw.WriteLine( "[SystemKeyAssign]" );
			sw.WriteLine();
			sw.Write( "Capture=" );
			this.tキーの書き出し( sw, this.KeyAssign.System.Capture );
			sw.WriteLine();
			sw.WriteLine();
			#endregion

			sw.Close();
		}
		public void tファイルから読み込み( string iniファイル名 )
		{
			this.ConfigIniファイル名 = iniファイル名;
			this.bConfigIniが存在している = File.Exists( this.ConfigIniファイル名 );
			if( this.bConfigIniが存在している )
			{
				string str;
				this.tキーアサインを全部クリアする();
				using ( StreamReader reader = new StreamReader( this.ConfigIniファイル名, Encoding.GetEncoding( "Shift_JIS" ) ) )
				{
					str = reader.ReadToEnd();
				}
				t文字列から読み込み( str );
				CDTXVersion version = new CDTXVersion( this.strDTXManiaのバージョン );
				//if( version.n整数部 <= 69 )
				//{
				//	this.tデフォルトのキーアサインに設定する();
				//}
			}
		}

		private void t文字列から読み込み( string strAllSettings )	// 2011.4.13 yyagi; refactored to make initial KeyConfig easier.
		{
			Eセクション種別 unknown = Eセクション種別.Unknown;
			string[] delimiter = { "\n" };
			string[] strSingleLine = strAllSettings.Split( delimiter, StringSplitOptions.RemoveEmptyEntries );
			foreach ( string s in strSingleLine )
			{
				string str = s.Replace( '\t', ' ' ).TrimStart( new char[] { '\t', ' ' } );
				if ( ( str.Length != 0 ) && ( str[ 0 ] != ';' ) )
				{
					try
					{
						string str3;
						string str4;
						if ( str[ 0 ] == '[' )
						{
							#region [ セクションの変更 ]
							//-----------------------------
							StringBuilder builder = new StringBuilder( 0x20 );
							int num = 1;
							while ( ( num < str.Length ) && ( str[ num ] != ']' ) )
							{
								builder.Append( str[ num++ ] );
							}
							string str2 = builder.ToString();
							if ( str2.Equals( "System" ) )
							{
								unknown = Eセクション種別.System;
							}
							else if ( str2.Equals( "Log" ) )
							{
								unknown = Eセクション種別.Log;
							}
							else if ( str2.Equals( "PlayOption" ) )
							{
								unknown = Eセクション種別.PlayOption;
							}
							else if ( str2.Equals( "ViewerOption" ) )
							{
								unknown = Eセクション種別.ViewerOption;
							}
							else if ( str2.Equals( "AutoPlay" ) )
							{
								unknown = Eセクション種別.AutoPlay;
							}
							else if ( str2.Equals( "HitRange" ) )
							{
								unknown = Eセクション種別.HitRange;
							}
							else if ( str2.Equals( "GUID" ) )
							{
								unknown = Eセクション種別.GUID;
							}
							else if ( str2.Equals( "DrumsKeyAssign" ) )
							{
								unknown = Eセクション種別.DrumsKeyAssign;
							}
							else if ( str2.Equals( "GuitarKeyAssign" ) )
							{
								unknown = Eセクション種別.GuitarKeyAssign;
							}
							else if ( str2.Equals( "BassKeyAssign" ) )
							{
								unknown = Eセクション種別.BassKeyAssign;
							}
							else if ( str2.Equals( "SystemKeyAssign" ) )
							{
								unknown = Eセクション種別.SystemKeyAssign;
							}
							else if( str2.Equals( "Temp" ) )
							{
								unknown = Eセクション種別.Temp;
							}
							else
							{
								unknown = Eセクション種別.Unknown;
							}
							//-----------------------------
							#endregion
						}
						else
						{
							string[] strArray = str.Split( new char[] { '=' } );
							if( strArray.Length == 2 )
							{
								str3 = strArray[ 0 ].Trim();
								str4 = strArray[ 1 ].Trim();
								switch( unknown )
								{
									#region [ [System] ]
									//-----------------------------
									case Eセクション種別.System:
										{
#if false		// #23625 2011.1.11 Config.iniからダメージ/回復値の定数変更を行う場合はここを有効にする 087リリースに合わせ機能無効化
										//----------------------------------------
												if (str3.Equals("GaugeFactorD"))
												{
													int p = 0;
													string[] splittedFactor = str4.Split(',');
													foreach (string s in splittedFactor) {
														this.fGaugeFactor[p++, 0] = Convert.ToSingle(s);
													}
												} else
												if (str3.Equals("GaugeFactorG"))
												{
													int p = 0;
													string[] splittedFactor = str4.Split(',');
													foreach (string s in splittedFactor)
													{
														this.fGaugeFactor[p++, 1] = Convert.ToSingle(s);
													}
												}
												else
												if (str3.Equals("DamageFactor"))
												{
													int p = 0;
													string[] splittedFactor = str4.Split(',');
													foreach (string s in splittedFactor)
													{
														this.fDamageLevelFactor[p++] = Convert.ToSingle(s);
													}
												}
												else
										//----------------------------------------
#endif
											#region [ Version ]
											if ( str3.Equals( "Version" ) )
											{
												this.strDTXManiaのバージョン = str4;
											}
											#endregion
											#region [ DTXPath ]
											else if( str3.Equals( "DTXPath" ) )
											{
												this.str曲データ検索パス = str4;
											}
											#endregion
											#region [ skin関係 ]
											else if ( str3.Equals( "SkinPath" ) )
											{
												string absSkinPath = str4;
												if ( !System.IO.Path.IsPathRooted( str4 ) )
												{
													absSkinPath = System.IO.Path.Combine( CDTXMania.strEXEのあるフォルダ, "System" );
													absSkinPath = System.IO.Path.Combine( absSkinPath, str4 );
													Uri u = new Uri( absSkinPath );
													absSkinPath = u.AbsolutePath.ToString();	// str4内に相対パスがある場合に備える
													absSkinPath = System.Web.HttpUtility.UrlDecode( absSkinPath );						// デコードする
													absSkinPath = absSkinPath.Replace( '/', System.IO.Path.DirectorySeparatorChar );	// 区切り文字が\ではなく/なので置換する
												}
												if ( absSkinPath[ absSkinPath.Length - 1 ] != System.IO.Path.DirectorySeparatorChar )	// フォルダ名末尾に\を必ずつけて、CSkin側と表記を統一する
												{
													absSkinPath += System.IO.Path.DirectorySeparatorChar;
												}
												this.strSystemSkinSubfolderFullName = absSkinPath;
											}
											else if ( str3.Equals( "SkinChangeByBoxDef" ) )
											{
												this.bUseBoxDefSkin = C変換.bONorOFF( str4[ 0 ] );
											}
											#endregion
											#region [ Window関係 ]
											else if ( str3.Equals( "FullScreen" ) )
											{
												this.b全画面モード = C変換.bONorOFF( str4[ 0 ] );
											}
											else if ( str3.Equals( "WindowX" ) )		// #30675 2013.02.04 ikanick add
											{
												this.n初期ウィンドウ開始位置X = C変換.n値を文字列から取得して範囲内に丸めて返す(
                                                    str4, 0,  System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - 1 , this.n初期ウィンドウ開始位置X );
											}
											else if ( str3.Equals( "WindowY" ) )		// #30675 2013.02.04 ikanick add
											{
												this.n初期ウィンドウ開始位置Y = C変換.n値を文字列から取得して範囲内に丸めて返す(
                                                    str4, 0,  System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - 1 , this.n初期ウィンドウ開始位置Y );
											}
											else if ( str3.Equals( "WindowWidth" ) )		// #23510 2010.10.31 yyagi add
											{
												this.nウインドウwidth = C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 1, 65535, this.nウインドウwidth );
												if( this.nウインドウwidth <= 0 )
												{
													this.nウインドウwidth = SampleFramework.GameWindowSize.Width;
												}
											}
											else if( str3.Equals( "WindowHeight" ) )		// #23510 2010.10.31 yyagi add
											{
												this.nウインドウheight = C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 1, 65535, this.nウインドウheight );
												if( this.nウインドウheight <= 0 )
												{
													this.nウインドウheight = SampleFramework.GameWindowSize.Height;
												}
											}
											else if ( str3.Equals( "DoubleClickFullScreen" ) )	// #26752 2011.11.27 yyagi
											{
												this.bIsAllowedDoubleClickFullscreen = C変換.bONorOFF( str4[ 0 ] );
											}
											else if ( str3.Equals( "EnableSystemMenu" ) )		// #28200 2012.5.1 yyagi
											{
												this.bIsEnabledSystemMenu = C変換.bONorOFF( str4[ 0 ] );
											}
											else if ( str3.Equals( "BackSleep" ) )				// #23568 2010.11.04 ikanick add
											{
												this.n非フォーカス時スリープms = C変換.n値を文字列から取得して範囲内にちゃんと丸めて返す( str4, 0, 50, this.n非フォーカス時スリープms );
											}
											#endregion
											#region [ WASAPI/ASIO関係 ]
											else if ( str3.Equals( "SoundDeviceType" ) )
											{
												this.nSoundDeviceType = C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 0, 2, this.nSoundDeviceType );
											}
											else if ( str3.Equals( "WASAPIBufferSizeMs" ) )
											{
											    this.nWASAPIBufferSizeMs = C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 0, 9999, this.nWASAPIBufferSizeMs );
											}
											else if ( str3.Equals( "ASIODevice" ) )
											{
												string[] asiodev = CEnumerateAllAsioDevices.GetAllASIODevices();
												this.nASIODevice = C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 0, asiodev.Length - 1, this.nASIODevice );
											}
											//else if ( str3.Equals( "ASIOBufferSizeMs" ) )
											//{
											//    this.nASIOBufferSizeMs = C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 0, 9999, this.nASIOBufferSizeMs );
											//}
											//else if ( str3.Equals( "DynamicBassMixerManagement" ) )
											//{
											//    this.bDynamicBassMixerManagement = C変換.bONorOFF( str4[ 0 ] );
											//}
											else if ( str3.Equals( "SoundTimerType" ) )			// #33689 2014.6.6 yyagi
											{
												this.bUseOSTimer = C変換.bONorOFF( str4[ 0 ] );
											}
											//else if ( str3.Equals( "MasterVolume" ) )
											//{
											//    this.nMasterVolume = C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 0, 100, this.nMasterVolume );
											//}
											#endregion
											else if ( str3.Equals( "VSyncWait" ) )
											{
												this.b垂直帰線待ちを行う = C変換.bONorOFF( str4[ 0 ] );
											}
											else if( str3.Equals( "SleepTimePerFrame" ) )		// #23568 2011.11.27 yyagi
											{
												this.nフレーム毎スリープms = C変換.n値を文字列から取得して範囲内にちゃんと丸めて返す( str4, -1, 50, this.nフレーム毎スリープms );
											}
											else if( str3.Equals( "BGAlpha" ) )
											{
												this.n背景の透過度 = C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 0, 0xff, this.n背景の透過度 );
											}
											else if( str3.Equals( "DamageLevel" ) )
											{
												this.eダメージレベル = (Eダメージレベル) C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 0, 2, (int) this.eダメージレベル );
											}
											else if ( str3.Equals( "StageFailed" ) )
											{
												this.bSTAGEFAILED有効 = C変換.bONorOFF( str4[ 0 ] );
											}
											#region [ AVI/BGA ]
											else if( str3.Equals( "AVI" ) )
											{
												this.bAVI有効 = C変換.bONorOFF( str4[ 0 ] );
											}
											else if( str3.Equals( "BGA" ) )
											{
												this.bBGA有効 = C変換.bONorOFF( str4[ 0 ] );
											}
											else if( str3.Equals( "ClipDispType" ) )
											{
												this.eClipDispType = (EClipDispType)C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 0, 3, (int) this.eClipDispType );
											}
											#endregion
											#region [ フィルイン関係 ]
											else if ( str3.Equals( "FillInEffect" ) )
											{
												this.bフィルイン有効 = C変換.bONorOFF( str4[ 0 ] );
											}
											else if ( str3.Equals( "AudienceSound" ) )
											{
												this.b歓声を発声する = C変換.bONorOFF( str4[ 0 ] );
											}
											#endregion
											#region [ プレビュー音 ]
											else if( str3.Equals( "PreviewSoundWait" ) )
											{
												this.n曲が選択されてからプレビュー音が鳴るまでのウェイトms = C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 0, 0x5f5e0ff, this.n曲が選択されてからプレビュー音が鳴るまでのウェイトms );
											}
											else if( str3.Equals( "PreviewImageWait" ) )
											{
												this.n曲が選択されてからプレビュー画像が表示開始されるまでのウェイトms = C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 0, 0x5f5e0ff, this.n曲が選択されてからプレビュー画像が表示開始されるまでのウェイトms );
											}
											#endregion
											//else if( str3.Equals( "AdjustWaves" ) )
											//{
											//	this.bWave再生位置自動調整機能有効 = C変換.bONorOFF( str4[ 0 ] );
											//}
											#region [ BGM/ドラムのヒット音 ]
											else if( str3.Equals( "BGMSound" ) )
											{
												this.bBGM音を発声する = C変換.bONorOFF( str4[ 0 ] );
											}
											#endregion
											else if( str3.Equals( "SaveScoreIni" ) )
											{
												this.bScoreIniを出力する = C変換.bONorOFF( str4[ 0 ] );
											}
											else if( str3.Equals( "RandomFromSubBox" ) )
											{
												this.bランダムセレクトで子BOXを検索対象とする = C変換.bONorOFF( str4[ 0 ] );
											}
											#region [ SoundMonitor ]
											else if( str3.Equals( "SoundMonitorDrums" ) )
											{
												this.b演奏音を強調する.Drums = C変換.bONorOFF( str4[ 0 ] );
											}
											else if( str3.Equals( "SoundMonitorGuitar" ) )
											{
												this.b演奏音を強調する.Guitar = C変換.bONorOFF( str4[ 0 ] );
											}
											else if( str3.Equals( "SoundMonitorBass" ) )
											{
												this.b演奏音を強調する.Bass = C変換.bONorOFF( str4[ 0 ] );
											}
											#endregion
											#region [ コンボ数 ]
											else if( str3.Equals( "MinComboDrums" ) )
											{
												this.n表示可能な最小コンボ数.Drums = C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 1, 0x1869f, this.n表示可能な最小コンボ数.Drums );
											}
											else if( str3.Equals( "MinComboGuitar" ) )
											{
												this.n表示可能な最小コンボ数.Guitar = C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 1, 0x1869f, this.n表示可能な最小コンボ数.Guitar );
											}
											else if( str3.Equals( "MinComboBass" ) )
											{
												this.n表示可能な最小コンボ数.Bass = C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 1, 0x1869f, this.n表示可能な最小コンボ数.Bass );
											}
											#endregion
											else if( str3.Equals( "ShowDebugStatus" ) )
											{
												this.b演奏情報を表示する = C変換.bONorOFF( str4[ 0 ] );
											}
											#region [ 選曲リストフォント ]
											else if( str3.Equals( "SelectListFontName" ) )
											{
												this.str選曲リストフォント = str4;
											}
											else if( str3.Equals( "SelectListFontSize" ) )
											{
												this.n選曲リストフォントのサイズdot = C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 1, 0x3e7, this.n選曲リストフォントのサイズdot );
											}
											else if( str3.Equals( "SelectListFontItalic" ) )
											{
												this.b選曲リストフォントを斜体にする = C変換.bONorOFF( str4[ 0 ] );
											}
											else if( str3.Equals( "SelectListFontBold" ) )
											{
												this.b選曲リストフォントを太字にする = C変換.bONorOFF( str4[ 0 ] );
											}
											else if( str3.Equals( "PrivateFontFontName" ) )
											{
												this.strPrivateFontで使うフォント名 = str4;
											}
											#endregion
											else if( str3.Equals( "ChipVolume" ) )
											{
												this.n手動再生音量 = C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 0, 100, this.n手動再生音量 );
											}
											else if( str3.Equals( "AutoChipVolume" ) )
											{
												this.n自動再生音量 = C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 0, 100, this.n自動再生音量 );
											}
											else if( str3.Equals( "StoicMode" ) )
											{
												this.bストイックモード = C変換.bONorOFF( str4[ 0 ] );
											}
											else if( str3.Equals( "ShowLagTime" ) )				// #25370 2011.6.3 yyagi
											{
												this.nShowLagType = C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 0, 2, this.nShowLagType );
											}
											else if ( str3.Equals( "JudgeDispPriority" ) )
											{
												this.e判定表示優先度 = (E判定表示優先度) C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 0, 1, (int) this.e判定表示優先度 );
											}
											else if ( str3.Equals( "AutoResultCapture" ) )			// #25399 2011.6.9 yyagi
											{
												this.bIsAutoResultCapture = C変換.bONorOFF( str4[ 0 ] );
											}
											else if ( str3.Equals( "TimeStretch" ) )				// #23664 2013.2.24 yyagi
											{
												this.bTimeStretch = C変換.bONorOFF( str4[ 0 ] );
											}
											#region [ AdjustTime ]
											else if( str3.Equals( "InputAdjustTimeDrums" ) )		// #23580 2011.1.3 yyagi
											{
												this.nInputAdjustTimeMs.Drums = C変換.n値を文字列から取得して範囲内に丸めて返す( str4, -99, 99, this.nInputAdjustTimeMs.Drums );
											}
											else if( str3.Equals( "InputAdjustTimeGuitar" ) )	// #23580 2011.1.3 yyagi
											{
												this.nInputAdjustTimeMs.Guitar = C変換.n値を文字列から取得して範囲内に丸めて返す( str4, -99, 99, this.nInputAdjustTimeMs.Guitar );
											}
											else if( str3.Equals( "InputAdjustTimeBass" ) )		// #23580 2011.1.3 yyagi
											{
												this.nInputAdjustTimeMs.Bass = C変換.n値を文字列から取得して範囲内に丸めて返す( str4, -99, 99, this.nInputAdjustTimeMs.Bass );
											}
											else if ( str3.Equals( "JudgeLinePosOffsetDrums" ) )		// #31602 2013.6.23 yyagi
											{
												this.nJudgeLinePosOffset.Drums = C変換.n値を文字列から取得して範囲内に丸めて返す( str4, -99, 99, this.nJudgeLinePosOffset.Drums );
											}
											else if ( str3.Equals( "JudgeLinePosOffsetGuitar" ) )		// #31602 2013.6.23 yyagi
											{
												this.nJudgeLinePosOffset.Guitar = C変換.n値を文字列から取得して範囲内に丸めて返す( str4, -99, 99, this.nJudgeLinePosOffset.Guitar );
											}
											else if ( str3.Equals( "JudgeLinePosOffsetBass" ) )			// #31602 2013.6.23 yyagi
											{
												this.nJudgeLinePosOffset.Bass = C変換.n値を文字列から取得して範囲内に丸めて返す( str4, -99, 99, this.nJudgeLinePosOffset.Bass );
											}
											else if ( str3.Equals( "JudgeLinePosModeGuitar" ) )	// #33891 2014.6.26 yyagi
											{
												this.e判定位置.Guitar = (E判定位置) C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 0, 2, (int) this.e判定位置.Guitar );
											}
											else if ( str3.Equals( "JudgeLinePosModeBass" ) )		// #33891 2014.6.26 yyagi
											{
												this.e判定位置.Bass = (E判定位置) C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 0, 2, (int) this.e判定位置.Bass );
											}
											#endregion
											else if( str3.Equals( "BufferedInput" ) )
											{
												this.bバッファ入力を行う = C変換.bONorOFF( str4[ 0 ] );
											}
											else if ( str3.Equals( "PolyphonicSounds" ) )		// #28228 2012.5.1 yyagi
											{
												this.nPoliphonicSounds = C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 1, 8, this.nPoliphonicSounds );
											}
											#region [ VelocityMin ]
											else if ( str3.Equals( "LCVelocityMin" ) )			// #23857 2010.12.12 yyagi
											{
												this.nVelocityMin.LC = C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 0, 127, this.nVelocityMin.LC );
											}
											else if( str3.Equals( "HHVelocityMin" ) )
											{
												this.nVelocityMin.HH = C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 0, 127, this.nVelocityMin.HH );
											}
											else if( str3.Equals( "SDVelocityMin" ) )			// #23857 2011.1.31 yyagi
											{
												this.nVelocityMin.SD = C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 0, 127, this.nVelocityMin.SD );
											}
											else if( str3.Equals( "BDVelocityMin" ) )			// #23857 2011.1.31 yyagi
											{
												this.nVelocityMin.BD = C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 0, 127, this.nVelocityMin.BD );
											}
											else if( str3.Equals( "HTVelocityMin" ) )			// #23857 2011.1.31 yyagi
											{
												this.nVelocityMin.HT = C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 0, 127, this.nVelocityMin.HT );
											}
											else if( str3.Equals( "LTVelocityMin" ) )			// #23857 2011.1.31 yyagi
											{
												this.nVelocityMin.LT = C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 0, 127, this.nVelocityMin.LT );
											}
											else if( str3.Equals( "FTVelocityMin" ) )			// #23857 2011.1.31 yyagi
											{
												this.nVelocityMin.FT = C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 0, 127, this.nVelocityMin.FT );
											}
											else if( str3.Equals( "CYVelocityMin" ) )			// #23857 2011.1.31 yyagi
											{
												this.nVelocityMin.CY = C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 0, 127, this.nVelocityMin.CY );
											}
											else if( str3.Equals( "RDVelocityMin" ) )			// #23857 2011.1.31 yyagi
											{
												this.nVelocityMin.RD = C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 0, 127, this.nVelocityMin.RD );
											}
											#endregion
											//else if ( str3.Equals( "NoMP3Streaming" ) )
											//{
											//    this.bNoMP3Streaming = C変換.bONorOFF( str4[ 0 ] );
                                            //}
                                            #region[ Ver.K追加 ]
											else if ( str3.Equals( "DirectShowMode" ) )		// #28228 2012.5.1 yyagi
											{
                                                this.bDirectShowMode = C変換.bONorOFF( str4[ 0 ] ); ;
											}
                                            #endregion
                                            else if( str3.Equals( "EndingAnime" ) )
                                            {
                                                this.bEndingAnime = C変換.bONorOFF( str4[ 0 ] );
                                            }

                                            continue;
										}
									//-----------------------------
									#endregion

									#region [ [Log] ]
									//-----------------------------
									case Eセクション種別.Log:
										{
											if( str3.Equals( "OutputLog" ) )
											{
												this.bログ出力 = C変換.bONorOFF( str4[ 0 ] );
											}
											else if( str3.Equals( "TraceCreatedDisposed" ) )
											{
												this.bLog作成解放ログ出力 = C変換.bONorOFF( str4[ 0 ] );
											}
											else if( str3.Equals( "TraceDTXDetails" ) )
											{
												this.bLogDTX詳細ログ出力 = C変換.bONorOFF( str4[ 0 ] );
											}
											else if( str3.Equals( "TraceSongSearch" ) )
											{
												this.bLog曲検索ログ出力 = C変換.bONorOFF( str4[ 0 ] );
											}
											continue;
										}
									//-----------------------------
									#endregion

									#region [ [PlayOption] ]
									//-----------------------------
									case Eセクション種別.PlayOption:
										{
											if( str3.Equals( "Dark" ) )
											{
												this.eDark = (Eダークモード) C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 0, 2, (int) this.eDark );
											}
                                            else if( str3.Equals( "ScrollMode" ) )
                                            {
                                                this.eScrollMode = ( EScrollMode )C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 0, 2, 0 );
                                            }
                                            /*
											else if( str3.Equals( "DrumsGraph" ) )  // #24074 2011.01.23 addikanick
											{
												this.bGraph.Drums = C変換.bONorOFF( str4[ 0 ] );
											}
                                            */
											#region [ Sudden ]
											else if( str3.Equals( "DrumsSudden" ) )
											{
												this.bSudden.Drums = C変換.bONorOFF( str4[ 0 ] );
											}
											else if( str3.Equals( "GuitarSudden" ) )
											{
												this.bSudden.Guitar = C変換.bONorOFF( str4[ 0 ] );
											}
											else if( str3.Equals( "BassSudden" ) )
											{
												this.bSudden.Bass = C変換.bONorOFF( str4[ 0 ] );
											}
											#endregion
											#region [ Hidden ]
											else if( str3.Equals( "DrumsHidden" ) )
											{
												this.bHidden.Drums = C変換.bONorOFF( str4[ 0 ] );
											}
											else if( str3.Equals( "GuitarHidden" ) )
											{
												this.bHidden.Guitar = C変換.bONorOFF( str4[ 0 ] );
											}
											else if( str3.Equals( "BassHidden" ) )
											{
												this.bHidden.Bass = C変換.bONorOFF( str4[ 0 ] );
											}
											#endregion
											#region [ Invisible ]
											else if ( str3.Equals( "DrumsInvisible" ) )
											{
												this.eInvisible.Drums = (EInvisible) C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 0, 2, (int) this.eInvisible.Drums );
											}
											else if ( str3.Equals( "GuitarInvisible" ) )
											{
												this.eInvisible.Guitar = (EInvisible) C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 0, 2, (int) this.eInvisible.Guitar ); 
											}
											else if ( str3.Equals( "BassInvisible" ) )
											{
												this.eInvisible.Bass = (EInvisible) C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 0, 2, (int) this.eInvisible.Bass );
											}
											//else if ( str3.Equals( "InvisibleDisplayTimeMs" ) )
											//{
											//    this.nDisplayTimesMs = C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 0, 9999999, (int) this.nDisplayTimesMs );
											//}
											//else if ( str3.Equals( "InvisibleFadeoutTimeMs" ) )
											//{
											//    this.nFadeoutTimeMs = C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 0, 9999999, (int) this.nFadeoutTimeMs );
											//}
											#endregion
											else if ( str3.Equals( "DrumsReverse" ) )
											{
												this.bReverse.Drums = C変換.bONorOFF( str4[ 0 ] );
											}
											else if( str3.Equals( "GuitarReverse" ) )
											{
												this.bReverse.Guitar = C変換.bONorOFF( str4[ 0 ] );
											}
											else if( str3.Equals( "BassReverse" ) )
											{
												this.bReverse.Bass = C変換.bONorOFF( str4[ 0 ] );
											}
											else if( str3.Equals( "GuitarRandom" ) )
											{
												this.eRandom.Guitar = (Eランダムモード) C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 0, 3, (int) this.eRandom.Guitar );
											}
											else if( str3.Equals( "BassRandom" ) )
											{
												this.eRandom.Bass = (Eランダムモード) C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 0, 3, (int) this.eRandom.Bass );
											}
											else if( str3.Equals( "GuitarLight" ) )
											{
												this.bLight.Guitar = C変換.bONorOFF( str4[ 0 ] );
											}
											else if( str3.Equals( "BassLight" ) )
											{
												this.bLight.Bass = C変換.bONorOFF( str4[ 0 ] );
											}
											else if( str3.Equals( "GuitarLeft" ) )
											{
												this.bLeft.Guitar = C変換.bONorOFF( str4[ 0 ] );
											}
											else if( str3.Equals( "BassLeft" ) )
											{
												this.bLeft.Bass = C変換.bONorOFF( str4[ 0 ] );
											}
											else if( str3.Equals( "DrumsScrollSpeed" ) )
											{
												this.n譜面スクロール速度.Drums = C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 0, 0x7cf, this.n譜面スクロール速度.Drums );
											}
											else if( str3.Equals( "GuitarScrollSpeed" ) )
											{
												this.n譜面スクロール速度.Guitar = C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 0, 0x7cf, this.n譜面スクロール速度.Guitar );
											}
											else if( str3.Equals( "BassScrollSpeed" ) )
											{
												this.n譜面スクロール速度.Bass = C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 0, 0x7cf, this.n譜面スクロール速度.Bass );
											}
											else if( str3.Equals( "PlaySpeed" ) )
											{
												this.n演奏速度 = C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 5, 40, this.n演奏速度 );
											}
											else if( str3.Equals( "ComboDisp" ) )
											{
												this.bドラムコンボ表示 = C変換.bONorOFF( str4[ 0 ] );
											}
											//else if ( str3.Equals( "JudgeDispPriorityDrums" ) )
											//{
											//    this.e判定表示優先度.Drums = (E判定表示優先度) C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 0, 1, (int) this.e判定表示優先度.Drums );
											//}
											//else if ( str3.Equals( "JudgeDispPriorityGuitar" ) )
											//{
											//    this.e判定表示優先度.Guitar = (E判定表示優先度) C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 0, 1, (int) this.e判定表示優先度.Guitar );
											//}
											//else if ( str3.Equals( "JudgeDispPriorityBass" ) )
											//{
											//    this.e判定表示優先度.Bass = (E判定表示優先度) C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 0, 1, (int) this.e判定表示優先度.Bass );
											//}
											else if ( str3.Equals( "Risky" ) )					// #23559 2011.6.23  yyagi
											{
												this.nRisky = C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 0, 10, this.nRisky );
											}
											else if ( str3.Equals( "TaikoTight" ) )
											{
												this.bTight = C変換.bONorOFF( str4[ 0 ] );
											}
                                            else if ( str3.Equals( "TaikoTight2P" ) )
											{
												this.bTight2P = C変換.bONorOFF( str4[ 0 ] );
											}
											else if ( str3.Equals( "BranchGuide" ) )
											{
												this.bBranchGuide = C変換.bONorOFF( str4[ 0 ] );
											}
                                            else if ( str3.Equals( "DefaultCourse" ) ) //2017.01.30 DD
                                            {
                                                this.nDefaultCourse = C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 0, 4, this.nDefaultCourse );
                                            }
											else if ( str3.Equals( "ScoreMode" ) )
											{
												this.nScoreMode = C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 0, 3, this.nScoreMode );
											}
											else if ( str3.Equals( "HispeedRandom" ) )
											{
												this.bHispeedRandom = C変換.bONorOFF( str4[ 0 ] );
											}
											else if ( str3.Equals( "BigNotesWaitTime" ) )
											{
												this.n両手判定の待ち時間 = C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 1, 100, this.n両手判定の待ち時間 );
											}
											else if ( str3.Equals( "BigNotesJudge" ) )
											{
												this.b大音符判定 = C変換.bONorOFF( str4[ 0 ] );
											}
											else if ( str3.Equals( "BranchAnime" ) )
											{
												this.nBranchAnime = C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 0, 1, this.nBranchAnime );
											}
											else if ( str3.Equals( "NoInfo" ) )
											{
												this.bNoInfo = C変換.bONorOFF( str4[ 0 ] );
											}
     									    else if ( str3.Equals( "Chara" ) )
											{
												this.bChara = C変換.bONorOFF( str4[ 0 ] );
											}
     									    else if ( str3.Equals( "CharaMotionCount" ) )
											{
												this.nCharaMotionCount = C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 1, 500, this.nCharaMotionCount );
											}
     									    else if ( str3.Equals( "CharaMotionCountClear" ) )
											{
												this.nCharaMotionCount_clear = C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 1, 500, this.nCharaMotionCount_clear );
											}
     									    else if ( str3.Equals( "CharaMotionCountGogo" ) )
											{
												this.nCharaMotionCount_gogo = C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 1, 500, this.nCharaMotionCount );
											}
     									    else if ( str3.Equals( "CharaMotionCountMax" ) )
											{
												this.nCharaMotionCount_max = C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 1, 500, this.nCharaMotionCount_max );
											}
     									    else if ( str3.Equals( "CharaMotionCountMaxGogo" ) )
											{
												this.nCharaMotionCount_maxgogo = C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 1, 500, this.nCharaMotionCount_maxgogo );
											}
                                            else if ( str3.Equals( "CharaMotionList" ) )
											{
												this.strCharaMotionList = str4;
											}
     									    else if ( str3.Equals( "CharaMotionListGogo" ) )
											{
												this.strCharaMotionList_gogo = str4;
											}
                                            else if ( str3.Equals( "CharaMotionListClear" ) )
											{
												this.strCharaMotionList_clear = str4;
											}
     									    else if ( str3.Equals( "CharaMotionListMax" ) )
											{
												this.strCharaMotionList_max = str4;
											}
     									    else if ( str3.Equals( "CharaMotionListMaxGogo" ) )
											{
												this.strCharaMotionList_maxgogo = str4;
											}
                                            else if ( str3.Equals( "DefaultSongSort" ) )
                                            {
                                                this.nDefaultSongSort = C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 0, 2, this.nDefaultSongSort );
                                            }
											else if( str3.Equals( "TaikoRandom" ) )
											{
												this.eRandom.Taiko = (Eランダムモード) C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 0, 4, (int) this.eRandom.Taiko );
											}
											else if( str3.Equals( "TaikoStealth" ) )
											{
												this.eSTEALTH = (Eステルスモード) C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 0, 3, (int) this.eSTEALTH );
											}
											else if( str3.Equals( "GameMode" ) )
											{
												this.eGameMode = (EGame) C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 0, 2, (int) this.eGameMode );
											}
											else if( str3.Equals( "JudgeCountDisplay" ) )
											{
												this.bJudgeCountDisplay = C変換.bONorOFF( str4[ 0 ] );
											}
											else if( str3.Equals( "Just" ) )
											{
												this.bJust = C変換.bONorOFF( str4[ 0 ] );
											}
                                            else if( str3.Equals( "PlayerCount" ) )
                                            {
                                                this.nPlayerCount = C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 1, 2, this.nPlayerCount );
                                            }
											continue;
										}
									//-----------------------------
									#endregion

									#region [ [ViewerOption] ]
									//-----------------------------
									case Eセクション種別.ViewerOption:
										{
											if ( str3.Equals( "ViewerDrumsScrollSpeed" ) )
											{
												this.nViewerScrollSpeed.Drums = C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 0, 1999, this.nViewerScrollSpeed.Drums );
											}
											else if ( str3.Equals( "ViewerGuitarScrollSpeed" ) )
											{
												this.nViewerScrollSpeed.Guitar = C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 0, 1999, this.nViewerScrollSpeed.Guitar );
											}
											else if ( str3.Equals( "ViewerBassScrollSpeed" ) )
											{
												this.nViewerScrollSpeed.Bass = C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 0, 1999, this.nViewerScrollSpeed.Bass );
											}
											else if ( str3.Equals( "ViewerVSyncWait" ) )
											{
												this.bViewerVSyncWait = C変換.bONorOFF( str4[ 0 ] );
											}
											else if ( str3.Equals( "ViewerShowDebugStatus" ) )
											{
												this.bViewerShowDebugStatus = C変換.bONorOFF( str4[ 0 ] );
											}
											else if ( str3.Equals( "ViewerTimeStretch" ) )
											{
												this.bViewerTimeStretch = C変換.bONorOFF( str4[ 0 ] );
											}
											else if ( str3.Equals( "ViewerGuitar" ) )
											{
												this.bViewerGuitar有効 = C変換.bONorOFF( str4[ 0 ] );
											}
											else if ( str3.Equals( "ViewerDrums" ) )
											{
												this.bViewerDrums有効 = C変換.bONorOFF( str4[ 0 ] );
											}
											continue;
										}
									//-----------------------------
									#endregion

									#region [ [AutoPlay] ]
									//-----------------------------
									case Eセクション種別.AutoPlay:
										if( str3.Equals( "LC" ) )
										{
											this.bAutoPlay.LC = C変換.bONorOFF( str4[ 0 ] );
										}
										if( str3.Equals( "HH" ) )
										{
										this.bAutoPlay.HH = C変換.bONorOFF( str4[ 0 ] );
										}
										else if( str3.Equals( "SD" ) )
										{
											this.bAutoPlay.SD = C変換.bONorOFF( str4[ 0 ] );
										}
										else if( str3.Equals( "BD" ) )
										{
											this.bAutoPlay.BD = C変換.bONorOFF( str4[ 0 ] );
										}
										else if( str3.Equals( "HT" ) )
										{
											this.bAutoPlay.HT = C変換.bONorOFF( str4[ 0 ] );
										}
										else if( str3.Equals( "LT" ) )
										{
											this.bAutoPlay.LT = C変換.bONorOFF( str4[ 0 ] );
										}
										else if( str3.Equals( "FT" ) )
										{
										    this.bAutoPlay.FT = C変換.bONorOFF( str4[ 0 ] );
										}
										else if( str3.Equals( "CY" ) )
										{
											this.bAutoPlay.CY = C変換.bONorOFF( str4[ 0 ] );
										}
										else if( str3.Equals( "LP" ) )
										{
										    this.bAutoPlay.LP = C変換.bONorOFF( str4[ 0 ] );
										}
										else if( str3.Equals( "LBD" ) )
										{
											this.bAutoPlay.LBD = C変換.bONorOFF( str4[ 0 ] );
										}
										//else if( str3.Equals( "Guitar" ) )
										//{
										//    this.bAutoPlay.Guitar = C変換.bONorOFF( str4[ 0 ] );
										//}
										else if ( str3.Equals( "GuitarR" ) )
										{
											this.bAutoPlay.GtR = C変換.bONorOFF( str4[ 0 ] );
										}
										else if ( str3.Equals( "GuitarG" ) )
										{
											this.bAutoPlay.GtG = C変換.bONorOFF( str4[ 0 ] );
										}
										else if ( str3.Equals( "GuitarB" ) )
										{
											this.bAutoPlay.GtB = C変換.bONorOFF( str4[ 0 ] );
										}
										else if ( str3.Equals( "GuitarPick" ) )
										{
											this.bAutoPlay.GtPick = C変換.bONorOFF( str4[ 0 ] );
										}
										else if ( str3.Equals( "GuitarWailing" ) )
										{
											this.bAutoPlay.GtW = C変換.bONorOFF( str4[ 0 ] );
										}
										//else if ( str3.Equals( "Bass" ) )
										//{
										//    this.bAutoPlay.Bass = C変換.bONorOFF( str4[ 0 ] );
										//}
										else if ( str3.Equals( "BassR" ) )
										{
											this.bAutoPlay.BsR = C変換.bONorOFF( str4[ 0 ] );
										}
										else if ( str3.Equals( "BassG" ) )
										{
											this.bAutoPlay.BsG = C変換.bONorOFF( str4[ 0 ] );
										}
										else if ( str3.Equals( "BassB" ) )
										{
											this.bAutoPlay.BsB = C変換.bONorOFF( str4[ 0 ] );
										}
										else if ( str3.Equals( "BassPick" ) )
										{
											this.bAutoPlay.BsPick = C変換.bONorOFF( str4[ 0 ] );
										}
										else if ( str3.Equals( "BassWailing" ) )
										{
											this.bAutoPlay.BsW = C変換.bONorOFF( str4[ 0 ] );
										}
										else if ( str3.Equals( "Taiko" ) )
										{
											this.b太鼓パートAutoPlay = C変換.bONorOFF( str4[ 0 ] );
										}
										else if ( str3.Equals( "Taiko2P" ) )
										{
											this.b太鼓パートAutoPlay2P = C変換.bONorOFF( str4[ 0 ] );
										}
										else if ( str3.Equals( "TaikoAutoRoll" ) )
										{
											this.bAuto先生の連打 = C変換.bONorOFF( str4[ 0 ] );
										}
										continue;
									//-----------------------------
									#endregion

									#region [ [HitRange] ]
									//-----------------------------
									case Eセクション種別.HitRange:
										if( str3.Equals( "Perfect" ) )
										{
											this.nヒット範囲ms.Perfect = C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 0, 0x3e7, this.nヒット範囲ms.Perfect );
											}
										else if( str3.Equals( "Great" ) )
										{
											this.nヒット範囲ms.Great = C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 0, 0x3e7, this.nヒット範囲ms.Great );
										}
										else if( str3.Equals( "Good" ) )
										{
											this.nヒット範囲ms.Good = C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 0, 0x3e7, this.nヒット範囲ms.Good );
										}
										else if( str3.Equals( "Poor" ) )
										{
											this.nヒット範囲ms.Poor = C変換.n値を文字列から取得して範囲内に丸めて返す( str4, 0, 0x3e7, this.nヒット範囲ms.Poor );
										}
										continue;
									//-----------------------------
									#endregion

									#region [ [GUID] ]
									//-----------------------------
									case Eセクション種別.GUID:
										if( str3.Equals( "JoystickID" ) )
										{
											this.tJoystickIDの取得( str4 );
										}
										continue;
									//-----------------------------
									#endregion

									#region [ [DrumsKeyAssign] ]
									//-----------------------------
									case Eセクション種別.DrumsKeyAssign:
										{
											if( str3.Equals( "HH" ) )
											{
												this.tキーの読み出しと設定( str4, this.KeyAssign.Drums.HH );
											}
											else if( str3.Equals( "SD" ) )
											{
												this.tキーの読み出しと設定( str4, this.KeyAssign.Drums.SD );
											}
											else if( str3.Equals( "BD" ) )
											{
												this.tキーの読み出しと設定( str4, this.KeyAssign.Drums.BD );
											}
											else if( str3.Equals( "HT" ) )
											{
												this.tキーの読み出しと設定( str4, this.KeyAssign.Drums.HT );
											}
											else if( str3.Equals( "LT" ) )
											{
												this.tキーの読み出しと設定( str4, this.KeyAssign.Drums.LT );
											}
											else if( str3.Equals( "FT" ) )
											{
												this.tキーの読み出しと設定( str4, this.KeyAssign.Drums.FT );
											}
											else if( str3.Equals( "CY" ) )
											{
												this.tキーの読み出しと設定( str4, this.KeyAssign.Drums.CY );
											}
											else if( str3.Equals( "HO" ) )
											{
												this.tキーの読み出しと設定( str4, this.KeyAssign.Drums.HHO );
											}
											else if( str3.Equals( "RD" ) )
											{
												this.tキーの読み出しと設定( str4, this.KeyAssign.Drums.RD );
											}
											else if( str3.Equals( "LC" ) )
											{
												this.tキーの読み出しと設定( str4, this.KeyAssign.Drums.LC );
											}
											else if( str3.Equals( "LP" ) )										// #27029 2012.1.4 from
											{																	//
												this.tキーの読み出しと設定( str4, this.KeyAssign.Drums.LP );	//
											}																	//
											else if( str3.Equals( "LBD" ) )										// #27029 2012.1.4 from
											{																	//
												this.tキーの読み出しと設定( str4, this.KeyAssign.Drums.LBD );	//
											}

											else if( str3.Equals( "LeftRed" ) )
											{
												this.tキーの読み出しと設定( str4, this.KeyAssign.Drums.LeftRed );
											}
											else if( str3.Equals( "RightRed" ) )
											{
												this.tキーの読み出しと設定( str4, this.KeyAssign.Drums.RightRed );
											}
											else if( str3.Equals( "LeftBlue" ) )										// #27029 2012.1.4 from
											{																	//
												this.tキーの読み出しと設定( str4, this.KeyAssign.Drums.LeftBlue );	//
											}																	//
											else if( str3.Equals( "RightBlue" ) )										// #27029 2012.1.4 from
											{																	//
												this.tキーの読み出しと設定( str4, this.KeyAssign.Drums.RightBlue );	//
											}

											else if( str3.Equals( "LeftRed2P" ) )
											{
												this.tキーの読み出しと設定( str4, this.KeyAssign.Drums.LeftRed2P );
											}
											else if( str3.Equals( "RightRed2P" ) )
											{
												this.tキーの読み出しと設定( str4, this.KeyAssign.Drums.RightRed2P );
											}
											else if( str3.Equals( "LeftBlue2P" ) )										// #27029 2012.1.4 from
											{																	//
												this.tキーの読み出しと設定( str4, this.KeyAssign.Drums.LeftBlue2P );	//
											}																	//
											else if( str3.Equals( "RightBlue2P" ) )										// #27029 2012.1.4 from
											{																	//
												this.tキーの読み出しと設定( str4, this.KeyAssign.Drums.RightBlue2P );	//
											}

											continue;
										}
									//-----------------------------
									#endregion

									#region [ [GuitarKeyAssign] ]
									//-----------------------------
									case Eセクション種別.GuitarKeyAssign:
										{
											if( str3.Equals( "R" ) )
											{
												this.tキーの読み出しと設定( str4, this.KeyAssign.Guitar.R );
											}
											else if( str3.Equals( "G" ) )
											{
												this.tキーの読み出しと設定( str4, this.KeyAssign.Guitar.G );
											}
											else if( str3.Equals( "B" ) )
											{
												this.tキーの読み出しと設定( str4, this.KeyAssign.Guitar.B );
											}
											else if( str3.Equals( "Pick" ) )
											{
												this.tキーの読み出しと設定( str4, this.KeyAssign.Guitar.Pick );
											}
											else if( str3.Equals( "Wail" ) )
											{
												this.tキーの読み出しと設定( str4, this.KeyAssign.Guitar.Wail );
											}
											else if( str3.Equals( "Decide" ) )
											{
												this.tキーの読み出しと設定( str4, this.KeyAssign.Guitar.Decide );
											}
											else if( str3.Equals( "Cancel" ) )
											{
												this.tキーの読み出しと設定( str4, this.KeyAssign.Guitar.Cancel );
											}
											continue;
										}
									//-----------------------------
									#endregion

									#region [ [BassKeyAssign] ]
									//-----------------------------
									case Eセクション種別.BassKeyAssign:
										if( str3.Equals( "R" ) )
										{
											this.tキーの読み出しと設定( str4, this.KeyAssign.Bass.R );
										}
										else if( str3.Equals( "G" ) )
										{
										this.tキーの読み出しと設定( str4, this.KeyAssign.Bass.G );
										}
										else if( str3.Equals( "B" ) )
										{
											this.tキーの読み出しと設定( str4, this.KeyAssign.Bass.B );
										}
										else if( str3.Equals( "Pick" ) )
										{
											this.tキーの読み出しと設定( str4, this.KeyAssign.Bass.Pick );
										}
										else if( str3.Equals( "Wail" ) )
										{
											this.tキーの読み出しと設定( str4, this.KeyAssign.Bass.Wail );
										}
										else if( str3.Equals( "Decide" ) )
										{
											this.tキーの読み出しと設定( str4, this.KeyAssign.Bass.Decide );
										}
										else if( str3.Equals( "Cancel" ) )
										{
											this.tキーの読み出しと設定( str4, this.KeyAssign.Bass.Cancel );
										}
										continue;
									//-----------------------------
									#endregion

									#region [ [SystemKeyAssign] ]
									//-----------------------------
									case Eセクション種別.SystemKeyAssign:
										if( str3.Equals( "Capture" ) )
										{
											this.tキーの読み出しと設定( str4, this.KeyAssign.System.Capture );
										}
										continue;
									//-----------------------------
									#endregion
								}
							}
						}
						continue;
					}
					catch ( Exception exception )
					{
						Trace.TraceError( exception.Message );
						continue;
					}
				}
			}
		}

		/// <summary>
		/// ギターとベースのキーアサイン入れ替え
		/// </summary>
		//public void SwapGuitarBassKeyAssign()		// #24063 2011.1.16 yyagi
		//{
		//    for ( int j = 0; j <= (int)EKeyConfigPad.Capture; j++ )
		//    {
		//        CKeyAssign.STKEYASSIGN t; //= new CConfigIni.CKeyAssign.STKEYASSIGN();
		//        for ( int k = 0; k < 16; k++ )
		//        {
		//            t = this.KeyAssign[ (int)EKeyConfigPart.GUITAR ][ j ][ k ];
		//            this.KeyAssign[ (int)EKeyConfigPart.GUITAR ][ j ][ k ] = this.KeyAssign[ (int)EKeyConfigPart.BASS ][ j ][ k ];
		//            this.KeyAssign[ (int)EKeyConfigPart.BASS ][ j ][ k ] = t;
		//        }
		//    }
		//    this.bIsSwappedGuitarBass = !bIsSwappedGuitarBass;
		//}


		// その他

		#region [ private ]
		//-----------------
		private enum Eセクション種別
		{
			Unknown,
			System,
			Log,
			PlayOption,
			ViewerOption,
			AutoPlay,
			HitRange,
			GUID,
			DrumsKeyAssign,
			GuitarKeyAssign,
			BassKeyAssign,
			SystemKeyAssign,
			Temp,
		}

		private bool _bDrums有効;
		private bool _bGuitar有効;
		private bool bConfigIniが存在している;
		private string ConfigIniファイル名;

		private void tJoystickIDの取得( string strキー記述 )
		{
			string[] strArray = strキー記述.Split( new char[] { ',' } );
			if( strArray.Length >= 2 )
			{
				int result = 0;
				if( ( int.TryParse( strArray[ 0 ], out result ) && ( result >= 0 ) ) && ( result <= 9 ) )
				{
					if( this.dicJoystick.ContainsKey( result ) )
					{
						this.dicJoystick.Remove( result );
					}
					this.dicJoystick.Add( result, strArray[ 1 ] );
				}
			}
		}
		private void tキーアサインを全部クリアする()
		{
			this.KeyAssign = new CKeyAssign();
			for( int i = 0; i <= (int)EKeyConfigPart.SYSTEM; i++ )
			{
				for( int j = 0; j <= (int)EKeyConfigPad.Capture; j++ )
				{
					this.KeyAssign[ i ][ j ] = new CKeyAssign.STKEYASSIGN[ 16 ];
					for( int k = 0; k < 16; k++ )
					{
						this.KeyAssign[ i ][ j ][ k ] = new CKeyAssign.STKEYASSIGN( E入力デバイス.不明, 0, 0 );
					}
				}
			}
		}
		private void tキーの書き出し( StreamWriter sw, CKeyAssign.STKEYASSIGN[] assign )
		{
			bool flag = true;
			for( int i = 0; i < 0x10; i++ )
			{
				if( assign[ i ].入力デバイス == E入力デバイス.不明 )
				{
					continue;
				}
				if( !flag )
				{
					sw.Write( ',' );
				}
				flag = false;
				switch( assign[ i ].入力デバイス )
				{
					case E入力デバイス.キーボード:
						sw.Write( 'K' );
						break;

					case E入力デバイス.MIDI入力:
						sw.Write( 'M' );
						break;

					case E入力デバイス.ジョイパッド:
						sw.Write( 'J' );
						break;

					case E入力デバイス.マウス:
						sw.Write( 'N' );
						break;
				}
				sw.Write( "{0}{1}", "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".Substring( assign[ i ].ID, 1 ), assign[ i ].コード );	// #24166 2011.1.15 yyagi: to support ID > 10, change 2nd character from Decimal to 36-numeral system. (e.g. J1023 -> JA23)
			}
		}
		private void tキーの読み出しと設定( string strキー記述, CKeyAssign.STKEYASSIGN[] assign )
		{
			string[] strArray = strキー記述.Split( new char[] { ',' } );
			for( int i = 0; ( i < strArray.Length ) && ( i < 0x10 ); i++ )
			{
				E入力デバイス e入力デバイス;
				int id;
				int code;
				string str = strArray[ i ].Trim().ToUpper();
				if ( str.Length >= 3 )
				{
					e入力デバイス = E入力デバイス.不明;
					switch ( str[ 0 ] )
					{
						case 'J':
							e入力デバイス = E入力デバイス.ジョイパッド;
							break;

						case 'K':
							e入力デバイス = E入力デバイス.キーボード;
							break;

						case 'L':
							continue;

						case 'M':
							e入力デバイス = E入力デバイス.MIDI入力;
							break;

						case 'N':
							e入力デバイス = E入力デバイス.マウス;
							break;
					}
				}
				else
				{
					continue;
				}
				id = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf( str[ 1 ] );	// #24166 2011.1.15 yyagi: to support ID > 10, change 2nd character from Decimal to 36-numeral system. (e.g. J1023 -> JA23)
				if( ( ( id >= 0 ) && int.TryParse( str.Substring( 2 ), out code ) ) && ( ( code >= 0 ) && ( code <= 0xff ) ) )
				{
					this.t指定した入力が既にアサイン済みである場合はそれを全削除する( e入力デバイス, id, code );
					assign[ i ].入力デバイス = e入力デバイス;
					assign[ i ].ID = id;
					assign[ i ].コード = code;
				}
			}
		}
		private void tデフォルトのキーアサインに設定する()
		{
			this.tキーアサインを全部クリアする();

			string strDefaultKeyAssign = @"
[DrumsKeyAssign]

HH=K035,M042,M093
SD=K033,M025,M026,M027,M028,M029,M031,M032,M034,M037,M038,M040,M0113
BD=K012,K0126,M033,M035,M036,M0112
HT=K031,M048,M050
LT=K011,M047
FT=K023,M041,M043,M045
CY=K022,M049,M052,M055,M057,M091
HO=K010,M046,M092
RD=K020,M051,M053,M059,M089
LC=K026
LP=M044
LBD=
LeftRed=K015
RightRed=K019
LeftBlue=K013
RightBlue=K020
LeftRed2P=
RightRed2P=
LeftBlue2P=
RightBlue2P=

[GuitarKeyAssign]

R=
G=
B=
Pick=
Wail=
Decide=
Cancel=

[BassKeyAssign]

R=
G=
B=
Pick=
Wail=
Decide=
Cancel=

[SystemKeyAssign]
Capture=K065
";
			t文字列から読み込み( strDefaultKeyAssign );
		}
		//-----------------
		#endregion
	}
}
