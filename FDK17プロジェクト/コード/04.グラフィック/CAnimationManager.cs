using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Animation;

namespace FDK
{
    /// <summary>
    /// DTXMania2のFDKから移植
    /// </summary>
    public class CAnimationManager : IDisposable
    {
        public Manager Manager => this._Manager;
        public Timer Timer => this._Timer;
        public TransitionLibrary TrasitionLibrary => this._TrasitionLibrary;

        public CAnimationManager()
        {
            this._Manager = new Manager();
            this._Timer = new Timer();
            this._TrasitionLibrary = new TransitionLibrary();
        }
        public void t進行()
        {
            this._Manager.Update( this._Timer.Time );
        }
        public void Dispose()
        {
            this._TrasitionLibrary?.Dispose();
            this._TrasitionLibrary = null;

            this._Timer?.Dispose();
            this._Timer = null;

            this._Manager.Dispose();
            this._Manager = null;
        }

        private Manager _Manager = null;
        private Timer _Timer = null;
        private TransitionLibrary _TrasitionLibrary = null;

        /// <summary>
        /// ストーリーボードのアニメーションが実行中であるかをbool型で返す
        /// </summary>
        /// <param name="ストーリーボード">対象のストーリーボード</param>
        /// <returns>アニメーション実行中であればtrue、実行中でなければfalse</returns>
        public static bool b進行中( Storyboard ストーリーボード )
        {
            if( ストーリーボード != null )
            {
                if( ストーリーボード.Status == StoryboardStatus.Playing ) return true;
                else return false;
            }
            else
            {
                return false;
            }
        }
    }
}
