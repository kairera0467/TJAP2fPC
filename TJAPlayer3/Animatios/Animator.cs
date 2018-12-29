using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TJAPlayer3;
using FDK;

namespace TJAPlayer3.Animatios
{
    class Animator : IAnimatable
    {
        public Animator(int startValue, int endValue, int tickInterval, bool isLoop)
        {
            Type = CounterType.Normal;
            StartValue = startValue;
            EndValue = endValue;
            TickInterval = tickInterval;
            IsLoop = isLoop;
            Counter = new CCounter();
        }
        public Animator(double startValue, double endValue, double tickInterval, bool isLoop)
        {
            Type = CounterType.Double;
            StartValue = startValue;
            EndValue = endValue;
            TickInterval = tickInterval;
            IsLoop = isLoop;
            Counter = new CCounter();
        }
        public void Start()
        {
            if (Counter == null) throw new NullReferenceException();
            switch (Type)
            {
                case CounterType.Normal:
                    Counter.t開始((int)StartValue, (int)EndValue, (int)TickInterval, TJAPlayer3.Timer);
                    break;
                case CounterType.Double:
                    Counter.t開始((double)StartValue, (double)EndValue, (double)TickInterval, CSound管理.rc演奏用タイマ);
                    break;
                default:
                    break;
            }
        }
        public void Stop()
        {
            if (Counter == null) throw new NullReferenceException();
            Counter.t停止();
        }
        public void Reset()
        {
            if (Counter == null) throw new NullReferenceException();
            Start();
        }

        public void Tick()
        {
            if (Counter == null) throw new NullReferenceException();
            switch (Type)
            {
                case CounterType.Normal:
                    if (IsLoop) Counter.t進行Loop(); else Counter.t進行();
                    break;
                case CounterType.Double:
                    if (IsLoop) Counter.t進行LoopDb(); else Counter.t進行db();
                    break;
                default:
                    break;
            }
        }

        public virtual object GetAnimation()
        {
            throw new NotImplementedException();
        }



        // フィールド
        protected CCounter Counter;
        protected readonly CounterType Type;
        protected readonly object StartValue;
        protected readonly object EndValue;
        protected readonly object TickInterval;
        protected readonly bool IsLoop;
    }

    enum CounterType
    {
        Normal,
        Double
    }
}
