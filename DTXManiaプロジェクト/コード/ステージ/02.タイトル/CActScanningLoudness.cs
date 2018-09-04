using System;
using FDK;

namespace DTXMania
{
	internal class CActScanningLoudness :  CActivity
	{
	    public bool bIsActivelyScanning;

		// CActivity 実装

		public override void On活性化()
		{
			if ( this.b活性化してる )
				return;
			base.On活性化();

			try
			{
				this.ctNowScanningLoudness = new CCounter();
				this.ctNowScanningLoudness.t開始( 0, 200, 29, CDTXMania.Timer );
			}
			finally
			{
			}
		}

		public override void On非活性化()
		{
			if ( this.b活性化してない )
				return;
			base.On非活性化();
			this.ctNowScanningLoudness = null;
		}

		public override int On進行描画()
		{
			if ( this.b活性化してない )
			{
				return 0;
			}
			this.ctNowScanningLoudness.t進行Loop();
			if ( bIsActivelyScanning && CDTXMania.Tx.Scanning_Loudness != null )
			{
                CDTXMania.Tx.Scanning_Loudness.n透明度 = (int) ( 176.0 + 80.0 * Math.Sin( (double) (2 * Math.PI * this.ctNowScanningLoudness.n現在の値 / 100.0 ) ) );
                CDTXMania.Tx.Scanning_Loudness.t2D描画( CDTXMania.app.Device, 18 + 89 + 18, 7 ); // 2018-09-03 twopointzero: display right of Enum_Song, using its width and margin
			}

			return 0;
		}

		private CCounter ctNowScanningLoudness;
	}
}
