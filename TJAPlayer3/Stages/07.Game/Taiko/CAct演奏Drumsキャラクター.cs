using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Diagnostics;
using FDK;


namespace TJAPlayer3
{
    //クラスの設置位置は必ず演奏画面共通に置くこと。
    //そうしなければBPM変化に対応できません。

    //完成している部分は以下のとおり。(画像完成+動作確認完了で完成とする)
    //_通常モーション
    //_ゴーゴータイムモーション
    //_クリア時モーション
    //
    internal class CAct演奏Drumsキャラクター : CActivity
    {
        public CAct演奏Drumsキャラクター()
        {

        }

        public override void On活性化()
        {
            ctChara_Normal = new CCounter();
            ctChara_GoGo = new CCounter();
            ctChara_Clear = new CCounter();

            this.ctキャラクターアクション_10コンボ = new CCounter();
            this.ctキャラクターアクション_10コンボMAX = new CCounter();
            this.ctキャラクターアクション_ゴーゴースタート = new CCounter();
            this.ctキャラクターアクション_ゴーゴースタートMAX = new CCounter();
            this.ctキャラクターアクション_ノルマ = new CCounter();
            this.ctキャラクターアクション_魂MAX = new CCounter();

            CharaAction_Balloon_Breaking = new CCounter();
            CharaAction_Balloon_Broke = new CCounter();
            CharaAction_Balloon_Miss = new CCounter();
            CharaAction_Balloon_Delay = new CCounter();

            this.b風船連打中 = false;
            this.b演奏中 = false;


            CharaAction_Balloon_FadeOut = new Animations.FadeOut(TJAPlayer3.Skin.Game_Chara_Balloon_FadeOut);
            // ふうせん系アニメーションの総再生時間は画像枚数 x Tick間隔なので、
            // フェードアウトの開始タイミングは、総再生時間 - フェードアウト時間。
            var tick = TJAPlayer3.Skin.Game_Chara_Balloon_Timer;
            var balloonBrokePtn = TJAPlayer3.Skin.Game_Chara_Ptn_Balloon_Broke;
            var balloonMissPtn = TJAPlayer3.Skin.Game_Chara_Ptn_Balloon_Miss;
            CharaAction_Balloon_FadeOut_StartMs[0] = (balloonBrokePtn * tick) - TJAPlayer3.Skin.Game_Chara_Balloon_FadeOut;
            CharaAction_Balloon_FadeOut_StartMs[1] = (balloonMissPtn * tick) - TJAPlayer3.Skin.Game_Chara_Balloon_FadeOut;
            if (balloonBrokePtn > 1) CharaAction_Balloon_FadeOut_StartMs[0] /= balloonBrokePtn - 1;
            if (balloonMissPtn > 1) CharaAction_Balloon_FadeOut_StartMs[1] /= balloonMissPtn - 1; // - 1はタイマー用
            this.bマイどんアクション中 = false;

            base.On活性化();
        }

        public override void On非活性化()
        {
            ctChara_Normal = null;
            ctChara_GoGo = null;
            ctChara_Clear = null;
            this.ctキャラクターアクション_10コンボ = null;
            this.ctキャラクターアクション_10コンボMAX = null;
            this.ctキャラクターアクション_ゴーゴースタート = null;
            this.ctキャラクターアクション_ゴーゴースタートMAX = null;
            this.ctキャラクターアクション_ノルマ = null;
            this.ctキャラクターアクション_魂MAX = null;

            CharaAction_Balloon_Breaking = null;
            CharaAction_Balloon_Broke = null;
            CharaAction_Balloon_Miss = null;
            CharaAction_Balloon_Delay = null;

            CharaAction_Balloon_FadeOut = null;
       
            base.On非活性化();
        }

