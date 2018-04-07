using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using DirectShowLib;
using DirectShowLib.DES;
using SharpDX;

namespace FDK
{
	public class CAviDS : IDisposable
	{
		const int timeOutMs = 1000; // グラフ state の遷移完了を待つタイムアウト期間 

		public uint nフレーム高さ
		{
			get
			{
				return (uint)nHeight;
			}
		}

		public uint nフレーム幅
		{
			get
			{
				return (uint)nWidth;
			}
		}
		public bool b再生中
		{
			get
			{
				return bPlaying;
			}
		}
		public bool b一時停止中
		{
			get
			{
				return bPause;
			}
		}

		int nWidth;
		int nHeight;
		long nMediaLength; // [ms]
		bool bPlaying;
		bool bPause;

		public int GetDuration()
		{
			return (int)(nMediaLength / 10000);
		}

		IGraphBuilder builder;
		VideoInfoHeader videoInfo;
		ISampleGrabber grabber;
		IMediaControl control;
		IMediaSeeking seeker;
		IMediaFilter filter;
		FilterState state;
		AMMediaType mediaType;
		IntPtr samplePtr = IntPtr.Zero;
		
		public CAviDS(string filename, double playSpeed)
		{
			int hr = 0x0;

			builder = (IGraphBuilder)new FilterGraph();

			#region [Sample Grabber]
			{
				grabber = new SampleGrabber() as ISampleGrabber;
				mediaType = new AMMediaType();
				mediaType.majorType = MediaType.Video;
				mediaType.subType = MediaSubType.RGB32;
				mediaType.formatType = FormatType.VideoInfo;
				hr = grabber.SetMediaType(mediaType);
				DsError.ThrowExceptionForHR(hr);

				hr = builder.AddFilter((IBaseFilter)grabber, "Sample Grabber");
				DsError.ThrowExceptionForHR(hr);
			}
			#endregion

			hr = builder.RenderFile(filename, null);
			DsError.ThrowExceptionForHR(hr);

			// Null レンダラに接続しないとウィンドウが表示される。
			// また、レンダリングを行わないため処理速度を向上できる。
			CDirectShow.ConnectNullRendererFromSampleGrabber(builder, grabber as IBaseFilter);
			CDirectShow.tグラフを解析しデバッグ出力する(builder);

			IVideoWindow videoWindow = builder as IVideoWindow;
			if (videoWindow != null)
			{
				videoWindow.put_AutoShow(OABool.False);
			}

			#region [Video Info]
			{
				hr = grabber.GetConnectedMediaType(mediaType);
				DsError.ThrowExceptionForHR(hr);

				videoInfo = (VideoInfoHeader)Marshal.PtrToStructure(mediaType.formatPtr, typeof(VideoInfoHeader));
				nWidth = videoInfo.BmiHeader.Width;
				nHeight = videoInfo.BmiHeader.Height;
			}
			#endregion

			#region[ Seeker ]
			{
				seeker = builder as IMediaSeeking;
				hr = seeker.GetDuration(out nMediaLength);
				DsError.ThrowExceptionForHR(hr);
				hr = seeker.SetRate(playSpeed / 20);
				DsError.ThrowExceptionForHR(hr);
			}
			#endregion

			#region [Control]
			{
				control = builder as IMediaControl;
			}
			#endregion

			#region [Filter]
			{
				filter = builder as IMediaFilter;
			}
			#endregion

			grabber.SetBufferSamples(true);
			this.Run();
			this.Pause();

			bPlaying = false;
			bPause = false;		// 外見えには演奏停止している。PAUSE中として外に見せないこと。
		}

		public void Seek(int timeInMs)
		{
			int hr = seeker.SetPositions(new DsLong(timeInMs * 10000), AMSeekingSeekingFlags.AbsolutePositioning, null, AMSeekingSeekingFlags.NoPositioning);
			DsError.ThrowExceptionForHR(hr);
			hr = control.GetState(timeOutMs, out state); // state is Running
			DsError.ThrowExceptionForHR(hr);
		}

		public void Run()
		{
			int hr = control.Run();
			DsError.ThrowExceptionForHR(hr);
			hr = control.GetState(timeOutMs, out state);
			DsError.ThrowExceptionForHR(hr);
			bPlaying = true;
			bPause = false;
		}

		public void Stop()
		{
			int hr = control.Stop();
			DsError.ThrowExceptionForHR(hr);
			hr = control.GetState(timeOutMs, out state);
			DsError.ThrowExceptionForHR(hr);
			bPlaying = false;
			bPause = false;
		}

		public void Pause()
		{
			int hr = control.Pause();
			DsError.ThrowExceptionForHR(hr);
			hr = control.GetState(timeOutMs, out state);
			DsError.ThrowExceptionForHR(hr);
			bPause = true;
		}

		public void ToggleRun()
		{
			int hr = control.GetState(timeOutMs, out state);
			DsError.ThrowExceptionForHR(hr);
			if( state == FilterState.Paused )
			{
				Run();
			}
			else if( state == FilterState.Running )
			{
				Pause();
			}
		}

		public unsafe void tGetBitmap(SharpDX.Direct3D9.Device device, CTexture ctex, int timeMs)
		{
			int bufferSize = 0;
			int hr = 0x0;
			
			hr = grabber.GetCurrentBuffer(ref bufferSize, IntPtr.Zero);
			DsError.ThrowExceptionForHR(hr);

			if ( samplePtr == IntPtr.Zero )
			{
				samplePtr = Marshal.AllocHGlobal(bufferSize);
			}

			DataRectangle rectangle3 = ctex.texture.LockRectangle(0, SharpDX.Direct3D9.LockFlags.None);
			hr = grabber.GetCurrentBuffer(ref bufferSize, rectangle3.DataPointer);
			DsError.ThrowExceptionForHR(hr);
			ctex.texture.UnlockRectangle(0);
		}

		#region [ Dispose-Finalize パターン実装 ]
		public void Dispose()
		{
			if (!this.bDisposed)
			{
				if (null != builder)
				{
					Marshal.ReleaseComObject(builder);
					builder = null;
				}
				if( null != grabber )
				{
					Marshal.ReleaseComObject(grabber);
					grabber = null;
				}
				if (null != mediaType)
				{
					DsUtils.FreeAMMediaType(mediaType);
					mediaType = null;
				}
				if (samplePtr != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(samplePtr);
				}

				GC.SuppressFinalize(this);
				this.bDisposed = true;
			}
		}

		~CAviDS()
		{
			this.Dispose();
		}
		#endregion

		private bool bDisposed = false;
	}
}
