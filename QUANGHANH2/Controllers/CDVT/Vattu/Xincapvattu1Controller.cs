﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Linq.Dynamic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using QUANGHANH2.SupportClass;
using QUANGHANH2.Models;
using System.Web.Script.Serialization;

namespace QUANGHANH2.Controllers.CDVT.Vattu
{
    public class Xincapvattu1Controller : Controller
    {
        // GET: Xincapvattu1
        [Auther(RightID = "33,27,179,180,181,183,184,185,186,187,189,195")]
        [Route("phan-xuong/xin-cap-vat-tu-sctx")]
        public ActionResult Index()
        {
            QUANGHANHABCEntities db = new QUANGHANHABCEntities();
            List<Supply> listSupply = db.Supplies.Where(x => x.unit != "L" && x.unit != "kWh").ToList();
            ViewBag.listSupply = listSupply;
            return View("/Views/CDVT/Vattu/Xincapvattulan1.cshtml");
        }

        [Route("phan-xuong/xin-cap-vat-tu-sctx/getinformation")]
        [HttpPost]
        public ActionResult GetInformation()
        {
            try
            {

                using (QUANGHANHABCEntities DBContext = new QUANGHANHABCEntities())
                {
                    // only taken by each department.
                    string department_id = Session["departID"].ToString();
                    List<Eq> listequipment;
                    int count = DBContext.SupplyPlans.Where(x => x.departmentid == department_id && x.date.Month == DateTime.Now.Month && x.status == 1).Count();
                    if (count > 0)
                    {
                        string query = "select equipmentId,equipment_name from Equipment inner join Department on Equipment.department_id=Department.department_id " +
                   " where Department.department_id='' ";
                        listequipment = DBContext.Database.SqlQuery<Eq>(query,
                          new SqlParameter("department_id", department_id)
                           ).ToList();
                    }
                    else
                    {


                        string query = "select equipmentId,equipment_name from Equipment inner join Department on Equipment.department_id=Department.department_id " +
                               " where Department.department_id=@department_id ";
                        listequipment = DBContext.Database.SqlQuery<Eq>(query,
                          new SqlParameter("department_id", department_id)
                           ).ToList();
                    }

                    int totalrows = listequipment.Count;
                    int totalrowsafterfiltering = listequipment.Count;


                    return Json(new { success = true, data = listequipment, draw = Request["draw"], recordsTotal = totalrows/*, recordsFiltered = totalrowsafterfiltering*/ }, JsonRequestBehavior.AllowGet);
                } }
            catch (Exception)
            {
                Response.Write("Có lỗi xảy ra, xin vui lòng nhập lại");
                return new HttpStatusCodeResult(400);
            } 
        }
        [Route("phan-xuong/xin-cap-vat-tu-sctx/getListSupply")]
        [HttpPost]
        public JsonResult getListSupply(String equipmentId)
        {
           using(QUANGHANHABCEntities db = new QUANGHANHABCEntities())
            { 

                {
                    List<SupplyPlanDB> m = db.Database.SqlQuery<SupplyPlanDB>("select supp.supplyid, s.supply_name,supp.dinh_muc, s.unit ,supp.quantity_plan,supp.id,(case when su.quantity is null then 0 else su.quantity end) 'quantity'  " +
                   "from Supply s inner join SupplyPlan supp on s.supply_id = supp.supplyid left join Supply_SCTX su on supp.equipmentid=su.equipmentId and supp.supplyid=su.supply_id  where supp.equipmentid = @equipmentid and month(date)=month(getdate()) and status=0", new SqlParameter("equipmentid", equipmentId)).ToList();

                    return Json(m);
                } }
        }

