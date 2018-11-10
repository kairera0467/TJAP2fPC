using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FDK;

namespace DTXMania
{
    class FastRender
    {
        public FastRender()
        {
            
        }

        public void Render()
        {
            for (int i = 0; i < CDTXMania.Skin.Game_Chara_Ptn_10combo; i++)
            {
                NullCheckAndRender(ref CDTXMania.Tx.Chara_10Combo[i]);
            }
            for (int i = 0; i < CDTXMania.Skin.Game_Chara_Ptn_10combo_Max; i++)
            {
                NullCheckAndRender(ref CDTXMania.Tx.Chara_10Combo_Maxed[i]);
            }
            for (int i = 0; i < CDTXMania.Skin.Game_Chara_Ptn_GoGoStart; i++)
            {
                NullCheckAndRender(ref CDTXMania.Tx.Chara_GoGoStart[i]);
            }
            for (int i = 0; i < CDTXMania.Skin.Game_Chara_Ptn_GoGoStart_Max; i++)
            {
                NullCheckAndRender(ref CDTXMania.Tx.Chara_GoGoStart_Maxed[i]);
            }
            for (int i = 0; i < CDTXMania.Skin.Game_Chara_Ptn_Normal; i++)
            {
                NullCheckAndRender(ref CDTXMania.Tx.Chara_Normal[i]);
            }
            for (int i = 0; i < CDTXMania.Skin.Game_Chara_Ptn_Clear; i++)
            {
                NullCheckAndRender(ref CDTXMania.Tx.Chara_Normal_Cleared[i]);
            }
            for (int i = 0; i < CDTXMania.Skin.Game_Chara_Ptn_ClearIn; i++)
            {
                NullCheckAndRender(ref CDTXMania.Tx.Chara_Become_Cleared[i]);
            }
            for (int i = 0; i < CDTXMania.Skin.Game_Chara_Ptn_SoulIn; i++)
            {
                NullCheckAndRender(ref CDTXMania.Tx.Chara_Become_Maxed[i]);
            }
            for (int i = 0; i < CDTXMania.Skin.Game_Chara_Ptn_Balloon_Breaking; i++)
            {
                NullCheckAndRender(ref CDTXMania.Tx.Chara_Balloon_Breaking[i]);
            }
            for (int i = 0; i < CDTXMania.Skin.Game_Chara_Ptn_Balloon_Broke; i++)
            {
                NullCheckAndRender(ref CDTXMania.Tx.Chara_Balloon_Broke[i]);
            }
            for (int i = 0; i < CDTXMania.Skin.Game_Chara_Ptn_Balloon_Miss; i++)
            {
                NullCheckAndRender(ref CDTXMania.Tx.Chara_Balloon_Miss[i]);
            }

            for (int i = 0; i < 5; i++)
            {
                for (int k = 0; k < CDTXMania.Skin.Game_Dancer_Ptn; k++)
                {
                    NullCheckAndRender(ref CDTXMania.Tx.Dancer[i][k]);
                }
            }

            NullCheckAndRender(ref CDTXMania.Tx.Effects_GoGoSplash);
            NullCheckAndRender(ref CDTXMania.Tx.Runner);
            for (int i = 0; i < CDTXMania.Skin.Game_Mob_Ptn; i++)
            {
                NullCheckAndRender(ref CDTXMania.Tx.Mob[i]);
            }
            
        }

        private void NullCheckAndRender(ref CTexture tx)
        {
            if (tx == null) return;
            tx.n透明度 = 0;
            tx.t2D描画(CDTXMania.app.Device, 0, 0);
            tx.n透明度 = 255;
        }
    }
}
