using System;
using System.Collections.Generic;
using System.Text;
using FDK;
using System.Drawing;
using System.Runtime.InteropServices;
using SharpDX.Animation;

namespace DTXMania
{
	internal class CAct演奏スコア共通 : CActivity
	{
		// プロパティ

		protected STDGBVALUE<long>[] nスコアの増分;
		protected STDGBVALUE<double>[] n現在の本当のスコア;
		protected STDGBVALUE<long>[] n現在表示中のスコア;
		protected CTexture txScore;
        protected CCounter ctTimer;

        // 2020.05.17 kairera0467
        // EXスコア
        // 常識的な譜面であれば理論値はノート数の2～4倍ぐらいにしかならないのでint型にした。
        protected int[] n現在のEXスコア;

        protected STスコア[] stScore;
        protected int n現在表示中のAddScore;

        [StructLayout( LayoutKind.Sequential )]
        protected struct STスコア
        {
            public bool b使用中;
            public bool b表示中;
            public bool bBonusScore;
            public CCounter ctTimer;
            public int nAddScore;
            public int nPlayer;
            public スコア文字[] _スコア文字;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct ST文字位置
        {
            public char ch;
            public Point pt;
        }
        private ST文字位置[] stFont;


		// コンストラクタ

		public CAct演奏スコア共通()
		{
            ST文字位置[] st文字位置Array = new ST文字位置[11];
            ST文字位置 st文字位置 = new ST文字位置();
            st文字位置.ch = '0';
            st文字位置.pt = new Point(0, 0);
            st文字位置Array[0] = st文字位置;
            ST文字位置 st文字位置2 = new ST文字位置();
            st文字位置2.ch = '1';
            st文字位置2.pt = new Point(24, 0);
            st文字位置Array[1] = st文字位置2;
            ST文字位置 st文字位置3 = new ST文字位置();
            st文字位置3.ch = '2';
            st文字位置3.pt = new Point(48, 0);
            st文字位置Array[2] = st文字位置3;
            ST文字位置 st文字位置4 = new ST文字位置();
            st文字位置4.ch = '3';
            st文字位置4.pt = new Point(72, 0);
            st文字位置Array[3] = st文字位置4;
            ST文字位置 st文字位置5 = new ST文字位置();
            st文字位置5.ch = '4';
            st文字位置5.pt = new Point(96, 0);
            st文字位置Array[4] = st文字位置5;
            ST文字位置 st文字位置6 = new ST文字位置();
            st文字位置6.ch = '5';
            st文字位置6.pt = new Point(120, 0);
            st文字位置Array[5] = st文字位置6;
            ST文字位置 st文字位置7 = new ST文字位置();
            st文字位置7.ch = '6';
            st文字位置7.pt = new Point(144, 0);
            st文字位置Array[6] = st文字位置7;
            ST文字位置 st文字位置8 = new ST文字位置();
            st文字位置8.ch = '7';
            st文字位置8.pt = new Point(168, 0);
            st文字位置Array[7] = st文字位置8;
            ST文字位置 st文字位置9 = new ST文字位置();
            st文字位置9.ch = '8';
            st文字位置9.pt = new Point(192, 0);
            st文字位置Array[8] = st文字位置9;
            ST文字位置 st文字位置10 = new ST文字位置();
            st文字位置10.ch = '9';
            st文字位置10.pt = new Point(216, 0);
            st文字位置Array[9] = st文字位置10;
            this.stFont = st文字位置Array;

            this.stScore = new STスコア[ 256 ];
			base.b活性化してない = true;
		}


		// メソッド

