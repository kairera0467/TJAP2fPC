﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Diagnostics;
using SlimDX;
using FDK;

namespace DTXMania
{
	/// <summary>
	/// CAct演奏Drumsゲージ と CAct演奏Gutiarゲージ のbaseクラス。ダメージ計算やDanger/Failed判断もこのクラスで行う。
    /// 
    /// 課題
    /// _STAGE FAILED OFF時にゲージ回復を止める
    /// _黒→閉店までの差を大きくする。
	/// </summary>
	internal class CAct演奏ゲージ共通 : CActivity
	{
		// プロパティ
		public CActLVLNFont actLVLNFont { get; protected set; }

		// コンストラクタ
		public CAct演奏ゲージ共通()
		{
			//actLVLNFont = new CActLVLNFont();		// On活性化()に移動
			//actLVLNFont.On活性化();
		}

		// CActivity 実装

		public override void On活性化()
		{
			actLVLNFont = new CActLVLNFont();
			actLVLNFont.On活性化();
			base.On活性化();
		}
		public override void On非活性化()
		{
			actLVLNFont.On非活性化();
			actLVLNFont = null;
			base.On非活性化();
		}
		
		const double GAUGE_MAX = 100.0;
		const double GAUGE_INITIAL =  2.0 / 3;
		const double GAUGE_MIN = -0.1;
		const double GAUGE_ZERO = 0.0;
		const double GAUGE_DANGER = 0.3;
	
		public bool bRisky							// Riskyモードか否か
		{
			get;
			private set;
		}
		public int nRiskyTimes_Initial				// Risky初期値
		{
			get;
			private set;
		}
		public int nRiskyTimes						// 残Miss回数
		{
			get;
			private set;
		}
		public bool IsFailed( int player )	// 閉店状態になったかどうか
		{
			if ( bRisky ) {
				return ( nRiskyTimes <= 0 );
			}

            if( CDTXMania.ConfigIni.eGaugeMode == Eゲージモード.Normal || CDTXMania.ConfigIni.eGaugeMode == Eゲージモード.DEATH )
            	return this.db現在のゲージ値[ player ] <= GAUGE_MIN;
            else
                return this.db現在のゲージ値[ player ] <= 0;
		}
		public bool IsDanger( E楽器パート part )	// DANGERかどうか
		{
			if ( bRisky )
			{
				switch ( nRiskyTimes_Initial ) {
					case 1:
						return false;
					case 2:
					case 3:
						return ( nRiskyTimes <= 1 );
					default: 
						return ( nRiskyTimes <= 2 );
				}
			}
			return ( this.db現在のゲージ値[ (int) part ] <= GAUGE_DANGER );
		}

		public double dbゲージ値	// Drums専用
		{
			get
			{
				return this.db現在のゲージ値[ 0 ];
			}
			set
			{
				this.db現在のゲージ値[ 0 ] = value;
				if ( this.db現在のゲージ値[ 0 ] > GAUGE_MAX )
				{
					this.db現在のゲージ値[ 0 ] = GAUGE_MAX;
				}
			}
		}

		public double dbゲージ値2P	// Drums専用
		{
			get
			{
				return this.db現在のゲージ値[ 1 ];
			}
			set
			{
				this.db現在のゲージ値[ 1 ] = value;
				if ( this.db現在のゲージ値[ 1 ] > GAUGE_MAX )
				{
					this.db現在のゲージ値[ 1 ] = GAUGE_MAX;
				}
			}
		}

