using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using FDK;

namespace DTXMania
{
	// グローバル定数

	public enum Eシステムサウンド
	{
		BGMオプション画面 = 0,
		BGMコンフィグ画面,
		BGM起動画面,
		BGM選曲画面,
		SOUNDステージ失敗音,
		SOUNDカーソル移動音,
		SOUNDゲーム開始音,
		SOUNDゲーム終了音,
		SOUNDステージクリア音,
		SOUNDタイトル音,
		SOUNDフルコンボ音,
		SOUND歓声音,
		SOUND曲読込開始音,
		SOUND決定音,
		SOUND取消音,
		SOUND変更音,
        //SOUND赤,
        //SOUND青,
        SOUND風船,
        SOUND曲決定音,
        SOUND成績発表,
		Count				// システムサウンド総数の計算用
    }

	internal class CSkin : IDisposable
	{
		// クラス

		public class Cシステムサウンド : IDisposable
		{
			// static フィールド

			public static CSkin.Cシステムサウンド r最後に再生した排他システムサウンド;

			// フィールド、プロパティ

			public bool bCompact対象;
			public bool bループ;
			public bool b読み込み未試行;
			public bool b読み込み成功;
			public bool b排他;
			public string strファイル名 = "";
			public bool b再生中
			{
				get
				{
					if( this.rSound[ 1 - this.n次に鳴るサウンド番号 ] == null )
						return false;

					return this.rSound[ 1 - this.n次に鳴るサウンド番号 ].b再生中;
				}
			}
			public int n位置_現在のサウンド
			{
				get
				{
					CSound sound = this.rSound[ 1 - this.n次に鳴るサウンド番号 ];
					if( sound == null )
						return 0;

					return sound.n位置;
				}
				set
				{
					CSound sound = this.rSound[ 1 - this.n次に鳴るサウンド番号 ];
					if( sound != null )
						sound.n位置 = value;
				}
			}
			public int n位置_次に鳴るサウンド
			{
				get
				{
					CSound sound = this.rSound[ this.n次に鳴るサウンド番号 ];
					if( sound == null )
						return 0;

					return sound.n位置;
				}
				set
				{
					CSound sound = this.rSound[ this.n次に鳴るサウンド番号 ];
					if( sound != null )
						sound.n位置 = value;
				}
			}
			public int n音量_現在のサウンド
			{
				get
				{
					CSound sound = this.rSound[ 1 - this.n次に鳴るサウンド番号 ];
					if( sound == null )
						return 0;

					return sound.n音量;
				}
				set
				{
					CSound sound = this.rSound[ 1 - this.n次に鳴るサウンド番号 ];
					if( sound != null )
						sound.n音量 = value;
				}
			}
			public int n音量_次に鳴るサウンド
			{
				get
				{
					CSound sound = this.rSound[ this.n次に鳴るサウンド番号 ];
					if( sound == null )
					{
						return 0;
					}
					return sound.n音量;
				}
				set
				{
					CSound sound = this.rSound[ this.n次に鳴るサウンド番号 ];
					if( sound != null )
					{
						sound.n音量 = value;
					}
				}
			}
			public int n長さ_現在のサウンド
			{
				get
				{
					CSound sound = this.rSound[ 1 - this.n次に鳴るサウンド番号 ];
					if( sound == null )
					{
						return 0;
					}
					return sound.n総演奏時間ms;
				}
			}
			public int n長さ_次に鳴るサウンド
			{
				get
				{
					CSound sound = this.rSound[ this.n次に鳴るサウンド番号 ];
					if( sound == null )
					{
						return 0;
					}
					return sound.n総演奏時間ms;
				}
			}


			/// <summary>
			/// コンストラクタ
			/// </summary>
			/// <param name="strファイル名"></param>
			/// <param name="bループ"></param>
			/// <param name="b排他"></param>
			/// <param name="bCompact対象"></param>
			public Cシステムサウンド(string strファイル名, bool bループ, bool b排他, bool bCompact対象)
			{
				this.strファイル名 = strファイル名;
				this.bループ = bループ;
				this.b排他 = b排他;
				this.bCompact対象 = bCompact対象;
				this.b読み込み未試行 = true;
			}
			public Cシステムサウンド()
			{
				this.b読み込み未試行 = true;
			}
			

			// メソッド

			public void t読み込み()
			{
				this.b読み込み未試行 = false;
				this.b読み込み成功 = false;
				if( string.IsNullOrEmpty( this.strファイル名 ) )
					throw new InvalidOperationException( "ファイル名が無効です。" );

				if( !File.Exists( CSkin.Path( this.strファイル名 ) ) )
				{
					throw new FileNotFoundException( this.strファイル名 );
				}
////				for( int i = 0; i < 2; i++ )		// #27790 2012.3.10 yyagi 2回読み出しを、1回読みだし＋1回メモリコピーに変更
////				{
//                    try
//                    {
//                        this.rSound[ 0 ] = CDTXMania.Sound管理.tサウンドを生成する( CSkin.Path( this.strファイル名 ) );
//                    }
//                    catch
//                    {
//                        this.rSound[ 0 ] = null;
//                        throw;
//                    }
//                    if ( this.rSound[ 0 ] == null )	// #28243 2012.5.3 yyagi "this.rSound[ 0 ].bストリーム再生する"時もCloneするようにし、rSound[1]がnullにならないよう修正→rSound[1]の再生正常化
//                    {
//                        this.rSound[ 1 ] = null;
//                    }
//                    else
//                    {
//                        this.rSound[ 1 ] = ( CSound ) this.rSound[ 0 ].Clone();	// #27790 2012.3.10 yyagi add: to accelerate loading chip sounds
//                        CDTXMania.Sound管理.tサウンドを登録する( this.rSound[ 1 ] );	// #28243 2012.5.3 yyagi add (登録漏れによりストリーム再生処理が発生していなかった)
//                    }

////				}

				for ( int i = 0; i < 2; i++ )		// 一旦Cloneを止めてASIO対応に専念
				{
					try
					{
						this.rSound[ i ] = CDTXMania.Sound管理.tサウンドを生成する( CSkin.Path( this.strファイル名 ) );
					}
					catch
					{
						this.rSound[ i ] = null;
						throw;
					}
				}
				this.b読み込み成功 = true;
			}
			public void t再生する()
			{
				if ( this.b読み込み未試行 )
				{
					try
					{
						t読み込み();
					}
					catch
					{
						this.b読み込み未試行 = false;
					}
				}
				if( this.b排他 )
				{
					if( r最後に再生した排他システムサウンド != null )
						r最後に再生した排他システムサウンド.t停止する();

					r最後に再生した排他システムサウンド = this;
				}
				CSound sound = this.rSound[ this.n次に鳴るサウンド番号 ];
				if( sound != null )
					sound.t再生を開始する( this.bループ );

				this.n次に鳴るサウンド番号 = 1 - this.n次に鳴るサウンド番号;
			}
			public void t停止する()
			{
				if( this.rSound[ 0 ] != null )
					this.rSound[ 0 ].t再生を停止する();

				if( this.rSound[ 1 ] != null )
					this.rSound[ 1 ].t再生を停止する();

				if( r最後に再生した排他システムサウンド == this )
					r最後に再生した排他システムサウンド = null;
			}

