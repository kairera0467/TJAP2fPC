﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using SlimDX;
using FDK;

namespace DTXMania
{
	/// <summary>
	/// プライベートフォントでの描画を扱うクラス。
	/// </summary>
	/// <exception cref="FileNotFoundException">フォントファイルが見つからない時に例外発生</exception>
	/// <exception cref="ArgumentException">スタイル指定不正時に例外発生</exception>
	/// <remarks>
	/// 簡単な使い方
	/// CPrivateFont prvFont = new CPrivateFont( CSkin.Path( @"Graphics\fonts\mplus-1p-bold.ttf" ), 36 );	// プライベートフォント
	/// とか
	/// CPrivateFont prvFont = new CPrivateFont( new FontFamily("MS UI Gothic"), 36, FontStyle.Bold );		// システムフォント
	/// とかした上で、
	/// Bitmap bmp = prvFont.DrawPrivateFont( "ABCDE", Color.White, Color.Black );							// フォント色＝白、縁の色＝黒の例。縁の色は省略可能
	/// とか
	/// Bitmap bmp = prvFont.DrawPrivateFont( "ABCDE", Color.White, Color.Black, Color.Yellow, Color.OrangeRed ); // 上下グラデーション(Yellow→OrangeRed)
	/// とかして、
	/// CTexture ctBmp = CDTXMania.tテクスチャの生成( bmp, false );
	/// ctBMP.t2D描画( ～～～ );
	/// で表示してください。
	/// 
	/// 注意点
	/// 任意のフォントでのレンダリングは結構負荷が大きいので、なるべｋなら描画フレーム毎にフォントを再レンダリングするようなことはせず、
	/// 一旦レンダリングしたものを描画に使い回すようにしてください。
	/// また、長い文字列を与えると、返されるBitmapも横長になります。この横長画像をそのままテクスチャとして使うと、
	/// 古いPCで問題を発生させやすいです。これを回避するには、一旦Bitmapとして取得したのち、256pixや512pixで分割して
	/// テクスチャに定義するようにしてください。
	/// </remarks>
	public class CPrivateFont : IDisposable
	{
		/// <summary>
		/// プライベートフォントのFontクラス。CPrivateFont()の初期化後に使用可能となる。
		/// プライベートフォントでDrawString()したい場合にご利用ください。
		/// </summary>
		public Font font
		{
			get => _font;
		}

		/// <summary>
		/// フォント登録失敗時に代替使用するフォント名。システムフォントのみ設定可能。
		/// 後日外部指定できるようにします。(＝コンストラクタで指定できるようにします)
		/// </summary>
		private string strAlternativeFont = "MS PGothic";

		#region [ コンストラクタ ]
		public CPrivateFont( FontFamily fontfamily, float pt, FontStyle style )
		{
			Initialize( null, null, fontfamily, pt, style );
		}
		public CPrivateFont( FontFamily fontfamily, float pt )
		{
			Initialize( null, null, fontfamily, pt, FontStyle.Regular );
		}
		public CPrivateFont(string fontpath, FontFamily fontfamily, float pt, FontStyle style)
		{
			Initialize(fontpath, null, fontfamily, pt, style);
		}
		public CPrivateFont( string fontpath, float pt, FontStyle style )
		{
			Initialize( fontpath, null, null, pt, style );
		}
		public CPrivateFont( string fontpath, float pt )
		{
			Initialize( fontpath, null, null, pt, FontStyle.Regular );
		}
		public CPrivateFont()
		{
			//throw new ArgumentException("CPrivateFont: 引数があるコンストラクタを使用してください。");
		}
		#endregion

