using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTXMania
{
    /// <summary>
    /// 新形式のスコア
    /// おおよその内容はDTXManiaのものをベースとしていますが、項目を太鼓向けに変えて不必要なものは削っています。
    /// </summary>
    public class CSongRecord
    {

        // リザルト単位の記録
        public struct ST演奏記録
        {
            public string str曲名;
            public long nスコア;
            public float fゲージ;
            public int n良;
            public int n可;
            public int n不可;
            public int n連打;
            public int n風船連打;
            public int n特殊連打;
            public int n最大コンボ;
            public int n全ノート数;
            public int n演奏速度分子;
            public int n演奏速度分母;
            /// <summary>
            /// 演奏記録の正当性を証明するためのハッシュ値。
            /// </summary>
            public string Hash;
            public string strプレイ日時;
            public int n判定範囲Perfect;
            public int n判定範囲Good;
            public int n判定範囲Miss;
        }
    }
}
