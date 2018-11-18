using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using SlimDX;
using SlimDX.DirectInput;

namespace FDK
{
	public class CInputJoystick : IInputDevice, IDisposable
	{
		// コンストラクタ

		public CInputJoystick( IntPtr hWnd, DeviceInstance di, DirectInput directInput )
		{
			this.e入力デバイス種別 = E入力デバイス種別.Joystick;
			this.GUID = di.InstanceGuid.ToString();
			this.ID = 0;
			try
			{
				this.devJoystick = new Joystick( directInput, di.InstanceGuid );
				this.devJoystick.SetCooperativeLevel( hWnd, CooperativeLevel.Foreground | CooperativeLevel.Exclusive );
				this.devJoystick.Properties.BufferSize = _rawBufferedDataArray.Length;
				Trace.TraceInformation( this.devJoystick.Information.InstanceName + "を生成しました。" );
			}
			catch( DirectInputException )
			{
				if( this.devJoystick != null )
				{
					this.devJoystick.Dispose();
					this.devJoystick = null;
				}
				Trace.TraceError( this.devJoystick.Information.InstanceName, new object[] { " の生成に失敗しました。" } );
				throw;
			}
			foreach( DeviceObjectInstance instance in this.devJoystick.GetObjects() )
			{
				if( ( instance.ObjectType & ObjectDeviceType.Axis ) != ObjectDeviceType.All )
				{
					this.devJoystick.GetObjectPropertiesById( (int) instance.ObjectType ).SetRange( -1000, 1000 );
					this.devJoystick.GetObjectPropertiesById( (int) instance.ObjectType ).DeadZone = 5000;		// 50%をデッドゾーンに設定
																												// 軸をON/OFFの2値で使うならこれで十分
				}
			}
			try
			{
				this.devJoystick.Acquire();
			}
			catch( DirectInputException e )
			{
				Trace.TraceError( e.ToString() );
				Trace.TraceError( "例外が発生しましたが処理を継続します。 (5658360e-2745-4c5c-ab2d-ff82287d3e8a)" );
			}

			for( int i = 0; i < this.bButtonState.Length; i++ )
				this.bButtonState[ i ] = false;
			for ( int i = 0; i < this.nPovState.Length; i++ )
				this.nPovState[ i ] = -1;

			//this.timer = new CTimer( CTimer.E種別.MultiMedia );

			this.list入力イベント = new List<STInputEvent>( 32 );
		}
		
		
		// メソッド
		
		public void SetID( int nID )
		{
			this.ID = nID;
		}

		#region [ IInputDevice 実装 ]
		//-----------------
		public E入力デバイス種別 e入力デバイス種別
		{ 
			get;
			private set;
		}
		public string GUID
		{
			get;
			private set;
		}
		public int ID
		{ 
			get; 
			private set;
		}
		public List<STInputEvent> list入力イベント 
		{
			get;
			private set; 
		}