		/// <summary>
		/// ゲージの初期化
		/// </summary>
		/// <param name="nRiskyTimes_Initial_">Riskyの初期値(0でRisky未使用)</param>
		public void Init(int nRiskyTimes_InitialVal )		// ゲージ初期化
		{
            if( CDTXMania.ConfigIni.eGaugeMode == Eゲージモード.Normal || CDTXMania.ConfigIni.eGaugeMode == Eゲージモード.DEATH )
            {
                this.dbゲージ値 = 0;
			    for ( int i = 0; i < 4; i++ )
			    {
                    this.db現在のゲージ値[ i ] = 0;
			    }
            }
            else
            {
                //IIDX系の場合は20%スタート
                this.dbゲージ値 = 20.0;
			    for ( int i = 0; i < 4; i++ )
			    {
                    this.db現在のゲージ値[ i ] = 20.0;
			    }
            }

            //白黒鍵盤とお皿のゲーム風ゲージ。今の所テクスチャの実装は間に合ってない
            if( CDTXMania.ConfigIni.eGaugeMode == Eゲージモード.IIDX )
            {
                float ret = 0; //係数
                int note = CDTXMania.DTX.nノーツ数[ 0 ];
                ret = ( 7.605f * note / ( 0.01f * note + 6.5f ) ) / CDTXMania.DTX.nノーツ数[ 0 ];
                this.dbゲージ増加量[ 0 ] = ret;
                this.dbゲージ増加量[ 1 ] = ret / 2.0f;
                this.dbゲージ増加量[ 2 ] = 2.0f;

                //譜面分岐
                for( int i = 0; i < 3; i++ )
                {
                    note = CDTXMania.DTX.nノーツ数[ i ];
                    ret = ( 7.605f * note / ( 0.01f * note + 6.5f ) ) / CDTXMania.DTX.nノーツ数[ i ];
                    this.dbゲージ増加量_Branch[ i, 0 ] = ret;
                    this.dbゲージ増加量_Branch[ i, 1 ] = ret / 2.0f;
                    this.dbゲージ増加量_Branch[ i, 2 ] = 2.0f;
                }
            }
            else
            {
			    //ダメージ値の計算は太鼓の達人譜面Wikiのものを参考にしました。
                //ゲージのMAXまでの最低コンボ数を計算
                float dbGaugeMaxComboValue = 0;
                float[] dbGaugeMaxComboValue_branch = new float[3];
                float dbDamageRate = 2.0f;

                if( nRiskyTimes_InitialVal > 0 )
                {
                    this.bRisky = true;
                    this.nRiskyTimes = CDTXMania.ConfigIni.nRisky;
                    this.nRiskyTimes_Initial = CDTXMania.ConfigIni.nRisky;
                }

                switch( CDTXMania.DTX.LEVEL.Taiko )
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                        {
                            if (CDTXMania.DTX.bチップがある.Branch)
                            {
                                dbGaugeMaxComboValue = CDTXMania.DTX.nノーツ数[ 3 ] * ( this.fGaugeMaxRate[ 0 ] / 100.0f );
                                for( int i = 0; i < 3; i++ )
                                {
                                    dbGaugeMaxComboValue_branch[i] = CDTXMania.DTX.nノーツ数[i] * ( this.fGaugeMaxRate[ 0 ] / 100.0f );
                                }
                                dbDamageRate = 0.625f;
                            }
                            else
                            {
                                dbGaugeMaxComboValue = CDTXMania.DTX.nノーツ数[ 3 ] * ( this.fGaugeMaxRate[ 0 ] / 100.0f );
                                dbDamageRate = 0.625f;
                            }
                            break;
                        }


                    case 8:
                        {
                            if (CDTXMania.DTX.bチップがある.Branch)
                            {
                                dbGaugeMaxComboValue = CDTXMania.DTX.nノーツ数[ 3 ] * ( this.fGaugeMaxRate[ 1 ] / 100.0f );
                                for( int i = 0; i < 3; i++ )
                                {
                                    dbGaugeMaxComboValue_branch[i] = CDTXMania.DTX.nノーツ数[i] * ( this.fGaugeMaxRate[ 1 ] / 100.0f );
                                }
                                dbDamageRate = 0.625f;
                            }
                            else
                            {
                                dbGaugeMaxComboValue = CDTXMania.DTX.nノーツ数[ 3 ] * ( this.fGaugeMaxRate[ 1 ] / 100.0f );
                                dbDamageRate = 0.625f;
                            }
                            break;
                        }

                    case 9:
                    case 10:
                        {
                            if (CDTXMania.DTX.bチップがある.Branch)
                            {
                                dbGaugeMaxComboValue = CDTXMania.DTX.nノーツ数[ 3 ] * ( this.fGaugeMaxRate[ 2 ] / 100.0f );
                                for( int i = 0; i < 3; i++ )
                                {
                                    dbGaugeMaxComboValue_branch[i] = CDTXMania.DTX.nノーツ数[i] * ( this.fGaugeMaxRate[ 2 ] / 100.0f );
                                }
                            }
                            else
                            {
                                dbGaugeMaxComboValue = CDTXMania.DTX.nノーツ数[ 3 ] * ( this.fGaugeMaxRate[ 2 ] / 100.0f );
                            }
                            break;
                        }

                    default:
                        {
                            if (CDTXMania.DTX.bチップがある.Branch)
                            {
                                dbGaugeMaxComboValue = CDTXMania.DTX.nノーツ数[ 3 ] * ( this.fGaugeMaxRate[ 2 ] / 100.0f );
                                for( int i = 0; i < 3; i++ )
                                {
                                    dbGaugeMaxComboValue_branch[i] = CDTXMania.DTX.nノーツ数[i] * ( this.fGaugeMaxRate[ 2 ] / 100.0f );
                                }
                            }
                            else
                            {
                                dbGaugeMaxComboValue = CDTXMania.DTX.nノーツ数[ 3 ] * ( this.fGaugeMaxRate[ 2 ] / 100.0f );
                            }
                            break;
                        }
                }

                int nGaugeRankValue = (int)( Math.Floor( 10000.0f / dbGaugeMaxComboValue ) );
                int[] nGaugeRankValue_branch = new int[3];

                for (int i = 0; i < 3; i++ )
                {
                    nGaugeRankValue_branch[i] = (int)( Math.Floor( 10000.0f / dbGaugeMaxComboValue_branch[i] ) );
                }

                //ゲージ値計算
                //実機に近い計算

                this.dbゲージ増加量[ 0 ] = nGaugeRankValue / 100.0f;
                this.dbゲージ増加量[ 1 ] = ( nGaugeRankValue / 100.0f ) * 0.5f;
                this.dbゲージ増加量[ 2 ] = ( nGaugeRankValue / 100.0f ) * dbDamageRate;

                for (int i = 0; i < 3; i++ )
                {
                    this.dbゲージ増加量_Branch[ i, 0 ] = nGaugeRankValue_branch[i] / 100.0f;
                    this.dbゲージ増加量_Branch[ i, 1 ] = ( nGaugeRankValue_branch[i] / 100.0f ) * 0.5f;
                    this.dbゲージ増加量_Branch[ i, 2 ] = ( nGaugeRankValue_branch[i] / 100.0f ) * dbDamageRate;
                }

                //this.dbゲージ増加量[ 0 ] = CDTXMania.DTX.bチップがある.Branch ? ( 130.0 / CDTXMania.DTX.nノーツ数[ 0 ] ) : ( 130.0 / CDTXMania.DTX.nノーツ数[ 3 ] );
                //this.dbゲージ増加量[ 1 ] = CDTXMania.DTX.bチップがある.Branch ? ( 65.0 / CDTXMania.DTX.nノーツ数[ 0 ] ) : 65.0 / CDTXMania.DTX.nノーツ数[ 3 ];
                //this.dbゲージ増加量[ 2 ] = CDTXMania.DTX.bチップがある.Branch ? ( -260.0 / CDTXMania.DTX.nノーツ数[ 0 ] ) : -260.0 / CDTXMania.DTX.nノーツ数[ 3 ];

                //2015.03.26 kairera0467 計算を初期化時にするよう修正。
                this.dbゲージ増加量[ 0 ] = (float)Math.Truncate( this.dbゲージ増加量[ 0 ] * 10000.0f ) / 10000.0f;
                this.dbゲージ増加量[ 1 ] = (float)Math.Truncate( this.dbゲージ増加量[ 1 ] * 10000.0f ) / 10000.0f;
                this.dbゲージ増加量[ 2 ] = (float)Math.Truncate( this.dbゲージ増加量[ 2 ] * 10000.0f ) / 10000.0f;

                for (int i = 0; i < 3; i++ )
                {
                    this.dbゲージ増加量_Branch[ i, 0 ] = (float)Math.Truncate( this.dbゲージ増加量_Branch[ i, 0 ] * 10000.0f ) / 10000.0f;
                    this.dbゲージ増加量_Branch[ i, 1 ] = (float)Math.Truncate( this.dbゲージ増加量_Branch[ i, 1 ] * 10000.0f ) / 10000.0f;
                    this.dbゲージ増加量_Branch[ i, 2 ] = (float)Math.Truncate( this.dbゲージ増加量_Branch[ i, 2 ] * 10000.0f ) / 10000.0f;
                }
                
                #region[ あれ ]
                // HARDゲージみたいなアレ。BEMANIに比べてノーツ数が多くならないものが多いことを考えて、良判定時の増加量は通常の太鼓ゲージと同じです。
                if( CDTXMania.ConfigIni.eGaugeMode == Eゲージモード.HARD )
                {
                    //this.dbゲージ増加量[ 0 ] = 0.16f;
                    this.dbゲージ増加量[ 1 ] = 0.0f;
                    this.dbゲージ増加量[ 2 ] = 9.0f;

                    //譜面分岐
                    for( int i = 0; i < 3; i++ )
                    {
                        //this.dbゲージ増加量_Branch[ i, 0 ] = 0.16f;
                        this.dbゲージ増加量_Branch[ i, 1 ] = 0.0f;
                        this.dbゲージ増加量_Branch[ i, 2 ] = 9.0f;
                    }
                }
                else if( CDTXMania.ConfigIni.eGaugeMode == Eゲージモード.EXHARD )
                {
                    this.dbゲージ増加量[ 0 ] = 0.16f;
                    this.dbゲージ増加量[ 1 ] = 0.0f;
                    this.dbゲージ増加量[ 2 ] = 18.0f;

                    //譜面分岐
                    for( int i = 0; i < 3; i++ )
                    {
                        //this.dbゲージ増加量_Branch[ i, 0 ] = 0.16f;
                        this.dbゲージ増加量_Branch[ i, 1 ] = 0.0f;
                        this.dbゲージ増加量_Branch[ i, 2 ] = 18.0f;
                    }
                }
                else if( CDTXMania.ConfigIni.eGaugeMode == Eゲージモード.DEATH )
                {
                    // ミスしたらその時点でゲージが空になる。
                    // ゲージが満タンでも消せるようにするため100にしている。(もしダメージ演算結果が0以下になっても後から補正してくれます)
                    this.dbゲージ増加量[ 2 ] = 100;
                    this.dbゲージ増加量_Branch[ 0, 2 ] = 100;
                    this.dbゲージ増加量_Branch[ 1, 2 ] = 100;
                    this.dbゲージ増加量_Branch[ 2, 2 ] = 100;
                }
                #endregion
            }
        }

