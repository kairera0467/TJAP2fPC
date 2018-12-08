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
            this.sqlConnectionSb = new SQLiteConnectionStringBuilder { DataSource = CDTXMania.strEXEのあるフォルダ + "\\tjap2fpc.sqlite" };
            this.connection = new SQLiteConnection(sqlConnectionSb.ToString());
        }

        /// <summary>
        /// UPDATE INSERT DELETE等のSQLを実行する
        /// </summary>
        /// <param name="sql">SQLクエリ</param>
        /// <returns></returns>
        public int tノンクエリSQL実行( string sql )
        {
            int ret = 0;
            SQLiteCommand cmd = null;
            try
            {
                this.connection.Open();

                cmd = this.connection.CreateCommand();
                cmd.CommandText = sql;

                ret = cmd.ExecuteNonQuery();

                this.connection.Close();
            }
            catch( Exception ex )
            {
                Trace.TraceError( ex.StackTrace );
            }

            return ret;
        }

        public void tクエリSQL実行( string sql )
        {
            SQLiteCommand cmd = null;
        }
    }
}