		protected void Initialize(string fontpath, string baseFontPath, FontFamily fontfamily, float pt, FontStyle style)
		{
			this._pfc = null;
			this._fontfamily = null;
			this._font = null;
			this._pt = pt;
			this._rectStrings = new Rectangle(0, 0, 0, 0);
			this._ptOrigin = new Point(0, 0);
			this.bDispose完了済み = false;
			this._baseFontname = baseFontPath;
			this.bIsSystemFont = false;

			float emSize = 0f;
			using (Bitmap b = new Bitmap(1, 1))
			{
				using (Graphics g = Graphics.FromImage(b))
				{
					emSize = pt * 96.0f / 72.0f * g.DpiX / 96.0f;	// DPIを考慮したpxサイズ。GraphicsUnit.Pixelと併用のこと
				}
			}

			if (fontfamily != null)
			{
				this._fontfamily = fontfamily;
			}
			else
			{
				try
				{
					//拡張子あり == 通常のPrivateFontパス指定
					if (Path.GetExtension(fontpath) != string.Empty)
					{
						this._pfc = new System.Drawing.Text.PrivateFontCollection();    //PrivateFontCollectionオブジェクトを作成する
						this._pfc.AddFontFile(fontpath);                                //PrivateFontCollectionにフォントを追加する
						_fontfamily = _pfc.Families[0];
						bIsSystemFont = false;
					}
					//拡張子なし == Arial, MS Gothicなど、システムフォントの指定
					else
					{
						this._font = PublicFont(Path.GetFileName(fontpath), emSize, style, GraphicsUnit.Pixel); 
						bIsSystemFont = true;
					}
				}
				catch (Exception e) when (e is System.IO.FileNotFoundException || e is System.Runtime.InteropServices.ExternalException)
				{
					Trace.TraceWarning(e.Message);
					Trace.TraceWarning("プライベートフォントの追加に失敗しました({0})。代わりに{1}の使用を試みます。", fontpath, strAlternativeFont);
					//throw new FileNotFoundException( "プライベートフォントの追加に失敗しました。({0})", Path.GetFileName( fontpath ) );
					//return;

					_fontfamily = null;
				}

				//foreach ( FontFamily ff in _pfc.Families )
				//{
				//	Debug.WriteLine( "fontname=" + ff.Name );
				//	if ( ff.Name == Path.GetFileNameWithoutExtension( fontpath ) )
				//	{
				//		_fontfamily = ff;
				//		break;
				//	}
				//}
				//if ( _fontfamily == null )
				//{
				//	Trace.TraceError( "プライベートフォントの追加後、検索に失敗しました。({0})", fontpath );
				//	return;
				//}
			}

			// システムフォントの登録に成功した場合
			if (bIsSystemFont && _font != null)
			{
				// 追加処理なし。何もしない
			}
			// PrivateFontの登録に成功した場合は、指定されたフォントスタイルをできるだけ適用する
			else if (_fontfamily != null)
			{
				if (!_fontfamily.IsStyleAvailable(style))
				{
					FontStyle[] FS = { FontStyle.Regular, FontStyle.Bold, FontStyle.Italic, FontStyle.Underline, FontStyle.Strikeout };
					style = FontStyle.Regular | FontStyle.Bold | FontStyle.Italic | FontStyle.Underline | FontStyle.Strikeout;  // null非許容型なので、代わりに全盛をNGワードに設定
					foreach (FontStyle ff in FS)
					{
						if (this._fontfamily.IsStyleAvailable(ff))
						{
							style = ff;
							Trace.TraceWarning("フォント{0}へのスタイル指定を、{1}に変更しました。", Path.GetFileName(fontpath), style.ToString());
							break;
						}
					}
					if (style == (FontStyle.Regular | FontStyle.Bold | FontStyle.Italic | FontStyle.Underline | FontStyle.Strikeout))
					{
						Trace.TraceWarning("フォント{0}は適切なスタイル{1}を選択できませんでした。", Path.GetFileName(fontpath), style.ToString());
					}
				}
				this._font = new Font(this._fontfamily, emSize, style, GraphicsUnit.Pixel); //PrivateFontCollectionの先頭のフォントのFontオブジェクトを作成する
			}
			// PrivateFontと通常フォント、どちらの登録もできていない場合は、MS PGothic改め代替フォントを代わりに設定しようと試みる
			else
			{
				this._font = PublicFont(strAlternativeFont, emSize, style, GraphicsUnit.Pixel);	
				if (this._font != null )
				{ 
					Trace.TraceInformation("{0}の代わりに{1}を指定しました。", Path.GetFileName(fontpath), strAlternativeFont);
					bIsSystemFont = true;
					return;
				}
				throw new FileNotFoundException(string.Format("プライベートフォントの追加に失敗し、{1}での代替処理にも失敗しました。({0})", Path.GetFileName(fontpath), strAlternativeFont));
			}
		}

		/// <summary>
		/// プライベートフォントではない、システムフォントの設定
		/// </summary>
		/// <param name="fontname">フォント名</param>
		/// <param name="emSize">フォントサイズ</param>
		/// <param name="style">フォントスタイル</param>
		/// <param name="unit">GraphicsUnit</param>
		/// <returns></returns>
		private Font PublicFont(string fontname, float emSize, FontStyle style, GraphicsUnit unit)
		{
			Font f = new Font(fontname, emSize, style, unit);
			FontFamily[] ffs = new System.Drawing.Text.InstalledFontCollection().Families;
			int lcid = System.Globalization.CultureInfo.GetCultureInfo("en-us").LCID;
			foreach (FontFamily ff in ffs)
			{
				// Trace.WriteLine( lcid ) );
				if (ff.GetName(lcid) == fontname)
				{
					this._fontfamily = ff;
					return f;
				}
			}
			return null;
		}

		[Flags]
		public enum DrawMode
		{
			Normal,
			Edge,
			Gradation,
            Vertical
		}

		#region [ DrawPrivateFontのオーバーロード群 ]
		/// <summary>
		/// 文字列を描画したテクスチャを返す
		/// </summary>
		/// <param name="drawstr">描画文字列</param>
		/// <param name="fontColor">描画色</param>
		/// <returns>描画済テクスチャ</returns>
		public Bitmap DrawPrivateFont( string drawstr, Color fontColor )
		{
			return DrawPrivateFont( drawstr, DrawMode.Normal, fontColor, Color.White, Color.White, Color.White );
		}

		/// <summary>
		/// 文字列を描画したテクスチャを返す
		/// </summary>
		/// <param name="drawstr">描画文字列</param>
		/// <param name="fontColor">描画色</param>
		/// <param name="edgeColor">縁取色</param>
		/// <returns>描画済テクスチャ</returns>
		public Bitmap DrawPrivateFont( string drawstr, Color fontColor, Color edgeColor )
		{
			return DrawPrivateFont( drawstr, DrawMode.Edge, fontColor, edgeColor, Color.White, Color.White );
		}

		/// <summary>
		/// 文字列を描画したテクスチャを返す
		/// </summary>
		/// <param name="drawstr">描画文字列</param>
		/// <param name="fontColor">描画色</param>
		/// <param name="gradationTopColor">グラデーション 上側の色</param>
		/// <param name="gradationBottomColor">グラデーション 下側の色</param>
		/// <returns>描画済テクスチャ</returns>
		//public Bitmap DrawPrivateFont( string drawstr, Color fontColor, Color gradationTopColor, Color gradataionBottomColor )
		//{
		//    return DrawPrivateFont( drawstr, DrawMode.Gradation, fontColor, Color.White, gradationTopColor, gradataionBottomColor );
		//}

