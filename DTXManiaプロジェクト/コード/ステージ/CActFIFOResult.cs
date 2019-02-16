using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using FDK;
using SharpDX.Animation;

namespace DTXMania
{
	internal class CActFIFOResult : CActivity
	{
		// メソッド

		public void tフェードアウト開始()
		{
            // フェードアウト(幕を閉める)
			this.mode = EFIFOモード.フェードアウト;
            
            // Animetionのテスト用コード
            var animation = CDTXMania.AnimationManager;
            var basetime = animation.Timer.Time;
            var start = basetime;

            var 幕上 = this._幕[ 0 ];
            var 幕下 = this._幕[ 1 ];

            // アニメーションを構築
            幕上.Dispose();
            幕下.Dispose();

            // 初期値を設定
            幕上.左上位置X = new Variable( animation.Manager, 0 );
            幕上.左上位置Y = new Variable( animation.Manager, -360 );
            幕下.左上位置X = new Variable( animation.Manager, 0 );
            幕下.左上位置Y = new Variable( animation.Manager, 720 );

            幕上.ストーリーボード = new Storyboard( animation.Manager );
            幕下.ストーリーボード = new Storyboard( animation.Manager );

            using (var 幕Y移動 = animation.TrasitionLibrary.AccelerateDecelerate(1.0, 0, 0.2, 0.8))
                幕上.ストーリーボード.AddTransition( 幕上.左上位置Y, 幕Y移動 );

            using (var 幕Y移動 = animation.TrasitionLibrary.AccelerateDecelerate(1.0, 360, 0.2, 0.8))
                幕下.ストーリーボード.AddTransition( 幕下.左上位置Y, 幕Y移動 );

            幕上.ストーリーボード.Schedule( start );
            幕下.ストーリーボード.Schedule( start );
		}
		public void tフェードイン開始()
		{
            // フェードイン(幕を透過させる)
			this.mode = EFIFOモード.フェードイン;
            
            // Animetionのテスト用コード
            var animation = CDTXMania.AnimationManager;
            var basetime = animation.Timer.Time;
            var start = basetime;

            var 幕上 = this._幕[ 0 ];
            var 幕下 = this._幕[ 1 ];

            // アニメーションを構築
            幕上.Dispose();
            幕下.Dispose();

            // 初期値を設定
            幕上.左上位置X = new Variable( animation.Manager, 0 );
            幕上.左上位置Y = new Variable( animation.Manager, 0 );
            幕下.左上位置X = new Variable( animation.Manager, 0 );
            幕下.左上位置Y = new Variable( animation.Manager, 360 );
            幕上.透明度 = new Variable( animation.Manager, 255 );
            幕下.透明度 = new Variable( animation.Manager, 255 );

            幕上.ストーリーボード = new Storyboard( animation.Manager );
            幕下.ストーリーボード = new Storyboard( animation.Manager );
            
            using (var 透明度変化 = animation.TrasitionLibrary.Linear(1.0, 0))
                幕上.ストーリーボード.AddTransition( 幕上.透明度, 透明度変化 );
            
            using (var 透明度変化 = animation.TrasitionLibrary.Linear(1.0, 0))
                幕下.ストーリーボード.AddTransition( 幕下.透明度, 透明度変化 );

            幕上.ストーリーボード.Schedule( start );
            幕下.ストーリーボード.Schedule( start );
		}
		public void tフェードイン完了()		// #25406 2011.6.9 yyagi
		{
			
		}

        // CActivity 実装
        public override void On活性化()
        {
            this._幕 = new 幕[2] { new 幕(), new 幕() };
            base.On活性化();
        }
        public override void On非活性化()
		{
			if( !base.b活性化してない )
			{
                CDTXMania.tテクスチャの解放( ref this.tx幕 );

                // 開放
                if( this._幕 != null )
                {
                    this._幕[ 0 ].Dispose();
                    this._幕[ 1 ].Dispose();

                    this._幕 = null;
                }
				base.On非活性化();
			}
		}
		public override void OnManagedリソースの作成()
		{
			if( !base.b活性化してない )
			{
				this.tx幕 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\8_background_mask.png" ) );
				base.OnManagedリソースの作成();
			}
		}
		public override int On進行描画()
		{
			if( base.b活性化してない || ( this._幕[ 0 ].ストーリーボード == null ) )
			{
				return 0;
			}
            // Size clientSize = CDTXMania.app.Window.ClientSize;	// #23510 2010.10.31 yyagi: delete as of no one use this any longer.
            if (this.tx幕 != null)
            {
                if (this.mode == EFIFOモード.フェードアウト)
                {
                    this.tx幕.t2D描画(CDTXMania.app.Device, 0, (int)this._幕[0].左上位置Y.Value, new Rectangle(0, 0, 1280, 380));
                    this.tx幕.t2D描画(CDTXMania.app.Device, 0, (int)this._幕[1].左上位置Y.Value, new Rectangle(0, 380, 1280, 360));
                }
                else
                {
                    this.tx幕.n透明度 = (int)this._幕[0].透明度.Value;
                    this.tx幕.t2D描画(CDTXMania.app.Device, 0, 0, new Rectangle(0, 0, 1280, 360));
                    this.tx幕.t2D描画(CDTXMania.app.Device, 0, 360, new Rectangle(0, 380, 1280, 360));
                }
            }
			if( this._幕[ 0 ].ストーリーボード.Status != StoryboardStatus.Ready )
			{
				return 0;
			}
			return 1;
		}


		// その他

#region [ private ]
		//-----------------
		private EFIFOモード mode;
        private CTexture tx幕;
        //-----------------
        #endregion
        #region[ 幕アニメーション ]
        // FROM先生方式でアニメーションさせるオブジェクトの各種情報をクラスにして扱う。
        protected class 幕 : IDisposable
        {
            // 1枚のテクスチャを使い回すためCTextureはここに入れていない。
            public Variable 左上位置X;
            public Variable 左上位置Y;
            public Variable 透明度;
            public Storyboard ストーリーボード;

            public void Dispose()
            {
                this.ストーリーボード?.Abandon();
                this.ストーリーボード = null;

                this.左上位置X?.Dispose();
                this.左上位置X = null;

                this.左上位置Y?.Dispose();
                this.左上位置Y = null;

                this.透明度?.Dispose();
                this.透明度 = null;
            }
        }
        private 幕[] _幕 = null;
        #endregion
    }
}