		#region [ DAMAGE ]
#if true		// DAMAGELEVELTUNING
		#region [ DAMAGELEVELTUNING ]
		// ----------------------------------
		public float[ , ] fDamageGaugeDelta = {			// #23625 2011.1.10 ickw_284: tuned damage/recover factors
			// drums,   guitar,  bass
			{  0.004f,  0.006f,  0.006f,  0.004f },
			{  0.002f,  0.003f,  0.003f,  0.002f },
			{  0.000f,  0.000f,  0.000f,  0.000f },
			{ -0.020f, -0.030f,	-0.030f, -0.020f },
			{ -0.050f, -0.050f, -0.050f, -0.050f }
		};
		public float[] fDamageLevelFactor = {
			0.5f, 1.0f, 1.5f
		};

        public float[] dbゲージ増加量 = new float[ 3 ];

        //譜面レベル, 判定
        public float[,] dbゲージ増加量_Branch = new float[3, 3];


        public float[] fGaugeMaxRate = 
        {
            70.7f,//1～7
            70f,  //8
            75.0f //9～10
        };//おおよその値。

		// ----------------------------------
#endregion
#endif

		public void Damage( E楽器パート screenmode, E楽器パート part, E判定 e今回の判定, int player )
		{
			float fDamage;
            int nコース = (int)CDTXMania.stage演奏ドラム画面.n現在のコース[ player ];

            switch ( e今回の判定 )
			{
				case E判定.Perfect:
				case E判定.Great:
                    {
                        if( CDTXMania.DTX.bチップがある.Branch )
                        {
                            fDamage = this.dbゲージ増加量_Branch[ nコース, 0 ];
                        }
                        else
					        fDamage = this.dbゲージ増加量[ 0 ];
                    }
                    break;
				case E判定.Good:
                    {
                        if( CDTXMania.DTX.bチップがある.Branch )
                        {
                            fDamage = this.dbゲージ増加量_Branch[ nコース, 1 ];
                        }
                        else
					        fDamage = this.dbゲージ増加量[ 1 ];
                    }
					break;
				case E判定.Poor:
				case E判定.Miss:
                    {
                        if( CDTXMania.DTX.bチップがある.Branch )
                        {
                            fDamage = this.dbゲージ増加量_Branch[ nコース, 2 ];
                        }
                        else
					        fDamage = this.dbゲージ増加量[ 2 ];

                        if( this.db現在のゲージ値[ player ] < 30.0f && CDTXMania.ConfigIni.eGaugeMode == Eゲージモード.HARD )
                        {
                            //HARDの場合30%補正
                            fDamage = 4.5f;
                        }
                        
                        if( fDamage >= 0 )
                        {
                            fDamage = -fDamage;
                        }
                    }
					break;

				default:
                    {
                        if( player == 0 ? CDTXMania.ConfigIni.b太鼓パートAutoPlay : CDTXMania.ConfigIni.b太鼓パートAutoPlay2P )
                        {
                            if( CDTXMania.DTX.bチップがある.Branch )
                            {
                                fDamage = this.dbゲージ増加量_Branch[ nコース, 0 ];
                            }
                            else
					            fDamage = this.dbゲージ増加量[ 0 ];
                        }
                        else
                            fDamage = 0;
					    break;
                    }
            }
			if( this.db現在のゲージ値[ player ] > 100.0 )
				this.db現在のゲージ値[ player ] = 100.0;
            else if( this.db現在のゲージ値[ player ] < 0.0 )
                this.db現在のゲージ値[ player ] = 0.0;

            this.db現在のゲージ値[ player ] = Math.Round(this.db現在のゲージ値[ player ] + fDamage, 5, MidpointRounding.ToEven);
            CDTXMania.stage演奏ドラム画面.nGauge = fDamage;

            switch( e今回の判定 )
            {
                case E判定.Poor:
                case E判定.Miss:
                    if( this.bRisky )
                    {
                        this.nRiskyTimes--;
                    }
                    break;
            }
	    }

        public virtual void Start(int nLane, E判定 judge, int player)
        {
        }

        public virtual void Stop( int player )
        {

        }

		//-----------------
		#endregion

		public double[] db現在のゲージ値 = new double[ 4 ];
        protected CCounter ct炎;
        protected CCounter ct虹アニメ;
		protected CTexture txゲージ;
        protected CTexture txゲージ背景;
		protected CTexture txゲージ2P;
        protected CTexture txゲージ背景2P;
        protected CTexture tx魂;
        protected CTexture tx炎;
        protected CTexture tx魂花火;

        protected CTexture[] txゲージ虹 = new CTexture[ 12 ];
        protected CTexture[] txゲージ虹2P = new CTexture[ 12 ];
        protected CTexture txゲージ線;
        protected CTexture txゲージ線2P;
	}
}