		/// <summary>
		/// 文字列を描画したテクスチャを返す
		/// </summary>
		/// <param name="drawstr">描画文字列</param>
		/// <param name="fontColor">描画色</param>
		/// <param name="edgeColor">縁取色</param>
		/// <param name="gradationTopColor">グラデーション 上側の色</param>
		/// <param name="gradationBottomColor">グラデーション 下側の色</param>
		/// <returns>描画済テクスチャ</returns>
		public Bitmap DrawPrivateFont( string drawstr, Color fontColor, Color edgeColor, Color gradationTopColor, Color gradataionBottomColor )
		{
			return DrawPrivateFont( drawstr, DrawMode.Edge | DrawMode.Gradation, fontColor, edgeColor, gradationTopColor, gradataionBottomColor );
		}

#if こちらは使わない // (Bitmapではなく、CTextureを返す版)
		/// <summary>
		/// 文字列を描画したテクスチャを返す
		/// </summary>
		/// <param name="drawstr">描画文字列</param>
		/// <param name="fontColor">描画色</param>
		/// <returns>描画済テクスチャ</returns>
		public CTexture DrawPrivateFont( string drawstr, Color fontColor )
		{
			Bitmap bmp = DrawPrivateFont( drawstr, DrawMode.Normal, fontColor, Color.White, Color.White, Color.White );
			return CDTXMania.tテクスチャの生成( bmp, false );
		}

		/// <summary>
		/// 文字列を描画したテクスチャを返す
		/// </summary>
		/// <param name="drawstr">描画文字列</param>
		/// <param name="fontColor">描画色</param>
		/// <param name="edgeColor">縁取色</param>
		/// <returns>描画済テクスチャ</returns>
		public CTexture DrawPrivateFont( string drawstr, Color fontColor, Color edgeColor )
		{
			Bitmap bmp = DrawPrivateFont( drawstr, DrawMode.Edge, fontColor, edgeColor, Color.White, Color.White );
			return CDTXMania.tテクスチャの生成( bmp, false );
		}

		/// <summary>
		/// 文字列を描画したテクスチャを返す
		/// </summary>
		/// <param name="drawstr">描画文字列</param>
		/// <param name="fontColor">描画色</param>
		/// <param name="gradationTopColor">グラデーション 上側の色</param>
		/// <param name="gradationBottomColor">グラデーション 下側の色</param>
		/// <returns>描画済テクスチャ</returns>
		//public CTexture DrawPrivateFont( string drawstr, Color fontColor, Color gradationTopColor, Color gradataionBottomColor )
		//{
		//    Bitmap bmp = DrawPrivateFont( drawstr, DrawMode.Gradation, fontColor, Color.White, gradationTopColor, gradataionBottomColor );
		//	  return CDTXMania.tテクスチャの生成( bmp, false );
		//}

		/// <summary>
		/// 文字列を描画したテクスチャを返す
		/// </summary>
		/// <param name="drawstr">描画文字列</param>
		/// <param name="fontColor">描画色</param>
		/// <param name="edgeColor">縁取色</param>
		/// <param name="gradationTopColor">グラデーション 上側の色</param>
		/// <param name="gradationBottomColor">グラデーション 下側の色</param>
		/// <returns>描画済テクスチャ</returns>
		public CTexture DrawPrivateFont( string drawstr, Color fontColor, Color edgeColor,  Color gradationTopColor, Color gradataionBottomColor )
		{
			Bitmap bmp = DrawPrivateFont( drawstr, DrawMode.Edge | DrawMode.Gradation, fontColor, edgeColor, gradationTopColor, gradataionBottomColor );
			return CDTXMania.tテクスチャの生成( bmp, false );
		}
#endif
		#endregion


