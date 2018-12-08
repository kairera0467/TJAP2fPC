using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Data.SQLite;
using System.Text;
using System.Threading.Tasks;

namespace DTXMania
{
    /// <summary>
    /// SQLiteデータベースを利用するためのクラス
    /// </summary>
    public class CDBUtil
    {
        SQLiteConnectionStringBuilder sqlConnectionSb;

        SQLiteConnection connection;
        public void initalize()
        {
            this.sqlConnectionSb = new SQLiteConnectionStringBuilder { DataSource = "tjap2fpc.sqlite" };
        }

        public int tノンクエリSQL実行( string sql )
        {
            SQLiteCommand cmd;
            try
            {
                this.connection = new SQLiteConnection( sqlConnectionSb.ToString() );

            }
            catch( Exception ex )
            {
                Trace.TraceError( ex.StackTrace );
            }

            return 0;
        }
    }
}
