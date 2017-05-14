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
        public void t再生( int nCombo )
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

        /// <summary>
        /// カーソルを戻す。
        /// コンボが切れた時に使う。
        /// </summary>
        public void tリセット()
        {
            this.nVoiceIndex = 0;
        }


		// CActivity 実装

		public override void On活性化()
		{
            this.listComboVoice = new List<CComboVoice>();
            this.nVoiceIndex = 0;
			base.On活性化();
		}
		public override void OnManagedリソースの作成()
		{
			if( !base.b活性化してない )
			{
                this.tReadSoundConfig();

                for( int i = 0; i < this.listComboVoice.Count; i++ )
                {
                    if( this.listComboVoice[ i ].bFileFound )
                        this.listComboVoice[ i ].soundComboVoice = CDTXMania.Sound管理.tサウンドを生成する( CSkin.Path( @"Sounds\" + this.listComboVoice[ i ].strFilePath ) );
                }

				base.OnManagedリソースの作成();
			}
		}
		public override void OnManagedリソースの解放()
		{
			if( !base.b活性化してない )
			{
                for( int i = 0; i < this.listComboVoice.Count; i++ )
                {
                    CDTXMania.Sound管理.tサウンドを破棄する( this.listComboVoice[ i ].soundComboVoice );
                }
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
                if( strArray[ 0 ] != "#SE" && strArray[ 1 ] != "COMBOVOICE" )
                    continue;

                var voice = new CComboVoice();
                voice.strFilePath = strArray[ 2 ];
                voice.nCombo = Convert.ToInt32( strArray[ 3 ] );
                voice.bFileFound = File.Exists( CSkin.Path( @"Sounds\" + strArray[ 2 ] ) );

                this.listComboVoice.Add( voice );
            }
        }


		#region [ private ]
		//-----------------
        private List<CComboVoice> listComboVoice;
        private int nVoiceIndex;
		//-----------------
		#endregion
	}

    public class CComboVoice : IComparable<CComboVoice>
    {
        public bool bFileFound;
        public int nCombo;
        public string strFilePath;
        public CSound soundComboVoice;

        public CComboVoice()
        {
            bFileFound = false;
            nCombo = 0;
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
