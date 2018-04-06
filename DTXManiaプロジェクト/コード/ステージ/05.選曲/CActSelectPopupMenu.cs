using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.IO;
using FDK;


namespace DTXMania
{
	internal class CActSelectPopupMenu : CActivity
	{

		// プロパティ

	
		public int GetIndex(int pos)
		{
			return lciMenuItems[ pos ].cItem.GetIndex();
		}
		public object GetObj現在値( int pos )
		{
			return lciMenuItems[ pos ].cItem.obj現在値();
		}
		public bool bGotoDetailConfig
		{
			get;
			internal set;
		}

		/// <summary>
		/// ソートメニュー機能を使用中かどうか。外部からこれをtrueにすると、ソートメニューが出現する。falseにすると消える。
		/// </summary>
		public bool bIsActivePopupMenu
		{
			get;
			private set;
		}
		public virtual void tActivatePopupMenu( E楽器パート einst )
		{
			nItemSelecting = -1;		// #24757 2011.4.1 yyagi: Clear sorting status in each stating menu.
			this.eInst = einst;
			this.bIsActivePopupMenu = true;
			this.bIsSelectingIntItem = false;
			this.bGotoDetailConfig = false;
		}
		public virtual void tDeativatePopupMenu()
		{
			this.bIsActivePopupMenu = false;
		}


		public void Initialize( List<CItemBase> menulist, bool showAllItems, string title )
		{
			Initialize( menulist, showAllItems, title, 0 );
		}

		public void Initialize( List<CItemBase> menulist, bool showAllItems, string title, int defaultPos )
		{
			prvFont = new CPrivateFastFont( CSkin.Path( @"Graphics\ipag.ttf" ), 18 );

			stqMenuTitle = new stQuickMenuItem();
			stqMenuTitle.cItem = new CItemBase();
			stqMenuTitle.cItem.str項目名 = title;
			stqMenuTitle.txName = CDTXMania.tテクスチャの生成( prvFont.DrawPrivateFont( title, Color.White, Color.Black ), false );
			stqMenuTitle.rectName = prvFont.RectStrings;
			lciMenuItems = new stQuickMenuItem[ menulist.Count ];
			for (int i = 0; i < menulist.Count; i++ )
			{
				stQuickMenuItem stqm = new stQuickMenuItem();
				stqm.cItem = menulist[ i ];
				stqm.txName = CDTXMania.tテクスチャの生成( prvFont.DrawPrivateFont( menulist[ i ].str項目名, Color.White, Color.Black ), false );
				stqm.rectName = prvFont.RectStrings;
				lciMenuItems[ i ] = stqm;
			}

			bShowAllItems = showAllItems;
			n現在の選択行 = defaultPos;
		}


		public void tEnter押下()
		{
			if ( this.bキー入力待ち )
			{
				CDTXMania.Skin.sound決定音.t再生する();

				if ( this.n現在の選択行 != lciMenuItems.Length - 1 )
				{
					if ( lciMenuItems[ n現在の選択行 ].cItem.e種別 == CItemBase.E種別.リスト ||
						 lciMenuItems[ n現在の選択行 ].cItem.e種別 == CItemBase.E種別.ONorOFFトグル ||
						 lciMenuItems[ n現在の選択行 ].cItem.e種別 == CItemBase.E種別.ONorOFFor不定スリーステート	)
					{
						lciMenuItems[ n現在の選択行 ].cItem.t項目値を次へ移動();
					}
					else if ( lciMenuItems[ n現在の選択行 ].cItem.e種別 == CItemBase.E種別.整数 )
					{
						bIsSelectingIntItem = !bIsSelectingIntItem;		// 選択状態/選択解除状態を反転する
					}
					else if ( lciMenuItems[ n現在の選択行 ].cItem.e種別 == CItemBase.E種別.切替リスト )
					{
						// 特に何もしない
					}
					else
					{
						throw new ArgumentException();
					}
					nItemSelecting = n現在の選択行;
				}
				tEnter押下Main( (int) lciMenuItems[ n現在の選択行 ].cItem.GetIndex() );

				this.bキー入力待ち = true;
			}
		}

