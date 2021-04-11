﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.Runtime.InteropServices;
using SlimDX;
using FDK;

namespace DTXMania
{
    internal class CAct演奏Drums連打 : CActivity
    {


        public CAct演奏Drums連打()
        {
            ST文字位置[] st文字位置Array = new ST文字位置[ 11 ];

			ST文字位置 st文字位置 = new ST文字位置();
			st文字位置.ch = '0';
			st文字位置.pt = new Point( 0, 0 );
			st文字位置Array[ 0 ] = st文字位置;
			ST文字位置 st文字位置2 = new ST文字位置();
			st文字位置2.ch = '1';
			st文字位置2.pt = new Point( 62, 0 );
			st文字位置Array[ 1 ] = st文字位置2;
			ST文字位置 st文字位置3 = new ST文字位置();
			st文字位置3.ch = '2';
			st文字位置3.pt = new Point( 124, 0 );
			st文字位置Array[ 2 ] = st文字位置3;
			ST文字位置 st文字位置4 = new ST文字位置();
			st文字位置4.ch = '3';
			st文字位置4.pt = new Point( 186, 0 );
			st文字位置Array[ 3 ] = st文字位置4;
			ST文字位置 st文字位置5 = new ST文字位置();
			st文字位置5.ch = '4';
			st文字位置5.pt = new Point( 248, 0 );
			st文字位置Array[ 4 ] = st文字位置5;
			ST文字位置 st文字位置6 = new ST文字位置();
			st文字位置6.ch = '5';
			st文字位置6.pt = new Point( 310, 0 );
			st文字位置Array[ 5 ] = st文字位置6;
			ST文字位置 st文字位置7 = new ST文字位置();
			st文字位置7.ch = '6';
			st文字位置7.pt = new Point( 372, 0 );
			st文字位置Array[ 6 ] = st文字位置7;
			ST文字位置 st文字位置8 = new ST文字位置();
			st文字位置8.ch = '7';
			st文字位置8.pt = new Point( 434, 0 );
			st文字位置Array[ 7 ] = st文字位置8;
			ST文字位置 st文字位置9 = new ST文字位置();
			st文字位置9.ch = '8';
			st文字位置9.pt = new Point( 496, 0 );
			st文字位置Array[ 8 ] = st文字位置9;
			ST文字位置 st文字位置10 = new ST文字位置();
			st文字位置10.ch = '9';
			st文字位置10.pt = new Point( 558, 0 );
			st文字位置Array[ 9 ] = st文字位置10;

			this.st文字位置 = st文字位置Array;

			base.b活性化してない = true;

        }

        public override void On活性化()
        {
            this.ct連打枠カウンター = new CCounter[ 4 ];
            for( int i = 0; i < 4; i++ )
            {
                this.ct連打枠カウンター[ i ] = new CCounter();
            }
            this.b表示 = new bool[]{ false, false, false, false };
            this.n連打数 = new int[ 4 ];
            base.On活性化();
        }

        public override void On非活性化()
        {
            base.On非活性化();
        }

        public override void OnManagedリソースの作成()
        {
            if( !this.b活性化してない )
            {
                this.tx連打枠 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_Rollballoon.png" ) );
                this.tx連打数字 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_RollNumber.png" ) );

                base.OnManagedリソースの作成();
            }
        }

        public override void OnManagedリソースの解放()
        {
            if( !this.b活性化してない )
            {
                CDTXMania.tテクスチャの解放( ref this.tx連打枠 );
                CDTXMania.tテクスチャの解放( ref this.tx連打数字 );

                base.OnManagedリソースの解放();
            }
        }

        public override int On進行描画( )
        {
            return base.On進行描画();
        }

        public int On進行描画( int n連打数, int player )
        {
            this.ct連打枠カウンター[ player ].t進行();

            //1PY:-3 2PY:514
            //仮置き
            int[] nRollBalloon = new int[] { -3, 514, 0, 0 };
            int[] nRollNumber = new int[] { 48, 559, 0, 0 };
            for( int i = 0; i < CDTXMania.ConfigIni.nPlayerCount; i++ )
            {
                if( this.ct連打枠カウンター[ player ].b終了値に達してない || this.b表示[ player ] )
                {
                    this.tx連打枠.t2D描画( CDTXMania.app.Device, 217, nRollBalloon[ player ] );
                    this.t文字表示( 330 + 62, nRollNumber[ player ], n連打数.ToString(), n連打数 );
                }
            }

            return base.On進行描画();
        }

        public void t枠表示時間延長( int player )
        {
            this.ct連打枠カウンター[ player ] = new CCounter( 0, 999, 2, CDTXMania.Timer );
        }


        public bool[] b表示;
        public int[] n連打数;
        public CCounter[] ct連打枠カウンター;
        private CTexture tx連打枠;
        private CTexture tx連打数字;
        private readonly ST文字位置[] st文字位置;

        [StructLayout(LayoutKind.Sequential)]
        private struct ST文字位置
        {
            public char ch;
            public Point pt;
        }

        private void t文字表示( int x, int y, string str, int n連打 )
		{
            int n桁数 = n連打.ToString().Length;
			foreach( char ch in str )
			{
				for( int i = 0; i < this.st文字位置.Length; i++ )
				{
					if( this.st文字位置[ i ].ch == ch )
					{
						Rectangle rectangle = new Rectangle( this.st文字位置[ i ].pt.X, this.st文字位置[ i ].pt.Y, 62, 80 );

						if( this.tx連打数字 != null )
						{
							this.tx連打数字.t2D描画( CDTXMania.app.Device, x - ( ( 62 * n桁数 ) / 2 ), y, rectangle );
						}
						break;
					}
				}
				x += ( 60 - ( n桁数 > 2 ? n桁数 * 2 : 0 ) );
			}
		}
    }
}
