using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using FDK;
using System.Drawing;

namespace DTXMania
{
    class GoGoSplash : CActivity
    {
        public GoGoSplash()
        {
            this.b活性化してない = true;
        }

        public override void On活性化()
        {
            Splash = new CCounter();
            base.On活性化();
        }

        public override void On非活性化()
        {
            base.On非活性化();
        }

        public override int On進行描画()
        {
            if (Splash == null) return base.On進行描画();
            Splash.t進行();
            if(Splash.b進行中)
            {
                for (int i = 0; i < CDTXMania.Skin.Game_Effect_GoGoSplash_X.Length; i++)
                {
                    CDTXMania.Tx.Effects_GoGoSplash?.t2D拡大率考慮下中心基準描画(CDTXMania.app.Device, CDTXMania.Skin.Game_Effect_GoGoSplash_X[i], 720, new Rectangle(CDTXMania.Skin.Game_Effect_GoGoSplash[0] * Splash.n現在の値, 0, CDTXMania.Skin.Game_Effect_GoGoSplash[0], CDTXMania.Skin.Game_Effect_GoGoSplash[1]));
                }
                if(Splash.b終了値に達した)
                {
                    Splash = new CCounter();
                }
            }
            return base.On進行描画();
        }

        public void StartSplash()
        {
            Splash = new CCounter(0, CDTXMania.Skin.Game_Effect_GoGoSplash[2] - 1, CDTXMania.Skin.Game_Effect_GoGoSplash_Timer, CDTXMania.Timer);
        }

        private CCounter Splash;
    }
}
