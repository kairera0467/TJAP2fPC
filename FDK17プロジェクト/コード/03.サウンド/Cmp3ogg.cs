using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;
using System.Threading;
using Un4seen.Bass;
using Un4seen.Bass.Misc;


namespace FDK
{
	public unsafe class Cmp3ogg : SoundDecoder
	{
		CWin32.WAVEFORMATEX _wfx;
		long _TotalPCMSize = 0;
		private int stream_in = -1;
		byte[] wavdata = null;


		public override int Open( string filename )
		{
			bool r = Bass.BASS_Init(0, 48000, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero);

			stream_in = Bass.BASS_StreamCreateFile(filename, 0, 0, BASSFlag.BASS_DEFAULT | BASSFlag.BASS_STREAM_DECODE);
			// BASS_DEFAULT: output 32bit (16bit stereo)
			if (stream_in == 0)
			{
				BASSError be = Bass.BASS_ErrorGetCode();
				Trace.TraceInformation("Cmp3: streamcreatefile error: " + be.ToString());
			}
			_TotalPCMSize = Bass.BASS_ChannelGetLength(stream_in);


//string fn = Path.GetFileName(filename); 
//Trace.TraceInformation("filename=" + fn + ", size=(decode): " + wavdata.Length + ", channelgetlength=" + _TotalPCMSize2 + ", " + _TotalPCMSize) ;

			return 1;
		}
		public override int GetFormat( int nHandle, ref CWin32.WAVEFORMATEX wfx )
		{
			var chinfo = Bass.BASS_ChannelGetInfo(stream_in);
			_wfx.wFormatTag = 1;                                        // 1 == PCM
			_wfx.nChannels = (ushort)chinfo.chans;                  //
			_wfx.nSamplesPerSec = (uint)chinfo.freq;                        //
			_wfx.nAvgBytesPerSec = (uint)(chinfo.freq * 2 * chinfo.chans);  //
			_wfx.nBlockAlign = (ushort)(2 * chinfo.chans);              // 16bit * mono/stereo
			_wfx.wBitsPerSample = 16;

			//Debug.WriteLine("**WAVEFORMATEX** in Cmp3.cs from stream info");
			//Debug.WriteLine("wFormatTag=      " + 1);
			//Debug.WriteLine("nChannels =      " + chinfo.chans.ToString("X4"));
			//Debug.WriteLine("nSamplesPerSec=  " + chinfo.freq.ToString("X8"));
			//Debug.WriteLine("nAvgBytesPerSec= " + (chinfo.freq * 4).ToString("X8"));
			//Debug.WriteLine("nBlockAlign=     " + (2 * chinfo.chans).ToString("X4"));
			//Debug.WriteLine("wBitsPerSample=  " + (16).ToString("X4"));

			wfx = _wfx;

			#region [ Debug info ]
			//Debug.WriteLine("**WAVEFORMATEX** in Cmp3.cs from binary array");
			//Debug.WriteLine("wFormatTag=      " + wfx.wFormatTag.ToString("X4"));
			//Debug.WriteLine("nChannels =      " + wfx.nChannels.ToString("X4"));
			//Debug.WriteLine("nSamplesPerSec=  " + wfx.nSamplesPerSec.ToString("X8"));
			//Debug.WriteLine("nAvgBytesPerSec= " + wfx.nAvgBytesPerSec.ToString("X8"));
			//Debug.WriteLine("nBlockAlign=     " + wfx.nBlockAlign.ToString("X4"));
			//Debug.WriteLine("wBitsPerSample=  " + wfx.wBitsPerSample.ToString("X4"));
			//Debug.WriteLine("cbSize=          " + wfx.cbSize.ToString("X4"));
			#endregion

			return 0;
		}
		public override uint GetTotalPCMSize( int nHandle )
		{
			return (uint)_TotalPCMSize;

			// return mp3GetTotalPCMSize( nHandle );
		}
		public override int Seek( int nHandle, uint dwPosition )
		{
			return 0;
			//return mp3Seek( nHandle, dwPosition );
		}
		public override int Decode( int nHandle, IntPtr pDest, uint szDestSize, int bLoop )
		{
			var ms_out = new MemoryStream();
			var bw_out = new BinaryWriter(ms_out);

			#region [ decode ]
			int LEN = 65536;
			byte[] data = new byte[LEN]; // 2 x 16-bit and length in is bytes
			int len = 0;
			do
			{
				len = Bass.BASS_ChannelGetData(stream_in, data, LEN);
				if (len < 0)
				{
					BASSError be = Bass.BASS_ErrorGetCode();
					Trace.TraceInformation("Cmp3: BASS_ChannelGetData Error: " + be.ToString());
				}
				bw_out.Write(data, 0, len);
			} while (len == LEN);
			#endregion

			wavdata = ms_out.ToArray();

			bw_out.Dispose(); bw_out = null;
			ms_out.Dispose(); ms_out = null;

			Marshal.Copy(wavdata, 0, pDest, wavdata.Length);

//SaveWav(filename);

			return 0;
		}

		public override void Close( int nHandle )
		{
			Bass.BASS_StreamFree(stream_in);
			Bass.BASS_Free();
			wavdata = null;
		}

		/// <summary>
		/// save wav file (for debugging)
		/// </summary>
		/// <param name="filename">input mp3/xa filename</param>
		private void SaveWav(string filename)
		{
			string outfile = Path.GetFileName(filename);
			var fs = new FileStream(outfile + ".wav", FileMode.Create);
			var st = new BinaryWriter(fs);

			st.Write(new byte[] { 0x52, 0x49, 0x46, 0x46 });      // 'RIFF'
			st.Write((int)(_TotalPCMSize + 44 - 8));      // filesize - 8 [byte]；今は不明なので後で上書きする。
			st.Write(new byte[] { 0x57, 0x41, 0x56, 0x45 });      // 'WAVE'
			st.Write(new byte[] { 0x66, 0x6D, 0x74, 0x20 });      // 'fmt '
			st.Write(new byte[] { 0x10, 0x00, 0x00, 0x00 });      // chunk size 16bytes
			st.Write(new byte[] { 0x01, 0x00 }, 0, 2);                  // formatTag 0001 PCM
			st.Write((short)_wfx.nChannels);                              // channels
			st.Write((int)_wfx.nSamplesPerSec);                             // samples per sec
			st.Write((int)_wfx.nAvgBytesPerSec);          // avg bytesper sec
			st.Write((short)_wfx.nBlockAlign);                        // blockalign = 16bit * mono/stereo
			st.Write((short)_wfx.wBitsPerSample);                  // bitspersample = 16bits

			st.Write(new byte[] { 0x64, 0x61, 0x74, 0x61 });      // 'data'
			st.Write((int) _TotalPCMSize);      // datasize 
			
			st.Write(wavdata);
Trace.TraceInformation($"wrote ({outfile}.wav) fsLength=" + fs.Length + ", TotalPCMSize=" + _TotalPCMSize + ", diff=" + (fs.Length - _TotalPCMSize));
			st.Dispose();
			fs.Dispose();
		}
	}
}
