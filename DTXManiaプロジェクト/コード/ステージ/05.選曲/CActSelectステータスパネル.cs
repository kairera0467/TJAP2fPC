using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using FDK;

namespace DTXMania
{
	internal class CActSelectステータスパネル : CActivity
	{
		// メソッド

		public CActSelectステータスパネル()
		{
			base.b活性化してない = true;
		}
		public void t選択曲が変更された()
		{

		}


		// CActivity 実装

		public override void On活性化()
		{

			base.On活性化();
		}
		public override void On非活性化()
		{

			base.On非活性化();
		}
		public override void OnManagedリソースの作成()
		{
			if( !base.b活性化してない )
			{

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

			}
			return 0;
		}


		// その他

		#region [ private ]
		//-----------------
		[StructLayout( LayoutKind.Sequential )]
		private struct ST数字
		{
			public char ch;
			public Rectangle rc;
			public ST数字( char ch, Rectangle rc )
			{
				this.ch = ch;
				this.rc = rc;
			}
		}

		private STDGBVALUE<bool> b現在選択中の曲がフルコンボ;
		private CCounter ct登場アニメ用;
		private CCounter ct難易度スクロール用;
		private CCounter ct難易度矢印用;
		private STDGBVALUE<double> db現在選択中の曲の最高スキル値;
		private STDGBVALUE<int> n現在選択中の曲のレベル;
		private STDGBVALUE<int> n現在選択中の曲の最高ランク;
		private int n現在選択中の曲の難易度;
		private int n難易度開始文字位置;
		private const int n難易度表示可能文字数 = 0x24;
		private int n本体X;
		private int n本体Y;
		private readonly Rectangle[] rcランク = new Rectangle[] { new Rectangle( 0, 0x20, 10, 10 ), new Rectangle( 10, 0x20, 10, 10 ), new Rectangle( 20, 0x20, 10, 10 ), new Rectangle( 0, 0x2a, 10, 10 ), new Rectangle( 10, 0x2a, 10, 10 ), new Rectangle( 20, 0x2a, 10, 10 ), new Rectangle( 0, 0x34, 10, 10 ) };
		private readonly Rectangle[] rc数字 = new Rectangle[] { new Rectangle( 0, 0, 15, 0x13 ), new Rectangle( 15, 0, 15, 0x13 ), new Rectangle( 30, 0, 15, 0x13 ), new Rectangle( 0x2d, 0, 15, 0x13 ), new Rectangle( 0, 0x13, 15, 0x13 ), new Rectangle( 15, 0x13, 15, 0x13 ), new Rectangle( 30, 0x13, 15, 0x13 ), new Rectangle( 0x2d, 0x13, 15, 0x13 ), new Rectangle( 0, 0x26, 15, 0x13 ), new Rectangle( 15, 0x26, 15, 0x13 ), new Rectangle( 30, 0x26, 15, 0x13 ), new Rectangle( 0x2d, 0x26, 15, 0x13 ) };
		private C曲リストノード r直前の曲;
		private string[] str難易度ラベル = new string[] { "", "", "", "", "" };
		private readonly ST数字[] st数字 = new ST数字[] { new ST数字( '0', new Rectangle( 0, 0, 8, 11 ) ), new ST数字( '1', new Rectangle( 8, 0, 8, 11 ) ), new ST数字( '2', new Rectangle( 0x10, 0, 8, 11 ) ), new ST数字( '3', new Rectangle( 0x18, 0, 8, 11 ) ), new ST数字( '4', new Rectangle( 0x20, 0, 8, 11 ) ), new ST数字( '5', new Rectangle( 40, 0, 8, 11 ) ), new ST数字( '6', new Rectangle( 0, 11, 8, 11 ) ), new ST数字( '7', new Rectangle( 8, 11, 8, 11 ) ), new ST数字( '8', new Rectangle( 0x10, 11, 8, 11 ) ), new ST数字( '9', new Rectangle( 0x18, 11, 8, 11 ) ), new ST数字( '.', new Rectangle( 0x20, 11, 4, 11 ) ), new ST数字( 'p', new Rectangle( 0x24, 11, 15, 11 ) ) };
		private CTexture txゲージ用数字他;
		private CTexture txスキルゲージ;
		private CTexture txパネル本体;
		private CTexture txレベル数字;
		private CTexture tx難易度用矢印;

		private int n現在の難易度ラベルが完全表示されているかを調べてスクロール方向を返す()
		{
			int num = 0;
			int length = 0;
			for( int i = 0; i < 5; i++ )
			{
				if( ( this.str難易度ラベル[ i ] != null ) && ( this.str難易度ラベル[ i ].Length > 0 ) )
				{
					length = this.str難易度ラベル[ i ].Length;
				}
				if( this.n現在選択中の曲の難易度 == i )
				{
					break;
				}
				if( ( this.str難易度ラベル[ i ] != null ) && ( this.str難易度ラベル.Length > 0 ) )
				{
					num += length + 2;
				}
			}
			if( num >= ( this.n難易度開始文字位置 + 0x24 ) )
			{
				return 1;
			}
			if( ( num + length ) <= this.n難易度開始文字位置 )
			{
				return -1;
			}
			if( ( ( num + length ) - 1 ) >= ( this.n難易度開始文字位置 + 0x24 ) )
			{
				return 1;
			}
			if( num < this.n難易度開始文字位置 )
			{
				return -1;
			}
			return 0;
		}
		//-----------------
		#endregion
	}
}
