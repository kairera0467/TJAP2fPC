using System;
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
            if( !string.IsNullOrEmpty( CDTXMania.ConfigIni.FontName) )
            {
                this.pfMusicName = new CPrivateFastFont( new FontFamily( CDTXMania.ConfigIni.FontName ), 30 );
                this.pfStageText = new CPrivateFastFont(new FontFamily(CDTXMania.ConfigIni.FontName), 30);
            }
            else
            {
                this.pfMusicName = new CPrivateFastFont(new FontFamily("MS UI Gothic"), 30);
                this.pfStageText = new CPrivateFastFont(new FontFamily("MS UI Gothic"), 30);
            }


		    using (var bmpSongTitle = pfMusicName.DrawPrivateFont(CDTXMania.DTX.TITLE, Color.White, Color.Black))
		    {
		        this.txMusicName = CDTXMania.tテクスチャの生成(bmpSongTitle, false);
		        txMusicName.vc拡大縮小倍率.X = CDTXMania.GetSongNameXScaling(ref txMusicName);
		    }

		    using (var bmpStageText = pfStageText.DrawPrivateFont(CDTXMania.Skin.Game_StageText, Color.White, Color.Black))
		    {
		        this.txStageText = CDTXMania.tテクスチャの生成(bmpStageText, false);
		    }

			base.On活性化();
		}
		public override void On非活性化()
		{
			if( this.ct登場用 != null )
			{
				this.ct登場用 = null;
			}
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
                CDTXMania.t安全にDisposeする(ref this.pfMusicName);
                CDTXMania.tテクスチャの解放( ref this.txMusicName );

                CDTXMania.t安全にDisposeする(ref this.pfStageText);
                CDTXMania.tテクスチャの解放(ref this.txStageText);
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

			this.txMusicName.t2D描画( CDTXMania.app.Device, 1254 - this.txMusicName.szテクスチャサイズ.Width * txMusicName.vc拡大縮小倍率.X, 6 );

            this.txStageText.t2D描画(CDTXMania.app.Device, 230, 6);

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

        private CTexture txStageText;
        private CPrivateFont pfStageText;
        //-----------------
		#endregion
	}
}