        public override void OnManagedリソースの作成()
        {
            this.arモーション番号 = C変換.ar配列形式のstringをint配列に変換して返す( TJAPlayer3.Skin.Game_Chara_Motion_Normal);
            this.arゴーゴーモーション番号 = C変換.ar配列形式のstringをint配列に変換して返す(TJAPlayer3.Skin.Game_Chara_Motion_GoGo);
            this.arクリアモーション番号 = C変換.ar配列形式のstringをint配列に変換して返す(TJAPlayer3.Skin.Game_Chara_Motion_Clear);
            if (arモーション番号 == null) this.arモーション番号 = C変換.ar配列形式のstringをint配列に変換して返す("0,0");
            if (arゴーゴーモーション番号 == null) this.arゴーゴーモーション番号 = C変換.ar配列形式のstringをint配列に変換して返す("0,0");
            if (arクリアモーション番号 == null) this.arクリアモーション番号 = C変換.ar配列形式のstringをint配列に変換して返す("0,0");
            ctChara_Normal = new CCounter(0, arモーション番号.Length - 1, 10, CSound管理.rc演奏用タイマ);
            ctChara_GoGo = new CCounter(0, arゴーゴーモーション番号.Length - 1, 10, CSound管理.rc演奏用タイマ);
            ctChara_Clear = new CCounter(0, arクリアモーション番号.Length - 1, 10, CSound管理.rc演奏用タイマ);
            if (CharaAction_Balloon_Delay != null) CharaAction_Balloon_Delay.n現在の値 = CharaAction_Balloon_Delay.n終了値;
            base.OnManagedリソースの作成();
        }

        public override void OnManagedリソースの解放()
        {
            base.OnManagedリソースの解放();
        }