		/// <summary>
		/// Decide押下時の処理を、継承先で記述する。
		/// </summary>
		/// <param name="val">CItemBaseの現在の設定値のindex</param>
		public virtual void tEnter押下Main( int val )
		{
		}
		/// <summary>
		/// Cancel押下時の追加処理があれば、継承先で記述する。
		/// </summary>
		public virtual void tCancel()
		{
		}
		/// <summary>
		/// 追加の描画処理。必要に応じて、継承先で記述する。
		/// </summary>
		public virtual void t進行描画sub()
		{
		}

		public void t次に移動()
		{
			if ( this.bキー入力待ち )
			{
				CDTXMania.Skin.soundカーソル移動音.t再生する();
				if ( bIsSelectingIntItem )
				{
					 lciMenuItems[ n現在の選択行 ].cItem.t項目値を前へ移動();		// 項目移動と数値上下は方向が逆になるので注意
				}
				else
				{
					if ( ++this.n現在の選択行 >= this.lciMenuItems.Length )
					{
						this.n現在の選択行 = 0;
					}
				}
			}
		}
		public void t前に移動()
		{
			if ( this.bキー入力待ち )
			{
				CDTXMania.Skin.soundカーソル移動音.t再生する();
				if ( bIsSelectingIntItem )
				{
					lciMenuItems[ n現在の選択行 ].cItem.t項目値を次へ移動();		// 項目移動と数値上下は方向が逆になるので注意
				}
				else
				{
					if ( --this.n現在の選択行 < 0 )
					{
						this.n現在の選択行 = this.lciMenuItems.Length - 1;
					}
				}
			}
		}

		// CActivity 実装

		public override void On活性化()
		{
	//		this.n現在の選択行 = 0;
			this.bキー入力待ち = true;
			for ( int i = 0; i < 4; i++ )
			{
				this.ctキー反復用[ i ] = new CCounter( 0, 0, 0, CDTXMania.Timer );
			}
			base.b活性化してない = true;

			this.bIsActivePopupMenu = false;
			this.font = new CActDFPFont();
			base.list子Activities.Add( this.font );
			nItemSelecting = -1;

			base.On活性化();
		}
		public override void On非活性化()
		{
			if ( !base.b活性化してない )
			{
				base.list子Activities.Remove( this.font );
				this.font.On非活性化();
				this.font = null;

				//CDTXMania.tテクスチャの解放( ref this.txCursor );
				//CDTXMania.tテクスチャの解放( ref this.txPopupMenuBackground );
				for ( int i = 0; i < 4; i++ )
				{
					this.ctキー反復用[ i ] = null;
				}
				base.On非活性化();
			}
		}
		public override void OnManagedリソースの作成()
		{
			if ( !base.b活性化してない )
			{
				//string pathCursor = CSkin.Path( @"Graphics\5_menu cursor.png" );;
				//string pathPopupMenuBackground = CSkin.Path( @"Graphics\5_sort menu background.png" );
				//if ( File.Exists ( pathCursor ) )
				//{
				//	this.txCursor = CDTXMania.tテクスチャの生成( pathCursor, false );
				//}
				//if ( File.Exists( pathPopupMenuBackground ) )
				//{
				//	this.txPopupMenuBackground = CDTXMania.tテクスチャの生成( pathPopupMenuBackground, false );
				//}
				base.OnManagedリソースの作成();
			}
		}
		public override void OnManagedリソースの解放()
		{
			if ( !base.b活性化してない )
			{
				//CDTXMania.tテクスチャの解放( ref this.txPopupMenuBackground );
				//CDTXMania.tテクスチャの解放( ref this.txCursor );
                CDTXMania.t安全にDisposeする( ref this.prvFont );
			}
			base.OnManagedリソースの解放();
		}

		public override int On進行描画()
		{
			throw new InvalidOperationException( "t進行描画(bool)のほうを使用してください。" );
		}

