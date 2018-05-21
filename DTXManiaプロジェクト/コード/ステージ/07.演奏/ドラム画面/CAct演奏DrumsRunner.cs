using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using FDK;

namespace DTXMania
{
    internal class CAct演奏DrumsRunner : CActivity
    {
        /// <summary>
        /// ランナー
        /// </summary>
        public CAct演奏DrumsRunner()
        {
            base.b活性化してない = true;
        }

        private int[] Size = new int[] { 124, 250 };
        private int Ptn = 24;
        private int Chara = 4;
        private int[] StartPoint = new int[] { 185, -50 };

        public void Start(int Player, bool IsMiss, CDTX.CChip pChip)
        {
            if(CDTXMania.Tx.Runner != null)
            {
                for (int i = 0; i < 128; i++)
                {
                    if (pChip.nチャンネル番号 < 0x15 || (pChip.nチャンネル番号 >= 0x1A))
                        {
                        if (!stRunners[i].b使用中)
                        {
                            stRunners[i].b使用中 = true;
                            stRunners[i].nPlayer = Player;
                            if (IsMiss == true)
                            {
                                stRunners[i].nType = 0;
                            }
                            else
                            {
                                stRunners[i].nType = random.Next(1, Chara + 1);
                            }
                            stRunners[i].ct進行 = new CCounter(0, 1280, 16, CDTXMania.Timer);
                            stRunners[i].ctPtn = new CCounter(0, Ptn, 90, CDTXMania.Timer);
                            stRunners[i].nOldValue = 0;
                            stRunners[i].fX = 0;
                            break;
                        }
                    }
                }
            }
        }

        public override void On活性化()
        {
            for (int i = 0; i < 128; i++)
            {
                stRunners[i] = new STRunner();
                stRunners[i].b使用中 = false;
                stRunners[i].ct進行 = new CCounter();
                stRunners[i].ctPtn = new CCounter();
            }
            base.On活性化();
        }

        public override void On非活性化()
        {
            for (int i = 0; i < 128; i++)
            {
                stRunners[i].ct進行 = null;
                stRunners[i].ctPtn = null;
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
            for (int i = 0; i < 128; i++)
            {
                if(stRunners[i].b使用中)
                {
                    stRunners[i].nOldValue = stRunners[i].ct進行.n現在の値;
                    stRunners[i].ct進行.t進行();
                    stRunners[i].ctPtn.t進行();
                    if (stRunners[i].ct進行.b終了値に達した || stRunners[i].fX > 1280)
                    {
                        stRunners[i].ct進行.t停止();
                        stRunners[i].b使用中 = false;
                    }
                    if (stRunners[i].nOldValue < stRunners[i].ct進行.n現在の値)
                    {
                        stRunners[i].fX += (float)CDTXMania.stage演奏ドラム画面.actPlayInfo.dbBPM / 20;
                    }
                    if(CDTXMania.Tx.Runner != null)
                    {
                        CDTXMania.Tx.Runner.t2D描画(CDTXMania.app.Device, (StartPoint[0] + stRunners[i].fX), StartPoint[1], new Rectangle(stRunners[i].ctPtn.n現在の値 * Size[0], stRunners[i].nType * Size[1], Size[0], Size[1]));
                    }
                }
            }
            return base.On進行描画();
        }

        #region[ private ]
        //-----------------
        [StructLayout(LayoutKind.Sequential)]
        private struct STRunner
        {
            public bool b使用中;
            public bool bMiss;
            public int nPlayer;
            public int nType;
            public int nOldValue;
            public float fX;
            public CCounter ct進行;
            public CCounter ctPtn;
        }
        private STRunner[] stRunners = new STRunner[128];
        Random random = new Random();
        //-----------------
        #endregion
    }
}
