using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TodoList.Models;

namespace TodoList.Controllers
{
    public class LoaiCongViecController : Controller
    {
        // GET: LoaiCongViec
        public ActionResult Index(LOAICONGVIEC congViec)
        {
            if (Session["IsChecked"] == null)
            {
                return RedirectToAction("Index", "TaiKhoan");
            }
            return View();
        }
        [ChildActionOnly]
        public ActionResult GetLoaiCongViecPartial()
        {
            var list = GetAllLoaiCongViec();
            if (list != null)
                return PartialView("_LoaiCongViecPartial", list);
            return View();
        }

        private List<LOAICONGVIEC> GetAllLoaiCongViec()
        {
            string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority +
                Request.ApplicationPath.TrimEnd('/') + "/";
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage res = httpClient.GetAsync(baseUrl + "api/LOAICONGVIECs").Result;
                if (res.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    List<LOAICONGVIEC> list = res.Content.ReadAsAsync<List<LOAICONGVIEC>>().Result;
                    return list;
                }
                return null;
            }
        }
        
    }
}