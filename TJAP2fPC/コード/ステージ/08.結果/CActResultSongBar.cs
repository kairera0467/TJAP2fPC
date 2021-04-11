﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using SlimDX;
using FDK;

namespace DTXMania
{
	internal class CActResultSongBar : CActivity
	{
		// コンストラクタ

		public CActResultSongBar()
		{
			base.b活性化してない = true;
		}


		// メソッド

		public void tアニメを完了させる()
		{
			this.ct登場用.n現在の値 = this.ct登場用.n終了値;
		}


		// CActivity 実装

		public override void On活性化()
		{
            if( !string.IsNullOrEmpty( CDTXMania.ConfigIni.strPrivateFontで使うフォント名 ) )
            {
                this.pfMusicName = new CPrivateFastFont( new FontFamily( CDTXMania.ConfigIni.strPrivateFontで使うフォント名 ), 30 );
            }
            else
                this.pfMusicName = new CPrivateFastFont( new FontFamily( "MS PGothic" ), 30 );


            Bitmap bmpSongTitle = new Bitmap(1, 1);
            bmpSongTitle = pfMusicName.DrawPrivateFont( CDTXMania.DTX.TITLE, Color.White, Color.Black );
            this.txMusicName = CDTXMania.tテクスチャの生成( bmpSongTitle, false );

			base.On活性化();
		}
		public override void On非活性化()
		{
			if( this.ct登場用 != null )
			{
				this.ct登場用 = null;
			}
            CDTXMania.t安全にDisposeする( ref this.pfMusicName );
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
                CDTXMania.tテクスチャの解放( ref this.txMusicName );
				base.OnManagedリソースの解放();
			}
		}
		public override int On進行描画()
		{
			if( base.b活性化してない )
			{
				return 0;
			}
			if( base.b初めての進行描画 )
			{
				this.ct登場用 = new CCounter( 0, 270, 4, CDTXMania.Timer );
				base.b初めての進行描画 = false;
			}
			this.ct登場用.t進行();

			this.txMusicName.t2D描画( CDTXMania.app.Device, 1268 - this.txMusicName.szテクスチャサイズ.Width, 6 );

			if( !this.ct登場用.b終了値に達した )
			{
				return 0;
			}
			return 1;
		}


		// その他

		#region [ private ]
		//-----------------
		private CCounter ct登場用;

        private CTexture txMusicName;
        private CPrivateFastFont pfMusicName;
		//-----------------
		#endregion
	}
}