        public override int On進行描画()
        {
            if (ctChara_Normal != null || TJAPlayer3.Skin.Game_Chara_Ptn_Normal != 0) ctChara_Normal.t進行LoopDb();
            if (ctChara_GoGo != null || TJAPlayer3.Skin.Game_Chara_Ptn_GoGo != 0) ctChara_GoGo.t進行LoopDb();
            if (ctChara_Clear != null || TJAPlayer3.Skin.Game_Chara_Ptn_Clear != 0) ctChara_Clear.t進行LoopDb();
            if (this.ctキャラクターアクション_10コンボ != null || TJAPlayer3.Skin.Game_Chara_Ptn_10combo != 0) this.ctキャラクターアクション_10コンボ.t進行db();
            if (this.ctキャラクターアクション_10コンボMAX != null || TJAPlayer3.Skin.Game_Chara_Ptn_10combo_Max != 0) this.ctキャラクターアクション_10コンボMAX.t進行db();
            if (this.ctキャラクターアクション_ゴーゴースタート != null || TJAPlayer3.Skin.Game_Chara_Ptn_GoGoStart != 0) this.ctキャラクターアクション_ゴーゴースタート.t進行db();
            if (this.ctキャラクターアクション_ゴーゴースタートMAX != null || TJAPlayer3.Skin.Game_Chara_Ptn_GoGoStart_Max != 0) this.ctキャラクターアクション_ゴーゴースタートMAX.t進行db();
            if (this.ctキャラクターアクション_ノルマ != null || TJAPlayer3.Skin.Game_Chara_Ptn_ClearIn != 0) this.ctキャラクターアクション_ノルマ.t進行db();
            if (this.ctキャラクターアクション_魂MAX != null || TJAPlayer3.Skin.Game_Chara_Ptn_SoulIn != 0) this.ctキャラクターアクション_魂MAX.t進行db();


            if ( this.b風船連打中 != true && this.bマイどんアクション中 != true && CharaAction_Balloon_Delay.b終了値に達した)
            {
                if ( !TJAPlayer3.stage演奏ドラム画面.bIsGOGOTIME[ 0 ] )
                {
                    if( TJAPlayer3.stage演奏ドラム画面.actGauge.db現在のゲージ値[ 0 ] >= 100.0 && TJAPlayer3.Skin.Game_Chara_Ptn_Clear != 0 )
                    {
                        if(TJAPlayer3.Skin.Game_Chara_Ptn_Clear != 0)
                            TJAPlayer3.Tx.Chara_Normal_Maxed[ this.arクリアモーション番号[(int)this.ctChara_Clear.db現在の値] ].t2D描画( TJAPlayer3.app.Device, TJAPlayer3.Skin.Game_Chara_X[0], TJAPlayer3.Skin.Game_Chara_Y[0] );
                    }
                    else if( TJAPlayer3.stage演奏ドラム画面.actGauge.db現在のゲージ値[ 0 ] >= 80.0 && TJAPlayer3.Skin.Game_Chara_Ptn_Clear != 0 )
                    {
                        if(TJAPlayer3.Skin.Game_Chara_Ptn_Clear != 0)
                        {
                            TJAPlayer3.Tx.Chara_Normal_Cleared[ this.arクリアモーション番号[ (int)this.ctChara_Clear.db現在の値 ] ].t2D描画( TJAPlayer3.app.Device, TJAPlayer3.Skin.Game_Chara_X[0], TJAPlayer3.Skin.Game_Chara_Y[0] );
                        }
                    }
                    else
                    {
                        if (TJAPlayer3.Skin.Game_Chara_Ptn_Normal != 0)
                        {
                            TJAPlayer3.Tx.Chara_Normal[ this.arモーション番号[ (int)this.ctChara_Normal.db現在の値 ] ].t2D描画( TJAPlayer3.app.Device, TJAPlayer3.Skin.Game_Chara_X[0], TJAPlayer3.Skin.Game_Chara_Y[0] );
                        }
                    }
                }
                else
                {
                    if( TJAPlayer3.stage演奏ドラム画面.actGauge.db現在のゲージ値[ 0 ] >= 100.0 && TJAPlayer3.Skin.Game_Chara_Ptn_GoGo != 0 )
                    {
                        if(TJAPlayer3.Skin.Game_Chara_Ptn_GoGo != 0)
                            TJAPlayer3.Tx.Chara_GoGoTime_Maxed[this.arゴーゴーモーション番号[(int)this.ctChara_GoGo.db現在の値] ].t2D描画( TJAPlayer3.app.Device, TJAPlayer3.Skin.Game_Chara_X[0], TJAPlayer3.Skin.Game_Chara_Y[0] );
                    }
                    else
                    {
                        if(TJAPlayer3.Skin.Game_Chara_Ptn_GoGo != 0)
                            TJAPlayer3.Tx.Chara_GoGoTime[ this.arゴーゴーモーション番号[ (int)this.ctChara_GoGo.db現在の値 ] ].t2D描画( TJAPlayer3.app.Device, TJAPlayer3.Skin.Game_Chara_X[0], TJAPlayer3.Skin.Game_Chara_Y[0] );
                    }
                }
            }

            if (this.b風船連打中 != true && bマイどんアクション中 == true && CharaAction_Balloon_Delay.b終了値に達した)
            {

                if (this.ctキャラクターアクション_10コンボ.b進行中db)
                {
                    if(TJAPlayer3.Tx.Chara_10Combo[0] != null && TJAPlayer3.Skin.Game_Chara_Ptn_10combo != 0)
                    {
                        TJAPlayer3.Tx.Chara_10Combo[(int)this.ctキャラクターアクション_10コンボ.db現在の値].t2D描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.Game_Chara_X[0], TJAPlayer3.Skin.Game_Chara_Y[0] );
                    }
                    if (this.ctキャラクターアクション_10コンボ.b終了値に達したdb)
                    {
                        this.bマイどんアクション中 = false;
                        this.ctキャラクターアクション_10コンボ.t停止();
                        this.ctキャラクターアクション_10コンボ.db現在の値 = 0D;
                    }
                }
                

                if (this.ctキャラクターアクション_10コンボMAX.b進行中db)
                {
                    if (TJAPlayer3.Tx.Chara_10Combo_Maxed[0] != null && TJAPlayer3.Skin.Game_Chara_Ptn_10combo_Max != 0)
                    {
                        TJAPlayer3.Tx.Chara_10Combo_Maxed[(int)this.ctキャラクターアクション_10コンボMAX.db現在の値].t2D描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.Game_Chara_X[0], TJAPlayer3.Skin.Game_Chara_Y[0]);
                    }
                    if (this.ctキャラクターアクション_10コンボMAX.b終了値に達したdb)
                    {
                        this.bマイどんアクション中 = false;
                        this.ctキャラクターアクション_10コンボMAX.t停止();
                        this.ctキャラクターアクション_10コンボMAX.db現在の値 = 0D;
                    }

                }