		public double Get( E楽器パート part, int player )
		{
			return this.n現在の本当のスコア[ player ][ (int) part ];
		}
		public void Set( E楽器パート part, double nScore, int player )
		{
            //現状、TAIKOパートでの演奏記録を結果ステージに持っていけないので、ドラムパートにも加算することでお茶を濁している。
            if( part == E楽器パート.TAIKO )
                part = E楽器パート.DRUMS;

			int nPart = (int) part;
			if( this.n現在の本当のスコア[ player ][ nPart ] != nScore )
			{
				this.n現在の本当のスコア[ player ][ nPart ] = nScore;
				this.nスコアの増分[ player ][ nPart ] = (long) ( ( (double) ( this.n現在の本当のスコア[ player ][ nPart ] - this.n現在表示中のスコア[ player ][ nPart ] ) ) / 20.0 );
				this.nスコアの増分[ player ].Guitar = (long) ( ( (double) ( this.n現在の本当のスコア[ player ][ nPart ] - this.n現在表示中のスコア[ player ][ nPart ] ) ) );
				if( this.nスコアの増分[ player ][ nPart ] < 1L )
				{
					this.nスコアの増分[ player ][ nPart ] = 1L;
				}
			}

            if( part == E楽器パート.DRUMS )
                part = E楽器パート.TAIKO;

			nPart = (int) part;
			if( this.n現在の本当のスコア[ player ][ nPart ] != nScore )
			{
				this.n現在の本当のスコア[ player ][ nPart ] = nScore;
				this.nスコアの増分[ player ][ nPart ] = (long) ( ( (double) ( this.n現在の本当のスコア[ player ][ nPart ] - this.n現在表示中のスコア[ player ][ nPart ] ) ) / 20.0 );
                this.nスコアの増分[ player ].Guitar = (long) ( ( (double) ( this.n現在の本当のスコア[ player ][ nPart ] - this.n現在表示中のスコア[ player ][ nPart ] ) ) );
				if( this.nスコアの増分[ player ][ nPart ] < 1L )
				{
					this.nスコアの増分[ player ][ nPart ] = 1L;
				}
			}
            
		}
		/// <summary>
		/// 点数を加える(各種AUTO補正つき)
		/// </summary>
		/// <param name="part"></param>
		/// <param name="bAutoPlay"></param>
		/// <param name="delta"></param>
		public void Add( E楽器パート part, STAUTOPLAY bAutoPlay, long delta, int player )
		{
			double rev = 1.0;
			switch ( part )
			{
				#region [ Unknown ]
				case E楽器パート.UNKNOWN:
					throw new ArgumentException();
				#endregion
			}
            this.ctTimer = new CCounter( 0, 500, 1, CDTXMania.Timer );

            for( int sc = 0; sc < 1; sc++ )
            {
                for( int i = 0; i < 256; i++ )
                {
                    if( this.stScore[ i ].b使用中 == false )
                    {
                        this.stScore[ i ].b使用中 = true;
                        this.stScore[ i ].b表示中 = true;
                        this.stScore[ i ].nAddScore = (int)delta;
                        this.stScore[ i ].ctTimer = new CCounter( 0, 500, 1, CDTXMania.Timer );
                        this.stScore[ i ].bBonusScore = false;
                        this.stScore[ i ].nPlayer = player;
                        this.n現在表示中のAddScore++;
                        break;
                    }
                }
            }

			this.Set( part, this.Get( part, player ) + delta * rev, player );
		}

        public void BonusAdd( int player )
        {
            for( int sc = 0; sc < 1; sc++ )
            {
                for( int i = 0; i < 256; i++ )
                {
                    if( this.stScore[ i ].b使用中 == false )
                    {
                        this.stScore[ i ].b使用中 = true;
                        this.stScore[ i ].b表示中 = true;
                        this.stScore[ i ].nAddScore = 10000;
                        this.stScore[ i ].ctTimer = new CCounter( 0, 500, 1, CDTXMania.Timer );
                        this.stScore[ i ].bBonusScore = true;
                        this.stScore[ i ].nPlayer = player;
                        this.n現在表示中のAddScore++;
                        break;
                    }
                }
            }

            this.Set( E楽器パート.TAIKO, this.Get( E楽器パート.TAIKO, player ) + 10000, player );
        }

        #region[ EX SCOREの実装 ]
        //IIDXと同じく良=2点、可=1点
        //TJAP2fPCでは大音符の両手/片手判定を導入しているため、大音符は両手良=4、両手可=3、片手良=2、片手可=1とする。
        // -オプションで片手判定をオフにしている場合は必ず両手扱いになるので、良=4、可=3となる。
        //連打が面倒なことになるが、風船連打はEXSCOREに含め(叩いた= 1)、黄色連打はEXSCOREに含めない。

        public int GetExScore(int player)
        {
            return this.n現在のEXスコア[ player ];
        }

        public void AddExScore(int player, E判定 e判定)
        {
            int delta;

            switch(e判定)
            {
                case E判定.Perfect:
                case E判定.Great:
                    delta = 2;
                    break;
                case E判定.Good:
                    delta = 1;
                    break;
                default:
                    delta = 0;
                    break;
            }

            this.n現在のEXスコア[ player ] += delta;
        }

