using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Drawing;
using System.IO;
using SlimDX;
using FDK;

namespace DTXMania
{
	internal class CAct演奏PauseMenu : CActSelectPopupMenu
	{
		private readonly string QuickCfgTitle = "ポーズ";
		// コンストラクタ

		public CAct演奏PauseMenu()
		{
            CAct演奏PauseMenuMain();
		}

        private void tMoveToPrev()
        {
            this.nCurrentTarget--;
        }

        private void tMoveToNext()
        {
            this.nCurrentTarget++;
        }

        private void CAct演奏PauseMenuMain()
		{
            this.bEsc有効 = false;
			lci = new List<List<List<CItemBase>>>();									// この画面に来る度に、メニューを作り直す。
			for ( int nConfSet = 0; nConfSet < 3; nConfSet++ )
			{
				lci.Add( new List<List<CItemBase>>() );									// ConfSet用の3つ分の枠。
				for ( int nInst = 0; nInst < 3; nInst++ )
				{
					lci[ nConfSet ].Add( null );										// Drum/Guitar/Bassで3つ分、枠を作っておく
					lci[ nConfSet ][ nInst ] = MakeListCItemBase( nConfSet, nInst );
				}
			}
			base.Initialize( lci[ 0 ][ 0 ], true, QuickCfgTitle, 2 );	// ConfSet=0, nInst=Drums
		}

		private List<CItemBase> MakeListCItemBase( int nConfigSet, int nInst )
		{
			List<CItemBase> l = new List<CItemBase>();

			#region [ 共通 SET切り替え/More/Return ]
			l.Add( new CSwitchItemList( "続ける", CItemBase.Eパネル種別.通常, 0, "", "", new string[] { "" } ) );
			l.Add( new CSwitchItemList( "やり直し", CItemBase.Eパネル種別.通常, 0, "", "", new string[] { "" } ) );
			l.Add( new CSwitchItemList( "演奏中止", CItemBase.Eパネル種別.通常, 0, "", "", new string[] { "", "" } ) );
			#endregion

			return l;
		}

		// メソッド
		public override void tActivatePopupMenu( E楽器パート einst )
		{
            this.CAct演奏PauseMenuMain();
            this.bやり直しを選択した = false;
			base.tActivatePopupMenu( einst );
		}
		//public void tDeativatePopupMenu()
		//{
		//	base.tDeativatePopupMenu();
		//}

		public override void t進行描画sub()
		{
            if( this.bやり直しを選択した )
            {
                if( !sw.IsRunning )
                    this.sw = Stopwatch.StartNew();
                if( sw.ElapsedMilliseconds > 1500 )
                {
                    CDTXMania.stage演奏ドラム画面.bPAUSE = false;
                    CDTXMania.stage演奏ドラム画面.t演奏やりなおし();

	    		    this.tDeativatePopupMenu();
                    this.sw.Reset();
                }
            }
		}

		public override void tEnter押下Main( int nSortOrder )
		{
            switch ( n現在の選択行 )
            {
				case (int) EOrder.Continue:
                    CDTXMania.stage演奏ドラム画面.bPAUSE = false;

                    CSound管理.rc演奏用タイマ.t再開();
					CDTXMania.Timer.t再開();
					CDTXMania.DTX.t全チップの再生再開();
                    CDTXMania.stage演奏ドラム画面.actAVI.tPauseControl();

					this.tDeativatePopupMenu();
					break;

				case (int) EOrder.Redoing:
                    this.bやり直しを選択した = true;
					break;

				case (int) EOrder.Return:
                    CSound管理.rc演奏用タイマ.t再開();
					CDTXMania.Timer.t再開();
                    CDTXMania.stage演奏ドラム画面.t演奏中止();
					this.tDeativatePopupMenu();
                    break;
                default:
                    break;
            }
		}

		public override void tCancel()
		{
		}

		// CActivity 実装

		public override void On活性化()
		{
			base.On活性化();
			this.bGotoDetailConfig = false;
            this.sw = new Stopwatch();
            this.nCurrentTarget = 0;
		}
		public override void On非活性化()
		{
			base.On非活性化();
		}
		public override void OnManagedリソースの作成()
		{
			if( !base.b活性化してない )
			{
				string pathパネル本体 = CSkin.Path( @"Graphics\ScreenSelect popup auto settings.png" );
				if ( File.Exists( pathパネル本体 ) )
				{
					this.txパネル本体 = CDTXMania.tテクスチャの生成( pathパネル本体, true );
				}

                this.listポーズメニュー項目 = new List<STポーズメニュー>();
                if( this.listポーズメニュー項目 != null )
                {
                    for( int i = 0; i < 3; i++ )
                    {
                        STポーズメニュー stPause = new STポーズメニュー();

                        switch( i )
                        {
                            case 0:
                                // 閉じる
                                
                                break;
                            case 1:
                                // やり直す
                                break;
                            case 2:
                                // 曲選択画面へ戻る
                                break;
                        }
                    }
                }

				base.OnManagedリソースの作成();
			}
		}
		public override void OnManagedリソースの解放()
		{
			if ( !base.b活性化してない )
			{
				CDTXMania.tテクスチャの解放( ref this.txパネル本体 );
				CDTXMania.tテクスチャの解放( ref this.tx文字列パネル );
				base.OnManagedリソースの解放();
			}
		}

        public override int On進行描画()
        {
            // 2019.02.02 kairera0467 新しいバージョン。CActSelectPopupMenuに依存していない。
            if( this.b活性化してない )
                return 0;

            if( this.b初めての進行描画 )
            {

            }


            // キー操作
            #region[ Up / Left ]

            #endregion
            #region[ Down / Right ]

            #endregion
            #region[ Enter ]

            #endregion
            #region[ Esc ]

            #endregion

            // 背景の描画


            // パネルの描画


            // 選択肢の描画


            // カーソルの描画


            #region[ デバッグ用表示 ]
            //--------------------
#if DEBUG
            int nバー基準Y = 64;
            CDTXMania.act文字コンソール.tPrint( 0, 32, C文字コンソール.Eフォント種別.白, this.nCurrentTarget.ToString() );

            for( int i = 0; i < 0; i++ )
            {

            }
#endif
            //--------------------
#endregion


            return base.On進行描画();
        }

        #region [ private ]
        //-----------------
        private int nCurrentTarget = 0;
		private List<List<List<CItemBase>>> lci;
		private enum EOrder : int
		{
			Continue,
			Redoing,
			Return, END,
			Default = 99
		};

		private CTexture txパネル本体;
		private CTexture tx文字列パネル;
        
        /// <summary>
        /// やり直しを確定した後、プレイヤーのリズムを考慮して1.5秒ほど停止させる。
        /// </summary>
        private Stopwatch sw;
        private bool bやり直しを選択した;

        protected List<STポーズメニュー> listポーズメニュー項目 = new List<STポーズメニュー>();
        // 2019.01.02 kairera0467
        protected struct STポーズメニュー
        {
            public string str項目名;
            public string str項目説明;

            public CTexture tx項目名;
            public CTexture tx説明文;
            public Eポーズメニュー種類 e項目種類;
            public Point pt項目名;
            public Rectangle rect項目説明;

            /// <summary>
            /// 項目を選択させたくない場合はfalseにすると選択できなくなります。
            /// </summary>
            public bool b選択可;
        }

        // 2019.01.02 kairera0467
        // ひとまずは基本的なセットのみ実装。
        public enum Eポーズメニュー種類
        {
            閉じる = 0,
            最初からやり直す = 1,
            曲選択に戻る = 2,
            未定義 = 99
        }
		//-----------------
		#endregion
	}


}