                if (this.ctキャラクターアクション_ゴーゴースタート.b進行中db)
                {
                    if (TJAPlayer3.Tx.Chara_GoGoStart[0] != null && TJAPlayer3.Skin.Game_Chara_Ptn_GoGoStart != 0)
                    {
                        TJAPlayer3.Tx.Chara_GoGoStart[(int)this.ctキャラクターアクション_ゴーゴースタート.db現在の値].t2D描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.Game_Chara_X[0], TJAPlayer3.Skin.Game_Chara_Y[0]);
                    }
                    if (this.ctキャラクターアクション_ゴーゴースタート.b終了値に達したdb)
                    {
                        this.bマイどんアクション中 = false;
                        this.ctキャラクターアクション_ゴーゴースタート.t停止();
                        this.ctキャラクターアクション_ゴーゴースタート.db現在の値 = 0D;
                        this.ctChara_GoGo.db現在の値 = TJAPlayer3.Skin.Game_Chara_Ptn_GoGo / 2;
                    }
                }

                if (this.ctキャラクターアクション_ゴーゴースタートMAX.b進行中db)
                {
                    if (TJAPlayer3.Tx.Chara_GoGoStart_Maxed[0] != null && TJAPlayer3.Skin.Game_Chara_Ptn_GoGoStart_Max != 0)
                    {
                        TJAPlayer3.Tx.Chara_GoGoStart_Maxed[(int)this.ctキャラクターアクション_ゴーゴースタートMAX.db現在の値].t2D描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.Game_Chara_X[0], TJAPlayer3.Skin.Game_Chara_Y[0]);
                    }
                    if (this.ctキャラクターアクション_ゴーゴースタートMAX.b終了値に達したdb)
                    {
                        this.bマイどんアクション中 = false;
                        this.ctキャラクターアクション_ゴーゴースタートMAX.t停止();
                        this.ctキャラクターアクション_ゴーゴースタートMAX.db現在の値 = 0D;
                        this.ctChara_GoGo.db現在の値 = TJAPlayer3.Skin.Game_Chara_Ptn_GoGo / 2;
                    }
                }

                if (this.ctキャラクターアクション_ノルマ.b進行中db)
                {
                    if (TJAPlayer3.Tx.Chara_Become_Cleared[0] != null && TJAPlayer3.Skin.Game_Chara_Ptn_ClearIn != 0)
                    {
                        TJAPlayer3.Tx.Chara_Become_Cleared[(int)this.ctキャラクターアクション_ノルマ.db現在の値].t2D描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.Game_Chara_X[0], TJAPlayer3.Skin.Game_Chara_Y[0]);
                    }
                    if (this.ctキャラクターアクション_ノルマ.b終了値に達したdb)
                    {
                        this.bマイどんアクション中 = false;
                        this.ctキャラクターアクション_ノルマ.t停止();
                        this.ctキャラクターアクション_ノルマ.db現在の値 = 0D;
                    }
                }

                if (this.ctキャラクターアクション_魂MAX.b進行中db)
                {
                    if (TJAPlayer3.Tx.Chara_Become_Maxed[0] != null && TJAPlayer3.Skin.Game_Chara_Ptn_SoulIn != 0)
                    {
                        TJAPlayer3.Tx.Chara_Become_Maxed[(int)this.ctキャラクターアクション_魂MAX.db現在の値].t2D描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.Game_Chara_X[0], TJAPlayer3.Skin.Game_Chara_Y[0]);
                    }
                    if (this.ctキャラクターアクション_魂MAX.b終了値に達したdb)
                    {
                        this.bマイどんアクション中 = false;
                        this.ctキャラクターアクション_魂MAX.t停止();
                        this.ctキャラクターアクション_魂MAX.db現在の値 = 0D;
                    }
                }
            }
            if (this.b風船連打中 != true && CharaAction_Balloon_Delay.b終了値に達した)
            {
                TJAPlayer3.stage演奏ドラム画面.PuchiChara.On進行描画(TJAPlayer3.Skin.Game_PuchiChara_X[0], TJAPlayer3.Skin.Game_PuchiChara_Y[0], TJAPlayer3.stage演奏ドラム画面.bIsAlreadyMaxed[0]);
            }
            return base.On進行描画();
        }

        public void OnDraw_Balloon()
        {
            if (TJAPlayer3.Skin.Game_Chara_Ptn_Balloon_Breaking != 0) CharaAction_Balloon_Breaking?.t進行();
            if (TJAPlayer3.Skin.Game_Chara_Ptn_Balloon_Broke != 0) CharaAction_Balloon_Broke?.t進行();
            CharaAction_Balloon_Delay?.t進行();
            if (TJAPlayer3.Skin.Game_Chara_Ptn_Balloon_Miss != 0) CharaAction_Balloon_Miss?.t進行();
            CharaAction_Balloon_FadeOut.Tick();

            //CharaAction_Balloon_Delay?.t進行();
            //CDTXMania.act文字コンソール.tPrint(0, 0, C文字コンソール.Eフォント種別.白, CharaAction_Balloon_Broke?.b進行中.ToString());
            //CDTXMania.act文字コンソール.tPrint(0, 20, C文字コンソール.Eフォント種別.白, CharaAction_Balloon_Miss?.b進行中.ToString());
            //CDTXMania.act文字コンソール.tPrint(0, 40, C文字コンソール.Eフォント種別.白, CharaAction_Balloon_Breaking?.b進行中.ToString());

            if (bマイどんアクション中)
            {
                var nowOpacity = CharaAction_Balloon_FadeOut.Counter.b進行中 ? (int)CharaAction_Balloon_FadeOut.GetAnimation() : 255;
                if (CharaAction_Balloon_Broke?.b進行中 == true && TJAPlayer3.Skin.Game_Chara_Ptn_Balloon_Broke != 0)
                {
                    if (CharaAction_Balloon_FadeOut.Counter.b停止中 && CharaAction_Balloon_Broke.n現在の値 > CharaAction_Balloon_FadeOut_StartMs[0])
                    {
                        CharaAction_Balloon_FadeOut.Start();
                    }
                    if(TJAPlayer3.Tx.Chara_Balloon_Broke[CharaAction_Balloon_Broke.n現在の値] != null)
                    {
                        TJAPlayer3.Tx.Chara_Balloon_Broke[CharaAction_Balloon_Broke.n現在の値].Opacity = nowOpacity;
                        TJAPlayer3.Tx.Chara_Balloon_Broke[CharaAction_Balloon_Broke.n現在の値].t2D描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.Game_Chara_Balloon_X[0], TJAPlayer3.Skin.Game_Chara_Balloon_Y[0]);
                    }
                    TJAPlayer3.stage演奏ドラム画面.PuchiChara.On進行描画(TJAPlayer3.Skin.Game_PuchiChara_BalloonX[0], TJAPlayer3.Skin.Game_PuchiChara_BalloonY[0], false, nowOpacity, true);
                    if (CharaAction_Balloon_Broke.b終了値に達した)
                    {
                        CharaAction_Balloon_Broke.t停止();
                        CharaAction_Balloon_Broke.n現在の値 = 0;
                        bマイどんアクション中 = false;
                    }
                }
                else if (CharaAction_Balloon_Miss?.b進行中 == true && TJAPlayer3.Skin.Game_Chara_Ptn_Balloon_Miss != 0)
                {
                    if (CharaAction_Balloon_FadeOut.Counter.b停止中 && CharaAction_Balloon_Miss.n現在の値 > CharaAction_Balloon_FadeOut_StartMs[1])
                    {
                        CharaAction_Balloon_FadeOut.Start();
                    }
                    if(TJAPlayer3.Tx.Chara_Balloon_Miss[CharaAction_Balloon_Miss.n現在の値] != null)
                    {
                        TJAPlayer3.Tx.Chara_Balloon_Miss[CharaAction_Balloon_Miss.n現在の値].Opacity = nowOpacity;
                        TJAPlayer3.Tx.Chara_Balloon_Miss[CharaAction_Balloon_Miss.n現在の値].t2D描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.Game_Chara_Balloon_X[0], TJAPlayer3.Skin.Game_Chara_Balloon_Y[0]);
                    }
                    TJAPlayer3.stage演奏ドラム画面.PuchiChara.On進行描画(TJAPlayer3.Skin.Game_PuchiChara_BalloonX[0], TJAPlayer3.Skin.Game_PuchiChara_BalloonY[0], false, nowOpacity, true);
                    if (CharaAction_Balloon_Miss.b終了値に達した)
                    {
                        CharaAction_Balloon_Miss.t停止();
                        CharaAction_Balloon_Miss.n現在の値 = 0;
                        bマイどんアクション中 = false;
                    }
                }
                else if (CharaAction_Balloon_Breaking?.b進行中 == true && TJAPlayer3.Skin.Game_Chara_Ptn_Balloon_Breaking != 0)
                {
                    TJAPlayer3.Tx.Chara_Balloon_Breaking[CharaAction_Balloon_Breaking.n現在の値]?.t2D描画(TJAPlayer3.app.Device, TJAPlayer3.Skin.Game_Chara_Balloon_X[0], TJAPlayer3.Skin.Game_Chara_Balloon_Y[0]);
                    TJAPlayer3.stage演奏ドラム画面.PuchiChara.On進行描画(TJAPlayer3.Skin.Game_PuchiChara_BalloonX[0], TJAPlayer3.Skin.Game_PuchiChara_BalloonY[0], false, 255, true);
                }

                //if (CDTXMania.stage演奏ドラム画面.actChara.CharaAction_Balloon_Breaking?.b終了値に達した == true)
                //{
                //    CDTXMania.stage演奏ドラム画面.actChara.bマイどんアクション中 = false;
                //    CDTXMania.stage演奏ドラム画面.actChara.CharaAction_Balloon_Breaking.t停止();
                //    CDTXMania.stage演奏ドラム画面.actChara.CharaAction_Balloon_Breaking.n現在の値 = 0;
                //}

            }
        }

        public void アクションタイマーリセット()
        {
            ctキャラクターアクション_10コンボ.t停止();
            ctキャラクターアクション_10コンボMAX.t停止();
            ctキャラクターアクション_ゴーゴースタート.t停止();
            ctキャラクターアクション_ゴーゴースタートMAX.t停止();
            ctキャラクターアクション_ノルマ.t停止();
            ctキャラクターアクション_魂MAX.t停止();
            ctキャラクターアクション_10コンボ.db現在の値 = 0D;
            ctキャラクターアクション_10コンボMAX.db現在の値 = 0D;
            ctキャラクターアクション_ゴーゴースタート.db現在の値 = 0D;
            ctキャラクターアクション_ゴーゴースタートMAX.db現在の値 = 0D;
            ctキャラクターアクション_ノルマ.db現在の値 = 0D;
            ctキャラクターアクション_魂MAX.db現在の値 = 0D;
            CharaAction_Balloon_Breaking?.t停止();
            CharaAction_Balloon_Broke?.t停止();
            CharaAction_Balloon_Miss?.t停止();
            //CharaAction_Balloon_Delay?.t停止();
            CharaAction_Balloon_Breaking.n現在の値 = 0;
            CharaAction_Balloon_Broke.n現在の値 = 0;
            CharaAction_Balloon_Miss.n現在の値 = 0;
            //CharaAction_Balloon_Delay.n現在の値 = 0;
        }

        public int[] arモーション番号;
        public int[] arゴーゴーモーション番号;
        public int[] arクリアモーション番号;

        public CCounter ctキャラクターアクション_10コンボ;
        public CCounter ctキャラクターアクション_10コンボMAX;
        public CCounter ctキャラクターアクション_ゴーゴースタート;
        public CCounter ctキャラクターアクション_ゴーゴースタートMAX;
        public CCounter ctキャラクターアクション_ノルマ;
        public CCounter ctキャラクターアクション_魂MAX;
        public CCounter CharaAction_Balloon_Breaking;
        public CCounter CharaAction_Balloon_Broke;
        public CCounter CharaAction_Balloon_Miss;
        public CCounter CharaAction_Balloon_Delay;

        public CCounter ctChara_Normal;
        public CCounter ctChara_GoGo;
        public CCounter ctChara_Clear;

        public Animations.FadeOut CharaAction_Balloon_FadeOut;
        private readonly int[] CharaAction_Balloon_FadeOut_StartMs = new int[2];

        public bool bマイどんアクション中;

        public bool b風船連打中;
        public bool b演奏中;
    }
}
