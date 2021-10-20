using IIITS.PGSQL.DAL;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace InventoryProduct.Models
{
    public class InventoryDetails
    {
        public string ItemName { get; set; }
        public string ItemDesc { get; set; }
        public int? ItemPrice { get; set; }
        public string InventoryName { get; set; }
        public int InventoryId { get; set; }
        public string ItemCode { get; set; }
        public decimal? NumUnits { get; set; }
        public string UnitMeasureVal { get; set; }
        public int UnitMeasureId { get; set; }
        public string Message { get; set; }
        public int UserId { get; set; }
        public int PkId { get; set; }
        public List<SelectListItem> ListUnitMeasure { get; set; } = new List<SelectListItem>();

        public void FetchMasterDetails()
        {
            DataTable dtable = new DataTable();
            try
            {
                PGSqlConnection ObjCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));
                NpgsqlCommand cmd = new NpgsqlCommand("sp_get_unit_dropdown_details");
                dtable = ObjCon.FetchDataTable(cmd);
                ListUnitMeasure = GetComboDetails(dtable);

                string[] sResult = new string[3];

                cmd = new NpgsqlCommand("fetch_inventory_details");
                cmd.Parameters.Add("invetory_id", NpgsqlDbType.Text);
                cmd.Parameters.Add("invetory_name", NpgsqlDbType.Text);
                cmd.Parameters["invetory_id"].Direction = ParameterDirection.Output;
                cmd.Parameters["invetory_name"].Direction = ParameterDirection.Output;
                sResult[0] = "invetory_id";
                sResult[1] = "invetory_name";
                sResult = ObjCon.Execute(cmd, sResult, 2);
                InventoryId = Convert.ToInt32(sResult[0]);
                InventoryName = sResult[1];

            }
            catch (Exception ex)
            {
                clsErrorLog.LogError(ex.Message, MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().DeclaringType.Name, ex.StackTrace);
                throw ex;
            }
        }

        public static List<SelectListItem> GetComboDetails(DataTable dt)
        {
            List<SelectListItem> lstData = new List<SelectListItem>();

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dRow in dt.Rows)
                {
                    lstData.Add(new SelectListItem { Text = Convert.ToString(dRow[1]), Value = Convert.ToString(dRow[0]) });
                }
            }

            return lstData;
        }

        public InventoryDetails SaveInventoryItemDetails(InventoryDetails Obj)
        {
            try
            {
                PGSqlConnection ObjCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));
                string[] sResult = new string[1];

                if(ItemDesc == null)
                {
                    ItemDesc = "";
                }
                NpgsqlCommand cmd = new NpgsqlCommand("insert_item_details");
                cmd.Parameters.AddWithValue("pkid", PkId);
                cmd.Parameters.AddWithValue("t_iid_ind_id", InventoryId);
                cmd.Parameters.AddWithValue("t_iid_item_code", ItemCode);
                cmd.Parameters.AddWithValue("t_iid_item_name", ItemName);
                cmd.Parameters.AddWithValue("t_iid_item_desc", ItemDesc);
                cmd.Parameters.AddWithValue("t_iid_item_price", ItemPrice);
                cmd.Parameters.AddWithValue("t_iid_umt_id", UnitMeasureId);
                cmd.Parameters.AddWithValue("t_iid_num_units", Convert.ToDecimal(NumUnits));
                cmd.Parameters.AddWithValue("t_iid_created_by", UserId);
                cmd.Parameters.Add("status", NpgsqlDbType.Text);
                cmd.Parameters["status"].Direction = ParameterDirection.Output;
                sResult[0] = "status";
                sResult = ObjCon.Execute(cmd, sResult, 1);
                Message = sResult[0];

            }
            catch (Exception ex)
            {
                clsErrorLog.LogError(ex.Message, MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().DeclaringType.Name, ex.StackTrace);
                throw ex;
            }
            return Obj;
        }

        public DataTable FetchInventoryItemDetails(int PkId)
        {
            DataTable dtable = new DataTable();
            try
            {
                PGSqlConnection ObjCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));
                NpgsqlCommand cmd = new NpgsqlCommand("fetch_inventory_item_details");
                cmd.Parameters.AddWithValue("pkid", PkId);
                dtable = ObjCon.FetchDataTable(cmd);
            }
            catch (Exception ex)
            {
                clsErrorLog.LogError(ex.Message, MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().DeclaringType.Name, ex.StackTrace);
                throw ex;
            }
            return dtable;
        }

        public int DeleteVal(int PkId)
        {
            int StatusId = 0;
            try
            {
                PGSqlConnection ObjCon = new PGSqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["pgSQLPassword"]));
                string sQry = "DELETE FROM TBL_INVOICE_ITEM_DETAILS WHERE IID_ID = " + PkId + "";
                ObjCon.ExecuteQry(sQry);
            }
            catch (Exception ex)
            {
                StatusId = -1;
                clsErrorLog.LogError(ex.Message, MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().DeclaringType.Name, ex.StackTrace);
                throw ex;
            }
            return StatusId;
        }
    }
}