		/// <summary>
		/// 文字列を描画したテクスチャを返す(メイン処理)
		/// </summary>
		/// <param name="rectDrawn">描画された領域</param>
		/// <param name="ptOrigin">描画文字列</param>
		/// <param name="drawstr">描画文字列</param>
		/// <param name="drawmode">描画モード</param>
		/// <param name="fontColor">描画色</param>
		/// <param name="edgeColor">縁取色</param>
		/// <param name="gradationTopColor">グラデーション 上側の色</param>
		/// <param name="gradationBottomColor">グラデーション 下側の色</param>
		/// <returns>描画済テクスチャ</returns>
		protected Bitmap DrawPrivateFont( string drawstr, DrawMode drawmode, Color fontColor, Color edgeColor, Color gradationTopColor, Color gradationBottomColor )
		{
			if ( this._fontfamily == null || drawstr == null || drawstr == "" )
			{
				// nullを返すと、その後bmp→texture処理や、textureのサイズを見て__の処理で全部例外が発生することになる。
				// それは非常に面倒なので、最小限のbitmapを返してしまう。
				// まずはこの仕様で進めますが、問題有れば(上位側からエラー検出が必要であれば)例外を出したりエラー状態であるプロパティを定義するなり検討します。
				if ( drawstr != "" )
				{
					Trace.TraceWarning( "DrawPrivateFont()の入力不正。最小値のbitmapを返します。" );
				}
				_rectStrings = new Rectangle( 0, 0, 0, 0 );
				_ptOrigin = new Point( 0, 0 );
				return new Bitmap(1, 1);
			}
			bool bEdge =      ( ( drawmode & DrawMode.Edge      ) == DrawMode.Edge );
			bool bGradation = ( ( drawmode & DrawMode.Gradation ) == DrawMode.Gradation );

			// 縁取りの縁のサイズは、とりあえずフォントの大きさの1/4とする
			float nEdgePt = (bEdge)? _pt / 4 : 0;

			// 描画サイズを測定する
			Size stringSize = System.Windows.Forms.TextRenderer.MeasureText( drawstr, this._font, new Size( int.MaxValue, int.MaxValue ),
				System.Windows.Forms.TextFormatFlags.NoPrefix |
				System.Windows.Forms.TextFormatFlags.NoPadding
			);
            stringSize.Width += 10; //2015.04.01 kairera0467 ROTTERDAM NATIONの描画サイズがうまくいかんので。

			//取得した描画サイズを基に、描画先のbitmapを作成する
			Bitmap bmp = new Bitmap( (int)(stringSize.Width + nEdgePt * 2), (int)(stringSize.Height + nEdgePt * 2) );
			bmp.MakeTransparent();
			Graphics g = Graphics.FromImage( bmp );
			g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

			StringFormat sf = new StringFormat();
			sf.LineAlignment = StringAlignment.Far;	// 画面下部（垂直方向位置）
			sf.Alignment = StringAlignment.Center;	// 画面中央（水平方向位置）

			// レイアウト枠
			Rectangle r = new Rectangle( 0, 0, (int)(stringSize.Width + nEdgePt * 2), (int)(stringSize.Height + nEdgePt * 2) );

			if ( bEdge )	// 縁取り有りの描画
			{
				// DrawPathで、ポイントサイズを使って描画するために、DPIを使って単位変換する
				// (これをしないと、単位が違うために、小さめに描画されてしまう)
				float sizeInPixels = _font.SizeInPoints * g.DpiY / 72;  // 1 inch = 72 points

				System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
				gp.AddString( drawstr, this._fontfamily, (int) this._font.Style, sizeInPixels, r, sf );

				// 縁取りを描画する
				Pen p = new Pen( edgeColor, nEdgePt );
				p.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
				g.DrawPath( p, gp );

				// 塗りつぶす
				Brush br;
				if ( bGradation )
				{
					br = new LinearGradientBrush( r, gradationTopColor, gradationBottomColor, LinearGradientMode.Vertical );
				}
				else
				{
					br = new SolidBrush( fontColor );
				}
				g.FillPath( br, gp );

				if ( br != null ) br.Dispose(); br = null;
				if ( p != null ) p.Dispose(); p = null;
				if ( gp != null ) gp.Dispose(); gp = null;
			}
			else
			{
				// 縁取りなしの描画
				System.Windows.Forms.TextRenderer.DrawText( g, drawstr, _font, new Point( 0, 0 ), fontColor );
			}
#if debug表示
			g.DrawRectangle( new Pen( Color.White, 1 ), new Rectangle( 1, 1, stringSize.Width-1, stringSize.Height-1 ) );
			g.DrawRectangle( new Pen( Color.Green, 1 ), new Rectangle( 0, 0, bmp.Width - 1, bmp.Height - 1 ) );
#endif
			_rectStrings = new Rectangle( 0, 0, stringSize.Width, stringSize.Height );
			_ptOrigin = new Point( (int)(nEdgePt * 2), (int)(nEdgePt * 2) );
			

			#region [ リソースを解放する ]
			if ( sf != null )	sf.Dispose();	sf = null;
			if ( g != null )	g.Dispose();	g = null;
			#endregion

			return bmp;
		}

