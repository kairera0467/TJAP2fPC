﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.IO;
using SlimDX;
using FDK;

namespace DTXMania
{
	internal class CActResultParameterPanel : CActivity
	{
		// コンストラクタ

		public CActResultParameterPanel()
		{
			ST文字位置[] st文字位置Array = new ST文字位置[ 11 ];
			ST文字位置 st文字位置 = new ST文字位置();
			st文字位置.ch = '0';
			st文字位置.pt = new Point( 0, 0 );
			st文字位置Array[ 0 ] = st文字位置;
			ST文字位置 st文字位置2 = new ST文字位置();
			st文字位置2.ch = '1';
			st文字位置2.pt = new Point( 32, 0 );
			st文字位置Array[ 1 ] = st文字位置2;
			ST文字位置 st文字位置3 = new ST文字位置();
			st文字位置3.ch = '2';
			st文字位置3.pt = new Point( 64, 0 );
			st文字位置Array[ 2 ] = st文字位置3;
			ST文字位置 st文字位置4 = new ST文字位置();
			st文字位置4.ch = '3';
			st文字位置4.pt = new Point( 96, 0 );
			st文字位置Array[ 3 ] = st文字位置4;
			ST文字位置 st文字位置5 = new ST文字位置();
			st文字位置5.ch = '4';
			st文字位置5.pt = new Point( 128, 0 );
			st文字位置Array[ 4 ] = st文字位置5;
			ST文字位置 st文字位置6 = new ST文字位置();
			st文字位置6.ch = '5';
			st文字位置6.pt = new Point( 160, 0 );
			st文字位置Array[ 5 ] = st文字位置6;
			ST文字位置 st文字位置7 = new ST文字位置();
			st文字位置7.ch = '6';
			st文字位置7.pt = new Point( 192, 0 );
			st文字位置Array[ 6 ] = st文字位置7;
			ST文字位置 st文字位置8 = new ST文字位置();
			st文字位置8.ch = '7';
			st文字位置8.pt = new Point( 224, 0 );
			st文字位置Array[ 7 ] = st文字位置8;
			ST文字位置 st文字位置9 = new ST文字位置();
			st文字位置9.ch = '8';
			st文字位置9.pt = new Point( 256, 0 );
			st文字位置Array[ 8 ] = st文字位置9;
			ST文字位置 st文字位置10 = new ST文字位置();
			st文字位置10.ch = '9';
			st文字位置10.pt = new Point( 288, 0 );
			st文字位置Array[ 9 ] = st文字位置10;
			ST文字位置 st文字位置11 = new ST文字位置();
			st文字位置11.ch = ' ';
			st文字位置11.pt = new Point( 0, 0 );
			st文字位置Array[ 10 ] = st文字位置11;
			this.st小文字位置 = st文字位置Array;

			ST文字位置[] st文字位置Array2 = new ST文字位置[ 11 ];
			ST文字位置 st文字位置12 = new ST文字位置();
			st文字位置12.ch = '0';
			st文字位置12.pt = new Point( 0, 0 );
			st文字位置Array2[ 0 ] = st文字位置12;
			ST文字位置 st文字位置13 = new ST文字位置();
			st文字位置13.ch = '1';
			st文字位置13.pt = new Point( 32, 0 );
			st文字位置Array2[ 1 ] = st文字位置13;
			ST文字位置 st文字位置14 = new ST文字位置();
			st文字位置14.ch = '2';
			st文字位置14.pt = new Point( 64, 0 );
			st文字位置Array2[ 2 ] = st文字位置14;
			ST文字位置 st文字位置15 = new ST文字位置();
			st文字位置15.ch = '3';
			st文字位置15.pt = new Point( 96, 0 );
			st文字位置Array2[ 3 ] = st文字位置15;
			ST文字位置 st文字位置16 = new ST文字位置();
			st文字位置16.ch = '4';
			st文字位置16.pt = new Point( 128, 0 );
			st文字位置Array2[ 4 ] = st文字位置16;
			ST文字位置 st文字位置17 = new ST文字位置();
			st文字位置17.ch = '5';
			st文字位置17.pt = new Point( 160, 0 );
			st文字位置Array2[ 5 ] = st文字位置17;
			ST文字位置 st文字位置18 = new ST文字位置();
			st文字位置18.ch = '6';
			st文字位置18.pt = new Point( 192, 0 );
			st文字位置Array2[ 6 ] = st文字位置18;
			ST文字位置 st文字位置19 = new ST文字位置();
			st文字位置19.ch = '7';
			st文字位置19.pt = new Point( 224, 0 );
			st文字位置Array2[ 7 ] = st文字位置19;
			ST文字位置 st文字位置20 = new ST文字位置();
			st文字位置20.ch = '8';
			st文字位置20.pt = new Point( 256, 0 );
			st文字位置Array2[ 8 ] = st文字位置20;
			ST文字位置 st文字位置21 = new ST文字位置();
			st文字位置21.ch = '9';
			st文字位置21.pt = new Point( 288, 0 );
			st文字位置Array2[ 9 ] = st文字位置21;
			ST文字位置 st文字位置22 = new ST文字位置();
			st文字位置22.ch = '%';
			st文字位置22.pt = new Point( 0x37, 0 );
			st文字位置Array2[ 10 ] = st文字位置22;
			this.st大文字位置 = st文字位置Array2;

            ST文字位置[] stScore文字位置Array = new ST文字位置[10];
            ST文字位置 stScore文字位置 = new ST文字位置();
            stScore文字位置.ch = '0';
            stScore文字位置.pt = new Point(0, 0);
            stScore文字位置Array[0] = stScore文字位置;
            ST文字位置 stScore文字位置2 = new ST文字位置();
            stScore文字位置2.ch = '1';
            stScore文字位置2.pt = new Point(24, 0);
            stScore文字位置Array[1] = stScore文字位置2;
            ST文字位置 stScore文字位置3 = new ST文字位置();
            stScore文字位置3.ch = '2';
            stScore文字位置3.pt = new Point(48, 0);
            stScore文字位置Array[2] = stScore文字位置3;
            ST文字位置 stScore文字位置4 = new ST文字位置();
            stScore文字位置4.ch = '3';
            stScore文字位置4.pt = new Point(72, 0);
            stScore文字位置Array[3] = stScore文字位置4;
            ST文字位置 stScore文字位置5 = new ST文字位置();
            stScore文字位置5.ch = '4';
            stScore文字位置5.pt = new Point(96, 0);
            stScore文字位置Array[4] = stScore文字位置5;
            ST文字位置 stScore文字位置6 = new ST文字位置();
            stScore文字位置6.ch = '5';
            stScore文字位置6.pt = new Point(120, 0);
            stScore文字位置Array[5] = stScore文字位置6;
            ST文字位置 stScore文字位置7 = new ST文字位置();
            stScore文字位置7.ch = '6';
            stScore文字位置7.pt = new Point(144, 0);
            stScore文字位置Array[6] = stScore文字位置7;
            ST文字位置 stScore文字位置8 = new ST文字位置();
            stScore文字位置8.ch = '7';
            stScore文字位置8.pt = new Point(168, 0);
            stScore文字位置Array[7] = stScore文字位置8;
            ST文字位置 stScore文字位置9 = new ST文字位置();
            stScore文字位置9.ch = '8';
            stScore文字位置9.pt = new Point(192, 0);
            stScore文字位置Array[8] = stScore文字位置9;
            ST文字位置 stScore文字位置10 = new ST文字位置();
            stScore文字位置10.ch = '9';
            stScore文字位置10.pt = new Point(216, 0);
            stScore文字位置Array[9] = stScore文字位置10;
            this.stScoreFont = stScore文字位置Array;



			this.ptFullCombo位置 = new Point[] { new Point( 0x80, 0xed ), new Point( 0xdf, 0xed ), new Point( 0x141, 0xed ) };
			base.b活性化してない = true;
		}


