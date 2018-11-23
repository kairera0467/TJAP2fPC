using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using FDK;
using System.IO;
using TJAPlayer3;

namespace DTXMania
{
    internal class Dan_Cert : CActivity
    {
        /// <summary>
        /// 段位認定
        /// </summary>
        public Dan_Cert()
        {
            base.b活性化してない = true;
        }

        //
        Dan_C[] Challenge = new Dan_C[3];
        //

        public void Start(int number)
        {
            NowShowingNumber = number;
            Counter_In = new CCounter(0, 1000, 1, CDTXMania.Timer);
            ScreenPoint = new double[] { CDTXMania.Skin.nScrollFieldBGX[0] - CDTXMania.Tx.DanC_Screen.szテクスチャサイズ.Width / 2, 1280 };
            IsAnimating = true;
        }

        public override void On活性化()
        {
            Challenge[0] = CDTXMania.DTX.Dan_C[0];
            Challenge[1] = CDTXMania.DTX.Dan_C[1];
            Challenge[2] = CDTXMania.DTX.Dan_C[2];

            // 始点を決定する。
            ExamCount = 0;
            for (int i = 0; i < 3; i++)
            {
                if (Challenge[i] != null && Challenge[i].GetEnable() == true)
                    this.ExamCount++;
            }

            for (int i = 0; i < 3; i++)
            {
                Status[i] = new ChallengeStatus();
                Status[i].Timer_Amount = new CCounter();
                Status[i].Timer_Gauge = new CCounter();
                Status[i].Timer_Failed = new CCounter();
            }
            Update();
            IsEnded = false;
            base.On活性化();
        }