        /// <summary>
		/// 文字列を描画したテクスチャを返す(メイン処理)
		/// </summary>
		/// <param name="rectDrawn">描画された領域</param>
		/// <param name="ptOrigin">描画文字列</param>
		/// <param name="drawstr">描画文字列</param>
		/// <param name="drawmode">描画モード</param>
		/// <param name="fontColor">描画色</param>
		/// <param name="edgeColor">縁取色</param>
		/// <param name="gradationTopColor">グラデーション 上側の色</param>
		/// <param name="gradationBottomColor">グラデーション 下側の色</param>
		/// <returns>描画済テクスチャ</returns>
		protected Bitmap DrawPrivateFont_V( string drawstr, Color fontColor, Color edgeColor, bool bVertical )
		{
			if ( this._fontfamily == null || drawstr == null || drawstr == "" )
			{
				// nullを返すと、その後bmp→texture処理や、textureのサイズを見て__の処理で全部例外が発生することになる。
				// それは非常に面倒なので、最小限のbitmapを返してしまう。
				// まずはこの仕様で進めますが、問題有れば(上位側からエラー検出が必要であれば)例外を出したりエラー状態であるプロパティを定義するなり検討します。
				if ( drawstr != "" )
				{
					Trace.TraceWarning( "DrawPrivateFont()の入力不正。最小値のbitmapを返します。" );
				}
				_rectStrings = new Rectangle( 0, 0, 0, 0 );
				_ptOrigin = new Point( 0, 0 );
				return new Bitmap(1, 1);
			}

            //StreamWriter stream = stream = new StreamWriter("Test.txt", false);

            //try
            //{
            //    stream = new StreamWriter("Test.txt", false);
            //}
            //catch (Exception ex)
            //{
            //    stream.Close();
            //    stream = new StreamWriter("Test.txt", false);
            //}

            string[] strName = new string[ drawstr.Length ];
            int[] arHeight = new int[ drawstr.Length ];
            for( int i = 0; i < drawstr.Length; i++ ) strName[i] = drawstr.Substring(i, 1);

            #region[ キャンバスの大きさ予測 ]
            //大きさを計算していく。
            int nHeight = 0;
            int nMaxWidth = 0;
            int nMaxHeight = 0;
            float fEdgePt = 8;
            float fMargin = 0.88f;
            for( int i = 0; i < strName.Length; i++ )
            {
                Size strSize = System.Windows.Forms.TextRenderer.MeasureText( strName[ i ], this._font, new Size( int.MaxValue, int.MaxValue ),
				System.Windows.Forms.TextFormatFlags.NoPrefix |
				System.Windows.Forms.TextFormatFlags.NoPadding );

                //stringformatは最初にやっていてもいいだろう。
			    StringFormat sFormat = new StringFormat();
			    sFormat.LineAlignment = StringAlignment.Center;	// 画面下部（垂直方向位置）
			    sFormat.Alignment = StringAlignment.Center;	// 画面中央（水平方向位置）


                //できるだけ正確な値を計算しておきたい...!
                Bitmap bmpDummy = new Bitmap( 1, 1 ); //とりあえず150
                Graphics gCal = Graphics.FromImage( bmpDummy );
                Rectangle rect正確なサイズ = this.MeasureStringPrecisely( gCal, strName[ i ], this._font, strSize, sFormat );
                int n余白サイズ = strSize.Height - rect正確なサイズ.Height;

                Rectangle rect = new Rectangle( 0, 0, rect正確なサイズ.Width + (int)(fEdgePt * 1.4), rect正確なサイズ.Height + (int)(fEdgePt * 1.4) );

                if( strName[ i ] == "ー" || strName[ i ] == "-" || strName[ i ] == "～" || strName[ i ] == "<" || strName[ i ] == ">" || strName[ i ] == "(" || strName[ i ] == ")" || strName[ i ] == "「" || strName[ i ] == "」" || strName[ i ] == "[" || strName[ i ] == "]" )
                {
                    //nHeight += ( rect正確なサイズ.Width ) + 3;
                    nHeight += ( strSize.Height ) + 3;
                    arHeight[ i ] = ( strSize.Height ) + 3;
                }
                else if( strName[ i ] == "_" ){ nHeight += ( rect正確なサイズ.Height ) + 6;  }
                else if( strName[ i ] == " " )
                { nHeight += ( 12 ); }
                else {
                    //nHeight += ( rect正確なサイズ.Height ) + 6;
                    nHeight += rect正確なサイズ.Height + (int)(fEdgePt * 1.4);
                    arHeight[ i ] = rect正確なサイズ.Height + (int)(fEdgePt * 1.4);
                }

                if( nMaxWidth < rect正確なサイズ.Width + (int)(fEdgePt * 1.4) ) { nMaxWidth = rect正確なサイズ.Width + (int)(fEdgePt * 1.4); }

                //念のため解放
                bmpDummy?.Dispose();
                gCal?.Dispose();

                //stream.WriteLine( "文字の大きさ{0},大きさ合計{1}", ( rect正確なサイズ.Height ) + 6, nHeight );
                
            }
            #endregion

            Bitmap bmpCambus = new Bitmap( nMaxWidth, (int)(nHeight * ( drawstr.Length > 1 ? 1 : 1 ) ) );
            Graphics Gcambus = Graphics.FromImage( bmpCambus );

            //キャンバス作成→1文字ずつ作成してキャンバスに描画という形がよさそうかな?
            int nNowPos = 0;
            int nAdded = 0;

            if( this._pt < 20 ) //補正
                nAdded += 4;

            for( int i = 0; i < strName.Length; i++ )
            {
                Size strSize = System.Windows.Forms.TextRenderer.MeasureText( strName[ i ], this._font, new Size( int.MaxValue, int.MaxValue ),
				System.Windows.Forms.TextFormatFlags.NoPrefix |
				System.Windows.Forms.TextFormatFlags.NoPadding );

                //stringformatは最初にやっていてもいいだろう。
			    StringFormat sFormat = new StringFormat();
			    sFormat.LineAlignment = StringAlignment.Center;	// 画面下部（垂直方向位置）
			    sFormat.Alignment = StringAlignment.Near;	// 画面中央（水平方向位置）

                //できるだけ正確な値を計算しておきたい...!
                Bitmap bmpDummy = new Bitmap( 150, 150 ); //とりあえず150
                Graphics gCal = Graphics.FromImage( bmpDummy );
                Rectangle rect正確なサイズ = this.MeasureStringPrecisely( gCal, strName[ i ], this._font, strSize, sFormat );
                int n余白サイズ = strSize.Height - rect正確なサイズ.Height;
                
                //Bitmap bmpV = new Bitmap( 36, ( strSize.Height + 12 ) - 6 );

                Bitmap bmpV = new Bitmap( (rect正確なサイズ.Width + (int)(fEdgePt * 1.4)), ( rect正確なサイズ.Height + (int)(fEdgePt * 1.4) ) );

			    bmpV.MakeTransparent();
			    Graphics gV = Graphics.FromImage( bmpV );
			    gV.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                Point pt描画位置補正 = new Point( 0, 0 );
                if( strName[ i ] == "ッ" )
                {
                    pt描画位置補正.Y = -3;
                }
                else if( strName[ i ] == "ー" || strName[ i ] == "～" || strName[ i ] == "・" )
                {
                    pt描画位置補正.Y = 6;
                }

                // 描画開始位置など
                Rectangle rect = new Rectangle( -(rect正確なサイズ.X / 2) + pt描画位置補正.X, (2 + pt描画位置補正.Y) - (rect正確なサイズ.Y / 2), (rect正確なサイズ.Width + (int)(fEdgePt * 1.4)) , rect正確なサイズ.Height + (int)(fEdgePt * 1.4));
                //Rectangle rect = new Rectangle( 0, -rect正確なサイズ.Y - 2, 36, rect正確なサイズ.Height + 10);
                
                // DrawPathで、ポイントサイズを使って描画するために、DPIを使って単位変換する
				// (これをしないと、単位が違うために、小さめに描画されてしまう)
				float sizeInPixels = _font.SizeInPoints * gV.DpiY / 72;  // 1 inch = 72 points

				System.Drawing.Drawing2D.GraphicsPath gpV = new System.Drawing.Drawing2D.GraphicsPath();
				gpV.AddString( strName[ i ], this._fontfamily, (int) this._font.Style, sizeInPixels, rect, sFormat );

				// 縁取りを描画する
				Pen pV = new Pen( edgeColor, fEdgePt );
				pV.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
				gV.DrawPath( pV, gpV );

				// 塗りつぶす
				Brush brV;
				{
					brV = new SolidBrush( fontColor );
				}
				gV.FillPath( brV, gpV );

				if ( brV != null ) brV.Dispose(); brV = null;
				if ( pV != null ) pV.Dispose(); pV = null;
				if ( gpV != null ) gpV.Dispose(); gpV = null;

                int n補正 = 0;
                int nY補正 = 0;

                if( strName[ i ] == "ー" || strName[ i ] == "-" || strName[ i ] == "～")
                {
                    bmpV.RotateFlip( RotateFlipType.Rotate90FlipNone );
                    n補正 = 2;
                }
                else if( strName[ i ] == "<" || strName[ i ] == ">" || strName[ i ] == "(" || strName[ i ] == ")" || strName[ i ] == "[" || strName[ i ] == "]" || strName[ i ] == "」" )
                {
                    bmpV.RotateFlip( RotateFlipType.Rotate90FlipNone );
                    n補正 = 2;
                }
                else if( strName[ i ] == "「" )
                {
                    bmpV.RotateFlip( RotateFlipType.Rotate90FlipNone );
                    n補正 = 2;
                }
                else if( strName[ i ] == "ァ" || strName[ i ] == "ィ" || strName[ i ] == "ゥ" || strName[ i ] == "ェ" || strName[ i ] == "ォ" )
                {
                    n補正 = 4;
                }
                else if( strName[ i ] == "ッ" )
                {
                    n補正 = 4;
                }
                else if( strName[ i ] == "・" )
                {
                    nY補正 = 2;
                }
                //else if( strName[ i ] == "_" )
                //    nNowPos = nNowPos + 20;
                else if( strName[ i ] == " " )
                    nNowPos = nNowPos + 10;

#if VerticalFont
                if( this._pt < 20 ) bmpV.Save( "String_s" + i.ToString() + "_s.png" );
                else bmpV.Save( "String_" + i.ToString() + ".png" );
#endif
                if( i == 0 )
                {
                    nNowPos = 0;
                }
                Gcambus.DrawImage( bmpV, ( bmpCambus.Width / 2 ) - ( bmpV.Size.Width / 2 ) + n補正, nNowPos + nY補正 );
                nNowPos += (int)( bmpV.Size.Height * fMargin );

#if VerticalFont
                if( this._pt < 20 )
                    bmpCambus.Save( "test_S.png" );
                else
                    bmpCambus.Save( "test.png" );
#endif

                if( bmpV != null ) bmpV.Dispose();
                if( gCal != null ) gCal.Dispose();

			    _rectStrings = new Rectangle( 0, 0, strSize.Width, strSize.Height );
			    _ptOrigin = new Point( 6 * 2, 6 * 2 );


                //stream.WriteLine( "黒無しサイズ{0},余白{1},黒あり予測サイズ{2},ポ↑ジ↓{3}",rect正確なサイズ.Height, n余白サイズ, rect正確なサイズ.Height + 8, nNowPos );
                
            }
            //stream.Close();

            if( Gcambus != null ) Gcambus.Dispose();

			//return bmp;
            return bmpCambus;
		}

