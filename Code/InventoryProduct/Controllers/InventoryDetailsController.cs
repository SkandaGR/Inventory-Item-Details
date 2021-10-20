using InventoryProduct.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace InventoryProduct.Controllers
{
    public class InventoryDetailsController : Controller
    {
        // GET: InventoryDetails
        public ActionResult InventoryDetails()
        {
            InventoryDetails Obj = new InventoryDetails();
            try
            {
                Obj.UserId = 1; //Should be from Session while logged in.
                Obj.FetchMasterDetails();
            }
            catch (Exception ex)
            {
                Obj.Message = "Something Went Wrong!";
                clsErrorLog.LogError(ex.Message,  MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().DeclaringType.Name, ex.StackTrace);
            }
            return View(Obj);
        }

        [HttpPost]
        public ActionResult InventoryDetails(InventoryDetails Obj)
        {
            try
            {
                Obj.UserId = 1; //Should be from Session while logged in.
                Obj = Obj.SaveInventoryItemDetails(Obj);
            }
            catch (Exception ex)
            {
                Obj.Message = "Something Went Wrong!";
                clsErrorLog.LogError(ex.Message, MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().DeclaringType.Name, ex.StackTrace);
            }
            return View(Obj);
        }

        public ActionResult FetchInventoryItemDetails()
        {
            InventoryDetails Obj = new InventoryDetails();
            List<InventoryDetails> ListInventory = new List<InventoryDetails>();
            try
            {
                DataTable dtable = Obj.FetchInventoryItemDetails(0);
                ListInventory = (from dr in dtable.AsEnumerable()
                                select new InventoryDetails()
                                {
                                    PkId = Convert.ToInt32(dr["t_ind_id"]),
                                    InventoryName = Convert.ToString(dr["t_ind_name"]),
                                    ItemCode = Convert.ToString(dr["t_iid_item_code"]),
                                    ItemName = Convert.ToString(dr["t_iid_item_name"]),
                                    ItemDesc = Convert.ToString(dr["t_iid_item_desc"]),
                                    ItemPrice = Convert.ToInt32(dr["t_iid_item_price"]),
                                    UnitMeasureVal = Convert.ToString(dr["t_umt_name"]),
                                    NumUnits = Convert.ToDecimal(dr["t_iid_num_units"])
                                }).ToList();
            }
            catch (Exception ex)
            {
                clsErrorLog.LogError(ex.Message, MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().DeclaringType.Name, ex.StackTrace);
            }
            return Json(new { data = ListInventory }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateVal(int PkId)
        {
            InventoryDetails Obj = new InventoryDetails();
            try
            {
                DataTable dtable = Obj.FetchInventoryItemDetails(PkId);
                if (dtable.Rows.Count > 0)
                {
                    Obj.PkId = Convert.ToInt32(dtable.Rows[0]["t_ind_id"]);
                    Obj.InventoryId = Convert.ToInt32(dtable.Rows[0]["t_iid_ind_id"]);
                    Obj.InventoryName = Convert.ToString(dtable.Rows[0]["t_ind_name"]);
                    Obj.ItemCode = Convert.ToString(dtable.Rows[0]["t_iid_item_code"]);
                    Obj.ItemName = Convert.ToString(dtable.Rows[0]["t_iid_item_name"]);
                    Obj.ItemDesc = Convert.ToString(dtable.Rows[0]["t_iid_item_desc"]);
                    Obj.ItemPrice = Convert.ToInt32(dtable.Rows[0]["t_iid_item_price"]);
                    Obj.UnitMeasureId = Convert.ToInt32(dtable.Rows[0]["t_iid_umt_id"]);
                    Obj.NumUnits = Convert.ToDecimal(dtable.Rows[0]["t_iid_num_units"]);
                }
            }
            catch (Exception ex)
            {
                clsErrorLog.LogError(ex.Message, MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().DeclaringType.Name, ex.StackTrace);
            }
            return Json(Obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteVal(int PkId)
        {
            InventoryDetails Obj = new InventoryDetails();
            int StatusId = 0;
            try
            {
                StatusId = Obj.DeleteVal(PkId);
            }
            catch (Exception ex)
            {
                clsErrorLog.LogError(ex.Message, MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().DeclaringType.Name, ex.StackTrace);
            }
            return Json(StatusId,JsonRequestBehavior.AllowGet);
        }
    }
}