        public void Update()
        {
            for (int i = 0; i < 3; i++)
            {
                if (Challenge[i] == null || !Challenge[i].GetEnable()) return;
                var isChangedAmount = false;
                switch (Challenge[i].GetExamType())
                {
                    case Exam.Type.Gauge:
                        isChangedAmount = Challenge[i].Update((int)CDTXMania.stage演奏ドラム画面.actGauge.db現在のゲージ値[0]);
                        break;
                    case Exam.Type.JudgePerfect:
                        isChangedAmount = Challenge[i].Update((int)CDTXMania.stage演奏ドラム画面.nヒット数_Auto含む.Drums.Perfect + CDTXMania.stage演奏ドラム画面.nヒット数_Auto含まない.Drums.Perfect);
                        break;
                    case Exam.Type.JudgeGood:
                        isChangedAmount = Challenge[i].Update((int)CDTXMania.stage演奏ドラム画面.nヒット数_Auto含む.Drums.Great + CDTXMania.stage演奏ドラム画面.nヒット数_Auto含まない.Drums.Great);
                        break;
                    case Exam.Type.JudgeBad:
                        isChangedAmount = Challenge[i].Update((int)CDTXMania.stage演奏ドラム画面.nヒット数_Auto含まない.Drums.Miss);
                        break;
                    case Exam.Type.Score:
                        isChangedAmount = Challenge[i].Update((int)CDTXMania.stage演奏ドラム画面.actScore.GetScore(0));
                        break;
                    case Exam.Type.Roll:
                        isChangedAmount = Challenge[i].Update((int)(CDTXMania.stage演奏ドラム画面.GetRoll(0)));
                        break;
                    case Exam.Type.Hit:
                        isChangedAmount = Challenge[i].Update((int)(CDTXMania.stage演奏ドラム画面.nヒット数_Auto含む.Drums.Perfect + CDTXMania.stage演奏ドラム画面.nヒット数_Auto含まない.Drums.Perfect + CDTXMania.stage演奏ドラム画面.nヒット数_Auto含む.Drums.Great + CDTXMania.stage演奏ドラム画面.nヒット数_Auto含まない.Drums.Great + CDTXMania.stage演奏ドラム画面.GetRoll(0)));
                        break;
                    case Exam.Type.Combo:
                        isChangedAmount = Challenge[i].Update((int)CDTXMania.stage演奏ドラム画面.actCombo.n現在のコンボ数.P1最高値);
                        break;
                    default:
                        break;
                }

                // 値が変更されていたらアニメーションを行う。
                if (isChangedAmount)
                {
                    if(Status[i].Timer_Amount != null && Status[i].Timer_Amount.b終了値に達してない)
                    {
                        Status[i].Timer_Amount = new CCounter(0, 11, 12, CDTXMania.Timer);
                        Status[i].Timer_Amount.n現在の値 = 1;
                    }
                    else
                    {
                        Status[i].Timer_Amount = new CCounter(0, 11, 12, CDTXMania.Timer);
                    }
                }

                // 条件の達成見込みがあるかどうか判断する。
                if (Challenge[i].GetExamRange() == Exam.Range.Less)
                {
                    Challenge[i].SetReached(!Challenge[i].IsCleared[0]);
                }
                else
                {
                    // 残り音符数が0になったときに判断されるやつ
                    if (CDTXMania.DTX.nノーツ数[3] - (CDTXMania.stage演奏ドラム画面.nヒット数_Auto含む.Drums.Perfect + CDTXMania.stage演奏ドラム画面.nヒット数_Auto含まない.Drums.Perfect) - (CDTXMania.stage演奏ドラム画面.nヒット数_Auto含む.Drums.Great + CDTXMania.stage演奏ドラム画面.nヒット数_Auto含まない.Drums.Great) - (CDTXMania.stage演奏ドラム画面.nヒット数_Auto含む.Drums.Miss + CDTXMania.stage演奏ドラム画面.nヒット数_Auto含まない.Drums.Miss) <= 0)
                    {
                        // 残り音符数ゼロ
                        switch (Challenge[i].GetExamType())
                        {
                            case Exam.Type.Gauge:
                                if (Challenge[i].Amount < Challenge[i].Value[0]) Challenge[i].SetReached(true);
                                break;
                            case Exam.Type.Score:
                                if (Challenge[i].Amount < Challenge[i].Value[0]) Challenge[i].SetReached(true);
                                break;
                            default:
                                // 何もしない
                                break;
                        }
                    }
                    // 常に監視されるやつ。
                    switch (Challenge[i].GetExamType())
                    {
                        case Exam.Type.JudgePerfect:
                        case Exam.Type.JudgeGood:
                        case Exam.Type.JudgeBad:
                        case Exam.Type.Combo:
                            if ((CDTXMania.DTX.nノーツ数[3] - (CDTXMania.stage演奏ドラム画面.nヒット数_Auto含む.Drums.Perfect + CDTXMania.stage演奏ドラム画面.nヒット数_Auto含まない.Drums.Perfect) - (CDTXMania.stage演奏ドラム画面.nヒット数_Auto含む.Drums.Great + CDTXMania.stage演奏ドラム画面.nヒット数_Auto含まない.Drums.Great) - (CDTXMania.stage演奏ドラム画面.nヒット数_Auto含む.Drums.Miss + CDTXMania.stage演奏ドラム画面.nヒット数_Auto含まない.Drums.Miss) < (Challenge[i].Value[0] - Challenge[i].Amount))) Challenge[i].SetReached(true);
                            break;
                        default:
                            break;
                    }

                    // 音源が終了したやつの分岐。
                    // ( CDTXMania.DTX.listChip.Count > 0 ) ? CDTXMania.DTX.listChip[ CDTXMania.DTX.listChip.Count - 1 ].n発声時刻ms : 0;
                    if(!IsEnded)
                    {
                        if (CDTXMania.DTX.listChip.Count <= 0) continue;
                        if (CDTXMania.DTX.listChip[CDTXMania.DTX.listChip.Count - 1].n発声時刻ms < CDTXMania.Timer.n現在時刻)
                        {
                            switch (Challenge[i].GetExamType())
                            {
                                case Exam.Type.Score:
                                case Exam.Type.Roll:
                                case Exam.Type.Hit:
                                    if (Challenge[i].Amount < Challenge[i].Value[0]) Challenge[i].SetReached(true);
                                    break;
                                default:
                                    break;
                            }
                            IsEnded = true;
                        }
                    }
                }
            }
        }

        public override void On非活性化()
        {
            for (int i = 0; i < 3; i++)
            {
                Challenge[i] = null;
            }

            for (int i = 0; i < 3; i++)
            {
                Status[i].Timer_Amount = null;
                Status[i].Timer_Gauge = null;
                Status[i].Timer_Failed = null;
            }
            IsEnded = false;
            base.On非活性化();
        }