		public void tポーリング( bool bWindowがアクティブ中, bool bバッファ入力を使用する )
		{
			#region [ bButtonフラグ初期化 ]
			for ( int i = 0; i < 256; i++ )
			{
				this.bButtonPushDown[ i ] = false;
				this.bButtonPullUp[ i ] = false;
			}
			#endregion

			if ( ( bWindowがアクティブ中 && !this.devJoystick.Acquire().IsFailure ) && !this.devJoystick.Poll().IsFailure )
			{
				// this.list入力イベント = new List<STInputEvent>( 32 );
				this.list入力イベント.Clear();						// #xxxxx 2012.6.11 yyagi; To optimize, I removed new();


				if( bバッファ入力を使用する )
				{
					#region [ a.バッファ入力 ]
					//-----------------------------

                    var length = this.devJoystick.GetDeviceData(_rawBufferedDataArray, false);
                    if (!Result.Last.IsSuccess)
                    {
                        return;
                    }
                    for (int i = 0; i < length; i++)
                    {
                        var rawBufferedData = _rawBufferedDataArray[i];

                        switch (rawBufferedData.Offset)
                        {
                            case 0:
                                #region [ X軸－ ]
                                //-----------------------------
                                bButtonUpDown( rawBufferedData, rawBufferedData.Data, 0, 1 );
                                //-----------------------------
                                #endregion
                                #region [ X軸＋ ]
                                //-----------------------------
                                bButtonUpDown( rawBufferedData, rawBufferedData.Data, 1, 0 );
                                //-----------------------------
                                #endregion
                                break;
                            case 4:
                                #region [ Y軸－ ]
                                //-----------------------------
                                bButtonUpDown( rawBufferedData, rawBufferedData.Data, 2, 3 );
                                //-----------------------------
                                #endregion
                                #region [ Y軸＋ ]
                                //-----------------------------
                                bButtonUpDown( rawBufferedData, rawBufferedData.Data, 3, 2 );
                                #endregion
                                break;
                            case 8:
                                #region [ Z軸－ ]
                                //-----------------------------
                                bButtonUpDown( rawBufferedData, rawBufferedData.Data, 4, 5 );
                                //-----------------------------
                                #endregion
                                #region [ Z軸＋ ]
                                //-----------------------------
                                bButtonUpDown( rawBufferedData, rawBufferedData.Data, 5, 4 );
                                #endregion
                                break;
                            case 32:
                                const int p = 0; // const until we support more than one POV hat
                                int nPovDegree = rawBufferedData.Data;
                                int nWay = ( nPovDegree + 2250 ) / 4500;
                                if ( nWay == 8 ) nWay = 0;

                                STInputEvent e = new STInputEvent();
                                if ( nPovDegree == -1 || nPovDegree == 0xFFFF)
                                {
                                    e.nKey = 6 + 128 + this.nPovState[ p ];
                                    this.nPovState[ p ] = -1;
                                    e.b押された = false;
                                    this.bButtonState[ e.nKey ] = false;
                                    this.bButtonPullUp[ e.nKey ] = true;
                                } else {
                                    this.nPovState[ p ] = nWay;
                                    e.nKey = 6 + 128 + nWay;
                                    e.b押された = true;
                                    this.bButtonState[ e.nKey ] = true;
                                    this.bButtonPushDown[ e.nKey ] = true;
                                }
                                e.nTimeStamp = CSound管理.rc演奏用タイマ.nサウンドタイマーのシステム時刻msへの変換( rawBufferedData.Timestamp );
                                this.list入力イベント.Add( e );
                                break;
                            default:
                                var buttonIndex = rawBufferedData.Offset - 48;
                                if (-1 < buttonIndex && buttonIndex < 32)
                                {
                                    var key = 6 + buttonIndex;
                                    var wasPressed = (rawBufferedData.Data & 128) == 128;

                                    STInputEvent item = new STInputEvent()
                                    {
                                        nKey = key,
                                        b押された = wasPressed,
                                        nTimeStamp = CSound管理.rc演奏用タイマ.nサウンドタイマーのシステム時刻msへの変換(rawBufferedData.Timestamp),
                                    };
                                    this.list入力イベント.Add(item);

                                    this.bButtonState[item.nKey] = wasPressed;
                                    this.bButtonPushDown[item.nKey] = wasPressed;
                                    this.bButtonPullUp[item.nKey] = !wasPressed;
                                }

                                break;
                        }
                    }

					//-----------------------------
					#endregion
				}
				else
				{
					#region [ b.状態入力 ]
					//-----------------------------
					JoystickState currentState = this.devJoystick.GetCurrentState();
					if( Result.Last.IsSuccess && currentState != null )
					{
						#region [ X軸－ ]
						//-----------------------------
						if( currentState.X < -500 )
						{
							if( this.bButtonState[ 0 ] == false )
							{
								STInputEvent ev = new STInputEvent()
								{
									nKey = 0,
									b押された = true,
									nTimeStamp = CSound管理.rc演奏用タイマ.nシステム時刻,	// 演奏用タイマと同じタイマを使うことで、BGMと譜面、入力ずれを防ぐ。
								};
								this.list入力イベント.Add( ev );

								this.bButtonState[ 0 ] = true;
								this.bButtonPushDown[ 0 ] = true;
							}
						}
						else
						{
							if( this.bButtonState[ 0 ] == true )
							{
								STInputEvent ev = new STInputEvent()
								{
									nKey = 0,
									b押された = false,
									nTimeStamp = CSound管理.rc演奏用タイマ.nシステム時刻,	// 演奏用タイマと同じタイマを使うことで、BGMと譜面、入力ずれを防ぐ。
								};
								this.list入力イベント.Add( ev );

								this.bButtonState[ 0 ] = false;
								this.bButtonPullUp[ 0 ] = true;
							}
						}
						//-----------------------------
						#endregion
						#region [ X軸＋ ]
						//-----------------------------
						if( currentState.X > 500 )
						{
							if( this.bButtonState[ 1 ] == false )
							{
								STInputEvent ev = new STInputEvent()
								{
									nKey = 1,
									b押された = true,
									nTimeStamp = CSound管理.rc演奏用タイマ.nシステム時刻,	// 演奏用タイマと同じタイマを使うことで、BGMと譜面、入力ずれを防ぐ。
								};
								this.list入力イベント.Add( ev );

								this.bButtonState[ 1 ] = true;
								this.bButtonPushDown[ 1 ] = true;
							}
						}
						else
						{
							if( this.bButtonState[ 1 ] == true )
							{
								STInputEvent event7 = new STInputEvent()
								{
									nKey = 1,
									b押された = false,
									nTimeStamp = CSound管理.rc演奏用タイマ.nシステム時刻,	// 演奏用タイマと同じタイマを使うことで、BGMと譜面、入力ずれを防ぐ。
								};
								this.list入力イベント.Add( event7 );

								this.bButtonState[ 1 ] = false;
								this.bButtonPullUp[ 1 ] = true;
							}
						}
						//-----------------------------
						#endregion
						#region [ Y軸－ ]
						//-----------------------------
						if( currentState.Y < -500 )
						{
							if( this.bButtonState[ 2 ] == false )
							{
								STInputEvent ev = new STInputEvent()
								{
									nKey = 2,
									b押された = true,
									nTimeStamp = CSound管理.rc演奏用タイマ.nシステム時刻,	// 演奏用タイマと同じタイマを使うことで、BGMと譜面、入力ずれを防ぐ。
								};
								this.list入力イベント.Add( ev );

								this.bButtonState[ 2 ] = true;
								this.bButtonPushDown[ 2 ] = true;
							}
						}
						else
						{
							if( this.bButtonState[ 2 ] == true )
							{
								STInputEvent ev = new STInputEvent()
								{
									nKey = 2,
									b押された = false,
									nTimeStamp = CSound管理.rc演奏用タイマ.nシステム時刻,	// 演奏用タイマと同じタイマを使うことで、BGMと譜面、入力ずれを防ぐ。
								};
								this.list入力イベント.Add( ev );

								this.bButtonState[ 2 ] = false;
								this.bButtonPullUp[ 2 ] = true;
							}
						}
						//-----------------------------
						#endregion
						#region [ Y軸＋ ]
						//-----------------------------
						if( currentState.Y > 500 )
						{
							if( this.bButtonState[ 3 ] == false )
							{
								STInputEvent ev = new STInputEvent()
								{
									nKey = 3,
									b押された = true,
									nTimeStamp = CSound管理.rc演奏用タイマ.nシステム時刻,	// 演奏用タイマと同じタイマを使うことで、BGMと譜面、入力ずれを防ぐ。
								};
								this.list入力イベント.Add( ev );

								this.bButtonState[ 3 ] = true;
								this.bButtonPushDown[ 3 ] = true;
							}
						}
						else
						{
							if( this.bButtonState[ 3 ] == true )
							{
								STInputEvent ev = new STInputEvent()
								{
									nKey = 3,
									b押された = false,
									nTimeStamp = CSound管理.rc演奏用タイマ.nシステム時刻,	// 演奏用タイマと同じタイマを使うことで、BGMと譜面、入力ずれを防ぐ。
								};
								this.list入力イベント.Add( ev );

								this.bButtonState[ 3 ] = false;
								this.bButtonPullUp[ 3 ] = true;
							}
						}
						//-----------------------------
						#endregion
						#region [ Z軸－ ]
						//-----------------------------
						if( currentState.Z < -500 )
						{
							if( this.bButtonState[ 4 ] == false )
							{
								STInputEvent ev = new STInputEvent()
								{
									nKey = 4,
									b押された = true,
									nTimeStamp = CSound管理.rc演奏用タイマ.nシステム時刻,	// 演奏用タイマと同じタイマを使うことで、BGMと譜面、入力ずれを防ぐ。
								};
								this.list入力イベント.Add( ev );

								this.bButtonState[ 4 ] = true;
								this.bButtonPushDown[ 4 ] = true;
							}
						}
						else
						{
							if( this.bButtonState[ 4 ] == true )
							{
								STInputEvent ev = new STInputEvent()
								{
									nKey = 4,
									b押された = false,
									nTimeStamp = CSound管理.rc演奏用タイマ.nシステム時刻,	// 演奏用タイマと同じタイマを使うことで、BGMと譜面、入力ずれを防ぐ。
								};
								this.list入力イベント.Add( ev );

								this.bButtonState[ 4 ] = false;
								this.bButtonPullUp[ 4 ] = true;
							}
						}
						//-----------------------------
						#endregion
						#region [ Z軸＋ ]
						//-----------------------------
						if( currentState.Z > 500 )
						{
							if( this.bButtonState[ 5 ] == false )
							{
								STInputEvent ev = new STInputEvent()
								{
									nKey = 5,
									b押された = true,
									nTimeStamp = CSound管理.rc演奏用タイマ.nシステム時刻,	// 演奏用タイマと同じタイマを使うことで、BGMと譜面、入力ずれを防ぐ。
								};
								this.list入力イベント.Add( ev );

								this.bButtonState[ 5 ] = true;
								this.bButtonPushDown[ 5 ] = true;
							}
						}
						else
						{
							if( this.bButtonState[ 5 ] == true )
							{
								STInputEvent event15 = new STInputEvent()
								{
									nKey = 5,
									b押された = false,
									nTimeStamp = CSound管理.rc演奏用タイマ.nシステム時刻,	// 演奏用タイマと同じタイマを使うことで、BGMと譜面、入力ずれを防ぐ。
								};
								this.list入力イベント.Add( event15 );

								this.bButtonState[ 5 ] = false;
								this.bButtonPullUp[ 5 ] = true;
							}
						}
						//-----------------------------
						#endregion
						#region [ ボタン ]
						//-----------------------------
						bool bIsButtonPressedReleased = false;
						bool[] buttons = currentState.GetButtons();
						for( int j = 0; ( j < buttons.Length ) && ( j < 128 ); j++ )
						{
							if( this.bButtonState[ 6 + j ] == false && buttons[ j ] )
							{
								STInputEvent item = new STInputEvent()
								{
									nKey = 6 + j,
									b押された = true,
									nTimeStamp = CSound管理.rc演奏用タイマ.nシステム時刻,	// 演奏用タイマと同じタイマを使うことで、BGMと譜面、入力ずれを防ぐ。
								};
								this.list入力イベント.Add( item );

								this.bButtonState[ 6 + j ] = true;
								this.bButtonPushDown[ 6 + j ] = true;
								bIsButtonPressedReleased = true;
							}
							else if( this.bButtonState[ 6 + j ] == true && !buttons[ j ] )
							{
								STInputEvent item = new STInputEvent()
								{
									nKey = 6 + j,
									b押された = false,
									nTimeStamp = CSound管理.rc演奏用タイマ.nシステム時刻,	// 演奏用タイマと同じタイマを使うことで、BGMと譜面、入力ずれを防ぐ。
								};
								this.list入力イベント.Add( item );

								this.bButtonState[ 6 + j ] = false;
								this.bButtonPullUp[ 6 + j ] = true;
								bIsButtonPressedReleased = true;
							}
						}
						//-----------------------------
						#endregion
						// #24341 2011.3.12 yyagi: POV support
						#region [ POV HAT 4/8way (only single POV switch is supported)]
						int[] povs = currentState.GetPointOfViewControllers();
						if ( povs != null )
						{
							if ( povs[ 0 ] >= 0 )
							{
								int nPovDegree = povs[ 0 ];
								int nWay = ( nPovDegree + 2250 ) / 4500;
								if ( nWay == 8 ) nWay = 0;

								if ( this.bButtonState[ 6 + 128 + nWay ] == false )
								{
									STInputEvent stevent = new STInputEvent()
									{
										nKey = 6 + 128 + nWay,
										//Debug.WriteLine( "POVS:" + povs[ 0 ].ToString( CultureInfo.CurrentCulture ) + ", " +stevent.nKey );
										b押された = true,
										nTimeStamp = CSound管理.rc演奏用タイマ.nシステム時刻,	// 演奏用タイマと同じタイマを使うことで、BGMと譜面、入力ずれを防ぐ。
									};
									this.list入力イベント.Add( stevent );

									this.bButtonState[ stevent.nKey ] = true;
									this.bButtonPushDown[ stevent.nKey ] = true;
								}
							}
							else if ( bIsButtonPressedReleased == false )	// #xxxxx 2011.12.3 yyagi 他のボタンが何も押され/離されてない＝POVが離された
							{
								int nWay = 0;
								for ( int i = 6 + 0x80; i < 6 + 0x80 + 8; i++ )
								{											// 離されたボタンを調べるために、元々押されていたボタンを探す。
									if ( this.bButtonState[ i ] == true )	// DirectInputを直接いじるならこんなことしなくて良いのに、あぁ面倒。
									{										// この処理が必要なために、POVを1個しかサポートできない。無念。
										nWay = i;
										break;
									}
								}
								if ( nWay != 0 )
								{
									STInputEvent stevent = new STInputEvent()
									{
										nKey = nWay,
										b押された = false,
										nTimeStamp = CSound管理.rc演奏用タイマ.nシステム時刻,	// 演奏用タイマと同じタイマを使うことで、BGMと譜面、入力ずれを防ぐ。
									};
									this.list入力イベント.Add( stevent );

									this.bButtonState[ nWay ] = false;
									this.bButtonPullUp[ nWay ] = true;
								}
							}
						}
						#endregion
					}
					//-----------------------------
					#endregion
				}
			}
		}

