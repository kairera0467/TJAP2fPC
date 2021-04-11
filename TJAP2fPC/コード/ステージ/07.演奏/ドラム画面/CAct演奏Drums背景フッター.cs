using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using FDK;

namespace DTXMania
{
    internal class CAct演奏Drums背景フッター : CActivity
    {
        public CAct演奏Drums背景フッター()
        {
            base.b活性化してない = true;
        }

        public override void On活性化()
        {
            base.On活性化();
        }

        public override void On非活性化()
        {
            if( !this.b活性化してない )
            {
                base.On非活性化();
            }
        }

        public override void OnManagedリソースの作成()
        {
            if( !this.b活性化してない )
            {
                //ファイル一覧を生成する
                string[] strUpperBG = Directory.GetFiles( CSkin.Path("Graphics\\Dancer_BG\\footer"), "*.png" );

                if( strUpperBG.Length > 0 )
                {
                    //一覧からランダムで選ぶ
                    Random rand = new Random();
                    int now = rand.Next( 0, strUpperBG.Length - 1 );
                    this.txフッター = CDTXMania.tテクスチャの生成( strUpperBG[ now ] );
                }
                
                base.OnManagedリソースの作成();
            }
        }

        public override void OnManagedリソースの解放()
        {
            if( !this.b活性化してない )
            {
                CDTXMania.t安全にDisposeする( ref this.txフッター );
                base.OnManagedリソースの解放();
            }
        }

        public override int On進行描画()
        {
            if( !this.b活性化してない )
            {
                if( this.b初めての進行描画 )
                {
                    this.b初めての進行描画 = false;
                }

                if( this.txフッター != null )
                {
                    this.txフッター.t2D描画( CDTXMania.app.Device, 0, 720 - this.txフッター.sz画像サイズ.Height );
                }
            }
            return base.On進行描画();
        }

        #region[ private ]
        //-----------------
        private CTexture txフッター;
        //-----------------
        #endregion
    }
}