			public void tRemoveMixer()
			{
				if ( CDTXMania.Sound管理.GetCurrentSoundDeviceType() != "DirectShow" )
				{
					for ( int i = 0; i < 2; i++ )
					{
						if ( this.rSound[ i ] != null )
						{
							CDTXMania.Sound管理.RemoveMixer( this.rSound[ i ] );
						}
					}
				}
			}

			#region [ IDisposable 実装 ]
			//-----------------
			public void Dispose()
			{
				if( !this.bDisposed済み )
				{
					for( int i = 0; i < 2; i++ )
					{
						if( this.rSound[ i ] != null )
						{
							CDTXMania.Sound管理.tサウンドを破棄する( this.rSound[ i ] );
							this.rSound[ i ] = null;
						}
					}
					this.b読み込み成功 = false;
					this.bDisposed済み = true;
				}
			}
			//-----------------
			#endregion

			#region [ private ]
			//-----------------
			private bool bDisposed済み;
			private int n次に鳴るサウンド番号;
			private CSound[] rSound = new CSound[ 2 ];
			//-----------------
			#endregion
		}

	
		// プロパティ

		public Cシステムサウンド bgmオプション画面 = null;
		public Cシステムサウンド bgmコンフィグ画面 = null;
		public Cシステムサウンド bgm起動画面 = null;
		public Cシステムサウンド bgm選曲画面 = null;
		public Cシステムサウンド soundSTAGEFAILED音 = null;
		public Cシステムサウンド soundカーソル移動音 = null;
		public Cシステムサウンド soundゲーム開始音 = null;
		public Cシステムサウンド soundゲーム終了音 = null;
		public Cシステムサウンド soundステージクリア音 = null;
		public Cシステムサウンド soundタイトル音 = null;
		public Cシステムサウンド soundフルコンボ音 = null;
		public Cシステムサウンド sound歓声音 = null;
		public Cシステムサウンド sound曲読込開始音 = null;
		public Cシステムサウンド sound決定音 = null;
		public Cシステムサウンド sound取消音 = null;
		public Cシステムサウンド sound変更音 = null;
        //add
        public Cシステムサウンド bgmリザルト = null;
        public Cシステムサウンド bgmリザルトループ = null;
        public Cシステムサウンド sound曲決定音 = null;
        public Cシステムサウンド sound成績発表 = null;

        //public Cシステムサウンド soundRed = null;
        //public Cシステムサウンド soundBlue = null;
        public Cシステムサウンド soundBalloon = null;


        public readonly int nシステムサウンド数 = (int)Eシステムサウンド.Count;
		public Cシステムサウンド this[ Eシステムサウンド sound ]
		{
			get
			{
				switch( sound )
				{
					case Eシステムサウンド.SOUNDカーソル移動音:
						return this.soundカーソル移動音;

					case Eシステムサウンド.SOUND決定音:
						return this.sound決定音;

					case Eシステムサウンド.SOUND変更音:
						return this.sound変更音;

					case Eシステムサウンド.SOUND取消音:
						return this.sound取消音;

					case Eシステムサウンド.SOUND歓声音:
						return this.sound歓声音;

					case Eシステムサウンド.SOUNDステージ失敗音:
						return this.soundSTAGEFAILED音;

					case Eシステムサウンド.SOUNDゲーム開始音:
						return this.soundゲーム開始音;

					case Eシステムサウンド.SOUNDゲーム終了音:
						return this.soundゲーム終了音;

					case Eシステムサウンド.SOUNDステージクリア音:
						return this.soundステージクリア音;

					case Eシステムサウンド.SOUNDフルコンボ音:
						return this.soundフルコンボ音;

					case Eシステムサウンド.SOUND曲読込開始音:
						return this.sound曲読込開始音;

					case Eシステムサウンド.SOUNDタイトル音:
						return this.soundタイトル音;

					case Eシステムサウンド.BGM起動画面:
						return this.bgm起動画面;

					case Eシステムサウンド.BGMオプション画面:
						return this.bgmオプション画面;

					case Eシステムサウンド.BGMコンフィグ画面:
						return this.bgmコンフィグ画面;

					case Eシステムサウンド.BGM選曲画面:
						return this.bgm選曲画面;

                    //case Eシステムサウンド.SOUND赤:
                    //    return this.soundRed;

                    //case Eシステムサウンド.SOUND青:
                    //    return this.soundBlue;

                    case Eシステムサウンド.SOUND風船:
                        return this.soundBalloon;

                    case Eシステムサウンド.SOUND曲決定音:
                        return this.sound曲決定音;

                    case Eシステムサウンド.SOUND成績発表:
                        return this.sound成績発表;
				}
				throw new IndexOutOfRangeException();
			}
		}
		public Cシステムサウンド this[ int index ]
		{
			get
			{
				switch( index )
				{
					case 0:
						return this.soundカーソル移動音;

					case 1:
						return this.sound決定音;

					case 2:
						return this.sound変更音;

					case 3:
						return this.sound取消音;

					case 4:
						return this.sound歓声音;

					case 5:
						return this.soundSTAGEFAILED音;

					case 6:
						return this.soundゲーム開始音;

					case 7:
						return this.soundゲーム終了音;

					case 8:
						return this.soundステージクリア音;

					case 9:
						return this.soundフルコンボ音;

					case 10:
						return this.sound曲読込開始音;

					case 11:
						return this.soundタイトル音;

					case 12:
						return this.bgm起動画面;

					case 13:
						return this.bgmオプション画面;

					case 14:
						return this.bgmコンフィグ画面;

					case 15:
						return this.bgm選曲画面;

                    //case 16:
                    //    return this.soundRed;

                    //case 17:
                    //    return this.soundBlue;

                    case 16:
                        return this.soundBalloon;

                    case 17:
                        return this.sound曲決定音;

                    case 18:
                        return this.sound成績発表;
				}
				throw new IndexOutOfRangeException();
			}
		}


		// スキンの切り替えについて___
		//
		// _スキンの種類は大きく分けて2種類。Systemスキンとboxdefスキン。
		// 　前者はSystem/フォルダにユーザーが自らインストールしておくスキン。
		// 　後者はbox.defで指定する、曲データ制作者が提示するスキン。
		//
		// _Config画面で、2種のスキンを区別無く常時使用するよう設定することができる。
		// _box.defの#SKINPATH 設定により、boxdefスキンを一時的に使用するよう設定する。
		// 　(box.defの効果の及ばない他のmuxic boxでは、当該boxdefスキンの有効性が無くなる)
		//
		// これを実現するために___
		// _Systemスキンの設定情報と、boxdefスキンの設定情報は、分離して持つ。
		// 　(strSystem～～ と、strBoxDef～～～)
		// _Config画面からは前者のみ書き換えできるようにし、
		// 　選曲画面からは後者のみ書き換えできるようにする。(SetCurrent...())
		// _読み出しは両者から行えるようにすると共に
		// 　選曲画面用に二種の情報を区別しない読み出し方法も提供する(GetCurrent...)