        ///// <summary>
        ///// 文字列を描画したテクスチャを返す(メイン処理)
        ///// </summary>
        ///// <param name="rectDrawn">描画された領域</param>
        ///// <param name="ptOrigin">描画文字列</param>
        ///// <param name="drawstr">描画文字列</param>
        ///// <param name="drawmode">描画モード</param>
        ///// <param name="fontColor">描画色</param>
        ///// <param name="edgeColor">縁取色</param>
        ///// <param name="gradationTopColor">グラデーション 上側の色</param>
        ///// <param name="gradationBottomColor">グラデーション 下側の色</param>
        ///// <returns>描画済テクスチャ</returns>
        //protected Bitmap DrawPrivateFont_V( string drawstr, Color fontColor, Color edgeColor, bool bVertical )
        //{
        //    if ( this._fontfamily == null || drawstr == null || drawstr == "" )
        //    {
        //        // nullを返すと、その後bmp→texture処理や、textureのサイズを見て__の処理で全部例外が発生することになる。
        //        // それは非常に面倒なので、最小限のbitmapを返してしまう。
        //        // まずはこの仕様で進めますが、問題有れば(上位側からエラー検出が必要であれば)例外を出したりエラー状態であるプロパティを定義するなり検討します。
        //        if ( drawstr != "" )
        //        {
        //            Trace.TraceWarning( "DrawPrivateFont()の入力不正。最小値のbitmapを返します。" );
        //        }
        //        _rectStrings = new Rectangle( 0, 0, 0, 0 );
        //        _ptOrigin = new Point( 0, 0 );
        //        return new Bitmap(1, 1);
        //    }

        //    //StreamWriter stream = stream = new StreamWriter("Test.txt", false);

        //    //try
        //    //{
        //    //    stream = new StreamWriter("Test.txt", false);
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    stream.Close();
        //    //    stream = new StreamWriter("Test.txt", false);
        //    //}

        //    string[] strName = new string[] { "焼","肉","定","食", "X", "G", "t", "e", "s", "t" };
        //    strName = new string[ drawstr.Length ];
        //    for( int i = 0; i < drawstr.Length; i++ ) strName[i] = drawstr.Substring(i, 1);
            

        //    Bitmap bmpCambus = new Bitmap( 48, ( drawstr.Length * 31 ) );
        //    Graphics Gcambus = Graphics.FromImage( bmpCambus );