		public bool bキーが押された( int nButton )
		{
			return this.bButtonPushDown[ nButton ];
		}
		public bool bキーが押されている( int nButton )
		{
			return this.bButtonState[ nButton ];
		}
		public bool bキーが離された( int nButton )
		{
			return this.bButtonPullUp[ nButton ];
		}
		public bool bキーが離されている( int nButton )
		{
			return !this.bButtonState[ nButton ];
		}
		//-----------------
		#endregion

		#region [ IDisposable 実装 ]
		//-----------------
		public void Dispose()
		{
			if( !this.bDispose完了済み )
			{
				if( this.devJoystick != null )
				{
					this.devJoystick.Dispose();
					this.devJoystick = null;
				}
				//if( this.timer != null )
				//{
				//    this.timer.Dispose();
				//    this.timer = null;
				//}
				if ( this.list入力イベント != null )
				{
					this.list入力イベント = null;
				}
				this.bDispose完了済み = true;
			}
		}
		//-----------------
		#endregion


		// その他

		#region [ private ]
		//-----------------
	    private readonly RawBufferedData[] _rawBufferedDataArray = new RawBufferedData[256];
		private readonly bool[] bButtonPullUp = new bool[ 0x100 ];
		private readonly bool[] bButtonPushDown = new bool[ 0x100 ];
		private readonly bool[] bButtonState = new bool[ 0x100 ];		// 0-5: XYZ, 6 - 0x128+5: buttons, 0x128+6 - 0x128+6+8: POV/HAT
		private readonly int[] nPovState = new int[ 4 ];					// POVの現在値を保持

