using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Data;
using System.Data.SQLite;
using System.Text;
using System.Threading.Tasks;

namespace DTXMania
{
    /// <summary>
    /// SQLiteデータベースを利用するためのクラス
    /// ToDo: 将来はSQLite以外のデータベースを扱うクラスはTJAP2fPC側では持たず、DB専用のドライバとして扱う予定。
    /// </summary>
    public class CDBUtil
    {
        //public 

        public int nDBバージョン
        {
            get;
            set;
        }

        private readonly string strDBFileName = "tjap2fpc";
        SQLiteConnectionStringBuilder sqlConnectionSb;
        SQLiteConnection connection;
        public void initalize( EDBモード eDBmode )
        {
            if( eDBmode == EDBモード.SQLite )
            {
                this.sqlConnectionSb = new SQLiteConnectionStringBuilder { DataSource = CDTXMania.strEXEのあるフォルダ + "\\"+ strDBFileName + ".sqlite" };
                this.connection = new SQLiteConnection(sqlConnectionSb.ToString());
            }
            else if( eDBmode == EDBモード.MySQL )
            {
                Trace.TraceWarning( "MySQLでのSQL接続は現在未実装です。" );
            }
            else if( eDBmode == EDBモード.Plugin )
            {
                Trace.TraceWarning( "プラグイン形式でのSQL接続は現在未実装です。" );
            }
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
            }
            catch( Exception ex )
            {
                Trace.TraceError( ex.StackTrace );
            }
            finally
            {
                cmd?.Dispose();
                this.connection?.Close();
            }

            return ret;
        }

        public DataTable tクエリSQL実行( string sql )
        {
            SQLiteCommand cmd = null;
            DataTable ret = new DataTable();


            return ret;
        }

        /// <summary>
        /// データベースを作成する
        /// ※未完成
        /// </summary>
        public void t初期DBを作成する()
        {
            int ret = 0;
            SQLiteCommand cmd = null;
            try
            {
                string strCommand = "CREATE DATABASE tjap2fpc;";
                this.tノンクエリSQL実行( strCommand );



            }
            catch( Exception ex )
            {
                Trace.TraceError( ex.StackTrace );
            }
        }
    }
}
