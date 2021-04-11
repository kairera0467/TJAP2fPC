using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using FDK;

namespace DTXMania
{
	internal class CAct演奏Combo音声 : CActivity
	{
		// コンストラクタ

		public CAct演奏Combo音声()
		{
			base.b活性化してない = true;
		}
		
		
		// メソッド
        public void t再生( int nCombo, int player )
        {
            if( player == 0 )
            {
                if( this.nVoiceIndex < this.listComboVoice.Count )
                {
                    if( nCombo == this.listComboVoice[ this.nVoiceIndex ].nCombo )
                    {
                        if( this.listComboVoice[ this.nVoiceIndex ].soundComboVoice != null )
                        {
                            this.listComboVoice[ this.nVoiceIndex ].soundComboVoice.tサウンドを先頭から再生する(); //2017.4.16 kairera0467 一度再生したコンボ音声が鳴らせなくなっていたので対策
                        }
                        this.nVoiceIndex++;
                    }
                }
            }
            else if( player == 1 )
            {
                if( this.nVoiceIndexP2 < this.listComboVoiceP2.Count )
                {
                    if( nCombo == this.listComboVoiceP2[ this.nVoiceIndexP2 ].nCombo )
                    {
                        if( this.listComboVoiceP2[ this.nVoiceIndexP2 ].soundComboVoice != null )
                        {
                            this.listComboVoiceP2[ this.nVoiceIndexP2 ].soundComboVoice.tサウンドを先頭から再生する();
                        }
                        this.nVoiceIndexP2++;
                    }
                }
            }

        }

        /// <summary>
        /// カーソルを戻す。
        /// コンボが切れた時に使う。
        /// </summary>
        public void tリセット( int player )
        {
            switch( player )
            {
                case 0:
                    this.nVoiceIndex = 0;
                    break;
                case 1:
                    this.nVoiceIndexP2 = 0;
                    break;
            }
        }


		// CActivity 実装

		public override void On活性化()
		{
            this.listComboVoice = new List<CComboVoice>();
            this.listComboVoiceP2 = new List<CComboVoice>();
            this.nVoiceIndex = 0;
            this.nVoiceIndexP2 = 0;
			base.On活性化();
		}
        public override void On非活性化()
        {
            base.On非活性化();
        }

        public override void OnManagedリソースの作成()
		{
			if( !base.b活性化してない )
			{
                this.tReadSoundConfig();

                for( int i = 0; i < this.listComboVoice?.Count; i++ )
                {
                    if( this.listComboVoice[ i ].bFileFound )
                        this.listComboVoice[ i ].soundComboVoice = CDTXMania.Sound管理.tサウンドを生成する( CSkin.Path( @"Sounds\" + this.listComboVoice[ i ].strFilePath ) );
                }
                for( int i = 0; i < this.listComboVoiceP2?.Count; i++ )
                {
                    if( this.listComboVoiceP2[ i ].bFileFound )
                        this.listComboVoiceP2[ i ].soundComboVoice = CDTXMania.Sound管理.tサウンドを生成する( CSkin.Path( @"Sounds\" + this.listComboVoiceP2[ i ].strFilePath ) );
                }
				base.OnManagedリソースの作成();
			}
		}
		public override void OnManagedリソースの解放()
		{
			if( !base.b活性化してない )
			{
                if( this.listComboVoice != null )
                {
                    for( int i = 0; i < this.listComboVoice.Count; i++ )
                    {
                        CDTXMania.Sound管理.tサウンドを破棄する( this.listComboVoice[ i ].soundComboVoice );
                    }
                }
                if( this.listComboVoiceP2 != null )
                {
                    for( int i = 0; i < this.listComboVoiceP2.Count; i++ )
                    {
                        CDTXMania.Sound管理.tサウンドを破棄する( this.listComboVoiceP2[ i ].soundComboVoice );
                    }
                }
                this.listComboVoice?.Clear();
                this.listComboVoiceP2?.Clear();
				base.OnManagedリソースの解放();
			}
		}

		// その他
        private void tReadSoundConfig()
        {
            if( File.Exists( CSkin.Path( @"Sound.csv" ) ) )
            {
                string str;
                using( StreamReader reader = new StreamReader( CSkin.Path( @"Sound.csv" ) ) )
                {
                    str = reader.ReadToEnd();
                }

                this.t文字列から読み込み( str );
            }

            //ここでコンボ数をキーにしてソート。
            this.listComboVoice.Sort();
            this.listComboVoiceP2.Sort();
        }

        private void t文字列から読み込み( string strAllSettings )
        {
            string[] delimiter = { "\n" };
            string[] strSingleLine = strAllSettings.Split( delimiter, StringSplitOptions.RemoveEmptyEntries );
            foreach( string s in strSingleLine )
            {
                if( s[ 0 ] != '#' ) //先頭文字が#でなければその行は無視
                    continue;
                s.Replace( '\r', ' ' );

                //正常なら5個になる。
                string[] strArray = s.Split( ',' );

                if( strArray.Length != 4 )
                    continue;
                if( strArray[ 0 ] != "#SE" && ( strArray[ 1 ] != "COMBOVOICE" || strArray[ 1 ] != "COMBOVOICE_P2" ) )
                    continue;

                if( strArray[ 1 ] == "COMBOVOICE" )
                {
                    var voice = new CComboVoice();
                    voice.strFilePath = strArray[ 2 ];
                    voice.nCombo = Convert.ToInt32( strArray[ 3 ] );
                    voice.bFileFound = File.Exists( CSkin.Path( @"Sounds\" + strArray[ 2 ] ) );
                    voice.nPlayer = 0;

                    this.listComboVoice.Add( voice );
                }
                else if( strArray[ 1 ] == "COMBOVOICE_P2" )
                {
                    var voice = new CComboVoice();
                    voice.strFilePath = strArray[ 2 ];
                    voice.nCombo = Convert.ToInt32( strArray[ 3 ] );
                    voice.bFileFound = File.Exists( CSkin.Path( @"Sounds\" + strArray[ 2 ] ) );
                    voice.nPlayer = 1;

                    this.listComboVoiceP2.Add( voice );
                }
            }
        }


		#region [ private ]
		//-----------------
        private List<CComboVoice> listComboVoice;
        private List<CComboVoice> listComboVoiceP2;
        private int nVoiceIndex;
        private int nVoiceIndexP2;
		//-----------------
		#endregion
	}

    public class CComboVoice : IComparable<CComboVoice>
    {
        public bool bFileFound;
        public int nCombo;
        public int nPlayer;
        public string strFilePath;
        public CSound soundComboVoice;

        public CComboVoice()
        {
            bFileFound = false;
            nCombo = 0;
            nPlayer = 0;
            strFilePath = "";
            soundComboVoice = null;
        }

        public int CompareTo( CComboVoice other )
        {
            if( this.nCombo > other.nCombo ) return 1;
            else if( this.nCombo < other.nCombo ) return -1;

            return 0;
        }
    }
}
