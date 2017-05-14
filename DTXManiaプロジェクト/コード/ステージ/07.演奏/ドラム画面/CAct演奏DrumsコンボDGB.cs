using System;
using System.Collections.Generic;
using System.Text;

namespace DTXMania
{
	internal class CAct演奏DrumsコンボDGB : CAct演奏Combo共通
	{
		// CAct演奏Combo共通 実装

		protected override void tコンボ表示・ギター( int nCombo値, int nジャンプインデックス )
		{
		}
		protected override void tコンボ表示・ドラム( int nCombo値, int nジャンプインデックス )
        {
            this.tコンボ表示・太鼓( nCombo値, nジャンプインデックス );
		}
		protected override void tコンボ表示・ベース( int nCombo値, int nジャンプインデックス )
		{
		}
	}
}
