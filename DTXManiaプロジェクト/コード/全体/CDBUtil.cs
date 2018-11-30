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
    /// SQLiteデータベースに接続するためのクラス
    /// </summary>
    public class CDBUtil
    {
        SQLiteConnection connection;
        public void open()
        {
            try
            {
                this.connection = new SQLiteConnection( "" );
            }
            catch( Exception ex )
            {
                Trace.TraceError( ex.StackTrace );
            }
        }

        public void close()
        {
            try
            {
                this.connection.Close();
            }
            catch( Exception ex )
            {
                Trace.TraceError( ex.StackTrace );
            }
        }
    }
}
