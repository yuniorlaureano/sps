using System;
using System.Collections.Generic;
//using System.Data.OracleClient;
using System.Data;
using System.Data.Common;
using Oracle.ManagedDataAccess.Client;

namespace DBModel.DbConnections
{

    /// <summary>
    /// Esta clase implementa la conexion a una base de datos de Oracle. 
    /// Autor: Yeison Lapaix Angomas
    /// Historial de Modificaciones:
    /// -----------------------------------------------------------------------------------------------------------
    /// Nombre                     Fecha de Actualizacion              Comentario
    /// Yeison Lapaix              27/10/2014                          Completando la documentacion de los metodos. 
    /// -----------------------------------------------------------------------------------------------------------
    /// </summary>
    public class OracleDBConnection : IDBConnection
    {


        public OracleDBConnection()
        {
            this.con = new OracleConnection();
        }
        
        /// <summary>
        /// Especifica el nombre del ConnectionString que especifica la base de datos donde se va conectar.
        /// Debe asegurarse de que la conexion este agregada en el AppConfig.
        /// </summary>
        /// <param name="ConnectionName">Nombre de conexion</param>
        public OracleDBConnection(String ConnectionName)
        {            
            this.con = new OracleConnection();
            this.ConnectionName = ConnectionName;
            this.con.ConnectionString = getConnectionString();
        }

        /// <summary>
        /// Retorna una objeto de tipo lista de parametro de Oracle.
        /// </summary>
        /// <returns></returns>
        //public DbParameterCollection getNewParameterList() {
        //    return new OracleParameterCollection();
        //}

        /// <summary>
        /// Retorna un objeto de tipo OracleParameter.
        /// </summary>
        /// <param name="name">Nombre de parametro</param>
        /// <param name="valor">Valor de parametro</param>
        /// <returns></returns>
        public DbParameter getNewParameter(string name,object valor) {
            OracleParameter rs = new OracleParameter(name, valor);
            rs.Direction = ParameterDirection.Input;
            return rs;
        }
        
        /// <summary>
        /// Returna un parametro de tipo cursor de OracleDb.
        /// </summary>
        /// <param name="name">Nombre del parametro</param>
        /// <returns></returns>
        public DbParameter getNewCursorParameter(string name)
        {
            OracleParameter cursor  = new OracleParameter(name, OracleDbType.RefCursor);            
            cursor.Direction = ParameterDirection.Output;
            return cursor;
        }
        
        /*
        public static OracleDBConnection getConnection(Connections ConnectionName)
        {
            if (connection == null || connection.ConnectionName== ConnectionName) {
                return new OracleDBConnection(ConnectionName);
            }
            return connection;
        }*/

        /// <summary>
        /// Ejecuta un script en la base de datos. 
        /// </summary>
        /// <param name="query">Sql Script que se va ejecutar</param>
        public override void ExecuteStatement(string query)
        {
            this.con.ConnectionString = getConnectionString(); 
            this.OpenConnection();
                this.cmd = new OracleCommand();
                this.cmd.CommandText = query;
                this.cmd.ExecuteNonQuery();
            this.CloseConnection();
        }

        /// <summary>
        /// Ejecuta un query en la base de datos y retorna un datatable con el resulset. 
        /// </summary>
        /// <param name="query">Query que se va ejecutar en la base de datos.</param>
        /// <returns></returns>
        public override System.Data.DataTable ExecuteQuery(string query)
        {
            DataTable result = new DataTable();
            try
            {

                this.cmd = new OracleCommand();
                this.cmd.Connection = this.con;
                this.con.ConnectionString = getConnectionString(); 
                this.OpenConnection();
                this.cmd.CommandText = query;

                DbDataReader dataReader = this.cmd.ExecuteReader();
                result.Load(dataReader);


                this.CloseConnection();
            }
            catch (OracleException ex) {
                throw ex;
            }
            return result;
        }
        
        /// <summary>
        /// Ejecuta un sql script en la base de datos y da la opcion de ejecutar procedure que contenga cursores que retorne un result set. 
        /// </summary>
        /// <param name="query">Script o nombre de procedure que se va ejecutar</param>
        /// <param name="args">Lista de argumentos que recibe el procedure</param>
        /// <param name="isProcedure">Se coloca true si es un procedure</param>
        /// <param name="hasResult">se coloca true si el cursor retorna un resultado.</param>
        /// <returns></returns>
        public override DataTable ExecuteStatementWithCursor(string query, List<DbParameter> args, bool isProcedure, bool hasResult)
        {
            this.cmd = new OracleCommand();
            this.cmd.CommandText = query;
            this.cmd.CommandType = (isProcedure) ? CommandType.StoredProcedure : CommandType.Text;

            if (args != null)
            {
                foreach (DbParameter arg in args)
                {
                    //OracleParameter argTmp = new OracleParameter();
                    //argTmp.Direction = arg.Direction;
                    //argTmp.Value = arg.Value;
                    //argTmp.ParameterName = arg.ParameterName;
                    //argTmp.DbType = arg.DbType;

                    this.cmd.Parameters.Add(arg);
                }
            }

            DataTable tb = null;
            try
            {
                cmd.Connection = this.con;
                this.con.ConnectionString = getConnectionString();   
                this.con.Open();
                if (hasResult)
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
            catch (OracleException sqlEx)
            {
                throw sqlEx;
            }
            finally
            {
                this.CloseConnection();
            }
            return tb;
        }

        public override DataSet ExecuteStatementDS(string query, List<DbParameter> args)
        {
            this.cmd = new OracleCommand();
            this.cmd.CommandText = query;
            this.cmd.CommandType = CommandType.StoredProcedure;

            if (args != null)
            {
                foreach (DbParameter arg in args)
                {
                    //OracleParameter argTmp = new OracleParameter();
                    //argTmp.Direction = arg.Direction;
                    //argTmp.Value = arg.Value;
                    //argTmp.ParameterName = arg.ParameterName;
                    //argTmp.DbType = arg.DbType;

                    this.cmd.Parameters.Add(arg);
                }
            }
            DataSet objDataSet = new DataSet();

            try
            {
                cmd.Connection = (OracleConnection)this.con;
                this.con.ConnectionString = getConnectionString();
                OracleDataAdapter objAdapter = new OracleDataAdapter((OracleCommand)cmd);


                objAdapter.Fill(objDataSet);
                objAdapter.Dispose();
                cmd.Dispose();

            }
            catch (OracleException sqlEx)
            {
                throw sqlEx;
            }
            finally
            {
                this.CloseConnection();
            }
            return objDataSet;
        }
        /// <summary>
        /// Ejecuta un sql statement en la base de datos.
        /// </summary>
        /// <param name="query">Script que se va ejecutar</param>
        /// <param name="args">Lista de argumentos de statement</param>
        /// <param name="isProcedure">Se colocar true si es un procedure, de lo contrario false</param>
        /// <returns>Retorna un datatable con el resutado</returns>
        public override DataTable ExecuteStatement(string query, List<DbParameter> args, bool isProcedure)
        {
            this.cmd = new OracleCommand();
            this.cmd.CommandText = query;
            this.cmd.CommandType = (isProcedure) ? CommandType.StoredProcedure : CommandType.Text;

            if (args != null) {
                foreach (DbParameter arg in args) {                                        
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
            catch (OracleException sqlEx)
            {
                if (sqlEx.ErrorCode == 1) {
                    throw new Exception("Error de duplicidad verifique el registro no se encuetre actualmente guardado.");
                }
                throw sqlEx;
            }
            finally
            {
                this.CloseConnection();
            }
            return tb;
        }

    }
}