        public override void OnManagedリソースの作成()
        {
            Dan_Plate = CDTXMania.tテクスチャの生成(Path.GetDirectoryName(CDTXMania.DTX.strファイル名の絶対パス) + @"\Dan_Plate.png");
            base.OnManagedリソースの作成();
        }

        public override void OnManagedリソースの解放()
        {
            Dan_Plate?.Dispose();
            base.OnManagedリソースの解放();
        }

        public override int On進行描画()
        {
            Counter_In?.t進行();
            Counter_Wait?.t進行();
            Counter_Out?.t進行();
            Counter_Text?.t進行();
            for (int i = 0; i < 3; i++)
            {
                Status[i].Timer_Amount?.t進行();
            }

            for (int i = 0; i < 3; i++)
            {
                if (Challenge[i] != null && Challenge[i].GetEnable())
                    CDTXMania.act文字コンソール.tPrint(0, 20 * i, C文字コンソール.Eフォント種別.白, Challenge[i].ToString());
                else
                    CDTXMania.act文字コンソール.tPrint(0, 20 * i, C文字コンソール.Eフォント種別.白, "None");
            }
            CDTXMania.act文字コンソール.tPrint(0, 80, C文字コンソール.Eフォント種別.白, String.Format("Notes Remain: {0}", CDTXMania.DTX.nノーツ数[3] - (CDTXMania.stage演奏ドラム画面.nヒット数_Auto含む.Drums.Perfect + CDTXMania.stage演奏ドラム画面.nヒット数_Auto含まない.Drums.Perfect) - (CDTXMania.stage演奏ドラム画面.nヒット数_Auto含む.Drums.Great + CDTXMania.stage演奏ドラム画面.nヒット数_Auto含まない.Drums.Great) - (CDTXMania.stage演奏ドラム画面.nヒット数_Auto含む.Drums.Miss + CDTXMania.stage演奏ドラム画面.nヒット数_Auto含まない.Drums.Miss)));

            // 背景を描画する。

            CDTXMania.Tx.DanC_Background?.t2D描画(CDTXMania.app.Device, 0, 0);
        

            // 残り音符数を描画する。
            var notesRemain = CDTXMania.DTX.nノーツ数[3] - (CDTXMania.stage演奏ドラム画面.nヒット数_Auto含む.Drums.Perfect + CDTXMania.stage演奏ドラム画面.nヒット数_Auto含まない.Drums.Perfect) - (CDTXMania.stage演奏ドラム画面.nヒット数_Auto含む.Drums.Great + CDTXMania.stage演奏ドラム画面.nヒット数_Auto含まない.Drums.Great) - (CDTXMania.stage演奏ドラム画面.nヒット数_Auto含む.Drums.Miss + CDTXMania.stage演奏ドラム画面.nヒット数_Auto含まない.Drums.Miss);

            DrawNumber(notesRemain, CDTXMania.Skin.Game_DanC_Number_XY[0], CDTXMania.Skin.Game_DanC_Number_XY[1], CDTXMania.Skin.Game_DanC_Number_Padding);

            // 段プレートを描画する。
            Dan_Plate?.t2D中心基準描画(CDTXMania.app.Device, CDTXMania.Skin.Game_DanC_Dan_Plate[0], CDTXMania.Skin.Game_DanC_Dan_Plate[1]);

            for (int i = 0; i < this.ExamCount; i++)
            {
                #region ゲージの土台を描画する。
                CDTXMania.Tx.DanC_Base?.t2D描画(CDTXMania.app.Device, CDTXMania.Skin.Game_DanC_X[ExamCount - 1], CDTXMania.Skin.Game_DanC_Y[ExamCount - 1] + CDTXMania.Skin.Game_DanC_Size[1] * i + (i * CDTXMania.Skin.Game_DanC_Padding));
                #endregion


                #region ゲージを描画する。
                var drawGaugeType = 0;
                if (Challenge[i].GetExamRange() == Exam.Range.More)
                {
                    if (Challenge[i].GetAmountToPercent() >= 100)
                        drawGaugeType = 2;
                    else if (Challenge[i].GetAmountToPercent() >= 70)
                        drawGaugeType = 1;
                    else
                        drawGaugeType = 0;
                }
                else
                {
                    if (Challenge[i].GetAmountToPercent() >= 100)
                        drawGaugeType = 2;
                    else if (Challenge[i].GetAmountToPercent() > 70)
                        drawGaugeType = 1;
                    else
                        drawGaugeType = 0;
                }
                CDTXMania.Tx.DanC_Gauge[drawGaugeType]?.t2D拡大率考慮下基準描画(CDTXMania.app.Device,
                    CDTXMania.Skin.Game_DanC_X[ExamCount - 1] + CDTXMania.Skin.Game_DanC_Offset[0], CDTXMania.Skin.Game_DanC_Y[ExamCount - 1] + CDTXMania.Skin.Game_DanC_Size[1] * (i + 1) + ((i + 1) * CDTXMania.Skin.Game_DanC_Padding) - CDTXMania.Skin.Game_DanC_Offset[1], new Rectangle(0, 0, (int)(Challenge[i].GetAmountToPercent() * 9.26), CDTXMania.Tx.DanC_Gauge[drawGaugeType].szテクスチャサイズ.Height));
                #endregion

                #region 現在の値を描画する。
                var nowAmount = 0;
                if (Challenge[i].GetExamRange() == Exam.Range.Less)
                {
                    nowAmount = Challenge[i].Value[0] - Challenge[i].Amount;
                }
                else
                {
                    nowAmount = Challenge[i].Amount;
                }
                if (nowAmount < 0) nowAmount = 0;
                
                DrawNumber(nowAmount, CDTXMania.Skin.Game_DanC_X[ExamCount - 1] + CDTXMania.Skin.Game_DanC_Number_Small_Number_Offset[0], CDTXMania.Skin.Game_DanC_Y[ExamCount - 1] + CDTXMania.Skin.Game_DanC_Size[1] * (i + 1) + ((i + 1) * CDTXMania.Skin.Game_DanC_Padding) - CDTXMania.Skin.Game_DanC_Number_Small_Number_Offset[1], CDTXMania.Skin.Game_DanC_Number_Small_Padding, CDTXMania.Skin.Game_DanC_Number_Small_Scale, CDTXMania.Skin.Game_DanC_Number_Small_Scale,ScoreScale[Status[i].Timer_Amount.n現在の値]);

                // 単位(あれば)
                switch (Challenge[i].GetExamType())
                {
                    case Exam.Type.Gauge:
                        // パーセント
                        CDTXMania.Tx.DanC_ExamUnit?.t2D拡大率考慮下基準描画(CDTXMania.app.Device, CDTXMania.Skin.Game_DanC_X[ExamCount - 1] + CDTXMania.Skin.Game_DanC_Number_Small_Number_Offset[0] + CDTXMania.Skin.Game_DanC_Number_Padding / 4 - CDTXMania.Skin.Game_DanC_Percent_Hit_Score_Padding[0], CDTXMania.Skin.Game_DanC_Y[ExamCount - 1] + CDTXMania.Skin.Game_DanC_Size[1] * (i + 1) + ((i + 1) * CDTXMania.Skin.Game_DanC_Padding) - CDTXMania.Skin.Game_DanC_Number_Small_Number_Offset[1], new Rectangle(0, CDTXMania.Skin.Game_DanC_ExamUnit_Size[1] * 0, CDTXMania.Skin.Game_DanC_ExamUnit_Size[0], CDTXMania.Skin.Game_DanC_ExamUnit_Size[1]));
                        break;
                    case Exam.Type.Score:
                        CDTXMania.Tx.DanC_ExamUnit?.t2D拡大率考慮下基準描画(CDTXMania.app.Device, CDTXMania.Skin.Game_DanC_X[ExamCount - 1] + CDTXMania.Skin.Game_DanC_Number_Small_Number_Offset[0] + CDTXMania.Skin.Game_DanC_Number_Padding / 4 - CDTXMania.Skin.Game_DanC_Percent_Hit_Score_Padding[2], CDTXMania.Skin.Game_DanC_Y[ExamCount - 1] + CDTXMania.Skin.Game_DanC_Size[1] * (i + 1) + ((i + 1) * CDTXMania.Skin.Game_DanC_Padding) - CDTXMania.Skin.Game_DanC_Number_Small_Number_Offset[1], new Rectangle(0, CDTXMania.Skin.Game_DanC_ExamUnit_Size[1] * 2, CDTXMania.Skin.Game_DanC_ExamUnit_Size[0], CDTXMania.Skin.Game_DanC_ExamUnit_Size[1]));

                        // 点
                        break;
                    case Exam.Type.Roll:
                    case Exam.Type.Hit:
                        CDTXMania.Tx.DanC_ExamUnit?.t2D拡大率考慮下基準描画(CDTXMania.app.Device, CDTXMania.Skin.Game_DanC_X[ExamCount - 1] + CDTXMania.Skin.Game_DanC_Number_Small_Number_Offset[0] + CDTXMania.Skin.Game_DanC_Number_Padding / 4 - CDTXMania.Skin.Game_DanC_Percent_Hit_Score_Padding[1], CDTXMania.Skin.Game_DanC_Y[ExamCount - 1] + CDTXMania.Skin.Game_DanC_Size[1] * (i + 1) + ((i + 1) * CDTXMania.Skin.Game_DanC_Padding) - CDTXMania.Skin.Game_DanC_Number_Small_Number_Offset[1], new Rectangle(0, CDTXMania.Skin.Game_DanC_ExamUnit_Size[1] * 1, CDTXMania.Skin.Game_DanC_ExamUnit_Size[0], CDTXMania.Skin.Game_DanC_ExamUnit_Size[1]));

                        // 打
                        break;
                    default:
                        // 何もしない
                        break;
                }

                #endregion


                #region 条件の文字を描画する。
                var offset = CDTXMania.Skin.Game_DanC_Exam_Offset[0];
                //offset -= CDTXMania.Skin.Game_DanC_ExamRange_Padding;
                // 条件の範囲
                CDTXMania.Tx.DanC_ExamRange?.t2D拡大率考慮下基準描画(CDTXMania.app.Device, CDTXMania.Skin.Game_DanC_X[ExamCount - 1] + offset - CDTXMania.Tx.DanC_ExamRange.szテクスチャサイズ.Width, CDTXMania.Skin.Game_DanC_Y[ExamCount - 1] + CDTXMania.Skin.Game_DanC_Size[1] * (i + 1) + ((i + 1) * CDTXMania.Skin.Game_DanC_Padding) - CDTXMania.Skin.Game_DanC_Exam_Offset[1], new Rectangle(0, CDTXMania.Skin.Game_DanC_ExamRange_Size[1] * (int)Challenge[i].GetExamRange(), CDTXMania.Skin.Game_DanC_ExamRange_Size[0], CDTXMania.Skin.Game_DanC_ExamRange_Size[1]));
                //offset -= CDTXMania.Skin.Game_DanC_ExamRange_Padding;
                offset -= CDTXMania.Skin.Game_DanC_ExamRange_Padding;

                // 単位(あれば)
                switch (Challenge[i].GetExamType())
                {
                    case Exam.Type.Gauge:
                        // パーセント
                        CDTXMania.Tx.DanC_ExamUnit?.t2D拡大率考慮下基準描画(CDTXMania.app.Device, CDTXMania.Skin.Game_DanC_X[ExamCount - 1] + offset - CDTXMania.Tx.DanC_ExamUnit.szテクスチャサイズ.Width, CDTXMania.Skin.Game_DanC_Y[ExamCount - 1] + CDTXMania.Skin.Game_DanC_Size[1] * (i + 1) + ((i + 1) * CDTXMania.Skin.Game_DanC_Padding) - CDTXMania.Skin.Game_DanC_Exam_Offset[1], new Rectangle(0, CDTXMania.Skin.Game_DanC_ExamUnit_Size[1] * 0, CDTXMania.Skin.Game_DanC_ExamUnit_Size[0], CDTXMania.Skin.Game_DanC_ExamUnit_Size[1]));
                        offset -= CDTXMania.Skin.Game_DanC_Percent_Hit_Score_Padding[0];
                        break;
                    case Exam.Type.Score:
                        CDTXMania.Tx.DanC_ExamUnit?.t2D拡大率考慮下基準描画(CDTXMania.app.Device, CDTXMania.Skin.Game_DanC_X[ExamCount - 1] + offset - CDTXMania.Tx.DanC_ExamUnit.szテクスチャサイズ.Width, CDTXMania.Skin.Game_DanC_Y[ExamCount - 1] + CDTXMania.Skin.Game_DanC_Size[1] * (i + 1) + ((i + 1) * CDTXMania.Skin.Game_DanC_Padding) - CDTXMania.Skin.Game_DanC_Exam_Offset[1], new Rectangle(0, CDTXMania.Skin.Game_DanC_ExamUnit_Size[1] * 2, CDTXMania.Skin.Game_DanC_ExamUnit_Size[0], CDTXMania.Skin.Game_DanC_ExamUnit_Size[1]));
                        offset -= CDTXMania.Skin.Game_DanC_Percent_Hit_Score_Padding[2];

                        // 点
                        break;
                    case Exam.Type.Roll:
                    case Exam.Type.Hit:
                        CDTXMania.Tx.DanC_ExamUnit?.t2D拡大率考慮下基準描画(CDTXMania.app.Device, CDTXMania.Skin.Game_DanC_X[ExamCount - 1] + offset - CDTXMania.Tx.DanC_ExamUnit.szテクスチャサイズ.Width, CDTXMania.Skin.Game_DanC_Y[ExamCount - 1] + CDTXMania.Skin.Game_DanC_Size[1] * (i + 1) + ((i + 1) * CDTXMania.Skin.Game_DanC_Padding) - CDTXMania.Skin.Game_DanC_Exam_Offset[1], new Rectangle(0, CDTXMania.Skin.Game_DanC_ExamUnit_Size[1] * 1, CDTXMania.Skin.Game_DanC_ExamUnit_Size[0], CDTXMania.Skin.Game_DanC_ExamUnit_Size[1]));
                        offset -= CDTXMania.Skin.Game_DanC_Percent_Hit_Score_Padding[1];

                        // 打
                        break;
                    default:
                        // 何もしない
                        break;
                }

                // 条件の数字
                DrawNumber(Challenge[i].Value[0], CDTXMania.Skin.Game_DanC_X[ExamCount - 1] + offset, CDTXMania.Skin.Game_DanC_Y[ExamCount - 1] + CDTXMania.Skin.Game_DanC_Size[1] * (i + 1) + ((i + 1) * CDTXMania.Skin.Game_DanC_Padding) - CDTXMania.Skin.Game_DanC_Exam_Offset[1], CDTXMania.Skin.Game_DanC_Number_Small_Padding, CDTXMania.Skin.Game_DanC_Number_Small_Scale, CDTXMania.Skin.Game_DanC_Number_Small_Scale);
                //offset -= CDTXMania.Skin.Game_DanC_Number_Small_Padding * (Challenge[i].Value[0].ToString().Length + 1);
                offset -= CDTXMania.Skin.Game_DanC_Number_Small_Padding * (Challenge[i].Value[0].ToString().Length);

                // 条件の種類
                CDTXMania.Tx.DanC_ExamType?.t2D拡大率考慮下基準描画(CDTXMania.app.Device, CDTXMania.Skin.Game_DanC_X[ExamCount - 1] + offset - CDTXMania.Tx.DanC_ExamType.szテクスチャサイズ.Width, CDTXMania.Skin.Game_DanC_Y[ExamCount - 1] + CDTXMania.Skin.Game_DanC_Size[1] * (i + 1) + ((i + 1) * CDTXMania.Skin.Game_DanC_Padding) - CDTXMania.Skin.Game_DanC_Exam_Offset[1], new Rectangle(0, CDTXMania.Skin.Game_DanC_ExamType_Size[1] * (int)Challenge[i].GetExamType(), CDTXMania.Skin.Game_DanC_ExamType_Size[0], CDTXMania.Skin.Game_DanC_ExamType_Size[1]));
                #endregion

                #region 条件達成失敗の画像を描画する。
                if(Challenge[i].GetReached())
                {
                    CDTXMania.Tx.DanC_Failed.t2D描画(CDTXMania.app.Device, CDTXMania.Skin.Game_DanC_X[ExamCount - 1], CDTXMania.Skin.Game_DanC_Y[ExamCount - 1] + CDTXMania.Skin.Game_DanC_Size[1] * i + (i * CDTXMania.Skin.Game_DanC_Padding));
                }
                #endregion
            }

            // 幕のアニメーション
            if (Counter_In != null)
            {
                if(Counter_In.b終了値に達してない)
                {
                    for (int i = Counter_In_Old; i < Counter_In.n現在の値; i++)
                    {
                        ScreenPoint[0] += (CDTXMania.Skin.nScrollFieldBGX[0] - ScreenPoint[0]) / 180.0;
                        ScreenPoint[1] += ((1280 / 2 + CDTXMania.Skin.nScrollFieldBGX[0] / 2) - ScreenPoint[1]) / 180.0;
                    }
                    Counter_In_Old = Counter_In.n現在の値;
                    CDTXMania.Tx.DanC_Screen?.t2D描画(CDTXMania.app.Device, (int)ScreenPoint[0], CDTXMania.Skin.nScrollFieldY[0], new Rectangle(0, 0, CDTXMania.Tx.DanC_Screen.szテクスチャサイズ.Width / 2, CDTXMania.Tx.DanC_Screen.szテクスチャサイズ.Height));
                    CDTXMania.Tx.DanC_Screen?.t2D描画(CDTXMania.app.Device, (int)ScreenPoint[1], CDTXMania.Skin.nScrollFieldY[0], new Rectangle(CDTXMania.Tx.DanC_Screen.szテクスチャサイズ.Width / 2, 0, CDTXMania.Tx.DanC_Screen.szテクスチャサイズ.Width / 2, CDTXMania.Tx.DanC_Screen.szテクスチャサイズ.Height));
                    CDTXMania.act文字コンソール.tPrint(0, 420, C文字コンソール.Eフォント種別.白, String.Format("{0} : {1}", ScreenPoint[0], ScreenPoint[1]));
                }
                if(Counter_In.b終了値に達した)
                {
                    Counter_In = null;
                    Counter_Wait = new CCounter(0, 3000, 1, CDTXMania.Timer);
                }
            }
            if (Counter_Wait != null)
            {
                if(Counter_Wait.b終了値に達してない)
                {
                    CDTXMania.Tx.DanC_Screen?.t2D描画(CDTXMania.app.Device, CDTXMania.Skin.nScrollFieldBGX[0], CDTXMania.Skin.nScrollFieldY[0]);
                }
                if(Counter_Wait.b終了値に達した)
                {
                    Counter_Wait = null;
                    Counter_Out = new CCounter(0, 500, 1, CDTXMania.Timer);
                    Counter_Text = new CCounter(0, 3000, 1, CDTXMania.Timer);
                }
            }
            if (Counter_Text != null)
            {
                // すげぇ深いネストだ!!!
                if (Counter_Text.b終了値に達してない)
                {
                    if (Counter_Text.n現在の値 > 2000)
                    {
                        for (int i = Counter_Text_Old; i < Counter_Text.n現在の値; i++)
                        {
                            if(Counter_Text.n現在の値 % 2 == 0)
                            {
                                if (CDTXMania.DTX.List_DanSongs[NowShowingNumber].Title != null)
                                {
                                    CDTXMania.DTX.List_DanSongs[NowShowingNumber].Title.n透明度--;
                                }
                                if (CDTXMania.DTX.List_DanSongs[NowShowingNumber].SubTitle != null)
                                {
                                    CDTXMania.DTX.List_DanSongs[NowShowingNumber].SubTitle.n透明度--;
                                }
                            }
                        }
                        Counter_Text_Old = Counter_Text.n現在の値;
                    }
                    
                    if (CDTXMania.DTX.List_DanSongs[NowShowingNumber].SubTitle == null)
                        CDTXMania.DTX.List_DanSongs[NowShowingNumber].Title?.t2D中心基準描画(CDTXMania.app.Device, 1280 / 2 + CDTXMania.Skin.nScrollFieldBGX[0] / 2, CDTXMania.Skin.nScrollFieldY[0] + 65);
                    else
                    {
                        CDTXMania.DTX.List_DanSongs[NowShowingNumber].Title?.t2D中心基準描画(CDTXMania.app.Device, 1280 / 2 + CDTXMania.Skin.nScrollFieldBGX[0] / 2, CDTXMania.Skin.nScrollFieldY[0] + 45);
                        CDTXMania.DTX.List_DanSongs[NowShowingNumber].SubTitle?.t2D中心基準描画(CDTXMania.app.Device, 1280 / 2 + CDTXMania.Skin.nScrollFieldBGX[0] / 2, CDTXMania.Skin.nScrollFieldY[0] + 85);
                    }
                }
                if (Counter_Text.b終了値に達した)
                {
                    Counter_Text = null;
                }
            }
            if (Counter_Out != null)
            {
                if (Counter_Out.b終了値に達してない)
                {
                    for (int i = Counter_Out_Old; i < Counter_Out.n現在の値; i++)
                    {
                        ScreenPoint[0] += -2;
                        ScreenPoint[1] += 2;
                    }
                    Counter_Out_Old = Counter_Out.n現在の値;
                    CDTXMania.Tx.DanC_Screen?.t2D描画(CDTXMania.app.Device, (int)ScreenPoint[0], CDTXMania.Skin.nScrollFieldY[0], new Rectangle(0, 0, CDTXMania.Tx.DanC_Screen.szテクスチャサイズ.Width / 2, CDTXMania.Tx.DanC_Screen.szテクスチャサイズ.Height));
                    CDTXMania.Tx.DanC_Screen?.t2D描画(CDTXMania.app.Device, (int)ScreenPoint[1], CDTXMania.Skin.nScrollFieldY[0], new Rectangle(CDTXMania.Tx.DanC_Screen.szテクスチャサイズ.Width / 2, 0, CDTXMania.Tx.DanC_Screen.szテクスチャサイズ.Width / 2, CDTXMania.Tx.DanC_Screen.szテクスチャサイズ.Height));
                    CDTXMania.act文字コンソール.tPrint(0, 420, C文字コンソール.Eフォント種別.白, String.Format("{0} : {1}", ScreenPoint[0], ScreenPoint[1]));
                }
                if (Counter_Out.b終了値に達した)
                {
                    Counter_Out = null;
                    IsAnimating = false;
                }
            }

            return base.On進行描画();
        }