        [Auther(RightID = "33,179,180,181,183,184,185,186,187,189,195")]
        [Route("phan-xuong/xin-cap-vat-tu-sctx/editoradd")]
        [HttpPost]
        public ActionResult InsertInformation(String equipmentid)
        {

            string department_id = Session["departID"].ToString();
            var supplyid = Request["supplyid"];
            var xin_cap = Request["xin_cap"];
            var general = Request["general"];
            var supplyplanid = Request["supplyplanid"];
            JavaScriptSerializer js = new JavaScriptSerializer();
            string[] listsupplyid = js.Deserialize<string[]>(supplyid);
            string[] listxin_cap = js.Deserialize<string[]>(xin_cap);
            string[] listgeneral = js.Deserialize<string[]>(general);
            string[] listsupplyplanid = js.Deserialize<string[]>(supplyplanid);

            QUANGHANHABCEntities db = new QUANGHANHABCEntities();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    
                    string bulk_insert= string.Empty;
                   
                    
                    Equipment e = db.Equipments.Where(x => x.equipmentId == equipmentid).First();
                    for (int i = 0; i < listsupplyid.Length; i++)
                    {
                        string sub_insert = $"if exists (select * from SupplyPlan  where id='{listsupplyplanid[i]}')  " +
                                            " begin " +
                                           " update SupplyPlan set " +
                                           $" supplyid = '{listsupplyid[i]}' , date = getdate(),quantity_plan = {Int32.Parse(listxin_cap[i])},dinh_muc={double.Parse(listgeneral[i])} " +
                                          $" where id = '{listsupplyplanid[i]}'" +
                                          " end " +
                                          " else " +
                                         " begin  " +
                                         $" insert into Supplyplan(supplyid, departmentid,equipmentid, [date],dinh_muc, quantity_plan,quantity, [status]) VALUES('{listsupplyid[i]}', '{department_id}','{equipmentid}', getdate(),{double.Parse(listgeneral[i])} ,{Int32.Parse(listxin_cap[i])},0, 0) " +
                                         " end;  ";
                        bulk_insert = string.Concat(bulk_insert, sub_insert);
                    }
                    db.Database.ExecuteSqlCommand(bulk_insert);
                    db.SaveChanges();

                    transaction.Commit();
                    return Json(new { success = true, message = "Chỉnh sửa thành công" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    transaction.Rollback();

                    return new HttpStatusCodeResult(400);
                }
            }
        }
        [Auther(RightID = "33,179,180,181,183,184,185,186,187,189,195")]
        [Route("phan-xuong/xin-cap-vat-tu-sctx/xincap")]
        [HttpPost]
        public ActionResult XinCap()
        {
            string department_id = Session["departID"].ToString();
            QUANGHANHABCEntities db = new QUANGHANHABCEntities();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    db.Database.ExecuteSqlCommand("update Supplyplan set status=1,date=getdate() where departmentid=@departmentid and month(date)=month(getDate())",
                    new SqlParameter("departmentid", department_id));
                    db.SaveChanges();

                    Notification nt = new Notification();
                    nt.id_problem = 0;
                    nt.description = "XCVT";
                    nt.department_id = department_id;
                    nt.date = DateTime.Now.Date;
                    nt.isread = false;
                    db.Notifications.Add(nt);
                    db.SaveChanges();

                    transaction.Commit();
                    return Json("", JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    transaction.Rollback();

                    return new HttpStatusCodeResult(400);
                }
            }
        }



