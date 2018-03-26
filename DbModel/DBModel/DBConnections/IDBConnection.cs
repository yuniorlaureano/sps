using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;
using DBModel.Enums;

namespace DBModel.DbConnections
{
    public abstract class  IDBConnection
    {
        public DbCommand cmd;
        public DbConnection con;
        public String ConnectionName;
        
        /// <summary>
        /// Returns the connection string of the current connection. 
        /// </summary>
        /// <returns></returns>
        public String getConnectionString() {
            try
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings[this.ConnectionName].ConnectionString;
            }
            catch (NullReferenceException ex) {
                this.ConnectionName = Connections.RD;
                return System.Configuration.ConfigurationManager.ConnectionStrings[this.ConnectionName].ConnectionString;
            }
            
        }
        public abstract DataTable ExecuteStatementWithCursor(string query, List<DbParameter> args, bool isProcedure, bool hasResult);

        public abstract DataSet ExecuteStatementDS(string query, List<System.Data.Common.DbParameter> args);
        /// <summary>
        /// Execute a query that not return data.
        /// </summary>
        /// <param name="query"></param>
        public abstract void ExecuteStatement(String query);

        /// <summary>
        /// Execute a query and retrive the resultset in a datatable.
        /// </summary>
        /// <param name="query">
        /// String of Query
        /// </param>
        /// <returns>
        /// Data of resultset
        /// </returns>
        public abstract DataTable ExecuteQuery(String query);

        /// <summary>
        /// Execute a nueve DbCommand in the database.
        /// </summary>
        /// <param name="cmd"></param>
        public abstract DataTable ExecuteStatement(string query, List<DbParameter> args, bool isProcedure);

//        public abstract DbConnection getConnection();

        /// <summary>
        /// Cancer the current request to the DB.
        /// </summary>
        private void CancelRequest(){
            if (this.cmd != null) {
                this.cmd.Cancel();
            }
        }

        /// <summary>
        /// Open a connection in the database. 
        /// </summary>
        public void OpenConnection() {
            if (this.con.State == ConnectionState.Closed) {
                this.con.Open();
            }
        }

        /// <summary>
        /// Close de current connection.
        /// </summary>
        public void CloseConnection() {
            if (this.con.State == ConnectionState.Open) {
                this.con.Close();
            }
        }
        
    }
}
