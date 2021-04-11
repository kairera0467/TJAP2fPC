﻿using System;
using System.Collections.Generic;
using System.Text;
using FDK;
using System.Diagnostics;

namespace DTXMania
{
	internal class CAct演奏演奏情報 : CActivity
	{
		// プロパティ

		public double dbBPM;
		public int n小節番号;
        public double dbSCROLL;

		// コンストラクタ

		public CAct演奏演奏情報()
		{
			base.b活性化してない = true;
		}

				
		// CActivity 実装

		public override void On活性化()
		{
			this.n小節番号 = 0;
			this.dbBPM = CDTXMania.DTX.BASEBPM;
            this.dbSCROLL = 1.0;
			base.On活性化();
		}
		public override int On進行描画()
		{
			throw new InvalidOperationException( "t進行描画(int x, int y) のほうを使用してください。" );
		}
		public void t進行描画( int x, int y )
		{
			if ( !base.b活性化してない )
			{
				y += 0x153;
				CDTXMania.act文字コンソール.tPrint( x, y, C文字コンソール.Eフォント種別.白, string.Format( "BGM/D/G/B Adj: {0:####0}/{1:####0}/{2:####0}/{3:####0} ms", CDTXMania.DTX.nBGMAdjust, CDTXMania.ConfigIni.nInputAdjustTimeMs.Drums, CDTXMania.ConfigIni.nInputAdjustTimeMs.Guitar, CDTXMania.ConfigIni.nInputAdjustTimeMs.Bass ) );
				y -= 0x10;
				int num = ( CDTXMania.DTX.listChip.Count > 0 ) ? CDTXMania.DTX.listChip[ CDTXMania.DTX.listChip.Count - 1 ].n発声時刻ms : 0;
				string str = "Time:          " + ( ( ( ( double ) CDTXMania.Timer.n現在時刻 ) / 1000.0 ) ).ToString( "####0.00" ) + " / " + ( ( ( ( double ) num ) / 1000.0 ) ).ToString( "####0.00" );
				CDTXMania.act文字コンソール.tPrint( x, y, C文字コンソール.Eフォント種別.白, str );
				y -= 0x10;
				CDTXMania.act文字コンソール.tPrint( x, y, C文字コンソール.Eフォント種別.白, string.Format( "Part:          {0:####0}", this.n小節番号 ) );
				y -= 0x10;
				CDTXMania.act文字コンソール.tPrint( x, y, C文字コンソール.Eフォント種別.白, string.Format( "BPM:           {0:####0.0000}", this.dbBPM ) );
				y -= 0x10;
				CDTXMania.act文字コンソール.tPrint( x, y, C文字コンソール.Eフォント種別.白, string.Format( "Frame:         {0:####0} fps", CDTXMania.FPS.n現在のFPS ) );
				y -= 0x10;
				CDTXMania.act文字コンソール.tPrint( x, y, C文字コンソール.Eフォント種別.白, string.Format( "NoteN:         {0:####0}", CDTXMania.DTX.nノーツ数[0] ) );
				y -= 0x10;
				CDTXMania.act文字コンソール.tPrint( x, y, C文字コンソール.Eフォント種別.白, string.Format( "NoteE:         {0:####0}", CDTXMania.DTX.nノーツ数[1] ) );
				y -= 0x10;
				CDTXMania.act文字コンソール.tPrint( x, y, C文字コンソール.Eフォント種別.白, string.Format( "NoteM:         {0:####0}", CDTXMania.DTX.nノーツ数[2] ) );
				y -= 0x10;
				CDTXMania.act文字コンソール.tPrint( x, y, C文字コンソール.Eフォント種別.白, string.Format( "NoteC:         {0:####0}", CDTXMania.DTX.nノーツ数[3] ) );
				y -= 0x10;
				CDTXMania.act文字コンソール.tPrint( x, y, C文字コンソール.Eフォント種別.白, string.Format( "SCROLL:        {0:####0.00}", this.dbSCROLL ) );
                y -= 0x10;
                CDTXMania.act文字コンソール.tPrint( x, y, C文字コンソール.Eフォント種別.白, string.Format( "SCOREMODE:     {0:####0}", CDTXMania.DTX.nScoreModeTmp ) );
                y -= 0x10;
                CDTXMania.act文字コンソール.tPrint( x, y, C文字コンソール.Eフォント種別.白, string.Format( "SCROLLMODE:    {0:####0}", Enum.GetName(typeof(EScrollMode), CDTXMania.ConfigIni.eScrollMode ) ) );


				//CDTXMania.act文字コンソール.tPrint( x, y, C文字コンソール.Eフォント種別.白, string.Format( "Sound CPU :    {0:####0.00}%", CDTXMania.Sound管理.GetCPUusage() ) );
				//y -= 0x10;
				//CDTXMania.act文字コンソール.tPrint( x, y, C文字コンソール.Eフォント種別.白, string.Format( "Sound Mixing:  {0:####0}", CDTXMania.Sound管理.GetMixingStreams() ) );
				//y -= 0x10;
				//CDTXMania.act文字コンソール.tPrint( x, y, C文字コンソール.Eフォント種別.白, string.Format( "Sound Streams: {0:####0}", CDTXMania.Sound管理.GetStreams() ) );
				//y -= 0x10;
			}
		}
	}
}
