using DTXMania;
using FDK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace TJAPlayer3
{
    class PuchiChara : CActivity
    {
        public PuchiChara()
        {
            base.b活性化してない = true;
        }

        public override void On活性化()
        {
            Counter = new CCounter(0, CDTXMania.Skin.Game_PuchiChara[2] - 1, CDTXMania.Skin.Game_PuchiChara_Timer, CDTXMania.Timer);
            SineCounter = new CCounter(0, 360, CDTXMania.Skin.Game_PuchiChara_SineTimer, CSound管理.rc演奏用タイマ);
            base.On活性化();
        }
        public override void On非活性化()
        {
            Counter = null;
            SineCounter = null;
            base.On非活性化();
        }
        
        public void ChangeBPM(double bpm)
        {
            Counter = new CCounter(0, CDTXMania.Skin.Game_PuchiChara[2] - 1, (int)(CDTXMania.Skin.Game_PuchiChara_Timer * bpm / CDTXMania.Skin.Game_PuchiChara[2]), CDTXMania.Timer);
            SineCounter = new CCounter(1, 360, CDTXMania.Skin.Game_PuchiChara_SineTimer * bpm / 180, CSound管理.rc演奏用タイマ);
        }

        /// <summary>
        /// ぷちキャラを描画する。(オーバーライドじゃないよ)
        /// </summary>
        /// <param name="x">X座標(中央)</param>
        /// <param name="y">Y座標(中央)</param>
        /// <param name="alpha">不透明度</param>
        /// <returns></returns>
        public int On進行描画(int x, int y, bool isGrowing, int alpha = 255, bool isBalloon = false)
        {
            if (!CDTXMania.ConfigIni.ShowPuchiChara) return base.On進行描画();
            if (Counter == null || SineCounter == null || CDTXMania.Tx.PuchiChara == null) return base.On進行描画();
            Counter.t進行Loop();
            SineCounter.t進行LoopDb();
            var sineY = Math.Sin(SineCounter.db現在の値 * (Math.PI / 180)) * (CDTXMania.Skin.Game_PuchiChara_Sine * (isBalloon ? CDTXMania.Skin.Game_PuchiChara_Scale[1] : CDTXMania.Skin.Game_PuchiChara_Scale[0]));
            CDTXMania.Tx.PuchiChara.vc拡大縮小倍率 = new SlimDX.Vector3((isBalloon ? CDTXMania.Skin.Game_PuchiChara_Scale[1] : CDTXMania.Skin.Game_PuchiChara_Scale[0]));
            CDTXMania.Tx.PuchiChara.n透明度 = alpha;
            CDTXMania.Tx.PuchiChara.t2D中心基準描画(CDTXMania.app.Device, x, y + (int)sineY, new Rectangle(Counter.n現在の値 * CDTXMania.Skin.Game_PuchiChara[0], (isGrowing ? CDTXMania.Skin.Game_PuchiChara[1] : 0), CDTXMania.Skin.Game_PuchiChara[0], CDTXMania.Skin.Game_PuchiChara[1]));
            return base.On進行描画();
        }

        private CCounter Counter;
        private CCounter SineCounter;
    }
}