        [Route("phan-xuong/xin-cap-vat-tu-sctx/returnsupplymaintainName")]
        [HttpPost]
        public JsonResult returnsupplymaintainname(String supplyid,string equipmentid)
        {

            try
            {
                int quantity;
                QUANGHANHABCEntities db = new QUANGHANHABCEntities();
                var supply = db.Supplies.Where(x => x.supply_id == supplyid).First();
                var supplyduphong = db.Supply_SCTX.Where(x => (x.supply_id == supplyid)&&x.equipmentId==equipmentid).SingleOrDefault();
                if (supplyduphong == null) quantity = 0;
                else quantity = supplyduphong.quantity;
                
                

                return Json(new
                {
                    supply_name = supply.supply_name ,
                    unit = supply.unit,
                    quantity = quantity
                }, JsonRequestBehavior.AllowGet); ;
            }
            catch (Exception)
            { 
                return Json( new {supply_name = "0",
                    unit = "0",
                    quantity = 0}, JsonRequestBehavior.AllowGet);
            }

        }
        //Duyệt Cấp
        [Auther(RightID = "33,179,180,181,183,184,185,186,187,189,195")]
        [Route("phan-xuong/xin-cap-vat-tu-sctx/duyet-cap/getinformation")]
        [HttpPost]
        public ActionResult EquipmentInformation()
        {
            try
            {

                using (QUANGHANHABCEntities DBContext = new QUANGHANHABCEntities())
                {
                    // only taken by each department.
                    string department_id = Session["departID"].ToString();
                   var listequipment = new  List<Eq >() ;

                    if (HasProvided())
                    {
                        string query = @"select distinct SupplyPlan.equipmentId,equipment_name from Equipment inner join SupplyPlan on Equipment.equipmentId=SupplyPlan.equipmentid
                                where SupplyPlan.departmentid = @department_id  and month(date)=month(getdate()) and quantity_provide is not null";
                        listequipment = DBContext.Database.SqlQuery<Eq>(query,
                          new SqlParameter("department_id", department_id)
                           ).ToList();
                    
                      }
                    int totalrows = listequipment.Count;
                    int totalrowsafterfiltering = listequipment.Count;


                    return Json(new { success = true, data = listequipment, draw = Request["draw"], recordsTotal = totalrows/*, recordsFiltered = totalrowsafterfiltering*/ }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                Response.Write("Có lỗi xảy ra, xin vui lòng nhập lại");
                return new HttpStatusCodeResult(400);
            }
        }
        //
        public bool HasProvided()
        {
            using (QUANGHANHABCEntities DBContext = new QUANGHANHABCEntities()) {
                return DBContext.Database.SqlQuery<int>(@"select count(quantity_provide)'number' from supplyplan
                                                       where quantity_provide is not null").First() >= 1;
            } }
        [Auther(RightID = "33,179,180,181,183,184,185,186,187,189,195")]
        [Route("phan-xuong/xin-cap-vat-tu-sctx/duyet-cap/getListSupply")]
        [HttpPost]
        public ActionResult ListSupplyGeted(String equipmentId)
        {
            using (QUANGHANHABCEntities db = new QUANGHANHABCEntities())
            { 

                {
                    List<SupplyPlanDB> mysupply = db.Database.SqlQuery<SupplyPlanDB>("select supp.id,supp.supplyid, s.supply_name,s.unit ,supp.quantity_plan,supp.quantity_provide,supp.quantity,supp.equipmentid  " +
                   "from Supply s inner join SupplyPlan supp on s.supply_id = supp.supplyid where supp.equipmentid = @equipmentid and status=1  and month(date)=month(getdate())", new SqlParameter("equipmentid", equipmentId)).ToList();

                   return Json(new
                    {
                        success = true,
                        data = mysupply,
                    }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        [Auther(RightID = "33,179,180,181,183,184,185,186,187,189,195")]
        [Route("phan-xuong/xin-cap-vat-tu-sctx/duyet-cap/edit")]
        [HttpPost]
        public ActionResult EditListSupplyGeted(List<SupplyPlanDB> listvattu )
        {
            using (QUANGHANHABCEntities context = new QUANGHANHABCEntities())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    //Truncate Table to delete all old records.
                    //Check for NULL.

                    try
                    { String departid = Session["departID"].ToString();
                        string bulk_insert = string.Empty;
                        int  newquatity = 0;
                        // DateTime today = DateTime.Today;
                        List<SupplyPlan> vattus = context.SupplyPlans.Where(x => x.date.Month == DateTime.Now.Month  && x.departmentid==departid).ToList();
                        foreach (SupplyPlanDB vattu in  listvattu)
                        {
                            foreach (SupplyPlan vat in vattus) {
                                if (vattu.id == vat.id)
                                {
                                    newquatity = vattu.quantity-vat.quantity;
                                    break;
                                }
                            }
                            
                                string sub_insert = $"  if exists(select * from Supply_SCTX  where supply_id = '{vattu.supplyid}' and equipmentId = '{vattu.equipmentid}') " +
                                                "begin " +
                                               " update Supply_SCTX set " +
                                               $"quantity = (select quantity where supply_id='{vattu.supplyid}' and equipmentId='{vattu.equipmentid}')+{newquatity} " +
                                               $"where supply_id = '{vattu.supplyid}' and equipmentId = '{vattu.equipmentid}' " +
                                               "end " +
                                               "else " +
                                               "begin " +
                                               $"insert into Supply_SCTX(supply_id, equipmentId, quantity) VALUES('{vattu.supplyid}', '{vattu.equipmentid}', {vattu.quantity}) " +
                                              "end ;" +
                                               $" update Supplyplan set quantity={vattu.quantity}, date=getdate() where id={vattu.id} ;";
                            bulk_insert = string.Concat(bulk_insert, sub_insert);

                        }

                        context.Database.ExecuteSqlCommand(bulk_insert);
                        context.SaveChanges();
                        transaction.Commit();
                        return Json(new { success = true, message = "Chỉnh sửa thành công" }, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return new HttpStatusCodeResult(400);    
                    }
                }
            }
        }
        }
    public class Eq
    {
        public string equipmentId { get; set; }
        public string equipment_name { get; set; }
    }
    public class SupplyPlanDB : SupplyPlan
    {
        public int duphong { get; set; }
        public string supply_name { get; set; }
        public string unit { get; set; }
       
    }
 
}