        //    //キャンバス作成→1文字ずつ作成してキャンバスに描画という形がよさそうかな?

        //    int nStartPos = 0;
        //    int nNowPos = 0;

        //    //forループで1文字ずつbitmap作成?
        //    for( int i = 0; i < strName.Length; i++ )
        //    {
        //        Size strSize = System.Windows.Forms.TextRenderer.MeasureText( strName[ i ], this._font, new Size( int.MaxValue, int.MaxValue ),
        //        System.Windows.Forms.TextFormatFlags.NoPrefix |
        //        System.Windows.Forms.TextFormatFlags.NoPadding );

        //        //Bitmap bmpV = new Bitmap( strSize.Width + 12, ( strSize.Height + 12 ) - 6 );
        //        Bitmap bmpV = new Bitmap( 36, ( strSize.Height + 12 ) - 6 );
        //        bmpV.MakeTransparent();
        //        Graphics gV = Graphics.FromImage( bmpV );
        //        gV.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

        //        StringFormat sFormat = new StringFormat();
        //        sFormat.LineAlignment = StringAlignment.Center;	// 画面下部（垂直方向位置）
        //        sFormat.Alignment = StringAlignment.Center;	// 画面中央（水平方向位置）


        //        //Rectangle rect = new Rectangle( 0, 0, strSize.Width + 12, ( strSize.Height + 12 ));
        //        Rectangle rect = new Rectangle( 0, 0, 36, ( strSize.Height + 12 ));

        //        // DrawPathで、ポイントサイズを使って描画するために、DPIを使って単位変換する
        //        // (これをしないと、単位が違うために、小さめに描画されてしまう)
        //        float sizeInPixels = _font.SizeInPoints * gV.DpiY / 72;  // 1 inch = 72 points

        //        System.Drawing.Drawing2D.GraphicsPath gpV = new System.Drawing.Drawing2D.GraphicsPath();
        //        gpV.AddString( strName[ i ], this._fontfamily, (int) this._font.Style, sizeInPixels, rect, sFormat );


        //        Rectangle rect正確なサイズ = this.MeasureStringPrecisely( gV, strName[ i ], this._font, strSize, sFormat );

        //        // 縁取りを描画する
        //        Pen pV = new Pen( edgeColor, 6 );
        //        pV.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
        //        gV.DrawPath( pV, gpV );

        //        // 塗りつぶす
        //        Brush brV;
        //        {
        //            brV = new SolidBrush( fontColor );
        //        }
        //        gV.FillPath( brV, gpV );

        //        if ( brV != null ) brV.Dispose(); brV = null;
        //        if ( pV != null ) pV.Dispose(); pV = null;
        //        if ( gpV != null ) gpV.Dispose(); gpV = null;

        //        int n補正 = 0;

        //        //bmpV.Save( "String" + i.ToString() + ".png" );
        //        if( strName[ i ] == "ー" || strName[ i ] == "-" || strName[ i ] == "～")
        //        {
        //            bmpV.RotateFlip( RotateFlipType.Rotate90FlipNone );
        //            nNowPos = nNowPos + 20;
        //            n補正 = 2;
        //        }
        //        else if( strName[ i ] == "<" || strName[ i ] == ">" || strName[ i ] == "(" || strName[ i ] == ")" )
        //        {
        //            bmpV.RotateFlip( RotateFlipType.Rotate90FlipNone );
        //            nNowPos = nNowPos + 8;
        //            n補正 = 2;
        //        }
        //        else if( strName[ i ] == "_" )
        //            nNowPos = nNowPos + 20;
        //        else if( strName[ i ] == " " )
        //            nNowPos = nNowPos + 10;

        //        int n余白サイズ = strSize.Height - rect正確なサイズ.Height;
        //        if( i == 0 )
        //        {
        //            nStartPos = -n余白サイズ + 2;
        //            nNowPos = -n余白サイズ + 2;
        //            Gcambus.DrawImage( bmpV, ( bmpCambus.Size.Width - bmpV.Size.Width ) + n補正, nStartPos );
        //            //nNowPos += ( rect正確なサイズ.Height + 6 );
        //        }
        //        else
        //        {
        //            nNowPos += ( strSize.Height - n余白サイズ ) + 4;
        //            Gcambus.DrawImage( bmpV, ( bmpCambus.Size.Width - bmpV.Size.Width ) + n補正, nNowPos );
        //        }

        //        if ( bmpV != null ) bmpV.Dispose();

        //        //bmpCambus.Save( "test.png" );

        //        _rectStrings = new Rectangle( 0, 0, strSize.Width, strSize.Height );
        //        _ptOrigin = new Point( 6 * 2, 6 * 2 );


        //        //stream.WriteLine( "黒無しサイズ{0},余白{1},黒あり予測サイズ{2},ポ↑ジ↓{3}",rect正確なサイズ.Height, n余白サイズ, rect正確なサイズ.Height + 6, nNowPos );
                
        //    }
        //    //stream.Close();

        //    //return bmp;
        //    return bmpCambus;
        //}


        //------------------------------------------------
        //使用:http://dobon.net/vb/dotnet/graphics/measurestring.html

