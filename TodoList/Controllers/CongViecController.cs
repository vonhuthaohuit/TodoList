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
    public class CongViecController : Controller
    {
        // GET: CongViec
        private QL_CONGVIEC db = new QL_CONGVIEC();
        public async Task<ActionResult> Index(CONGVIEC congViec)
        {
            if (Session["IsChecked"] == null)
            {
                return RedirectToAction("Index", "TaiKhoan");
            }
            string tenDangNhap = Session["tenDangNhap"].ToString();
            string maTaiKhoan = db.TAIKHOANs.Where(t => t.TENTAIKHOAN == tenDangNhap).FirstOrDefault().MATAIKHOAN;
            ViewBag.Title = "Công việc";
            ViewBag.TitlePage = "Tất cả";
            var list = await GetAllCongViec(maTaiKhoan);
            if (list != null)
                return View(list);
            return View();
        }

        private async Task<List<CONGVIEC>> GetAllCongViec(string maTaiKhoan)
        {
            string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority +
                Request.ApplicationPath.TrimEnd('/') + "/";
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage res = await httpClient.GetAsync(baseUrl + "api/CONGVIECs/GetCongViecTheoTaiKhoan?maTaiKhoan=" + maTaiKhoan);
                if (res.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    //List<CONGVIEC> list = new List<CONGVIEC>();
                    //list = res.Content.ReadAsAsync<List<CONGVIEC>>().Result;
                    var singleItem = res.Content.ReadAsAsync<CONGVIEC>().Result;
                    var list = new List<CONGVIEC> { singleItem };
                    return list;
                }
                return null;
            }
        }
        private async Task<List<CONGVIEC>> GetCongViecByLoaiAsync(string maLoai)
        {
            string baseUrl = $"{Request.Url.Scheme}://{Request.Url.Authority}{Request.ApplicationPath.TrimEnd('/')}/";

            using (var httpClient = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync($"{baseUrl}api/CONGVIECs?MALOAI={maLoai}");
                    if (response.IsSuccessStatusCode)
                    {
                        List<CONGVIEC> list = await response.Content.ReadAsAsync<List<CONGVIEC>>();
                        return list;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error fetching data: {ex.Message}");
                }

                return null;
            }
        }

        public async Task<ActionResult> ViewCongViecTheoLoai(string maLoai)
        {
            string tenLoai = db.LOAICONGVIECs.Where(t => t.MALOAI == maLoai).Select(t => t.TENLOAI).FirstOrDefault();
            ViewBag.TitlePage = tenLoai;
            var list = await GetCongViecByLoaiAsync(maLoai);
            if (list != null)
                return View(list);
            return View();
        }
        

    }
}