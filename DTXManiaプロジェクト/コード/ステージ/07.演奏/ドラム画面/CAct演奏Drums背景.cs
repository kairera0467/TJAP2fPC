using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using FDK;

namespace DTXMania
{
    internal class CAct演奏Drums背景 : CActivity
    {
        // 本家っぽい背景を表示させるメソッド。
        //
        // おそらく近日中に仕様変えます(ぉい
        //
        public CAct演奏Drums背景()
        {
            base.b活性化してない = true;
        }

        public void tFadeIn()
        {
            this.ct上背景FIFOタイマー = new CCounter( 0, 100, 6, CDTXMania.Timer );
            this.eFadeMode = EFIFOモード.フェードイン;
        }

        public void tFadeOut()
        {
            this.ct上背景FIFOタイマー = new CCounter( 0, 100, 6, CDTXMania.Timer );
            this.eFadeMode = EFIFOモード.フェードアウト;
        }

        public override void On活性化()
        {
            this.dicParameter = new Dictionary<string, string>();

            //Upper_BG内のフォルダ一覧を生成する
            string[] strUpperBG = Directory.GetDirectories( CSkin.Path( "Graphics\\Upper_BG" ) );

            //ランダムで選んで基礎パスを生成
            Random rand = new Random();
            this.str上背景フォルダパス = CSkin.Path( @"Graphics\Upper_BG\"+ "01" + @"\" );

            //設定ファイルがあれば読み込む
            if( File.Exists( this.str上背景フォルダパス + @"setting.ini" ) )
            {
                StreamReader reader = new StreamReader( CSkin.Path( this.str上背景フォルダパス + @"setting.ini" ), Encoding.GetEncoding( "Shift_JIS" ) );
                string strSetting = reader.ReadToEnd();

                strSetting = strSetting.Replace( Environment.NewLine, "\n" );
                string[] delimiter = { "\n" };
                string[] strLine = strSetting.Split( delimiter, StringSplitOptions.RemoveEmptyEntries );

                for( int i = 0; i < strLine.Length; i++ )
                {
                    if( strLine[ i ].StartsWith( ";" ) )
                        continue; //コメント行の場合は無視

                    //まずSplit
                    string[] arLine = strLine[ i ].Split( '=' );

                    if( arLine.Length != 2 )
                        continue; //引数が2つじゃなければ無視。

                    foreach( string para in this.strSettingKey )
                    {
                        if( arLine[ 0 ] == para )
                        {
                            this.dicParameter.Add( para, arLine[ 1 ] );
                            break;
                        }
                    }

                }
                reader?.Close();

                //設定していく
                if( this.dicParameter.ContainsKey( "BackGroundLoopWidth" ) ) {
                    this.nLoopWidth = Convert.ToInt32( this.dicParameter[ "BackGroundLoopWidth" ] );
                }
                if ( this.dicParameter.ContainsKey( "BackGroundScrollSpeed" ) ) {
                    //未実装
                }
            }


            base.On活性化();
        }

        public override void On非活性化()
        {
            if( !this.b活性化してない )
            {
                CDTXMania.t安全にDisposeする( ref this.ct上背景FIFOタイマー );
                CDTXMania.t安全にDisposeする( ref this.ct上背景スクロール用タイマー );
                CDTXMania.t安全にDisposeする( ref this.ct下背景スクロール用タイマー1 );

                base.On非活性化();
            }
        }

        public override void OnManagedリソースの作成()
        {
            if( !this.b活性化してない )
            {
                this.dicTexture = new Dictionary<string, CTexture>();
                this.dicTexture.Add( "BackGround", CDTXMania.tテクスチャの生成( CSkin.Path( this.str上背景フォルダパス + this.dicParameter[ "BackGroundFileName" ] ) ) );
                this.dicTexture.Add( "BackGroundClear", CDTXMania.tテクスチャの生成( CSkin.Path( this.str上背景フォルダパス + this.dicParameter[ "BackGroundClearFileName" ] ) ) );
                this.dicTexture.Add( "BackGroundMiss", CDTXMania.tテクスチャの生成( CSkin.Path( this.str上背景フォルダパス + this.dicParameter[ "BackGroundMissFileName" ] ) ) );

                if( this.dicParameter.ContainsKey( "BackGroundP2FileName" ) ) {
                    this.dicTexture.Add( "BackGroundP2", CDTXMania.tテクスチャの生成( CSkin.Path( this.str上背景フォルダパス + this.dicParameter[ "BackGroundFileName" ] ) ) );
                } else {
                    this.dicTexture.Add( "BackGroundP2", CDTXMania.tテクスチャの生成( CSkin.Path( this.str上背景フォルダパス + this.dicParameter[ "BackGroundP2FileName" ] ) ) );
                }
                if ( this.dicParameter.ContainsKey( "BackGroundClearP2FileName" ) ) {
                    this.dicTexture.Add( "BackGroundClearP2", CDTXMania.tテクスチャの生成( CSkin.Path( this.str上背景フォルダパス + this.dicParameter[ "BackGroundClearFileName" ] ) ) );
                } else {
                    this.dicTexture.Add( "BackGroundClearP2", CDTXMania.tテクスチャの生成( CSkin.Path( this.str上背景フォルダパス + this.dicParameter[ "BackGroundClearP2FileName" ] ) ) );
                }
                if ( this.dicParameter.ContainsKey( "BackGroundMissP2FileName" ) ) {
                    this.dicTexture.Add( "BackGroundMissP2", CDTXMania.tテクスチャの生成( CSkin.Path( this.str上背景フォルダパス + this.dicParameter[ "BackGroundMissFileName" ] ) ) );
                } else {
                    this.dicTexture.Add( "BackGroundMissP2", CDTXMania.tテクスチャの生成( CSkin.Path( this.str上背景フォルダパス + this.dicParameter[ "BackGroundMissP2FileName" ] ) ) );
                }

                this.tx下背景メイン = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\Dancer_BG\01\bg.png" ) );
                this.tx下背景クリアメイン = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\Dancer_BG\01\bg_clear.png" ) );
                this.tx下背景クリアサブ1 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\Dancer_BG\01\bg_clear_01.png" ) );
                this.ct上背景スクロール用タイマー = new CCounter( 1, 328, 40, CDTXMania.Timer );
                this.ct下背景スクロール用タイマー1 = new CCounter( 1, 1257, 6, CDTXMania.Timer );
                this.ct上背景FIFOタイマー = new CCounter();
                base.OnManagedリソースの作成();
            }
        }

        public override void OnManagedリソースの解放()
        {
            if( !this.b活性化してない )
            {
                CDTXMania.tテクスチャの解放( ref this.tx下背景メイン );
                CDTXMania.tテクスチャの解放( ref this.tx下背景クリアメイン );
                CDTXMania.tテクスチャの解放( ref this.tx下背景クリアサブ1 );

                foreach( CTexture tex in this.dicTexture.Values  )
                {
                    tex?.Dispose();
                }
                this.dicTexture.Clear();
                this.dicParameter.Clear();
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

                int[] nBgY = new int[] { 0, 536 };
                this.ct上背景FIFOタイマー.t進行();
                this.ct上背景スクロール用タイマー.t進行Loop();
                this.ct下背景スクロール用タイマー1.t進行Loop();

                //おそらく1280 + 端数 + ( ループ幅 * 2 ) まで敷き詰めたら問題無いはず...
                for( int i = 0; i < CDTXMania.ConfigIni.nPlayerCount; i++ )
                {
                    if( CDTXMania.ConfigIni.nPlayerCount == 1 && i == 1 ) continue;
                
                    if( i == 0 )
                    {
                        //フェードイン中の見栄えが悪くなるためミス中でも表示させること
                        if( this.dicTexture[ "BackGround" ] != null )
                        {
                            for( int j = 0; j < 5; j++ )
                            {
                                this.dicTexture[ "BackGround" ].t2D描画( CDTXMania.app.Device, ( j * this.nLoopWidth ) - this.ct上背景スクロール用タイマー.n現在の値, nBgY[ i ] );
                            }
                        }
                        if( this.dicTexture[ "BackGroundClear" ] != null )
                        {
                            if( CDTXMania.stage演奏ドラム画面.actGauge.db現在のゲージ値[ 0 ] < 80.0 )
                                this.dicTexture[ "BackGroundClear" ].n透明度 = 0;
                            else
                                this.dicTexture[ "BackGroundClear" ].n透明度 = ( ( this.ct上背景FIFOタイマー.n現在の値 * 0xff ) / 100 );

                            for ( int j = 0; j < 5; j++ )
                            {
                                this.dicTexture[ "BackGroundClear" ].t2D描画( CDTXMania.app.Device, ( j * this.nLoopWidth ) - this.ct上背景スクロール用タイマー.n現在の値, nBgY[ i ] );
                            }
                        }
                        if( CDTXMania.stage演奏ドラム画面.bMiss中[ i ] )
                        {
                            if( this.dicTexture[ "BackGroundMiss" ] != null )
                            {
                                this.dicTexture[ "BackGroundMiss" ].n透明度 = ( ( this.ct上背景FIFOタイマー.n現在の値 * 0xff ) / 100 );
                                for( int j = 0; j < 5; j++ )
                                {
                                    this.dicTexture[ "BackGroundMiss" ].t2D描画( CDTXMania.app.Device, ( j * this.nLoopWidth ) - this.ct上背景スクロール用タイマー.n現在の値, nBgY[ i ] );
                                }
                            }
                        }
                    }
                    else if( i >= 1 )
                    {
                        if( !CDTXMania.stage演奏ドラム画面.bMiss中[ i ] )
                        {
                            if( this.dicTexture[ "BackGroundP" + i ] != null )
                            {
                                for( int j = 0; j < 5; j++ )
                                {
                                    this.dicTexture[ "BackGroundP" + i ].t2D描画( CDTXMania.app.Device, ( j * this.nLoopWidth ) - this.ct上背景スクロール用タイマー.n現在の値, nBgY[ i ] );
                                }
                            }
                            if( this.dicTexture[ "BackGroundClear" + i ] != null )
                            {
                                if( CDTXMania.stage演奏ドラム画面.actGauge.db現在のゲージ値[ 0 ] < 80.0 )
                                    this.dicTexture[ "BackGroundClear" + i ].n透明度 = 0;
                                else
                                    this.dicTexture[ "BackGroundClear" + i ].n透明度 = ( ( this.ct上背景FIFOタイマー.n現在の値 * 0xff ) / 100 );

                                for ( int j = 0; j < 5; j++ )
                                {
                                    this.dicTexture[ "BackGroundClear" + i ].t2D描画( CDTXMania.app.Device, ( j * this.nLoopWidth ) - this.ct上背景スクロール用タイマー.n現在の値, nBgY[ i ] );
                                }
                            }
                        }
                        else
                        {
                            if( this.dicTexture[ "BackGroundMiss" + i ] != null )
                            {
                                this.dicTexture[ "BackGroundMiss" + i ].n透明度 = ( ( this.ct上背景FIFOタイマー.n現在の値 * 0xff ) / 100 );
                                for( int j = 0; j < 5; j++ )
                                {
                                    this.dicTexture[ "BackGroundMiss" + i ].t2D描画( CDTXMania.app.Device, ( j * this.nLoopWidth ) - this.ct上背景スクロール用タイマー.n現在の値, nBgY[ i ] );
                                }
                            }
                        }
                    }


                }




                if( CDTXMania.ConfigIni.nPlayerCount == 1 )
                {
                    {
                        if( this.tx下背景メイン != null )
                        {
                            this.tx下背景メイン.t2D描画( CDTXMania.app.Device, 0, 360 );
                        }
                    }
                    if( CDTXMania.stage演奏ドラム画面.actGauge.db現在のゲージ値[ 0 ] >= 80.0 )
                    {
                        if( this.tx下背景クリアメイン != null && this.tx下背景クリアサブ1 != null )
                        {
                            this.tx下背景クリアメイン.n透明度 = ( ( this.ct上背景FIFOタイマー.n現在の値 * 0xff ) / 100 );
                            this.tx下背景クリアサブ1.n透明度 = ( ( this.ct上背景FIFOタイマー.n現在の値 * 0xff ) / 100 );
                    
                            this.tx下背景クリアメイン.t2D描画( CDTXMania.app.Device, 0, 360 );

                            int nループ幅 = 1257;
                            this.tx下背景クリアサブ1.t2D描画( CDTXMania.app.Device, 0 - this.ct下背景スクロール用タイマー1.n現在の値, 360 );
                            this.tx下背景クリアサブ1.t2D描画( CDTXMania.app.Device, ( 1 * nループ幅 ) - this.ct下背景スクロール用タイマー1.n現在の値, 360 );
                        }
                    }
                }
            }
            return base.On進行描画();
        }

        #region[ private ]
        //-----------------
        private CCounter ct上背景スクロール用タイマー; //上背景のX方向スクロール用
        private CCounter ct下背景スクロール用タイマー1; //下背景パーツ1のX方向スクロール用
        private CCounter ct上背景FIFOタイマー;
        private CTexture tx上背景メイン;
        private CTexture tx上背景クリアメイン;
        private CTexture tx上背景ミスメイン;
        private CTexture tx上背景メイン_P2;
        private CTexture tx上背景クリアメイン_P2;
        private CTexture tx上背景ミスメイン_P2;

        private CTexture tx下背景メイン;
        private CTexture tx下背景クリアメイン;
        private CTexture tx下背景クリアサブ1;
        private string str上背景フォルダパス;

        private Dictionary<string, CTexture> dicTexture;
        private Dictionary<string, string> dicParameter;
        private EFIFOモード eFadeMode;
        private int nLoopWidth;

        private string[] strSettingKey = new string[]
        {
            "BackGroundFileName",
            "BackGroundP2FileName",
            "BackGroundClearFileName",
            "BackGroundClearP2FileName",
            "BackGroundMissFileName",
            "BackGroundMissP2FileName",
            "BackGroundLoopWidth",
            "BackGroundScrollSpeed"
        };
        //-----------------
        #endregion
    }
}
