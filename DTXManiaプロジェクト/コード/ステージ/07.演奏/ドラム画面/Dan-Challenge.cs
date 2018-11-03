using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using FDK;

namespace DTXMania
{
    internal class Dan_Challenge : CActivity
    {
        /// <summary>
        /// 段位チャレンジ
        /// </summary>
        public Dan_Challenge()
        {
            base.b活性化してない = true;
        }

        //
        Dan_C[] Challenge = new Dan_C[3];
        //

        public override void On活性化()
        {
            Challenge[0] = new Dan_C(Dan_C.ExamType.Gauge, new int[] { 95, 100 }, Dan_C.ExamRange.More);
            Challenge[1] = new Dan_C(Dan_C.ExamType.Combo, new int[] { 360, 520 }, Dan_C.ExamRange.More);
            Challenge[2] = new Dan_C(Dan_C.ExamType.Hit, new int[] { 1800, 1900 }, Dan_C.ExamRange.More);
            base.On活性化();
        }

        public void Update()
        {
            for (int i = 0; i < 3; i++)
            {
                switch (Challenge[i].Type)
                {
                    case Dan_C.ExamType.Gauge:
                        Challenge[i].Update((int)CDTXMania.stage演奏ドラム画面.actGauge.db現在のゲージ値[0]);
                        break;
                    case Dan_C.ExamType.JudgePerfect:
                        Challenge[i].Update((int)CDTXMania.stage演奏ドラム画面.nヒット数_Auto含む.Drums.Perfect);
                        break;
                    case Dan_C.ExamType.JudgeGood:
                        Challenge[i].Update((int)CDTXMania.stage演奏ドラム画面.nヒット数_Auto含む.Drums.Great);
                        break;
                    case Dan_C.ExamType.JudgeBad:
                        Challenge[i].Update((int)CDTXMania.stage演奏ドラム画面.nヒット数_Auto含む.Drums.Miss);
                        break;
                    case Dan_C.ExamType.Score:
                        Challenge[i].Update((int)CDTXMania.stage演奏ドラム画面.actScore.GetScore(0));
                        break;
                    case Dan_C.ExamType.Roll:
                        Challenge[i].Update((int)(CDTXMania.stage演奏ドラム画面.GetRoll(0)));
                        break;
                    case Dan_C.ExamType.Hit:
                        Challenge[i].Update((int)(CDTXMania.stage演奏ドラム画面.nヒット数_Auto含む.Drums.Perfect + CDTXMania.stage演奏ドラム画面.nヒット数_Auto含む.Drums.Great + CDTXMania.stage演奏ドラム画面.GetRoll(0)));
                        break;
                    case Dan_C.ExamType.Combo:
                        Challenge[i].Update((int)CDTXMania.stage演奏ドラム画面.actCombo.n現在のコンボ数.P1最高値);
                        break;
                    default:
                        break;
                }
            }
        }

        public override void On非活性化()
        {
            for (int i = 0; i < 3; i++)
            {
                Challenge[i] = null;
            }
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
            for (int i = 0; i < 3; i++)
            {
                CDTXMania.act文字コンソール.tPrint(0, 20 * i, C文字コンソール.Eフォント種別.白, String.Format("Type: {0} / Value: {1}/{2} / Range: {3} / Amount: {4} / Clear: {5}/{6}", Challenge[i].Type.ToString(), Challenge[i].Value[0].ToString(), Challenge[i].Value[1].ToString(), Challenge[i].Range.ToString(), Challenge[i].Amount.ToString(), Challenge[i].IsCleared[0].ToString(), Challenge[i].IsCleared[1].ToString()));
            }

            return base.On進行描画();
        }

        #region[ private ]
        //-----------------
        //-----------------
        #endregion
    }
}
