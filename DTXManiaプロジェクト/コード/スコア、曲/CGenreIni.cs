using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimDX;
using FDK;

namespace DTXMania
{
    public class CGenreIni
    {
        //>>次郎標準
        //[Genre]
        //GenreName=太鼓の達人3DS
        //GenreColor=#222222
        //FontColor=#c8c8c8
        //
        //>>TJAP2fPC拡張
        //FontOutLineColor=#000000
        //FolderType=
        //;0=BOXとして扱わない 1=BOXとして扱う

        public string strGenreName
        {
            get;
            private set;
        }
        public int nFolderType
        {
            get;
            private set;
        }
        private Color4 cGenreColor;
        private Color4 cFontColor;
        private Color4 cFontOutLineColor;
        
        public CGenreIni()
        {
            this.strGenreName = "";
            this.nFolderType = 0;
            this.cGenreColor = new Color4( 0, 0, 0 );
            this.cFontColor = new Color4( 0, 0, 0 );
            this.cFontOutLineColor = new Color4( 255, 255, 255 );
        }

        /// <summary>
        /// 渡されたgenre.iniのパスから読み込んで設定していく
        /// </summary>
        /// <param name="path">genre.iniの絶対パス</param>
        public CGenreIni(string path)
        {
            StreamReader sr = new StreamReader(path, Encoding.Default);
            string[] strSingleLine = sr.ReadToEnd().Split( new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in strSingleLine)
            {
                string str = s.Replace('\t', ' ').TrimStart(new char[] { '\t', ' ' });
                if ((str.Length != 0) && (str[0] != ';'))
                {
                    try
                    {
                        string strCommand;
                        string strParam;
                        string[] strArray = str.Split(new char[] { '=' });
                        if (strArray.Length == 2)
                        {
                            strCommand = strArray[0].Trim();
                            strParam = strArray[1].Trim();

                            #region[ 演奏 ]
                            //-----------------------------
                            if( strCommand == "GenreName" )
                            {
                                this.strGenreName = strParam;
                            }
                            else if( strCommand == "GenreColor" )
                            {
                                this.cGenreColor = C変換.strColorCodeToColor4( strParam );
                            }
                            else if( strCommand == "FontColor" )
                            {
                                this.cFontColor = C変換.strColorCodeToColor4( strParam );
                            }
                            else if( strCommand == "FontOutLineColor" )
                            {
                                this.cFontOutLineColor = C変換.strColorCodeToColor4( strParam );
                            }
                            else if( strCommand == "FolderType" )
                            {
                                this.nFolderType = C変換.n値を文字列から取得して範囲内に丸めて返す( strParam, 0, 1, 0 );
                            }
                            #endregion
                        }
                    }
                    catch( Exception ex )
                    {
                        Trace.TraceError( ex.StackTrace );
                    }
                }
            }
        }
    }
}
