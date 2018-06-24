using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Drawing;
using System.Threading;
using SlimDX;
using FDK;

namespace DTXMania
{
	internal class CActConfigList : CActivity
	{
		// プロパティ

		public bool bIsKeyAssignSelected		// #24525 2011.3.15 yyagi
		{
			get
			{
				Eメニュー種別 e = this.eメニュー種別;
				if ( e == Eメニュー種別.KeyAssignBass || e == Eメニュー種別.KeyAssignDrums ||
					e == Eメニュー種別.KeyAssignGuitar || e == Eメニュー種別.KeyAssignSystem )
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}
		public bool bIsFocusingParameter		// #32059 2013.9.17 yyagi
		{
			get
			{
				return b要素値にフォーカス中;
			}
		}
		public bool b現在選択されている項目はReturnToMenuである
		{
			get
			{
				CItemBase currentItem = this.list項目リスト[ this.n現在の選択項目 ];
				if ( currentItem == this.iSystemReturnToMenu || currentItem == this.iDrumsReturnToMenu ||
					currentItem == this.iGuitarReturnToMenu || currentItem == this.iBassReturnToMenu )
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}
		public CItemBase ib現在の選択項目
		{
			get
			{
				return this.list項目リスト[ this.n現在の選択項目 ];
			}
		}
		public int n現在の選択項目;


		// メソッド
		#region [ t項目リストの設定_System() ]
		public void t項目リストの設定_System()
		{
			this.tConfigIniへ記録する();
			this.list項目リスト.Clear();

			// #27029 2012.1.5 from: 説明文は最大9行→13行に変更。

			this.iSystemReturnToMenu = new CItemBase( "<< もどる", CItemBase.Eパネル種別.その他,
				"左側のメニューに戻ります。",
				"Return to left menu." );
			this.list項目リスト.Add( this.iSystemReturnToMenu );

            //this.iCommonDark = new CItemList( "Dark", CItemBase.Eパネル種別.通常, (int) CDTXMania.ConfigIni.eDark,
            //    "HALF: 背景、レーン、ゲージが表示\nされなくなります。\nFULL: さらに小節線、拍線、判定ラ\nイン、パッドも表示されなくなります。",
            //    "OFF: all display parts are shown.\nHALF: wallpaper, lanes and gauge are\n disappeared.\nFULL: additionaly to HALF, bar/beat\n lines, hit bar, pads are disappeared.",
            //    new string[] { "OFF", "HALF", "FULL" } );
            //this.list項目リスト.Add( this.iCommonDark );

            this.iTaikoPlayerCount = new CItemInteger( "プレイ人数", 1, 2, CDTXMania.ConfigIni.nPlayerCount,
                "プレイ人数切り替え：\n2にすると演奏画面が2人プレイ専用のレイアウトになり、2P専用譜面を読み込むようになります。",
                "" );
            this.list項目リスト.Add( this.iTaikoPlayerCount );

			//this.iSystemRisky = new CItemInteger( "Risky", 0, 10, CDTXMania.ConfigIni.nRisky,
			//	"Riskyモードの設定:\n1以上の値にすると、その回数分の\nPoor/MissでFAILEDとなります。\n0にすると無効になり、\nDamageLevelに従ったゲージ増減と\nなります。\nStageFailedの設定と併用できます。",
			//	"Risky mode:\nSet over 1, in case you'd like to specify\n the number of Poor/Miss times to be\n FAILED.\nSet 0 to disable Risky mode." );
			//this.list項目リスト.Add( this.iSystemRisky );

			//this.iCommonPlaySpeed = new CItemInteger("再生速度", 5, 40, CDTXMania.ConfigIni.n演奏速度,
			//	"曲の演奏速度を、速くしたり遅くした\n" +
			//	"りすることができます。\n" +
			//	"（※一部のサウンドカードでは正しく\n" +
			//	"　再生できない可能性があります。）\n" +
			//	"\n" +
			//	"TimeStretchがONのときに、演奏\n" +
			//	"速度をx0.850以下にすると、チップの\n" +
			//	"ズレが大きくなります。",
			//	"It changes the song speed.\n" +
			//	"For example, you can play in half\n" +
			//	" speed by setting PlaySpeed = 0.500\n" +
			//	" for your practice.\n" +
			//	"\n" +
			//	"Note: It also changes the songs' pitch.\n" +
			//	"In case TimeStretch=ON, some sound\n" +
			//	"lag occurs slower than x0.900.");
			//this.list項目リスト.Add( this.iCommonPlaySpeed );

			this.iSystemFullscreen = new CItemToggle( "全画面表示", CDTXMania.ConfigIni.b全画面モード,
				"画面モード設定：\nON で全画面モード、OFF でウィンド\nウモードになります。",
				"Fullscreen mode or window mode." );
			this.list項目リスト.Add( this.iSystemFullscreen );

			this.iSystemVSyncWait = new CItemToggle( "垂直同期", CDTXMania.ConfigIni.b垂直帰線待ちを行う,
				"垂直帰線同期：\n画面の描画をディスプレイの垂直帰\n線中に行なう場合には ON を指定し\nます。ON にすると、ガタつきのない\n滑らかな画面描画が実現されます。",
				"Turn ON to wait VSync (Vertical\n Synchronizing signal) at every\n drawings. (so FPS becomes 60)\nIf you have enough CPU/GPU power,\n the scroll would become smooth." );
			this.list項目リスト.Add( this.iSystemVSyncWait );

			this.iSystemTimeStretch = new CItemToggle( "速度変更時ピッチを変えない", CDTXMania.ConfigIni.bTimeStretch,
				"演奏速度の変更方式:\n" + 
				"ONにすると、演奏速度の変更を、\n" +
				"周波数変更ではなく\n" +
				"タイムストレッチで行います。" +
				"\n" +
				"これをONにすると、サウンド処理に\n" +
				"より多くのCPU性能を使用します。\n" +
				"また、演奏速度をx0.850以下にすると、\n" +
				"チップのズレが大きくなります。",
				"How to change the playing speed:\n" +
				"Turn ON to use time stretch\n" +
				"to change the play speed." +
				"\n" +
				"If you set TimeStretch=ON, it usese\n" +
				"more CPU power. And some sound\n" +
				"lag occurs slower than x0.900.");
			this.list項目リスト.Add( this.iSystemTimeStretch );


			//this.iSystemStageFailed = new CItemToggle( "StageFailed", CDTXMania.ConfigIni.bSTAGEFAILED有効,
			//	"STAGE FAILED 有効：\nON にすると、ゲージがなくなった時\nに STAGE FAILED となり演奏が中断\nされます。OFF の場合は、ゲージが\nなくなっても最後まで演奏できます。",
			//	"Turn OFF if you don't want to encount\n GAME OVER." );
			//this.list項目リスト.Add( this.iSystemStageFailed );
			this.iSystemRandomFromSubBox = new CItemToggle( "ランダム選曲の子BOX対象", CDTXMania.ConfigIni.bランダムセレクトで子BOXを検索対象とする,
				"子BOXをRANDOMの対象とする：\nON にすると、RANDOM SELECT 時\nに子BOXも選択対象とします。",
				"Turn ON to use child BOX (subfolders)\n at RANDOM SELECT." );
			this.list項目リスト.Add( this.iSystemRandomFromSubBox );


	
			//this.iSystemAdjustWaves = new CItemToggle( "AdjustWaves", CDTXMania.ConfigIni.bWave再生位置自動調整機能有効,
			//    "サウンド再生位置自動補正：\n" +
			//	"ハードウェアやOSに起因するサウン\n" +
			//	"ドのずれを強制的に補正します。\n" +
			//	"BGM のように再生時間の長い音声\n" +
			//	"データが使用されている曲で効果が\n" +
			//	"あります。" +
			//	"\n" +
			//	"※ DirectSound使用時のみ有効です。",
			//    "Automatic wave playing position\n" +
			//	" adjustment feature. If you turn it ON,\n" +
			//	" it decrease the lag which comes from\n" +
			//	" the difference of hardware/OS.\n" +
			//	"Usually, you should turn it ON." +
			//	"\n"+
			//	"Note: This setting is effetive\n" +
			//	" only when DirectSound is used.");
			//this.list項目リスト.Add( this.iSystemAdjustWaves );

			this.iSystemAVI = new CItemToggle( "AVIの使用", CDTXMania.ConfigIni.bAVI有効,
				"AVIの使用：\n動画(AVI)を再生可能にする場合に\nON にします。AVI の再生には、それ\nなりのマシンパワーが必要とされます。",
				"To use AVI playback or not." );
			this.list項目リスト.Add( this.iSystemAVI );
			this.iSystemBGA = new CItemToggle( "BGAの使用", CDTXMania.ConfigIni.bBGA有効,
				"BGAの使用：\n画像(BGA)を表示可能にする場合に\nON にします。BGA の再生には、それ\nなりのマシンパワーが必要とされます。",
				"To draw BGA (back ground animations)\n or not." );
			this.list項目リスト.Add( this.iSystemBGA );
			//this.iSystemPreviewSoundWait = new CItemInteger( "PreSoundWait", 0, 0x2710, CDTXMania.ConfigIni.n曲が選択されてからプレビュー音が鳴るまでのウェイトms,
			//	"プレビュー音演奏までの時間：\n曲にカーソルが合わされてからプレ\nビュー音が鳴り始めるまでの時間を\n指定します。\n0 ～ 10000 [ms] が指定可能です。",
			//	"Delay time(ms) to start playing preview\n sound in SELECT MUSIC screen.\nYou can specify from 0ms to 10000ms." );
			//this.list項目リスト.Add( this.iSystemPreviewSoundWait );
			//this.iSystemPreviewImageWait = new CItemInteger( "PreImageWait", 0, 0x2710, CDTXMania.ConfigIni.n曲が選択されてからプレビュー画像が表示開始されるまでのウェイトms,
			//	"プレビュー画像表示までの時間：\n曲にカーソルが合わされてからプレ\nビュー画像が表示されるまでの時間\nを指定します。\n0 ～ 10000 [ms] が指定可能です。",
			//	"Delay time(ms) to show preview image\n in SELECT MUSIC screen.\nYou can specify from 0ms to 10000ms." );
			//this.list項目リスト.Add( this.iSystemPreviewImageWait );
			this.iSystemDebugInfo = new CItemToggle( "デバッグ情報の表示", CDTXMania.ConfigIni.b演奏情報を表示する,
				"演奏情報の表示：\n演奏中、BGA領域の下部に演奏情報\n（FPS、BPM、演奏時間など）を表示し\nます。\nまた、小節線の横に小節番号が表示\nされるようになります。",
				"To show song informations on playing\n BGA area. (FPS, BPM, total time etc)\nYou can ON/OFF the indications\n by pushing [Del] while playing drums,\n guitar or bass." );
			this.list項目リスト.Add( this.iSystemDebugInfo );
			this.iSystemBGAlpha = new CItemInteger( "背景画像の透明度", 0, 0xff, CDTXMania.ConfigIni.n背景の透過度,
				"背景画像の半透明割合：\n背景画像をDTXManiaのフレーム画像\nと合成する際の、背景画像の透明度\nを指定します。\n0 が完全透明で、255 が完全不透明\nとなります。",
				"The degree for transparing playing\n screen and wallpaper.\n\n0=completely transparent,\n255=no transparency" );
			this.list項目リスト.Add( this.iSystemBGAlpha );
			this.iSystemBGMSound = new CItemToggle( "BGMの再生", CDTXMania.ConfigIni.bBGM音を発声する,
				"BGMの再生：\nこれをOFFにすると、BGM を再生しな\nくなります。",
				"Turn OFF if you don't want to play\n BGM." );
			this.list項目リスト.Add( this.iSystemBGMSound );
            //this.iSystemAudienceSound = new CItemToggle( "Audience", CDTXMania.ConfigIni.b歓声を発声する,
            //    "歓声の再生：\nこれをOFFにすると、歓声を再生しな\nくなります。",
            //    "Turn ON if you want to be cheered\n at the end of fill-in zone or not." );
            //this.list項目リスト.Add( this.iSystemAudienceSound );
            //this.iSystemDamageLevel = new CItemList( "DamageLevel", CItemBase.Eパネル種別.通常, (int) CDTXMania.ConfigIni.eダメージレベル,
            //    "ゲージ減少割合：\nMiss ヒット時のゲージの減少度合い\nを指定します。\nRiskyが1以上の場合は無効となります",
            //    "Damage level at missing (and\n recovering level) at playing.\nThis setting is ignored when Risky >= 1.",
            //    new string[] { "Small", "Normal", "Large" } );
            //this.list項目リスト.Add( this.iSystemDamageLevel );
			this.iSystemSaveScore = new CItemToggle( "自己ベストのスコアを保存", CDTXMania.ConfigIni.bScoreIniを出力する,
				"演奏記録の保存：\nON で演奏記録を ～.score.ini ファイ\nルに保存します。\n",
				"To save high-scores/skills, turn it ON.\nTurn OFF in case your song data are\n in read-only media (CD-ROM etc).\nNote that the score files also contain\n 'BGM Adjust' parameter. So if you\n want to keep adjusting parameter,\n you need to set SaveScore=ON." );
			this.list項目リスト.Add( this.iSystemSaveScore );


            //this.iSystemChipVolume = new CItemInteger( "ChipVolume", 0, 100, CDTXMania.ConfigIni.n手動再生音量,
            //    "打音の音量：\n入力に反応して再生されるチップの音\n量を指定します。\n0 ～ 100 % の値が指定可能です。\n",
            //    "The volumes for chips you hit.\nYou can specify from 0 to 100%." );
            //this.list項目リスト.Add( this.iSystemChipVolume );
            //this.iSystemAutoChipVolume = new CItemInteger( "AutoVolume", 0, 100, CDTXMania.ConfigIni.n自動再生音量,
            //    "自動再生音の音量：\n自動的に再生されるチップの音量を指\n定します。\n0 ～ 100 % の値が指定可能です。\n",
            //    "The volumes for AUTO chips.\nYou can specify from 0 to 100%." );
            //this.list項目リスト.Add( this.iSystemAutoChipVolume );
            //this.iSystemStoicMode = new CItemToggle( "StoicMode", CDTXMania.ConfigIni.bストイックモード,
            //    "ストイック（禁欲）モード：\n以下をまとめて表示ON/OFFします。\n_プレビュー画像/動画\n_リザルト画像/動画\n_NowLoading画像\n_演奏画面の背景画像\n_BGA 画像 / AVI 動画\n_グラフ画像\n",
            //    "Turn ON to disable drawing\n * preview image / movie\n * result image / movie\n * nowloading image\n * wallpaper (in playing screen)\n * BGA / AVI (in playing screen)" );
            //this.list項目リスト.Add( this.iSystemStoicMode );
            //this.iSystemShowLag = new CItemList( "ShowLagTime", CItemBase.Eパネル種別.通常, CDTXMania.ConfigIni.nShowLagType,
            //    "ズレ時間表示：\nジャストタイミングからのズレ時間(ms)\nを表示します。\n  OFF: ズレ時間を表示しません。\n  ON: ズレ時間を表示します。\n  GREAT-: PERFECT以外の時のみ\n表示します。",
            //    "About displaying the lag from\n the \"just timing\".\n  OFF: Don't show it.\n  ON: Show it.\n  GREAT-: Show it except you've\n  gotten PERFECT.",
            //    new string[] { "OFF", "ON", "GREAT-" } );
            //this.list項目リスト.Add( this.iSystemShowLag );
			//this.iSystemAutoResultCapture = new CItemToggle( "自己ベストの画面自動保存", CDTXMania.ConfigIni.bIsAutoResultCapture,
			//	"リザルト画像自動保存機能：\nONにすると、ハイスコア/ハイスキル時に\n自動でリザルト画像を曲データと同じ\nフォルダに保存します。",
			//	"AutoSaveResult:\nTurn ON to save your result screen\n image automatically when you get\n hiscore/hiskill." );
			//this.list項目リスト.Add( this.iSystemAutoResultCapture );

            //this.iSystemJudgeDispPriority = new CItemList( "JudgePriority", CItemBase.Eパネル種別.通常, (int) CDTXMania.ConfigIni.e判定表示優先度,
            //    "判定文字列とコンボ表示の優先順位を\n" +
            //    "指定します。\n" +
            //    "\n" +
            //    " Under: チップの下に表示します。\n" +
            //    " Over:  チップの上に表示します。",
            //    "The display prioity between chips\n" +
            //    " and judge mark/combo.\n" +
            //    "\n" +
            //    " Under: Show them under the chips.\n" +
            //    " Over:  Show them over the chips.",
            //    new string[] { "Under", "Over" } );
            //this.list項目リスト.Add( this.iSystemJudgeDispPriority );	

			//this.iSystemBufferedInput = new CItemToggle( "BufferedInput", CDTXMania.ConfigIni.bバッファ入力を行う,
			//	"バッファ入力モード：\nON にすると、FPS を超える入力解像\n度を実現します。\nOFF にすると、入力解像度は FPS に\n等しくなります。",
			//	"To select joystick input method.\n\nON to use buffer input. No lost/lags.\nOFF to use realtime input. It may\n causes lost/lags for input.\n Moreover, input frequency is\n synchronized with FPS." );
			//this.list項目リスト.Add( this.iSystemBufferedInput );
			this.iLogOutputLog = new CItemToggle( "Log出力", CDTXMania.ConfigIni.bログ出力,
				"Traceログ出力：\nDTXManiaLog.txt にログを出力します。\n変更した場合は、DTXMania の再起動\n後に有効となります。",
				"Turn ON to put debug log to\n DTXManiaLog.txt\nTo take it effective, you need to\n re-open DTXMania." );
			this.list項目リスト.Add( this.iLogOutputLog );

			// #24820 2013.1.3 yyagi
			this.iSystemSoundType = new CItemList("サウンドの出力方式", CItemList.Eパネル種別.通常, CDTXMania.ConfigIni.nSoundDeviceType,
				"サウンドの出力方式:\n" +
				"WASAPI, ASIO, DSound(DirectSound)\n" +
				"の中からサウンド出力方式を選択\n" +
				"します。\n" +
				"WASAPIはVista以降でのみ使用可能\n" +
				"です。ASIOは対応機器でのみ使用\n" +
				"可能です。\n" +
				"WASAPIかASIOを指定することで、\n" +
				"遅延の少ない演奏を楽しむことが\n" +
				"できます。\n" +
				"\n" +
				"※ 設定はCONFIGURATION画面の\n" +
				"　終了時に有効になります。",
				"Sound output type:\n" +
				"You can choose WASAPI, ASIO or\n" +
				"DShow(DirectShow).\n" +
				"WASAPI can use only after Vista.\n" +
				"ASIO can use on the\n" +
				"\"ASIO-supported\" sound device.\n" +
				"You should use WASAPI or ASIO\n" +
				"to decrease the sound lag.\n" +
				"\n" +
				"Note: Exit CONFIGURATION to make\n" +
				"     the setting take effect.",
				new string[] { "DSound", "ASIO", "WASAPI" });
			this.list項目リスト.Add(this.iSystemSoundType);

			// #24820 2013.1.15 yyagi
			this.iSystemWASAPIBufferSizeMs = new CItemInteger("WASAPIのバッファサイズ", 0, 99999, CDTXMania.ConfigIni.nWASAPIBufferSizeMs,
			    "WASAPI使用時のバッファサイズ:\n" +
			    "0～99999ms を指定可能です。\n" +
			    "0を指定すると、OSがバッファの\n" +
			    "サイズを自動設定します。\n" +
			    "値を小さくするほど発音ラグが\n" +
			    "減少しますが、音割れや異常動作を\n" +
			    "引き起こす場合があります。\n" +
			    "※ 設定はCONFIGURATION画面の\n" +
			    "　終了時に有効になります。",
			    "Sound buffer size for WASAPI:\n" +
			    "You can set from 0 to 99999ms.\n" +
			    "Set 0 to use a default sysytem\n" +
			    "buffer size.\n" +
			    "Smaller value makes smaller lag,\n" +
			    "but it may cause sound troubles.\n" +
			    "\n" +
			    "Note: Exit CONFIGURATION to make\n" +
			    "     the setting take effect." );
			this.list項目リスト.Add( this.iSystemWASAPIBufferSizeMs );

			// #24820 2013.1.17 yyagi
			string[] asiodevs = CEnumerateAllAsioDevices.GetAllASIODevices();
			this.iSystemASIODevice = new CItemList( "ASIOデバイスの選択", CItemList.Eパネル種別.通常, CDTXMania.ConfigIni.nASIODevice,
				"ASIOデバイス:\n" +
				"ASIO使用時のサウンドデバイスを\n" +
				"選択します。\n" +
				"\n" +
				"※ 設定はCONFIGURATION画面の\n" +
				"　終了時に有効になります。",
				"ASIO Sound Device:\n" +
				"Select the sound device to use\n" +
				"under ASIO mode.\n" +
				"\n" +
				"Note: Exit CONFIGURATION to make\n" +
				"     the setting take effect.",
				asiodevs );
			this.list項目リスト.Add( this.iSystemASIODevice );

			// #24820 2013.1.3 yyagi
			//this.iSystemASIOBufferSizeMs = new CItemInteger("ASIOBuffSize", 0, 99999, CDTXMania.ConfigIni.nASIOBufferSizeMs,
			//    "ASIO使用時のバッファサイズ:\n" +
			//    "0～99999ms を指定可能です。\n" +
			//    "推奨値は0で、サウンドデバイスでの\n" +
			//    "設定値をそのまま使用します。\n" +
			//    "(サウンドデバイスのASIO設定は、\n" +
			//    " ASIO capsなどで行います)\n" +
			//    "値を小さくするほど発音ラグが\n" +
			//    "減少しますが、音割れや異常動作を\n" +
			//    "引き起こす場合があります。\n" +
			//    "\n" +
			//    "※ 設定はCONFIGURATION画面の\n" +
			//    "　終了時に有効になります。",
			//    "Sound buffer size for ASIO:\n" +
			//    "You can set from 0 to 99999ms.\n" +
			//    "You should set it to 0, to use\n" +
			//    "a default value specified to\n" +
			//    "the sound device.\n" +
			//    "Smaller value makes smaller lag,\n" +
			//    "but it may cause sound troubles.\n" +
			//    "\n" +
			//    "Note: Exit CONFIGURATION to make\n" +
			//    "     the setting take effect." );
			//this.list項目リスト.Add( this.iSystemASIOBufferSizeMs );

			// #33689 2014.6.17 yyagi
			this.iSystemSoundTimerType = new CItemToggle( "OSタイマーを使用する", CDTXMania.ConfigIni.bUseOSTimer,
				"OSタイマーを使用するかどうか:\n" +
				"演奏タイマーとして、DTXMania独自の\n" +
				"タイマーを使うか、OS標準のタイマー\n" +
				"を使うかを選択します。\n" +
				"OS標準タイマーを使うとスクロールが\n" +
				"滑らかになりますが、演奏で音ズレが\n" +
				"発生することがあります。(そのため\n" +
				"AdjustWavesの効果が適用されます。)\n" +
				"\n" +
				"この指定はWASAPI/ASIO使用時のみ有効\n" +
				"です。\n",
				"Use OS Timer or not:\n" +
				"If this settings is ON, DTXMania uses\n" +
				"OS Standard timer. It brings smooth\n" +
				"scroll, but may cause some sound lag.\n" +
				"(so AdjustWaves is also avilable)\n" +
				"\n" +
				"If OFF, DTXMania uses its original\n" +
				"timer and the effect is vice versa.\n" +
				"\n" +
				"This settings is avilable only when\n" +
				"you uses WASAPI/ASIO.\n"
			);
			this.list項目リスト.Add( this.iSystemSoundTimerType );

            //this.iTaikoAutoSection = new CItemToggle( "AutoSection", CDTXMania.ConfigIni.bAutoSection,
            //    "譜面分岐時に自動でSECTION処理をします。\n" +
            //    "tjaにSECTIONをつけるのが面倒な時に使用してください。",
            //    "\n" +
            //    "");
            //this.list項目リスト.Add( this.iTaikoAutoSection );

			//this.iSystemSkinSubfolder = new CItemList( "Skin (全体)", CItemBase.Eパネル種別.通常, nSkinIndex,
			//	"スキン切替：\n" +
			//	"スキンを切り替えます。\n",
			//	//"CONFIGURATIONを抜けると、設定した\n" +
			//	//"スキンに変更されます。",
			//	"Skin:\n" +
			//	"Change skin.",
			//	skinNames );
			//this.list項目リスト.Add( this.iSystemSkinSubfolder );
			//this.iSystemUseBoxDefSkin = new CItemToggle( "Skin (Box)", CDTXMania.ConfigIni.bUseBoxDefSkin,
			//	"Music boxスキンの利用：\n" +
			//	"特別なスキンが設定されたMusic box\n" +
			//	"に出入りしたときに、自動でスキンを\n" +
			//	"切り替えるかどうかを設定します。\n",
				//"\n" +
				//"(Music Boxスキンは、box.defファイル\n" +
				//" で指定できます)\n",
			//	"Box skin:\n" +
			//	"Automatically change skin\n" +
			//	"specified in box.def file." );
			//this.list項目リスト.Add( this.iSystemUseBoxDefSkin );
	
			this.iSystemGoToKeyAssign = new CItemBase( "キー設定", CItemBase.Eパネル種別.通常,
			"システムのキー入力に関する項目を設\n定します。",
			"Settings for the system key/pad inputs." );
			this.list項目リスト.Add( this.iSystemGoToKeyAssign );


			this.iSystemReloadDTX = new CItemBase( "曲データ再読込み", CItemBase.Eパネル種別.通常,
				"曲データの一覧情報を取得し直します。",
				"Reload song data." );
			this.list項目リスト.Add( this.iSystemReloadDTX );


            OnListMenuの初期化();
			this.n現在の選択項目 = 0;
			this.eメニュー種別 = Eメニュー種別.System;
		}
		#endregion
		#region [ t項目リストの設定_Drums() ]
		public void t項目リストの設定_Drums()
		{
			this.tConfigIniへ記録する();
			this.list項目リスト.Clear();

			// #27029 2012.1.5 from: 説明文は最大9行→13行に変更。

			this.iDrumsReturnToMenu = new CItemBase( "<< もどる", CItemBase.Eパネル種別.その他,
				"左側のメニューに戻ります。",
				"Return to left menu." );
			this.list項目リスト.Add( this.iDrumsReturnToMenu );

			#region [ AutoPlay ]
            //this.iDrumsAutoPlayAll = new CItemThreeState( "AutoPlay (All)", CItemThreeState.E状態.不定,
            //    "全パッドの自動演奏の ON/OFF を\n" +
            //    "まとめて切り替えます。",
            //    "You can change whether Auto or not\n" +
            //    " for all drums lanes at once." );
            //this.list項目リスト.Add( this.iDrumsAutoPlayAll );

            //this.iDrumsLeftCymbal = new CItemToggle( "    LeftCymbal", CDTXMania.ConfigIni.bAutoPlay.LC,
            //    "左シンバルを自動で演奏します。",
            //    "To play LeftCymbal automatically." );
            //this.list項目リスト.Add( this.iDrumsLeftCymbal );

            //this.iDrumsHiHat = new CItemToggle( "    HiHat", CDTXMania.ConfigIni.bAutoPlay.HH,
            //    "ハイハットを自動で演奏します。\n" +
            //    "（クローズ、オープンとも）",
            //    "To play HiHat automatically.\n" +
            //    "(It effects to both HH-close and\n HH-open)" );
            //this.list項目リスト.Add( this.iDrumsHiHat );

            //this.iDrumsSnare = new CItemToggle( "    Snare", CDTXMania.ConfigIni.bAutoPlay.SD,
            //    "スネアを自動で演奏します。",
            //    "To play Snare automatically." );
            //this.list項目リスト.Add( this.iDrumsSnare );

            //this.iDrumsBass = new CItemToggle( "    BassDrum", CDTXMania.ConfigIni.bAutoPlay.BD,
            //    "バスドラムを自動で演奏します。",
            //    "To play Bass Drum automatically." );
            //this.list項目リスト.Add( this.iDrumsBass );

            //this.iDrumsHighTom = new CItemToggle( "    HighTom", CDTXMania.ConfigIni.bAutoPlay.HT,
            //    "ハイタムを自動で演奏します。",
            //    "To play High Tom automatically." );
            //this.list項目リスト.Add( this.iDrumsHighTom );

            //this.iDrumsLowTom = new CItemToggle( "    LowTom", CDTXMania.ConfigIni.bAutoPlay.LT,
            //    "ロータムを自動で演奏します。",
            //    "To play Low Tom automatically." );
            //this.list項目リスト.Add( this.iDrumsLowTom );

            //this.iDrumsFloorTom = new CItemToggle( "    FloorTom", CDTXMania.ConfigIni.bAutoPlay.FT,
            //    "フロアタムを自動で演奏します。",
            //    "To play Floor Tom automatically." );
            //this.list項目リスト.Add( this.iDrumsFloorTom );

            //this.iDrumsCymbalRide = new CItemToggle( "    Cym/Ride", CDTXMania.ConfigIni.bAutoPlay.CY,
            //    "右シンバルとライドシンバルを自動で\n" +
            //    "演奏します。",
            //    "To play both right- and Ride-Cymbal\n" +
            //    " automatically." );
            //this.list項目リスト.Add( this.iDrumsCymbalRide );

            //this.iDrumsLeftPedal = new CItemToggle( "    LeftPedal", CDTXMania.ConfigIni.bAutoPlay.LP,
            //    "左ペダルを自動で演奏します。",
            //    "To play Floor Tom automatically." );
            //this.list項目リスト.Add( this.iDrumsLeftPedal );

            //this.iDrumsLeftBassDrum = new CItemToggle( "    LeftBassDrum", CDTXMania.ConfigIni.bAutoPlay.LBD,
            //    "左バスドラムを自動で\n" +
            //    "演奏します。",
            //    "To play both LeftBassDrum\n" +
            //    " automatically." );
            //this.list項目リスト.Add( this.iDrumsLeftBassDrum );

			this.iTaikoAutoPlay = new CItemToggle( "オートプレイ 1P", CDTXMania.ConfigIni.b太鼓パートAutoPlay,
				"すべての音符を自動で演奏します。\n" +
				"",
				"To play both Taiko\n" +
				" automatically." );
			this.list項目リスト.Add( this.iTaikoAutoPlay );

			this.iTaikoAutoPlay2P = new CItemToggle("オートプレイ 2P", CDTXMania.ConfigIni.b太鼓パートAutoPlay,
				"すべての音符を自動で演奏します。\n" +
				"",
				"To play both Taiko\n" +
				" automatically." );
			this.list項目リスト.Add( this.iTaikoAutoPlay2P );

			this.iTaikoAutoRoll = new CItemToggle( "オートが黄色連打を叩く", CDTXMania.ConfigIni.bAuto先生の連打,
				"OFFにするとAUTO先生が黄色連打を\n" +
				"叩かなくなります。",
				"To play both Taiko\n" +
				" automatically." );
			this.list項目リスト.Add( this.iTaikoAutoRoll );
			#endregion

			//this.iDrumsScrollSpeed = new CItemInteger( "ScrollSpeed", 0, 0x7cf, CDTXMania.ConfigIni.n譜面スクロール速度.Drums,
			//	"演奏時のドラム譜面のスクロールの\n" +
			//	"速度を指定します。\n" +
			//	"x0.5 ～ x1000.0 を指定可能です。",
			//	"To change the scroll speed for the\n" +
			//	"drums lanes.\n" +
			//	"You can set it from x0.5 to x1000.0.\n" +
			//	"(ScrollSpeed=x0.5 means half speed)" );
			//this.list項目リスト.Add( this.iDrumsScrollSpeed );

            //this.iDrumsSudHid = new CItemList( "Sud+Hid", CItemBase.Eパネル種別.通常, getDefaultSudHidValue(E楽器パート.DRUMS ),
            //    "ドラムチップの表示方式:\n" +
            //    "OFF:　　チップを常に表示します。\n" +
            //    "Sudden: チップが譜面の下の方から\n" +
            //    "　　　　表示されるようになります。\n" +
            //    "Hidden: チップが譜面の下の方で表示\n" +
            //    "　　　　されなくなります。\n" +
            //    "Sud+Hid: SuddenとHiddenの効果を\n" +
            //    "　　　　同時にかけます。\n" +
            //    "S(emi)-Invisible:\n" +
            //    "　　　　通常はチップを透明にしますが、\n" +
            //    "　　　　Poor/Miss時しばらく表示します。\n" +
            //    "F(ull)-Invisible:\n" +
            //    "　　　　チップを常に透明にします。\n" +
            //    "　　　　暗譜での練習にお使いください。",

            //    "Drums chips display type:\n" +
            //    "OFF:    Always show chips.\n" +
            //    "Sudden: The chips are disappered\n" +
            //    "        until they come near the hit\n" +
            //    "        bar, and suddenly appears.\n" +
            //    "Hidden: The chips are hidden by\n" +
            //    "        approaching to the hit bar.\n" +
            //    "Sud+Hid: Both Sudden and Hidden.\n" +
            //    "S(emi)-Invisible:\n" +
            //    "        Usually you can't see the chips\n" +
            //    "        except you've gotten Poor/Miss.\n" +
            //    "F(ull)-Invisible:\n" +
            //    "        You can't see the chips at all.",
            //    new string[] { "OFF", "Sudden", "Hidden", "Sud+Hid", "S-Invisible", "F-Invisible" } );
            //this.list項目リスト.Add( this.iDrumsSudHid );

			//this.iDrumsSudden = new CItemToggle( "Sudden", CDTXMania.ConfigIni.bSudden.Drums,
			//    "ドラムチップが譜面の下の方から表\n" +
			//    "示されるようになります。",
			//    "Drums chips are disappered until they\n" +
			//    "come near the hit bar, and suddenly\n" +
			//    "appears." );
			//this.list項目リスト.Add( this.iDrumsSudden );

			//this.iDrumsHidden = new CItemToggle( "Hidden", CDTXMania.ConfigIni.bHidden.Drums,
			//    "ドラムチップが譜面の下の方で表示\n" +
			//    "されなくなります。",
			//    "Drums chips are hidden by approaching\n" +
			//    "to the hit bar. " );
			//this.list項目リスト.Add( this.iDrumsHidden );

			//this.iDrumsInvisible = new CItemList( "Invisible", CItemBase.Eパネル種別.通常, (int) CDTXMania.ConfigIni.eInvisible.Drums,
			//    "ドラムチップの透明化\n" +
			//    "OFF: チップを常に表示します。\n" +
			//    "SEMI: 通常はチップを透明にしますが、\n" +
			//    "　　　Poor/Miss時はしばらく表示します。\n" +
			//    "FULL: チップを常に透明にします。\n" +
			//    "　　　暗譜での練習にお使いください。\n" +
			//    "これをONにすると、SuddenとHiddenの\n" +
			//    "効果は無効になります。",
			//    "Invisible drums chip:\n" +
			//    "If you set Blindfold=ON, you can't\n" +
			//    "see the chips at all.",
			//    new string[] { "OFF", "SEMI", "FULL" } );
			//this.list項目リスト.Add( this.iDrumsInvisible );

            //this.iCommonDark = new CItemList( "Dark", CItemBase.Eパネル種別.通常, (int) CDTXMania.ConfigIni.eDark,
            //    "HALF: 背景、レーン、ゲージが表示\n" +
            //    "されなくなります。\n" +
            //    "FULL: さらに小節線、拍線、判定ラ\n" +
            //    "イン、パッドも表示されなくなります。",
            //    "OFF: all display parts are shown.\n" +
            //    "HALF: wallpaper, lanes and gauge are\n" +
            //    " disappeared.\n" +
            //    "FULL: additionaly to HALF, bar/beat\n" +
            //    " lines, hit bar, pads are disappeared.",
            //    new string[] { "OFF", "HALF", "FULL" } );
            //this.list項目リスト.Add( this.iCommonDark );


            //this.iDrumsReverse = new CItemToggle( "Reverse", CDTXMania.ConfigIni.bReverse.Drums,
            //    "ドラムチップが譜面の下から上に流\n" +
            //    "れるようになります。",
            //    "The scroll way is reversed. Drums chips\n"
            //    + "flow from the bottom to the top." );
            //this.list項目リスト.Add( this.iDrumsReverse );

			this.iSystemRisky = new CItemInteger( "指定数ミスで演奏終了", 0, 10, CDTXMania.ConfigIni.nRisky,
				"Riskyモードの設定:\n" +
				"1以上の値にすると、その回数分の\n" +
				"不可で演奏が強制終了します。\n" +
				"0にすると無効になり、\n" +
				"ノルマゲージのみになります。\n" +
				"\n" +
				"",
				"Risky mode:\n" +
				"Set over 1, in case you'd like to specify\n" +
				" the number of Poor/Miss times to be\n" +
				" FAILED.\n" +
				"Set 0 to disable Risky mode." );
			this.list項目リスト.Add( this.iSystemRisky );

			//this.iTaikoRandom = new CItemList( "Random", CItemBase.Eパネル種別.通常, (int) CDTXMania.ConfigIni.eRandom.Taiko,
			//	"いわゆるランダム。\n  RANDOM: ちょっと変わる\n  MIRROR: あべこべ \n  SUPER: そこそこヤバい\n  HYPER: 結構ヤバい\nなお、実装は適当な模様",
			//	"Notes come randomly.\n\n Part: swapping lanes randomly for each\n  measures.\n Super: swapping chip randomly\n Hyper: swapping randomly\n  (number of lanes also changes)",
			//	new string[] { "OFF", "RANDOM", "MIRROR", "SUPER", "HYPER" } );
			//this.list項目リスト.Add( this.iTaikoRandom );

			//this.iTaikoStealth = new CItemList( "Stealth", CItemBase.Eパネル種別.通常, (int) CDTXMania.ConfigIni.eSTEALTH,
			//	"DORON:ドロン\n"+
            //  "STEALTH:ステルス",
			//	"DORON:Hidden for NoteImage.\n"+
            //  "STEALTH:Hidden for NoteImage and SeNotes",
			//	new string[] { "OFF", "DORON", "STEALTH" } );
			//this.list項目リスト.Add( this.iTaikoStealth );

			//this.iTaikoNoInfo = new CItemToggle( "NoInfo", CDTXMania.ConfigIni.bNoInfo,
			//	"有効にすると曲情報などが見えなくなります。\n" +
			//	"",
			//	"It becomes MISS to hit pad without\n" +
			//	" chip." );
			//this.list項目リスト.Add( this.iTaikoNoInfo );

			this.iTaikoJust = new CItemToggle( "ぴったりモード", CDTXMania.ConfigIni.bJust,
				"有効にすると「良」以外の判定が全て不可になります。\n" +
				"",
				"有効にすると「良」以外の判定が全て不可になります。" );
			this.list項目リスト.Add( this.iTaikoJust );

			//this.iDrumsTight = new CItemToggle( "何もないところで叩くとミス", CDTXMania.ConfigIni.bTight,
			//	"ドラムチップのないところでパッドを\n" +
			//	"叩くとミスになります。",
			//	"It becomes MISS to hit pad without\n" +
			//	" chip." );
			//this.list項目リスト.Add( this.iDrumsTight );
            
			//this.iSystemMinComboDrums = new CItemInteger( "最小表示コンボ数", 1, 0x1869f, CDTXMania.ConfigIni.n表示可能な最小コンボ数.Drums,
			//	"表示可能な最小コンボ数（ドラム）：\n" +
			//	"画面に表示されるコンボの最小の数\n" +
			//	"を指定します。\n" +
			//	"1 ～ 99999 の値が指定可能です。",
			//	"Initial number to show the combo\n" +
			//	" for the drums.\n" +
			//	"You can specify from 1 to 99999." );
			//this.list項目リスト.Add( this.iSystemMinComboDrums );


			// #23580 2011.1.3 yyagi
			//this.iDrumsInputAdjustTimeMs = new CItemInteger( "InputAdjust", -99, 99, CDTXMania.ConfigIni.nInputAdjustTimeMs.Drums,
			//	"ドラムの入力タイミングの微調整を\n" +
			//	"行います。\n" +
			//	"-99 ～ 99ms まで指定可能です。\n" +
			//	"入力ラグを軽減するためには、負の\n" +
			//	"値を指定してください。\n",
			//	"To adjust the drums input timing.\n" +
			//	"You can set from -99 to 99ms.\n" +
			//	"To decrease input lag, set minus value." );
			//this.list項目リスト.Add( this.iDrumsInputAdjustTimeMs );

			// #24074 2011.01.23 add ikanick
            //this.iDrumsGraph = new CItemToggle( "Graph", CDTXMania.ConfigIni.bGraph.Drums,
            //    "最高スキルと比較できるグラフを\n" +
            //    "表示します。\n" +
            //    "オートプレイだと表示されません。",
            //    "To draw Graph \n" +
            //    " or not." );
            //this.list項目リスト.Add( this.iDrumsGraph );

            //this.iTaikoHispeedRandom = new CItemToggle("HSRandom", CDTXMania.ConfigIni.bHispeedRandom,
            //    "1ノーツごとのスクロール速度をランダムにします。\n" +
            //    "ドンカマ2000の練習にどうぞ。",
            //    "\n" +
            //    "");
            //this.list項目リスト.Add(this.iTaikoHispeedRandom);

            this.iTaikoDefaultCourse = new CItemList( "標準選択難易度", CItemBase.Eパネル種別.通常, CDTXMania.ConfigIni.nDefaultCourse,
                "デフォルトで選択される難易度\n" +
                " \n" +
                " ",
                new string[] { "かんたん", "ふつう", "むずかしい", "おに", "おに(裏譜面)" });
            this.list項目リスト.Add(this.iTaikoDefaultCourse);

            this.iTaikoScoreMode = new CItemList("スコアの計算方法", CItemBase.Eパネル種別.通常, CDTXMania.ConfigIni.nScoreMode,
                "スコア計算方法\n" +
                "TYPE-A: 太鼓1～太鼓7\n" +
                "TYPE-B: 太鼓8～太鼓14\n" +
                "TYPE-C: 現行の計算方式です。\n" +
                "TYPE-D: 真打ちモード \n",
                " \n" +
                " \n" +
                " ",
                new string[] { "旧筐体1～7", "旧筐体8～14", "新筐体", "真打" });
            this.list項目リスト.Add(this.iTaikoScoreMode);

            this.iTaikoBranchGuide = new CItemToggle("譜面分岐のデバッグ表示", CDTXMania.ConfigIni.bBranchGuide,
                "譜面分岐の参考になる数値などを表示します。\n" +
                "オートプレイだと表示されません。",
                "\n" +
                "");
            this.list項目リスト.Add(this.iTaikoBranchGuide);

            this.iTaikoBranchAnime = new CItemList("譜面分岐アニメーション", CItemBase.Eパネル種別.通常, CDTXMania.ConfigIni.nBranchAnime,
                "譜面分岐時のアニメーション\n" +
                "TYPE-A: 太鼓7～太鼓14\n" +
                "TYPE-B: 太鼓15～\n" +
                " \n",
                " \n" +
                " \n" +
                " ",
                new string[] { "旧筐体7～14", "新筐体" });
            this.list項目リスト.Add(this.iTaikoBranchAnime);

            this.iTaikoChara = new CItemToggle("キャラクターの表示", CDTXMania.ConfigIni.bChara,
                "キャラクター画像(β版)を表示します。\n" +
                "挙動が少し怪しいです。",
                "\n" +
                "");
            this.list項目リスト.Add(this.iTaikoChara);

            //this.iTaikoGameMode = new CItemList("GameMode", CItemBase.Eパネル種別.通常, (int)CDTXMania.ConfigIni.eGameMode,
            //    "ゲームモード\n" +
            //    "(1人プレイ専用)\n" +
            //    "TYPE-A: 完走!叩ききりまショー!\n" +
            //    "TYPE-B: 完走!叩ききりまショー!(激辛)\n" +
            //    " \n",
            //    " \n" +
            //    " \n" +
            //    " ",
            //    new string[] { "OFF", "TYPE-A", "TYPE-B" });
            //this.list項目リスト.Add( this.iTaikoGameMode );

            this.iTaikoBigNotesJudge = new CItemToggle( "大音符の両手判定", CDTXMania.ConfigIni.b大音符判定,
                "大音符の両手判定を有効にします。",
                "大音符の両手判定を有効にします。");
            this.list項目リスト.Add( this.iTaikoBigNotesJudge );

            this.iTaikoJudgeCountDisp = new CItemToggle( "判定数の表示", CDTXMania.ConfigIni.bJudgeCountDisplay,
                "左下に判定数を表示します。\n" +
                "(1人プレイ専用)",
                "Show the JudgeCount\n" +
                "(SinglePlay Only)");
            this.list項目リスト.Add( this.iTaikoJudgeCountDisp );
            
			this.iDrumsGoToKeyAssign = new CItemBase( "キー設定", CItemBase.Eパネル種別.通常,
				"ドラムのキー入力に関する項目を設\n"+
				"定します。",
				"Settings for the drums key/pad inputs." );
			this.list項目リスト.Add( this.iDrumsGoToKeyAssign );



            OnListMenuの初期化();
			this.n現在の選択項目 = 0;
			this.eメニュー種別 = Eメニュー種別.Drums;
		}
		#endregion
		#region [ t項目リストの設定_Guitar() ]
		public void t項目リストの設定_Guitar()
		{
			this.tConfigIniへ記録する();
			this.list項目リスト.Clear();

			// #27029 2012.1.5 from: 説明文は最大9行→13行に変更。

			this.iGuitarReturnToMenu = new CItemBase( "<< Return To Menu", CItemBase.Eパネル種別.その他,
				"左側のメニューに戻ります。",
				"Return to left menu." );
			this.list項目リスト.Add( this.iGuitarReturnToMenu );

            OnListMenuの初期化();
            this.n現在の選択項目 = 0;
			this.eメニュー種別 = Eメニュー種別.Guitar;
		}
		#endregion
		#region [ t項目リストの設定_Bass() ]
		public void t項目リストの設定_Bass()
		{
			this.tConfigIniへ記録する();
			this.list項目リスト.Clear();

			// #27029 2012.1.5 from: 説明文は最大9行→13行に変更。

			this.iBassReturnToMenu = new CItemBase( "<< Return To Menu", CItemBase.Eパネル種別.その他,
				"左側のメニューに戻ります。",
				"Return to left menu." );
			this.list項目リスト.Add( this.iBassReturnToMenu );

            OnListMenuの初期化();
			this.n現在の選択項目 = 0;
			this.eメニュー種別 = Eメニュー種別.Bass;
		}
		#endregion

		/// <summary>Sud+Hidの初期値を返す</summary>
		/// <param name="eInst"></param>
		/// <returns>
		/// 0: None
		/// 1: Sudden
		/// 2: Hidden
		/// 3: Sud+Hid
		/// 4: Semi-Invisible
		/// 5: Full-Invisible
		/// </returns>
		private int getDefaultSudHidValue( E楽器パート eInst )
		{
			int defvar;
			int nInst = (int) eInst;
			if ( CDTXMania.ConfigIni.eInvisible[ nInst ] != EInvisible.OFF )
			{
				defvar = (int) CDTXMania.ConfigIni.eInvisible[ nInst ] + 3;
			}
			else
			{
				defvar = ( CDTXMania.ConfigIni.bSudden[ nInst ] ? 1 : 0 ) +
						 ( CDTXMania.ConfigIni.bHidden[ nInst ] ? 2 : 0 );
			}
			return defvar;
		}

		/// <summary>
		/// ESC押下時の右メニュー描画
		/// </summary>
		public void tEsc押下()
		{
			if ( this.b要素値にフォーカス中 )		// #32059 2013.9.17 add yyagi
			{
				this.b要素値にフォーカス中 = false;
			}

			if ( this.eメニュー種別 == Eメニュー種別.KeyAssignSystem )
			{
				t項目リストの設定_System();
			}
			else if ( this.eメニュー種別 == Eメニュー種別.KeyAssignDrums )
			{
				t項目リストの設定_Drums();
			}
			else if ( this.eメニュー種別 == Eメニュー種別.KeyAssignGuitar )
			{
				t項目リストの設定_Guitar();
			}
			else if ( this.eメニュー種別 == Eメニュー種別.KeyAssignBass )
			{
				t項目リストの設定_Bass();
			}
			// これ以外なら何もしない
		}
		public void tEnter押下()
		{
			CDTXMania.Skin.sound決定音.t再生する();
			if( this.b要素値にフォーカス中 )
			{
				this.b要素値にフォーカス中 = false;
			}
			else if( this.list項目リスト[ this.n現在の選択項目 ].e種別 == CItemBase.E種別.整数 )
			{
				this.b要素値にフォーカス中 = true;
			}
			else if( this.b現在選択されている項目はReturnToMenuである )
			{
				//this.tConfigIniへ記録する();
				//CONFIG中にスキン変化が発生すると面倒なので、一旦マスクした。
			}
			#region [ 個々のキーアサイン ]
			else if( this.list項目リスト[ this.n現在の選択項目 ] == this.iKeyAssignDrumsLC )
			{
				CDTXMania.stageコンフィグ.tパッド選択通知( EKeyConfigPart.DRUMS, EKeyConfigPad.LC );
			}
			else if( this.list項目リスト[ this.n現在の選択項目 ] == this.iKeyAssignDrumsHHC )
			{
				CDTXMania.stageコンフィグ.tパッド選択通知( EKeyConfigPart.DRUMS, EKeyConfigPad.HH );
			}
			else if( this.list項目リスト[ this.n現在の選択項目 ] == this.iKeyAssignDrumsHHO )
			{
				CDTXMania.stageコンフィグ.tパッド選択通知( EKeyConfigPart.DRUMS, EKeyConfigPad.HHO );
			}
			else if( this.list項目リスト[ this.n現在の選択項目 ] == this.iKeyAssignDrumsSD )
			{
				CDTXMania.stageコンフィグ.tパッド選択通知( EKeyConfigPart.DRUMS, EKeyConfigPad.SD );
			}
			else if( this.list項目リスト[ this.n現在の選択項目 ] == this.iKeyAssignDrumsBD )
			{
				CDTXMania.stageコンフィグ.tパッド選択通知( EKeyConfigPart.DRUMS, EKeyConfigPad.BD );
			}
			else if( this.list項目リスト[ this.n現在の選択項目 ] == this.iKeyAssignDrumsHT )
			{
				CDTXMania.stageコンフィグ.tパッド選択通知( EKeyConfigPart.DRUMS, EKeyConfigPad.HT );
			}
			else if( this.list項目リスト[ this.n現在の選択項目 ] == this.iKeyAssignDrumsLT )
			{
				CDTXMania.stageコンフィグ.tパッド選択通知( EKeyConfigPart.DRUMS, EKeyConfigPad.LT );
			}
			else if( this.list項目リスト[ this.n現在の選択項目 ] == this.iKeyAssignDrumsFT )
			{
				CDTXMania.stageコンフィグ.tパッド選択通知( EKeyConfigPart.DRUMS, EKeyConfigPad.FT );
			}
			else if( this.list項目リスト[ this.n現在の選択項目 ] == this.iKeyAssignDrumsCY )
			{
				CDTXMania.stageコンフィグ.tパッド選択通知( EKeyConfigPart.DRUMS, EKeyConfigPad.CY );
			}
			else if( this.list項目リスト[ this.n現在の選択項目 ] == this.iKeyAssignDrumsRD )
			{
				CDTXMania.stageコンフィグ.tパッド選択通知( EKeyConfigPart.DRUMS, EKeyConfigPad.RD );
			}
			else if( this.list項目リスト[ this.n現在の選択項目 ] == this.iKeyAssignDrumsLP )			// #27029 2012.1.4 from
			{																							//
				CDTXMania.stageコンフィグ.tパッド選択通知( EKeyConfigPart.DRUMS, EKeyConfigPad.LP );	//
			}																							//
            else if( this.list項目リスト[ this.n現在の選択項目 ] == this.iKeyAssignDrumsLBD )
            {
                CDTXMania.stageコンフィグ.tパッド選択通知( EKeyConfigPart.DRUMS, EKeyConfigPad.LBD);
            }
			else if( this.list項目リスト[ this.n現在の選択項目 ] == this.iKeyAssignGuitarR )
			{
				CDTXMania.stageコンフィグ.tパッド選択通知( EKeyConfigPart.GUITAR, EKeyConfigPad.R );
			}
			else if( this.list項目リスト[ this.n現在の選択項目 ] == this.iKeyAssignGuitarG )
			{
				CDTXMania.stageコンフィグ.tパッド選択通知( EKeyConfigPart.GUITAR, EKeyConfigPad.G );
			}
			else if( this.list項目リスト[ this.n現在の選択項目 ] == this.iKeyAssignGuitarB )
			{
				CDTXMania.stageコンフィグ.tパッド選択通知( EKeyConfigPart.GUITAR, EKeyConfigPad.B );
			}
			else if( this.list項目リスト[ this.n現在の選択項目 ] == this.iKeyAssignGuitarPick )
			{
				CDTXMania.stageコンフィグ.tパッド選択通知( EKeyConfigPart.GUITAR, EKeyConfigPad.Pick );
			}
			else if( this.list項目リスト[ this.n現在の選択項目 ] == this.iKeyAssignGuitarWail )
			{
				CDTXMania.stageコンフィグ.tパッド選択通知( EKeyConfigPart.GUITAR, EKeyConfigPad.Wail );
			}
			else if( this.list項目リスト[ this.n現在の選択項目 ] == this.iKeyAssignGuitarDecide )
			{
				CDTXMania.stageコンフィグ.tパッド選択通知( EKeyConfigPart.GUITAR, EKeyConfigPad.Decide );
			}
			else if( this.list項目リスト[ this.n現在の選択項目 ] == this.iKeyAssignGuitarCancel )
			{
				CDTXMania.stageコンフィグ.tパッド選択通知( EKeyConfigPart.GUITAR, EKeyConfigPad.Cancel );
			}
			else if( this.list項目リスト[ this.n現在の選択項目 ] == this.iKeyAssignBassR )
			{
				CDTXMania.stageコンフィグ.tパッド選択通知( EKeyConfigPart.BASS, EKeyConfigPad.R );
			}
			else if( this.list項目リスト[ this.n現在の選択項目 ] == this.iKeyAssignBassG )
			{
				CDTXMania.stageコンフィグ.tパッド選択通知( EKeyConfigPart.BASS, EKeyConfigPad.G );
			}
			else if( this.list項目リスト[ this.n現在の選択項目 ] == this.iKeyAssignBassB )
			{
				CDTXMania.stageコンフィグ.tパッド選択通知( EKeyConfigPart.BASS, EKeyConfigPad.B );
			}
			else if( this.list項目リスト[ this.n現在の選択項目 ] == this.iKeyAssignBassPick )
			{
				CDTXMania.stageコンフィグ.tパッド選択通知( EKeyConfigPart.BASS, EKeyConfigPad.Pick );
			}
			else if( this.list項目リスト[ this.n現在の選択項目 ] == this.iKeyAssignBassWail )
			{
				CDTXMania.stageコンフィグ.tパッド選択通知( EKeyConfigPart.BASS, EKeyConfigPad.Wail );
			}
			else if( this.list項目リスト[ this.n現在の選択項目 ] == this.iKeyAssignBassDecide )
			{
				CDTXMania.stageコンフィグ.tパッド選択通知( EKeyConfigPart.BASS, EKeyConfigPad.Decide );
			}
			else if ( this.list項目リスト[ this.n現在の選択項目 ] == this.iKeyAssignBassCancel )
			{
				CDTXMania.stageコンフィグ.tパッド選択通知( EKeyConfigPart.BASS, EKeyConfigPad.Cancel );
			}

            //太鼓のキー設定。
			else if( this.list項目リスト[ this.n現在の選択項目 ] == this.iKeyAssignTaikoLRed )
			{
				CDTXMania.stageコンフィグ.tパッド選択通知( EKeyConfigPart.DRUMS, EKeyConfigPad.LRed );
			}
			else if( this.list項目リスト[ this.n現在の選択項目 ] == this.iKeyAssignTaikoRRed )
			{
				CDTXMania.stageコンフィグ.tパッド選択通知( EKeyConfigPart.DRUMS, EKeyConfigPad.RRed );
			}
			else if( this.list項目リスト[ this.n現在の選択項目 ] == this.iKeyAssignTaikoLBlue )
			{
				CDTXMania.stageコンフィグ.tパッド選択通知( EKeyConfigPart.DRUMS, EKeyConfigPad.LBlue );
			}
			else if ( this.list項目リスト[ this.n現在の選択項目 ] == this.iKeyAssignTaikoRBlue )
			{
				CDTXMania.stageコンフィグ.tパッド選択通知( EKeyConfigPart.DRUMS, EKeyConfigPad.RBlue );
			}

            //太鼓のキー設定。2P
			else if( this.list項目リスト[ this.n現在の選択項目 ] == this.iKeyAssignTaikoLRed2P )
			{
				CDTXMania.stageコンフィグ.tパッド選択通知( EKeyConfigPart.DRUMS, EKeyConfigPad.LRed2P );
			}
			else if( this.list項目リスト[ this.n現在の選択項目 ] == this.iKeyAssignTaikoRRed2P )
			{
				CDTXMania.stageコンフィグ.tパッド選択通知( EKeyConfigPart.DRUMS, EKeyConfigPad.RRed2P );
			}
			else if( this.list項目リスト[ this.n現在の選択項目 ] == this.iKeyAssignTaikoLBlue2P )
			{
				CDTXMania.stageコンフィグ.tパッド選択通知( EKeyConfigPart.DRUMS, EKeyConfigPad.LBlue2P );
			}
			else if ( this.list項目リスト[ this.n現在の選択項目 ] == this.iKeyAssignTaikoRBlue2P )
			{
				CDTXMania.stageコンフィグ.tパッド選択通知( EKeyConfigPart.DRUMS, EKeyConfigPad.RBlue2P );
			}

			else if ( this.list項目リスト[ this.n現在の選択項目 ] == this.iKeyAssignSystemCapture )
			{
				CDTXMania.stageコンフィグ.tパッド選択通知( EKeyConfigPart.SYSTEM, EKeyConfigPad.Capture);
			}
			#endregion
			else
			{
		 		// #27029 2012.1.5 from
                //if( ( this.iSystemBDGroup.n現在選択されている項目番号 == (int) EBDGroup.どっちもBD ) &&
                //    ( ( this.list項目リスト[ this.n現在の選択項目 ] == this.iSystemHHGroup ) || ( this.list項目リスト[ this.n現在の選択項目 ] == this.iSystemHitSoundPriorityHH ) ) )
                //{
                //    // 変更禁止（何もしない）
                //}
                //else
                //{
                //    // 変更許可
                    this.list項目リスト[ this.n現在の選択項目 ].tEnter押下();
                //}


				// Enter押下後の後処理

				if( this.list項目リスト[ this.n現在の選択項目 ] == this.iSystemFullscreen )
				{
					CDTXMania.app.b次のタイミングで全画面_ウィンドウ切り替えを行う = true;
				}
				else if( this.list項目リスト[ this.n現在の選択項目 ] == this.iSystemVSyncWait )
				{
					CDTXMania.ConfigIni.b垂直帰線待ちを行う = this.iSystemVSyncWait.bON;
					CDTXMania.app.b次のタイミングで垂直帰線同期切り替えを行う = true;
				}
				#region [ AutoPlay #23886 2012.5.8 yyagi ]
				else if ( this.list項目リスト[ this.n現在の選択項目 ] == this.iDrumsAutoPlayAll )
				{
					this.t全部のドラムパッドのAutoを切り替える( this.iDrumsAutoPlayAll.e現在の状態 == CItemThreeState.E状態.ON );
				}
				else if ( this.list項目リスト[ this.n現在の選択項目 ] == this.iGuitarAutoPlayAll )
				{
					this.t全部のギターパッドのAutoを切り替える( this.iGuitarAutoPlayAll.e現在の状態 == CItemThreeState.E状態.ON );
				}
				else if ( this.list項目リスト[ this.n現在の選択項目 ] == this.iBassAutoPlayAll )
				{
					this.t全部のベースパッドのAutoを切り替える( this.iBassAutoPlayAll.e現在の状態 == CItemThreeState.E状態.ON );
				}
				#endregion
				#region [ キーアサインへの遷移と脱出 ]
				else if ( this.list項目リスト[ this.n現在の選択項目 ] == this.iSystemGoToKeyAssign )			// #24609 2011.4.12 yyagi
				{
					t項目リストの設定_KeyAssignSystem();
				}
				else if ( this.list項目リスト[ this.n現在の選択項目 ] == this.iKeyAssignSystemReturnToMenu )	// #24609 2011.4.12 yyagi
				{
					t項目リストの設定_System();
				}
				else if ( this.list項目リスト[ this.n現在の選択項目 ] == this.iDrumsGoToKeyAssign )				// #24525 2011.3.15 yyagi
				{
					t項目リストの設定_KeyAssignDrums();
				}
				else if ( this.list項目リスト[ this.n現在の選択項目 ] == this.iKeyAssignDrumsReturnToMenu )		// #24525 2011.3.15 yyagi
				{
					t項目リストの設定_Drums();
				}
				else if ( this.list項目リスト[ this.n現在の選択項目 ] == this.iGuitarGoToKeyAssign )			// #24525 2011.3.15 yyagi
				{
					t項目リストの設定_KeyAssignGuitar();
				}
				else if ( this.list項目リスト[ this.n現在の選択項目 ] == this.iKeyAssignGuitarReturnToMenu )	// #24525 2011.3.15 yyagi
				{
					t項目リストの設定_Guitar();
				}
				else if ( this.list項目リスト[ this.n現在の選択項目 ] == this.iBassGoToKeyAssign )				// #24525 2011.3.15 yyagi
				{
					t項目リストの設定_KeyAssignBass();
				}
				else if ( this.list項目リスト[ this.n現在の選択項目 ] == this.iKeyAssignBassReturnToMenu )		// #24525 2011.3.15 yyagi
				{
					t項目リストの設定_Bass();
				}
				#endregion
				else if ( this.list項目リスト[ this.n現在の選択項目 ] == this.iSystemUseBoxDefSkin )			// #28195 2012.5.6 yyagi
				{
					CSkin.bUseBoxDefSkin = this.iSystemUseBoxDefSkin.bON;
				}
				#region [ スキン項目でEnterを押下した場合に限り、スキンの縮小サンプルを生成する。]
				else if ( this.list項目リスト[ this.n現在の選択項目 ] == this.iSystemSkinSubfolder )			// #28195 2012.5.2 yyagi
				{
					tGenerateSkinSample();
				}
				#endregion
				#region [ 曲データ一覧の再読み込み ]
				else if ( this.list項目リスト[ this.n現在の選択項目 ] == this.iSystemReloadDTX )				// #32081 2013.10.21 yyagi
				{
					if ( CDTXMania.EnumSongs.IsEnumerating )
					{
						// Debug.WriteLine( "バックグラウンドでEnumeratingSongs中だったので、一旦中断します。" );
						CDTXMania.EnumSongs.Abort();
						CDTXMania.actEnumSongs.On非活性化();
					}

					CDTXMania.EnumSongs.StartEnumFromDisk();
					CDTXMania.EnumSongs.ChangeEnumeratePriority( ThreadPriority.Normal );
					CDTXMania.actEnumSongs.bコマンドでの曲データ取得 = true;
					CDTXMania.actEnumSongs.On活性化();
				}
				#endregion
			}
		}

		private void tGenerateSkinSample()
		{
			nSkinIndex = ( ( CItemList ) this.list項目リスト[ this.n現在の選択項目 ] ).n現在選択されている項目番号;
			if ( nSkinSampleIndex != nSkinIndex )
			{
				string path = skinSubFolders[ nSkinIndex ];
				path = System.IO.Path.Combine( path, @"Graphics\1_Title\Background.png" );
				Bitmap bmSrc = new Bitmap( path );
				Bitmap bmDest = new Bitmap( bmSrc.Width / 4, bmSrc.Height / 4 );
				Graphics g = Graphics.FromImage( bmDest );
				g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
				g.DrawImage( bmSrc, new Rectangle( 0, 0, bmSrc.Width / 4, bmSrc.Height / 4 ),
					0, 0, bmSrc.Width, bmSrc.Height, GraphicsUnit.Pixel );
				if ( txSkinSample1 != null )
				{
					CDTXMania.t安全にDisposeする( ref txSkinSample1 );
				}
				txSkinSample1 = CDTXMania.tテクスチャの生成( bmDest, false );
				g.Dispose();
				bmDest.Dispose();
				bmSrc.Dispose();
				nSkinSampleIndex = nSkinIndex;
			}
		}

		#region [ 項目リストの設定 ( Exit, KeyAssignSystem/Drums/Guitar/Bass) ]
		public void t項目リストの設定_Exit()
		{
			this.tConfigIniへ記録する();
			this.eメニュー種別 = Eメニュー種別.Unknown;
		}
		public void t項目リストの設定_KeyAssignSystem()
		{
			//this.tConfigIniへ記録する();
			this.list項目リスト.Clear();

			// #27029 2012.1.5 from: 説明文は最大9行→13行に変更。

			this.iKeyAssignSystemReturnToMenu = new CItemBase( "<< もどる", CItemBase.Eパネル種別.その他,
				"左側のメニューに戻ります。",
				"Return to left menu." );
			this.list項目リスト.Add( this.iKeyAssignSystemReturnToMenu );
			this.iKeyAssignSystemCapture = new CItemBase( "スクリーンショット",
				"キャプチャキー設定：\n画面キャプチャのキーの割り当てを設\n定します。",
				"Capture key assign:\nTo assign key for screen capture.\n (You can use keyboard only. You can't\nuse pads to capture screenshot." );
			this.list項目リスト.Add( this.iKeyAssignSystemCapture );

            OnListMenuの初期化();
			this.n現在の選択項目 = 0;
			this.eメニュー種別 = Eメニュー種別.KeyAssignSystem;
		}
		public void t項目リストの設定_KeyAssignDrums()
		{
//			this.tConfigIniへ記録する();
			this.list項目リスト.Clear();

			// #27029 2012.1.5 from: 説明文は最大9行→13行に変更。

			this.iKeyAssignDrumsReturnToMenu = new CItemBase( "<< もどる", CItemBase.Eパネル種別.その他,
				"左側のメニューに戻ります。",
				"Return to left menu.");
			this.list項目リスト.Add(this.iKeyAssignDrumsReturnToMenu);
            //this.iKeyAssignDrumsLC = new CItemBase( "LeftCymbal",
            //    "ドラムのキー設定：\n左シンバルへのキーの割り当てを設\n定します。",
            //    "Drums key assign:\nTo assign key/pads for LeftCymbal\n button.");
            //this.list項目リスト.Add(this.iKeyAssignDrumsLC);
            //this.iKeyAssignDrumsHHC = new CItemBase( "HiHat(Close)",
            //    "ドラムのキー設定：\nハイハット（クローズ）へのキーの割り\n当てを設定します。",
            //    "Drums key assign:\nTo assign key/pads for HiHat(Close)\n button.");
            //this.list項目リスト.Add( this.iKeyAssignDrumsHHC );
            //this.iKeyAssignDrumsHHO = new CItemBase( "HiHat(Open)",
            //    "ドラムのキー設定：\nハイハット（オープン）へのキーの割り\n当てを設定します。",
            //    "Drums key assign:\nTo assign key/pads for HiHat(Open)\n button.");
            //this.list項目リスト.Add(this.iKeyAssignDrumsHHO);
            //this.iKeyAssignDrumsSD = new CItemBase( "Snare",
            //    "ドラムのキー設定：\nスネアへのキーの割り当てを設定し\nます。",
            //    "Drums key assign:\nTo assign key/pads for Snare button.");
            //this.list項目リスト.Add(this.iKeyAssignDrumsSD);
            //this.iKeyAssignDrumsBD = new CItemBase( "Bass",
            //    "ドラムのキー設定：\nバスドラムへのキーの割り当てを設定\nします。",
            //    "Drums key assign:\nTo assign key/pads for Bass button.");
            //this.list項目リスト.Add(this.iKeyAssignDrumsBD);
            //this.iKeyAssignDrumsHT = new CItemBase( "HighTom",
            //    "ドラムのキー設定：\nハイタムへのキーの割り当てを設定\nします。",
            //    "Drums key assign:\nTo assign key/pads for HighTom\n button.");
            //this.list項目リスト.Add(this.iKeyAssignDrumsHT);
            //this.iKeyAssignDrumsLT = new CItemBase( "LowTom",
            //    "ドラムのキー設定：\nロータムへのキーの割り当てを設定\nします。",
            //    "Drums key assign:\nTo assign key/pads for LowTom button.");
            //this.list項目リスト.Add(this.iKeyAssignDrumsLT);
            //this.iKeyAssignDrumsFT = new CItemBase( "FloorTom",
            //    "ドラムのキー設定：\nフロアタムへのキーの割り当てを設\n定します。",
            //    "Drums key assign:\nTo assign key/pads for FloorTom\n button.");
            //this.list項目リスト.Add(this.iKeyAssignDrumsFT);
            //this.iKeyAssignDrumsCY = new CItemBase( "RightCymbal",
            //    "ドラムのキー設定：\n右シンバルへのキーの割り当てを設\n定します。",
            //    "Drums key assign:\nTo assign key/pads for RightCymbal\n button.");
            //this.list項目リスト.Add(this.iKeyAssignDrumsCY);
            //this.iKeyAssignDrumsRD = new CItemBase( "RideCymbal",
            //    "ドラムのキー設定：\nライドシンバルへのキーの割り当て\nを設定します。",
            //    "Drums key assign:\nTo assign key/pads for RideCymbal\n button.");
            //this.list項目リスト.Add(this.iKeyAssignDrumsRD);
            //this.iKeyAssignDrumsLP = new CItemBase( "HiHatPedal",									// #27029 2012.1.4 from
            //    "ドラムのキー設定：\nハイハットペダルへのキーの\n割り当てを設定します。",	        //
            //    "Drums key assign:\nTo assign key/pads for HiHatPedal\n button." );					//
            //this.list項目リスト.Add( this.iKeyAssignDrumsLP );										//
            //this.iKeyAssignDrumsLBD = new CItemBase( "LeftBassDrum",
            //    "ドラムのキー設定：\n左バスペダルへのキーの\n割り当てを設定します。",
            //    "Drums key assign:\nTo assign key/pads for LeftBassDrum\n button." );
            //this.list項目リスト.Add( this.iKeyAssignDrumsLBD );	


			this.iKeyAssignTaikoLRed = new CItemBase( "面(左) 1P",
				"左側の面へのキーの割り当てを設\n定します。",
				"Drums key assign:\nTo assign key/pads for RightCymbal\n button.");
			this.list項目リスト.Add(this.iKeyAssignTaikoLRed);
			this.iKeyAssignTaikoRRed = new CItemBase("面(右) 1P",
			    "右側の面へのキーの割り当て\nを設定します。",
				"Drums key assign:\nTo assign key/pads for RideCymbal\n button.");
			this.list項目リスト.Add(this.iKeyAssignTaikoRRed);
			this.iKeyAssignTaikoLBlue = new CItemBase( "縁(左) 1P",
				"左側のふちへのキーの\n割り当てを設定します。",	
				"Drums key assign:\nTo assign key/pads for HiHatPedal\n button." );
			this.list項目リスト.Add( this.iKeyAssignTaikoLBlue );
            this.iKeyAssignTaikoRBlue = new CItemBase( "縁(右) 1P",
                "右側のふちへのキーの\n割り当てを設定します。",
				"Drums key assign:\nTo assign key/pads for LeftBassDrum\n button." );
			this.list項目リスト.Add( this.iKeyAssignTaikoRBlue );

			this.iKeyAssignTaikoLRed2P = new CItemBase("面(左) 2P",
				"左側の面へのキーの割り当てを設\n定します。",
				"Drums key assign:\nTo assign key/pads for RightCymbal\n button.");
			this.list項目リスト.Add( this.iKeyAssignTaikoLRed2P );
			this.iKeyAssignTaikoRRed2P = new CItemBase("面(右) 2P",
			    "右側の面へのキーの割り当て\nを設定します。",
				"Drums key assign:\nTo assign key/pads for RideCymbal\n button.");
			this.list項目リスト.Add( this.iKeyAssignTaikoRRed2P );
			this.iKeyAssignTaikoLBlue2P = new CItemBase("縁(左) 2P",
				"左側のふちへのキーの\n割り当てを設定します。",	
				"Drums key assign:\nTo assign key/pads for HiHatPedal\n button." );
			this.list項目リスト.Add( this.iKeyAssignTaikoLBlue2P );
            this.iKeyAssignTaikoRBlue2P = new CItemBase("縁(右) 2P",
                "右側のふちへのキーの\n割り当てを設定します。",
				"Drums key assign:\nTo assign key/pads for LeftBassDrum\n button." );
			this.list項目リスト.Add( this.iKeyAssignTaikoRBlue2P );

            OnListMenuの初期化();
			this.n現在の選択項目 = 0;
			this.eメニュー種別 = Eメニュー種別.KeyAssignDrums;
		}
		public void t項目リストの設定_KeyAssignGuitar()
		{
//			this.tConfigIniへ記録する();
			this.list項目リスト.Clear();

			// #27029 2012.1.5 from: 説明文は最大9行→13行に変更。

			this.iKeyAssignGuitarReturnToMenu = new CItemBase( "<< ReturnTo Menu", CItemBase.Eパネル種別.その他,
				"左側のメニューに戻ります。",
				"Return to left menu.");
			this.list項目リスト.Add(this.iKeyAssignGuitarReturnToMenu);
            //this.iKeyAssignGuitarR = new CItemBase( "R",
            //    "ギターのキー設定：\nRボタンへのキーの割り当てを設定し\nます。",
            //    "Guitar key assign:\nTo assign key/pads for R button.");
            //this.list項目リスト.Add(this.iKeyAssignGuitarR);
            //this.iKeyAssignGuitarG = new CItemBase( "G",
            //    "ギターのキー設定：\nGボタンへのキーの割り当てを設定し\nます。",
            //    "Guitar key assign:\nTo assign key/pads for G button.");
            //this.list項目リスト.Add(this.iKeyAssignGuitarG);
            //this.iKeyAssignGuitarB = new CItemBase( "B",
            //    "ギターのキー設定：\nBボタンへのキーの割り当てを設定し\nます。",
            //    "Guitar key assign:\nTo assign key/pads for B button.");
            //this.list項目リスト.Add(this.iKeyAssignGuitarB);
            //this.iKeyAssignGuitarPick = new CItemBase( "Pick",
            //    "ギターのキー設定：\nピックボタンへのキーの割り当てを設\n定します。",
            //    "Guitar key assign:\nTo assign key/pads for Pick button.");
            //this.list項目リスト.Add(this.iKeyAssignGuitarPick);
            //this.iKeyAssignGuitarWail = new CItemBase( "Wailing",
            //    "ギターのキー設定：\nWailingボタンへのキーの割り当てを\n設定します。",
            //    "Guitar key assign:\nTo assign key/pads for Wailing button.");
            //this.list項目リスト.Add(this.iKeyAssignGuitarWail);
            //this.iKeyAssignGuitarDecide = new CItemBase( "Decide",
            //    "ギターのキー設定：\n決定ボタンへのキーの割り当てを設\n定します。",
            //    "Guitar key assign:\nTo assign key/pads for Decide button.");
            //this.list項目リスト.Add(this.iKeyAssignGuitarDecide);
            //this.iKeyAssignGuitarCancel = new CItemBase( "Cancel",
            //    "ギターのキー設定：\nキャンセルボタンへのキーの割り当\nてを設定します。",
            //    "Guitar key assign:\nTo assign key/pads for Cancel button.");
            //this.list項目リスト.Add(this.iKeyAssignGuitarCancel);

            OnListMenuの初期化();
			this.n現在の選択項目 = 0;
			this.eメニュー種別 = Eメニュー種別.KeyAssignGuitar;
		}
		public void t項目リストの設定_KeyAssignBass()
		{
//			this.tConfigIniへ記録する();
			this.list項目リスト.Clear();

			// #27029 2012.1.5 from: 説明文は最大9行→13行に変更。

			this.iKeyAssignBassReturnToMenu = new CItemBase( "<< ReturnTo Menu", CItemBase.Eパネル種別.その他,
				"左側のメニューに戻ります。",
				"Return to left menu." );
			this.list項目リスト.Add( this.iKeyAssignBassReturnToMenu );
            //this.iKeyAssignBassR = new CItemBase( "R",
            //    "ベースのキー設定：\nRボタンへのキーの割り当てを設定し\nます。",
            //    "Bass key assign:\nTo assign key/pads for R button." );
            //this.list項目リスト.Add( this.iKeyAssignBassR );
            //this.iKeyAssignBassG = new CItemBase( "G",
            //    "ベースのキー設定：\nGボタンへのキーの割り当てを設定し\nます。",
            //    "Bass key assign:\nTo assign key/pads for G button." );
            //this.list項目リスト.Add( this.iKeyAssignBassG );
            //this.iKeyAssignBassB = new CItemBase( "B",
            //    "ベースのキー設定：\nBボタンへのキーの割り当てを設定し\nます。",
            //    "Bass key assign:\nTo assign key/pads for B button." );
            //this.list項目リスト.Add( this.iKeyAssignBassB );
            //this.iKeyAssignBassPick = new CItemBase( "Pick",
            //    "ベースのキー設定：\nピックボタンへのキーの割り当てを設\n定します。",
            //    "Bass key assign:\nTo assign key/pads for Pick button." );
            //this.list項目リスト.Add( this.iKeyAssignBassPick );
            //this.iKeyAssignBassWail = new CItemBase( "Wailing",
            //    "ベースのキー設定：\nWailingボタンへのキーの割り当てを設\n定します。",
            //    "Bass key assign:\nTo assign key/pads for Wailing button." );
            //this.list項目リスト.Add( this.iKeyAssignBassWail );
            //this.iKeyAssignBassDecide = new CItemBase( "Decide",
            //    "ベースのキー設定：\n決定ボタンへのキーの割り当てを設\n定します。",
            //    "Bass key assign:\nTo assign key/pads for Decide button." );
            //this.list項目リスト.Add( this.iKeyAssignBassDecide );
            //this.iKeyAssignBassCancel = new CItemBase( "Cancel",
            //    "ベースのキー設定：\nキャンセルボタンへのキーの割り当\nてを設定します。",
            //    "Bass key assign:\nTo assign key/pads for Cancel button." );
            //this.list項目リスト.Add( this.iKeyAssignBassCancel );
            OnListMenuの初期化();
			this.n現在の選択項目 = 0;
			this.eメニュー種別 = Eメニュー種別.KeyAssignBass;
		}
		#endregion
		public void t次に移動()
		{
			CDTXMania.Skin.soundカーソル移動音.t再生する();
			if( this.b要素値にフォーカス中 )
			{
				this.list項目リスト[ this.n現在の選択項目 ].t項目値を前へ移動();
				t要素値を上下に変更中の処理();
			}
			else
			{
				this.n目標のスクロールカウンタ += 100;
			}
		}
		public void t前に移動()
		{
			CDTXMania.Skin.soundカーソル移動音.t再生する();
			if( this.b要素値にフォーカス中 )
			{
				this.list項目リスト[ this.n現在の選択項目 ].t項目値を次へ移動();
				t要素値を上下に変更中の処理();
			}
			else
			{
				this.n目標のスクロールカウンタ -= 100;
			}
		}
		private void t要素値を上下に変更中の処理()
		{
			//if ( this.list項目リスト[ this.n現在の選択項目 ] == this.iSystemMasterVolume )				// #33700 2014.4.26 yyagi
			//{
			//    CDTXMania.Sound管理.nMasterVolume = this.iSystemMasterVolume.n現在の値;
			//}
		}


		// CActivity 実装

		public override void On活性化()
		{
			if( this.b活性化してる )
				return;

			this.list項目リスト = new List<CItemBase>();
			this.eメニュー種別 = Eメニュー種別.Unknown;

			#region [ スキン選択肢と、現在選択中のスキン(index)の準備 #28195 2012.5.2 yyagi ]
			int ns = ( CDTXMania.Skin.strSystemSkinSubfolders == null ) ? 0 : CDTXMania.Skin.strSystemSkinSubfolders.Length;
			int nb = ( CDTXMania.Skin.strBoxDefSkinSubfolders == null ) ? 0 : CDTXMania.Skin.strBoxDefSkinSubfolders.Length;
			skinSubFolders = new string[ ns + nb ];
			for ( int i = 0; i < ns; i++ )
			{
				skinSubFolders[ i ] = CDTXMania.Skin.strSystemSkinSubfolders[ i ];
			}
			for ( int i = 0; i < nb; i++ )
			{
				skinSubFolders[ ns + i ] = CDTXMania.Skin.strBoxDefSkinSubfolders[ i ];
			}
			skinSubFolder_org = CDTXMania.Skin.GetCurrentSkinSubfolderFullName( true );
			Array.Sort( skinSubFolders );
			skinNames = CSkin.GetSkinName( skinSubFolders );
			nSkinIndex = Array.BinarySearch( skinSubFolders, skinSubFolder_org );
			if ( nSkinIndex < 0 )	// 念のため
			{
				nSkinIndex = 0;
			}
			nSkinSampleIndex = -1;
			#endregion

            if ( !string.IsNullOrEmpty(CDTXMania.ConfigIni.FontName))
			    this.prvFont = new CPrivateFastFont(new FontFamily(CDTXMania.ConfigIni.FontName), 20 );	// t項目リストの設定 の前に必要
            else
                this.prvFont = new CPrivateFastFont(new FontFamily("MS UI Gothic"), 20);

            //			this.listMenu = new List<stMenuItemRight>();

            this.t項目リストの設定_Bass();		// #27795 2012.3.11 yyagi; System設定の中でDrumsの設定を参照しているため、
			this.t項目リストの設定_Guitar();	// 活性化の時点でDrumsの設定も入れ込んでおかないと、System設定中に例外発生することがある。
			this.t項目リストの設定_Drums();	// 
			this.t項目リストの設定_System();	// 順番として、最後にSystemを持ってくること。設定一覧の初期位置がSystemのため。
			this.b要素値にフォーカス中 = false;
			this.n目標のスクロールカウンタ = 0;
			this.n現在のスクロールカウンタ = 0;
			this.nスクロール用タイマ値 = -1;
			this.ct三角矢印アニメ = new CCounter();

			this.iSystemSoundType_initial			= this.iSystemSoundType.n現在選択されている項目番号;	// CONFIGに入ったときの値を保持しておく
			// this.iSystemWASAPIBufferSizeMs_initial	= this.iSystemWASAPIBufferSizeMs.n現在の値;				// CONFIG脱出時にこの値から変更されているようなら
			// this.iSystemASIOBufferSizeMs_initial	= this.iSystemASIOBufferSizeMs.n現在の値;				// サウンドデバイスを再構築する
			this.iSystemASIODevice_initial			= this.iSystemASIODevice.n現在選択されている項目番号;	//
			this.iSystemSoundTimerType_initial      = this.iSystemSoundTimerType.GetIndex();				//
			base.On活性化();
		}
		public override void On非活性化()
		{
			if( this.b活性化してない )
				return;

			this.tConfigIniへ記録する();
			this.list項目リスト.Clear();
			this.ct三角矢印アニメ = null;
            
			prvFont.Dispose();
			base.On非活性化();
			#region [ Skin変更 ]
			if ( CDTXMania.Skin.GetCurrentSkinSubfolderFullName( true ) != this.skinSubFolder_org )
			{
				CDTXMania.stageChangeSkin.tChangeSkinMain();	// #28195 2012.6.11 yyagi CONFIG脱出時にSkin更新
			}
			#endregion

			// #24820 2013.1.22 yyagi CONFIGでWASAPI/ASIO/DirectSound関連の設定を変更した場合、サウンドデバイスを再構築する。
			// #33689 2014.6.17 yyagi CONFIGでSoundTimerTypeの設定を変更した場合も、サウンドデバイスを再構築する。
			#region [ サウンドデバイス変更 ]
			if ( this.iSystemSoundType_initial != this.iSystemSoundType.n現在選択されている項目番号 ||
				 this.iSystemWASAPIBufferSizeMs_initial != this.iSystemWASAPIBufferSizeMs.n現在の値 ||
				// this.iSystemASIOBufferSizeMs_initial != this.iSystemASIOBufferSizeMs.n現在の値 ||
				this.iSystemASIODevice_initial != this.iSystemASIODevice.n現在選択されている項目番号 ||
				this.iSystemSoundTimerType_initial != this.iSystemSoundTimerType.GetIndex() )
			{
				ESoundDeviceType soundDeviceType;
				switch ( this.iSystemSoundType.n現在選択されている項目番号 )
				{
					case 0:
						soundDeviceType = ESoundDeviceType.DirectSound;
						break;
					case 1:
						soundDeviceType = ESoundDeviceType.ASIO;
						break;
					case 2:
						soundDeviceType = ESoundDeviceType.ExclusiveWASAPI;
						break;
					default:
						soundDeviceType = ESoundDeviceType.Unknown;
						break;
				}

				CDTXMania.Sound管理.t初期化( soundDeviceType,
										this.iSystemWASAPIBufferSizeMs.n現在の値,
										0,
										// this.iSystemASIOBufferSizeMs.n現在の値,
										this.iSystemASIODevice.n現在選択されている項目番号,
										this.iSystemSoundTimerType.bON );
				CDTXMania.app.ShowWindowTitleWithSoundType();
			}
			#endregion
			#region [ サウンドのタイムストレッチモード変更 ]
			FDK.CSound管理.bIsTimeStretch = this.iSystemTimeStretch.bON;
			#endregion
		}
		public override void OnManagedリソースの作成()
		{
			if( this.b活性化してない )
				return;

			//this.tx通常項目行パネル = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\4_itembox.png" ), false );
			//this.txその他項目行パネル = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\4_itembox other.png" ), false );
			//this.tx三角矢印 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\4_triangle arrow.png" ), false );
			this.txSkinSample1 = null;		// スキン選択時に動的に設定するため、ここでは初期化しない
			base.OnManagedリソースの作成();
		}
		public override void OnManagedリソースの解放()
		{
			if( this.b活性化してない )
				return;

			CDTXMania.tテクスチャの解放( ref this.txSkinSample1 );
			//CDTXMania.tテクスチャの解放( ref this.tx通常項目行パネル );
			//CDTXMania.tテクスチャの解放( ref this.txその他項目行パネル );
			//CDTXMania.tテクスチャの解放( ref this.tx三角矢印 );
		
			base.OnManagedリソースの解放();
		}
		private void OnListMenuの初期化()
		{
			OnListMenuの解放();
			this.listMenu = new stMenuItemRight[ this.list項目リスト.Count ];
		}

		/// <summary>
		/// 事前にレンダリングしておいたテクスチャを解放する。
		/// </summary>
		private void OnListMenuの解放()
		{
			if ( listMenu != null )
			{
				for ( int i = 0; i < listMenu.Length; i++ )
				{
					if ( listMenu[ i ].txParam != null )
					{
						listMenu[ i ].txParam.Dispose();
					}
					if ( listMenu[ i ].txMenuItemRight != null )
					{
						listMenu[ i ].txMenuItemRight.Dispose();
					}
				}
				this.listMenu = null;
			}
		}
		public override int On進行描画()
		{
			throw new InvalidOperationException( "t進行描画(bool)のほうを使用してください。" );
		}
		public int t進行描画( bool b項目リスト側にフォーカスがある )
		{
			if( this.b活性化してない )
				return 0;

			// 進行

			#region [ 初めての進行描画 ]
			//-----------------
			if( base.b初めての進行描画 )
			{
                this.nスクロール用タイマ値 = CSound管理.rc演奏用タイマ.n現在時刻;
				this.ct三角矢印アニメ.t開始( 0, 9, 50, CDTXMania.Timer );
			
				base.b初めての進行描画 = false;
			}
			//-----------------
			#endregion

			this.b項目リスト側にフォーカスがある = b項目リスト側にフォーカスがある;		// 記憶

			#region [ 項目スクロールの進行 ]
			//-----------------
			long n現在時刻 = CDTXMania.Timer.n現在時刻;
			if( n現在時刻 < this.nスクロール用タイマ値 ) this.nスクロール用タイマ値 = n現在時刻;

			const int INTERVAL = 2;	// [ms]
			while( ( n現在時刻 - this.nスクロール用タイマ値 ) >= INTERVAL )
			{
				int n目標項目までのスクロール量 = Math.Abs( (int) ( this.n目標のスクロールカウンタ - this.n現在のスクロールカウンタ ) );
				int n加速度 = 0;

				#region [ n加速度の決定；目標まで遠いほど加速する。]
				//-----------------
				if( n目標項目までのスクロール量 <= 100 )
				{
					n加速度 = 2;
				}
				else if( n目標項目までのスクロール量 <= 300 )
				{
					n加速度 = 3;
				}
				else if( n目標項目までのスクロール量 <= 500 )
				{
					n加速度 = 4;
				}
				else
				{
					n加速度 = 8;
				}
				//-----------------
				#endregion
				#region [ this.n現在のスクロールカウンタに n加速度 を加減算。]
				//-----------------
				if( this.n現在のスクロールカウンタ < this.n目標のスクロールカウンタ )
				{
					this.n現在のスクロールカウンタ += n加速度;
					if( this.n現在のスクロールカウンタ > this.n目標のスクロールカウンタ )
					{
						// 目標を超えたら目標値で停止。
						this.n現在のスクロールカウンタ = this.n目標のスクロールカウンタ;
					}
				}
				else if( this.n現在のスクロールカウンタ > this.n目標のスクロールカウンタ )
				{
					this.n現在のスクロールカウンタ -= n加速度;
					if( this.n現在のスクロールカウンタ < this.n目標のスクロールカウンタ )
					{
						// 目標を超えたら目標値で停止。
						this.n現在のスクロールカウンタ = this.n目標のスクロールカウンタ;
					}
				}
				//-----------------
				#endregion
				#region [ 行超え処理、ならびに目標位置に到達したらスクロールを停止して項目変更通知を発行。]
				//-----------------
				if( this.n現在のスクロールカウンタ >= 100 )
				{
					this.n現在の選択項目 = this.t次の項目( this.n現在の選択項目 );
					this.n現在のスクロールカウンタ -= 100;
					this.n目標のスクロールカウンタ -= 100;
					if( this.n目標のスクロールカウンタ == 0 )
					{
						CDTXMania.stageコンフィグ.t項目変更通知();
					}
				}
				else if( this.n現在のスクロールカウンタ <= -100 )
				{
					this.n現在の選択項目 = this.t前の項目( this.n現在の選択項目 );
					this.n現在のスクロールカウンタ += 100;
					this.n目標のスクロールカウンタ += 100;
					if( this.n目標のスクロールカウンタ == 0 )
					{
						CDTXMania.stageコンフィグ.t項目変更通知();
					}
				}
				//-----------------
				#endregion

				this.nスクロール用タイマ値 += INTERVAL;
			}
			//-----------------
			#endregion
			
			#region [ ▲印アニメの進行 ]
			//-----------------
			if( this.b項目リスト側にフォーカスがある && ( this.n目標のスクロールカウンタ == 0 ) )
				this.ct三角矢印アニメ.t進行Loop();
			//-----------------
			#endregion


			// 描画

			this.ptパネルの基本座標[ 4 ].X = this.b項目リスト側にフォーカスがある ? 0x228 : 0x25a;		// メニューにフォーカスがあるなら、項目リストの中央は頭を出さない。

			#region [ 計11個の項目パネルを描画する。]
			//-----------------
			int nItem = this.n現在の選択項目;
			for( int i = 0; i < 4; i++ )
				nItem = this.t前の項目( nItem );

			for( int n行番号 = -4; n行番号 < 6; n行番号++ )		// n行番号 == 0 がフォーカスされている項目パネル。
			{
				#region [ 今まさに画面外に飛びだそうとしている項目パネルは描画しない。]
				//-----------------
				if( ( ( n行番号 == -4 ) && ( this.n現在のスクロールカウンタ > 0 ) ) ||		// 上に飛び出そうとしている
					( ( n行番号 == +5 ) && ( this.n現在のスクロールカウンタ < 0 ) ) )		// 下に飛び出そうとしている
				{
					nItem = this.t次の項目( nItem );
					continue;
				}
				//-----------------
				#endregion

				int n移動元の行の基本位置 = n行番号 + 4;
				int n移動先の行の基本位置 = ( this.n現在のスクロールカウンタ <= 0 ) ? ( ( n移動元の行の基本位置 + 1 ) % 10 ) : ( ( ( n移動元の行の基本位置 - 1 ) + 10 ) % 10 );
				int x = this.ptパネルの基本座標[ n移動元の行の基本位置 ].X + ( (int) ( ( this.ptパネルの基本座標[ n移動先の行の基本位置 ].X - this.ptパネルの基本座標[ n移動元の行の基本位置 ].X ) * ( ( (double) Math.Abs( this.n現在のスクロールカウンタ ) ) / 100.0 ) ) );
				int y = this.ptパネルの基本座標[ n移動元の行の基本位置 ].Y + ( (int) ( ( this.ptパネルの基本座標[ n移動先の行の基本位置 ].Y - this.ptパネルの基本座標[ n移動元の行の基本位置 ].Y ) * ( ( (double) Math.Abs( this.n現在のスクロールカウンタ ) ) / 100.0 ) ) );

				#region [ 現在の行の項目パネル枠を描画。]
				//-----------------
				switch( this.list項目リスト[ nItem ].eパネル種別 )
				{
					case CItemBase.Eパネル種別.通常:
                    case CItemBase.Eパネル種別.その他:
                        if ( CDTXMania.Tx.Config_ItemBox != null )
                            CDTXMania.Tx.Config_ItemBox.t2D描画( CDTXMania.app.Device, x, y );
						break;
				}
				//-----------------
				#endregion
				#region [ 現在の行の項目名を描画。]
				//-----------------
				if ( listMenu[ nItem ].txMenuItemRight != null )	// 自前のキャッシュに含まれているようなら、再レンダリングせずキャッシュを使用
				{
					listMenu[ nItem ].txMenuItemRight.t2D描画( CDTXMania.app.Device, x + 20, y + 12 );
				}
				else
				{
					Bitmap bmpItem = prvFont.DrawPrivateFont( this.list項目リスト[ nItem ].str項目名, Color.White, Color.Black );
					listMenu[ nItem ].txMenuItemRight = CDTXMania.tテクスチャの生成( bmpItem );
					//					ctItem.t2D描画( CDTXMania.app.Device, ( x + 0x12 ) * Scale.X, ( y + 12 ) * Scale.Y - 20 );
					//					CDTXMania.tテクスチャの解放( ref ctItem );
					CDTXMania.t安全にDisposeする( ref bmpItem );
				}
				//CDTXMania.stageコンフィグ.actFont.t文字列描画( x + 0x12, y + 12, this.list項目リスト[ nItem ].str項目名 );
				//-----------------
				#endregion
				#region [ 現在の行の項目の要素を描画。]
				//-----------------
				string strParam = null;
				bool b強調 = false;
				switch ( this.list項目リスト[ nItem ].e種別 )
				{
					case CItemBase.E種別.ONorOFFトグル:
						#region [ *** ]
						//-----------------
						//CDTXMania.stageコンフィグ.actFont.t文字列描画( x + 210, y + 12, ( (CItemToggle) this.list項目リスト[ nItem ] ).bON ? "ON" : "OFF" );
						strParam = ( (CItemToggle) this.list項目リスト[ nItem ] ).bON ? "ON" : "OFF";
						break;
					//-----------------
						#endregion

					case CItemBase.E種別.ONorOFFor不定スリーステート:
						#region [ *** ]
						//-----------------
						switch ( ( (CItemThreeState) this.list項目リスト[ nItem ] ).e現在の状態 )
						{
							case CItemThreeState.E状態.ON:
								strParam = "ON";
								break;

							case CItemThreeState.E状態.不定:
								strParam = "- -";
								break;

							default:
								strParam = "OFF";
								break;
						}
						//CDTXMania.stageコンフィグ.actFont.t文字列描画( x + 210, y + 12, "ON" );
						break;
					//-----------------
						#endregion

					case CItemBase.E種別.整数:		// #24789 2011.4.8 yyagi: add PlaySpeed supports (copied them from OPTION)
						#region [ *** ]
						//-----------------
						if ( this.list項目リスト[ nItem ] == this.iCommonPlaySpeed )
						{
							double d = ( (double) ( (CItemInteger) this.list項目リスト[ nItem ] ).n現在の値 ) / 20.0;
							//CDTXMania.stageコンフィグ.actFont.t文字列描画( x + 210, y + 12, d.ToString( "0.000" ), ( n行番号 == 0 ) && this.b要素値にフォーカス中 );
							strParam = d.ToString( "0.000" );
						}
						else if ( this.list項目リスト[ nItem ] == this.iDrumsScrollSpeed || this.list項目リスト[ nItem ] == this.iGuitarScrollSpeed || this.list項目リスト[ nItem ] == this.iBassScrollSpeed )
						{
							float f = ( ( (CItemInteger) this.list項目リスト[ nItem ] ).n現在の値 + 1 ) * 0.5f;
							//CDTXMania.stageコンフィグ.actFont.t文字列描画( x + 210, y + 12, f.ToString( "x0.0" ), ( n行番号 == 0 ) && this.b要素値にフォーカス中 );
							strParam = f.ToString( "x0.0" );
						}
						else
						{
							//CDTXMania.stageコンフィグ.actFont.t文字列描画( x + 210, y + 12, ( (CItemInteger) this.list項目リスト[ nItem ] ).n現在の値.ToString(), ( n行番号 == 0 ) && this.b要素値にフォーカス中 );
							strParam = ( (CItemInteger) this.list項目リスト[ nItem ] ).n現在の値.ToString();
						}
						b強調 = ( n行番号 == 0 ) && this.b要素値にフォーカス中;
						break;
					//-----------------
						#endregion

					case CItemBase.E種別.リスト:	// #28195 2012.5.2 yyagi: add Skin supports
						#region [ *** ]
						//-----------------
						{
							CItemList list = (CItemList) this.list項目リスト[ nItem ];
							//CDTXMania.stageコンフィグ.actFont.t文字列描画( x + 210, y + 12, list.list項目値[ list.n現在選択されている項目番号 ] );
							strParam = list.list項目値[ list.n現在選択されている項目番号 ];

							#region [ 必要な場合に、Skinのサンプルを生成・描画する。#28195 2012.5.2 yyagi ]
							if ( this.list項目リスト[ this.n現在の選択項目 ] == this.iSystemSkinSubfolder )
							{
								tGenerateSkinSample();		// 最初にSkinの選択肢にきたとき(Enterを押す前)に限り、サンプル生成が発生する。
								if ( txSkinSample1 != null )
								{
									txSkinSample1.t2D描画( CDTXMania.app.Device, 124, 409 );
								}
							}
							#endregion
							break;
						}
					//-----------------
						#endregion
				}
				if ( b強調 )
				{
					Bitmap bmpStr = b強調 ?
						prvFont.DrawPrivateFont( strParam, Color.Black, Color.White, Color.Yellow, Color.OrangeRed ) :
						prvFont.DrawPrivateFont( strParam, Color.Black, Color.White );
					CTexture txStr = CDTXMania.tテクスチャの生成( bmpStr, false );
					txStr.t2D描画( CDTXMania.app.Device, x + 400, y + 12 );
					CDTXMania.tテクスチャの解放( ref txStr );
					CDTXMania.t安全にDisposeする( ref bmpStr );
				}
				else
				{
					int nIndex = this.list項目リスト[ nItem ].GetIndex();
					if ( listMenu[ nItem ].nParam != nIndex || listMenu[ nItem ].txParam == null )
					{
						stMenuItemRight stm = listMenu[ nItem ];
						stm.nParam = nIndex;
						object o = this.list項目リスト[ nItem ].obj現在値();
						stm.strParam = ( o == null ) ? "" : o.ToString();

						Bitmap bmpStr =
							prvFont.DrawPrivateFont( strParam, Color.White, Color.Black );
						stm.txParam = CDTXMania.tテクスチャの生成( bmpStr, false );
						CDTXMania.t安全にDisposeする( ref bmpStr );

						listMenu[ nItem ] = stm;
					}
					listMenu[ nItem ].txParam.t2D描画( CDTXMania.app.Device, ( x + 400 ), y + 12 );
				}
				//-----------------
				#endregion
				
				nItem = this.t次の項目( nItem );
			}
			//-----------------
			#endregion
			
			#region [ 項目リストにフォーカスがあって、かつスクロールが停止しているなら、パネルの上下に▲印を描画する。]
			//-----------------
			if( this.b項目リスト側にフォーカスがある && ( this.n目標のスクロールカウンタ == 0 ) )
			{
				int x;
				int y_upper;
				int y_lower;
			
				// 位置決定。

				if( this.b要素値にフォーカス中 )
				{
					x = 552;	// 要素値の上下あたり。
					y_upper = 0x117 - this.ct三角矢印アニメ.n現在の値;
					y_lower = 0x17d + this.ct三角矢印アニメ.n現在の値;
				}
				else
				{
					x = 552;	// 項目名の上下あたり。
					y_upper = 0x129 - this.ct三角矢印アニメ.n現在の値;
					y_lower = 0x16b + this.ct三角矢印アニメ.n現在の値;
				}

				// 描画。
				
				if( CDTXMania.Tx.Config_Arrow != null )
				{
                    CDTXMania.Tx.Config_Arrow.t2D描画( CDTXMania.app.Device, x, y_upper, new Rectangle( 0, 0, 0x40, 0x18 ) );
                    CDTXMania.Tx.Config_Arrow.t2D描画( CDTXMania.app.Device, x, y_lower, new Rectangle( 0, 0x18, 0x40, 0x18 ) );
				}
			}
			//-----------------
			#endregion
			return 0;
		}
	

		// その他

		#region [ private ]
		//-----------------
		private enum Eメニュー種別
		{
			System,
			Drums,
			Guitar,
			Bass,
			KeyAssignSystem,		// #24609 2011.4.12 yyagi: 画面キャプチャキーのアサイン
			KeyAssignDrums,
			KeyAssignGuitar,
			KeyAssignBass,
			Unknown

		}

		private bool b項目リスト側にフォーカスがある;
		private bool b要素値にフォーカス中;
		private CCounter ct三角矢印アニメ;
		private Eメニュー種別 eメニュー種別;
		#region [ キーコンフィグ ]
		private CItemBase iKeyAssignSystemCapture;			// #24609
		private CItemBase iKeyAssignSystemReturnToMenu;		// #24609
		private CItemBase iKeyAssignBassB;
		private CItemBase iKeyAssignBassCancel;
		private CItemBase iKeyAssignBassDecide;
		private CItemBase iKeyAssignBassG;
		private CItemBase iKeyAssignBassPick;
		private CItemBase iKeyAssignBassR;
		private CItemBase iKeyAssignBassReturnToMenu;
		private CItemBase iKeyAssignBassWail;
		private CItemBase iKeyAssignDrumsBD;
		private CItemBase iKeyAssignDrumsCY;
		private CItemBase iKeyAssignDrumsFT;
		private CItemBase iKeyAssignDrumsHHC;
		private CItemBase iKeyAssignDrumsHHO;
		private CItemBase iKeyAssignDrumsHT;
		private CItemBase iKeyAssignDrumsLC;
		private CItemBase iKeyAssignDrumsLT;
		private CItemBase iKeyAssignDrumsRD;
		private CItemBase iKeyAssignDrumsReturnToMenu;
		private CItemBase iKeyAssignDrumsSD;
		private CItemBase iKeyAssignDrumsLP;	// #27029 2012.1.4 from
		private CItemBase iKeyAssignDrumsLBD;	// #27029 2012.1.4 from
		private CItemBase iKeyAssignGuitarB;
		private CItemBase iKeyAssignGuitarCancel;
		private CItemBase iKeyAssignGuitarDecide;
		private CItemBase iKeyAssignGuitarG;
		private CItemBase iKeyAssignGuitarPick;
		private CItemBase iKeyAssignGuitarR;
		private CItemBase iKeyAssignGuitarReturnToMenu;
		private CItemBase iKeyAssignGuitarWail;

		private CItemBase iKeyAssignTaikoLRed;
		private CItemBase iKeyAssignTaikoRRed;
		private CItemBase iKeyAssignTaikoLBlue;
		private CItemBase iKeyAssignTaikoRBlue;
		private CItemBase iKeyAssignTaikoLRed2P;
		private CItemBase iKeyAssignTaikoRRed2P;
		private CItemBase iKeyAssignTaikoLBlue2P;
		private CItemBase iKeyAssignTaikoRBlue2P;

		#endregion
		private CItemToggle iLogOutputLog;
		private CItemToggle iSystemAdjustWaves;
		private CItemToggle iSystemAudienceSound;
		private CItemInteger iSystemAutoChipVolume;
		private CItemToggle iSystemAVI;
		private CItemToggle iSystemBGA;
//		private CItemToggle iSystemGraph; #24074 2011.01.23 comment-out ikanick オプション(Drums)へ移行
		private CItemInteger iSystemBGAlpha;
		private CItemToggle iSystemBGMSound;
		private CItemInteger iSystemChipVolume;
		private CItemList iSystemCYGroup;
		private CItemToggle iSystemCymbalFree;
		private CItemList iSystemDamageLevel;
		private CItemToggle iSystemDebugInfo;
//		private CItemToggle iSystemDrums;
		private CItemToggle iSystemFillIn;
		private CItemList iSystemFTGroup;
		private CItemToggle iSystemFullscreen;
//		private CItemToggle iSystemGuitar;
		private CItemList iSystemHHGroup;
		private CItemList iSystemBDGroup;		// #27029 2012.1.4 from
		private CItemToggle iSystemHitSound;
		private CItemList iSystemHitSoundPriorityCY;
		private CItemList iSystemHitSoundPriorityFT;
		private CItemList iSystemHitSoundPriorityHH;
		private CItemInteger iSystemMinComboBass;
		private CItemInteger iSystemMinComboDrums;
		private CItemInteger iSystemMinComboGuitar;
		private CItemInteger iSystemPreviewImageWait;
		private CItemInteger iSystemPreviewSoundWait;
		private CItemToggle iSystemRandomFromSubBox;
		private CItemBase iSystemReturnToMenu;
		private CItemToggle iSystemSaveScore;
		private CItemToggle iSystemSoundMonitorBass;
		private CItemToggle iSystemSoundMonitorDrums;
		private CItemToggle iSystemSoundMonitorGuitar;
		private CItemToggle iSystemStageFailed;
		private CItemToggle iSystemStoicMode;
		private CItemToggle iSystemVSyncWait;
		private CItemList	iSystemShowLag;					// #25370 2011.6.3 yyagi
		private CItemToggle iSystemAutoResultCapture;		// #25399 2011.6.9 yyagi
		private CItemToggle iSystemBufferedInput;
		private CItemInteger iSystemRisky;					// #23559 2011.7.27 yyagi
		private CItemList iSystemSoundType;					// #24820 2013.1.3 yyagi
		private CItemInteger iSystemWASAPIBufferSizeMs;		// #24820 2013.1.15 yyagi
//		private CItemInteger iSystemASIOBufferSizeMs;		// #24820 2013.1.3 yyagi
		private CItemList	iSystemASIODevice;				// #24820 2013.1.17 yyagi

		private int iSystemSoundType_initial;
		private int iSystemWASAPIBufferSizeMs_initial;
//		private int iSystemASIOBufferSizeMs_initial;
		private int iSystemASIODevice_initial;
		private CItemToggle iSystemSoundTimerType;			// #33689 2014.6.17 yyagi
		private int iSystemSoundTimerType_initial;			// #33689 2014.6.17 yyagi

		private CItemToggle iSystemTimeStretch;				// #23664 2013.2.24 yyagi
		private CItemList iSystemJudgePosGuitar;			// #33891 2014.6.26 yyagi
		private CItemList iSystemJudgePosBass;				// #33891 2014.6.26 yyagi

		//private CItemList iDrumsJudgeDispPriority;	//
		//private CItemList iGuitarJudgeDispPriority;	//
		//private CItemList iBassJudgeDispPriority;		//
		private CItemList iSystemJudgeDispPriority;

		private List<CItemBase> list項目リスト;
		private long nスクロール用タイマ値;
		private int n現在のスクロールカウンタ;
		private int n目標のスクロールカウンタ;
        private Point[] ptパネルの基本座標 = new Point[] { new Point(0x25a, 4), new Point(0x25a, 0x4f), new Point(0x25a, 0x9a), new Point(0x25a, 0xe5), new Point(0x228, 0x130), new Point(0x25a, 0x17b), new Point(0x25a, 0x1c6), new Point(0x25a, 0x211), new Point(0x25a, 0x25c), new Point(0x25a, 0x2a7) };
		//private CTexture txその他項目行パネル;
		//private CTexture tx三角矢印;
		//private CTexture tx通常項目行パネル;

		private CPrivateFastFont prvFont;
		//private List<string> list項目リスト_str最終描画名;
		private struct stMenuItemRight
		{
			//	public string strMenuItem;
			public CTexture txMenuItemRight;
			public int nParam;
			public string strParam;
			public CTexture txParam;
		}
		private stMenuItemRight[] listMenu;

		private CTexture txSkinSample1;				// #28195 2012.5.2 yyagi
		private string[] skinSubFolders;			//
		private string[] skinNames;					//
		private string skinSubFolder_org;			//
		private int nSkinSampleIndex;				//
		private int nSkinIndex;						//

		private CItemBase iDrumsGoToKeyAssign;
		private CItemBase iGuitarGoToKeyAssign;
		private CItemBase iBassGoToKeyAssign;
		private CItemBase iSystemGoToKeyAssign;		// #24609

		private CItemList iSystemGRmode;

		//private CItemToggle iBassAutoPlay;
		private CItemThreeState iBassAutoPlayAll;			// #23886 2012.5.8 yyagi
		private CItemToggle iBassR;							//
		private CItemToggle iBassG;							//
		private CItemToggle iBassB;							//
		private CItemToggle iBassPick;						//
		private CItemToggle iBassW;							//
	
		//private CItemToggle iBassHidden;
		private CItemToggle iBassLeft;
		private CItemToggle iBassLight;
		private CItemList iBassPosition;
		private CItemList iBassRandom;
		private CItemBase iBassReturnToMenu;
		private CItemToggle iBassReverse;
		private CItemInteger iBassScrollSpeed;
		//private CItemToggle iBassSudden;
		private CItemList iCommonDark;
		private CItemInteger iCommonPlaySpeed;
//		private CItemBase iCommonReturnToMenu;

		private CItemThreeState iDrumsAutoPlayAll;
		private CItemToggle iDrumsBass;
		private CItemToggle iDrumsCymbalRide;
		private CItemToggle iDrumsFloorTom;
		//private CItemToggle iDrumsHidden;
		private CItemToggle iDrumsHighTom;
		private CItemToggle iDrumsHiHat;
		private CItemToggle iDrumsLeftCymbal;
		private CItemToggle iDrumsLowTom;
		private CItemList iDrumsPosition;
		private CItemBase iDrumsReturnToMenu;
		private CItemToggle iDrumsReverse;
		private CItemInteger iDrumsScrollSpeed;
		private CItemToggle iDrumsSnare;
		//private CItemToggle iDrumsSudden;
		private CItemToggle iDrumsTight;
		private CItemToggle iDrumsGraph;        // #24074 2011.01.23 add ikanick
        private CItemToggle iDrumsLeftPedal;
        private CItemToggle iDrumsLeftBassDrum;
        private CItemToggle iDrumsComboDisp;

        private CItemToggle iTaikoAutoPlay;
        private CItemToggle iTaikoAutoPlay2P;
        private CItemToggle iTaikoAutoRoll;
        private CItemToggle iTaikoBranchGuide;
        private CItemList iTaikoDefaultCourse; //2017.01.30 DD デフォルトでカーソルをあわせる難易度
        private CItemList iTaikoScoreMode;
        private CItemToggle iTaikoAutoSection;
        private CItemToggle iTaikoHispeedRandom;
        private CItemList iTaikoBranchAnime;
        private CItemToggle iTaikoChara;
        private CItemToggle iTaikoNoInfo;
		private CItemList iTaikoRandom;
        private CItemList iTaikoStealth;
        private CItemList iTaikoGameMode;
        private CItemToggle iTaikoJust;
        private CItemToggle iTaikoJudgeCountDisp;
        private CItemToggle iTaikoBigNotesJudge;
        private CItemInteger iTaikoPlayerCount;

		//private CItemToggle iGuitarAutoPlay;
		private CItemThreeState iGuitarAutoPlayAll;			// #23886 2012.5.8 yyagi
		private CItemToggle iGuitarR;						//
		private CItemToggle iGuitarG;						//
		private CItemToggle iGuitarB;						//
		private CItemToggle iGuitarPick;					//
		private CItemToggle iGuitarW;						//

		//private CItemToggle iGuitarHidden;
		private CItemToggle iGuitarLeft;
		private CItemToggle iGuitarLight;
		private CItemList iGuitarPosition;
		private CItemList iGuitarRandom;
		private CItemBase iGuitarReturnToMenu;
		private CItemToggle iGuitarReverse;
		private CItemInteger iGuitarScrollSpeed;
		//private CItemToggle iGuitarSudden;
		private CItemInteger iDrumsInputAdjustTimeMs;		// #23580 2011.1.3 yyagi
		private CItemInteger iGuitarInputAdjustTimeMs;		//
		private CItemInteger iBassInputAdjustTimeMs;		//
		private CItemList iSystemSkinSubfolder;				// #28195 2012.5.2 yyagi
		private CItemToggle iSystemUseBoxDefSkin;			// #28195 2012.5.6 yyagi
		private CItemList iDrumsSudHid;						// #32072 2013.9.20 yyagi
		private CItemList iGuitarSudHid;					// #32072 2013.9.20 yyagi
		private CItemList iBassSudHid;						// #32072 2013.9.20 yyagi
		private CItemBase iSystemReloadDTX;					// #32081 2013.10.21 yyagi
		//private CItemInteger iSystemMasterVolume;			// #33700 2014.4.26 yyagi

		private int t前の項目( int nItem )
		{
			if( --nItem < 0 )
			{
				nItem = this.list項目リスト.Count - 1;
			}
			return nItem;
		}
		private int t次の項目( int nItem )
		{
			if( ++nItem >= this.list項目リスト.Count )
			{
				nItem = 0;
			}
			return nItem;
		}
		private void t全部のドラムパッドのAutoを切り替える( bool bAutoON )
		{
			this.iDrumsLeftCymbal.bON = this.iDrumsHiHat.bON = this.iDrumsSnare.bON = this.iDrumsBass.bON = this.iDrumsHighTom.bON = this.iDrumsLowTom.bON = this.iDrumsFloorTom.bON = this.iDrumsCymbalRide.bON = bAutoON;
		}
		private void t全部のギターパッドのAutoを切り替える( bool bAutoON )
		{
			this.iGuitarR.bON = this.iGuitarG.bON = this.iGuitarB.bON = this.iGuitarPick.bON = this.iGuitarW.bON = bAutoON;
		}
		private void t全部のベースパッドのAutoを切り替える( bool bAutoON )
		{
			this.iBassR.bON = this.iBassG.bON = this.iBassB.bON = this.iBassPick.bON = this.iBassW.bON = bAutoON;
		}
		private void tConfigIniへ記録する()
		{
			switch( this.eメニュー種別 )
			{
				case Eメニュー種別.System:
					this.tConfigIniへ記録する_System();
					this.tConfigIniへ記録する_KeyAssignSystem();
					return;

				case Eメニュー種別.Drums:
					this.tConfigIniへ記録する_Drums();
					this.tConfigIniへ記録する_KeyAssignDrums();
					return;

				case Eメニュー種別.Guitar:
					this.tConfigIniへ記録する_Guitar();
					this.tConfigIniへ記録する_KeyAssignGuitar();
					return;

				case Eメニュー種別.Bass:
					this.tConfigIniへ記録する_Bass();
					this.tConfigIniへ記録する_KeyAssignBass();
					return;
			}
		}
		private void tConfigIniへ記録する_KeyAssignBass()
		{
		}
		private void tConfigIniへ記録する_KeyAssignDrums()
		{
		}
		private void tConfigIniへ記録する_KeyAssignGuitar()
		{
		}
		private void tConfigIniへ記録する_KeyAssignSystem()
		{
		}
		private void tConfigIniへ記録する_System()
		{
            //CDTXMania.ConfigIni.eDark = (Eダークモード) this.iCommonDark.n現在選択されている項目番号;
			//CDTXMania.ConfigIni.n演奏速度 = this.iCommonPlaySpeed.n現在の値;

            //CDTXMania.ConfigIni.bGuitar有効 = ( ( ( this.iSystemGRmode.n現在選択されている項目番号 + 1 ) / 2 ) == 1 );
                //this.iSystemGuitar.bON;
            //CDTXMania.ConfigIni.bDrums有効 = ( ( ( this.iSystemGRmode.n現在選択されている項目番号 + 1 ) % 2 ) == 1 );
                //this.iSystemDrums.bON;

			CDTXMania.ConfigIni.b全画面モード = this.iSystemFullscreen.bON;
			//CDTXMania.ConfigIni.bSTAGEFAILED有効 = this.iSystemStageFailed.bON;
			CDTXMania.ConfigIni.bランダムセレクトで子BOXを検索対象とする = this.iSystemRandomFromSubBox.bON;

			//CDTXMania.ConfigIni.bWave再生位置自動調整機能有効 = this.iSystemAdjustWaves.bON;
			CDTXMania.ConfigIni.b垂直帰線待ちを行う = this.iSystemVSyncWait.bON;
			//CDTXMania.ConfigIni.bバッファ入力を行う = this.iSystemBufferedInput.bON;
			CDTXMania.ConfigIni.bAVI有効 = this.iSystemAVI.bON;
			CDTXMania.ConfigIni.bBGA有効 = this.iSystemBGA.bON;
//			CDTXMania.ConfigIni.bGraph有効 = this.iSystemGraph.bON;#24074 2011.01.23 comment-out ikanick オプション(Drums)へ移行
			//CDTXMania.ConfigIni.n曲が選択されてからプレビュー音が鳴るまでのウェイトms = this.iSystemPreviewSoundWait.n現在の値;
			//CDTXMania.ConfigIni.n曲が選択されてからプレビュー画像が表示開始されるまでのウェイトms = this.iSystemPreviewImageWait.n現在の値;
			CDTXMania.ConfigIni.b演奏情報を表示する = this.iSystemDebugInfo.bON;
			CDTXMania.ConfigIni.n背景の透過度 = this.iSystemBGAlpha.n現在の値;
			CDTXMania.ConfigIni.bBGM音を発声する = this.iSystemBGMSound.bON;
			//CDTXMania.ConfigIni.b歓声を発声する = this.iSystemAudienceSound.bON;
			//CDTXMania.ConfigIni.eダメージレベル = (Eダメージレベル) this.iSystemDamageLevel.n現在選択されている項目番号;
			CDTXMania.ConfigIni.bScoreIniを出力する = this.iSystemSaveScore.bON;

			CDTXMania.ConfigIni.bログ出力 = this.iLogOutputLog.bON;
			//CDTXMania.ConfigIni.n手動再生音量 = this.iSystemChipVolume.n現在の値;
			//CDTXMania.ConfigIni.n自動再生音量 = this.iSystemAutoChipVolume.n現在の値;
			//CDTXMania.ConfigIni.bストイックモード = this.iSystemStoicMode.bON;

			//CDTXMania.ConfigIni.nShowLagType = this.iSystemShowLag.n現在選択されている項目番号;				// #25370 2011.6.3 yyagi
			//CDTXMania.ConfigIni.bIsAutoResultCapture = this.iSystemAutoResultCapture.bON;					// #25399 2011.6.9 yyagi

			//CDTXMania.ConfigIni.nRisky = this.iSystemRisky.n現在の値;										// #23559 2011.7.27 yyagi

			CDTXMania.ConfigIni.strSystemSkinSubfolderFullName = skinSubFolders[ nSkinIndex ];				// #28195 2012.5.2 yyagi
			CDTXMania.Skin.SetCurrentSkinSubfolderFullName( CDTXMania.ConfigIni.strSystemSkinSubfolderFullName, true );
			//CDTXMania.ConfigIni.bUseBoxDefSkin = this.iSystemUseBoxDefSkin.bON;								// #28195 2012.5.6 yyagi

			CDTXMania.ConfigIni.nSoundDeviceType = this.iSystemSoundType.n現在選択されている項目番号;		// #24820 2013.1.3 yyagi
			CDTXMania.ConfigIni.nWASAPIBufferSizeMs = this.iSystemWASAPIBufferSizeMs.n現在の値;				// #24820 2013.1.15 yyagi
//			CDTXMania.ConfigIni.nASIOBufferSizeMs = this.iSystemASIOBufferSizeMs.n現在の値;					// #24820 2013.1.3 yyagi
			CDTXMania.ConfigIni.nASIODevice = this.iSystemASIODevice.n現在選択されている項目番号;			// #24820 2013.1.17 yyagi
			CDTXMania.ConfigIni.bUseOSTimer = this.iSystemSoundTimerType.bON;								// #33689 2014.6.17 yyagi

			CDTXMania.ConfigIni.bTimeStretch = this.iSystemTimeStretch.bON;									// #23664 2013.2.24 yyagi
//Trace.TraceInformation( "saved" );
//Trace.TraceInformation( "Skin現在Current : " + CDTXMania.Skin.GetCurrentSkinSubfolderFullName(true) );
//Trace.TraceInformation( "Skin現在System  : " + CSkin.strSystemSkinSubfolderFullName );
//Trace.TraceInformation( "Skin現在BoxDef  : " + CSkin.strBoxDefSkinSubfolderFullName );
			//CDTXMania.ConfigIni.nMasterVolume = this.iSystemMasterVolume.n現在の値;							// #33700 2014.4.26 yyagi
			//CDTXMania.ConfigIni.e判定表示優先度 = (E判定表示優先度) this.iSystemJudgeDispPriority.n現在選択されている項目番号;
            //CDTXMania.ConfigIni.bAutoSection = this.iTaikoAutoSection.bON;
            CDTXMania.ConfigIni.nPlayerCount = this.iTaikoPlayerCount.n現在の値;
		}
		private void tConfigIniへ記録する_Bass()
		{
			//CDTXMania.ConfigIni.bAutoPlay.Bass = this.iBassAutoPlay.bON;
            //CDTXMania.ConfigIni.bAutoPlay.BsR = this.iBassR.bON;
            //CDTXMania.ConfigIni.bAutoPlay.BsG = this.iBassG.bON;
            //CDTXMania.ConfigIni.bAutoPlay.BsB = this.iBassB.bON;
            //CDTXMania.ConfigIni.bAutoPlay.BsPick = this.iBassPick.bON;
            //CDTXMania.ConfigIni.bAutoPlay.BsW = this.iBassW.bON;
            //CDTXMania.ConfigIni.n譜面スクロール速度.Bass = this.iBassScrollSpeed.n現在の値;
            //                                    // "Sudden" || "Sud+Hid"
            //CDTXMania.ConfigIni.bSudden.Bass = ( this.iBassSudHid.n現在選択されている項目番号 == 1 || this.iBassSudHid.n現在選択されている項目番号 == 3 ) ? true : false;
            //                                    // "Hidden" || "Sud+Hid"
            //CDTXMania.ConfigIni.bHidden.Bass = ( this.iBassSudHid.n現在選択されている項目番号 == 2 || this.iBassSudHid.n現在選択されている項目番号 == 3 ) ? true : false;
            //if      ( this.iBassSudHid.n現在選択されている項目番号 == 4 ) CDTXMania.ConfigIni.eInvisible.Bass = EInvisible.SEMI;	// "S-Invisible"
            //else if ( this.iBassSudHid.n現在選択されている項目番号 == 5 ) CDTXMania.ConfigIni.eInvisible.Bass = EInvisible.FULL;	// "F-Invisible"
            //else                                                          CDTXMania.ConfigIni.eInvisible.Bass = EInvisible.OFF;
            //CDTXMania.ConfigIni.bReverse.Bass = this.iBassReverse.bON;
            //CDTXMania.ConfigIni.判定文字表示位置.Bass = (E判定文字表示位置) this.iBassPosition.n現在選択されている項目番号;
            //CDTXMania.ConfigIni.eRandom.Bass = (Eランダムモード) this.iBassRandom.n現在選択されている項目番号;
            //CDTXMania.ConfigIni.bLight.Bass = this.iBassLight.bON;
            //CDTXMania.ConfigIni.bLeft.Bass = this.iBassLeft.bON;
            //CDTXMania.ConfigIni.nInputAdjustTimeMs.Bass = this.iBassInputAdjustTimeMs.n現在の値;		// #23580 2011.1.3 yyagi

            //CDTXMania.ConfigIni.b演奏音を強調する.Bass = this.iSystemSoundMonitorBass.bON;
            //CDTXMania.ConfigIni.n表示可能な最小コンボ数.Bass = this.iSystemMinComboBass.n現在の値;
            //CDTXMania.ConfigIni.e判定位置.Bass = (E判定位置) this.iSystemJudgePosBass.n現在選択されている項目番号;					// #33891 2014.6.26 yyagi
			//CDTXMania.ConfigIni.e判定表示優先度.Bass = (E判定表示優先度) this.iBassJudgeDispPriority.n現在選択されている項目番号;
		}
		private void tConfigIniへ記録する_Drums()
		{
            //CDTXMania.ConfigIni.bAutoPlay.LC = this.iDrumsLeftCymbal.bON;
            //CDTXMania.ConfigIni.bAutoPlay.HH = this.iDrumsHiHat.bON;
            //CDTXMania.ConfigIni.bAutoPlay.SD = this.iDrumsSnare.bON;
            //CDTXMania.ConfigIni.bAutoPlay.BD = this.iDrumsBass.bON;
            //CDTXMania.ConfigIni.bAutoPlay.HT = this.iDrumsHighTom.bON;
            //CDTXMania.ConfigIni.bAutoPlay.LT = this.iDrumsLowTom.bON;
            //CDTXMania.ConfigIni.bAutoPlay.FT = this.iDrumsFloorTom.bON;
            //CDTXMania.ConfigIni.bAutoPlay.CY = this.iDrumsCymbalRide.bON;
            //CDTXMania.ConfigIni.bAutoPlay.LP = this.iDrumsLeftPedal.bON;
            //CDTXMania.ConfigIni.bAutoPlay.LBD = this.iDrumsLeftBassDrum.bON;
            CDTXMania.ConfigIni.b太鼓パートAutoPlay = this.iTaikoAutoPlay.bON;
            CDTXMania.ConfigIni.b太鼓パートAutoPlay2P = this.iTaikoAutoPlay2P.bON;
            CDTXMania.ConfigIni.bAuto先生の連打 = this.iTaikoAutoRoll.bON;

			//CDTXMania.ConfigIni.n譜面スクロール速度.Drums = this.iDrumsScrollSpeed.n現在の値;
            //CDTXMania.ConfigIni.bドラムコンボ表示 = this.iDrumsComboDisp.bON;
												// "Sudden" || "Sud+Hid"
            //CDTXMania.ConfigIni.bSudden.Drums = ( this.iDrumsSudHid.n現在選択されている項目番号 == 1 || this.iDrumsSudHid.n現在選択されている項目番号 == 3 ) ? true : false;
												// "Hidden" || "Sud+Hid"
            //CDTXMania.ConfigIni.bHidden.Drums = ( this.iDrumsSudHid.n現在選択されている項目番号 == 2 || this.iDrumsSudHid.n現在選択されている項目番号 == 3 ) ? true : false;
            //if      ( this.iDrumsSudHid.n現在選択されている項目番号 == 4 ) CDTXMania.ConfigIni.eInvisible.Drums = EInvisible.SEMI;	// "S-Invisible"
            //else if ( this.iDrumsSudHid.n現在選択されている項目番号 == 5 ) CDTXMania.ConfigIni.eInvisible.Drums = EInvisible.FULL;	// "F-Invisible"
            //else                                                           CDTXMania.ConfigIni.eInvisible.Drums = EInvisible.OFF;
            //CDTXMania.ConfigIni.bReverse.Drums = this.iDrumsReverse.bON;
            //CDTXMania.ConfigIni.判定文字表示位置.Drums = (E判定文字表示位置) this.iDrumsPosition.n現在選択されている項目番号;
			//CDTXMania.ConfigIni.bTight = this.iDrumsTight.bON;
			//CDTXMania.ConfigIni.nInputAdjustTimeMs.Drums = this.iDrumsInputAdjustTimeMs.n現在の値;		// #23580 2011.1.3 yyagi
            //CDTXMania.ConfigIni.bGraph.Drums = this.iDrumsGraph.bON;// #24074 2011.01.23 add ikanick

            //CDTXMania.ConfigIni.eHHGroup = (EHHGroup) this.iSystemHHGroup.n現在選択されている項目番号;
            //CDTXMania.ConfigIni.eFTGroup = (EFTGroup) this.iSystemFTGroup.n現在選択されている項目番号;
            //CDTXMania.ConfigIni.eCYGroup = (ECYGroup) this.iSystemCYGroup.n現在選択されている項目番号;
            //CDTXMania.ConfigIni.eBDGroup = (EBDGroup) this.iSystemBDGroup.n現在選択されている項目番号;
            //CDTXMania.ConfigIni.eHitSoundPriorityHH = (E打ち分け時の再生の優先順位) this.iSystemHitSoundPriorityHH.n現在選択されている項目番号;
            //CDTXMania.ConfigIni.eHitSoundPriorityFT = (E打ち分け時の再生の優先順位) this.iSystemHitSoundPriorityFT.n現在選択されている項目番号;
            //CDTXMania.ConfigIni.eHitSoundPriorityCY = (E打ち分け時の再生の優先順位) this.iSystemHitSoundPriorityCY.n現在選択されている項目番号;
            //CDTXMania.ConfigIni.bフィルイン有効 = this.iSystemFillIn.bON;
            //CDTXMania.ConfigIni.b演奏音を強調する.Drums = this.iSystemSoundMonitorDrums.bON;
            //CDTXMania.ConfigIni.bドラム打音を発声する = this.iSystemHitSound.bON;
			//CDTXMania.ConfigIni.n表示可能な最小コンボ数.Drums = this.iSystemMinComboDrums.n現在の値;
            //CDTXMania.ConfigIni.bシンバルフリー = this.iSystemCymbalFree.bON;

            //CDTXMania.ConfigIni.eDark = (Eダークモード)this.iCommonDark.n現在選択されている項目番号;
			CDTXMania.ConfigIni.nRisky = this.iSystemRisky.n現在の値;						// #23559 2911.7.27 yyagi
			//CDTXMania.ConfigIni.e判定表示優先度.Drums = (E判定表示優先度) this.iDrumsJudgeDispPriority.n現在選択されている項目番号;

            CDTXMania.ConfigIni.bBranchGuide = this.iTaikoBranchGuide.bON;
            CDTXMania.ConfigIni.nDefaultCourse = this.iTaikoDefaultCourse.n現在選択されている項目番号;
            CDTXMania.ConfigIni.nScoreMode = this.iTaikoScoreMode.n現在選択されている項目番号;
            CDTXMania.ConfigIni.nBranchAnime = this.iTaikoBranchAnime.n現在選択されている項目番号;
            //CDTXMania.ConfigIni.bHispeedRandom = this.iTaikoHispeedRandom.bON;
            CDTXMania.ConfigIni.bChara = this.iTaikoChara.bON;
            //CDTXMania.ConfigIni.bNoInfo = this.iTaikoNoInfo.bON;
            //CDTXMania.ConfigIni.eRandom.Taiko = (Eランダムモード)this.iTaikoRandom.n現在選択されている項目番号;
            //CDTXMania.ConfigIni.eSTEALTH = (Eステルスモード)this.iTaikoStealth.n現在選択されている項目番号;
            //CDTXMania.ConfigIni.eGameMode = (EGame)this.iTaikoGameMode.n現在選択されている項目番号;
            CDTXMania.ConfigIni.bJust = this.iTaikoJust.bON;
            CDTXMania.ConfigIni.bJudgeCountDisplay = this.iTaikoJudgeCountDisp.bON;
            CDTXMania.ConfigIni.b大音符判定 = this.iTaikoBigNotesJudge.bON;
		}
		private void tConfigIniへ記録する_Guitar()
		{
			//CDTXMania.ConfigIni.bAutoPlay.Guitar = this.iGuitarAutoPlay.bON;
            //CDTXMania.ConfigIni.bAutoPlay.GtR = this.iGuitarR.bON;
            //CDTXMania.ConfigIni.bAutoPlay.GtG = this.iGuitarG.bON;
            //CDTXMania.ConfigIni.bAutoPlay.GtB = this.iGuitarB.bON;
            //CDTXMania.ConfigIni.bAutoPlay.GtPick = this.iGuitarPick.bON;
            //CDTXMania.ConfigIni.bAutoPlay.GtW = this.iGuitarW.bON;
            //CDTXMania.ConfigIni.n譜面スクロール速度.Guitar = this.iGuitarScrollSpeed.n現在の値;
												// "Sudden" || "Sud+Hid"
			//CDTXMania.ConfigIni.bSudden.Guitar = ( this.iGuitarSudHid.n現在選択されている項目番号 == 1 || this.iGuitarSudHid.n現在選択されている項目番号 == 3 ) ? true : false;
												// "Hidden" || "Sud+Hid"
			//CDTXMania.ConfigIni.bHidden.Guitar = ( this.iGuitarSudHid.n現在選択されている項目番号 == 2 || this.iGuitarSudHid.n現在選択されている項目番号 == 3 ) ? true : false;
            //if      ( this.iGuitarSudHid.n現在選択されている項目番号 == 4 ) CDTXMania.ConfigIni.eInvisible.Guitar = EInvisible.SEMI;	// "S-Invisible"
            //else if ( this.iGuitarSudHid.n現在選択されている項目番号 == 5 ) CDTXMania.ConfigIni.eInvisible.Guitar = EInvisible.FULL;	// "F-Invisible"
            //else                                                            CDTXMania.ConfigIni.eInvisible.Guitar = EInvisible.OFF;
            //CDTXMania.ConfigIni.bReverse.Guitar = this.iGuitarReverse.bON;
            //CDTXMania.ConfigIni.判定文字表示位置.Guitar = (E判定文字表示位置) this.iGuitarPosition.n現在選択されている項目番号;
            //CDTXMania.ConfigIni.eRandom.Guitar = (Eランダムモード) this.iGuitarRandom.n現在選択されている項目番号;
            //CDTXMania.ConfigIni.bLight.Guitar = this.iGuitarLight.bON;
            //CDTXMania.ConfigIni.bLeft.Guitar = this.iGuitarLeft.bON;
            //CDTXMania.ConfigIni.nInputAdjustTimeMs.Guitar = this.iGuitarInputAdjustTimeMs.n現在の値;	// #23580 2011.1.3 yyagi

            //CDTXMania.ConfigIni.n表示可能な最小コンボ数.Guitar = this.iSystemMinComboGuitar.n現在の値;
            //CDTXMania.ConfigIni.b演奏音を強調する.Guitar = this.iSystemSoundMonitorGuitar.bON;
            //CDTXMania.ConfigIni.e判定位置.Guitar = (E判定位置) this.iSystemJudgePosGuitar.n現在選択されている項目番号;					// #33891 2014.6.26 yyagi
			//CDTXMania.ConfigIni.e判定表示優先度.Guitar = (E判定表示優先度) this.iGuitarJudgeDispPriority.n現在選択されている項目番号;
		}
		//-----------------
		#endregion
	}
}
