using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTXMania
{
    /// <summary>
    /// TJA譜面の情報を格納するクラス
    /// </summary>
    public class CTJA
    {
        // コースごとのヘッダ
        public int nCourse;
        public int nLife;
        public int nScoreMode;
        public int[,] nScoreInit = new int[ 2, 5 ]; //[ x, y ] x=通常or真打 y=コース
        public int[] nScoreDiff = new int[ 5 ]; //[y]
        public bool[,] b配点が指定されている = new bool[ 3, 5 ];

        public bool bHasBranch = false;

        // コンストラクタ

        public CTJA()
        {

        }
    }
}