		private object lockBoxDefSkin;
		public static bool bUseBoxDefSkin = true;						// box.defからのスキン変更を許容するか否か

		public string strSystemSkinRoot = null;
		public string[] strSystemSkinSubfolders = null;		// List<string>だとignoreCaseな検索が面倒なので、配列に逃げる :-)
		private string[] _strBoxDefSkinSubfolders = null;
		public string[] strBoxDefSkinSubfolders
		{
			get
			{
				lock ( lockBoxDefSkin )
				{
					return _strBoxDefSkinSubfolders;
				}
			}
			set
			{
				lock ( lockBoxDefSkin )
				{
					_strBoxDefSkinSubfolders = value;
				}
			}
		}			// 別スレッドからも書き込みアクセスされるため、スレッドセーフなアクセス法を提供

		private static string strSystemSkinSubfolderFullName;			// Config画面で設定されたスキン
		private static string strBoxDefSkinSubfolderFullName = "";		// box.defで指定されているスキン

		/// <summary>
		/// スキンパス名をフルパスで取得する
		/// </summary>
		/// <param name="bFromUserConfig">ユーザー設定用ならtrue, box.defからの設定ならfalse</param>
		/// <returns></returns>
		public string GetCurrentSkinSubfolderFullName( bool bFromUserConfig )
		{
			if ( !bUseBoxDefSkin || bFromUserConfig == true || strBoxDefSkinSubfolderFullName == "" )
			{
				return strSystemSkinSubfolderFullName;
			}
			else
			{
				return strBoxDefSkinSubfolderFullName;
			}
		}
		/// <summary>
		/// スキンパス名をフルパスで設定する
		/// </summary>
		/// <param name="value">スキンパス名</param>
		/// <param name="bFromUserConfig">ユーザー設定用ならtrue, box.defからの設定ならfalse</param>
		public void SetCurrentSkinSubfolderFullName( string value, bool bFromUserConfig )
		{
			if ( bFromUserConfig )
			{
				strSystemSkinSubfolderFullName = value;
			}
			else
			{
				strBoxDefSkinSubfolderFullName = value;
			}
		}


		// コンストラクタ
		public CSkin( string _strSkinSubfolderFullName, bool _bUseBoxDefSkin )
		{
			lockBoxDefSkin = new object();
			strSystemSkinSubfolderFullName = _strSkinSubfolderFullName;
			bUseBoxDefSkin = _bUseBoxDefSkin;
			InitializeSkinPathRoot();
			ReloadSkinPaths();
			PrepareReloadSkin();
		}
		public CSkin()
		{
			lockBoxDefSkin = new object();
			InitializeSkinPathRoot();
			bUseBoxDefSkin = true;
			ReloadSkinPaths();
			PrepareReloadSkin();
		}
		private string InitializeSkinPathRoot()
		{
			strSystemSkinRoot = System.IO.Path.Combine( CDTXMania.strEXEのあるフォルダ, "System" + System.IO.Path.DirectorySeparatorChar );
			return strSystemSkinRoot;
		}

		/// <summary>
		/// Skin(Sounds)を再読込する準備をする(再生停止,Dispose,ファイル名再設定)。
		/// あらかじめstrSkinSubfolderを適切に設定しておくこと。
		/// その後、ReloadSkinPaths()を実行し、strSkinSubfolderの正当性を確認した上で、本メソッドを呼び出すこと。
		/// 本メソッド呼び出し後に、ReloadSkin()を実行することで、システムサウンドを読み込み直す。
		/// ReloadSkin()の内容は本メソッド内に含めないこと。起動時はReloadSkin()相当の処理をCEnumSongsで行っているため。
		/// </summary>
		public void PrepareReloadSkin()
		{
			Trace.TraceInformation( "SkinPath設定: {0}",
				( strBoxDefSkinSubfolderFullName == "" ) ?
				strSystemSkinSubfolderFullName :
				strBoxDefSkinSubfolderFullName
			);

			for ( int i = 0; i < nシステムサウンド数; i++ )
			{
				if ( this[ i ] != null && this[i].b読み込み成功 )
				{
					this[ i ].t停止する();
					this[ i ].Dispose();
				}
			}
			this.soundカーソル移動音	= new Cシステムサウンド( @"Sounds\Move.ogg",			false, false, false );
			this.sound決定音			= new Cシステムサウンド( @"Sounds\Decide.ogg",			false, false, false );
			this.sound変更音			= new Cシステムサウンド( @"Sounds\Change.ogg",			false, false, false );
			this.sound取消音			= new Cシステムサウンド( @"Sounds\Cancel.ogg",			false, false, true  );
			this.sound歓声音			= new Cシステムサウンド( @"Sounds\Audience.ogg",		false, false, true  );
			this.soundSTAGEFAILED音		= new Cシステムサウンド( @"Sounds\Stage failed.ogg",	false, true,  true  );
			this.soundゲーム開始音		= new Cシステムサウンド( @"Sounds\Game start.ogg",		false, false, false );
			this.soundゲーム終了音		= new Cシステムサウンド( @"Sounds\Game end.ogg",		false, true,  false );
			this.soundステージクリア音	= new Cシステムサウンド( @"Sounds\Stage clear.ogg",		false, true,  true  );
			this.soundフルコンボ音		= new Cシステムサウンド( @"Sounds\Full combo.ogg",		false, false, true  );
			this.sound曲読込開始音		= new Cシステムサウンド( @"Sounds\Now loading.ogg",		false, true,  true  );
			this.soundタイトル音		= new Cシステムサウンド( @"Sounds\Title.ogg",			false, true,  false );
			this.bgm起動画面			= new Cシステムサウンド( @"Sounds\Setup BGM.ogg",		true,  true,  false );
			this.bgmオプション画面		= new Cシステムサウンド( @"Sounds\Option BGM.ogg",		true,  true,  false );
			this.bgmコンフィグ画面		= new Cシステムサウンド( @"Sounds\Config BGM.ogg",		true,  true,  false );
			this.bgm選曲画面			= new Cシステムサウンド( @"Sounds\Select BGM.ogg",		true,  true,  false );

            //this.soundRed               = new Cシステムサウンド( @"Sounds\dong.ogg",            false, false, true  );
            //this.soundBlue              = new Cシステムサウンド( @"Sounds\ka.ogg",              false, false, true  );
            this.soundBalloon           = new Cシステムサウンド( @"Sounds\balloon.ogg",         false, false, true  );
            this.sound曲決定音          = new Cシステムサウンド( @"Sounds\SongDecide.ogg",      false, false, true  );
            this.sound成績発表          = new Cシステムサウンド( @"Sounds\ResultIn.ogg",          false, false, false );

            tReadSkinConfig();
		}

