using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTXMania
{
    /// <summary>
    /// ユーザーデータ
    /// </summary>
    public class CUser
    {
        public int UserID;
        public string strユーザー名;
        public DateTime dtデータ作成日時;
        /// <summary>
        /// 最終プレイ日時
        /// ・ログイン(ゲーム開始)時
        /// ・楽曲プレイ時
        /// の2箇所で更新する。
        /// </summary>
        public DateTime dt最終プレイ日時;

        /// <summary>
        /// 太鼓カウンター
        /// 暫定仕様:9,999,999,999,999,999,999打(999京9999億...9999打)まで
        /// </summary>
        public ulong 太鼓カウンター;
        
        // TODO:オプション保存

    }
}