		// メソッド

		public void tアニメを完了させる()
		{
			this.ct表示用.n現在の値 = this.ct表示用.n終了値;
		}


		// CActivity 実装

		public override void On活性化()
		{
			this.sdDTXで指定されたフルコンボ音 = null;
			this.bフルコンボ音再生済み = false;
			base.On活性化();
		}
		public override void On非活性化()
		{
			if( this.ct表示用 != null )
			{
				this.ct表示用 = null;
			}
			if( this.sdDTXで指定されたフルコンボ音 != null )
			{
				CDTXMania.Sound管理.tサウンドを破棄する( this.sdDTXで指定されたフルコンボ音 );
				this.sdDTXで指定されたフルコンボ音 = null;
			}
			base.On非活性化();
		}
		public override void OnManagedリソースの作成()
		{
			if( !base.b活性化してない )
			{
				this.txパネル本体 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\8_Result_panel.png" ) );
				this.tx文字 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\8_Result_number_s.png" ) );
				//this.txFullCombo = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\ScreenResult fullcombo.png" ) );
				this.txWhite = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\Tile white 64x64.png" ) );
                this.tx判定A = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\8_judge.png" ) );
                this.tx判定B = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\8_judge2.png" ) );
                this.txScore = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\8_Score_number.png" ) );
                this.txScoreA = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\8_Score_number_B.png" ) );
                this.txゲージ = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\8_ResultGauge.png" ) );
                this.txゲージ土台 = CDTXMania.tテクスチャの生成( CSkin.Path(@"Graphics\8_ResultGauge_base.png") );
                this.tx魂 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_Soul.png" ) );
                this.tx炎 = CDTXMania.tテクスチャの生成Af( CSkin.Path(@"Graphics\7_Soul_fire.png") );
                this.txネームプレート = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_NamePlate.png" ) );
				base.OnManagedリソースの作成();
			}
		}
		public override void OnManagedリソースの解放()
		{
			if( !base.b活性化してない )
			{
				CDTXMania.tテクスチャの解放( ref this.txパネル本体 );
				CDTXMania.tテクスチャの解放( ref this.tx文字 );
				//CDTXMania.tテクスチャの解放( ref this.txFullCombo );
				CDTXMania.tテクスチャの解放( ref this.txWhite );
				CDTXMania.tテクスチャの解放( ref this.tx判定A );
				CDTXMania.tテクスチャの解放( ref this.tx判定B );
                CDTXMania.tテクスチャの解放( ref this.txScore );
                CDTXMania.tテクスチャの解放( ref this.txScoreA );
                CDTXMania.tテクスチャの解放( ref this.txゲージ );
                CDTXMania.tテクスチャの解放( ref this.txゲージ土台 );
                CDTXMania.tテクスチャの解放( ref this.tx魂 );
                CDTXMania.tテクスチャの解放( ref this.tx炎 );
                CDTXMania.tテクスチャの解放( ref this.txネームプレート );
				base.OnManagedリソースの解放();
			}
		}
		public override int On進行描画()
		{
			if( base.b活性化してない )
			{
				return 0;
			}
			if( base.b初めての進行描画 )
			{
				this.ct表示用 = new CCounter( 0, 0x3e7, 2, CDTXMania.Timer );
				base.b初めての進行描画 = false;
			}
			this.ct表示用.t進行();
			if( this.txパネル本体 != null )
			{
				this.txパネル本体.t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nResultPanelP1X, CDTXMania.Skin.nResultPanelP1Y );
			}
			if( this.txScoreA != null )
			{
				this.txScoreA.t2D描画( CDTXMania.app.Device, 753, 249 ); //点
			}
            if( this.tx判定A != null )
            {
				this.tx判定A.t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nResultJudge1_P1X, CDTXMania.Skin.nResultJudge1_P1Y );
            }
            if( this.tx判定B != null )
            {
				this.tx判定B.t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nResultJudge2_P1X, CDTXMania.Skin.nResultJudge2_P1Y );
            }
            if( this.txゲージ土台 != null && this.txゲージ != null )
            {
                //int nRectX = (int)( CDTXMania.stage結果.st演奏記録.Drums.fゲージ / 2) * 12;
                double Rate = CDTXMania.stage結果.st演奏記録.Drums.fゲージ;
                //nRectX = CDTXMania.stage結果.st演奏記録.Drums.fゲージ >= 80.0f ? 80 : nRectX;
                this.txゲージ土台.t2D描画( CDTXMania.app.Device, CDTXMania.Skin.nResultGaugeBaseP1X, CDTXMania.Skin.nResultGaugeBaseP1Y, new Rectangle( 0, 0, 691, 47 ) );
                #region[ ゲージ本体 ]
                if( Rate > 2 )
                    this.txゲージ.t2D描画( CDTXMania.app.Device, 559, 145, new Rectangle( 0, 20, 12, 20 ) );
                if( Rate > 4 )
                    this.txゲージ.t2D描画( CDTXMania.app.Device, 559 + 12, 145, new Rectangle( 0, 20, 12, 20 ) );
                if( Rate > 6 )
                    this.txゲージ.t2D描画( CDTXMania.app.Device, 559 + 24, 145, new Rectangle( 0, 20, 12, 20 ) );
                if( Rate > 8 )
                    this.txゲージ.t2D描画( CDTXMania.app.Device, 559 + 36, 145, new Rectangle( 12, 20, 13, 20 ) );
                if( Rate > 10 )
                    this.txゲージ.t2D描画( CDTXMania.app.Device, 559 + 49, 145, new Rectangle( 12, 20, 13, 20 ) );
                if( Rate > 12 )
                    this.txゲージ.t2D描画( CDTXMania.app.Device, 559 + 62, 145, new Rectangle( 0, 20, 12, 20 ) );
                if( Rate > 14 )
                    this.txゲージ.t2D描画( CDTXMania.app.Device, 559 + 74, 145, new Rectangle( 0, 20, 12, 20 ) );
                if( Rate > 16 )
                    this.txゲージ.t2D描画( CDTXMania.app.Device, 559 + 86, 145, new Rectangle( 12, 20, 13, 20 ) );
                if( Rate > 18 )
                    this.txゲージ.t2D描画( CDTXMania.app.Device, 559 + 99, 145, new Rectangle( 12, 20, 13, 20 ) );
                if( Rate > 20 )
                    this.txゲージ.t2D描画( CDTXMania.app.Device, 559 + 112, 145, new Rectangle( 12, 20, 13, 20 ) );
                if( Rate > 22 )
                    this.txゲージ.t2D描画( CDTXMania.app.Device, 559 + 125, 145, new Rectangle( 12, 20, 13, 20 ) );
                if( Rate > 24 )
                    this.txゲージ.t2D描画( CDTXMania.app.Device, 559 + 138, 145, new Rectangle( 0, 20, 12, 20 ) );
                if( Rate > 26 )
                    this.txゲージ.t2D描画( CDTXMania.app.Device, 559 + 150, 145, new Rectangle( 0, 20, 12, 20 ) );
                if( Rate > 28 )
                    this.txゲージ.t2D描画( CDTXMania.app.Device, 559 + 162, 145, new Rectangle( 12, 20, 13, 20 ) );
                if( Rate > 30 )
                    this.txゲージ.t2D描画( CDTXMania.app.Device, 559 + 175, 145, new Rectangle( 0, 20, 12, 20 ) );
                if( Rate > 32 )
                    this.txゲージ.t2D描画( CDTXMania.app.Device, 559 + 187, 145, new Rectangle( 12, 20, 13, 20 ) );
                if( Rate > 34 )
                    this.txゲージ.t2D描画( CDTXMania.app.Device, 559 + 200, 145, new Rectangle( 0, 20, 12, 20 ) );
                if( Rate > 36 )
                    this.txゲージ.t2D描画( CDTXMania.app.Device, 559 + 212, 145, new Rectangle( 12, 20, 13, 20 ) );
                if( Rate > 38 )
                    this.txゲージ.t2D描画( CDTXMania.app.Device, 559 + 225, 145, new Rectangle( 12, 20, 13, 20 ) );
                if( Rate > 40 )
                    this.txゲージ.t2D描画( CDTXMania.app.Device, 559 + 238, 145, new Rectangle( 12, 20, 13, 20 ) );
                if( Rate > 42 )
                    this.txゲージ.t2D描画( CDTXMania.app.Device, 559 + 251, 145, new Rectangle( 0, 20, 12, 20 ) );
                if( Rate > 44 )
                    this.txゲージ.t2D描画( CDTXMania.app.Device, 559 + 263, 145, new Rectangle( 12, 20, 13, 20 ) );
                if( Rate > 46 )
                    this.txゲージ.t2D描画( CDTXMania.app.Device, 559 + 276, 145, new Rectangle( 0, 20, 12, 20 ) );
                if( Rate > 48 )
                    this.txゲージ.t2D描画( CDTXMania.app.Device, 559 + 288, 145, new Rectangle( 12, 20, 13, 20 ) );
                if( Rate > 50 )
                    this.txゲージ.t2D描画( CDTXMania.app.Device, 559 + 301, 145, new Rectangle( 0, 20, 12, 20 ) );
                if( Rate > 52 )
                    this.txゲージ.t2D描画( CDTXMania.app.Device, 559 + 313, 145, new Rectangle( 12, 20, 13, 20 ) );
                if( Rate > 54 )
                    this.txゲージ.t2D描画( CDTXMania.app.Device, 559 + 326, 145, new Rectangle( 12, 20, 13, 20 ) );
                if( Rate > 56 )
                    this.txゲージ.t2D描画( CDTXMania.app.Device, 559 + 339, 145, new Rectangle( 12, 20, 13, 20 ) );
                if( Rate > 58 )
                    this.txゲージ.t2D描画( CDTXMania.app.Device, 559 + 352, 145, new Rectangle( 0, 20, 12, 20 ) );
                if( Rate > 60 )
                    this.txゲージ.t2D描画( CDTXMania.app.Device, 559 + 364, 145, new Rectangle( 12, 20, 13, 20 ) );
                if( Rate > 62 )
                    this.txゲージ.t2D描画( CDTXMania.app.Device, 559 + 377, 145, new Rectangle( 12, 20, 13, 20 ) );
                if( Rate > 64 )
                    this.txゲージ.t2D描画( CDTXMania.app.Device, 559 + 390, 145, new Rectangle( 0, 20, 12, 20 ) );
                if( Rate > 66 )
                    this.txゲージ.t2D描画( CDTXMania.app.Device, 559 + 402, 145, new Rectangle( 12, 20, 13, 20 ) );
                if( Rate > 68 )
                    this.txゲージ.t2D描画( CDTXMania.app.Device, 559 + 415, 145, new Rectangle( 0, 20, 12, 20 ) );
                if( Rate > 70 )
                    this.txゲージ.t2D描画( CDTXMania.app.Device, 559 + 427, 145, new Rectangle( 12, 20, 13, 20 ) );
                if( Rate > 72 )
                    this.txゲージ.t2D描画( CDTXMania.app.Device, 559 + 440, 145, new Rectangle( 0, 20, 12, 20 ) );
                if( Rate > 74 )
                    this.txゲージ.t2D描画( CDTXMania.app.Device, 559 + 452, 145, new Rectangle( 12, 20, 13, 20 ) );
                if( Rate > 76 )
                    this.txゲージ.t2D描画( CDTXMania.app.Device, 559 + 465, 145, new Rectangle( 12, 20, 13, 20 ) );
                if( Rate > 78 )
                    this.txゲージ.t2D描画( CDTXMania.app.Device, 559 + 478, 145, new Rectangle( 12, 20, 13, 20 ) );

                if( Rate > 80 )
                    this.txゲージ.t2D描画( CDTXMania.app.Device, 559 + 491, 125, new Rectangle( 25, 0, 12, 40 ) );
                if( Rate > 82 )
                    this.txゲージ.t2D描画( CDTXMania.app.Device, 559 + 503, 125, new Rectangle( 49, 0, 13, 40 ) );
                if( Rate > 84 )
                    this.txゲージ.t2D描画( CDTXMania.app.Device, 559 + 516, 125, new Rectangle( 37, 0, 12, 40 ) );
                if( Rate > 86 )
                    this.txゲージ.t2D描画( CDTXMania.app.Device, 559 + 528, 125, new Rectangle( 49, 0, 13, 40 ) );
                if( Rate > 88 )
                    this.txゲージ.t2D描画( CDTXMania.app.Device, 559 + 541, 125, new Rectangle( 37, 0, 12, 40 ) );
                if( Rate > 90 )
                    this.txゲージ.t2D描画( CDTXMania.app.Device, 559 + 553, 125, new Rectangle( 49, 0, 13, 40 ) );
                if( Rate > 92 )
                    this.txゲージ.t2D描画( CDTXMania.app.Device, 559 + 566, 125, new Rectangle( 49, 0, 13, 40 ) );
                if( Rate > 94 )
                    this.txゲージ.t2D描画( CDTXMania.app.Device, 559 + 579, 125, new Rectangle( 37, 0, 12, 40 ) );
                if( Rate > 96 )
                    this.txゲージ.t2D描画( CDTXMania.app.Device, 559 + 591, 125, new Rectangle( 49, 0, 13, 40 ) );
                if( Rate > 98 )
                    this.txゲージ.t2D描画( CDTXMania.app.Device, 559 + 604, 125, new Rectangle( 37, 0, 12, 40 ) );
                if( Rate > 100 )
                    this.txゲージ.t2D描画( CDTXMania.app.Device, 559 + 616, 125, new Rectangle( 49, 0, 10, 40 ) );

                #endregion
            }
            if( this.tx魂 != null )
            {
                if( this.tx炎 != null && CDTXMania.stage結果.st演奏記録.Drums.fゲージ >= 100.0f )
                    this.tx炎.t2D描画( CDTXMania.app.Device, 1100, 34, new Rectangle( 0, 0, 230, 230 ) );
                this.tx魂.t2D描画( CDTXMania.app.Device, 1174, 107, new Rectangle( 0, 0, 80, 80 ) );
            }
            //演奏中のやつ使いまわせなかった。ファック。
            this.tスコア文字表示( CDTXMania.Skin.nResultScoreP1X, CDTXMania.Skin.nResultScoreP1Y, string.Format( "{0,7:######0}",CDTXMania.stage結果.st演奏記録.Drums.nスコア ) );
            this.t小文字表示( CDTXMania.Skin.nResultGreatP1X, CDTXMania.Skin.nResultGreatP1Y, string.Format( "{0,4:###0}", CDTXMania.stage結果.st演奏記録.Drums.nPerfect数.ToString() ) );
            this.t小文字表示( CDTXMania.Skin.nResultGoodP1X, CDTXMania.Skin.nResultGoodP1Y, string.Format( "{0,4:###0}", CDTXMania.stage結果.st演奏記録.Drums.nGreat数.ToString() ) );
            this.t小文字表示( CDTXMania.Skin.nResultBadP1X, CDTXMania.Skin.nResultBadP1Y, string.Format( "{0,4:###0}", CDTXMania.stage結果.st演奏記録.Drums.nMiss数.ToString() ) );

            this.t小文字表示( CDTXMania.Skin.nResultComboP1X, CDTXMania.Skin.nResultComboP1Y, string.Format( "{0,4:###0}", CDTXMania.stage結果.st演奏記録.Drums.n最大コンボ数.ToString() ) );
            this.t小文字表示( CDTXMania.Skin.nResultRollP1X, CDTXMania.Skin.nResultRollP1Y, string.Format( "{0,4:###0}", CDTXMania.stage結果.st演奏記録.Drums.n連打数.ToString() ) );
            //CDTXMania.act文字コンソール.tPrint( 960, 200, C文字コンソール.Eフォント種別.白, string.Format( "{0,4:###0}",CDTXMania.stage結果.st演奏記録.Drums.nPerfect数.ToString()) );
            //CDTXMania.act文字コンソール.tPrint( 960, 236, C文字コンソール.Eフォント種別.白, string.Format( "{0,4:###0}",CDTXMania.stage結果.st演奏記録.Drums.nGreat数.ToString()) );
            //CDTXMania.act文字コンソール.tPrint( 960, 276, C文字コンソール.Eフォント種別.白, string.Format( "{0,4:###0}",CDTXMania.stage結果.st演奏記録.Drums.nMiss数.ToString()) );

            //CDTXMania.act文字コンソール.tPrint( 1150, 200, C文字コンソール.Eフォント種別.白, string.Format( "{0,4:###0}",CDTXMania.stage結果.st演奏記録.Drums.n最大コンボ数.ToString()) );
			int num = this.ct表示用.n現在の値;

            this.txネームプレート.t2D描画( CDTXMania.app.Device, 254, 93 );

			if( !this.ct表示用.b終了値に達した )
			{
				return 0;
			}
			return 1;
		}
		

		// その他

		#region [ private ]
		//-----------------
		[StructLayout( LayoutKind.Sequential )]
		private struct ST文字位置
		{
			public char ch;
			public Point pt;
		}

		private bool bフルコンボ音再生済み;
		private CCounter ct表示用;
		private readonly Point[] ptFullCombo位置;
		private CSound sdDTXで指定されたフルコンボ音;
		private readonly ST文字位置[] st小文字位置;
		private readonly ST文字位置[] st大文字位置;
        private ST文字位置[] stScoreFont;
		private CTexture txFullCombo;
		private CTexture txWhite;
		private CTexture txパネル本体;
        private CTexture tx判定A;
        private CTexture tx判定B;
        private CTexture txScore;
        private CTexture txScoreA;
        private CTexture txゲージ土台;
        private CTexture txゲージ;
        private CTexture tx魂;
        private CTextureAf tx炎;
		private CTexture tx文字;
        private CTexture txネームプレート;

		private void t小文字表示( int x, int y, string str )
		{
			foreach( char ch in str )
			{
				for( int i = 0; i < this.st小文字位置.Length; i++ )
				{
                    if( ch == ' ' )
                    {
                        break;
                    }

					if( this.st小文字位置[ i ].ch == ch )
					{
						Rectangle rectangle = new Rectangle( this.st小文字位置[ i ].pt.X, this.st小文字位置[ i ].pt.Y, 32, 38 );
						if( this.tx文字 != null )
						{
							this.tx文字.t2D描画( CDTXMania.app.Device, x, y, rectangle );
						}
						break;
					}
				}
				x += 22;
			}
		}
		private void t大文字表示( int x, int y, string str )
		{
			this.t大文字表示( x, y, str, false );
		}
		private void t大文字表示( int x, int y, string str, bool b強調 )
		{
			foreach( char ch in str )
			{
				for( int i = 0; i < this.st大文字位置.Length; i++ )
				{
					if( this.st大文字位置[ i ].ch == ch )
					{
						Rectangle rectangle = new Rectangle( this.st大文字位置[ i ].pt.X, this.st大文字位置[ i ].pt.Y, 11, 0x10 );
						if( ch == '.' )
						{
							rectangle.Width -= 2;
							rectangle.Height -= 2;
						}
						if( this.tx文字 != null )
						{
							this.tx文字.t2D描画( CDTXMania.app.Device, x, y, rectangle );
						}
						break;
					}
				}
				x += 8;
			}
		}

        protected void tスコア文字表示(int x, int y, string str)
        {
            foreach (char ch in str)
            {
                for (int i = 0; i < this.stScoreFont.Length; i++)
                {
                    if (this.stScoreFont[i].ch == ch)
                    {
                        Rectangle rectangle = new Rectangle(this.stScoreFont[ i ].pt.X, 0, 24, 40);
                        if (this.txScore != null)
                        {
                            this.txScore.t2D描画(CDTXMania.app.Device, x, y, rectangle);
                        }
                        break;
                    }
                }
                x += 24;
            }
        }
		//-----------------
		#endregion
	}
}
