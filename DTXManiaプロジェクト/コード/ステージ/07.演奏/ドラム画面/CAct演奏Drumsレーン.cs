using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using FDK;

namespace DTXMania
{
    internal class CAct演奏Drumsレーン : CActivity
    {
        public CAct演奏Drumsレーン()
        {
            base.b活性化してない = true;
        }

        public override void On活性化()
        {
            base.On活性化();
        }

        public override void On非活性化()
        {
            CDTXMania.t安全にDisposeする( ref this.ct分岐アニメ進行 );
            base.On非活性化();
        }

        public override void OnManagedリソースの作成()
        {
            this.tx普通譜面[0] = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\7_field_normal_base.png"));
            this.tx玄人譜面[0] = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\7_field_expert_base.png"));
            this.tx達人譜面[0] = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\7_field_master_base.png"));
            this.tx普通譜面[1] = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\7_field_normal.png"));
            this.tx玄人譜面[1] = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\7_field_expert.png"));
            this.tx達人譜面[1] = CDTXMania.tテクスチャの生成(CSkin.Path(@"Graphics\7_field_master.png"));
            this.ct分岐アニメ進行 = new CCounter();
            this.tx普通譜面[0].n透明度 = 255;

            base.OnManagedリソースの作成();
        }

        public override void OnManagedリソースの解放()
        {
            CDTXMania.tテクスチャの解放(ref this.tx普通譜面[0]);
            CDTXMania.tテクスチャの解放(ref this.tx玄人譜面[0]);
            CDTXMania.tテクスチャの解放(ref this.tx達人譜面[0]);
            CDTXMania.tテクスチャの解放(ref this.tx普通譜面[1]);
            CDTXMania.tテクスチャの解放(ref this.tx玄人譜面[1]);
            CDTXMania.tテクスチャの解放(ref this.tx達人譜面[1]);

            base.OnManagedリソースの解放();
        }

