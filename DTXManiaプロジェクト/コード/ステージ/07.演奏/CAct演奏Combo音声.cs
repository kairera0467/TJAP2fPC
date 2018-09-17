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
        public void tリセット()
        {
            this.nVoiceIndex = 0;
            this.nVoiceIndexP2 = 0;
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
		public override void OnManagedリソースの作成()
		{
			if( !base.b活性化してない )
			{
                // フォルダ内を走査してコンボボイスをListに入れていく
                Console.WriteLine(CSkin.Path(@"Sounds\Combo_1P\"));
                // 1P コンボボイス
                if (Directory.Exists(CSkin.Path(@"Sounds\Combo_1P\")))
                {
                    foreach (var item in System.IO.Directory.GetFiles(CSkin.Path(@"Sounds\Combo_1P\")))
                    {
                        var comboVoice = new CComboVoice();
                        comboVoice.bFileFound = true;
                        comboVoice.nPlayer = 0;
                        comboVoice.strFilePath = item;
                        comboVoice.soundComboVoice = CDTXMania.Sound管理.tサウンドを生成する(item, ESoundGroup.Voice);
                        comboVoice.nCombo = int.Parse(Path.GetFileNameWithoutExtension(item));
                        listComboVoice.Add(comboVoice);
                    }
                    if (listComboVoice.Count > 0)
                        listComboVoice.Sort();
                }

                // 2P コンボボイス
                if (Directory.Exists(CSkin.Path(@"Sounds\Combo_2P\")))
                {
                    foreach (var item in System.IO.Directory.GetFiles(CSkin.Path(@"Sounds\Combo_2P\")))
                    {
                        var comboVoice = new CComboVoice();
                        comboVoice.bFileFound = true;
                        comboVoice.nPlayer = 1;
                        comboVoice.strFilePath = item;
                        comboVoice.soundComboVoice = CDTXMania.Sound管理.tサウンドを生成する(item, ESoundGroup.Voice);
                        comboVoice.nCombo = int.Parse(Path.GetFileNameWithoutExtension(item));
                        listComboVoiceP2.Add(comboVoice);
                    }
                    if (listComboVoiceP2.Count > 0)
                        listComboVoiceP2.Sort();
                }

    			base.OnManagedリソースの作成();
			}
		}
		public override void OnManagedリソースの解放()
		{
			if( !base.b活性化してない )
			{
                foreach (var item in listComboVoice)
                {
                    CDTXMania.Sound管理.tサウンドを破棄する(item.soundComboVoice);
                }
                listComboVoice?.Clear();
                foreach (var item in listComboVoiceP2)
                {
                    CDTXMania.Sound管理.tサウンドを破棄する(item.soundComboVoice);
                }
                listComboVoiceP2?.Clear();

				base.OnManagedリソースの解放();
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
