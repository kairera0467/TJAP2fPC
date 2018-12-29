using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TJAPlayer3.Animatios
{
    /// <summary>
    /// アニメーション インターフェイス。
    /// </summary>
    interface IAnimatable
    {
        /// <summary>
        /// アニメーションを開始します。
        /// </summary>
        void Start();
        /// <summary>
        /// アニメーションを停止します。
        /// </summary>
        void Stop();
        /// <summary>
        /// アニメーションをリセットします。
        /// </summary>
        void Reset();
        /// <summary>
        /// アニメーションの進行を行います。
        /// </summary>
        /// <returns>アニメーションのパラメータ。実装するクラスによって異なる。</returns>
        object Tick();
    }
}