        /// <summary>
        /// Graphics.DrawStringで文字列を描画した時の大きさと位置を正確に計測する
        /// </summary>
        /// <param name="g">文字列を描画するGraphics</param>
        /// <param name="text">描画する文字列</param>
        /// <param name="font">描画に使用するフォント</param>
        /// <param name="proposedSize">これ以上大きいことはないというサイズ。
        /// できるだけ小さくすること。</param>
        /// <param name="stringFormat">描画に使用するStringFormat</param>
        /// <returns>文字列が描画される範囲。
        /// 見つからなかった時は、Rectangle.Empty。</returns>
        private Rectangle MeasureStringPrecisely(Graphics g,
            string text, Font font, Size proposedSize, StringFormat stringFormat)
        {
            //解像度を引き継いで、Bitmapを作成する
            Bitmap bmp = new Bitmap(proposedSize.Width, proposedSize.Height, g);
            //BitmapのGraphicsを作成する
            Graphics bmpGraphics = Graphics.FromImage(bmp);
            //Graphicsのプロパティを引き継ぐ
            bmpGraphics.TextRenderingHint = g.TextRenderingHint;
            bmpGraphics.TextContrast = g.TextContrast;
            bmpGraphics.PixelOffsetMode = g.PixelOffsetMode;
            //文字列の描かれていない部分の色を取得する
            Color backColor = bmp.GetPixel(0, 0);
            //実際にBitmapに文字列を描画する
            bmpGraphics.DrawString(text, font, Brushes.Black,
                new RectangleF(0f, 0f, proposedSize.Width, proposedSize.Height),
                stringFormat);
            bmpGraphics.Dispose();
            //文字列が描画されている範囲を計測する
            Rectangle resultRect = MeasureForegroundArea(bmp, backColor);
            bmp.Dispose();

            return resultRect;
        }

        /// <summary>
        /// 指定されたBitmapで、backColor以外の色が使われている範囲を計測する
        /// </summary>
        private Rectangle MeasureForegroundArea(Bitmap bmp, Color backColor)
        {
            int backColorArgb = backColor.ToArgb();
            int maxWidth = bmp.Width;
            int maxHeight = bmp.Height;

            //左側の空白部分を計測する
            int leftPosition = -1;
            for (int x = 0; x < maxWidth; x++)
            {
                for (int y = 0; y < maxHeight; y++)
                {
                    //違う色を見つけたときは、位置を決定する
                    if (bmp.GetPixel(x, y).ToArgb() != backColorArgb)
                    {
                        leftPosition = x;
                        break;
                    }
                }
                if (0 <= leftPosition)
                {
                    break;
                }
            }
            //違う色が見つからなかった時
            if (leftPosition < 0)
            {
                return Rectangle.Empty;
            }

            //右側の空白部分を計測する
            int rightPosition = -1;
            for (int x = maxWidth - 1; leftPosition < x; x--)
            {
                for (int y = 0; y < maxHeight; y++)
                {
                    if (bmp.GetPixel(x, y).ToArgb() != backColorArgb)
                    {
                        rightPosition = x;
                        break;
                    }
                }
                if (0 <= rightPosition)
                {
                    break;
                }
            }
            if (rightPosition < 0)
            {
                rightPosition = leftPosition;
            }

            //上の空白部分を計測する
            int topPosition = -1;
            for (int y = 0; y < maxHeight; y++)
            {
                for (int x = leftPosition; x <= rightPosition; x++)
                {
                    if (bmp.GetPixel(x, y).ToArgb() != backColorArgb)
                    {
                        topPosition = y;
                        break;
                    }
                }
                if (0 <= topPosition)
                {
                    break;
                }
            }
            if (topPosition < 0)
            {
                return Rectangle.Empty;
            }

            //下の空白部分を計測する
            int bottomPosition = -1;
            for (int y = maxHeight - 1; topPosition < y; y--)
            {
                for (int x = leftPosition; x <= rightPosition; x++)
                {
                    if (bmp.GetPixel(x, y).ToArgb() != backColorArgb)
                    {
                        bottomPosition = y;
                        break;
                    }
                }
                if (0 <= bottomPosition)
                {
                    break;
                }
            }
            if (bottomPosition < 0)
            {
                bottomPosition = topPosition;
            }

            //結果を返す
            return new Rectangle(leftPosition, topPosition,
                rightPosition - leftPosition, bottomPosition - topPosition);
        }

        private Rectangle MeasureForegroundArea(Bitmap bmp)
        {
            return MeasureForegroundArea(bmp, bmp.GetPixel(0, 0));
        }

        //------------------------------------------------

		/// <summary>
		/// 最後にDrawPrivateFont()した文字列の描画領域を取得します。
		/// </summary>
		public Rectangle RectStrings
		{
			get
			{
				return _rectStrings;
			}
			protected set
			{
				_rectStrings = value;
			}
		}
		public Point PtOrigin
		{
			get
			{
				return _ptOrigin;
			}
			protected set
			{
				_ptOrigin = value;
			}
		}

#region [ IDisposable 実装 ]
		//-----------------
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		protected void Dispose(bool disposeManagedObjects)
		{
			if (this.bDispose完了済み)
				return;

			if (disposeManagedObjects)
			{
				// (A) Managed リソースの解放
				if (this._font != null)
				{
					this._font.Dispose();
					this._font = null;
				}
				if (this._pfc != null)
				{
					this._pfc.Dispose();
					this._pfc = null;
				}
				if (this._fontfamily != null)
				{
					this._fontfamily.Dispose();
					this._fontfamily = null;
				}
			}

			// (B) Unamanaged リソースの解放

			this.bDispose完了済み = true;
		}
		//-----------------
		~CPrivateFont()
		{
			this.Dispose(false);
		}
		//-----------------
#endregion

#region [ private ]
		//-----------------
		protected bool bDispose完了済み;
		protected Font _font;

		private System.Drawing.Text.PrivateFontCollection _pfc;
		private FontFamily _fontfamily;
		private float _pt;
		private Rectangle _rectStrings;
		private Point _ptOrigin;
        private string _baseFontname = null;
        private bool bIsSystemFont;
		//-----------------
#endregion
	}
}