        /// <summary>
        /// 段位チャレンジの数字フォントで数字を描画します。
        /// </summary>
        /// <param name="value">値。</param>
        /// <param name="x">一桁目のX座標。</param>
        /// <param name="y">一桁目のY座標</param>
        /// <param name="padding">桁数間の字間</param>
        /// <param name="scaleX">拡大率X</param>
        /// <param name="scaleY">拡大率Y</param>
        /// <param name="scaleJump">アニメーション用拡大率(Yに加算される)。</param>
        private void DrawNumber(int value, int x, int y, int padding, float scaleX = 1.0f, float scaleY = 1.0f, float scaleJump = 0.0f)
        {
            var notesRemainDigit = 0;
            for (int i = value.ToString().Length; i > 0; i--)
            {
                var number = Convert.ToInt32(value.ToString()[i - 1].ToString());
                Rectangle rectangle = new Rectangle(CDTXMania.Skin.Game_DanC_Number_Size[0] * number - 1, 0, CDTXMania.Skin.Game_DanC_Number_Size[0], CDTXMania.Skin.Game_DanC_Number_Size[1]);
                if(CDTXMania.Tx.DanC_Number != null)
                {
                    CDTXMania.Tx.DanC_Number.vc拡大縮小倍率.X = scaleX;
                    CDTXMania.Tx.DanC_Number.vc拡大縮小倍率.Y = scaleY + scaleJump;
                }
                CDTXMania.Tx.DanC_Number?.t2D拡大率考慮下中心基準描画(CDTXMania.app.Device, x - (notesRemainDigit * padding), y, rectangle);
                notesRemainDigit++;
            }
        }

        private readonly float[] ScoreScale = new float[]
        {
            0.000f,
            0.111f, // リピート
            0.222f,
            0.185f,
            0.148f,
            0.129f,
            0.111f,
            0.074f,
            0.065f,
            0.033f,
            0.015f,
            0.000f
        };

        [StructLayout(LayoutKind.Sequential)]
        struct ChallengeStatus
        {
            public SlimDX.Color4 Color;
            public CCounter Timer_Gauge;
            public CCounter Timer_Amount;
            public CCounter Timer_Failed;
        }
        
        #region[ private ]
        //-----------------
        private int ExamCount;
        private ChallengeStatus[] Status = new ChallengeStatus[3];
        private CTexture Dan_Plate;
        private bool IsEnded;

        // アニメ関連
        private int NowShowingNumber;
        private CCounter Counter_In, Counter_Wait, Counter_Out, Counter_Text;
        private double[] ScreenPoint;
        private int Counter_In_Old, Counter_Out_Old, Counter_Text_Old;
        public bool IsAnimating;
        //-----------------
        #endregion
    }
}