		public int t進行描画()
		{
			if ( !base.b活性化してない && this.bIsActivePopupMenu )
			{
				if ( this.bキー入力待ち )
				{
					#region [ Shift-F1: CONFIG画面 ]
					if ( ( CDTXMania.Input管理.Keyboard.bキーが押されている( (int) SlimDX.DirectInput.Key.RightShift ) || CDTXMania.Input管理.Keyboard.bキーが押されている( (int) SlimDX.DirectInput.Key.LeftShift ) ) &&
						CDTXMania.Input管理.Keyboard.bキーが押された( (int) SlimDX.DirectInput.Key.F1 ) )
					{	// [SHIFT] + [F1] CONFIG
						CDTXMania.Skin.sound取消音.t再生する();
						tCancel();
						this.bGotoDetailConfig = true;
					}
					#endregion
					#region [ キー入力: キャンセル ]
					else if ( ( CDTXMania.Input管理.Keyboard.bキーが押された( (int) SlimDX.DirectInput.Key.Escape )
						|| CDTXMania.Pad.b押された( E楽器パート.DRUMS, Eパッド.FT )
						|| CDTXMania.Pad.b押されたGB( Eパッド.Cancel ) )
                        && this.bEsc有効 )
					{	// キャンセル
						CDTXMania.Skin.sound取消音.t再生する();
						tCancel();
						this.bIsActivePopupMenu = false;
					}
					#endregion

					#region [ キー入力: 決定 ]
					// E楽器パート eInst = E楽器パート.UNKNOWN;
					ESortAction eAction = ESortAction.END;
					if ( CDTXMania.Pad.b押された( E楽器パート.GUITAR, Eパッド.Decide ) )
					{
						eInst = E楽器パート.GUITAR;
						eAction = ESortAction.Decide;
					}
					else if ( CDTXMania.Pad.b押された( E楽器パート.BASS, Eパッド.Decide ) )
					{
						eInst = E楽器パート.BASS;
						eAction = ESortAction.Decide;
					}
					else if (
						CDTXMania.Pad.b押された( E楽器パート.DRUMS, Eパッド.Decide )	// #24756 2011.4.1 yyagi: Add condition "Drum-Decide" to enable CY in Sort Menu.
						|| CDTXMania.Pad.b押された( E楽器パート.DRUMS, Eパッド.RD )
						|| CDTXMania.Pad.b押された( E楽器パート.DRUMS, Eパッド.LC )
						|| ( CDTXMania.ConfigIni.bEnterがキー割り当てのどこにも使用されていない && CDTXMania.Input管理.Keyboard.bキーが押された( (int) SlimDX.DirectInput.Key.Return ) ) )
					{
						eInst = E楽器パート.DRUMS;
						eAction = ESortAction.Decide;
					}
					if ( eAction == ESortAction.Decide )	// 決定
					{
						this.tEnter押下();
					}
					#endregion
					#region [ キー入力: 前に移動 ]
					this.ctキー反復用.Up.tキー反復( CDTXMania.Input管理.Keyboard.bキーが押されている( (int) SlimDX.DirectInput.Key.UpArrow ), new CCounter.DGキー処理( this.t前に移動 ) );
					this.ctキー反復用.R.tキー反復( CDTXMania.Pad.b押されているGB( Eパッド.R ), new CCounter.DGキー処理( this.t前に移動 ) );
					if ( CDTXMania.Pad.b押された( E楽器パート.DRUMS, Eパッド.SD ) )
					{
						this.t前に移動();
					}
					#endregion
					#region [ キー入力: 次に移動 ]
					this.ctキー反復用.Down.tキー反復( CDTXMania.Input管理.Keyboard.bキーが押されている( (int) SlimDX.DirectInput.Key.DownArrow ), new CCounter.DGキー処理( this.t次に移動 ) );
					this.ctキー反復用.B.tキー反復( CDTXMania.Pad.b押されているGB( Eパッド.B ), new CCounter.DGキー処理( this.t次に移動 ) );
					if ( CDTXMania.Pad.b押された( E楽器パート.DRUMS, Eパッド.LT ) )
					{
						this.t次に移動();
					}
					#endregion
				}
				#region [ ポップアップメニュー 背景描画 ]
				if ( CDTXMania.Tx.Menu_Title != null )
				{
                    CDTXMania.Tx.Menu_Title.t2D描画( CDTXMania.app.Device, 160, 40 );
				}
				#endregion
				#region [ ソートメニュータイトル描画 ]
				int x = 240, y = 44;
				stqMenuTitle.txName.t2D描画( CDTXMania.app.Device, x, y );
				#endregion
				#region [ カーソル描画 ]
				if ( CDTXMania.Tx.Menu_Highlight != null )
				{
					int height = 32;
					int curX = 180;
					int curY = 46 + ( height * ( this.n現在の選択行 + 1 ) );
                    CDTXMania.Tx.Menu_Highlight.t2D描画( CDTXMania.app.Device, curX, curY, new Rectangle( 0, 0, 16, 32 ) );
					curX += 0x10;
					Rectangle rectangle = new Rectangle( 8, 0, 0x10, 0x20 );
					for ( int j = 0; j < 16; j++ )
					{
                        CDTXMania.Tx.Menu_Highlight.t2D描画( CDTXMania.app.Device, curX, curY, rectangle );
						curX += 16;
					}
                    CDTXMania.Tx.Menu_Highlight.t2D描画( CDTXMania.app.Device, curX, curY, new Rectangle( 0x10, 0, 16, 32 ) );
				}
				#endregion
				#region [ ソート候補文字列描画 ]
				for ( int i = 0; i < lciMenuItems.Length; i++ )
				{
					bool bItemBold = ( i == nItemSelecting && !bShowAllItems ) ? true : false;
					//font.t文字列描画( 190, 80 + i * 32, lciMenuItems[ i ].cItem.str項目名, bItemBold, 1.0f );
					if ( lciMenuItems[ i ].txName != null )
					{
						int height = lciMenuItems[ i ].rectName.Height;
						lciMenuItems[ i ].txName.t2D描画( CDTXMania.app.Device, 180, 77 + i * 32 );
					}

                    bool bValueBold = (bItemBold || (i == nItemSelecting && bIsSelectingIntItem)) ? true : false;
                    if (bItemBold || bShowAllItems)
                    {
                        string s;
                        switch (lciMenuItems[i].cItem.str項目名)
                        {
                            case "演奏速度":
                                {
                                    double d = (double)((int)lciMenuItems[i].cItem.obj現在値() / 20.0);
                                    s = "x" + d.ToString("0.000");
                                }
                                break;
                            case "ばいそく":
                                {
                                    double d = (double)((((int)lciMenuItems[i].cItem.obj現在値()) + 1) / 2.0);
                                    s = "x" + d.ToString("0.0");
                                }
                                break;

                            default:
                                s = lciMenuItems[i].cItem.obj現在値().ToString();
                                break;
                        }
                        //font.t文字列描画( (int)(340 * Scale.X), (int)(80 + i * 32), s, bValueBold, 1.0f * Scale.Y);
                        Bitmap bmpStr = bValueBold ?
                            prvFont.DrawPrivateFont(s, Color.White, Color.Black, Color.Yellow, Color.OrangeRed) :
                            prvFont.DrawPrivateFont(s, Color.White, Color.Black);
                        CTexture ctStr = CDTXMania.tテクスチャの生成(bmpStr, false);
                        ctStr.t2D描画(CDTXMania.app.Device, 330, 77 + i * 32);
                        CDTXMania.tテクスチャの解放(ref ctStr);
                    }
				}
				#endregion
				t進行描画sub();
			}
			return 0;
		}


		// その他

		#region [ private ]
		//-----------------
		private bool bキー入力待ち;
        protected bool bEsc有効;

		internal int n現在の選択行;
		internal E楽器パート eInst = E楽器パート.UNKNOWN;

		//private CTexture txPopupMenuBackground;
		//private CTexture txCursor;
		private CActDFPFont font;
        CPrivateFastFont prvFont;

        internal struct stQuickMenuItem
        {
            internal CItemBase cItem;
            internal CTexture txName;
            internal Rectangle rectName;
        }
        private stQuickMenuItem[] lciMenuItems;
        private stQuickMenuItem stqMenuTitle;
		private string strMenuTitle;
		private bool bShowAllItems;
		private bool bIsSelectingIntItem;

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
					switch ( index )
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
					switch ( index )
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
		private STキー反復用カウンタ ctキー反復用;

		private enum ESortAction : int
		{
			Cancel, Decide, Previous, Next, END
		}
		private int nItemSelecting;		// 「n現在の選択行」とは別に設ける。sortでメニュー表示直後にアイテムの中身を表示しないようにするため
		//-----------------
		#endregion
	}
}
