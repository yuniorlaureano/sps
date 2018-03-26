using System;   
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Configuration;

namespace DBModel.DbConnections
{
    public class MSSQLConnection : IDBConnection
    {
        

        public string ConnectionString { get; set; }

        public MSSQLConnection(string ConnectionName) {
            this.con = new SqlConnection();
            this.ConnectionName = ConnectionName;
        }

        public override System.Data.DataTable ExecuteStatementWithCursor(string query, List<System.Data.Common.DbParameter> args, bool isProcedure, bool hasResult)
        {
            throw new NotImplementedException();
            
        }

        public override void ExecuteStatement(string query)
        {
            this.con.ConnectionString = this.getConnectionString();           
            OpenConnection();
                this.cmd = new SqlCommand();
                this.cmd.CommandText = query;
                this.cmd.ExecuteNonQuery();
            CloseConnection();                        
        }
        public override System.Data.DataTable ExecuteQuery(string query)
        {
            DataTable tb = null;
            try
            {
                this.con.ConnectionString = this.getConnectionString();
                OpenConnection();
                this.cmd = new SqlCommand();
                this.cmd.CommandText = query;
                this.cmd.Connection = this.con;
                DbDataReader reader = this.cmd.ExecuteReader();
                tb = new DataTable();
                tb.Load(reader);
                CloseConnection();
            }
            catch (SqlException ex) {
                throw ex;
            }
            return tb;
        }
        public override System.Data.DataTable ExecuteStatement(string query, List<System.Data.Common.DbParameter> args, bool isProcedure)
        {
            this.cmd = new SqlCommand();
            this.cmd.CommandText = query;
            this.cmd.CommandType = (isProcedure) ? CommandType.StoredProcedure : CommandType.Text;

            if (args != null)
            {
                foreach (DbParameter arg in args)
                {
                    this.cmd.Parameters.Add(arg);
                }
            }

            DataTable tb = null;
            try
            {
                cmd.Connection = this.con;
                this.con.ConnectionString = getConnectionString();
                this.con.Open();
                if (cmd.CommandType == CommandType.Text)
                {
                    DbDataReader rd = cmd.ExecuteReader();
                    tb = new DataTable();
                    tb.Load(rd);
                }
                else
                {
                    cmd.ExecuteNonQuery();
                }
                this.con.Close();
            }
            catch (SqlException sqlEx)
            {
                throw sqlEx;
            }
            finally
            {
                this.CloseConnection();
            }
            return tb;
            
        }
        public int ExecuteStatementProcedure(string query, List<System.Data.Common.DbParameter> args)
        {
            int rowsAffected = 0;
            this.cmd = new SqlCommand();
            this.cmd.CommandText = query;
            this.cmd.CommandType = CommandType.StoredProcedure;

            if (args != null)
            {
                foreach (DbParameter arg in args)
                {
                    //this.cmd.Parameters.Clear();
                    this.cmd.Parameters.Add(arg);
                }
            }

            DataTable tb = null;
            try
            {
                cmd.Connection = this.con;
                this.con.ConnectionString = getConnectionString();
                this.con.Open();
                if (cmd.CommandType == CommandType.Text)
                {
                    DbDataReader rd = cmd.ExecuteReader();
                    tb = new DataTable();
                    tb.Load(rd);
                    rd.Close();
                }
                else
                {
                    rowsAffected = cmd.ExecuteNonQuery();
                }
                this.con.Close();
            }
            catch (SqlException sqlEx)
            {
                throw sqlEx;
            }
            finally
            {
                this.CloseConnection();
            }

            return rowsAffected;

        }
        public  System.Data.DataTable ExecuteStatement(string query, List<System.Data.Common.DbParameter> args)
        {
            SqlDataReader rd;
            System.Data.DataTable dt = new DataTable();

            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = getConnectionString();

            SqlCommand sqlcomm = new SqlCommand(query, conn);
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.CommandTimeout = 460;

            if (args != null)
            {
                foreach (DbParameter arg in args)
                {
                    sqlcomm.Parameters.Add(arg);
                }
            }
           

            try
            {
                conn.Open();
                rd = sqlcomm.ExecuteReader();
                dt.Load(rd);
                rd.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return dt;
        }
        public override System.Data.DataSet ExecuteStatementDS(string query, List<System.Data.Common.DbParameter> args)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = getConnectionString();

                DataSet dataset = new DataSet();
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = new SqlCommand(query, conn);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapter.SelectCommand.CommandTimeout = 360;
                if (args != null)
                {
                    foreach (DbParameter arg in args)
                    {
                        adapter.SelectCommand.Parameters.Add(arg);
                    }
                }
                try
                {
                    conn.Open();
                    adapter.Fill(dataset);
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    conn.Close();
                }

                return dataset;
            
        }
        public SqlParameter getDateParameter(string name, DateTime? valor)
        {

            SqlParameter rs = new SqlParameter(name, valor);
            if (valor == null) {
                rs.Value = System.Data.SqlTypes.SqlDateTime.Null;
            }
            rs.Direction = ParameterDirection.Input;
            return rs;
        }
        public SqlParameter getNewParameter(string name, object valor)
        {
            SqlParameter rs = new SqlParameter(name, valor);
            rs.Direction = ParameterDirection.Input;
            return rs;
        }
    
    }
}
