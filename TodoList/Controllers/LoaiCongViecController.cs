using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TodoList.Models;

namespace TodoList.Controllers
{
    public class LoaiCongViecController : Controller
    {
        private QL_CONGVIEC db = new QL_CONGVIEC();
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
            if(Session["tenDangNhap"] == null)
            {
                return null;
            }
            string tenDangNhap = Session["tenDangNhap"].ToString();
            
            string maTaiKhoan = db.TAIKHOANs.Where(t => t.TENTAIKHOAN == tenDangNhap).FirstOrDefault().MATAIKHOAN;
            string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority +
                Request.ApplicationPath.TrimEnd('/') + "/";
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage res = httpClient.GetAsync(baseUrl + "api/LOAICONGVIECs?maTaiKhoan=" + maTaiKhoan).Result;
                if (res.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    //var singleItem = res.Content.ReadAsAsync<LOAICONGVIEC>().Result;
                    //var list = new List<LOAICONGVIEC> { singleItem };
                    List<LOAICONGVIEC> list = res.Content.ReadAsAsync<List<LOAICONGVIEC>>().Result;
                    return list;
                }
                return null;
            }
        }
        private string tuDongTangMaLoai()
        {
            var maxMaLoai = db.LOAICONGVIECs
            .OrderByDescending(t => t.MALOAI)
            .Select(t => t.MALOAI)
            .FirstOrDefault();

            if (string.IsNullOrEmpty(maxMaLoai))
            {
                return "LCV01";
            }

            var numberPart = maxMaLoai.Substring(3);
            if (int.TryParse(numberPart, out int number))
            {
                return "LCV" + (number + 1).ToString();
            }
            throw new Exception("Không tạo được loại  !!!");
        }
        public async Task<ActionResult> ThemLoaiCongViec(LOAICONGVIEC loaiCongViec)
        {
            if(loaiCongViec.TENLOAI == null)
            {
                return View(loaiCongViec);
            }
            string tenDangNhap = Session["tenDangNhap"]?.ToString();
            string maTaiKhoan = db.TAIKHOANs
                .Where(t => t.TENTAIKHOAN == tenDangNhap)
                .FirstOrDefault()?.MATAIKHOAN;

            if (maTaiKhoan == null)
            {
                ModelState.AddModelError("", "Không tìm thấy tài khoản.");
                return View(loaiCongViec);
            }

            loaiCongViec.MALOAI = tuDongTangMaLoai();
            loaiCongViec.MATAIKHOAN = maTaiKhoan;
            loaiCongViec.NGAYTAOLOAI = DateTime.Now;

            using (var httpClient = new HttpClient())
            {
                string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority +
                    Request.ApplicationPath.TrimEnd('/') + "/";
                HttpResponseMessage res = await httpClient.PostAsJsonAsync(baseUrl + "api/LOAICONGVIECs/CreateLoaiCongViec", loaiCongViec);

                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "CongViec");
                }
                else
                {
                    ModelState.AddModelError("", "Có lỗi xảy ra khi tạo loại công việc.");
                    return View(loaiCongViec);
                }
            }
        }
        public async Task<ActionResult> DeleteLoaiCongViec(string maLoaiCongViec)
        {
            using (var httpClient = new HttpClient())
            {
                string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority +
                    Request.ApplicationPath.TrimEnd('/') + "/";
                HttpResponseMessage res = await httpClient.DeleteAsync($"{baseUrl}api/LOAICONGVIECs/DeleteLoaiCongViec?maLoaiCongViec={maLoaiCongViec}");


                if (res.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Xóa loại công việc thành công.";
                }
                else
                {
                    ModelState.AddModelError("", "Có lỗi xảy ra khi xoá loại công việc.");
                }
            }
            return RedirectToAction("Index", "CongViec");
        }
        public async Task<ActionResult> UpdateLoaiCongViec(string maLoaiCongViec, LOAICONGVIEC loaiCongViec)
        {
            LOAICONGVIEC loaiChuaUpdate = (LOAICONGVIEC) db.LOAICONGVIECs.Where(t => t.MALOAI == maLoaiCongViec).FirstOrDefault();
            if (loaiCongViec.TENLOAI == null)
            {
                return View(loaiChuaUpdate);
            }
            if (!ModelState.IsValid)
            {
                return View(loaiChuaUpdate);
            }


            using (var httpClient = new HttpClient())
            {
                string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority +
                    Request.ApplicationPath.TrimEnd('/') + "/";
                var jsonContent = new StringContent(JsonConvert.SerializeObject(loaiCongViec), Encoding.UTF8, "application/json");

                HttpResponseMessage res = await httpClient.PutAsync($"{baseUrl}api/LOAICONGVIECs/UpdateLoaiCongViec?maLoaiCongViec={maLoaiCongViec}", jsonContent);

                if (res.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Sửa thành công";
                }
                else
                {
                    ModelState.AddModelError("", "Có lỗi xảy ra khi sửa loại công việc.");

                    return View();  
                }
            }
            return RedirectToAction("Index", "CongViec");
        }
    }
}