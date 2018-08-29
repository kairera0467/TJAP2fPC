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
	internal class CActSelect曲リスト : CActivity
	{
		// プロパティ

		public bool bIsEnumeratingSongs
		{
			get;
			set;
		}
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
        public void n現在のアンカ難易度レベル_渡( int nCurrentLevel )
        {
            this.n現在のアンカ難易度レベル = nCurrentLevel;
        }
		public int n現在のアンカ難易度レベル 
		{
			get;
			private set;
		}
		public int n現在選択中の曲の現在の難易度レベル
		{
			get
			{
				return this.n現在のアンカ難易度レベルに最も近い難易度レベルを返す( this.r現在選択中の曲 );
			}
		}
		public Cスコア r現在選択中のスコア
		{
			get
			{
				if( this.r現在選択中の曲 != null )
				{
					return this.r現在選択中の曲.arスコア[ this.n現在選択中の曲の現在の難易度レベル ];
				}
				return null;
			}
		}
		public C曲リストノード r現在選択中の曲 
		{
			get;
			private set;
		}

		public int nスクロールバー相対y座標
		{
			get;
			private set;
		}

		// t選択曲が変更された()内で使う、直前の選曲の保持
		// (前と同じ曲なら選択曲変更に掛かる再計算を省略して高速化するため)
		private C曲リストノード song_last = null;

		
		// コンストラクタ

		public CActSelect曲リスト()
        {
            #region[ レベル数字 ]
            STレベル数字[] stレベル数字Ar = new STレベル数字[ 10 ];
            STレベル数字 st数字0 = new STレベル数字();
            STレベル数字 st数字1 = new STレベル数字();
            STレベル数字 st数字2 = new STレベル数字();
            STレベル数字 st数字3 = new STレベル数字();
            STレベル数字 st数字4 = new STレベル数字();
            STレベル数字 st数字5 = new STレベル数字();
            STレベル数字 st数字6 = new STレベル数字();
            STレベル数字 st数字7 = new STレベル数字();
            STレベル数字 st数字8 = new STレベル数字();
            STレベル数字 st数字9 = new STレベル数字();

            st数字0.ch = '0';
            st数字1.ch = '1';
            st数字2.ch = '2';
            st数字3.ch = '3';
            st数字4.ch = '4';
            st数字5.ch = '5';
            st数字6.ch = '6';
            st数字7.ch = '7';
            st数字8.ch = '8';
            st数字9.ch = '9';
            st数字0.ptX = 0;
            st数字1.ptX = 22;
            st数字2.ptX = 44;
            st数字3.ptX = 66;
            st数字4.ptX = 88;
            st数字5.ptX = 110;
            st数字6.ptX = 132;
            st数字7.ptX = 154;
            st数字8.ptX = 176;
            st数字9.ptX = 198;

            stレベル数字Ar[0] = st数字0;
            stレベル数字Ar[1] = st数字1;
            stレベル数字Ar[2] = st数字2;
            stレベル数字Ar[3] = st数字3;
            stレベル数字Ar[4] = st数字4;
            stレベル数字Ar[5] = st数字5;
            stレベル数字Ar[6] = st数字6;
            stレベル数字Ar[7] = st数字7;
            stレベル数字Ar[8] = st数字8;
            stレベル数字Ar[9] = st数字9;
            this.st小文字位置 = stレベル数字Ar;
            #endregion

            this.ct登場アニメ用 = new CCounter[ CDTXMania.Skin.nSelectSongPanelCount ];

            if( CDTXMania.Skin.eSelectLayoutType == ESelectLayout.bootleg )
                this.nパネル枚数 = 13;
            else if( CDTXMania.Skin.eSelectLayoutType == ESelectLayout.altnative )
                this.nパネル枚数 = 15;

            this.r現在選択中の曲 = null;
            this.n現在のアンカ難易度レベル = CDTXMania.ConfigIni.nDefaultCourse;
			base.b活性化してない = true;
			this.bIsEnumeratingSongs = false;
		}


		// メソッド

		public int n現在のアンカ難易度レベルに最も近い難易度レベルを返す( C曲リストノード song )
		{
			// 事前チェック。

			if( song == null )
				return this.n現在のアンカ難易度レベル;	// 曲がまったくないよ

			if( song.arスコア[ this.n現在のアンカ難易度レベル ] != null )
				return this.n現在のアンカ難易度レベル;	// 難易度ぴったりの曲があったよ

			if( ( song.eノード種別 == C曲リストノード.Eノード種別.BOX ) || ( song.eノード種別 == C曲リストノード.Eノード種別.BACKBOX ) )
				return 0;								// BOX と BACKBOX は関係無いよ


			// 現在のアンカレベルから、難易度上向きに検索開始。

			int n最も近いレベル = this.n現在のアンカ難易度レベル;

			for( int i = 0; i < 5; i++ )
			{
				if( song.arスコア[ n最も近いレベル ] != null )
					break;	// 曲があった。

				n最も近いレベル = ( n最も近いレベル + 1 ) % 5;	// 曲がなかったので次の難易度レベルへGo。（5以上になったら0に戻る。）
			}


			// 見つかった曲がアンカより下のレベルだった場合……
			// アンカから下向きに検索すれば、もっとアンカに近い曲があるんじゃね？

			if( n最も近いレベル < this.n現在のアンカ難易度レベル )
			{
				// 現在のアンカレベルから、難易度下向きに検索開始。

				n最も近いレベル = this.n現在のアンカ難易度レベル;

				for( int i = 0; i < 5; i++ )
				{
					if( song.arスコア[ n最も近いレベル ] != null )
						break;	// 曲があった。

					n最も近いレベル = ( ( n最も近いレベル - 1 ) + 5 ) % 5;	// 曲がなかったので次の難易度レベルへGo。（0未満になったら4に戻る。）
				}
			}

			return n最も近いレベル;
		}
		public C曲リストノード r指定された曲が存在するリストの先頭の曲( C曲リストノード song )
		{
			List<C曲リストノード> songList = GetSongListWithinMe( song );
			return ( songList == null ) ? null : songList[ 0 ];
		}
		public C曲リストノード r指定された曲が存在するリストの末尾の曲( C曲リストノード song )
		{
			List<C曲リストノード> songList = GetSongListWithinMe( song );
			return ( songList == null ) ? null : songList[ songList.Count - 1 ];
		}

		private List<C曲リストノード> GetSongListWithinMe( C曲リストノード song )
		{
			if ( song.r親ノード == null )					// root階層のノートだったら
			{
				return CDTXMania.Songs管理.list曲ルート;	// rootのリストを返す
			}
			else
			{
				if ( ( song.r親ノード.list子リスト != null ) && ( song.r親ノード.list子リスト.Count > 0 ) )
				{
					return song.r親ノード.list子リスト;
				}
				else
				{
					return null;
				}
			}
		}


		public delegate void DGSortFunc( List<C曲リストノード> songList, E楽器パート eInst, int order, params object[] p);
		/// <summary>
		/// 主にCSong管理.cs内にあるソート機能を、delegateで呼び出す。
		/// </summary>
		/// <param name="sf">ソート用に呼び出すメソッド</param>
		/// <param name="eInst">ソート基準とする楽器</param>
		/// <param name="order">-1=降順, 1=昇順</param>
		public void t曲リストのソート( DGSortFunc sf, E楽器パート eInst, int order, params object[] p )
		{
			List<C曲リストノード> songList = GetSongListWithinMe( this.r現在選択中の曲 );
			if ( songList == null )
			{
				// 何もしない;
			}
			else
			{
//				CDTXMania.Songs管理.t曲リストのソート3_演奏回数の多い順( songList, eInst, order );
				sf( songList, eInst, order, p );
//				this.r現在選択中の曲 = CDTXMania
				this.t現在選択中の曲を元に曲バーを再構成する();
			}
		}

		public bool tBOXに入る()
		{
//Trace.TraceInformation( "box enter" );
//Trace.TraceInformation( "Skin現在Current : " + CDTXMania.Skin.GetCurrentSkinSubfolderFullName(false) );
//Trace.TraceInformation( "Skin現在System  : " + CSkin.strSystemSkinSubfolderFullName );
//Trace.TraceInformation( "Skin現在BoxDef  : " + CSkin.strBoxDefSkinSubfolderFullName );
//Trace.TraceInformation( "Skin現在: " + CSkin.GetSkinName( CDTXMania.Skin.GetCurrentSkinSubfolderFullName(false) ) );
//Trace.TraceInformation( "Skin現pt: " + CDTXMania.Skin.GetCurrentSkinSubfolderFullName(false) );
//Trace.TraceInformation( "Skin指定: " + CSkin.GetSkinName( this.r現在選択中の曲.strSkinPath ) );
//Trace.TraceInformation( "Skinpath: " + this.r現在選択中の曲.strSkinPath );
			bool ret = false;
			if ( CSkin.GetSkinName( CDTXMania.Skin.GetCurrentSkinSubfolderFullName( false ) ) != CSkin.GetSkinName( this.r現在選択中の曲.strSkinPath )
				&& CSkin.bUseBoxDefSkin )
			{
				ret = true;
				// BOXに入るときは、スキン変更発生時のみboxdefスキン設定の更新を行う
				CDTXMania.Skin.SetCurrentSkinSubfolderFullName(
					CDTXMania.Skin.GetSkinSubfolderFullNameFromSkinName( CSkin.GetSkinName( this.r現在選択中の曲.strSkinPath ) ), false );
			}

//Trace.TraceInformation( "Skin変更: " + CSkin.GetSkinName( CDTXMania.Skin.GetCurrentSkinSubfolderFullName(false) ) );
//Trace.TraceInformation( "Skin変更Current : "+  CDTXMania.Skin.GetCurrentSkinSubfolderFullName(false) );
//Trace.TraceInformation( "Skin変更System  : "+  CSkin.strSystemSkinSubfolderFullName );
//Trace.TraceInformation( "Skin変更BoxDef  : "+  CSkin.strBoxDefSkinSubfolderFullName );

			if( ( this.r現在選択中の曲.list子リスト != null ) && ( this.r現在選択中の曲.list子リスト.Count > 0 ) )
			{
				this.r現在選択中の曲 = this.r現在選択中の曲.list子リスト[ 0 ];
				this.t現在選択中の曲を元に曲バーを再構成する();
				this.t選択曲が変更された(false);									// #27648 項目数変更を反映させる
			}
			return ret;
		}
		public bool tBOXを出る()
		{
//Trace.TraceInformation( "box exit" );
//Trace.TraceInformation( "Skin現在Current : " + CDTXMania.Skin.GetCurrentSkinSubfolderFullName(false) );
//Trace.TraceInformation( "Skin現在System  : " + CSkin.strSystemSkinSubfolderFullName );
//Trace.TraceInformation( "Skin現在BoxDef  : " + CSkin.strBoxDefSkinSubfolderFullName );
//Trace.TraceInformation( "Skin現在: " + CSkin.GetSkinName( CDTXMania.Skin.GetCurrentSkinSubfolderFullName(false) ) );
//Trace.TraceInformation( "Skin現pt: " + CDTXMania.Skin.GetCurrentSkinSubfolderFullName(false) );
//Trace.TraceInformation( "Skin指定: " + CSkin.GetSkinName( this.r現在選択中の曲.strSkinPath ) );
//Trace.TraceInformation( "Skinpath: " + this.r現在選択中の曲.strSkinPath );
			bool ret = false;
			if ( CSkin.GetSkinName( CDTXMania.Skin.GetCurrentSkinSubfolderFullName( false ) ) != CSkin.GetSkinName( this.r現在選択中の曲.strSkinPath )
				&& CSkin.bUseBoxDefSkin )
			{
				ret = true;
			}
			// スキン変更が発生しなくても、boxdef圏外に出る場合は、boxdefスキン設定の更新が必要
			// (ユーザーがboxdefスキンをConfig指定している場合への対応のために必要)
			// tBoxに入る()とは処理が微妙に異なるので注意
			CDTXMania.Skin.SetCurrentSkinSubfolderFullName(
				( this.r現在選択中の曲.strSkinPath == "" ) ? "" : CDTXMania.Skin.GetSkinSubfolderFullNameFromSkinName( CSkin.GetSkinName( this.r現在選択中の曲.strSkinPath ) ), false );
//Trace.TraceInformation( "SKIN変更: " + CSkin.GetSkinName( CDTXMania.Skin.GetCurrentSkinSubfolderFullName(false) ) );
//Trace.TraceInformation( "SKIN変更Current : "+  CDTXMania.Skin.GetCurrentSkinSubfolderFullName(false) );
//Trace.TraceInformation( "SKIN変更System  : "+  CSkin.strSystemSkinSubfolderFullName );
//Trace.TraceInformation( "SKIN変更BoxDef  : "+  CSkin.strBoxDefSkinSubfolderFullName );
			if ( this.r現在選択中の曲.r親ノード != null )
			{
				this.r現在選択中の曲 = this.r現在選択中の曲.r親ノード;
				this.t現在選択中の曲を元に曲バーを再構成する();
				this.t選択曲が変更された(false);									// #27648 項目数変更を反映させる
			}
			return ret;
		}
		public void t現在選択中の曲を元に曲バーを再構成する()
		{
			this.tバーの初期化();
			//for( int i = 0; i < 13; i++ )
			//{
			//	this.t曲名バーの生成( i, this.stバー情報[ i ].strタイトル文字列, this.stバー情報[ i ].col文字色 );
			//}
		}
		public void t次に移動()
		{
			if( this.r現在選択中の曲 != null )
			{
				this.n目標のスクロールカウンタ += 100;
			}
		}
		public void t前に移動()
		{
			if( this.r現在選択中の曲 != null )
			{
				this.n目標のスクロールカウンタ -= 100;
			}
		}
		public void t難易度レベルをひとつ進める()
		{
			if( ( this.r現在選択中の曲 == null ) || ( this.r現在選択中の曲.nスコア数 <= 1 ) )
				return;		// 曲にスコアが０～１個しかないなら進める意味なし。
			

			// 難易度レベルを＋１し、現在選曲中のスコアを変更する。

			this.n現在のアンカ難易度レベル = this.n現在のアンカ難易度レベルに最も近い難易度レベルを返す( this.r現在選択中の曲 );

			for( int i = 0; i < 5; i++ )
			{
				this.n現在のアンカ難易度レベル = ( this.n現在のアンカ難易度レベル + 1 ) % 5;	// ５以上になったら０に戻る。
				if( this.r現在選択中の曲.arスコア[ this.n現在のアンカ難易度レベル ] != null )	// 曲が存在してるならここで終了。存在してないなら次のレベルへGo。
					break;
			}

            int panelcount = CDTXMania.Skin.nSelectSongPanelCount;
            int panelcount_half = CDTXMania.Skin.nSelectSongPanelCount / 2;

			// 曲毎に表示しているスキル値を、新しい難易度レベルに合わせて取得し直す。（表示されている13曲全部。）

			C曲リストノード song = this.r現在選択中の曲;
			for( int i = 0; i < 5; i++ )
				song = this.r前の曲( song );

			for( int i = this.n現在の選択行 - 6; i < ( ( this.n現在の選択行 - 6 ) + panelcount ); i++ )
			{
				int index = ( i + panelcount ) % panelcount;
				for( int m = 0; m < 3; m++ )
				{
					this.stバー情報[ index ].nスキル値[ m ] = (int) song.arスコア[ this.n現在のアンカ難易度レベルに最も近い難易度レベルを返す( song ) ].譜面情報.最大スキル[ m ];
				}
				song = this.r次の曲( song );
			}


			// 選曲ステージに変更通知を発出し、関係Activityの対応を行ってもらう。

			CDTXMania.stage選曲.t選択曲変更通知();
		}
        /// <summary>
        /// 不便だったから作った。
        /// </summary>
		public void t難易度レベルをひとつ戻す()
		{
			if( ( this.r現在選択中の曲 == null ) || ( this.r現在選択中の曲.nスコア数 <= 1 ) )
				return;		// 曲にスコアが０～１個しかないなら進める意味なし。
			

			// 難易度レベルを＋１し、現在選曲中のスコアを変更する。

			this.n現在のアンカ難易度レベル = this.n現在のアンカ難易度レベルに最も近い難易度レベルを返す( this.r現在選択中の曲 );

            this.n現在のアンカ難易度レベル--;
            if( this.n現在のアンカ難易度レベル < 0 ) // 0より下になったら4に戻す。
            {
                this.n現在のアンカ難易度レベル = 4;
            }

            //2016.08.13 kairera0467 かんたん譜面が無い譜面(ふつう、むずかしいのみ)で、難易度を最上位に戻せない不具合の修正。
            bool bLabel0NotFound = true;
            for( int i = this.n現在のアンカ難易度レベル; i >= 0; i-- )
            {
                if( this.r現在選択中の曲.arスコア[ i ] != null )
                {
                    this.n現在のアンカ難易度レベル = i;
                    bLabel0NotFound = false;
                    break;
                }
            }
            if( bLabel0NotFound )
            {
                for( int i = 4; i >= 0; i-- )
                {
                    if( this.r現在選択中の曲.arスコア[ i ] != null )
                    {
                        this.n現在のアンカ難易度レベル = i;
                        break;
                    }
                }
            }

            int panelcount = CDTXMania.Skin.nSelectSongPanelCount;
            int panelcount_half = CDTXMania.Skin.nSelectSongPanelCount / 2;

            // 曲毎に表示しているスキル値を、新しい難易度レベルに合わせて取得し直す。（表示されている13曲全部。）

            C曲リストノード song = this.r現在選択中の曲;
			for( int i = 0; i < 5; i++ )
				song = this.r前の曲( song );

			for( int i = this.n現在の選択行 - 6; i < ( ( this.n現在の選択行 - 6 ) + panelcount ); i++ )
			{
				int index = ( i + panelcount ) % panelcount;
				for( int m = 0; m < 3; m++ )
				{
					this.stバー情報[ index ].nスキル値[ m ] = (int) song.arスコア[ this.n現在のアンカ難易度レベルに最も近い難易度レベルを返す( song ) ].譜面情報.最大スキル[ m ];
				}
				song = this.r次の曲( song );
			}


			// 選曲ステージに変更通知を発出し、関係Activityの対応を行ってもらう。

			CDTXMania.stage選曲.t選択曲変更通知();
		}


		/// <summary>
		/// 曲リストをリセットする
		/// </summary>
		/// <param name="cs"></param>
		public void Refresh(CSongs管理 cs, bool bRemakeSongTitleBar )		// #26070 2012.2.28 yyagi
		{
			if ( cs != null && cs.list曲ルート.Count > 0 )	// 新しい曲リストを検索して、1曲以上あった
			{
                this.On非活性化();
				CDTXMania.Songs管理 = cs;
                this.On活性化();

				if ( this.r現在選択中の曲 != null )			// r現在選択中の曲==null とは、「最初songlist.dbが無かった or 検索したが1曲もない」
				{
					this.r現在選択中の曲 = searchCurrentBreadcrumbsPosition( CDTXMania.Songs管理.list曲ルート, this.r現在選択中の曲.strBreadcrumbs );
					if ( bRemakeSongTitleBar )					// 選曲画面以外に居るときには再構成しない (非活性化しているときに実行すると例外となる)
					{
						this.t現在選択中の曲を元に曲バーを再構成する();
					}
#if false			// list子リストの中まではmatchしてくれないので、検索ロジックは手書きで実装 (searchCurrentBreadcrumbs())
					string bc = this.r現在選択中の曲.strBreadcrumbs;
					Predicate<C曲リストノード> match = delegate( C曲リストノード c )
					{
						return ( c.strBreadcrumbs.Equals( bc ) );
					};
					int nMatched = CDTXMania.Songs管理.list曲ルート.FindIndex( match );

					this.r現在選択中の曲 = ( nMatched == -1 ) ? null : CDTXMania.Songs管理.list曲ルート[ nMatched ];
					this.t現在選択中の曲を元に曲バーを再構成する();
#endif
					return;
				}
			}
			//this.On非活性化();
			this.r現在選択中の曲 = null;
			//this.On活性化();
		}


		/// <summary>
		/// 現在選曲している位置を検索する
		/// (曲一覧クラスを新しいものに入れ替える際に用いる)
		/// </summary>
		/// <param name="ln">検索対象のList</param>
		/// <param name="bc">検索するパンくずリスト(文字列)</param>
		/// <returns></returns>
		private C曲リストノード searchCurrentBreadcrumbsPosition( List<C曲リストノード> ln, string bc )
		{
			foreach (C曲リストノード n in ln)
			{
				if ( n.strBreadcrumbs == bc )
				{
					return n;
				}
				else if ( n.list子リスト != null && n.list子リスト.Count > 0 )	// 子リストが存在するなら、再帰で探す
				{
					C曲リストノード r = searchCurrentBreadcrumbsPosition( n.list子リスト, bc );
					if ( r != null ) return r;
				}
			}
			return null;
		}

		/// <summary>
		/// BOXのアイテム数と、今何番目を選択しているかをセットする
		/// </summary>
		public void t選択曲が変更された( bool bForce )	// #27648
		{
			C曲リストノード song = CDTXMania.stage選曲.r現在選択中の曲;
			if ( song == null )
				return;
			if ( song == song_last && bForce == false )
				return;
				
			song_last = song;
			List<C曲リストノード> list = ( song.r親ノード != null ) ? song.r親ノード.list子リスト : CDTXMania.Songs管理.list曲ルート;
			int index = list.IndexOf( song ) + 1;
			if ( index <= 0 )
			{
				nCurrentPosition = nNumOfItems = 0;
			}
			else
			{
				nCurrentPosition = index;
				nNumOfItems = list.Count;
			}
            CDTXMania.stage選曲.act演奏履歴パネル.tSongChange();
		}



		// CActivity 実装

		public override void On活性化()
		{
			if( this.b活性化してる )
				return;

            //「tバーの初期化」より先に生成すること
            if( !string.IsNullOrEmpty( CDTXMania.ConfigIni.strPrivateFontで使うフォント名 ) )
            {
                this.pfMusicName = new CPrivateFastFont( new FontFamily( CDTXMania.ConfigIni.strPrivateFontで使うフォント名 ), CDTXMania.Skin.fSelectSongFontSize );
                this.pfSubtitle = new CPrivateFastFont( new FontFamily( CDTXMania.ConfigIni.strPrivateFontで使うフォント名 ), CDTXMania.Skin.fSelectSongFontSizeSub );
            }
            else
            {
                this.pfMusicName = new CPrivateFastFont( new FontFamily( "MS PGothic" ), CDTXMania.Skin.fSelectSongFontSize );
                this.pfSubtitle = new CPrivateFastFont( new FontFamily( "MS PGothic" ), CDTXMania.Skin.fSelectSongFontSizeSub );
            }

			this.e楽器パート = E楽器パート.DRUMS;
			this.b登場アニメ全部完了 = false;
			this.n目標のスクロールカウンタ = 0;
			this.n現在のスクロールカウンタ = 0;
			this.nスクロールタイマ = -1;

			// フォント作成。
			// 曲リスト文字は２倍（面積４倍）でテクスチャに描画してから縮小表示するので、フォントサイズは２倍とする。

			FontStyle regular = FontStyle.Regular;
			if( CDTXMania.ConfigIni.b選曲リストフォントを斜体にする ) regular |= FontStyle.Italic;
			if( CDTXMania.ConfigIni.b選曲リストフォントを太字にする ) regular |= FontStyle.Bold;
			this.ft曲リスト用フォント = new Font( CDTXMania.ConfigIni.str選曲リストフォント, (float) ( CDTXMania.ConfigIni.n選曲リストフォントのサイズdot * 2 ), regular, GraphicsUnit.Pixel );
			

			// 現在選択中の曲がない（＝はじめての活性化）なら、現在選択中の曲をルートの先頭ノードに設定する。

			if( ( this.r現在選択中の曲 == null ) && ( CDTXMania.Songs管理.list曲ルート.Count > 0 ) )
				this.r現在選択中の曲 = CDTXMania.Songs管理.list曲ルート[ 0 ];

			// バー情報を初期化する。

			this.tバーの初期化();

            this.ct三角矢印アニメ = new CCounter();

			base.On活性化();

			this.t選択曲が変更された(true);		// #27648 2012.3.31 yyagi 選曲画面に入った直後の 現在位置/全アイテム数 の表示を正しく行うため
		}
		public override void On非活性化()
		{
			if( this.b活性化してない )
				return;

			CDTXMania.t安全にDisposeする( ref this.ft曲リスト用フォント );

			for( int i = 0; i < this.ct登場アニメ用.Length; i++ )
				this.ct登場アニメ用[ i ] = null;

            this.ct三角矢印アニメ = null;

			base.On非活性化();
		}
		public override void OnManagedリソースの作成()
		{
			if( !this.b活性化してない )
            {


                CDTXMania.tオブジェクトを確認してテクスチャを生成( ref this.tx曲バー_難易度[ 0 ], CSkin.Path( @"Graphics\5_songboard_Easy.png" ), false, false );
                CDTXMania.tオブジェクトを確認してテクスチャを生成( ref this.tx曲バー_難易度[ 1 ], CSkin.Path( @"Graphics\5_songboard_Normal.png" ), false, false );
                CDTXMania.tオブジェクトを確認してテクスチャを生成( ref this.tx曲バー_難易度[ 2 ], CSkin.Path( @"Graphics\5_songboard_Hard.png" ), false, false );
                CDTXMania.tオブジェクトを確認してテクスチャを生成( ref this.tx曲バー_難易度[ 3 ], CSkin.Path( @"Graphics\5_songboard_Master.png" ), false, false );
                CDTXMania.tオブジェクトを確認してテクスチャを生成( ref this.tx曲バー_難易度[ 4 ], CSkin.Path( @"Graphics\5_songboard_Edit.png" ), false, false );

                CDTXMania.tオブジェクトを確認してテクスチャを生成( ref this.tx難易度星, CSkin.Path( @"Graphics\5_levelstar.png" ), false, false );
                //CDTXMania.tオブジェクトを確認してテクスチャを生成( ref this.tx難易度パネル, CSkin.Path( @"Graphics\5_level_panel.png" ), false, false );
                CDTXMania.tオブジェクトを確認してテクスチャを生成( ref this.tx譜面分岐曲バー用, CSkin.Path( @"Graphics\5_songboard_branch.png" ), false, false );
                CDTXMania.tオブジェクトを確認してテクスチャを生成( ref this.tx譜面分岐中央パネル用, CSkin.Path( @"Graphics\5_center panel_branch.png" ), false, false );

                CDTXMania.tオブジェクトを確認してテクスチャを生成( ref this.tx上部ジャンル名, CSkin.Path( @"Graphics\5_genrename.png" ), false, false );
                CDTXMania.tオブジェクトを確認してテクスチャを生成( ref this.txレベル数字フォント, CSkin.Path( @"Graphics\5_levelfont.png" ), false, false );

                CDTXMania.tオブジェクトを確認してテクスチャを生成( ref this.txカーソル左, CSkin.Path( @"Graphics\5_cursor left.png" ), false, false );
                CDTXMania.tオブジェクトを確認してテクスチャを生成( ref this.txカーソル右, CSkin.Path( @"Graphics\5_cursor right.png" ), false, false );

                CDTXMania.tオブジェクトを確認してテクスチャを生成( ref this.txバー中央, CSkin.Path( @"Graphics\5_center panel.png" ), false, false );  

                CDTXMania.tオブジェクトを確認してテクスチャを生成( ref this.tx曲バー_JPOP, CSkin.Path( @"Graphics\5_songboard_JPOP.png" ), false, false );
                CDTXMania.tオブジェクトを確認してテクスチャを生成( ref this.tx曲バー_アニメ, CSkin.Path( @"Graphics\5_songboard_anime.png" ), false, false );
                CDTXMania.tオブジェクトを確認してテクスチャを生成( ref this.tx曲バー_ゲーム, CSkin.Path( @"Graphics\5_songboard_game.png" ), false, false );
                CDTXMania.tオブジェクトを確認してテクスチャを生成( ref this.tx曲バー_ナムコ, CSkin.Path( @"Graphics\5_songboard_namco.png" ), false, false );
                CDTXMania.tオブジェクトを確認してテクスチャを生成( ref this.tx曲バー_クラシック, CSkin.Path( @"Graphics\5_songboard_classic.png" ), false, false );
                CDTXMania.tオブジェクトを確認してテクスチャを生成( ref this.tx曲バー_バラエティ, CSkin.Path( @"Graphics\5_songboard_variety.png" ), false, false );
                CDTXMania.tオブジェクトを確認してテクスチャを生成( ref this.tx曲バー_どうよう, CSkin.Path( @"Graphics\5_songboard_child.png" ), false, false );
                CDTXMania.tオブジェクトを確認してテクスチャを生成( ref this.tx曲バー_ボカロ, CSkin.Path( @"Graphics\5_songboard_vocaloid.png" ), false, false );
                CDTXMania.tオブジェクトを確認してテクスチャを生成( ref this.tx曲バー, CSkin.Path( @"Graphics\5_songboard.png" ), false, false );
                //2018.7.7 kairera0467 リリース時には戻します
                if( CDTXMania.Skin.eDiffSelectMode == EDiffSelectMode.曲から選ぶ )
                {
                    CDTXMania.tオブジェクトを確認してテクスチャを生成( ref this.txバー中央_アニメ中, CSkin.Path( @"Graphics\5_中央パネルアニメ中.png" ), false, false );
                    CDTXMania.tオブジェクトを確認してテクスチャを生成( ref this.tx難易度パネル, CSkin.Path( @"Graphics\5_中央パネル難易度看板素材.png" ), false, false );
                    CDTXMania.tオブジェクトを確認してテクスチャを生成( ref this.tx難易度文字中央パネル用, CSkin.Path( @"Graphics\5_難易度看板文字.png" ), false, false );
                    CDTXMania.tオブジェクトを確認してテクスチャを生成( ref this.tx難易度アイコン, CSkin.Path( @"Graphics\5_center difficon.png" ), false, false );
                }
                else
                {
                    CDTXMania.tオブジェクトを確認してテクスチャを生成( ref this.tx難易度パネル, CSkin.Path( @"Graphics\5_level_panel.png" ), false, false );
                }

                int c = ( CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "ja" ) ? 0 : 1;
            
			    #region [ Songs not found画像 ]
			    try
			    {
				    using( Bitmap image = new Bitmap( 640, 128 ) )
				    using( Graphics graphics = Graphics.FromImage( image ) )
				    {
					    string[] s1 = { "曲データが見つかりません。", "Songs not found." };
					    string[] s2 = { "曲データをDTXManiaGR.exe以下の", "You need to install songs." };
					    string[] s3 = { "フォルダにインストールして下さい。", "" };
					    graphics.DrawString( s1[c], this.ft曲リスト用フォント, Brushes.DarkGray, (float) 2f, (float) 2f );
					    graphics.DrawString( s1[c], this.ft曲リスト用フォント, Brushes.White, (float) 0f, (float) 0f );
					    graphics.DrawString( s2[c], this.ft曲リスト用フォント, Brushes.DarkGray, (float) 2f, (float) 44f );
					    graphics.DrawString( s2[c], this.ft曲リスト用フォント, Brushes.White, (float) 0f, (float) 42f );
					    graphics.DrawString( s3[c], this.ft曲リスト用フォント, Brushes.DarkGray, (float) 2f, (float) 86f );
					    graphics.DrawString( s3[c], this.ft曲リスト用フォント, Brushes.White, (float) 0f, (float) 84f );

					    this.txSongNotFound = new CTexture( CDTXMania.app.Device, image, CDTXMania.TextureFormat );

					    this.txSongNotFound.vc拡大縮小倍率 = new Vector3( 0.5f, 0.5f, 1f );	// 半分のサイズで表示する。
				    }
			    }
			    catch( CTextureCreateFailedException )
			    {
				    Trace.TraceError( "SoungNotFoundテクスチャの作成に失敗しました。" );
				    this.txSongNotFound = null;
			    }
			    #endregion
			    #region [ "曲データを検索しています"画像 ]
			    try
			    {
				    using ( Bitmap image = new Bitmap( 640, 96 ) )
				    using ( Graphics graphics = Graphics.FromImage( image ) )
				    {
					    string[] s1 = { "曲データを検索しています。", "Now enumerating songs." };
					    string[] s2 = { "そのまましばらくお待ち下さい。", "Please wait..." };
					    graphics.DrawString( s1[c], this.ft曲リスト用フォント, Brushes.DarkGray, (float) 2f, (float) 2f );
					    graphics.DrawString( s1[c], this.ft曲リスト用フォント, Brushes.White, (float) 0f, (float) 0f );
					    graphics.DrawString( s2[c], this.ft曲リスト用フォント, Brushes.DarkGray, (float) 2f, (float) 44f );
					    graphics.DrawString( s2[c], this.ft曲リスト用フォント, Brushes.White, (float) 0f, (float) 42f );

					    this.txEnumeratingSongs = new CTexture( CDTXMania.app.Device, image, CDTXMania.TextureFormat );

					    this.txEnumeratingSongs.vc拡大縮小倍率 = new Vector3( 0.5f, 0.5f, 1f );	// 半分のサイズで表示する。
				    }
			    }
			    catch ( CTextureCreateFailedException )
			    {
				    Trace.TraceError( "txEnumeratingSongsテクスチャの作成に失敗しました。" );
				    this.txEnumeratingSongs = null;
			    }
                #endregion
                #region [ 曲数表示 ]
                //this.txアイテム数数字 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\ScreenSelect skill number on gauge etc.png" ), false );
                #endregion

                //最初の生成
                for( int i = 0; i < this.stバー情報.Length; i++ )
                {
                    if( !String.IsNullOrEmpty( this.stバー情報[ i ].strタイトル文字列 ) )
                    {
                        this.stバー情報[ i ].txタイトル?.Dispose();
                        this.stバー情報[ i ].txタイトル = this.t曲名テクスチャを生成する( this.stバー情報[ i ].strタイトル文字列 );
                    }
                }

                #region[ 以前のテクスチャ生成 ]
                //this.tx曲バー_JPOP = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_songboard_JPOP.png" ), false );
                //this.tx曲バー_アニメ = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_songboard_anime.png" ), false );
                //this.tx曲バー_ゲーム = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_songboard_game.png" ), false );
                //this.tx曲バー_ナムコ = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_songboard_namco.png" ), false );
                //this.tx曲バー_クラシック = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_songboard_classic.png" ), false );
                //this.tx曲バー_バラエティ = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_songboard_variety.png" ), false );
                //this.tx曲バー_どうよう = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_songboard_child.png" ), false );
                //this.tx曲バー_ボカロ = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_songboard_vocaloid.png" ), false );
                //this.tx曲バー = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_songboard.png" ), false );

                //this.tx曲バー_難易度[0] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_songboard_Easy.png" ) );
                //this.tx曲バー_難易度[1] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_songboard_Normal.png" ) );
                //this.tx曲バー_難易度[2] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_songboard_Hard.png" ) );
                //this.tx曲バー_難易度[3] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_songboard_Master.png" ) );
                //this.tx曲バー_難易度[4] = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_songboard_Edit.png" ) );

                //this.tx難易度星 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_levelstar.png" ), false );
                //this.tx難易度パネル = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_level_panel.png" ), false );
                //this.tx譜面分岐曲バー用 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_songboard_branch.png" ) );
                //this.tx譜面分岐中央パネル用 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_center panel_branch.png" ) );
                //this.txバー中央 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_center panel.png" ) );
                //this.tx上部ジャンル名 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_genrename.png" ) );
                //this.txレベル数字フォント = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_levelfont.png" ) );

                //this.txカーソル左 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_cursor left.png" ) );
                //this.txカーソル右 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\5_cursor right.png" ) );

                //int c = ( CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "ja" ) ? 0 : 1;
            
			    #region [ Songs not found画像 ]
			    //try
			    //{
				   // using( Bitmap image = new Bitmap( 640, 128 ) )
				   // using( Graphics graphics = Graphics.FromImage( image ) )
				   // {
					  //  string[] s1 = { "曲データが見つかりません。", "Songs not found." };
					  //  string[] s2 = { "曲データをDTXManiaGR.exe以下の", "You need to install songs." };
					  //  string[] s3 = { "フォルダにインストールして下さい。", "" };
					  //  graphics.DrawString( s1[c], this.ft曲リスト用フォント, Brushes.DarkGray, (float) 2f, (float) 2f );
					  //  graphics.DrawString( s1[c], this.ft曲リスト用フォント, Brushes.White, (float) 0f, (float) 0f );
					  //  graphics.DrawString( s2[c], this.ft曲リスト用フォント, Brushes.DarkGray, (float) 2f, (float) 44f );
					  //  graphics.DrawString( s2[c], this.ft曲リスト用フォント, Brushes.White, (float) 0f, (float) 42f );
					  //  graphics.DrawString( s3[c], this.ft曲リスト用フォント, Brushes.DarkGray, (float) 2f, (float) 86f );
					  //  graphics.DrawString( s3[c], this.ft曲リスト用フォント, Brushes.White, (float) 0f, (float) 84f );

					  //  this.txSongNotFound = new CTexture( CDTXMania.app.Device, image, CDTXMania.TextureFormat );

					  //  this.txSongNotFound.vc拡大縮小倍率 = new Vector3( 0.5f, 0.5f, 1f );	// 半分のサイズで表示する。
				   // }
			    //}
			    //catch( CTextureCreateFailedException )
			    //{
				   // Trace.TraceError( "SoungNotFoundテクスチャの作成に失敗しました。" );
				   // this.txSongNotFound = null;
			    //}
			    #endregion
			    #region [ "曲データを検索しています"画像 ]
			    //try
			    //{
				   // using ( Bitmap image = new Bitmap( 640, 96 ) )
				   // using ( Graphics graphics = Graphics.FromImage( image ) )
				   // {
					  //  string[] s1 = { "曲データを検索しています。", "Now enumerating songs." };
					  //  string[] s2 = { "そのまましばらくお待ち下さい。", "Please wait..." };
					  //  graphics.DrawString( s1[c], this.ft曲リスト用フォント, Brushes.DarkGray, (float) 2f, (float) 2f );
					  //  graphics.DrawString( s1[c], this.ft曲リスト用フォント, Brushes.White, (float) 0f, (float) 0f );
					  //  graphics.DrawString( s2[c], this.ft曲リスト用フォント, Brushes.DarkGray, (float) 2f, (float) 44f );
					  //  graphics.DrawString( s2[c], this.ft曲リスト用フォント, Brushes.White, (float) 0f, (float) 42f );

					  //  this.txEnumeratingSongs = new CTexture( CDTXMania.app.Device, image, CDTXMania.TextureFormat );

					  //  this.txEnumeratingSongs.vc拡大縮小倍率 = new Vector3( 0.5f, 0.5f, 1f );	// 半分のサイズで表示する。
				   // }
			    //}
			    //catch ( CTextureCreateFailedException )
			    //{
				   // Trace.TraceError( "txEnumeratingSongsテクスチャの作成に失敗しました。" );
				   // this.txEnumeratingSongs = null;
			    //}
                #endregion
                #region [ 曲数表示 ]
                //this.txアイテム数数字 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\ScreenSelect skill number on gauge etc.png" ), false );
                #endregion
                #endregion


			    base.OnManagedリソースの作成();
            }
		}
		public override void OnManagedリソースの解放()
		{
			if( this.b活性化してない )
				return;

			//CDTXMania.t安全にDisposeする( ref this.txアイテム数数字 );

			for( int i = 0; i < this.stバー情報.Length; i++ )
            {
                CDTXMania.t安全にDisposeする( ref this.stバー情報[ i ].txタイトル );
            }
            CDTXMania.t安全にDisposeする( ref this.tx選択している曲の曲名 );
            CDTXMania.t安全にDisposeする( ref this.tx選択している曲のサブタイトル );

			//CDTXMania.t安全にDisposeする( ref this.txスキル数字 );
			CDTXMania.t安全にDisposeする( ref this.txEnumeratingSongs );
			CDTXMania.t安全にDisposeする( ref this.txSongNotFound );

			CDTXMania.t安全にDisposeする( ref this.tx曲バー_JPOP );
			CDTXMania.t安全にDisposeする( ref this.tx曲バー_アニメ );
			CDTXMania.t安全にDisposeする( ref this.tx曲バー_ゲーム );
			CDTXMania.t安全にDisposeする( ref this.tx曲バー_ナムコ );
			CDTXMania.t安全にDisposeする( ref this.tx曲バー_クラシック );
			CDTXMania.t安全にDisposeする( ref this.tx曲バー_どうよう );
			CDTXMania.t安全にDisposeする( ref this.tx曲バー_バラエティ );
			CDTXMania.t安全にDisposeする( ref this.tx曲バー_ボカロ );
            CDTXMania.t安全にDisposeする( ref this.tx曲バー );
            CDTXMania.t安全にDisposeする( ref this.tx譜面分岐曲バー用 );

       		for( int i = 0; i < 5; i++ )
            {
				CDTXMania.t安全にDisposeする( ref this.tx曲バー_難易度[ i ] );
            }

            CDTXMania.tテクスチャの解放( ref this.tx難易度パネル );
            CDTXMania.tテクスチャの解放( ref this.txバー中央 );
            CDTXMania.tテクスチャの解放( ref this.txバー中央_アニメ中 );
            CDTXMania.tテクスチャの解放( ref this.tx難易度アイコン );
            CDTXMania.tテクスチャの解放( ref this.tx難易度星 );
            CDTXMania.tテクスチャの解放( ref this.tx難易度文字中央パネル用 );
            CDTXMania.tテクスチャの解放( ref this.tx譜面分岐中央パネル用 );
            CDTXMania.tテクスチャの解放( ref this.tx上部ジャンル名 );
            CDTXMania.tテクスチャの解放( ref this.txレベル数字フォント );

            CDTXMania.tテクスチャの解放( ref this.txカーソル左 );
            CDTXMania.tテクスチャの解放( ref this.txカーソル右 );

            CDTXMania.tテクスチャの解放( ref this.txMusicName );

            CDTXMania.t安全にDisposeする( ref this.pfMusicName );
            CDTXMania.t安全にDisposeする( ref this.pfSubtitle );

			base.OnManagedリソースの解放();
		}
		public override int On進行描画()
		{
			if( this.b活性化してない )
				return 0;

            int panelcount = CDTXMania.Skin.nSelectSongPanelCount;
            int panelcount_half = 5; //CDTXMania.Skin.nSelectSongPanelCount / 2;
			#region [ 初めての進行描画 ]
			//-----------------
			if( this.b初めての進行描画 )
			{
				for( int i = 0; i < panelcount; i++ )
					this.ct登場アニメ用[ i ] = new CCounter( -i * 10, 100, 3, CDTXMania.Timer );

				this.nスクロールタイマ = CSound管理.rc演奏用タイマ.n現在時刻;
				CDTXMania.stage選曲.t選択曲変更通知();

                this.n矢印スクロール用タイマ値 = CSound管理.rc演奏用タイマ.n現在時刻;
				this.ct三角矢印アニメ.t開始( 0, 19, 40, CDTXMania.Timer );
				
				base.b初めての進行描画 = false;
			}
			//-----------------
			#endregion

			
			// まだ選択中の曲が決まってなければ、曲ツリールートの最初の曲にセットする。

			if( ( this.r現在選択中の曲 == null ) && ( CDTXMania.Songs管理.list曲ルート.Count > 0 ) )
				this.r現在選択中の曲 = CDTXMania.Songs管理.list曲ルート[ 0 ];


			// 本ステージは、(1)登場アニメフェーズ → (2)通常フェーズ　と二段階にわけて進む。
			// ２つしかフェーズがないので CStage.eフェーズID を使ってないところがまた本末転倒。

			
			// 進行。
            this.ct三角矢印アニメ.t進行Loop();


			if( !this.b登場アニメ全部完了 )
			{
				#region [ (1) 登場アニメフェーズの進行。]
				//-----------------
				for( int i = 0; i < panelcount; i++ )	// パネルは全13枚。
				{
					this.ct登場アニメ用[ i ].t進行();

					if( this.ct登場アニメ用[ i ].b終了値に達した )
						this.ct登場アニメ用[ i ].t停止();
				}

				// 全部の進行が終わったら、this.b登場アニメ全部完了 を true にする。

				this.b登場アニメ全部完了 = true;
				for( int i = 0; i < panelcount; i++ )	// パネルは全13枚。
				{
					if( this.ct登場アニメ用[ i ].b進行中 )
					{
						this.b登場アニメ全部完了 = false;	// まだ進行中のアニメがあるなら false のまま。
						break;
					}
				}
				//-----------------
				#endregion
			}
			else
			{
				#region [ (2) 通常フェーズの進行。]
				//-----------------
				long n現在時刻 = CSound管理.rc演奏用タイマ.n現在時刻;
				
				if( n現在時刻 < this.nスクロールタイマ )	// 念のため
					this.nスクロールタイマ = n現在時刻;

				const int nアニメ間隔 = 2;
				while( ( n現在時刻 - this.nスクロールタイマ ) >= nアニメ間隔 )
				{
					int n加速度 = 1;
					int n残距離 = Math.Abs( (int) ( this.n目標のスクロールカウンタ - this.n現在のスクロールカウンタ ) );

					#region [ 残距離が遠いほどスクロールを速くする（＝n加速度を多くする）。]
					//-----------------
					if( n残距離 <= 100 )
					{
						n加速度 = 2;
					}
					else if( n残距離 <= 300 )
					{
						n加速度 = 3;
					}
					else if( n残距離 <= 500 )
					{
						n加速度 = 4;
					}
					else
					{
						n加速度 = 8;
					}
					//-----------------
					#endregion

					#region [ 加速度を加算し、現在のスクロールカウンタを目標のスクロールカウンタまで近づける。 ]
					//-----------------
					if( this.n現在のスクロールカウンタ < this.n目標のスクロールカウンタ )		// (A) 正の方向に未達の場合：
					{
						this.n現在のスクロールカウンタ += n加速度;								// カウンタを正方向に移動する。

						if( this.n現在のスクロールカウンタ > this.n目標のスクロールカウンタ )
							this.n現在のスクロールカウンタ = this.n目標のスクロールカウンタ;	// 到着！スクロール停止！
					}

					else if( this.n現在のスクロールカウンタ > this.n目標のスクロールカウンタ )	// (B) 負の方向に未達の場合：
					{
						this.n現在のスクロールカウンタ -= n加速度;								// カウンタを負方向に移動する。

						if( this.n現在のスクロールカウンタ < this.n目標のスクロールカウンタ )	// 到着！スクロール停止！
							this.n現在のスクロールカウンタ = this.n目標のスクロールカウンタ;
					}
					//-----------------
					#endregion

					if( this.n現在のスクロールカウンタ >= 100 )		// １行＝100カウント。
					{
						#region [ パネルを１行上にシフトする。]
						//-----------------

						// 選択曲と選択行を１つ下の行に移動。

						this.r現在選択中の曲 = this.r次の曲( this.r現在選択中の曲 );
						this.n現在の選択行 = ( this.n現在の選択行 + 1 ) % panelcount;


						// 選択曲から７つ下のパネル（＝新しく最下部に表示されるパネル。消えてしまう一番上のパネルを再利用する）に、新しい曲の情報を記載する。

						C曲リストノード song = this.r現在選択中の曲;
						for( int i = 0; i < 6; i++ )
							song = this.r次の曲( song );

						int index = ( this.n現在の選択行 + 6 ) % panelcount;	// 新しく最下部に表示されるパネルのインデックス（0～12）。
						this.stバー情報[ index ].strタイトル文字列 = song.strタイトル;
						this.stバー情報[ index ].col文字色 = song.col文字色;
                        this.stバー情報[ index ].strジャンル = song.strジャンル;
                        this.stバー情報[ index ].strサブタイトル = song.strサブタイトル;
                        this.stバー情報[ index ].ar難易度 = song.nLevel;
                        for( int f = 0; f < 5; f++ )
                        {
                            if( song.arスコア[ f ] != null )
                                this.stバー情報[ index ].b分岐 = song.arスコア[ f ].譜面情報.b譜面分岐;
                        }


						// stバー情報[] の内容を1行ずつずらす。
						
						C曲リストノード song2 = this.r現在選択中の曲;
						for( int i = 0; i < 6; i++ )
							song2 = this.r前の曲( song2 );

						for( int i = 0; i < panelcount; i++ )
						{
							int n = ( ( ( this.n現在の選択行 - 6 ) + i ) + panelcount ) % panelcount;
							this.stバー情報[ n ].eバー種別 = this.e曲のバー種別を返す( song2 );
							song2 = this.r次の曲( song2 );
                            this.stバー情報[ i ].txタイトル?.Dispose();
                            this.stバー情報[ i ].txタイトル = this.t曲名テクスチャを生成する( this.stバー情報[ i ].strタイトル文字列 );
						}

						
						// 新しく最下部に表示されるパネル用のスキル値を取得。

						for( int i = 0; i < 3; i++ )
							this.stバー情報[ index ].nスキル値[ i ] = (int) song.arスコア[ this.n現在のアンカ難易度レベルに最も近い難易度レベルを返す( song ) ].譜面情報.最大スキル[ i ];


						// 1行(100カウント)移動完了。

						this.n現在のスクロールカウンタ -= 100;
						this.n目標のスクロールカウンタ -= 100;

						this.t選択曲が変更された(false);				// スクロールバー用に今何番目を選択しているかを更新

                        this.tx選択している曲の曲名?.Dispose();
                        this.tx選択している曲の曲名 = null;
                        this.tx選択している曲のサブタイトル?.Dispose();
                        this.tx選択している曲のサブタイトル = null;
                        
						if( this.n目標のスクロールカウンタ == 0 )
							CDTXMania.stage選曲.t選択曲変更通知();		// スクロール完了＝選択曲変更！

						//-----------------
						#endregion
					}
					else if( this.n現在のスクロールカウンタ <= -100 )
					{
						#region [ パネルを１行下にシフトする。]
						//-----------------

						// 選択曲と選択行を１つ上の行に移動。

						this.r現在選択中の曲 = this.r前の曲( this.r現在選択中の曲 );
						this.n現在の選択行 = ( ( this.n現在の選択行 - 1 ) + panelcount ) % panelcount;


						// 選択曲から５つ上のパネル（＝新しく最上部に表示されるパネル。消えてしまう一番下のパネルを再利用する）に、新しい曲の情報を記載する。

						C曲リストノード song = this.r現在選択中の曲;
						for( int i = 0; i < 6; i++ )
							song = this.r前の曲( song );

						int index = ( ( this.n現在の選択行 -6 ) + panelcount ) % panelcount;	// 新しく最上部に表示されるパネルのインデックス（0～12）。
						this.stバー情報[ index ].strタイトル文字列 = song.strタイトル;
						this.stバー情報[ index ].col文字色 = song.col文字色;
                        this.stバー情報[ index ].strサブタイトル = song.strサブタイトル;
                        this.stバー情報[ index ].strジャンル = song.strジャンル;
                        this.stバー情報[ index ].ar難易度 = song.nLevel;
                        for( int f = 0; f < 5; f++ )
                        {
                            if( song.arスコア[ f ] != null )
                                this.stバー情報[ index ].b分岐 = song.arスコア[ f ].譜面情報.b譜面分岐;
                        }

						// stバー情報[] の内容を1行ずつずらす。
						
						C曲リストノード song2 = this.r現在選択中の曲;
						for( int i = 0; i < 6; i++ )
							song2 = this.r前の曲( song2 );

						for( int i = 0; i < CDTXMania.Skin.nSelectSongPanelCount; i++ )
						{
							int n = ( ( ( this.n現在の選択行 - 6 ) + i ) + panelcount ) % panelcount;
							this.stバー情報[ n ].eバー種別 = this.e曲のバー種別を返す( song2 );
							song2 = this.r次の曲( song2 );
                            CDTXMania.t安全にDisposeする( ref this.stバー情報[ i ].txタイトル );
                            this.stバー情報[ i ].txタイトル = this.t曲名テクスチャを生成する( this.stバー情報[ i ].strタイトル文字列 );
						}

		
						// 新しく最上部に表示されるパネル用のスキル値を取得。
						
						for( int i = 0; i < 3; i++ )
							this.stバー情報[ index ].nスキル値[ i ] = (int) song.arスコア[ this.n現在のアンカ難易度レベルに最も近い難易度レベルを返す( song ) ].譜面情報.最大スキル[ i ];


						// 1行(100カウント)移動完了。

						this.n現在のスクロールカウンタ += 100;
						this.n目標のスクロールカウンタ += 100;

						this.t選択曲が変更された(false);				// スクロールバー用に今何番目を選択しているかを更新

                        this.tx選択している曲の曲名?.Dispose();
                        this.tx選択している曲の曲名 = null;
                        this.tx選択している曲のサブタイトル?.Dispose();
                        this.tx選択している曲のサブタイトル = null;
						
						if( this.n目標のスクロールカウンタ == 0 )
							CDTXMania.stage選曲.t選択曲変更通知();		// スクロール完了＝選択曲変更！
						//-----------------
						#endregion
					}

					this.nスクロールタイマ += nアニメ間隔;
				}
				//-----------------
				#endregion
			}


			// 描画。

			if( this.r現在選択中の曲 == null )
			{
				#region [ 曲が１つもないなら「Songs not found.」を表示してここで帰れ。]
				//-----------------
				if ( bIsEnumeratingSongs )
				{
					if ( this.txEnumeratingSongs != null )
					{
						this.txEnumeratingSongs.t2D描画( CDTXMania.app.Device, 320, 160 );
					}
				}
				else
				{
					if ( this.txSongNotFound != null )
						this.txSongNotFound.t2D描画( CDTXMania.app.Device, 320, 160 );
				}
				//-----------------
				#endregion

				return 0;
			}

            int i選曲バーX座標 = 673; //選曲バーの座標用
            int i選択曲バーX座標 = 665; //選択曲バーの座標用

            // kairera0467
            // 登場アニメ全く役に立たない&使われないので登場フェーズは削除
			{
				#region [ (2) 通常フェーズの描画。]
				//-----------------
				for( int i = 0; i < panelcount; i++ )	// パネルは全13枚。
				{
					if( ( i == 0 && this.n現在のスクロールカウンタ > 0 ) ||		// 最上行は、上に移動中なら表示しない。
						( i == ( panelcount - 1 ) && this.n現在のスクロールカウンタ < 0 ) )		// 最下行は、下に移動中なら表示しない。
						continue;

					int nパネル番号 = ( ( ( this.n現在の選択行 - 6 ) + i ) + panelcount ) % panelcount;
					int n見た目の行番号 = i;
                    int n次のパネル番号 = (this.n現在のスクロールカウンタ <= 0) ? ((i + 1) % panelcount ) : ( ( ( i - 1 ) + panelcount ) % panelcount );
					//int x = this.ptバーの基本座標[ n見た目の行番号 ].X + ( (int) ( ( this.ptバーの基本座標[ n次のパネル番号 ].X - this.ptバーの基本座標[ n見た目の行番号 ].X ) * ( ( (double) Math.Abs( this.n現在のスクロールカウンタ ) ) / 100.0 ) ) );
                    int x = CDTXMania.Skin.nSelectSongPanelX[ n見た目の行番号 ];
                    int xAnime = CDTXMania.Skin.nSelectSongPanelX[ n見た目の行番号 ] + ( (int) ( ( CDTXMania.Skin.nSelectSongPanelX[ n次のパネル番号 ] - CDTXMania.Skin.nSelectSongPanelX[ n見た目の行番号 ] ) * ( ( (double) Math.Abs( this.n現在のスクロールカウンタ ) ) / 100.0 ) ) );
					int y = this.ptバーの基本座標[ n見た目の行番号 ].Y + ( (int) ( ( this.ptバーの基本座標[ n次のパネル番号 ].Y - this.ptバーの基本座標[ n見た目の行番号 ].Y ) * ( ( (double) Math.Abs( this.n現在のスクロールカウンタ ) ) / 100.0 ) ) );
		
                    if( CDTXMania.stage選曲.ctDiffSelect移動待ち?.n現在の値 > 0 && !CDTXMania.stage選曲.ctDiffSelect移動待ち.b終了値に達した )
                    {
                        // 難易度選択画面を開くアニメーション
                        if( i < 6 )
                            xAnime -= CDTXMania.stage選曲.ctDiffSelect移動待ち.n現在の値 < 480 ? (int)(500 * (CDTXMania.stage選曲.ctDiffSelect移動待ち.n現在の値 / 480.0f)) : 500;
                        else if( i > 6 )
                            xAnime += CDTXMania.stage選曲.ctDiffSelect移動待ち.n現在の値 < 480 ? (int)(500 * (CDTXMania.stage選曲.ctDiffSelect移動待ち.n現在の値 / 480.0f)) : 500;
                    }
                    else if( CDTXMania.stage選曲.act難易度選択画面.bIsDifficltSelect && CDTXMania.stage選曲.ctDiffSelect移動待ち.b終了値に達した )
                    {
                        xAnime = 1280 + this.tx曲バー.szテクスチャサイズ.Width + 2;
                    }
                    else if( CDTXMania.stage選曲.ctDiffSelect戻り待ち?.n現在の値 >= 0 && !CDTXMania.stage選曲.ctDiffSelect戻り待ち.b終了値に達した )
                    {
                        // 難易度選択画面を閉じるアニメーション
                        if( i < 6 )
                            xAnime -= CDTXMania.stage選曲.ctDiffSelect戻り待ち.n現在の値 > 582 ? 500 - (int)(500 * ((CDTXMania.stage選曲.ctDiffSelect戻り待ち.n現在の値 - 582) / 480.0f)) : 500;
                        else if( i > 6 )
                            xAnime += CDTXMania.stage選曲.ctDiffSelect戻り待ち.n現在の値 > 582 ? 500 - (int)(500 * ((CDTXMania.stage選曲.ctDiffSelect戻り待ち.n現在の値 - 582) / 480.0f)) : 500;
                    }
                    
					//else
                    //if( !CDTXMania.stage選曲.act難易度選択画面.bIsDifficltSelect )
					{
						// (B) スクロール中の選択曲バー、またはその他のバーの描画。

						#region [ バーテクスチャを描画。]
						//-----------------
                        if( this.stバー情報[ nパネル番号 ].ar難易度 != null )
                        {
                            if( this.stバー情報[ nパネル番号 ].eバー種別 == Eバー種別.Score && CDTXMania.stage選曲.actSortSongs.e現在のソート == CActSortSongs.EOrder.Title )
                            {
                                if( i != 6 ? true : this.n現在のスクロールカウンタ != 0 )
                                {
                                    this.tジャンル別選択されていない曲バーの描画( xAnime, CDTXMania.Skin.nSelectSongPanelY, "難易度ソート" );
                                }
                            }
                            else if( this.stバー情報[ nパネル番号 ].eバー種別 == Eバー種別.Score && CDTXMania.stage選曲.actSortSongs.e現在のソート != CActSortSongs.EOrder.Title )
                            {
                                if( i != 6 ? true : this.n現在のスクロールカウンタ != 0 )
                                {
                                    //this.tジャンル別バーの描画( x, y, this.stバー情報[ nパネル番号 ].strジャンル );
                                    //this.tジャンル別選択されていない曲バーの描画( this.ptバーの座標[ n見た目の行番号 ].X, 180, this.stバー情報[ nパネル番号 ].strジャンル );
                                    this.tジャンル別選択されていない曲バーの描画( xAnime, CDTXMania.Skin.nSelectSongPanelY, this.stバー情報[ nパネル番号 ].strジャンル );
                                }
                            }
                            else if( this.stバー情報[ nパネル番号 ].eバー種別 != Eバー種別.Score || CDTXMania.stage選曲.actSortSongs.e現在のソート == CActSortSongs.EOrder.Title )
                            {
                                if( i != 6 ? true : this.n現在のスクロールカウンタ != 0 )
                                {
                                    this.tジャンル別選択されていない曲バーの描画( xAnime, CDTXMania.Skin.nSelectSongPanelY, "" );
                                }
                            }

                            if( ( this.stバー情報[ nパネル番号 ].b分岐[ CDTXMania.stage選曲.n現在選択中の曲の難易度 ] == true && i != 6 ) && CDTXMania.Skin.eDiffSelectMode == EDiffSelectMode.難易度から選ぶ )
                            {
                                //this.tx譜面分岐曲バー用.t2D描画( CDTXMania.app.Device, this.ptバーの座標[ n見た目の行番号 ].X + 76, 160 );
                                this.tx譜面分岐曲バー用.t2D描画( CDTXMania.app.Device, xAnime + 76, CDTXMania.Skin.nSelectSongPanelY );
                            }
                        }
						//-----------------
						#endregion
						#region [ タイトル名テクスチャを描画。]
						//-----------------
						//if( this.stバー情報[ nパネル番号 ].txタイトル名 != null )
						//	this.stバー情報[ nパネル番号 ].txタイトル名.t2D描画( CDTXMania.app.Device, x + 88, y + 10 );

                        if( this.stバー情報[ nパネル番号 ].txタイトル != null )
                        {
                            //this.stバー情報[ nパネル番号 ].txタイトル.t2D描画( CDTXMania.app.Device, this.ptバーの座標[ n見た目の行番号 ].X + 30, 210 );
                            this.stバー情報[ nパネル番号 ].txタイトル.t2D描画( CDTXMania.app.Device, xAnime + CDTXMania.Skin.nSelectSongPanelTitleX, CDTXMania.Skin.nSelectSongPanelY + CDTXMania.Skin.nSelectSongPanelTitleY );
                        }

                        if( this.stバー情報[ nパネル番号 ].ar難易度 != null && CDTXMania.Skin.eDiffSelectMode == EDiffSelectMode.難易度から選ぶ )
                        {
                            //CDTXMania.act文字コンソール.tPrint( xAnime + 70, 564, C文字コンソール.Eフォント種別.白, this.stバー情報[ nパネル番号 ].ar難易度[ CDTXMania.stage選曲.n現在選択中の曲の難易度 ].ToString() );
                            int nX補正 = 0;
                            if( this.stバー情報[ nパネル番号 ].ar難易度[ CDTXMania.stage選曲.n現在選択中の曲の難易度 ].ToString().Length == 2 )
                                nX補正 = -6;
                            this.t小文字表示( xAnime + 65 + nX補正, 559, this.stバー情報[ nパネル番号 ].ar難易度[ CDTXMania.stage選曲.n現在選択中の曲の難易度 ].ToString() );
                        }
						//-----------------
						#endregion
						#region [ スキル値を描画。]
						//-----------------
						if( ( this.stバー情報[ nパネル番号 ].eバー種別 == Eバー種別.Score ) && ( this.e楽器パート != E楽器パート.UNKNOWN ) )
							this.tスキル値の描画( x + 14, y + 14, this.stバー情報[ nパネル番号 ].nスキル値[ (int) this.e楽器パート ] );
						//-----------------
						#endregion
					}

				}
                
                //CDTXMania.act文字コンソール.tPrint(0, 100, C文字コンソール.Eフォント種別.灰, CDTXMania.stage選曲.r現在選択中のスコア.譜面情報.nレベル[ CDTXMania.stage選曲.n現在選択中の曲の難易度 ].ToString() );
                if( this.n現在のスクロールカウンタ == 0 )
                {
                    if( CDTXMania.Skin.eDiffSelectMode == EDiffSelectMode.曲から選ぶ )
                    {
                        if( CDTXMania.stage選曲.act難易度選択画面.bIsDifficltSelect )
                        {
                            if( CDTXMania.stage選曲.ctDiffSelect移動待ち.b進行中 )
                            {
                                if( CDTXMania.stage選曲.ctDiffSelect移動待ち?.n現在の値 < 480 )
                                {
                                    this.txバー中央?.t2D描画( CDTXMania.app.Device, 440, 95 );
                                }
                                else
                                {
                                    int count = CDTXMania.stage選曲.ctDiffSelect移動待ち.n現在の値;
                                    // 14コマ
                                    this.txバー中央_アニメ中.vc拡大縮小倍率.X = 1.0f;

                                    if( count <= 780 )
                                    {
                                        this.txバー中央_アニメ中?.t2D描画( CDTXMania.app.Device, 435 - (int)(195.0f * (( count - 480.0f ) / 300.0f)), 95, new Rectangle( 2, 2, 30, 480 ) );
                                        this.txバー中央_アニメ中.vc拡大縮小倍率.X = 349.0f + (390.0f * (( count - 480.0f ) / 300.0f) ); // 349 -> 739 (390)
                                        this.txバー中央_アニメ中?.t2D描画( CDTXMania.app.Device, 465 - (int)(195.0f * (( count - 480.0f ) / 300.0f)), 95, new Rectangle( 75, 2, 1, 480 ) );
                                        this.txバー中央_アニメ中.vc拡大縮小倍率.X = 1.0f;
                                        this.txバー中央_アニメ中?.t2D描画( CDTXMania.app.Device, 814 + (int)(195.0f * (( count - 480.0f ) / 300.0f)), 95, new Rectangle( 38, 2, 30, 480 ) );
                                    }
                                    else if( count <= 1030 )
                                    {
                                        //左上
                                        this.txバー中央_アニメ中?.t2D描画( CDTXMania.app.Device, 240, 103 - (int)(60f * (( count - 780.0f ) / 250.0f)), new Rectangle( 2, 10, 30, 30 ) );
                                        //右上
                                        this.txバー中央_アニメ中?.t2D描画( CDTXMania.app.Device, 1009, 103 - (int)(60f * (( count - 780.0f ) / 250.0f)), new Rectangle( 38, 10, 30, 30 ) );

                                        this.txバー中央_アニメ中.vc拡大縮小倍率.X = 349.0f + 390.0f;
                                        this.txバー中央_アニメ中?.t2D描画( CDTXMania.app.Device, 270, 131, new Rectangle( 75, 38, 1, 442 ) ); //中央
                                        //上縁
                                        this.txバー中央_アニメ中?.t2D描画( CDTXMania.app.Device, 270, 103 - (int)(60f * (( count - 780.0f ) / 250.0f)), new Rectangle( 75, 10, 1, 30 ) );
                                        this.txバー中央_アニメ中.vc拡大縮小倍率.Y = 60.0f * (( count - 780.0f ) / 250.0f);
                                        this.txバー中央_アニメ中?.t2D描画( CDTXMania.app.Device, 270, 133 - (int)(60f * (( count - 780.0f ) / 250.0f)), new Rectangle( 75, 26, 1, 1 ) );
                                        this.txバー中央_アニメ中.vc拡大縮小倍率.X = 1.0f;
                                        this.txバー中央_アニメ中?.t2D描画( CDTXMania.app.Device, 240, 133 - (int)(60f * (( count - 780.0f ) / 250.0f)), new Rectangle( 2, 26, 30, 1 ) );
                                        this.txバー中央_アニメ中?.t2D描画( CDTXMania.app.Device, 1009, 133 - (int)(60f * (( count - 780.0f ) / 250.0f)), new Rectangle( 38, 26, 30, 1 ) );

                                        this.txバー中央_アニメ中.vc拡大縮小倍率.Y = 1.0f;

                                        this.txバー中央_アニメ中?.t2D描画( CDTXMania.app.Device, 240, 131, new Rectangle( 2, 38, 30, 442 ) );
                                        //右
                                        this.txバー中央_アニメ中?.t2D描画( CDTXMania.app.Device, 1009, 131, new Rectangle( 38, 38, 30, 442 ) );
                                    }
                                    else
                                    {
                                        this.txバー中央_アニメ中?.t2D描画( CDTXMania.app.Device, 240, 131, new Rectangle( 2, 38, 30, 442 ) ); //左
                                        this.txバー中央_アニメ中.vc拡大縮小倍率.X = 349.0f + 390.0f; // 349 -> 739 (390)
                                        this.txバー中央_アニメ中?.t2D描画( CDTXMania.app.Device, 270, 131, new Rectangle( 75, 38, 1, 442 ) ); // 中央
                                        this.txバー中央_アニメ中?.t2D描画( CDTXMania.app.Device, 270, 43, new Rectangle( 75, 10, 1, 30 ) );
                                        this.txバー中央_アニメ中.vc拡大縮小倍率.Y = 72.0f;
                                        this.txバー中央_アニメ中?.t2D描画( CDTXMania.app.Device, 270, 59, new Rectangle( 75, 26, 1, 1 ) );
                                        this.txバー中央_アニメ中.vc拡大縮小倍率.X = 1.0f; //両端中
                                        this.txバー中央_アニメ中?.t2D描画( CDTXMania.app.Device, 240, 59, new Rectangle( 2, 26, 30, 1 ) );
                                        this.txバー中央_アニメ中?.t2D描画( CDTXMania.app.Device, 1009, 59, new Rectangle( 38, 26, 30, 1 ) );

                                        this.txバー中央_アニメ中.vc拡大縮小倍率.Y = 1.0f;


                                        this.txバー中央_アニメ中?.t2D描画( CDTXMania.app.Device, 240, 43, new Rectangle( 2, 10, 30, 30 ) );

                                        this.txバー中央_アニメ中?.t2D描画( CDTXMania.app.Device, 1009, 43, new Rectangle( 38, 10, 30, 30 ) );
                                        this.txバー中央_アニメ中?.t2D描画( CDTXMania.app.Device, 1009, 131, new Rectangle( 38, 38, 30, 442 ) ); //右
                                    }
                                }
                            }

                        }
                        else
                        {
                            if( CDTXMania.stage選曲.ctDiffSelect戻り待ち.b進行中 && CDTXMania.stage選曲.ctDiffSelect戻り待ち.b終了値に達してない )
                            {
                                int count = CDTXMania.stage選曲.ctDiffSelect戻り待ち.n現在の値;
                                //count = 260;
                                this.txバー中央_アニメ中.vc拡大縮小倍率.X = 1.0f;
                                if( count < 250 )
                                {
                                    //左上
                                    this.txバー中央_アニメ中?.t2D描画( CDTXMania.app.Device, 240, 103 + (int)(60f * (( count - 250.0f ) / 250.0f)), new Rectangle( 2, 10, 30, 30 ) );
                                    //右上
                                    this.txバー中央_アニメ中?.t2D描画( CDTXMania.app.Device, 1009, 103 + (int)(60f * (( count - 250.0f ) / 250.0f)), new Rectangle( 38, 10, 30, 30 ) );

                                    this.txバー中央_アニメ中.vc拡大縮小倍率.X = 349.0f + 390.0f;
                                    this.txバー中央_アニメ中?.t2D描画( CDTXMania.app.Device, 270, 131, new Rectangle( 75, 38, 1, 442 ) ); //中央
                                    //上縁
                                    this.txバー中央_アニメ中?.t2D描画( CDTXMania.app.Device, 270, 103 + (int)(60f * (( count - 250.0f ) / 250.0f)), new Rectangle( 75, 10, 1, 30 ) );
                                    this.txバー中央_アニメ中.vc拡大縮小倍率.Y = 60.0f - (60.0f * (( count - 282.0f ) / 250.0f));
                                    this.txバー中央_アニメ中?.t2D描画( CDTXMania.app.Device, 270, 133 + (int)(60f * (( count - 250.0f ) / 250.0f)), new Rectangle( 75, 26, 1, 1 ) );
                                    this.txバー中央_アニメ中.vc拡大縮小倍率.X = 1.0f;
                                    this.txバー中央_アニメ中?.t2D描画( CDTXMania.app.Device, 240, 133 + (int)(60f * (( count - 250.0f ) / 250.0f)), new Rectangle( 2, 26, 30, 1 ) );
                                    this.txバー中央_アニメ中?.t2D描画( CDTXMania.app.Device, 1009, 133 + (int)(60f * (( count - 250.0f ) / 250.0f)), new Rectangle( 38, 26, 30, 1 ) );

                                    this.txバー中央_アニメ中.vc拡大縮小倍率.Y = 1.0f;

                                    this.txバー中央_アニメ中?.t2D描画( CDTXMania.app.Device, 240, 131, new Rectangle( 2, 38, 30, 442 ) );
                                    //右
                                    this.txバー中央_アニメ中?.t2D描画( CDTXMania.app.Device, 1009, 131, new Rectangle( 38, 38, 30, 442 ) );
                                }
                                else if( count >= 250 && count < 500 )
                                {
                                    this.txバー中央_アニメ中?.t2D描画( CDTXMania.app.Device, 240 + (int)(210.0f * (( count - 250.0f ) / 250.0f)), 103, new Rectangle( 2, 10, 30, 460 ) ); //左

                                    //this.txバー中央_アニメ中.vc拡大縮小倍率.X = 349.0f + ( 390.0f - ( 390.0f * (( count - 250.0f ) / 250.0f ) ) ); // 349 -> 739 (390)
                                    //this.txバー中央_アニメ中?.t2D描画( CDTXMania.app.Device, 270 + (int)(60f * (( count - 250.0f ) / 250.0f)), 103, new Rectangle( 75, 10, 1, 460 ) ); // 中央
                                    //this.txバー中央_アニメ中?.t2D描画( CDTXMania.app.Device, 270 + (int)(60f * (( count - 250.0f ) / 250.0f)), 103, new Rectangle( 75, 10, 1, 30 ) );

                                    //左半分
                                    this.txバー中央_アニメ中.vc拡大縮小倍率.X = ( 211.0f - ( 211.0f * (( count - 250.0f ) / 250.0f ) ) );
                                    this.txバー中央_アニメ中?.t2D描画( CDTXMania.app.Device, 480 - (int)( 211.0f - ( 211.0f * (( count - 250.0f ) / 250.0f ) ) ), 103, new Rectangle( 75, 10, 1, 460 ) );
                                    
                                    //右半分
                                    this.txバー中央_アニメ中?.t2D描画( CDTXMania.app.Device, 798, 103, new Rectangle( 75, 10, 1, 460 ) );

                                    //最低限用意する領域 318px
                                    this.txバー中央_アニメ中.vc拡大縮小倍率.X = 318.0f;
                                    this.txバー中央_アニメ中?.t2D描画( CDTXMania.app.Device, 480, 103, new Rectangle( 75, 10, 1, 460 ) );

                                    this.txバー中央_アニメ中.vc拡大縮小倍率.X = 1.0f;
                                    this.txバー中央_アニメ中.vc拡大縮小倍率.Y = 1.0f;
                                    
                                    this.txバー中央_アニメ中?.t2D描画( CDTXMania.app.Device, 1009 - (int)(210.0f * (( count - 250.0f ) / 250.0f)), 103, new Rectangle( 38, 10, 30, 442 ) );
                                    //this.txバー中央_アニメ中?.t2D描画( CDTXMania.app.Device, 1009 - (int)(210.0f * (( count - 250.0f ) / 250.0f)), 131, new Rectangle( 38, 38, 30, 442 ) ); //右
                                }
                                else
                                {
                                    this.txバー中央?.t2D描画( CDTXMania.app.Device, 440, 95 );
                                }
                            }
                            else
                            {
                                this.txバー中央?.t2D描画( CDTXMania.app.Device, 440, 95 );
                            }
                            //this.txバー中央?.t2D描画( CDTXMania.app.Device, 440, 95 );
                        }

                        int starwidth = CDTXMania.Skin.nSelectSongDiffIconSpacingX;
                        int starheight = CDTXMania.Skin.nSelectSongDiffIconHeight + CDTXMania.Skin.nSelectSongDiffIconSpacingY;

                        for( int i = 0; i < 4; i++ )
                        {
                            // 透明度操作
                            if( CDTXMania.stage選曲.act難易度選択画面.bIsDifficltSelect )
                            {
                                if( CDTXMania.stage選曲.ctDiffSelect移動待ち.n現在の値 > 0 && CDTXMania.stage選曲.ctDiffSelect移動待ち.n現在の値 <= 110 )
                                {
                                    this.tx難易度パネル.n透明度 = 255 - (int)(( (CDTXMania.stage選曲.ctDiffSelect移動待ち.n現在の値) / 110.0f) * 255);
                                    this.tx難易度文字中央パネル用.n透明度 = 255 - (int)(( (CDTXMania.stage選曲.ctDiffSelect移動待ち.n現在の値) / 110.0f) * 255);
                                    this.tx難易度星.n透明度 = 255 - (int)(( (CDTXMania.stage選曲.ctDiffSelect移動待ち.n現在の値) / 110.0f) * 255);
                                    this.tx難易度アイコン.n透明度 = 255 - (int)(( (CDTXMania.stage選曲.ctDiffSelect移動待ち.n現在の値) / 110.0f) * 255);
                                }
                                else if( CDTXMania.stage選曲.ctDiffSelect移動待ち.n現在の値 > 110 )
                                {
                                    this.tx難易度パネル.n透明度 = 0;
                                    this.tx難易度文字中央パネル用.n透明度 = 0;
                                    this.tx難易度星.n透明度 = 0;
                                    this.tx難易度アイコン.n透明度 = 0;
                                }
                            }
                            else
                            {
                                this.tx難易度パネル.n透明度 = 255;
                                this.tx難易度文字中央パネル用.n透明度 = 255;
                                this.tx難易度星.n透明度 = 255;
                                this.tx難易度アイコン.n透明度 = 255;
                            }


                            Rectangle rectDiffString = new Rectangle( (this.tx難易度文字中央パネル用.szテクスチャサイズ.Width / 4) * i, 0, this.tx難易度文字中央パネル用.szテクスチャサイズ.Width / 4, this.tx難易度文字中央パネル用.szテクスチャサイズ.Height );
                            this.tx難易度パネル?.t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nSelectSongDiffPanelX + ( CDTXMania.Skin.nSelectSongDiffPanelSpacingX * i), CDTXMania.Skin.nSelectSongDiffPanelY + (CDTXMania.Skin.nSelectSongDiffPanelSpacingY * i) );
                            this.tx難易度文字中央パネル用?.t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nSelectSongDiffPanelX + CDTXMania.Skin.nSelectSongDiffPanelStringX + ( CDTXMania.Skin.nSelectSongDiffPanelSpacingX * i), CDTXMania.Skin.nSelectSongDiffPanelY + CDTXMania.Skin.nSelectSongDiffPanelStringY + (CDTXMania.Skin.nSelectSongDiffPanelSpacingY * i), rectDiffString );
                            this.tx難易度アイコン.t2D描画( CDTXMania.app.Device, (CDTXMania.Skin.nSelectSongDiffPanelX + (this.tx難易度パネル.szテクスチャサイズ.Width / 2) + CDTXMania.Skin.nSelectSongDiffPanelStringX + ( CDTXMania.Skin.nSelectSongDiffPanelSpacingX * i)) - 32, (CDTXMania.Skin.nSelectSongDiffPanelY + CDTXMania.Skin.nSelectSongDiffPanelStringY + (CDTXMania.Skin.nSelectSongDiffPanelSpacingY * i)) - 38, new Rectangle( 0, 60 * i, 65, 60 ) );

                            int lv = CDTXMania.stage選曲.r現在選択中のスコア.譜面情報.nレベル[ i ];
                            for( int j = 0; j < 10; j++ )
                            {
                                this.tx難易度星?.t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nSelectSongDiffIconX + (starwidth * i), CDTXMania.Skin.nSelectSongDiffIconY - (starheight * j), new Rectangle( 0, CDTXMania.Skin.nSelectSongDiffIconHeight, CDTXMania.Skin.nSelectSongDiffIconWidth, CDTXMania.Skin.nSelectSongDiffIconHeight ) );
                            }
                            for( int j = 0; j < lv; j++ )
                            {
                                if( j > 9 ) break;
                                this.tx難易度星?.t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nSelectSongDiffIconX + (starwidth * i), CDTXMania.Skin.nSelectSongDiffIconY - (starheight * j), new Rectangle( 0, 0, CDTXMania.Skin.nSelectSongDiffIconWidth, CDTXMania.Skin.nSelectSongDiffIconHeight ) );
                            }
                        }
                    }
                    else
                    {
                        this.txバー中央?.t2D描画( CDTXMania.app.Device, 487, 137 );

                        this.tx難易度パネル?.t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nSelectSongDiffPanelX, CDTXMania.Skin.nSelectSongDiffPanelY );
                        if( CDTXMania.stage選曲.r現在選択中のスコア.譜面情報.b譜面分岐[ CDTXMania.stage選曲.n現在選択中の曲の難易度 ] )
                            this.tx譜面分岐中央パネル用?.t2D描画( CDTXMania.app.Device, 570, 347 );
                    }



                    #region[ 星 ]
                    if( this.tx難易度星 != null )
                    {
                        if( CDTXMania.Skin.eDiffSelectMode == EDiffSelectMode.曲から選ぶ )
                        {

                        }
                        else
                        {
                            for( int n = 0; n < CDTXMania.stage選曲.r現在選択中のスコア.譜面情報.nレベル[ CDTXMania.stage選曲.n現在選択中の曲の難易度 ]; n++ )
                            {
                                if( n > 9 ) break;
                                this.tx難易度星?.t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nSelectSongDiffIconX, CDTXMania.Skin.nSelectSongDiffIconY - ( n * (CDTXMania.Skin.nSelectSongDiffIconHeight + CDTXMania.Skin.nSelectSongDiffIconSpacingY) ), new Rectangle( CDTXMania.Skin.nSelectSongDiffIconX * ( CDTXMania.stage選曲.n現在選択中の曲の難易度 + 1), 0, CDTXMania.Skin.nSelectSongDiffIconWidth, CDTXMania.Skin.nSelectSongDiffIconHeight ) );
                            }
                        }

                    }
                    #endregion
                }

                #region [ 項目リストにフォーカスがあって、かつスクロールが停止しているなら、パネルの上下に▲印を描画する。]
	    		//-----------------
		    	if( ( this.n目標のスクロールカウンタ == 0 ) && ( CDTXMania.stage選曲.ctDiffSelect戻り待ち.b進行中 ? CDTXMania.stage選曲.ctDiffSelect戻り待ち.b終了値に達した : !CDTXMania.stage選曲.act難易度選択画面.bIsDifficltSelect ) )
			    {
    				int y;
	    			int x_upper;
		    		int x_lower;
			
			    	// 位置決定。

				    x_upper = CDTXMania.Skin.nSelectSongPanelCursorLX - this.ct三角矢印アニメ.n現在の値;
		    		x_lower = CDTXMania.Skin.nSelectSongPanelCursorRX + this.ct三角矢印アニメ.n現在の値;
			    	y = CDTXMania.Skin.nSelectSongPanelCursorY;
    				
	    			// 描画。
				
                    if( !CDTXMania.stage選曲.act難易度選択画面.bIsDifficltSelect )
                    {
		    		    if( this.txカーソル左 != null )
			    	    {
				    	    this.txカーソル左.t2D描画( CDTXMania.app.Device, x_upper, y );
    				    }
                        if( this.txカーソル右 != null )
                        {
			    		    this.txカーソル右.t2D描画( CDTXMania.app.Device, x_lower, y );
                        }
                    }
			    }
			    //-----------------
			    #endregion


                for( int i = 0; i < panelcount; i++ )	// パネルは全13枚。
				{
					if( ( i == 0 && this.n現在のスクロールカウンタ > 0 ) ||		// 最上行は、上に移動中なら表示しない。
						( i == (panelcount - 1) && this.n現在のスクロールカウンタ < 0 ) )		// 最下行は、下に移動中なら表示しない。
						continue;

					int nパネル番号 = ( ( ( this.n現在の選択行 - 6 ) + i ) + panelcount ) % panelcount;
					int n見た目の行番号 = i;
					int n次のパネル番号 = ( this.n現在のスクロールカウンタ <= 0 ) ? ( ( i + 1 ) % panelcount ) : ( ( ( i - 1 ) + panelcount ) % panelcount );
					//int x = this.ptバーの基本座標[ n見た目の行番号 ].X + ( (int) ( ( this.ptバーの基本座標[ n次のパネル番号 ].X - this.ptバーの基本座標[ n見た目の行番号 ].X ) * ( ( (double) Math.Abs( this.n現在のスクロールカウンタ ) ) / 100.0 ) ) );
                    int x = i選曲バーX座標;
                    int xAnime = this.ptバーの座標[ n見た目の行番号 ].X + ( (int) ( ( this.ptバーの座標[ n次のパネル番号 ].X - this.ptバーの座標[ n見た目の行番号 ].X ) * ( ( (double) Math.Abs( this.n現在のスクロールカウンタ ) ) / 100.0 ) ) );
                    int xSelectAnime = 0;
                    int ySelectAnime = 0;
					int y = this.ptバーの基本座標[ n見た目の行番号 ].Y + ( (int) ( ( this.ptバーの基本座標[ n次のパネル番号 ].Y - this.ptバーの基本座標[ n見た目の行番号 ].Y ) * ( ( (double) Math.Abs( this.n現在のスクロールカウンタ ) ) / 100.0 ) ) );

                    if( ( i == 6 ) && ( this.n現在のスクロールカウンタ == 0 ) )
					{
						// (A) スクロールが停止しているときの選択曲バーの描画。

                        int y選曲 = 269;

						#region [ バーテクスチャを描画。]
						//-----------------
						//this.tバーの描画( i選択曲バーX座標 - 80, y選曲 - 30, this.stバー情報[ nパネル番号 ].eバー種別, true );
						//-----------------
						#endregion
						#region [ タイトル名テクスチャを描画。]
						//-----------------
                        if( this.stバー情報[ nパネル番号 ].strタイトル文字列 != "" && this.stバー情報[ nパネル番号 ].strタイトル文字列 != null && this.tx選択している曲の曲名 == null )
                            this.tx選択している曲の曲名 = this.t曲名テクスチャを生成する( this.stバー情報[ nパネル番号 ].strタイトル文字列 );
                        if( this.stバー情報[ nパネル番号 ].strサブタイトル != "" && this.stバー情報[ nパネル番号 ].strサブタイトル != null && this.tx選択している曲のサブタイトル == null )
                            this.tx選択している曲のサブタイトル = this.tサブタイトルテクスチャを生成する( this.stバー情報[ nパネル番号 ].strサブタイトル );



                        if( CDTXMania.Skin.eDiffSelectMode == EDiffSelectMode.曲から選ぶ )
                        {
                            if (CDTXMania.stage選曲.act難易度選択画面.bIsDifficltSelect)
                            {
                                int count = CDTXMania.stage選曲.ctDiffSelect移動待ち.n現在の値;
                                if( count >= 480 && count <= 780 )
                                {
                                    xSelectAnime = (int)(175f * (( count - 480.0f ) / 300.0f));
                                }
                                else if( count >= 780 && count <= 1030 )
                                {
                                    xSelectAnime = 175;
                                    ySelectAnime = -(int)(38f * (( count - 780.0f ) / 250.0f));
                                }
                                else if( count > 1030 )
                                {
                                    xSelectAnime = 175;
                                    ySelectAnime = -38;
                                }
                            }
                        }

                        if( this.tx選択している曲のサブタイトル != null )
                        {
                            int nサブタイY = (int)(CDTXMania.Skin.nSelectSongPanelCenterSubTitleY - (this.tx選択している曲のサブタイトル.sz画像サイズ.Height * this.tx選択している曲のサブタイトル.vc拡大縮小倍率.Y ));
							this.tx選択している曲のサブタイトル.t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nSelectSongPanelCenterSubTitleX + xSelectAnime, nサブタイY + ySelectAnime );
                            if( this.tx選択している曲の曲名 != null )
                            {
						    	this.tx選択している曲の曲名.t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nSelectSongPanelCenterTitleX[ 1 ] + xSelectAnime, CDTXMania.Skin.nSelectSongPanelCenterTitleY + ySelectAnime );
                            }
                        }
                        else
                        {
                            if( this.tx選択している曲の曲名 != null )
                            {
	    						this.tx選択している曲の曲名.t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nSelectSongPanelCenterTitleX[ 0 ] + xSelectAnime, CDTXMania.Skin.nSelectSongPanelCenterTitleY + ySelectAnime );
                            }
                        }

						//if( this.stバー情報[ nパネル番号 ].txタイトル名 != null )
						//	this.stバー情報[ nパネル番号 ].txタイトル名.t2D描画( CDTXMania.app.Device, i選択曲バーX座標 + 65, y選曲 + 6 );

                        //CDTXMania.act文字コンソール.tPrint( i選曲バーX座標 - 20, y選曲 + 6, C文字コンソール.Eフォント種別.白, this.r現在選択中のスコア.譜面情報.b譜面分岐[3].ToString() );
						//-----------------
						#endregion
						#region [ スキル値を描画。]
						//-----------------
						//if( ( this.stバー情報[ nパネル番号 ].eバー種別 == Eバー種別.Score ) && ( this.e楽器パート != E楽器パート.UNKNOWN ) )
							//this.tスキル値の描画( 0xf4, 0xd3, this.stバー情報[ nパネル番号 ].nスキル値[ (int) this.e楽器パート ] );
						//-----------------
						#endregion
					}

				}
				//-----------------
				#endregion
			}
			#region [ スクロール地点の計算(描画はCActSelectShowCurrentPositionにて行う) #27648 ]
			int py;
			double d = 0;
			if ( nNumOfItems > 1 )
			{
				d = ( 336 - 6 - 8 ) / (double) ( nNumOfItems - 1 );
				py = (int) ( d * ( nCurrentPosition - 1 ) );
			}
			else
			{
				d = 0;
				py = 0;
			}
			int delta = (int) ( d * this.n現在のスクロールカウンタ / 100 );
			if ( py + delta <= 336 - 6 - 8 )
			{
				this.nスクロールバー相対y座標 = py + delta;
			}
			#endregion

			#region [ アイテム数の描画 #27648 ]
			tアイテム数の描画();
			#endregion

            if( ( ( this.e曲のバー種別を返す( this.r現在選択中の曲 ) ) == Eバー種別.Score && CDTXMania.stage選曲.actSortSongs.e現在のソート != CActSortSongs.EOrder.Title ) && this.nStrジャンルtoNum( this.r現在選択中の曲.strジャンル ) != 8 )
            {
                if( this.tx上部ジャンル名 != null )
                {
                    // 透明度操作
                    if( CDTXMania.stage選曲.act難易度選択画面.bIsDifficltSelect )
                    {
                        if( CDTXMania.stage選曲.ctDiffSelect移動待ち.n現在の値 > 0 && CDTXMania.stage選曲.ctDiffSelect移動待ち.n現在の値 <= 110 )
                        {
                            this.tx上部ジャンル名.n透明度 = 255 - (int)(( (CDTXMania.stage選曲.ctDiffSelect移動待ち.n現在の値) / 110.0f) * 255);
                        }
                        else if( CDTXMania.stage選曲.ctDiffSelect移動待ち.n現在の値 > 110 )
                        {
                            this.tx上部ジャンル名.n透明度 = 0;
                        }
                    }
                    else
                    {
                        this.tx上部ジャンル名.n透明度 = 255;
                    }

                    this.tx上部ジャンル名.t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nSelectGenreStringX - (this.tx上部ジャンル名.szテクスチャサイズ.Width / 2), CDTXMania.Skin.nSelectGenreStringY, new Rectangle( 0, 60 * this.nStrジャンルtoNum( this.r現在選択中の曲.strジャンル ), 288, 60 ) );
                }
            }
            return 0;
		}
		

		// その他

		#region [ private ]
		//-----------------
		private enum Eバー種別 { Score, Box, Other }

		private struct STバー
		{
			public CTexture Score;
			public CTexture Box;
			public CTexture Other;
			public CTexture this[ int index ]
			{
				get
				{
					switch( index )
					{
						case 0:
							return this.Score;

						case 1:
							return this.Box;

						case 2:
							return this.Other;
					}
					throw new IndexOutOfRangeException();
				}
				set
				{
					switch( index )
					{
						case 0:
							this.Score = value;
							return;

						case 1:
							this.Box = value;
							return;

						case 2:
							this.Other = value;
							return;
					}
					throw new IndexOutOfRangeException();
				}
			}
		}

		private struct STバー情報
		{
			public CActSelect曲リスト.Eバー種別 eバー種別;
			public string strタイトル文字列;
			public STDGBVALUE<int> nスキル値;
			public Color col文字色;
            public int[] ar難易度;
            public bool[] b分岐;
            public string strジャンル;
            public string strサブタイトル;
            public CTexture txタイトル;
            public CTexture txサブタイトル;
		}

		private struct ST選曲バー
		{
			public CTexture Score;
			public CTexture Box;
			public CTexture Other;
			public CTexture this[ int index ]
			{
				get
				{
					switch( index )
					{
						case 0:
							return this.Score;

						case 1:
							return this.Box;

						case 2:
							return this.Other;
					}
					throw new IndexOutOfRangeException();
				}
				set
				{
					switch( index )
					{
						case 0:
							this.Score = value;
							return;

						case 1:
							this.Box = value;
							return;

						case 2:
							this.Other = value;
							return;
					}
					throw new IndexOutOfRangeException();
				}
			}
		}

		private bool b登場アニメ全部完了;
		private Color color文字影 = Color.FromArgb( 0x40, 10, 10, 10 );
		private CCounter[] ct登場アニメ用 = new CCounter[ 13 ];
        private CCounter ct三角矢印アニメ;
        private CPrivateFastFont pfMusicName;
        private CPrivateFastFont pfSubtitle;
		private E楽器パート e楽器パート;
		private Font ft曲リスト用フォント;
		private long nスクロールタイマ;
		private int n現在のスクロールカウンタ;
		private int n現在の選択行;
		private int n目標のスクロールカウンタ;
        private int nパネル枚数 = 13;
        private readonly Point[] ptバーの基本座標 = new Point[] { new Point( 0x2c4, 5 ), new Point( 0x272, 56 ), new Point( 0x242, 107 ), new Point( 0x222, 158 ), new Point( 0x210, 209 ), new Point( 0x1d0, 270 ), new Point( 0x224, 362 ), new Point( 0x242, 413 ), new Point( 0x270, 464 ), new Point( 0x2ae, 515 ), new Point( 0x314, 566 ), new Point( 0x3e4, 617 ), new Point( 0x500, 668 ) };
        private Point[] ptバーの座標 = new Point[] 
        { new Point( -218, 180 ), new Point( -77, 180 ), new Point( 64, 180 ), new Point( 205, 180 ), new Point( 346, 180 ), 
          new Point( 590, 180 ),
          new Point( 833, 180 ), new Point( 974, 180 ), new Point( 1115, 180 ), new Point( 1256, 180 ), new Point( 1397, 180 ), new Point( 1397, 180 ), new Point( 1397, 180 ) };

		private STバー情報[] stバー情報 = new STバー情報[ 13 ];
		private CTexture txSongNotFound, txEnumeratingSongs;
		private CTexture txスキル数字;
		private CTexture txアイテム数数字;
        private CTexture txバー中央;
        private CTexture txバー中央_アニメ中;
        private CTexture tx難易度アイコン;
        private CTexture tx選択している曲の曲名;
        private CTexture tx選択している曲のサブタイトル;

        private CTexture tx曲バー_アニメ;
        private CTexture tx曲バー_JPOP;
        private CTexture tx曲バー_クラシック;
        private CTexture tx曲バー_ゲーム;
        private CTexture tx曲バー_ナムコ;
        private CTexture tx曲バー_バラエティ;
        private CTexture tx曲バー_どうよう;
        private CTexture tx曲バー_ボカロ;
        private CTexture tx曲バー;

        private CTexture[] tx曲バー_難易度 = new CTexture[ 5 ];

        private CTexture tx譜面分岐曲バー用;
        private CTexture tx難易度パネル;
        private CTexture tx上部ジャンル名;


        private CTexture txカーソル左;
        private CTexture txカーソル右;

        private CTexture txMusicName;

        private CTexture tx難易度星;
        private CTexture tx譜面分岐中央パネル用;
        private CTexture tx難易度文字中央パネル用;

        private long n矢印スクロール用タイマ値;

		private int nCurrentPosition = 0;
		private int nNumOfItems = 0;

		//private string strBoxDefSkinPath = "";
		private Eバー種別 e曲のバー種別を返す( C曲リストノード song )
		{
			if( song != null )
			{
				switch( song.eノード種別 )
				{
					case C曲リストノード.Eノード種別.SCORE:
					case C曲リストノード.Eノード種別.SCORE_MIDI:
						return Eバー種別.Score;

					case C曲リストノード.Eノード種別.BOX:
					case C曲リストノード.Eノード種別.BACKBOX:
						return Eバー種別.Box;
				}
			}
			return Eバー種別.Other;
		}
		private C曲リストノード r次の曲( C曲リストノード song )
		{
			if( song == null )
				return null;

			List<C曲リストノード> list = ( song.r親ノード != null ) ? song.r親ノード.list子リスト : CDTXMania.Songs管理.list曲ルート;
	
			int index = list.IndexOf( song );

			if( index < 0 )
				return null;

			if( index == ( list.Count - 1 ) )
				return list[ 0 ];

			return list[ index + 1 ];
		}
		private C曲リストノード r前の曲( C曲リストノード song )
		{
			if( song == null )
				return null;

			List<C曲リストノード> list = ( song.r親ノード != null ) ? song.r親ノード.list子リスト : CDTXMania.Songs管理.list曲ルート;

			int index = list.IndexOf( song );
	
			if( index < 0 )
				return null;

			if( index == 0 )
				return list[ list.Count - 1 ];

			return list[ index - 1 ];
		}
		private void tスキル値の描画( int x, int y, int nスキル値 )
		{
			if( nスキル値 <= 0 || nスキル値 > 100 )		// スキル値 0 ＝ 未プレイ なので表示しない。
				return;

			int color = ( nスキル値 == 100 ) ? 3 : ( nスキル値 / 25 );

			int n百の位 = nスキル値 / 100;
			int n十の位 = ( nスキル値 % 100 ) / 10;
			int n一の位 = ( nスキル値 % 100 ) % 10;


			// 百の位の描画。

			if( n百の位 > 0 )
				this.tスキル値の描画_１桁描画( x, y, n百の位, color );


			// 十の位の描画。

			if( n百の位 != 0 || n十の位 != 0 )
				this.tスキル値の描画_１桁描画( x + 7, y, n十の位, color );


			// 一の位の描画。

			this.tスキル値の描画_１桁描画( x + 14, y, n一の位, color );
		}
		private void tスキル値の描画_１桁描画( int x, int y, int n数値, int color )
		{
			int dx = ( n数値 % 5 ) * 9;
			int dy = ( n数値 / 5 ) * 12;
			
			switch( color )
			{
				case 0:
					if( this.txスキル数字 != null )
						this.txスキル数字.t2D描画( CDTXMania.app.Device, x, y, new Rectangle( 45 + dx, 24 + dy, 9, 12 ) );
					break;

				case 1:
					if( this.txスキル数字 != null )
						this.txスキル数字.t2D描画( CDTXMania.app.Device, x, y, new Rectangle( 45 + dx, dy, 9, 12 ) );
					break;

				case 2:
					if( this.txスキル数字 != null )
						this.txスキル数字.t2D描画( CDTXMania.app.Device, x, y, new Rectangle( dx, 24 + dy, 9, 12 ) );
					break;

				case 3:
					if( this.txスキル数字 != null )
						this.txスキル数字.t2D描画( CDTXMania.app.Device, x, y, new Rectangle( dx, dy, 9, 12 ) );
					break;
			}
		}
		private void tバーの初期化()
		{
			C曲リストノード song = this.r現在選択中の曲;
			
			if( song == null )
				return;

			for( int i = 0; i < 6; i++ )
				song = this.r前の曲( song );

			if( song == null )
				return;

			for( int i = 0; i < 13; i++ )
			{
				this.stバー情報[ i ].strタイトル文字列 = song.strタイトル;
                this.stバー情報[ i ].strジャンル = song.strジャンル;
				this.stバー情報[ i ].col文字色 = song.col文字色;
				this.stバー情報[ i ].eバー種別 = this.e曲のバー種別を返す( song );
                this.stバー情報[ i ].strサブタイトル = song.strサブタイトル;
                this.stバー情報[ i ].ar難易度 = song.nLevel;

			    for( int f = 0; f < 5; f++ )
                {
                    if( song.arスコア[ f ] != null )
                        this.stバー情報[ i ].b分岐 = song.arスコア[ f ].譜面情報.b譜面分岐;
                }
				
				for( int j = 0; j < 3; j++ )
					this.stバー情報[ i ].nスキル値[ j ] = (int) song.arスコア[ this.n現在のアンカ難易度レベルに最も近い難易度レベルを返す( song ) ].譜面情報.最大スキル[ j ];

                CDTXMania.t安全にDisposeする( ref this.stバー情報[ i ].txタイトル );
                this.stバー情報[ i ].txタイトル = this.t曲名テクスチャを生成する( this.stバー情報[ i ].strタイトル文字列 );

				song = this.r次の曲( song );
			}

			this.n現在の選択行 = 6;
		}
		private void tジャンル別選択されていない曲バーの描画( int x, int y, string strジャンル )
		{
			if( x >= SampleFramework.GameWindowSize.Width || y >= SampleFramework.GameWindowSize.Height )
				return;

            var rc = new Rectangle( 0, 48, 128, 48 );

			switch( strジャンル )
            {
                case "J-POP":
				    #region [ J-POP ]
    				//-----------------
	    			if( this.tx曲バー_JPOP != null )
		    			this.tx曲バー_JPOP.t2D描画( CDTXMania.app.Device, x, y );
	    			//-----------------
		    		#endregion
                    break;
                case "アニメ":
				    #region [ アニメ ]
    				//-----------------
	    			if( this.tx曲バー_アニメ != null )
		    			this.tx曲バー_アニメ.t2D描画( CDTXMania.app.Device, x, y );
	    			//-----------------
		    		#endregion
                    break;
                case "ゲームミュージック":
				    #region [ ゲーム ]
    				//-----------------
	    			if( this.tx曲バー_ゲーム != null )
		    			this.tx曲バー_ゲーム.t2D描画( CDTXMania.app.Device, x, y );
	    			//-----------------
		    		#endregion
                    break;
                case "ナムコオリジナル":
				    #region [ ナムコオリジナル ]
    				//-----------------
	    			if( this.tx曲バー_ナムコ != null )
		    			this.tx曲バー_ナムコ.t2D描画( CDTXMania.app.Device, x, y );
	    			//-----------------
		    		#endregion
                    break;
                case "クラシック":
				    #region [ クラシック ]
    				//-----------------
	    			if( this.tx曲バー_クラシック != null )
		    			this.tx曲バー_クラシック.t2D描画( CDTXMania.app.Device, x, y );
	    			//-----------------
		    		#endregion
                    break;
                case "バラエティ":
				    #region [ バラエティ ]
    				//-----------------
	    			if( this.tx曲バー_バラエティ != null )
		    			this.tx曲バー_バラエティ.t2D描画( CDTXMania.app.Device, x, y );
	    			//-----------------
		    		#endregion
                    break;
                case "どうよう":
				    #region [ どうよう ]
    				//-----------------
	    			if( this.tx曲バー_どうよう != null )
		    			this.tx曲バー_どうよう.t2D描画( CDTXMania.app.Device, x, y );
	    			//-----------------
		    		#endregion
                    break;
                case "ボーカロイド":
                case "VOCALOID":
				    #region [ ボカロ ]
    				//-----------------
	    			if( this.tx曲バー_ボカロ != null )
		    			this.tx曲バー_ボカロ.t2D描画( CDTXMania.app.Device, x, y );
	    			//-----------------
		    		#endregion
                    break;
                case "難易度ソート":
				    #region [ 難易度ソート ]
    				//-----------------
	    			if( this.tx曲バー_難易度[ this.n現在選択中の曲の現在の難易度レベル ] != null )
		    			this.tx曲バー_難易度[ this.n現在選択中の曲の現在の難易度レベル ].t2D描画( CDTXMania.app.Device, x, y );
	    			//-----------------
		    		#endregion
                    break;
                default:
				    #region [ その他の場合 ]
    				//-----------------
	    			if( this.tx曲バー != null )
		    			this.tx曲バー.t2D描画( CDTXMania.app.Device, x, y );
	    			//-----------------
		    		#endregion
                    break;
			}
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
        private CTexture t曲名テクスチャを生成する( string str文字 )
        {
            Bitmap bmp;
            
            bmp = this.pfMusicName.DrawPrivateFont( str文字, Color.White, Color.Black, true );

            CTexture tx文字テクスチャ = CDTXMania.tテクスチャの生成( bmp, false );

            if( tx文字テクスチャ.szテクスチャサイズ.Height > CDTXMania.Skin.nSelectSongPanelCenterTitleHeight )
            {
                tx文字テクスチャ.vc拡大縮小倍率.Y = (float)( (float)CDTXMania.Skin.nSelectSongPanelCenterTitleHeight / tx文字テクスチャ.szテクスチャサイズ.Height );
            }

            bmp?.Dispose();

            return tx文字テクスチャ;
        }
        private CTexture tサブタイトルテクスチャを生成する( string str文字 )
        {
            Bitmap bmp;
            
            bmp = this.pfSubtitle.DrawPrivateFont( str文字, Color.White, Color.Black, true );

            CTexture tx文字テクスチャ = CDTXMania.tテクスチャの生成( bmp, false );

            if( tx文字テクスチャ.szテクスチャサイズ.Height > CDTXMania.Skin.nSelectSongPanelCenterSubTitleHeight )
            {
                tx文字テクスチャ.vc拡大縮小倍率.Y = (float)( (float)CDTXMania.Skin.nSelectSongPanelCenterSubTitleHeight / tx文字テクスチャ.szテクスチャサイズ.Height );
            }

            bmp?.Dispose();

            return tx文字テクスチャ;
        }

		private void tアイテム数の描画()
		{
			string s = nCurrentPosition.ToString() + "/" + nNumOfItems.ToString();
			int x = 639 - 8 - 12;
			int y = 362;

			for ( int p = s.Length - 1; p >= 0; p-- )
			{
				tアイテム数の描画_１桁描画( x, y, s[ p ] );
				x -= 8;
			}
		}
		private void tアイテム数の描画_１桁描画( int x, int y, char s数値 )
		{
			int dx, dy;
			if ( s数値 == '/' )
			{
				dx = 48;
				dy = 0;
			}
			else
			{
				int n = (int) s数値 - (int) '0';
				dx = ( n % 6 ) * 8;
				dy = ( n / 6 ) * 12;
			}
			if ( this.txアイテム数数字 != null )
			{
				this.txアイテム数数字.t2D描画( CDTXMania.app.Device, x, y, new Rectangle( dx, dy, 8, 12 ) );
			}
		}


        //数字フォント
        private CTexture txレベル数字フォント;
        [StructLayout( LayoutKind.Sequential )]
        private struct STレベル数字
        {
            public char ch;
            public int ptX;
        }
        private STレベル数字[] st小文字位置 = new STレベル数字[ 10 ];
        private void t小文字表示(int x, int y, string str)
        {
            foreach (char ch in str)
            {
                for (int i = 0; i < this.st小文字位置.Length; i++)
                {
                    if( this.st小文字位置[i].ch == ch )
                    {
                        Rectangle rectangle = new Rectangle( this.st小文字位置[i].ptX, 0, 22, 28 );
                        if (this.txレベル数字フォント != null)
                        {
                            this.txレベル数字フォント.t2D描画(CDTXMania.app.Device, x, y, rectangle);
                        }
                        break;
                    }
                }
                x += 16;
            }
        }
		//-----------------
		#endregion
	}
}
