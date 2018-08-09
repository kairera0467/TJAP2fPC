using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using FDK;

namespace DTXMania
{
    internal class CAct演奏DrumsFooter : CActivity
    {
        /// <summary>
        /// フッター
        /// </summary>
        public CAct演奏DrumsFooter()
        {
            base.b活性化してない = true;
        }

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
            base.OnManagedリソースの作成();
        }

        public override void OnManagedリソースの解放()
        {
            base.OnManagedリソースの解放();
        }

        public override int On進行描画()
        {
            if (CDTXMania.Tx.Mob_Footer != null)
            {
                CDTXMania.Tx.Mob_Footer.t2D描画(CDTXMania.app.Device, 0, 720 - CDTXMania.Tx.Mob_Footer.szテクスチャサイズ.Height);
            }
            return base.On進行描画();
        }

        #region[ private ]
        //-----------------
        //-----------------
        #endregion
    }
}