		public void ReloadSkin()
		{
			for ( int i = 0; i < nシステムサウンド数; i++ )
			{
				if ( !this[ i ].b排他 )	// BGM系以外のみ読み込む。(BGM系は必要になったときに読み込む)
				{
					Cシステムサウンド cシステムサウンド = this[ i ];
					if ( !CDTXMania.bコンパクトモード || cシステムサウンド.bCompact対象 )
					{
						try
						{
							cシステムサウンド.t読み込み();
							Trace.TraceInformation( "システムサウンドを読み込みました。({0})", cシステムサウンド.strファイル名 );
						}
						catch ( FileNotFoundException )
						{
							Trace.TraceWarning( "システムサウンドが存在しません。({0})", cシステムサウンド.strファイル名 );
						}
						catch ( Exception e )
						{
							Trace.TraceError( e.Message );
							Trace.TraceWarning( "システムサウンドの読み込みに失敗しました。({0})", cシステムサウンド.strファイル名 );
						}
					}
				}
			}
		}


		/// <summary>
		/// Skinの一覧を再取得する。
		/// System/*****/Graphics (やSounds/) というフォルダ構成を想定している。
		/// もし再取得の結果、現在使用中のSkinのパス(strSystemSkinSubfloderFullName)が消えていた場合は、
		/// 以下の優先順位で存在確認の上strSystemSkinSubfolderFullNameを再設定する。
		/// 1. System/Default/
		/// 2. System/*****/ で最初にenumerateされたもの
 		/// 3. System/ (従来互換)
		/// </summary>
		public void ReloadSkinPaths()
		{
			#region [ まず System/*** をenumerateする ]
			string[] tempSkinSubfolders = System.IO.Directory.GetDirectories( strSystemSkinRoot, "*" );
			strSystemSkinSubfolders = new string[ tempSkinSubfolders.Length ];
			int size = 0;
			for ( int i = 0; i < tempSkinSubfolders.Length; i++ )
			{
				#region [ 検出したフォルダがスキンフォルダかどうか確認する]
				if ( !bIsValid( tempSkinSubfolders[ i ] ) )
					continue;
				#endregion
				#region [ スキンフォルダと確認できたものを、strSkinSubfoldersに入れる ]
				// フォルダ名末尾に必ず\をつけておくこと。さもないとConfig読み出し側(必ず\をつける)とマッチできない
				if ( tempSkinSubfolders[ i ][ tempSkinSubfolders[ i ].Length - 1 ] != System.IO.Path.DirectorySeparatorChar )
				{
					tempSkinSubfolders[ i ] += System.IO.Path.DirectorySeparatorChar;
				}
				strSystemSkinSubfolders[ size ] = tempSkinSubfolders[ i ];
				Trace.TraceInformation( "SkinPath検出: {0}", strSystemSkinSubfolders[ size ] );
				size++;
				#endregion
			}
			Trace.TraceInformation( "SkinPath入力: {0}", strSystemSkinSubfolderFullName );
			Array.Resize( ref strSystemSkinSubfolders, size );
			Array.Sort( strSystemSkinSubfolders );	// BinarySearch実行前にSortが必要
			#endregion

			#region [ 現在のSkinパスがbox.defスキンをCONFIG指定していた場合のために、最初にこれが有効かチェックする。有効ならこれを使う。 ]
			if ( bIsValid( strSystemSkinSubfolderFullName ) &&
				Array.BinarySearch( strSystemSkinSubfolders, strSystemSkinSubfolderFullName,
				StringComparer.InvariantCultureIgnoreCase ) < 0 )
			{
				strBoxDefSkinSubfolders = new string[ 1 ]{ strSystemSkinSubfolderFullName };
				return;
			}
			#endregion

			#region [ 次に、現在のSkinパスが存在するか調べる。あれば終了。]
			if ( Array.BinarySearch( strSystemSkinSubfolders, strSystemSkinSubfolderFullName,
				StringComparer.InvariantCultureIgnoreCase ) >= 0 )
				return;
			#endregion
			#region [ カレントのSkinパスが消滅しているので、以下で再設定する。]
			/// 以下の優先順位で現在使用中のSkinパスを再設定する。
			/// 1. System/Default/
			/// 2. System/*****/ で最初にenumerateされたもの
			/// 3. System/ (従来互換)
			#region [ System/Default/ があるなら、そこにカレントSkinパスを設定する]
			string tempSkinPath_default = System.IO.Path.Combine( strSystemSkinRoot, "Default" + System.IO.Path.DirectorySeparatorChar );
			if ( Array.BinarySearch( strSystemSkinSubfolders, tempSkinPath_default, 
				StringComparer.InvariantCultureIgnoreCase ) >= 0 )
			{
				strSystemSkinSubfolderFullName = tempSkinPath_default;
				return;
			}
			#endregion
			#region [ System/SkinFiles.*****/ で最初にenumerateされたものを、カレントSkinパスに再設定する ]
			if ( strSystemSkinSubfolders.Length > 0 )
			{
				strSystemSkinSubfolderFullName = strSystemSkinSubfolders[ 0 ];
				return;
			}
			#endregion
			#region [ System/ に、カレントSkinパスを再設定する。]
			strSystemSkinSubfolderFullName = strSystemSkinRoot;
			strSystemSkinSubfolders = new string[ 1 ]{ strSystemSkinSubfolderFullName };
			#endregion
			#endregion
		}

		// メソッド

		public static string Path( string strファイルの相対パス )
		{
			if ( strBoxDefSkinSubfolderFullName == "" || !bUseBoxDefSkin )
			{
				return System.IO.Path.Combine( strSystemSkinSubfolderFullName, strファイルの相対パス );
			}
			else
			{
				return System.IO.Path.Combine( strBoxDefSkinSubfolderFullName, strファイルの相対パス );
			}
		}

		/// <summary>
		/// フルパス名を与えると、スキン名として、ディレクトリ名末尾の要素を返す
		/// 例: C:\foo\bar\ なら、barを返す
		/// </summary>
		/// <param name="skinpath">スキンが格納されたパス名(フルパス)</param>
		/// <returns>スキン名</returns>
		public static string GetSkinName( string skinPathFullName )
		{
			if ( skinPathFullName != null )
			{
				if ( skinPathFullName == "" )		// 「box.defで未定義」用
					skinPathFullName = strSystemSkinSubfolderFullName;
				string[] tmp = skinPathFullName.Split( System.IO.Path.DirectorySeparatorChar );
				return tmp[ tmp.Length - 2 ];		// ディレクトリ名の最後から2番目の要素がスキン名(最後の要素はnull。元stringの末尾が\なので。)
			}
			return null;
		}
		public static string[] GetSkinName( string[] skinPathFullNames )
		{
			string[] ret = new string[ skinPathFullNames.Length ];
			for ( int i = 0; i < skinPathFullNames.Length; i++ )
			{
				ret[ i ] = GetSkinName( skinPathFullNames[ i ] );
			}
			return ret;
		}


		public string GetSkinSubfolderFullNameFromSkinName( string skinName )
		{
			foreach ( string s in strSystemSkinSubfolders )
			{
				if ( GetSkinName( s ) == skinName )
					return s;
			}
			foreach ( string b in strBoxDefSkinSubfolders )
			{
				if ( GetSkinName( b ) == skinName )
					return b;
			}
			return null;
		}

