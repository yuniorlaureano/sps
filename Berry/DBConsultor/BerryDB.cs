using Berry.Models;
using Berry.Utils;
using DBModel.DbConnections;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace Berry.DBConsultor
{
    public class BerryDB
    {
        MSSQLConnection sqlCon;
        OracleDBConnection oraCon;
        string sqlConnName = string.Empty;
        string oraConnName = string.Empty;
        Security security;

        public BerryDB(){

            string operation = ConfigurationManager.AppSettings["operation"];
            sqlConnName = "BerrySQLDB_" + operation;
            oraConnName = "BerryYPDB_" + operation;
        }
        #region SMS Compares
        public DataSet GetDimmasVsSMSReport()
        {
            sqlCon = new MSSQLConnection(sqlConnName);

            List<DbParameter> args = new List<DbParameter>();
            DataSet ds = sqlCon.ExecuteStatementDS("berry.sp_compare_dimmas_sms ", args);

            return ds;
        }

        public DataSet GetSMSvsDimmasReport()
        {
            sqlCon = new MSSQLConnection(sqlConnName);
            List<DbParameter> args = new List<DbParameter>();
            DataSet ds = sqlCon.ExecuteStatementDS("berry.sp_compare_sms_dimmas", args);
            return ds;
        }

        public DataSet GetSMSvsDimmasRejectsReport()
        {
            sqlCon = new MSSQLConnection(sqlConnName);
            List<DbParameter> args = new List<DbParameter>();
            DataSet ds = sqlCon.ExecuteStatementDS("berry.sp_compare_sms_dimmas_rejects", args);
            return ds;
        }

        public DataSet GetTotalDifCompares()
        {
            sqlCon = new MSSQLConnection(sqlConnName);
            List<DbParameter> args = new List<DbParameter>();
            DataSet ds = sqlCon.ExecuteStatementDS("berry.get_total_dif_reports", args);
            ds.Tables[0].TableName = "SMS Vs Dimmas";
            ds.Tables[1].TableName = "Dimmas Vs SMS";
            ds.Tables[2].TableName = "SMS Vs Dimmas Rejects";
            return ds;
        }

        public DataTable GetTotalStatusCompares()
        {
            sqlCon = new MSSQLConnection(sqlConnName);
            List<DbParameter> args = new List<DbParameter>();
            DataTable dt = sqlCon.ExecuteStatement("berry.get_total_status_reports", args);
            return dt;
        }
        public string RejectCompareTransaction(int semNumero, int repAccountID, int canvEdition, string canvCode, int repSecuencia, string auditor, string comentario, int audit_id)
        {
            string comment = string.Empty;
            sqlCon = new MSSQLConnection(sqlConnName);
            List<DbParameter> args = new List<DbParameter>();
            args.Add(sqlCon.getNewParameter("@sem_numero", semNumero));
            args.Add(sqlCon.getNewParameter("@rep_account_id", repAccountID));
            args.Add(sqlCon.getNewParameter("@canv_edition", canvEdition));
            args.Add(sqlCon.getNewParameter("@canv_code", canvCode));
            args.Add(sqlCon.getNewParameter("@rep_secuencia", repSecuencia));
            args.Add(sqlCon.getNewParameter("@auditor", auditor));
            args.Add(sqlCon.getNewParameter("@audit_comment", comentario));
            args.Add(sqlCon.getNewParameter("@audit_genrepd_id", audit_id));

            DataTable dt = sqlCon.ExecuteStatement("berry.reject_compare_transaction", args);
            if (dt.Rows.Count > 0)
            {
                comment = dt.Rows[0][0].ToString();
            }

            return comment;
        }

        public string AuditCompareTransaction(int semNumero, int repAccountID, int canvEdition, string canvCode, int repSecuencia, int audit_id, string auditor)
        {
            string comment = string.Empty;
            sqlCon = new MSSQLConnection(sqlConnName);
            List<DbParameter> args = new List<DbParameter>();
            args.Add(sqlCon.getNewParameter("@sem_numero", semNumero));
            args.Add(sqlCon.getNewParameter("@rep_account_id", repAccountID));
            args.Add(sqlCon.getNewParameter("@canv_edition", canvEdition));
            args.Add(sqlCon.getNewParameter("@canv_code", canvCode));
            args.Add(sqlCon.getNewParameter("@rep_secuencia", repSecuencia));
            args.Add(sqlCon.getNewParameter("@audit_genrepd_id", audit_id));
            args.Add(sqlCon.getNewParameter("@auditor", auditor));

            DataTable dt = sqlCon.ExecuteStatement("berry.audit_compare_transaction", args);
            if (dt.Rows.Count > 0)
            {
                comment = dt.Rows[0][0].ToString();
            }

            return comment;
        }

        #endregion SMS Compares

        #region Universe Compare
        public DataTable GetComparesDimmasBerry()
        {
            oraCon = new OracleDBConnection(oraConnName);
            List<DbParameter> args = new List<DbParameter>();
            args.Add(oraCon.getNewCursorParameter("resultset"));
            DataTable dt = oraCon.ExecuteStatementWithCursor("berry.get_compare_dimmas_berry", args, true, true);
            return dt;
        }

        public DataTable GetDimmasBerryAcum()
        {

            oraCon = new OracleDBConnection(oraConnName);
            List<DbParameter> args = new List<DbParameter>();
            args.Add(oraCon.getNewCursorParameter("resultset"));
            DataTable dt = oraCon.ExecuteStatementWithCursor("berry.get_compare_dimmas_berry_acum", args, true, true);
            return dt;
        }

        public DataTable GetCompareDimmasNoBerry()
        {
            oraCon = new OracleDBConnection(oraConnName);
            List<DbParameter> args = new List<DbParameter>();
            args.Add(oraCon.getNewCursorParameter("resultset"));
            DataTable dt = oraCon.ExecuteStatementWithCursor("berry.get_compare_dimmas_no_berry", args, true, true);
            return dt;
        }


        #endregion Universe Compare

        #region Berry Process
        public void GenerateBerry(string startWeek, string endWeek)
        {
            oraCon = new OracleDBConnection(oraConnName);
            List<DbParameter> args = new List<DbParameter>();
            args.Add(oraCon.getNewParameter("IN_START_DATE", startWeek));
            args.Add(oraCon.getNewParameter("IN_END_DATE", endWeek));
            DataTable dt = oraCon.ExecuteStatementWithCursor("berry.SP_BERRY", args, true, true);
        }

        //public void GenerateBerryByProduct(string canvCode, int canvEdition, int week1, string productGroup, string db)
        //{
        //    List<DbParameter> args = new List<DbParameter>(); 
        //    oraCon = new OracleDBConnection(oraConnName);
        //    string conn = string.Empty;
        //    //query = "SP_BERY";

        //    conn =  "Berry" + db + "DB";
   
        //    args.Add(oraCon.getNewParameter("LC_CANV_CODE", canvCode));
        //    args.Add(oraCon.getNewParameter("LN_CANV_EDT", canvEdition));
        //    args.Add(oraCon.getNewParameter("LN_CANV_WEEK", week1));
        //    args.Add(oraCon.getNewParameter("lc_ProdGroup", productGroup));
        //    oraCon.ExecuteStatement("sp_bery", args, true);
        //}

        //public DataTable GetActiveWeekProdGrp(string db)
        //{
        //    oraCon = new OracleDBConnection(oraConnName);
        //    List<DbParameter> args = new List<DbParameter>();
        //    //args.Add(oraCon.getNewParameter("pint_edicion", canvEdition));
        //    //args.Add(oraCon.getNewParameter("pstr_start_week", startWeek));
        //    //args.Add(oraCon.getNewParameter("pstr_end_week", endWeek));
        //    args.Add(oraCon.getNewParameter("DB", db));
        //    args.Add(oraCon.getNewCursorParameter("resultset"));
        //    DataTable dt = oraCon.ExecuteStatementWithCursor("berry.GET_ACTVL_WKL", args, true, true);
        //    return dt;
        //}

        public DataTable AutoCompleteOpenCanvEdition(string db)
        {
            oraCon = new OracleDBConnection(oraConnName);
            List<DbParameter> args = new List<DbParameter>();
            args.Add(oraCon.getNewParameter("DB", db));
            args.Add(oraCon.getNewCursorParameter("resultset"));
            DataTable dt = oraCon.ExecuteStatementWithCursor("berry.AUTCMP_CANV_EDIT", args, true, true);
            return dt;
        }

        public DataTable AutoCompleteClosedCanvEdition(string db)
        {
            oraCon = new OracleDBConnection(oraConnName);
            List<DbParameter> args = new List<DbParameter>();
            args.Add(oraCon.getNewParameter("DB", db));
            args.Add(oraCon.getNewCursorParameter("resultset"));
            DataTable dt = oraCon.ExecuteStatementWithCursor("berry.AUTCMP_CLOSED_CANV_EDIT", args, true, true);
            return dt;
        }

        #endregion Berry Process

        #region Berry Closure Process
        public DataTable GetActiveWeek(int canvEdition, string startWeek, string endWeek)
        {
            oraCon = new OracleDBConnection(oraConnName);
            List<DbParameter> args = new List<DbParameter>();
            args.Add(oraCon.getNewParameter("pint_edicion", canvEdition));
            args.Add(oraCon.getNewParameter("pstr_start_week", startWeek));
            args.Add(oraCon.getNewParameter("pstr_end_week", endWeek));
            args.Add(oraCon.getNewCursorParameter("resultset"));
            DataTable dt = oraCon.ExecuteStatementWithCursor("berry.GET_ACTIVE_WEEK", args, true, true);
            return dt;
        }

        public DataTable GetActiveWeekByDate(string db, int canvEdition, string startDate, string endDate)
        {
            oraCon = new OracleDBConnection(oraConnName);
            List<DbParameter> args = new List<DbParameter>();
            args.Add(oraCon.getNewParameter("DB", db));
            args.Add(oraCon.getNewParameter("IN_CANV_EDITION", canvEdition));
            args.Add(oraCon.getNewParameter("IN_START_DATE", startDate));
            args.Add(oraCon.getNewParameter("IN_END_DATE", endDate));
            args.Add(oraCon.getNewCursorParameter("resultset"));
            DataTable dt = oraCon.ExecuteStatementWithCursor("berry.GET_ACTVL_WKL_BY_DATE", args, true, true);
            return dt;
        }

        public void CloseBerry(string db, int canvEdition, string startWeek, string endWeek)
        {
            oraCon = new OracleDBConnection(oraConnName);
            List<DbParameter> args = new List<DbParameter>();
            args.Add(oraCon.getNewParameter("pint_edicion", canvEdition));
            args.Add(oraCon.getNewParameter("pstr_start_week", startWeek));
            args.Add(oraCon.getNewParameter("pstr_end_week", endWeek));
            args.Add(oraCon.getNewParameter("DB", db));
            oraCon.ExecuteStatementWithCursor("berry.CLOSE_WEEKLY", args ,true ,false);
        }

        public void OpenClosedBerry(string db, int canvEdition, string startWeek, string endWeek)
        {
            oraCon = new OracleDBConnection(oraConnName);
            List<DbParameter> args = new List<DbParameter>();
            args.Add(oraCon.getNewParameter("IN_CANV_EDITION", canvEdition));
            args.Add(oraCon.getNewParameter("IN_START_DATE", startWeek));
            args.Add(oraCon.getNewParameter("IN_END_DATE", endWeek));
            args.Add(oraCon.getNewParameter("DB", db));
            oraCon.ExecuteStatementWithCursor("berry.OPEN_CLOSED_WEEKLY", args, true, false);
        }

        #endregion Berry Closure Process

        #region Berry Opening Process
        public DataTable GetCloseWeeks()
        {
            oraCon = new OracleDBConnection(oraConnName);
            List<DbParameter> args = new List<DbParameter>();
            args.Add(oraCon.getNewCursorParameter("resultset"));
            DataTable dt = oraCon.ExecuteStatementWithCursor("berry.GET_CLOSE_WEEK", args, true, true);
            return dt;
        }

        public DataTable OpenWeek(string db, string canvCode, string canvEdition, string canvWeek)
        {
            oraCon = new OracleDBConnection(oraConnName);
            List<DbParameter> args = new List<DbParameter>();
            args.Add(oraCon.getNewParameter("pstr_canv_code", canvCode));
            args.Add(oraCon.getNewParameter("pstr_canv_edition", canvEdition));
            args.Add(oraCon.getNewParameter("pstr_week", canvWeek));
            args.Add(oraCon.getNewParameter("pstr_db", db));      
            args.Add(oraCon.getNewCursorParameter("resultset"));
            DataTable dt = oraCon.ExecuteStatementWithCursor("berry.OPEN_WEEK", args, true, true);
            return dt;
        }
        #endregion Berry Opening Process

        #region Billing Report
        public DataTable GetBillingPivotReport(int canvEdition, string startWeek, string endWeek)
        {
            oraCon = new OracleDBConnection(oraConnName);
            List<DbParameter> args = new List<DbParameter>();
            args.Add(oraCon.getNewParameter("pint_edicion", canvEdition));
            args.Add(oraCon.getNewParameter("pstr_start_week", startWeek));
            args.Add(oraCon.getNewParameter("pstr_end_week", endWeek));
            args.Add(oraCon.getNewCursorParameter("resultset"));
            DataTable dt = oraCon.ExecuteStatementWithCursor("berry.GET_BILLING_PIVOT_REPORT", args, true, true);
            return dt;
        }

        public DataTable GetBillingReport(int canvEdition, string startWeek, string endWeek)
        {
            oraCon = new OracleDBConnection(oraConnName);
            List<DbParameter> args = new List<DbParameter>();
            args.Add(oraCon.getNewParameter("pint_edicion", canvEdition));
            args.Add(oraCon.getNewParameter("pstr_start_week", startWeek));
            args.Add(oraCon.getNewParameter("pstr_end_week", endWeek));
            args.Add(oraCon.getNewCursorParameter("resultset"));
            DataTable dt = oraCon.ExecuteStatementWithCursor("berry.GET_BILLING_REPORT", args, true, true);
            return dt;
        }

        #endregion Billing Report

        #region Pay Period Report
        public DataTable GetPayPeriod(int canvEdition)
        {
            oraCon = new OracleDBConnection(oraConnName);
            List<DbParameter> args = new List<DbParameter>();
            args.Add(oraCon.getNewParameter("pint_edicion", canvEdition));
            args.Add(oraCon.getNewCursorParameter("resultdata"));
            DataTable dt = oraCon.ExecuteStatementWithCursor("berry.GET_PAY_PERIOD", args, true, true);
            return dt;
        }

        public DataSet GetPayPeriodPivotReport(int canvEdition, int periodID, string rol, int employeeID)
        {
            DataSet ds = new DataSet();
            try
            {
                oraCon = new OracleDBConnection(oraConnName);
                List<DbParameter> args = new List<DbParameter>();
                args.Add(oraCon.getNewParameter("pint_edicion", canvEdition));
                args.Add(oraCon.getNewParameter("pint_period_id", periodID));
                args.Add(oraCon.getNewParameter("pstr_rol", rol));
                args.Add(oraCon.getNewParameter("pint_employee", employeeID));
                args.Add(oraCon.getNewCursorParameter("pivotReport"));
                args.Add(oraCon.getNewCursorParameter("detailReport"));
                ds = oraCon.ExecuteStatementDS("berry.GET_PAY_PERIOD_REPORT", args);
            }
            catch (Exception e)
            {
                throw;

            }
            return ds;
        }


        #endregion Pay Period Report

        #region Berry Creator Week

        public string CreateCanvWeek(int canvEdition, string canvCode, DateTime endDate)
        {
            string resultado = "Error - No se pudo realizar la transacción"; 
            security = new Security();
            oraCon = new OracleDBConnection(oraConnName);
            List<DbParameter> args = new List<DbParameter>();
            args.Add(oraCon.getNewParameter("pstr_canv_code", canvCode));
            args.Add(oraCon.getNewParameter("pstr_canv_edition", canvEdition));
            args.Add(oraCon.getNewParameter("pdt_end_week", endDate));
            args.Add(oraCon.getNewParameter("pstr_lastmodby", security.GetWinUser()));
            args.Add(oraCon.getNewCursorParameter("resultset"));
            DataTable dt = oraCon.ExecuteStatementWithCursor("berry.CREATE_CANV_WEEK", args, true, true);

            if(dt.Rows.Count >= 1){
                resultado = dt.Rows[0][0].ToString();
            }


            return resultado;
        }
        #endregion

        public DataTable GetHistDetail(int canvEdition, string canvCode, string db, int subscrID)
        {
            oraCon = new OracleDBConnection(oraConnName);
            List<DbParameter> args = new List<DbParameter>();
            args.Add(oraCon.getNewParameter("pint_edicion", canvEdition));
            args.Add(oraCon.getNewParameter("pstr_canv_code", canvCode));
            args.Add(oraCon.getNewParameter("pstr_db", db));
            args.Add(oraCon.getNewParameter("pint_subscr_id", subscrID));
            args.Add(oraCon.getNewCursorParameter("resultdata"));
            DataTable dt = oraCon.ExecuteStatementWithCursor("berry.GET_HIST_DETAIL_REPORT", args, true, true);
            return dt;
        }

        public DataTable GetActiveCanvass()
        {
            oraCon = new OracleDBConnection(oraConnName);
            List<DbParameter> args = new List<DbParameter>();
            args.Add(oraCon.getNewCursorParameter("resultdata"));
            DataTable dt = oraCon.ExecuteStatementWithCursor("berry.GET_ACTIVE_CANV_CODE", args, true, true);
            return dt;
        }

        public DataTable GetActiveRepSales()
        {
            oraCon = new OracleDBConnection(oraConnName);
            List<DbParameter> args = new List<DbParameter>();
            args.Add(oraCon.getNewCursorParameter("resultdata"));
            DataTable dt = oraCon.ExecuteStatementWithCursor("berry.GET_ACTIVE_REP_SALES", args, true, true);
            return dt;
        }

        public DataTable GetGrpCodeList()
        {
            oraCon = new OracleDBConnection(oraConnName);
            List<DbParameter> args = new List<DbParameter>();
            args.Add(oraCon.getNewCursorParameter("resultdata"));
            DataTable dt = oraCon.ExecuteStatementWithCursor("berry.GET_PROD_GRP_LIST", args, true, true);
            return dt;
        }

        public DataTable GetCanvWeekDB(int canvEdition, string canvCode, string db)
        {
            oraCon = new OracleDBConnection(oraConnName);
            List<DbParameter> args = new List<DbParameter>();
            args.Add(oraCon.getNewParameter("pint_edicion", canvEdition));
            args.Add(oraCon.getNewParameter("pstr_canv_code", canvCode));
            args.Add(oraCon.getNewParameter("pstr_db", db));
            args.Add(oraCon.getNewCursorParameter("resultdata"));
            DataTable dt = oraCon.ExecuteStatementWithCursor("berry.GET_ACTIVE_CANV_WEEK_DB", args, true, true);
            return dt;
        }
     
        #region Action
        public DataTable InsertCommHistTrans(CommisionHistTrans transaction)
        {
            security = new Security();

            oraCon = new OracleDBConnection(oraConnName);
            List<DbParameter> args = new List<DbParameter>();
            args.Add(oraCon.getNewParameter("ln_subscr_id", transaction.Subscr_ID));
            args.Add(oraCon.getNewParameter("lc_prod_grp_code", transaction.Prod_Grp_Code));
            args.Add(oraCon.getNewParameter("lc_Canv_code", transaction.Canv_Code));
            args.Add(oraCon.getNewParameter("ln_canv_edition", transaction.Canv_Edition));
            args.Add(oraCon.getNewParameter("ln_canv_week", transaction.Canv_Week));
            args.Add(oraCon.getNewParameter("ln_employeeid", transaction.Employee_id));
            args.Add(oraCon.getNewParameter("ln_PROD_CO", transaction.Prod_Co));
            args.Add(oraCon.getNewParameter("ln_prod_ci", transaction.Prod_Ci));
            args.Add(oraCon.getNewParameter("ln_RENEW_CI", transaction.Renew_Ci));
            args.Add(oraCon.getNewParameter("ln_INCREASE_CI", transaction.Increase_Ci));
            args.Add(oraCon.getNewParameter("ln_DECREASE_CI", transaction.Decrease_Ci));
            args.Add(oraCon.getNewParameter("ln_NP_CI", transaction.Np_Ci));
            args.Add(oraCon.getNewParameter("ln_OP_CI", transaction.Op_Ci));
            args.Add(oraCon.getNewParameter("ln_OTC_CI", transaction.Otc_Ci));
            args.Add(oraCon.getNewParameter("ln_CTRL_LOSS", transaction.Ctrl_Loss));
            args.Add(oraCon.getNewParameter("ln_UNCTRL_LOSS", transaction.Unctrl_Loss));
            args.Add(oraCon.getNewParameter("ln_db", transaction.DB));
            args.Add(oraCon.getNewParameter("ln_user", security.GetWinUser()));
            args.Add(oraCon.getNewCursorParameter("resultdata"));
            DataTable dt = oraCon.ExecuteStatementWithCursor("berry.INSERT_TRANSACTION", args, true, true);
            return dt;
        }

        public string UpdateCommHistTrans(CommisionHistTrans transaction)
        {
            security = new Security();
            string comment = string.Empty;

            oraCon = new OracleDBConnection(oraConnName);
            List<DbParameter> args = new List<DbParameter>();
            args.Add(oraCon.getNewParameter("ln_subscr_id", transaction.Subscr_ID));
            args.Add(oraCon.getNewParameter("lc_prod_grp_code", transaction.Prod_Grp_Code));
            args.Add(oraCon.getNewParameter("lc_Canv_code", transaction.Canv_Code));
            args.Add(oraCon.getNewParameter("ln_canv_edition", transaction.Canv_Edition));
            args.Add(oraCon.getNewParameter("ln_canv_week", transaction.Canv_Week));
            args.Add(oraCon.getNewParameter("ln_assignment_no", transaction.Assigment_No));
            args.Add(oraCon.getNewParameter("ln_employee_name", transaction.Employee_Name));
            args.Add(oraCon.getNewParameter("ln_PROD_CO", transaction.Prod_Co));
            args.Add(oraCon.getNewParameter("ln_prod_ci", transaction.Prod_Ci));
            args.Add(oraCon.getNewParameter("ln_RENEW_CI", transaction.Renew_Ci));
            args.Add(oraCon.getNewParameter("ln_INCREASE_CI", transaction.Increase_Ci));
            args.Add(oraCon.getNewParameter("ln_DECREASE_CI", transaction.Decrease_Ci));
            args.Add(oraCon.getNewParameter("ln_NP_CI", transaction.Np_Ci));
            args.Add(oraCon.getNewParameter("ln_OP_CI", transaction.Op_Ci));
            args.Add(oraCon.getNewParameter("ln_OTC_CI", transaction.Otc_Ci));
            args.Add(oraCon.getNewParameter("ln_CTRL_LOSS", transaction.Ctrl_Loss));
            args.Add(oraCon.getNewParameter("ln_UNCTRL_LOSS", transaction.Unctrl_Loss));
            args.Add(oraCon.getNewParameter("lc_key", transaction.Key));
            args.Add(oraCon.getNewParameter("ln_db", transaction.DB));
            args.Add(oraCon.getNewParameter("ln_user", security.GetWinUser()));
            args.Add(oraCon.getNewCursorParameter("resultdata"));
            DataTable dt = oraCon.ExecuteStatementWithCursor("berry.UPDATE_COMM_TRANSACTION", args, true, true);

            if (dt.Rows.Count > 0)
            {
                comment = dt.Rows[0][0].ToString();
            }

            return comment;
        }

        public string DeleteCommHistTrans(CommisionHistTrans transaction)
        {
            security = new Security();
            string comment = string.Empty;

            oraCon = new OracleDBConnection(oraConnName);
            List<DbParameter> args = new List<DbParameter>();
            args.Add(oraCon.getNewParameter("lc_key", transaction.Key));
            args.Add(oraCon.getNewParameter("ln_db", transaction.DB));
            args.Add(oraCon.getNewParameter("ln_user", security.GetWinUser()));
            args.Add(oraCon.getNewCursorParameter("resultdata"));

            DataTable dt = oraCon.ExecuteStatementWithCursor("berry.DELETE_COMM_TRANSACTION", args, true, true);

            if (dt.Rows.Count > 0)
            {
                comment = dt.Rows[0][0].ToString();
            }

            return comment;

        }

        #endregion Action

        #region Common
        public DataTable GetActiveCanvEdition()
        {
            oraCon = new OracleDBConnection(oraConnName);
            List<DbParameter> args = new List<DbParameter>();
            args.Add(oraCon.getNewCursorParameter("resultdata"));
            DataTable dt = oraCon.ExecuteStatementWithCursor("berry.GET_EDITION", args, true, true);
            return dt;
        }

        public DataTable GetActiveDateByEdition(string db, int canvEdition)
        {
            oraCon = new OracleDBConnection(oraConnName);
            List<DbParameter> args = new List<DbParameter>();
            args.Add(oraCon.getNewParameter("DB", db));
            args.Add(oraCon.getNewParameter("IN_EDITION", canvEdition));
            args.Add(oraCon.getNewCursorParameter("RESULTSET"));
            DataTable dt = oraCon.ExecuteStatementWithCursor("berry.AUTCMP_CALENDAR_DATE", args, true, true);
            return dt;
        }

        public DataTable GetClosedDateByEdition(string db, int canvEdition)
        {
            oraCon = new OracleDBConnection(oraConnName);
            List<DbParameter> args = new List<DbParameter>();
            args.Add(oraCon.getNewParameter("DB", db));
            args.Add(oraCon.getNewParameter("IN_EDITION", canvEdition));
            args.Add(oraCon.getNewCursorParameter("RESULTSET"));
            DataTable dt = oraCon.ExecuteStatementWithCursor("berry.AUTCMP_CLOSE_CALENDAR_DATE", args, true, true);
            return dt;
        }

        public DataTable GetCloseWeekByDate(string db, int canvEdition, string startDate, string endDate)
        {
            oraCon = new OracleDBConnection(oraConnName);
            List<DbParameter> args = new List<DbParameter>();
            args.Add(oraCon.getNewParameter("DB", db));
            args.Add(oraCon.getNewParameter("IN_CANV_EDITION", canvEdition));
            args.Add(oraCon.getNewParameter("IN_START_DATE", startDate));
            args.Add(oraCon.getNewParameter("IN_END_DATE", endDate));
            args.Add(oraCon.getNewCursorParameter("resultset"));
            DataTable dt = oraCon.ExecuteStatementWithCursor("berry.GET_CLOSED_WKL_BY_DATE", args, true, true);
            return dt;
        }

        public User GetRoles(string userName, string moduleCode)
        {
            sqlCon = new MSSQLConnection("BerrySQLDBEVA_RD");
            List<DbParameter> args = new List<DbParameter>();
            args.Add(sqlCon.getNewParameter("@userCode", ""));
            args.Add(sqlCon.getNewParameter("@userName", userName));
            args.Add(sqlCon.getNewParameter("@pyc_codigo", moduleCode));
            DataTable dt = sqlCon.ExecuteStatement("dbo.sp_GetRoles", args);
            
            User user = null;
            if (dt != null && dt.Rows.Count > 0)
            {
                user = new User();
                user.Roles = new List<string>();

                user.UserId = Convert.ToInt32(dt.Rows[0]["usr_codigo"].ToString());
                user.UserName = userName;

                foreach (DataRow item in dt.Rows)
                {
                    user.Roles.Add(item["grp_codigo"].ToString());
                }
            }

            return user;
        }
        #endregion Common

        public DataTable GetUnitSalesDetail(int canvEdition, string startWeek, string endWeek)
        {
            oraCon = new OracleDBConnection(oraConnName);
            List<DbParameter> args = new List<DbParameter>();
            args.Add(oraCon.getNewParameter("pint_edicion", canvEdition));
            args.Add(oraCon.getNewParameter("pstr_start_week", startWeek));
            args.Add(oraCon.getNewParameter("pstr_end_week", endWeek));
            args.Add(oraCon.getNewCursorParameter("resultset"));

            DataTable dt = oraCon.ExecuteStatementWithCursor("BERRY.GET_UNIT_SALES_DETAIL", args, true, true);

            return dt;
        }

        #region RegionalReport
        public DataTable GetRegionSalesDetail(int canvEdition, string startWeek, string endWeek)
        {
            oraCon = new OracleDBConnection(oraConnName);
            List<DbParameter> args = new List<DbParameter>();
            args.Add(oraCon.getNewParameter("pint_edicion", canvEdition));
            args.Add(oraCon.getNewParameter("pstr_start_week", startWeek));
            args.Add(oraCon.getNewParameter("pstr_end_week", endWeek));
            args.Add(oraCon.getNewCursorParameter("resultset"));

            DataTable dt = oraCon.ExecuteStatementWithCursor("BERRY.GET_REGION_SALES_DETAIL", args, true, true);

            return dt;
        }

        public DataTable GetOfficeDetail(int canvEdition, string startWeek, string endWeek)
        {
            oraCon = new OracleDBConnection(oraConnName);
            List<DbParameter> args = new List<DbParameter>();
            args.Add(oraCon.getNewParameter("pint_edicion", canvEdition));
            args.Add(oraCon.getNewParameter("pstr_start_week", startWeek));
            args.Add(oraCon.getNewParameter("pstr_end_week", endWeek));
            args.Add(oraCon.getNewCursorParameter("resultset"));

            DataTable dt = oraCon.ExecuteStatementWithCursor("BERRY.GET_OFFICE_SALES_DETAIL", args, true, true);

            return dt;
        }

        public DataTable GetVisitsDetail(int canvEdition, string startWeek, string endWeek)
        {
            oraCon = new OracleDBConnection(oraConnName);
            List<DbParameter> args = new List<DbParameter>();
            args.Add(oraCon.getNewParameter("pint_edicion", canvEdition));
            args.Add(oraCon.getNewParameter("pstr_start_week", startWeek));
            args.Add(oraCon.getNewParameter("pstr_end_week", endWeek));
            args.Add(oraCon.getNewCursorParameter("resultset"));

            DataTable dt = oraCon.ExecuteStatementWithCursor("BERRY.GET_VISITS_DETAIL", args, true, true);

            return dt;
        }

     #endregion


        #region CanvWeekController

        /// <summary>
        /// Retorna las semanas abiertas.
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetOpenedWeek()
        {
            oraCon = new OracleDBConnection(oraConnName);
            List<DbParameter> args = new List<DbParameter>();
            args.Add(oraCon.getNewCursorParameter("resultset"));
            DataTable dt = oraCon.ExecuteStatementWithCursor("berry.GET_OPENED_WEEK", args, true, true);

            return dt;
        }

        /// <summary>
        /// Permite cerrar una semana
        /// </summary>
        /// <param name="database">BR, O YP</param>
        /// <param name="startWeek">fecha de inicio</param>
        /// <param name="endWeek">fecha final</param>
        public void CloseWeek(string startWeek, string endWeek, int week)
        {
            oraCon = new OracleDBConnection(oraConnName);
            List<DbParameter> args = new List<DbParameter>();
            args.Add(oraCon.getNewParameter("IN_START_DATE", startWeek));
            args.Add(oraCon.getNewParameter("IN_END_DATE", endWeek));
            args.Add(oraCon.getNewParameter("IN_WEEK", week));
            oraCon.ExecuteStatementWithCursor("berry.CLOSE_WEEK", args, true, false);
        }

        /// <summary>
        /// Retorna las semanas cerradas.
        /// </summary>
        /// <param name="database">BR, O YP</param>
        /// <returns>DataTable</returns>
        public DataTable GetClosedWeek()
        {
            oraCon = new OracleDBConnection(oraConnName);
            List<DbParameter> args = new List<DbParameter>();

            args.Add(oraCon.getNewCursorParameter("resultset"));

            DataTable dt = oraCon.ExecuteStatementWithCursor("berry.GET_CLOSED_WEEK", args, true, true);

            return dt;
        }

        /// <summary>
        /// Permite abrir una semana
        /// </summary>
        /// <param name="database">BR, O YP</param>
        /// <param name="startWeek">fecha de inicio</param>
        /// <param name="endWeek">fecha final</param>
        public void OpenWeek(string startWeek, string endWeek, int week)
        {
            oraCon = new OracleDBConnection(oraConnName);
            List<DbParameter> args = new List<DbParameter>();
            args.Add(oraCon.getNewParameter("IN_START_DATE", startWeek));
            args.Add(oraCon.getNewParameter("IN_END_DATE", endWeek));
            args.Add(oraCon.getNewParameter("IN_WEEK", week));
            oraCon.ExecuteStatementWithCursor("berry.OPEN_WEEK", args, true, false);
        }

        /// <summary>
        /// Obtiene los canv_week
        /// </summary>
        /// <param name="database"></param>
        /// <returns>DataTable</returns>
        public DataTable GetWeekByDb()
        {
            oraCon = new OracleDBConnection(oraConnName);
            List<DbParameter> args = new List<DbParameter>();
            args.Add(oraCon.getNewCursorParameter("resultset"));

            DataTable dt = oraCon.ExecuteStatementWithCursor("berry.AUTCMP_WEEK_BY_DB", args, true, true);

            return dt;
        }

        /// <summary>
        /// Obtiene los canv_week
        /// </summary>
        /// <param name="database"></param>
        /// <returns>DataTable</returns>
        public DataTable GetWeekDetialByDb(string database)
        {
            oraCon = new OracleDBConnection(oraConnName);
            List<DbParameter> args = new List<DbParameter>();
            args.Add(oraCon.getNewParameter("DB", database));
            args.Add(oraCon.getNewCursorParameter("resultset"));

            DataTable dt = oraCon.ExecuteStatementWithCursor("berry.AUTCMP_WEEKDETAILS_BY_DB", args, true, true);

            return dt;
        }

        /// <summary>
        /// Optine las fechas conrespondiente al canvweek
        /// </summary>
        /// <param name="db"></param>
        /// <param name="canvEdition"></param>
        /// <returns>DataTable</returns>
        public DataTable GetActiveDateByCanvWeek(int week)
        {
            oraCon = new OracleDBConnection(oraConnName);
            List<DbParameter> args = new List<DbParameter>();
            args.Add(oraCon.getNewParameter("IN_WEEK", week));
            args.Add(oraCon.getNewCursorParameter("RESULTSET"));
            DataTable dt = oraCon.ExecuteStatementWithCursor("berry.AUTCMP_ClD_DATE_BY_WEEK", args, true, true);
            return dt;
        }

        /// <summary>
        /// Optine los registros de weekly por fecha
        /// </summary>
        /// <param name="db"></param>
        /// <param name="canvEdition"></param>
        /// <returns>DataTable</returns>
        //public DataTable GetOpenedWeekByDate(string database, int week, string startDate, string endDate)
        //{
        //    oraCon = new OracleDBConnection(oraConnName);
        //    List<DbParameter> args = new List<DbParameter>();
        //    args.Add(oraCon.getNewParameter("DB", database));
        //    args.Add(oraCon.getNewParameter("IN_WEEK", week));
        //    args.Add(oraCon.getNewParameter("IN_START_DATE", startDate));
        //    args.Add(oraCon.getNewParameter("IN_END_DATE", endDate));
        //    args.Add(oraCon.getNewCursorParameter("resultset"));
        //    DataTable dt = oraCon.ExecuteStatementWithCursor("berry.GET_OPENED_WKL_BY_DATE", args, true, true);
        //    return dt;
        //}

        /// <summary>
        /// Obtiene los detalles de los canv_week por fecha.
        /// </summary>
        /// <param name="database"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns>DataTable</returns>
        public DataTable GetOpenedWeekDetailsByDate(string startDate, string endDate)
        {
            oraCon = new OracleDBConnection(oraConnName);
            List<DbParameter> args = new List<DbParameter>();
            args.Add(oraCon.getNewParameter("IN_START_DATE", startDate));
            args.Add(oraCon.getNewParameter("IN_END_DATE", endDate));
            args.Add(oraCon.getNewCursorParameter("RESULTSET"));
            DataTable dt = oraCon.ExecuteStatementWithCursor("berry.GET_CANV_WEEK_DETAILS", args, true, true);
            return dt;
        }

        /// <summary>
        /// Obtiene un little reporte despues de que corre berry.
        /// </summary>
        /// <param name="database"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns>DataTable</returns>
        public DataTable GetPostGeneratedBerryByDate(string startDate, string endDate)
        {
            oraCon = new OracleDBConnection(oraConnName);
            List<DbParameter> args = new List<DbParameter>();
            args.Add(oraCon.getNewParameter("IN_START_DATE", startDate));
            args.Add(oraCon.getNewParameter("IN_END_DATE", endDate));
            args.Add(oraCon.getNewCursorParameter("RESULTSET"));
            DataTable dt = oraCon.ExecuteStatementWithCursor("berry.GET_POST_BERRY_GN", args, true, true);
            return dt;
        }

        /// <summary>
        /// Obtiene un little reporte despues de que corre berry.
        /// </summary>
        /// <param name="database"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns>DataTable</returns>
        public DataTable GetPostGeneratedBerryBarByDate(string startDate, string endDate)
        {
            oraCon = new OracleDBConnection(oraConnName);
            List<DbParameter> args = new List<DbParameter>();
            args.Add(oraCon.getNewParameter("IN_START_DATE", startDate));
            args.Add(oraCon.getNewParameter("IN_END_DATE", endDate));
            args.Add(oraCon.getNewCursorParameter("RESULTSET"));
            DataTable dt = oraCon.ExecuteStatementWithCursor("berry.GET_POST_BERRY_GN_BAR", args, true, true);
            return dt;
        }
        #endregion

        /// <summary>
        /// Escribe el reporte postberrygenerated a excel.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="sheetName"></param>
        /// <param name="fileName"></param>
        /// <param name="savingPath"></param>
        /// <returns></returns>
        public string ExportPostGeneratedWeeklyReport(string startDate, string endDate, string sheetName, string fileName, string savingPath)
        {
            oraCon = new OracleDBConnection(oraConnName);
            List<DbParameter> args = new List<DbParameter>();
            args.Add(oraCon.getNewParameter("IN_START_DATE", startDate));
            args.Add(oraCon.getNewParameter("IN_END_DATE", endDate));
            args.Add(oraCon.getNewCursorParameter("RESULTSET"));
            DataTable dt = oraCon.ExecuteStatementWithCursor("berry.GET_POST_BERRY_GN_RPORT", args, true, true);

            return WriteToExcel( dt, sheetName, fileName, savingPath);
        }

        /// <summary>
        /// Generar un archivo de excel a partir de un datatable
        /// </summary>
        /// <param name="table">Datatle</param>
        /// <param name="sheetName">Nombre de la hoja de excel</param>
        /// <param name="fileName">Nombre de archivo de excel</param>
        /// <param name="savingPath">Ruta donde se almacena el archivo</param>
        /// <returns></returns>
        public string WriteToExcel(DataTable table, string sheetName, string fileName, string savingPath)
        {
            string archivo = savingPath + fileName + ".xlsx";
            List<string> header = GetDataTableHeader(table.Columns);

            using (SpreadsheetDocument workbook = SpreadsheetDocument.Create(archivo, SpreadsheetDocumentType.Workbook))
            {
                OpenXmlWriter writer;

                workbook.AddWorkbookPart();
                WorksheetPart wsp = workbook.WorkbookPart.AddNewPart<WorksheetPart>();

                writer = OpenXmlWriter.Create(wsp);
                writer.WriteStartElement(new Worksheet());
                writer.WriteStartElement(new SheetData());

                WriteExcelHeader(header.ToArray(), writer);
                WriteExcelValues(table, header, writer);

                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.Close();

                writer = OpenXmlWriter.Create(workbook.WorkbookPart);
                writer.WriteStartElement(new Workbook());
                writer.WriteStartElement(new Sheets());

                writer.WriteElement(new Sheet()
                {
                    Name = sheetName,
                    SheetId = 1,
                    Id = workbook.WorkbookPart.GetIdOfPart(wsp)
                });

                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.Close();

                workbook.Close();
            }

            return archivo;
        }

        /// <summary>
        /// Escribe el las columnas de datatable, al header del excel 
        /// </summary>
        /// <param name="header">Lista con las columnas del datatable</param>
        /// <param name="writer">Objeto excel sobre el que se escribira</param>
        public void WriteExcelHeader(string[] header, OpenXmlWriter writer)
        {
            List<OpenXmlAttribute> row = new List<OpenXmlAttribute> { new OpenXmlAttribute("r", null, "1") };
            writer.WriteStartElement(new Row(), row);
            List<OpenXmlAttribute> cell = new List<OpenXmlAttribute> { new OpenXmlAttribute("t", null, "inlineStr") };
            foreach (string h in header)
            {
                writer.WriteStartElement(new Cell(), cell);
                writer.WriteElement(new InlineString(new Text(h)));
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }

        /// <summary>
        /// Escribe los rows de un datatbale a excel
        /// </summary>
        /// <param name="table">Datatle</param>
        /// <param name="header">Lista con las columnas del datatable</param>
        /// <param name="writer">Objeto excel sobre el que se escribira</param>
        public void WriteExcelValues(DataTable table, List<string> header, OpenXmlWriter writer)
        {
            List<OpenXmlAttribute> row;
            List<OpenXmlAttribute> cell = new List<OpenXmlAttribute> { new OpenXmlAttribute("t", null, "inlineStr") };
            //List<OpenXmlAttribute> intCell = new List<OpenXmlAttribute> { new OpenXmlAttribute("t", null, "n") };

            for (int i = 1; i < table.Rows.Count; i++)
            {
                row = new List<OpenXmlAttribute> { new OpenXmlAttribute("r", null, (i + 1).ToString()) };
                writer.WriteStartElement(new Row(), row);

                foreach (string th in header)
                {
                    writer.WriteStartElement(new Cell(), cell);
                    writer.WriteElement(new InlineString(new Text(table.Rows[i-1][th].ToString())));
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
            }
        }

        /// <summary>
        /// Extrae las columnas de un datatable, y lo escribe a una lista de string
        /// </summary>
        /// <param name="columns">Las columnas del datatable</param>
        /// <returns>List<string></returns>
        private List<string> GetDataTableHeader(DataColumnCollection columns)
        {
            List<string> header = new List<string>();
            foreach (DataColumn dtc in columns)
            {
                header.Add(dtc.ColumnName);
            }

            return header;
        }

        /// <summary>
        /// Obtiene la ultima semana gerada
        /// </summary>
        /// <param name="database"></param>
        /// <returns>DataTable</returns>
        public DataTable GetLastGeneratedWeek()
        {
            oraCon = new OracleDBConnection(oraConnName);
            List<DbParameter> args = new List<DbParameter>();
            args.Add(oraCon.getNewCursorParameter("resultset"));

            DataTable dt = oraCon.ExecuteStatementWithCursor("berry.GET_LAST_RUN", args, true, true);

            return dt;
        }

        public DataTable GetDashboardRnCount()
        {
            oraCon = new OracleDBConnection(oraConnName);
            List<DbParameter> args = new List<DbParameter>();
            args.Add(oraCon.getNewCursorParameter("resultset"));

            DataTable dt = oraCon.ExecuteStatementWithCursor("berry.GET_DASBOARD_NR_COUNT", args, true, true);

            return dt;
        }
    }
}