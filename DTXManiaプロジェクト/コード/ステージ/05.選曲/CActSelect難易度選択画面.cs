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

namespace TJAPlayer3
{
    /// <summary>
    /// 難易度選択画面。
    /// この難易度選択画面はAC7～AC14のような方式であり、WiiまたはAC15移行の方式とは異なる。
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
                    if( TJAPlayer3.stage選曲.r現在選択中の曲.arスコア[ i ] != null ) return i;
                    if( i == 4 ) return this.t指定した方向に近い難易度番号を返す( 0, 0 );
                }
            }
            else
            {
                for( int i = pos; i > -1; i-- )
                {
                    if( pos == i ) continue;
                    if( TJAPlayer3.stage選曲.r現在選択中の曲.arスコア[ i ] != null ) return i;
                    if( i == 0 ) return this.t指定した方向に近い難易度番号を返す( 1, 4 );
                }
            }
            return pos;
        }
        
        public void t次に移動()
		{
			if( TJAPlayer3.stage選曲.r現在選択中の曲 != null )
			{
		        if( this.n現在の選択行 < 5 )
                {
                    this.n現在の選択行 = this.t指定した方向に近い難易度番号を返す( 0, this.n現在の選択行 );
                }
                this.ct移動 = new CCounter( 1, 710, 1, CSound管理.rc演奏用タイマ );
			}
		}
		public void t前に移動()
		{
			if( TJAPlayer3.stage選曲.r現在選択中の曲 != null )
			{
		        if( this.n現在の選択行 > 0 )
                {
                    this.n現在の選択行 = this.t指定した方向に近い難易度番号を返す( 1, this.n現在の選択行 );
                }
                //else
                //{
                //    this.n現在の選択行 = this.t指定した方向に近い難易度番号を返す( 1, 5 );
                //}
			}
		}
		public void t選択画面初期化()
		{
			//かんたんから一番近いところにカーソルを移動させる。
            for( int i = 0; i < (int)Difficulty.Total; i++ )
            {
                if( TJAPlayer3.stage選曲.r現在選択中の曲.arスコア[ i ] != null )
                {
                    this.n現在の選択行 = i;
                    break;
                }
            }

            int n譜面数 = 0;
            for( int i = 0; i < (int)Difficulty.Total; i++ )
			{
                if( TJAPlayer3.stage選曲.r現在選択中の曲.arスコア[ i ] != null ) n譜面数++;
            }
            for( int i = 0; i < (int)Difficulty.Total; i++ )
			{
                //描画順と座標を決める。
                switch( n譜面数 )
                {
                    case 1:
                    case 2:
                        this.n描画順 = new[] { 0, 1, 2, 3, 4 };
                        this.n踏み台座標 = new[] { 12, 252, 492, 732, 972 };
                        break;
                    case 3:
                        this.n描画順 = new[] { 0, 2, 1, 3, 4 };
                        this.n踏み台座標 = new[] { 12, 492, 252, 732, 972 };
                        break;
                    case 4:
                        this.n描画順 = new[] { 0, 2, 1, 3, 4 };
                        this.n踏み台座標 = new[] { 12, 492, 252, 732, 972 };
                        break;
                    case 5:
                        this.n描画順 = new[] { 0, 3, 1, 4, 2 };
                        this.n踏み台座標 = new[] { 12, 492, 972, 252, 732 };
                        break;
                }

            }
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

			base.On非活性化();
		}
		public override void OnManagedリソースの作成()
		{
			if( this.b活性化してない )
				return;

            this.tx背景 = TJAPlayer3.tテクスチャの生成( CSkin.Path( @"Graphics\5_diffselect_background.png" ) );
            this.txヘッダー = TJAPlayer3.tテクスチャの生成( CSkin.Path( @"Graphics\5_diffselect_header_panel.png" ) );
            this.txフッター = TJAPlayer3.tテクスチャの生成( CSkin.Path( @"Graphics\5_footer panel.png" ) );

            this.tx説明背景 = TJAPlayer3.tテクスチャの生成( CSkin.Path( @"Graphics\5_information_BG.png" ) );
            this.tx説明1 = TJAPlayer3.tテクスチャの生成( CSkin.Path( @"Graphics\5_information.png" ) );

            this.soundSelectAnnounce = TJAPlayer3.Sound管理.tサウンドを生成する( CSkin.Path( @"Sounds\DiffSelect.ogg" ), ESoundGroup.SoundEffect );

            for( int i = 0; i < (int)Difficulty.Total; i++ )
            {
                this.tx踏み台[ i ] = TJAPlayer3.tテクスチャの生成( CSkin.Path( @"Graphics\5_diffSelect_table" + i.ToString() + @".png" ) );
            }

			base.OnManagedリソースの作成();
		}
		public override void OnManagedリソースの解放()
		{
			if( this.b活性化してない )
				return;

            TJAPlayer3.tテクスチャの解放( ref this.tx背景 );
            TJAPlayer3.tテクスチャの解放( ref this.txヘッダー );
            TJAPlayer3.tテクスチャの解放( ref this.txフッター );

            TJAPlayer3.tテクスチャの解放( ref this.tx説明背景 );
            TJAPlayer3.tテクスチャの解放( ref this.tx説明1 );

            TJAPlayer3.t安全にDisposeする( ref this.soundSelectAnnounce );

            for( int i = 0; i < (int)Difficulty.Total; i++ )
            {
                TJAPlayer3.tテクスチャの解放( ref this.tx踏み台[ i ] );
            }

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
					this.ct登場アニメ用[ i ] = new CCounter( -i * 10, 100, 3, TJAPlayer3.Timer );
				this.nスクロールタイマ = CSound管理.rc演奏用タイマ.n現在時刻;
				TJAPlayer3.stage選曲.t選択曲変更通知();

                this.n矢印スクロール用タイマ値 = CSound管理.rc演奏用タイマ.n現在時刻;
				this.ct三角矢印アニメ.t開始( 0, 19, 40, TJAPlayer3.Timer );
				
                this.soundSelectAnnounce.tサウンドを再生する();
				base.b初めての進行描画 = false;
			}
			//-----------------
			#endregion

			// 本ステージは、(1)登場アニメフェーズ → (2)通常フェーズ　と二段階にわけて進む。
			// ２つしかフェーズがないので CStage.eフェーズID を使ってないところがまた本末転倒。

			
			// 進行。
            //this.ct三角矢印アニメ.t進行Loop();

            if( this.tx背景 != null )
                this.tx背景.t2D描画( TJAPlayer3.app.Device, 0, 0 );

			if( !this.b登場アニメ全部完了 )
			{
				#region [ (1) 登場アニメフェーズの進行。]
				//-----------------
				for( int i = 0; i < 13; i++ )	// パネルは全13枚。
				{
					this.ct登場アニメ用[ i ].t進行();

					if( this.ct登場アニメ用[ i ].b終了値に達した )
						this.ct登場アニメ用[ i ].t停止();
				}

				// 全部の進行が終わったら、this.b登場アニメ全部完了 を true にする。

				this.b登場アニメ全部完了 = true;
				for( int i = 0; i < 13; i++ )	// パネルは全13枚。
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

                //キー操作
                if( TJAPlayer3.Input管理.Keyboard.bキーが押された( (int) SlimDX.DirectInput.Key.RightArrow ) )
                {
                    this.t次に移動();
                }
                else if( TJAPlayer3.Input管理.Keyboard.bキーが押された( (int) SlimDX.DirectInput.Key.LeftArrow ) )
                {
                    this.t前に移動();
                }
                else if ( ( TJAPlayer3.Pad.b押されたDGB( Eパッド.Decide ) ||
						( ( TJAPlayer3.ConfigIni.bEnterがキー割り当てのどこにも使用されていない && TJAPlayer3.Input管理.Keyboard.bキーが押された( (int) SlimDX.DirectInput.Key.Return ) ) ) ) )
                {
                    TJAPlayer3.stage選曲.actPresound.tサウンド停止();
                    switch( TJAPlayer3.stage選曲.r現在選択中の曲.eノード種別 )
                    {
                        case C曲リストノード.Eノード種別.SCORE:
                            {
                                TJAPlayer3.Skin.sound決定音.t再生する();
                                TJAPlayer3.stage選曲.t曲を選択する( this.n現在の選択行 );
                            }
                            break;
                        case C曲リストノード.Eノード種別.RANDOM:
                            {
                                TJAPlayer3.Skin.sound曲決定音.t再生する();
                                TJAPlayer3.stage選曲.t曲を選択する( this.n現在の選択行 );
                            }
                            break;
                    }
                }
                else if( TJAPlayer3.Input管理.Keyboard.bキーが押された( (int) SlimDX.DirectInput.Key.F7 ) )
                {
                    this.bIsDifficltSelect = false;
                }

				//-----------------
				#endregion
			}


			// 描画。

            int i選曲バーX座標 = 673; //選曲バーの座標用
            int i選択曲バーX座標 = 665; //選択曲バーの座標用


			if( !this.b登場アニメ全部完了 )
			{
				#region [ (1) 登場アニメフェーズの描画。]
				//-----------------
				for( int i = 0; i < 4; i++ )	// パネルは全13枚。
				{

				}
				//-----------------
				#endregion
			}
			else
			{
				#region [ (2) 通常フェーズの描画。]
				//-----------------
                int nバー基準X = 64;
                TJAPlayer3.act文字コンソール.tPrint( 0, 32, C文字コンソール.Eフォント種別.白, this.n現在の選択行.ToString() );

				for( int i = 0; i < (int)Difficulty.Total; i++ )
				{
                    if( TJAPlayer3.stage選曲.r現在選択中の曲.arスコア[ i ] == null )
                        continue;

                    string strFlag = this.n現在の選択行 == i ? "NowSelect" : "UnSelect";
                    C文字コンソール.Eフォント種別 bColorFlag = this.n現在の選択行 == i ? C文字コンソール.Eフォント種別.赤 : C文字コンソール.Eフォント種別.灰;

                    nバー基準X = nバー基準X + 16;
                    TJAPlayer3.act文字コンソール.tPrint( 0, nバー基準X, bColorFlag, strFlag );

                    
				}

                //1→3→5→2→4の順で描画する。

				for( int j = 0; j < (int)Difficulty.Total; j++ )
				{
                    if( TJAPlayer3.stage選曲.r現在選択中の曲.arスコア[ n描画順[ j ] ] == null )
                        continue;
                    //if( j == 4 )
                    //    break;

                    if( this.tx踏み台[ n描画順[ j ] ] != null )
                    {
                        bool bEven = false;
                        if( n描画順[ j ] % 2 == 0 && n描画順[ j ] != 0 )
                            bEven = true;

                        this.tx踏み台[ n描画順[ j ] ].t2D描画( TJAPlayer3.app.Device, n踏み台座標[ j ], 720 - this.tx踏み台[ n描画順[ j ] ].szテクスチャサイズ.Height + ( bEven ? 35 : 0 ) );
                    }
				}


				//-----------------
				#endregion
			}
            if( this.txヘッダー != null )
                this.txヘッダー.t2D描画( TJAPlayer3.app.Device, 0, 0 );
            if( this.txフッター != null )
                this.txフッター.t2D描画( TJAPlayer3.app.Device, 0, 720 - this.txフッター.sz画像サイズ.Height );

            if( this.tx説明背景 != null )
                this.tx説明背景.t2D描画( TJAPlayer3.app.Device, 340, 600 );
            if( this.tx説明1 != null )
                this.tx説明1.t2D描画( TJAPlayer3.app.Device, 340, 600 );

			return 0;
		}
		

		// その他

		#region [ private ]
		//-----------------

		private bool b登場アニメ全部完了;
		private CCounter[] ct登場アニメ用 = new CCounter[ 13 ];
        private CCounter ct三角矢印アニメ;
        private CCounter ct移動;
		private long nスクロールタイマ;
		private int n現在のスクロールカウンタ;
		private int n現在の選択行;
		private int n目標のスクロールカウンタ;

        private CTexture[] tx踏み台 = new CTexture[(int)Difficulty.Total];

        private CTexture tx背景;
        private CTexture txヘッダー;
        private CTexture txフッター;

        private CTexture tx説明背景;
        private CTexture tx説明1;

        private CSound soundSelectAnnounce;


        private long n矢印スクロール用タイマ値;

        private int[] n描画順;
        private int[] n踏み台座標;
		//-----------------
		#endregion
	}
}