		/// <summary>
		/// スキンパス名が妥当かどうか
		/// (タイトル画像にアクセスできるかどうかで判定する)
		/// </summary>
		/// <param name="skinPathFullName">妥当性を確認するスキンパス(フルパス)</param>
		/// <returns>妥当ならtrue</returns>
		public bool bIsValid( string skinPathFullName )
		{
			string filePathTitle;
			filePathTitle = System.IO.Path.Combine( skinPathFullName, @"Graphics\1_Title\Background.png" );
			return ( File.Exists( filePathTitle ) );
		}


		public void tRemoveMixerAll()
		{
			for ( int i = 0; i < nシステムサウンド数; i++ )
			{
				if ( this[ i ] != null && this[ i ].b読み込み成功 )
				{
					this[ i ].t停止する();
					this[ i ].tRemoveMixer();
				}
			}

		}

        /// <summary>
        /// 変数の初期化
        /// </summary>
        public void tSkinConfigInit()
        {
            this.eDiffDispMode = E難易度表示タイプ.n曲目に表示;
            this.b現在のステージ数を表示しない = false;
        }

        public void tReadSkinConfig()
        {
            if( File.Exists( CSkin.Path( @"SkinConfig.ini" ) ) )
            {
                string str;
				using ( StreamReader reader = new StreamReader( CSkin.Path( @"SkinConfig.ini" ), Encoding.GetEncoding( "Shift_JIS" ) ) )
                {
				    str = reader.ReadToEnd();
                }
                this.t文字列から読み込み( str );
            }
            else
            {
                Trace.TraceInformation( "SkinConfig.iniが見つかりませんでした。デフォルトの設定値を使用します。" );
            }
        }

