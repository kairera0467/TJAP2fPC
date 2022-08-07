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
        /// <returns>実行結果(件数)</returns>
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
                ret = -1;
            }

            return ret;
        }

        /// <summary>
        /// SELECTのSQLを実行して結果をDataTable形式で返す
        /// エラーの場合はnullで返される
        /// </summary>
        /// <param name="sql">SQLクエリ</param>
        /// <returns>実行結果</returns>
        public DataTable tクエリSQL実行( string sql )
        {
            DataTable ret = new DataTable();

            try
            {
                this.connection.Open();

                SQLiteDataAdapter dataAdapter;
                dataAdapter = new SQLiteDataAdapter(sql, connection);
                dataAdapter.Fill( ret );
                dataAdapter?.Dispose();

                this.connection?.Close();
            }
            catch( Exception ex )
            {
                Trace.TraceError( ex.StackTrace );
            }
            finally
            {
                ret = null;
            }


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

                // ユーザーデータ
                strCommand = "CREATE TABLE playerdata " +
                    "user_id INT NOT NULL" +
                    "";
            }
            catch( Exception ex )
            {
                Trace.TraceError( ex.StackTrace );
            }
        }
    }
}