        public void AddExScore(int player, int delta)
        {
            this.n現在のEXスコア[ player ] += delta;
        }
        #endregion

        // CActivity 実装

        public override void On活性化()
		{
            this.n現在表示中のスコア = new STDGBVALUE<long>[ 4 ];
            this.n現在の本当のスコア = new STDGBVALUE<double>[ 4 ];
            this.nスコアの増分 = new STDGBVALUE<long>[ 4 ];
            this.n現在のEXスコア = new int[ 4 ];
			for( int i = 0; i < 4; i++ )
			{
				this.n現在表示中のスコア[ i ][ i ] = 0L;
				this.n現在の本当のスコア[ i ][ i ] = 0L;
				this.nスコアの増分[ i ][ i ] = 0L;
                this.n現在のEXスコア[ i ] = 0;
			}
            for( int sc = 0; sc < 256; sc++ )
            {
                this.stScore[ sc ].b使用中 = false;
                this.stScore[ sc ].ctTimer = new CCounter();
                this.stScore[ sc ].nAddScore = 0;
                this.stScore[ sc ].bBonusScore = false;
                this.stScore[ sc ]._スコア文字 = new スコア文字[] // 一応10桁用意しておけば十分だろう...
                {
                    new スコア文字(),
                    new スコア文字(),
                    new スコア文字(),
                    new スコア文字(),
                    new スコア文字(),
                    new スコア文字(),
                    new スコア文字(),
                    new スコア文字(),
                    new スコア文字(),
                    new スコア文字()
                };
            }

            this.n現在表示中のAddScore = 0;

            this.ctTimer = new CCounter();
			base.On活性化();
		}
		public override void OnManagedリソースの作成()
		{
			if( !base.b活性化してない )
			{
				this.txScore = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\7_Score_number.png" ) );
				base.OnManagedリソースの作成();
			}
		}
		public override void OnManagedリソースの解放()
		{
			if( !base.b活性化してない )
			{
				CDTXMania.tテクスチャの解放( ref this.txScore );
				base.OnManagedリソースの解放();
			}
		}

        protected void t小文字表示( int x, int y, string str, int mode )
        {
            foreach( char ch in str )
            {
                for( int i = 0; i < this.stFont.Length; i++ )
                {
                    if( this.stFont[ i ].ch == ch )
                    {
                        Rectangle rectangle = new Rectangle(this.stFont[i].pt.X, this.stFont[i].pt.Y, 24, 34);
                        switch( mode )
                        {
                            case 0:
                                if( this.txScore != null )
                                {
                                    this.txScore.color4 = new SlimDX.Color4( 1.0f, 1.0f, 1.0f );
                                    this.txScore.t2D描画( CDTXMania.app.Device, x, y, rectangle );
                                }
                                break;
                            case 1:
                                if( this.txScore != null )
                                {
                                    //this.txScore.color4 = new SlimDX.Color4( 1.0f, 0.5f, 0.4f );
                                    this.txScore.color4 = CDTXMania.Skin.cScoreColor1P;
                                    this.txScore.t2D描画( CDTXMania.app.Device, x, y, rectangle );
                                }
                                break;
                            case 2:
                                if( this.txScore != null )
                                {
                                    //this.txScore.color4 = new SlimDX.Color4( 0.4f, 0.5f, 1.0f );
                                    this.txScore.color4 = CDTXMania.Skin.cScoreColor2P;
                                    this.txScore.t2D描画( CDTXMania.app.Device, x, y, rectangle );
                                }
                                break;
                        }
                        break;
                    }
                }
                x += 20;
            }
        }

        protected class スコア文字 : IDisposable
        {
            // 1桁ずつ扱う
            public int n桁;
            public Variable 文字X;
            public Variable 文字Y;
            public Variable 不透明度;
            public Storyboard sbストーリーボード;

            public スコア文字()
            {
                this.n桁 = 0;
            }

            public スコア文字( int 桁 )
            {
                this.n桁 = 桁;
            }

            public void Dispose()
            {
                this.sbストーリーボード?.Abandon();
                this.sbストーリーボード = null;

                this.文字X?.Dispose();
                this.文字X = null;

                this.文字Y?.Dispose();
                this.文字Y = null;

                this.不透明度?.Dispose();
                this.不透明度 = null;

                this.n桁 = 0;
            }
        }
	}
}