        private void t文字列から読み込み( string strAllSettings )	// 2011.4.13 yyagi; refactored to make initial KeyConfig easier.
        {
            string[] delimiter = { "\n" };
            string[] strSingleLine = strAllSettings.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in strSingleLine)
            {
                string str = s.Replace('\t', ' ').TrimStart(new char[] { '\t', ' ' });
                if ((str.Length != 0) && (str[0] != ';'))
                {
                    try
                    {
                        string strCommand;
                        string strParam;
                        string[] strArray = str.Split(new char[] { '=' });
                        if (strArray.Length == 2)
                        {
                            strCommand = strArray[0].Trim();
                            strParam = strArray[1].Trim();

                            #region スキン設定
                            if (strCommand == "Name")
                            {
                                this.Skin_Name = strParam;
                            }
                            else if (strCommand == "Version")
                            {
                                this.Skin_Version = strParam;
                            }
                            else if (strCommand == "Creator")
                            {
                                this.Skin_Creator = strParam;
                            }
                            #endregion

                            #region 背景(スクロール)
                            else if (strCommand == "Background_Scroll_Y")
                            {
                                string[] strSplit = strParam.Split(',');
                                for (int i = 0; i < 2; i++)
                                {
                                    this.Background_Scroll_Y[i] = int.Parse(strSplit[i]);
                                }
                            }
                            #endregion
                            else if (strCommand == "Game_Score_X")
                            {
                                string[] strSplit = strParam.Split(',');
                                for (int i = 0; i < 2; i++)
                                {
                                    this.nScoreX[i] = int.Parse(strSplit[i]);
                                }
                            }
                            else if (strCommand == "Game_Score_Y")
                            {
                                string[] strSplit = strParam.Split(',');
                                for (int i = 0; i < 2; i++)
                                {
                                    this.nScoreY[i] = int.Parse(strSplit[i]);
                                }
                            }
                            else if (strCommand == "Game_ScoreAdd_X")
                            {
                                string[] strSplit = strParam.Split(',');
                                for (int i = 0; i < 2; i++)
                                {
                                    this.nScoreAddX[i] = int.Parse(strSplit[i]);
                                }
                            }
                            else if (strCommand == "Game_ScoreAdd_Y")
                            {
                                string[] strSplit = strParam.Split(',');
                                for (int i = 0; i < 2; i++)
                                {
                                    this.nScoreAddY[i] = int.Parse(strSplit[i]);
                                }
                            }
                            else if (strCommand == "Game_ScoreAddBonus_X")
                            {
                                string[] strSplit = strParam.Split(',');
                                for (int i = 0; i < 2; i++)
                                {
                                    this.nScoreAddBonusX[i] = int.Parse(strSplit[i]);
                                }
                            }
                            else if (strCommand == "Game_ScoreAddBonus_Y")
                            {
                                string[] strSplit = strParam.Split(',');
                                for (int i = 0; i < 2; i++)
                                {
                                    this.nScoreAddBonusY[i] = int.Parse(strSplit[i]);
                                }
                            }
                            else if (strCommand == "Game_Balloon_Balloon_X")
                            {
                                string[] strSplit = strParam.Split(',');
                                for (int i = 0; i < 2; i++)
                                {
                                    this.nBurstBalloonX[i] = int.Parse(strSplit[i]);
                                }
                            }
                            else if (strCommand == "Game_Balloon_Balloon_Y")
                            {
                                string[] strSplit = strParam.Split(',');
                                for (int i = 0; i < 2; i++)
                                {
                                    this.nBurstBalloonY[i] = int.Parse(strSplit[i]);
                                }
                            }
                            else if (strCommand == "Game_Balloon_Balloon_Frame_X")
                            {
                                string[] strSplit = strParam.Split(',');
                                for (int i = 0; i < 2; i++)
                                {
                                    this.nBurstFrameX[i] = int.Parse(strSplit[i]);
                                }
                            }
                            else if (strCommand == "Game_Balloon_Balloon_Frame_Y")
                            {
                                string[] strSplit = strParam.Split(',');
                                for (int i = 0; i < 2; i++)
                                {
                                    this.nBurstFrameY[i] = int.Parse(strSplit[i]);
                                }
                            }
                            else if (strCommand == "Game_Balloon_Balloon_Number_X")
                            {
                                string[] strSplit = strParam.Split(',');
                                for (int i = 0; i < 2; i++)
                                {
                                    this.nBurstNumberX[i] = int.Parse(strSplit[i]);
                                }
                            }
                            else if (strCommand == "Game_Balloon_Balloon_Number_Y")
                            {
                                string[] strSplit = strParam.Split(',');
                                for (int i = 0; i < 2; i++)
                                {
                                    this.nBurstNumberY[i] = int.Parse(strSplit[i]);
                                }
                            }

                            #region[ 演奏 ]
                            //-----------------------------
                            else if (strCommand == "ScrollFieldP1Y")
                            {
                                this.nScrollFieldY[0] = C変換.n値を文字列から取得して返す(strParam, 192);
                            }
                            else if (strCommand == "ScrollFieldP2Y")
                            {
                                this.nScrollFieldY[1] = C変換.n値を文字列から取得して返す(strParam, 192);
                            }
                            else if (strCommand == "SENotesP1Y")
                            {
                                this.nSENotesY[0] = C変換.n値を文字列から取得して返す(strParam, this.nSENotesY[0]);
                            }
                            else if (strCommand == "SENotesP2Y")
                            {
                                this.nSENotesY[1] = C変換.n値を文字列から取得して返す(strParam, this.nSENotesY[1]);
                            }
                            else if (strCommand == "JudgePointP1Y")
                            {
                                this.nJudgePointY[0] = C変換.n値を文字列から取得して返す(strParam, this.nJudgePointY[0]);
                            }
                            else if (strCommand == "JudgePointP2Y")
                            {
                                this.nJudgePointY[1] = C変換.n値を文字列から取得して返す(strParam, this.nJudgePointY[1]);
                            }

                            else if (strCommand == "PlayerCharacterP1X")
                            {
                                this.nPlayerCharacterX[0] = C変換.n値を文字列から取得して返す(strParam, 0);
                            }
                            else if (strCommand == "PlayerCharacterP1Y")
                            {
                                this.nPlayerCharacterY[0] = C変換.n値を文字列から取得して返す(strParam, 0);
                            }
                            else if (strCommand == "CourseSymbolP1X")
                            {
                                this.nCourseSymbolX[0] = C変換.n値を文字列から取得して返す(strParam, this.nCourseSymbolX[0]);
                            }
                            else if (strCommand == "CourseSymbolP1Y")
                            {
                                this.nCourseSymbolY[0] = C変換.n値を文字列から取得して返す(strParam, this.nCourseSymbolY[0]);
                            }

                            else if (strCommand == "DiffDispMode")
                            {
                                this.eDiffDispMode = (E難易度表示タイプ)C変換.n値を文字列から取得して範囲内に丸めて返す(strParam, 0, 2, (int)this.eDiffDispMode);
                            }
                            else if (strCommand == "NowStageDisp")
                            {
                                this.b現在のステージ数を表示しない = C変換.bONorOFF(strParam[0]);
                            }
                            else if (strCommand == "StageText")
                            {
                                this.str曲数テキスト = strParam;
                            }
                            else if (strCommand == "IsStageTextRed")
                            {
                                this.b曲数テキストを赤くする = C変換.bONorOFF(strParam[0]);
                            }
                            //else if( strCommand == "AddScoreColorP1" )
                            //{
                            //    string[] arColor = strParam.Split(',');
                            //    if( arColor.Length == 3 )
                            //    {
                            //        this.cScoreColor1P = C変換.n255ToColor4( Convert.ToInt16( arColor[ 0 ] ), Convert.ToInt16( arColor[ 1 ] ), Convert.ToInt16( arColor[ 2 ] ) );
                            //    }
                            //}
                            //else if( strCommand == "AddScoreColorP2" )
                            //{
                            //    string[] arColor = strParam.Split(',');
                            //    if( arColor.Length == 3 )
                            //    {
                            //        this.cScoreColor2P = C変換.n255ToColor4( Convert.ToInt16( arColor[ 0 ] ), Convert.ToInt16( arColor[ 1 ] ), Convert.ToInt16( arColor[ 2 ] ) );
                            //    }
                            //}

                            //-----------------------------
                            #endregion
                            #region[ 成績発表 ]
                            //-----------------------------
                            else if (strCommand == "ResultPanelP1X")
                            {
                                this.nResultPanelP1X = C変換.n値を文字列から取得して返す(strParam, 515);
                            }
                            else if (strCommand == "ResultPanelP1Y")
                            {
                                this.nResultPanelP1Y = C変換.n値を文字列から取得して返す(strParam, 75);
                            }
                            else if (strCommand == "ResultPanelP2X")
                            {
                                this.nResultPanelP2X = C変換.n値を文字列から取得して返す(strParam, 515);
                            }
                            else if (strCommand == "ResultPanelP2Y")
                            {
                                this.nResultPanelP2Y = C変換.n値を文字列から取得して返す(strParam, 75);
                            }
                            else if (strCommand == "ResultScoreP1X")
                            {
                                this.nResultScoreP1X = C変換.n値を文字列から取得して返す(strParam, 582);
                            }
                            else if (strCommand == "ResultScoreP1Y")
                            {
                                this.nResultScoreP1Y = C変換.n値を文字列から取得して返す(strParam, 252);
                            }
                            //-----------------------------
                            #endregion
                            #region[ その他 ]
                            #endregion

                            #region 新・SkinConfig
                            #region Game
                            else if (strCommand == "Game_Notes_Anime")
                            {
                                Game_Notes_Anime = int.Parse(strParam);
                            }
                            else if (strCommand == "Game_StageText")
                            {
                                Game_StageText = strParam;
                            }
                            else if (strCommand == "Game_StageText_IsRed")
                            {
                                Game_StageText_IsRed = int.Parse(strParam);
                            }
                            #region CourseSymbol
                            else if (strCommand == "Game_CourseSymbol_X")
                            {
                                string[] strSplit = strParam.Split(',');
                                for (int i = 0; i < 2; i++)
                                {
                                    Game_CourseSymbol_X[i] = int.Parse(strSplit[i]);
                                }
                            }
                            else if (strCommand == "Game_CourseSymbol_Y")
                            {
                                string[] strSplit = strParam.Split(',');
                                for (int i = 0; i < 2; i++)
                                {
                                    Game_CourseSymbol_Y[i] = int.Parse(strSplit[i]);
                                }
                            }
                            #endregion
                            #region Chara
                            else if (strCommand == "Game_Chara_X")
                            {
                                string[] strSplit = strParam.Split(',');
                                for (int i = 0; i < 2; i++)
                                {
                                    Game_Chara_X[i] = int.Parse(strSplit[i]);
                                }
                            }
                            else if (strCommand == "Game_Chara_Y")
                            {
                                string[] strSplit = strParam.Split(',');
                                for (int i = 0; i < 2; i++)
                                {
                                    Game_Chara_Y[i] = int.Parse(strSplit[i]);
                                }
                            }
                            // パターン数の設定はTextureLoader.csで反映されます。
                            else if (strCommand == "Game_Chara_Motion_Normal")
                            {
                                Game_Chara_Motion_Normal = strParam;
                            }
                            else if (strCommand == "Game_Chara_Motion_Clear")
                            {
                                Game_Chara_Motion_Clear = strParam;
                            }
                            else if (strCommand == "Game_Chara_Motion_GoGo")
                            {
                                Game_Chara_Motion_GoGo = strParam;
                            }
                            else if (strCommand == "Game_Chara_Beat_Normal")
                            {
                                Game_Chara_Beat_Normal = int.Parse(strParam);
                            }
                            else if (strCommand == "Game_Chara_Beat_Clear")
                            {
                                Game_Chara_Beat_Clear = int.Parse(strParam);
                            }
                            else if (strCommand == "Game_Chara_Beat_GoGo")
                            {
                                Game_Chara_Beat_GoGo = int.Parse(strParam);
                            }
                            #endregion
                            #region Dancer
                            else if (strCommand == "Game_Dancer_X")
                            {
                                string[] strSplit = strParam.Split(',');
                                for (int i = 0; i < 5; i++)
                                {
                                    Game_Dancer_X[i] = int.Parse(strSplit[i]);
                                }
                            }
                            else if (strCommand == "Game_Dancer_Y")
                            {
                                string[] strSplit = strParam.Split(',');
                                for (int i = 0; i < 5; i++)
                                {
                                    Game_Dancer_Y[i] = int.Parse(strSplit[i]);
                                }
                            }
                            else if (strCommand == "Game_Dancer_Motion")
                            {
                                Game_Dancer_Motion = strParam;
                            }
                            // Game_Dancer_PtnはTextrueLoader.csで反映されます。
                            else if (strCommand == "Game_Dancer_Beat")
                            {
                                Game_Dancer_Beat = int.Parse(strParam);
                            }
                            else if (strCommand == "Game_Dancer_Gauge")
                            {
                                string[] strSplit = strParam.Split(',');
                                for (int i = 0; i < 5; i++)
                                {
                                    Game_Dancer_Gauge[i] = int.Parse(strSplit[i]);
                                }
                            }
                            #endregion
                            #region Mob
                            else if (strCommand == "Game_Mob_Beat")
                            {
                                Game_Mob_Beat = int.Parse(strParam);
                            }
                            else if (strCommand == "Game_Mob_Ptn_Beat")
                            {
                                Game_Mob_Ptn_Beat = int.Parse(strParam);
                            }
                            #endregion
                            #region Taiko
                            else if (strCommand == "Game_Taiko_NamePlate_X")
                            {
                                string[] strSplit = strParam.Split(',');
                                for (int i = 0; i < 2; i++)
                                {
                                    Game_Taiko_NamePlate_X[i] = int.Parse(strSplit[i]);
                                }
                            }
                            else if (strCommand == "Game_Taiko_NamePlate_Y")
                            {
                                string[] strSplit = strParam.Split(',');
                                for (int i = 0; i < 2; i++)
                                {
                                    Game_Taiko_NamePlate_Y[i] = int.Parse(strSplit[i]);
                                }
                            }

                            else if (strCommand == "Game_Taiko_PlayerNumber_X")
                            {
                                string[] strSplit = strParam.Split(',');
                                for (int i = 0; i < 2; i++)
                                {
                                    Game_Taiko_PlayerNumber_X[i] = int.Parse(strSplit[i]);
                                }
                            }

                            else if (strCommand == "Game_Taiko_PlayerNumber_Y")
                            {
                                string[] strSplit = strParam.Split(',');
                                for (int i = 0; i < 2; i++)
                                {
                                    Game_Taiko_PlayerNumber_Y[i] = int.Parse(strSplit[i]);
                                }
                            }

                            #endregion
                            #region Runner
                            else if (strCommand == "Game_Runner_Size")
                            {
                                string[] strSplit = strParam.Split(',');
                                for (int i = 0; i < 2; i++)
                                {
                                    Game_Runner_Size[i] = int.Parse(strSplit[i]);
                                }
                            }
                            else if (strCommand == "Game_Runner_Ptn")
                            {
                                Game_Runner_Ptn = int.Parse(strParam);
                            }
                            else if (strCommand == "Game_Runner_Type")
                            {
                                Game_Runner_Type = int.Parse(strParam);
                            }
                            else if (strCommand == "Game_Runner_StartPoint_X")
                            {
                                string[] strSplit = strParam.Split(',');
                                for (int i = 0; i < 2; i++)
                                {
                                    Game_Runner_StartPoint_X[i] = int.Parse(strSplit[i]);
                                }
                            }
                            else if (strCommand == "Game_Runner_StartPoint_Y")
                            {
                                string[] strSplit = strParam.Split(',');
                                for (int i = 0; i < 2; i++)
                                {
                                    Game_Runner_StartPoint_Y[i] = int.Parse(strSplit[i]);
                                }
                            }
                            else if (strCommand == "Game_Runner_Timer")
                            {
                                Game_Runner_Timer = int.Parse(strParam);
                            }
                            #endregion
                            #endregion
                            #endregion
                        }
                        continue;
                    }
                    catch( Exception exception )
                    {
                        Trace.TraceError( exception.Message );
                        continue;
                    }
                }
            }
        }