		private bool bDispose完了済み;
		private Joystick devJoystick;

	    //private CTimer timer;

		private void bButtonUpDown( RawBufferedData data, int axisdata, int target, int contrary )	// #26871 2011.12.3 軸の反転に対応するためにリファクタ
		{
			int targetsign = ( target < contrary ) ? -1 : 1;
			if ( Math.Abs( axisdata ) > 500 && ( targetsign == Math.Sign( axisdata ) ) )			// 軸の最大値の半分を超えていて、かつ
			{
				if ( bDoUpDownCore( target, data, false ) )											// 直前までは超えていなければ、今回ON
				{
					//Debug.WriteLine( "X-ON " + data.TimeStamp + " " + axisdata );
				}
				else
				{
					//Debug.WriteLine( "X-ONx " + data.TimeStamp + " " + axisdata );
				}
				bDoUpDownCore( contrary, data, true );												// X軸+ == ON から X軸-のONレンジに来たら、X軸+はOFF
			}
			else if ( ( axisdata <= 0 && targetsign <= 0 ) || ( axisdata >= 0 && targetsign >= 0 ) )	// 軸の最大値の半分を超えておらず、かつ  
			{
				//Debug.WriteLine( "X-OFF? " + data.TimeStamp + " " + axisdata );
				if ( bDoUpDownCore( target, data, true ) )											// 直前までは超えていたのならば、今回OFF
				{
					//Debug.WriteLine( "X-OFF " + data.TimeStamp + " " + axisdata );
				}
				else if ( bDoUpDownCore( contrary, data, true ) )									// X軸+ == ON から X軸-のOFFレンジにきたら、X軸+はOFF
				{
					//Debug.WriteLine( "X-OFFx " + data.TimeStamp + " " + axisdata );
				}
			}
		}

		/// <summary>
		/// 必要に応じて軸ボタンの上げ下げイベントを発生する
		/// </summary>
		/// <param name="target">軸ボタン番号 0=-X 1=+X ... 5=+Z</param>
		/// <param name="data"></param>
		/// <param name="currentMode">直前のボタン状態 true=押されていた</param>
		/// <returns>上げ下げイベント発生時true</returns>
		private bool bDoUpDownCore( int target, RawBufferedData data, bool lastMode )
		{
			if ( this.bButtonState[ target ] == lastMode )
			{
				STInputEvent e = new STInputEvent()
				{
					nKey = target,
					b押された = !lastMode,
					nTimeStamp = CSound管理.rc演奏用タイマ.nサウンドタイマーのシステム時刻msへの変換( data.Timestamp ),
				};
				this.list入力イベント.Add( e );

				this.bButtonState[ target ] = !lastMode;
				if ( lastMode )
				{
					this.bButtonPullUp[ target ] = true;
				}
				else
				{
					this.bButtonPushDown[ target ] = true;
				}
				return true;
			}
			return false;
		}
		//-----------------
		#endregion
	}
}
