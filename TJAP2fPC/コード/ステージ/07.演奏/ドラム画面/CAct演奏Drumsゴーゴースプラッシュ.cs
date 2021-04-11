using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using SlimDX;
using FDK;

namespace DTXMania
{
    /// <summary>
    /// ゴーゴータイム開始時に出る煙エフェクト
    /// ＜仕様＞
    ///  -短い間隔で出されてもエフェクト開始されますが、同時に画面に出すのは2つまでです。
    /// </summary>
	internal class CAct演奏Drumsゴーゴースプラッシュ : CActivity
	{
		// コンストラクタ

		public CAct演奏Drumsゴーゴースプラッシュ()
		{
			base.b活性化してない = true;
		}
		
		
		// メソッド
        public virtual void tSplashStart()
		{
            if( this.txゴーゴースプラッシュ != null )
            {
                for( int i = 0; i < 2; i++ )
                {
                    if( !this.stゴーゴースプラッシュ[ i ].b使用中 )
                    {
                        this.stゴーゴースプラッシュ[ i ].b使用中 = true;
                        this.stゴーゴースプラッシュ[ i ].ct進行 = new CCounter( 0, 29, 20, CDTXMania.Timer ); // カウンタ
                        break;
                    }
                }
            }
		}


		// CActivity 実装

		public override void On活性化()
		{
            for( int i = 0; i < 2; i++ )
			{
				this.stゴーゴースプラッシュ[ i ].ct進行 = new CCounter();
                this.stゴーゴースプラッシュ[ i ].b使用中 = false;
			}
            base.On活性化();
		}
		public override void On非活性化()
		{
            for( int i = 0; i < 2; i++ )
			{
				this.stゴーゴースプラッシュ[ i ].ct進行 = null;
			}
			base.On非活性化();
		}
		public override void OnManagedリソースの作成()
		{
			if( !base.b活性化してない )
			{
                this.txゴーゴースプラッシュ = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_gogosplash.png" ) );
                if( this.txゴーゴースプラッシュ != null ) this.txゴーゴースプラッシュ.b加算合成 = true;
				base.OnManagedリソースの作成();
			}
		}
		public override void OnManagedリソースの解放()
		{
			if( !base.b活性化してない )
			{
                CDTXMania.tテクスチャの解放( ref this.txゴーゴースプラッシュ );

				base.OnManagedリソースの解放();
			}
		}
		public override int On進行描画()
		{
			if( !base.b活性化してない )
			{
                for( int i = 0; i < 2; i++ )
                {
                    if( this.stゴーゴースプラッシュ[ i ].b使用中 )
                    {
                        this.stゴーゴースプラッシュ[ i ].ct進行.t進行();
                        if( this.stゴーゴースプラッシュ[ i ].ct進行.b終了値に達した )
                        {
                            this.stゴーゴースプラッシュ[ i ].ct進行.t停止();
                            this.stゴーゴースプラッシュ[ i ].b使用中 = false;
                        }

                        if( this.txゴーゴースプラッシュ != null )
                        {
                            for( int j = 0; j < 5; j++ )
                                this.txゴーゴースプラッシュ.t2D描画( CDTXMania.app.Device, 0 + ( 260 * j ), 320, new Rectangle( 256 * this.stゴーゴースプラッシュ[ i ].ct進行.n現在の値, 0, 256, 420 ) );
                        }

                    }
                }

			}
			return 0;
		}
		

		// その他

		#region [ private ]
		//-----------------
        private CTexture txゴーゴースプラッシュ;

        [StructLayout(LayoutKind.Sequential)]
        private struct STゴーゴースプラッシュ
        {
            public bool b使用中;
            public CCounter ct進行;
        }

        private STゴーゴースプラッシュ[] stゴーゴースプラッシュ = new STゴーゴースプラッシュ[ 2 ];

		//-----------------
		#endregion
	}
}