        private void t座標の追従設定()
        {
            //
            if( bFieldBgPointOverride == true )
            {

            }
        }

		#region [ IDisposable 実装 ]
		//-----------------
		public void Dispose()
		{
			if( !this.bDisposed済み )
			{
				for( int i = 0; i < this.nシステムサウンド数; i++ )
					this[ i ].Dispose();

				this.bDisposed済み = true;
			}
		}
		//-----------------
		#endregion


		// その他

		#region [ private ]
		//-----------------
		private bool bDisposed済み;
        //-----------------
        #endregion

        #region スキン設定
        #endregion

        #region 背景(スクロール)
        public int[] Background_Scroll_Y = new int[] {0, 536};
        #endregion


        #region[ 座標 ]
        //2017.08.11 kairera0467 DP実用化に向けてint配列に変更

        //フィールド位置　Xは判定枠部分の位置。Yはフィールドの最上部の座標。
        //現時点ではノーツ画像、Senotes画像、判定枠が連動する。
        //Xは中央基準描画、Yは左上基準描画
        public int[] nScrollFieldX = new int[]{414, 414};
        public int[] nScrollFieldY = new int[]{192, 368};

        //中心座標指定
        public int[] nJudgePointX = new int[] { 413, 413, 413, 413 };
        public int[] nJudgePointY = new int[] { 256, 433, 0, 0  };
                
        //フィールド背景画像
        //ScrollField座標への追従設定が可能。
        //分岐背景、ゴーゴー背景が連動する。(全て同じ大きさ、位置で作成すること。)
        //左上基準描画
        public bool bFieldBgPointOverride = false;
        public int[] nScrollFieldBGX = new int[]{ 333, 333, 333, 333 };
        public int[] nScrollFieldBGY = new int[]{ 192, 368, 0, 0 };

        //SEnotes
        //音符座標に加算
        public int[] nSENotesY = new int[]{ 131, 131 };

        //光る太鼓部分
        public int nMtaikoBackgroundX = 0;
        public int nMtaikoBackgroundY = 184;
        public int nMtaikoFieldX = 0;
        public int nMtaikoFieldY = 184;
        public int nMtaikoMainX = 0;
        public int nMtaikoMainY = 0;

        //コンボ
        public int[] nComboNumberX = new int[] { 0, 0, 0, 0 };
        public int[] nComboNumberY = new int[] { 212, 388, 0, 0 };
        public int[] nComboNumberTextY = new int[] { 271, 447, 0, 0 };
        public int[] nComboNumberTextLargeY = new int[] { 270, 446, 0, 0 };
        public float fComboNumberSpacing = 0;
        public float fComboNumberSpacing_l = 0;

        //コンボ吹き出し(2Pまで)
        public int[] nComboBalloonX = new int[] { 253, 253 };
        public int[] nComboBalloonY = new int[] { -11, 498 };
        public int[] nComboBalloonNumberX = new int[] { 312, 312 };
        public int[] nComboBalloonNumberY = new int[] { 34, 540 };
        public int[] nComboBalloonNumber_1000X = new int[] { 335, 335 };
        public int[] nComboBalloonNumber_1000Y = new int[] { 34, 540 };
        public int[] nComboBalloonTextX = new int[] { 471, 471 };
        public int[] nComboBalloonTextY = new int[] { 55, 561 };
        public int[] nComboBalloonText_1000X = new int[] { 491, 491 };
        public int[] nComboBalloonText_1000Y = new int[] { 55, 561 };

