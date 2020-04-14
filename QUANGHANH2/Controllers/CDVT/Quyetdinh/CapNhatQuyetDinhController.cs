﻿using QUANGHANH2.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QUANGHANH2.Controllers.CDVT.Quyetdinh
{
    public class CapNhatQuyetDinhController : Controller
    {
        [Route("phong-cdvt/quyet-dinh/update")]
        [HttpPost]
        public ActionResult Update()
        {
            int documentary_id = int.Parse(Request["documentary_id"]);
            string documentary_code = Request["documentary_code"];
            string reason = Request["reason"];
            string out_in_come = Request["out_in_come"];
            using (QUANGHANHABCEntities db = new QUANGHANHABCEntities())
            {
                using (DbContextTransaction trans = db.Database.BeginTransaction())
                {
                    try
                    {

                        if (db.Documentaries.Where(x => x.documentary_code == documentary_code).FirstOrDefault() != null)
                            return Json(new { success = false, message = "Số quyết định đã tồn tại" });
                        Documentary doc = db.Documentaries.Find(documentary_id);
                        if (doc == null)
                            return Json(new { success = false, message = "Quyết định không tồn tại" });
                        doc.documentary_code = documentary_code == "" ? null : documentary_code;
                        doc.reason = reason;
                        doc.out_in_come = out_in_come;

                        if (doc.documentary_code != null)
                        {
                            Notification noti = new Notification();
                            noti.description = "";
                            switch (doc.documentary_type)
                            {
                                case 1:
                                    db.Database.ExecuteSqlCommand(@"update Equipment set current_Status = 3
where equipmentId in
(select equipmentId from Documentary_repair_details where documentary_id = @documentary_id)", new SqlParameter("documentary_id", documentary_id));
                                    noti.description = "sua chua";
                                    break;
                                case 2:
                                    db.Database.ExecuteSqlCommand(@"update Equipment set current_Status = 5
where equipmentId in
(select equipmentId from Documentary_maintain_details where documentary_id = @documentary_id)", new SqlParameter("documentary_id", documentary_id));
                                    noti.description = "bao duong";
                                    break;
                                case 3:
                                    db.Database.ExecuteSqlCommand(@"update Equipment set current_Status = 6
where equipmentId in
(select equipmentId from Documentary_moveline_details where documentary_id = @documentary_id)", new SqlParameter("documentary_id", documentary_id));
                                    db.Database.ExecuteSqlCommand(@"update Equipment set current_Status = 6
where equipmentId in
(select distinct equipmentId_dikem from Supply_Documentary_Equipment where documentary_id = @documentary_id and equipmentId_dikem is not null)", new SqlParameter("documentary_id", documentary_id));
                                    noti.description = "dieu dong";
                                    break;
                                case 4:
                                    db.Database.ExecuteSqlCommand(@"update Equipment set current_Status = 7
where equipmentId in
(select equipmentId from Documentary_revoke_details where documentary_id = @documentary_id)", new SqlParameter("documentary_id", documentary_id));
                                    break;
                                case 5:
                                    db.Database.ExecuteSqlCommand(@"update Equipment set current_Status = 8
where equipmentId in
(select equipmentId from Documentary_liquidation_details where documentary_id = @documentary_id)", new SqlParameter("documentary_id", documentary_id));
                                    break;
                                case 6:
                                    db.Database.ExecuteSqlCommand(@"update Equipment set current_Status = 9
where equipmentId in 
(select equipmentId from Documentary_big_maintain_details where documentary_id = @documentary_id)", new SqlParameter("documentary_id", documentary_id));
                                    noti.description = "trung dai tu";
                                    break;
                                case 7:
                                    db.Database.ExecuteSqlCommand(@"update Equipment set current_Status = 16
where equipmentId in
(select equipmentId from Documentary_Improve_Detail where documentary_id = @documentary_id)", new SqlParameter("documentary_id", documentary_id));
                                    noti.description = "cai tien";
                                    break;
                                case 8:
                                    break;
                                default:
                                    return Json(new { success = false, message = "Loại quyết định không tồn tại" });
                            }
                            if (!noti.description.Equals(""))
                            {
                                noti.date = DateTime.Now.Date;
                                noti.department_id = doc.department_id_to;
                                noti.id_problem = doc.documentary_id;
                                noti.isread = false;
                                db.Notifications.Add(noti);
                                db.SaveChanges();
                            }
                        }

                        db.SaveChanges();
                        trans.Commit();
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        return Json(new { success = false, message = "Có lỗi xảy ra" });
                    }
                }
            }
            return Json(new { success = true, message = "Cập nhật thành công" });
        }

        [Route("phong-cdvt/quyet-dinh/delete")]
        [HttpPost]
        public ActionResult Delete()
        {
            int documentary_id = int.Parse(Request["documentary_id"]);
            using (QUANGHANHABCEntities db = new QUANGHANHABCEntities())
            {
                Documentary doc = db.Documentaries.Find(documentary_id);
                if (doc == null)
                    return Json(new { success = false, message = "Mã quyết định không tồn tại" });

                if (doc.documentary_code != null)
                    return Json(new { success = false, message = "Bạn không được xóa quyết định này" });

                using (DbContextTransaction trans = db.Database.BeginTransaction())
                {
                    try
                    {
                        switch (doc.documentary_type)
                        {
                            case 8:
                                db.Database.ExecuteSqlCommand("DELETE FROM Supply_Documentary_Camera WHERE documentary_id = " + documentary_id);
                                db.Database.ExecuteSqlCommand("DELETE FROM Documentary_camera_repair_details WHERE documentary_id = " + documentary_id);
                                db.Database.ExecuteSqlCommand("DELETE FROM Documentary WHERE documentary_id = " + documentary_id);
                                break;
                            default:
                                db.Documentaries.Remove(doc);
                                break;
                        }
                        db.SaveChanges();
                        trans.Commit();
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        return Json(new { success = false, message = "Có lỗi xảy ra" });
                    }
                }
                return Json(new { success = true, message = "Xóa thành công" });
            }
        }
    }
}