using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using SlimDX;
using FDK;

namespace DTXMania
{
	internal class FlyingNotes : CActivity
	{
		// コンストラクタ

		public FlyingNotes()
		{
            base.b活性化してない = true;
            // 角度の決定
            Console.WriteLine("##############################################");
            var height1P = Math.Abs(CDTXMania.Skin.Game_Effect_FlyingNotes_EndPoint_Y[0] - CDTXMania.Skin.Game_Effect_FlyingNotes_StartPoint_Y[0]);
            var width1P = Math.Abs((CDTXMania.Skin.Game_Effect_FlyingNotes_EndPoint_X[0] - CDTXMania.Skin.Game_Effect_FlyingNotes_StartPoint_X[0])) / 2;
            //Console.WriteLine("{0}, {1}", width1P, height1P );
            var height2P = Math.Abs(CDTXMania.Skin.Game_Effect_FlyingNotes_EndPoint_Y[1] - CDTXMania.Skin.Game_Effect_FlyingNotes_StartPoint_Y[1]);
            var width2P = Math.Abs((CDTXMania.Skin.Game_Effect_FlyingNotes_EndPoint_X[1] - CDTXMania.Skin.Game_Effect_FlyingNotes_StartPoint_X[1])) / 2;
            //Console.WriteLine("{0}, {1}", width2P, height2P);
            Theta[0] = (int)((Math.Atan2(height1P , width1P) * 180.0) / Math.PI);
            Theta[1] = (int)((Math.Atan2(height2P , width2P) * 180.0) / Math.PI);
           
            Console.WriteLine("{0}, {1}", Theta[0], Theta[1]);
        }
		
		
		// メソッド
        public virtual void Start( int nLane, int nPlayer )
		{
            if (CDTXMania.Tx.Notes != null)
            {
                for (int i = 0; i < 128; i++)
                {
                    if(!Flying[i].IsUsing)
                    {
                        // 初期化
                        Flying[i].IsUsing = true;
                        Flying[i].Lane = nLane;
                        Flying[i].Player = nPlayer;
                        Flying[i].X = CDTXMania.Skin.Game_Effect_FlyingNotes_StartPoint_X[nPlayer];
                        Flying[i].Y = CDTXMania.Skin.Game_Effect_FlyingNotes_StartPoint_Y[nPlayer];
                        Flying[i].OldValue = 0;
                        Flying[i].Counter = new CCounter(0, 180 - Theta[nPlayer], CDTXMania.Skin.Game_Effect_FlyingNotes_Timer, CDTXMania.Timer);
                    }
                }
            }
        }

		// CActivity 実装

		public override void On活性化()
		{
            for (int i = 0; i < 128; i++)
            {
                Flying[i] = new Status();
                Flying[i].IsUsing = false;
                Flying[i].Counter = new CCounter();
            }
            base.On活性化();
		}
		public override void On非活性化()
		{
            for (int i = 0; i < 128; i++)
            {
                Flying[i].Counter = null;
            }
			base.On非活性化();
		}
		public override void OnManagedリソースの作成()
		{
			if( !base.b活性化してない )
			{
				base.OnManagedリソースの作成();
			}
		}
		public override void OnManagedリソースの解放()
		{
			if( !base.b活性化してない )
			{
				base.OnManagedリソースの解放();
			}
		}
		public override int On進行描画()
		{
			if( !base.b活性化してない )
			{
                for (int i = 0; i < 128; i++)
                {
                    //if( CDTXMania.Skin.nScrollFieldX[0] > 414 + 4 )
                    //    break;
                    //if( CDTXMania.Skin.nScrollFieldX[0] < 414 - 4 )
                    //    break;

                    if (Flying[i].IsUsing)
                    {
                        Flying[i].OldValue = Flying[i].Counter.n現在の値;
                        Flying[i].Counter.t進行();
                        if (Flying[i].Counter.b終了値に達した)
                        {
                            Flying[i].Counter.t停止();
                            Flying[i].IsUsing = false;
                            CDTXMania.stage演奏ドラム画面.actGauge.Start(Flying[i].Lane, E判定.Perfect, Flying[i].
                                Player);
                            CDTXMania.stage演奏ドラム画面.actChipEffects.Start(Flying[i].Player, Flying[i].Lane);
                        }
                        for (int n = Flying[i].OldValue; n < Flying[i].Counter.n現在の値; n++)
                        {
                            if( Flying[i].Player == 0 )
                            {
                                // 
                            }
                        }

                        if (CDTXMania.Tx.Notes != null)
                        {
                        }
                    }
                }
			}
			return 0;
		}
		

		#region [ private ]
		//-----------------

        [StructLayout(LayoutKind.Sequential)]
        private struct Status
        {
            public int Lane;
            public int Player;
            public bool IsUsing;
            public CCounter Counter;
            public int OldValue;
            public float X;
            public float Y;
        }

        private Status[] Flying = new Status[128];
        private int[] Theta = new int[2];

        //-----------------
        #endregion
    }
}