        //スコア
        public int[] nScoreX = new int[] { 20, 20, 0, 0 };
        public int[] nScoreY = new int[] { 194, 499, 0, 0 };
        public int[] nScoreAddX = new int[] { 20, 20, 0, 0 };
        public int[] nScoreAddY = new int[] { 156, 536, 0, 0 };
        public int[] nScoreAddBonusX = new int[] { 20, 20, 0, 0 };
        public int[] nScoreAddBonusY = new int[] { 100, 580, 0, 0 };
        //public SlimDX.Color4 cScoreColor1P = new SlimDX.Color4( 1.0f, 1.0f, 0.5f, 0.4f );
        //public SlimDX.Color4 cScoreColor2P = new SlimDX.Color4( 1.0f, 0.4f, 0.5f, 1.0f );

        //風船
        //次郎とは違い、風船画像はBalloon、吹き出しはFrameという名前にしています。
        public int[] nBurstBalloonX = new int[] { 382, 382, 0, 0 };
        public int[] nBurstBalloonY = new int[] { 115, 290, 0, 0 };
        public int[] nBurstFrameX = new int[] { 382, 382, 0, 0 };
        public int[] nBurstFrameY = new int[] { 80, 260, 0, 0 };
        public int[] nBurstNumberX = new int[] { 486, 486, 0, 0 };
        public int[] nBurstNumberY = new int[] { 114, 300, 0, 0 };

        //コースシンボル
        //中央基準
        public int[] nCourseSymbolX = new int[]{ 72, 72, 0, 0 };
        public int[] nCourseSymbolY = new int[]{ 180, 230, 0, 0 };

        //分岐吹き出し(中央基準)
        public int[] nBranchBalloonX = new int[] { 0, 0, 0, 0 };
        public int[] nBranchBalloonY = new int[] { 0, 0, 0, 0 };

        //キャラクター画像の位置
        public int[] nPlayerCharacterX = new int[]{0};
        public int[] nPlayerCharacterY = new int[]{0};
        public int[] nPlayerCharacterBalloonX = new int[] { 240, 240, 0, 0 };
        public int[] nPlayerCharacterBalloonY = new int[] { 0, 297, 0, 0 };

        public E難易度表示タイプ eDiffDispMode;
        public bool b現在のステージ数を表示しない;

        public string str曲数テキスト = "1曲目";
        public bool b曲数テキストを赤くする = false;

        //リザルト画面
        //現在のデフォルト値はダミーです。
        public int nResultPanelP1X = 515;
        public int nResultPanelP1Y = 75;
        public int nResultPanelP2X = 515;
        public int nResultPanelP2Y = 75;
        public int nResultScoreP1X = 582;
        public int nResultScoreP1Y = 252;
        public int nResultJudge1_P1X = 815;
        public int nResultJudge1_P1Y = 182;
        public int nResultJudge2_P1X = 968;
        public int nResultJudge2_P1Y = 174;
        public int nResultGreatP1X = 875;
        public int nResultGreatP1Y = 188;
        public int nResultGreatP2X = 875;
        public int nResultGreatP2Y = 188;
        public int nResultGoodP1X = 875;
        public int nResultGoodP1Y = 226;
        public int nResultGoodP2X = 875;
        public int nResultGoodP2Y = 226;
        public int nResultBadP1X = 875;
        public int nResultBadP1Y = 266;
        public int nResultBadP2X = 875;
        public int nResultBadP2Y = 266;
        public int nResultComboP1X = 1144;
        public int nResultComboP1Y = 188;
        public int nResultComboP2X = 1144;
        public int nResultComboP2Y = 188;
        public int nResultRollP1X = 1144;
        public int nResultRollP1Y = 226;
        public int nResultRollP2X = 1144;
        public int nResultRollP2Y = 226;
        public int nResultGaugeBaseP1X = 555;
        public int nResultGaugeBaseP1Y = 122;
        public int nResultGaugeBaseP2X = 555;
        public int nResultGaugeBaseP2Y = 122;
        public int nResultGaugeBodyP1X = 559;
        public int nResultGaugeBodyP1Y = 125;
        #endregion

        #region 新・SkinConfig
        #region General
        public string Skin_Name = "Unknown";
        public string Skin_Version = "Unknown";
        public string Skin_Creator = "Unknown";
        #endregion
        #region Game
        public int Game_Notes_Anime = 0;
        public string Game_StageText = "1曲目";
        public int Game_StageText_IsRed = 0;
        #region Chara
        public int[] Game_Chara_X = new int[] { 0, 0 };
        public int[] Game_Chara_Y = new int[] { 0, 537 };
        public int Game_Chara_Ptn_Normal,
            Game_Chara_Ptn_GoGo,
            Game_Chara_Ptn_Clear,
            Game_Chara_Ptn_10combo,
            Game_Chara_Ptn_10combo_Max,
            Game_Chara_Ptn_GoGoStart,
            Game_Chara_Ptn_GoGoStart_Max,
            Game_Chara_Ptn_ClearIn,
            Game_Chara_Ptn_SoulIn = 0;
        public string Game_Chara_Motion_Normal,
            Game_Chara_Motion_Clear,
            Game_Chara_Motion_GoGo = "0";
        public int Game_Chara_Beat_Normal = 1;
        public int Game_Chara_Beat_Clear = 2;
        public int Game_Chara_Beat_GoGo = 2;
        #endregion
        #region Dancer
        public int[] Game_Dancer_X = new int[] { 640, 430, 856, 215, 1070 };
        public int[] Game_Dancer_Y = new int[] { 500, 500, 500, 500, 500 };
        public string Game_Dancer_Motion = "0";
        public int Game_Dancer_Ptn = 0;
        public int Game_Dancer_Beat = 8;
        public int[] Game_Dancer_Gauge = new int[] { 0, 20, 40, 60, 80 };
        #endregion
        #region Mob
        public int Game_Mob_Ptn = 0;
        public int Game_Mob_Beat,
            Game_Mob_Ptn_Beat = 1;
        #endregion
        #region CourseSymbol
        public int[] Game_CourseSymbol_X = new int[] { 64, 64 };
        public int[] Game_CourseSymbol_Y = new int[] { 232, 432 };
        #endregion
        #region Taiko
        public int[] Game_Taiko_NamePlate_X = new int[] { 0, 0 };
        public int[] Game_Taiko_NamePlate_Y = new int[] { 288, 368 };
        public int[] Game_Taiko_PlayerNumber_X = new int[] { 4, 4 };
        public int[] Game_Taiko_PlayerNumber_Y = new int[] { 233, 435 };
        #endregion
        #region Runner
        public int[] Game_Runner_Size = new int[] { 60, 125 };
        public int Game_Runner_Ptn = 48;
        public int Game_Runner_Type = 4;
        public int[] Game_Runner_StartPoint_X = new int[] { 175, 175 };
        public int[] Game_Runner_StartPoint_Y = new int[] { 40, 560 };
        public int Game_Runner_Timer = 16;
        #endregion
        #endregion
        #endregion
    }
}