        public override int On進行描画()
        {
            if( !this.ct分岐アニメ進行.b停止中 )
            {
                this.ct分岐アニメ進行.t進行();
                if( this.ct分岐アニメ進行.b終了値に達した )
                {
                    this.bState = false;
                    this.ct分岐アニメ進行.t停止();
                }
            }

            //if( this.bState == true )
            //{
            //    if( this.ct分岐アニメ進行.b進行中 == false )
            //    {
            //        this.ct分岐アニメ進行.n現在の値 = 0;
            //    }
            //        this.ct分岐アニメ進行.t進行();
            //    if( this.ct分岐アニメ進行.b終了値に達した )
            //    {
            //        this.bState = false;
            //    }
            //}



            if( CDTXMania.stage演奏ドラム画面.bUseBranch == true )
            {
                //switch (CDTXMania.stage演奏ドラム画面.n次回のコース)
                //{
                //    case 0:
                //        this.tx普通譜面[0].t2D描画(CDTXMania.app.Device, 333, 192);
                //        break;
                //    case 1:
                //        this.tx玄人譜面[0].t2D描画(CDTXMania.app.Device, 333, 192);
                //        break;
                //    case 2:
                //        this.tx達人譜面[0].t2D描画(CDTXMania.app.Device, 333, 192);
                //        break;
                //}


                if (this.ct分岐アニメ進行.b進行中)
                {

                    #region[ 普通譜面・レベルアップ ]
                    //普通→玄人
                    if (n1 == 0 && n2 == 1)
                    {
                        this.tx普通譜面[0].t2D描画(CDTXMania.app.Device, 333, 192);
                        this.tx玄人譜面[0].t2D描画(CDTXMania.app.Device, 333, 192);
                        this.tx玄人譜面[0].n透明度 = this.ct分岐アニメ進行.n現在の値 > 100 ? 255 : ( ( ( this.ct分岐アニメ進行.n現在の値 * 0xff ) / 100 ) );
                    }
                    //普通→達人
                    if (n1 == 0 && n2 == 2)
                    {
                        this.tx普通譜面[0].t2D描画(CDTXMania.app.Device, 333, 192);
                        if( this.ct分岐アニメ進行.n現在の値 < 100 )
                        {
                            this.n透明度 = this.ct分岐アニメ進行.n現在の値 > 100 ? 255 : ( ( ( this.ct分岐アニメ進行.n現在の値 * 0xff ) / 100 ) );
                            this.tx玄人譜面[0].t2D描画(CDTXMania.app.Device, 333, 192);
                            this.tx玄人譜面[0].n透明度 = this.n透明度;
                        }
                        else if( this.ct分岐アニメ進行.n現在の値 >= 100 && this.ct分岐アニメ進行.n現在の値 < 150 )
                        {
                            this.tx玄人譜面[0].t2D描画(CDTXMania.app.Device, 333, 192);
                            this.tx玄人譜面[0].n透明度 = 255;
                        }
                        else if( this.ct分岐アニメ進行.n現在の値 >= 150 )
                        {
                            this.n透明度 = this.ct分岐アニメ進行.n現在の値 > 250 ? 255 : ( ( ( (this.ct分岐アニメ進行.n現在の値 - 150) * 0xff ) / 100 ) );
                            this.tx玄人譜面[0].t2D描画(CDTXMania.app.Device, 333, 192);
                            this.tx達人譜面[0].t2D描画(CDTXMania.app.Device, 333, 192);
                            this.tx達人譜面[0].n透明度 = this.n透明度;
                        }
                    }
                    #endregion
                    #region[ 玄人譜面・レベルアップ ]
                    if (n1 == 1 && n2 == 2)
                    {
                        this.tx玄人譜面[0].t2D描画(CDTXMania.app.Device, 333, 192);
                        this.tx達人譜面[0].t2D描画(CDTXMania.app.Device, 333, 192);
                        this.tx達人譜面[0].n透明度 = this.ct分岐アニメ進行.n現在の値 > 100 ? 255 : ( ( ( this.ct分岐アニメ進行.n現在の値 * 0xff ) / 100 ) );
                    }
                    #endregion

                    #region[ 玄人譜面・レベルダウン ]
                    if (n1 == 1 && n2 == 0)
                    {
                        this.tx玄人譜面[0].t2D描画(CDTXMania.app.Device, 333, 192);
                        this.tx普通譜面[0].t2D描画(CDTXMania.app.Device, 333, 192);
                        this.tx普通譜面[0].n透明度 = this.ct分岐アニメ進行.n現在の値 > 100 ? 255 : ( ( ( this.ct分岐アニメ進行.n現在の値 * 0xff ) / 100 ) );
                    }
                    #endregion
                    #region[ 達人譜面・レベルダウン ]
                    if (n1 == 2 && n2 == 0)
                    {
                        this.tx達人譜面[0].t2D描画(CDTXMania.app.Device, 333, 192);
                        if( this.ct分岐アニメ進行.n現在の値 < 100 )
                        {
                            this.n透明度 = this.ct分岐アニメ進行.n現在の値 > 100 ? 255 : ( ( ( this.ct分岐アニメ進行.n現在の値 * 0xff ) / 100 ) );
                            this.tx玄人譜面[0].t2D描画(CDTXMania.app.Device, 333, 192);
                            this.tx玄人譜面[0].n透明度 = this.n透明度;
                        }
                        else if( this.ct分岐アニメ進行.n現在の値 >= 100 && this.ct分岐アニメ進行.n現在の値 < 150 )
                        {
                            this.tx玄人譜面[0].t2D描画(CDTXMania.app.Device, 333, 192);
                            this.tx玄人譜面[0].n透明度 = 255;
                        }
                        else if( this.ct分岐アニメ進行.n現在の値 >= 150 )
                        {
                            this.n透明度 = this.ct分岐アニメ進行.n現在の値 > 250 ? 255 : ( ( ( (this.ct分岐アニメ進行.n現在の値 - 150) * 0xff ) / 100 ) );
                            this.tx玄人譜面[0].t2D描画(CDTXMania.app.Device, 333, 192);
                            this.tx普通譜面[0].t2D描画(CDTXMania.app.Device, 333, 192);
                            this.tx普通譜面[0].n透明度 = this.n透明度;
                        }
                    }
                    if( n1 == 2 && n2 == 1 )
                    {
                        this.tx達人譜面[0].t2D描画(CDTXMania.app.Device, 333, 192);
                        this.tx玄人譜面[0].t2D描画(CDTXMania.app.Device, 333, 192);
                        this.tx玄人譜面[0].n透明度 = this.ct分岐アニメ進行.n現在の値 > 100 ? 255 : ( ( ( this.ct分岐アニメ進行.n現在の値 * 0xff ) / 100 ) );
                    }
                    #endregion
                }

                    //if( !this.ct分岐アニメ進行.b進行中 )
                    //{

                    //    switch (CDTXMania.stage演奏ドラム画面.n次回のコース)
                    //    {
                    //        case 0:
                    //            this.tx普通譜面[1].t2D描画(CDTXMania.app.Device, 333, 192);
                    //            break;
                    //        case 1:
                    //            this.tx玄人譜面[1].t2D描画(CDTXMania.app.Device, 333, 192);
                    //            break;
                    //        case 2:
                    //            this.tx達人譜面[1].t2D描画(CDTXMania.app.Device, 333, 192);
                    //            break;
                    //    }
                    //}

                    

                //if( this.nY != 0 )
                    
                    //if (this.ct分岐アニメ進行.b進行中)
                    //{

                    //    #region[ 普通譜面・レベルアップ ]
                    //    //普通→玄人
                    //    if (n1 == 0 && n2 == 1)
                    //    {
                    //        this.tx普通譜面[1].n透明度 = 255;
                    //        this.tx玄人譜面[1].n透明度 = 255;
                    //        this.tx達人譜面[1].n透明度 = 255;

                    //        this.tx普通譜面[1].n透明度 = this.ct分岐アニメ進行.n現在の値 > 100 ? 0 : ( 255 - ( ( this.ct分岐アニメ進行.n現在の値 * 0xff ) / 60 ) );
                    //        //this.tx玄人譜面[1].n透明度 = this.ct分岐アニメ進行.n現在の値 > 100 ? 255 : ( ( ( this.ct分岐アニメ進行.n現在の値 * 0xff ) / 60 ) );
                    //        if (this.ct分岐アニメ進行.n現在の値 < 60)
                    //        {
                    //            this.nY = this.ct分岐アニメ進行.n現在の値 / 2;
                    //            this.tx普通譜面[1].t2D描画(CDTXMania.app.Device, 333, 192 + nY);
                    //        }

                    //        this.tx玄人譜面[1].t2D描画(CDTXMania.app.Device, 333, 162 + nY);
                    //    }
                    //    //普通→達人
                    //    if (n1 == 0 && n2 == 2)
                    //    {
                    //        this.tx普通譜面[1].n透明度 = 255;
                    //        this.tx玄人譜面[1].n透明度 = 255;
                    //        this.tx達人譜面[1].n透明度 = 255;

                    //        if (this.ct分岐アニメ進行.n現在の値 < 60)
                    //        {
                    //            this.nY = this.ct分岐アニメ進行.n現在の値 / 2;
                    //            this.tx普通譜面[1].t2D描画(CDTXMania.app.Device, 333, 192 + nY);
                    //            this.tx普通譜面[1].n透明度 = this.ct分岐アニメ進行.n現在の値 > 100 ? 0 : ( 255 - ( ( this.ct分岐アニメ進行.n現在の値 * 0xff ) / 100 ) );
                    //            this.tx玄人譜面[1].t2D描画(CDTXMania.app.Device, 333, 172 + nY);
                    //        }
                    //        else if( this.ct分岐アニメ進行.n現在の値 >= 60 && this.ct分岐アニメ進行.n現在の値 < 150 )
                    //        {
                    //            this.nY = 21;
                    //            this.tx玄人譜面[1].t2D描画(CDTXMania.app.Device, 333, 192);
                    //            this.tx玄人譜面[1].n透明度 = 255;
                    //            this.tx達人譜面[1].n透明度 = 255;
                    //        }
                    //        else if( this.ct分岐アニメ進行.n現在の値 >= 150 && this.ct分岐アニメ進行.n現在の値 < 210 )
                    //        {
                    //            this.nY = ((this.ct分岐アニメ進行.n現在の値 - 150) / 2);
                    //            this.tx玄人譜面[1].t2D描画(CDTXMania.app.Device, 333, 192 + nY);
                    //            this.tx玄人譜面[1].n透明度 = this.ct分岐アニメ進行.n現在の値 > 100 ? 0 : ( 255 - ( ( this.ct分岐アニメ進行.n現在の値 * 0xff ) / 100 ) );
                    //            this.tx達人譜面[1].t2D描画(CDTXMania.app.Device, 333, 172 + nY);
                    //        }
                    //        else
                    //        {
                    //            this.tx達人譜面[1].n透明度 = 255;
                    //            this.tx達人譜面[1].t2D描画(CDTXMania.app.Device, 333, 192);
                    //        }


                    //        //this.tx普通譜面[1].t2D描画(CDTXMania.app.Device, 333, 192 + nY);
                    //        //this.tx達人譜面[1].t2D描画(CDTXMania.app.Device, 333, 172 + nY);

                    //    }
                    //    #endregion
                    //    #region[ 玄人譜面・レベルアップ ]
                    //    //玄人→達人
                    //    if (n1 == 1 && n2 == 2)
                    //    {
                    //        this.tx普通譜面[1].n透明度 = 255;
                    //        this.tx玄人譜面[1].n透明度 = 255;
                    //        this.tx達人譜面[1].n透明度 = 255;

                    //        this.tx玄人譜面[1].n透明度 = this.ct分岐アニメ進行.n現在の値 > 100 ? 0 : (255 - ((this.ct分岐アニメ進行.n現在の値 * 0xff) / 60));
                    //        if (this.ct分岐アニメ進行.n現在の値 < 60)
                    //        {
                    //            this.nY = this.ct分岐アニメ進行.n現在の値 / 2;
                    //            this.tx玄人譜面[1].t2D描画(CDTXMania.app.Device, 333, 192 + nY);
                    //        }

                    //        this.tx達人譜面[1].t2D描画(CDTXMania.app.Device, 333, 162 + nY);
                    //    }
                    //    #endregion
                    //    #region[ 玄人譜面・レベルダウン ]
                    //    if (n1 == 1 && n2 == 0)
                    //    {
                    //        this.tx普通譜面[1].n透明度 = 255;
                    //        this.tx玄人譜面[1].n透明度 = 255;
                    //        this.tx達人譜面[1].n透明度 = 255;

                    //        this.tx玄人譜面[1].n透明度 = this.ct分岐アニメ進行.n現在の値 > 100 ? 0 : (255 - ((this.ct分岐アニメ進行.n現在の値 * 0xff) / 60));
                    //        if (this.ct分岐アニメ進行.n現在の値 < 60)
                    //        {
                    //            this.nY = this.ct分岐アニメ進行.n現在の値 / 2;
                    //            this.tx玄人譜面[1].t2D描画(CDTXMania.app.Device, 333, 192 - nY);
                    //        }

                    //        this.tx普通譜面[1].t2D描画(CDTXMania.app.Device, 333, 222 - nY);  
                    //    }
                    //    #endregion
                    //    #region[ 達人譜面・レベルダウン ]
                    //    if (n1 == 2 && n2 == 0)
                    //    {
                    //        this.tx普通譜面[1].n透明度 = 255;
                    //        this.tx玄人譜面[1].n透明度 = 255;
                    //        this.tx達人譜面[1].n透明度 = 255;

                    //        if (this.ct分岐アニメ進行.n現在の値 < 60)
                    //        {
                    //            this.nY = this.ct分岐アニメ進行.n現在の値 / 2;
                    //            this.tx達人譜面[1].n透明度 = this.ct分岐アニメ進行.n現在の値 > 100 ? 0 : (255 - ((this.ct分岐アニメ進行.n現在の値 * 0xff) / 60));
                    //            this.tx達人譜面[1].t2D描画(CDTXMania.app.Device, 333, 192 - nY);
                    //            this.tx玄人譜面[1].t2D描画(CDTXMania.app.Device, 333, 222 - nY);
                    //        }
                    //        else if (this.ct分岐アニメ進行.n現在の値 >= 60 && this.ct分岐アニメ進行.n現在の値 < 150)
                    //        {
                    //            this.nY = 21;
                    //            this.tx玄人譜面[1].t2D描画(CDTXMania.app.Device, 333, 192);
                    //            this.tx玄人譜面[1].n透明度 = 255;
                    //            this.tx達人譜面[1].n透明度 = 255;
                    //        }
                    //        else if (this.ct分岐アニメ進行.n現在の値 >= 150 && this.ct分岐アニメ進行.n現在の値 < 210)
                    //        {
                    //            this.nY = ((this.ct分岐アニメ進行.n現在の値 - 150) / 2);
                    //            this.tx玄人譜面[1].t2D描画(CDTXMania.app.Device, 333, 192 - nY);
                    //            this.tx玄人譜面[1].n透明度 = this.ct分岐アニメ進行.n現在の値 > 100 ? 0 : (255 - ((this.ct分岐アニメ進行.n現在の値 * 0xff) / 100));
                    //            this.tx普通譜面[1].t2D描画(CDTXMania.app.Device, 333, 222 - nY);
                    //        }
                    //        else
                    //        {
                    //            this.tx普通譜面[1].n透明度 = 255;
                    //            this.tx普通譜面[1].t2D描画(CDTXMania.app.Device, 333, 192);
                    //        }
                    //    }
                    //    if (n1 == 2 && n2 == 1)
                    //    {
                    //        this.tx普通譜面[1].n透明度 = 255;
                    //        this.tx玄人譜面[1].n透明度 = 255;
                    //        this.tx達人譜面[1].n透明度 = 255;

                    //        this.tx達人譜面[1].n透明度 = this.ct分岐アニメ進行.n現在の値 > 100 ? 0 : (255 - ((this.ct分岐アニメ進行.n現在の値 * 0xff) / 60));
                    //        if (this.ct分岐アニメ進行.n現在の値 < 60)
                    //        {
                    //            this.nY = this.ct分岐アニメ進行.n現在の値 / 2;
                    //            this.tx達人譜面[1].t2D描画(CDTXMania.app.Device, 333, 192 - nY);
                    //        }

                    //        this.tx玄人譜面[1].t2D描画(CDTXMania.app.Device, 333, 222 - nY);
                    //    }
                    //    #endregion
                    //}





            }
            //CDTXMania.act文字コンソール.tPrint(0, 16, C文字コンソール.Eフォント種別.白, this.ct分岐アニメ進行.n現在の値.ToString());
            //CDTXMania.act文字コンソール.tPrint(0, 32, C文字コンソール.Eフォント種別.白, this.nY.ToString());



            return base.On進行描画();
        }

        public virtual void t分岐レイヤー・コース変化(int n現在, int n次回)
        {
            if (n現在 == n次回)
            {
                return;
            }


            this.ct分岐アニメ進行 = new CCounter(0, 300, 2, CDTXMania.Timer);
            this.bState = true;
            //this.ct分岐アニメ進行.n現在の値 = 0;

            this.n1 = n現在;
            this.n2 = n次回;

        }

        #region[ private ]
        //-----------------
        public bool bState;
        public CCounter ct分岐アニメ進行;
        private int n1;
        private int n2;
        private int n透明度;
        private int nY;
        private CTexture[] tx普通譜面 = new CTexture[2];
        private CTexture[] tx玄人譜面 = new CTexture[2];
        private CTexture[] tx達人譜面 = new CTexture[2];
        //-----------------
        #endregion
    }
}
