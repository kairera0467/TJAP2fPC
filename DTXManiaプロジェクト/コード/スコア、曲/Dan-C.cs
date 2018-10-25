using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTXMania
{
    /// <summary>
    /// 段位チャレンジを管理するクラス。
    /// </summary>
    class Dan_C
    {
        public Dan_C()
        {

        }

        /// <summary>
        /// 段位チャレンジの条件を初期化し、生成します。
        /// </summary>
        /// <param name="examType">条件の種別。</param>
        /// <param name="value">条件の合格量。</param>
        /// <param name="examRange">条件の合格の範囲。</param>
        public Dan_C(ExamType examType, int[] value, ExamRange examRange)
        {
            IsEnable = true;
            Type = examType;
            Value = value;
            Range = examRange;
        }

        /// <summary>
        /// 条件と現在の値を評価して、クリアしたかどうかを判断します。
        /// </summary>
        /// <param name="nowValue">その条件の現在の値。</param>
        public bool Update(int nowValue)
        {
            var isChangedAmount = false;
            if (!IsEnable) return isChangedAmount;
            if (Amount < nowValue) isChangedAmount = true;
            if (Range == ExamRange.Less && nowValue > Value[0]) isChangedAmount = false; // n未満でその数を超えたらfalseを返す。
            Amount = nowValue;
            switch (Type)
            {
                case ExamType.Gauge:
                    SetCleared();
                    break;
                case ExamType.JudgePerfect:
                        SetCleared();
                    break;
                case ExamType.JudgeGood:
                        SetCleared();
                    break;
                case ExamType.JudgeBad:
                        SetCleared();
                    break;
                case ExamType.Score:
                        SetCleared();
                    break;
                case ExamType.Roll:
                        SetCleared();
                    break;
                case ExamType.Hit:
                        SetCleared();
                    break;
                case ExamType.Combo:
                        SetCleared();
                    break;
                default:
                    break;
            }
            return isChangedAmount;
        }

        public bool[] GetCleared()
        {
            return IsCleared;
        }

        /// <summary>
        /// ゲージの描画のための百分率を返す。
        /// </summary>
        /// <returns>Amountの百分率。</returns>
        public int GetAmountToPercent()
        {
            var percent = 0.0D;
            if(Value[0] == 0)
            {
                return 0;
            }
            if(Range == ExamRange.More)
            {
                switch (Type)
                {
                    case ExamType.Gauge:
                    case ExamType.JudgePerfect:
                    case ExamType.JudgeGood:
                    case ExamType.JudgeBad:
                    case ExamType.Score:
                    case ExamType.Roll:
                    case ExamType.Hit:
                    case ExamType.Combo:
                        percent = 1.0 * Amount / Value[0];
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (Type)
                {
                    case ExamType.Gauge:
                    case ExamType.JudgePerfect:
                    case ExamType.JudgeGood:
                    case ExamType.JudgeBad:
                    case ExamType.Score:
                    case ExamType.Roll:
                    case ExamType.Hit:
                    case ExamType.Combo:
                        percent = (1.0 * (Value[0] - Amount)) / Value[0];
                        break;
                    default:
                        break;
                }
            }
            percent = percent * 100.0;
            if (percent < 0.0)
                percent = 0.0D;
            if (percent > 100.0)
                percent = 100.0D;
            return (int)percent;
        }
        
        private void SetCleared()
        {
            if (Range == ExamRange.More)
            {
                if (Amount >= Value[0])
                {
                    IsCleared[0] = true;
                    if (Amount >= Value[1])
                    {
                        IsCleared[1] = true;
                    }
                }
                else
                {
                    IsCleared[0] = false;
                    IsCleared[1] = false;
                }
            }
            else
            {
                if (Amount < Value[1])
                {
                    IsCleared[1] = true;
                }
                else
                {
                    IsCleared[1] = false;
                }
                if (Amount < Value[0])
                {
                    IsCleared[0] = true;
                }
                else
                {
                    IsCleared[0] = false;
                }
            }        
        }

        /// <summary>
        /// 段位チャレンジの条件の種別。
        /// </summary>
        public enum ExamType
        {
            Gauge,
            JudgePerfect,
            JudgeGood,
            JudgeBad,
            Score,
            Roll,
            Hit,
            Combo
        }

        /// <summary>
        /// 段位チャレンジの合格範囲。
        /// </summary>
        public enum ExamRange
        {
            /// <summary>
            /// 以上
            /// </summary>
            More,
            /// <summary>
            /// 未満
            /// </summary>
            Less
        }

        // フィールド
        /// <summary>
        /// その条件が有効であるかどうか。
        /// </summary>
        public bool IsEnable;
        /// <summary>
        /// 条件の種別。
        /// </summary>
        public ExamType Type;
        /// <summary>
        /// 条件の値。
        /// </summary>
        public int[] Value = new int[] { 0, 0 };
        /// <summary>
        /// 量。
        /// </summary>
        public int Amount;
        /// <summary>
        /// 条件の範囲。
        /// </summary>
        public ExamRange Range;

        /// <summary>
        /// 条件をクリアしているか否か。
        /// </summary>
        public readonly bool[] IsCleared = new[] { false, false };

    }
}
