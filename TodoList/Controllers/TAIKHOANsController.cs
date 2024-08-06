using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using TodoList.Models;

namespace TodoList.Controllers
{
    public class TAIKHOANsController : ApiController
    {
        private QL_CONGVIEC db = new QL_CONGVIEC();

        // GET: api/TAIKHOANs
        public IQueryable<TAIKHOAN> GetTAIKHOANs()
        {
            return db.TAIKHOANs;
        }

        // GET: api/TAIKHOANs/5
        [ResponseType(typeof(TAIKHOAN))]
        public IHttpActionResult GetTAIKHOAN(int id)
        {
            TAIKHOAN tAIKHOAN = db.TAIKHOANs.Find(id);
            if (tAIKHOAN == null)
            {
                return NotFound();
            }

            return Ok(tAIKHOAN);
        }
        [HttpGet]
        [ResponseType(typeof(TAIKHOAN))]
        public IHttpActionResult GetTaiKhoan(string tenDangNhapLogin, string matKhauLogin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = db.TAIKHOANs
                .FirstOrDefault(u => u.TENTAIKHOAN == tenDangNhapLogin && u.MATKHAU == matKhauLogin);

            if (user == null)
            {
                return Unauthorized();
            }

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TAIKHOANExists(int id)
        {
            return db.TAIKHOANs.Count(e => e.ID == id) > 0;
        }
        [HttpGet]
        [Route("api/TAIKHOANs/Login")]
        [ResponseType(typeof(TAIKHOAN))]
        public async Task<IHttpActionResult> Login(string tenDangNhapLogin, string matKhauLogin)
        {
            if (string.IsNullOrEmpty(tenDangNhapLogin) || string.IsNullOrEmpty(matKhauLogin))
            {
                return BadRequest("Tên đăng nhập và mật khẩu không được để trống.");
            }

            TAIKHOAN tk = await KiemTraDangNhap(tenDangNhapLogin, matKhauLogin);

            if (tk == null)
            {
                return BadRequest("Tài khoản không hợp lệ hoặc không tồn tại.");
            }

            return Ok("Đăng nhập thành công!");
        }

        private async Task<TAIKHOAN> KiemTraDangNhap(string tenDangNhap, string matKhau)
        {
            string tk = db.TAIKHOANs.Where(t => t.TENTAIKHOAN == tenDangNhap).ToString();
            if (!db.TAIKHOANs.FirstOrDefault().MATKHAU.Equals(matKhau))
            {
                return null;
            }
            string mk = db.TAIKHOANs.Where(t => t.MATKHAU == matKhau).FirstOrDefault().MATKHAU;
            if (tk != null && mk != null)
            {
                return new TAIKHOAN { TENTAIKHOAN = tenDangNhap };
            }
            return null;
        }
        [HttpGet]
        [ResponseType(typeof(TAIKHOAN))]
        public IHttpActionResult XemThongTin(string maTaiKhoan)
        {
            TAIKHOAN tAIKHOAN = db.TAIKHOANs.Where(t => t.MATAIKHOAN == maTaiKhoan).FirstOrDefault();
            if (tAIKHOAN == null)
            {
                return NotFound();
            }

            return Ok(tAIKHOAN);
        }
        private string tuDongTangMaTaiKhoan()
        {
            var maxMaTaiKhoan = db.TAIKHOANs
            .OrderByDescending(t => t.MATAIKHOAN)
            .Select(t => t.MATAIKHOAN)
            .FirstOrDefault();

            if (string.IsNullOrEmpty(maxMaTaiKhoan))
            {
                return "TK01";
            }

            var numberPart = maxMaTaiKhoan.Substring(2);
            if (int.TryParse(numberPart, out int number))
            {
                return "TK" + (number + 1).ToString();
            }
            throw new Exception("Không thể tạo tài khoản !!!");
        }
        private string tuDongTangMaThongTin()
        {
            var maxMaThongTin = db.THONGTINCANHANs
            .OrderByDescending(t => t.ID)
            .Select(t => t.MATHONGTIN)
            .FirstOrDefault();

            if (string.IsNullOrEmpty(maxMaThongTin))
            {
                return "TT01";
            }

            var numberPart = maxMaThongTin.Substring(2);
            if (int.TryParse(numberPart, out int number))
            {
                return "TT" + (number + 1).ToString();
            }
            throw new Exception("Không thể tạo thông tin !!!");
        }
        [ResponseType(typeof(TAIKHOAN))]
        public async Task<IHttpActionResult> Register( TAIKHOAN model)
        {
            if (model == null || string.IsNullOrEmpty(model.TENTAIKHOAN) || string.IsNullOrEmpty(model.MATKHAU))
            {
                return BadRequest("Tên đăng nhập và mật khẩu không được để trống.");
            }

            var existingUser = await db.TAIKHOANs.FirstOrDefaultAsync(u => u.TENTAIKHOAN == model.TENTAIKHOAN);
            if (existingUser != null)
            {
                return BadRequest("Tên đăng nhập đã tồn tại. Vui lòng chọn tên đăng nhập khác.");
            }

            TAIKHOAN tk = new TAIKHOAN
            {
                MATAIKHOAN = tuDongTangMaTaiKhoan(),
                TENTAIKHOAN = model.TENTAIKHOAN,
                MATKHAU = model.MATKHAU
            };

            THONGTINCANHAN tt = new THONGTINCANHAN
            {
                MATHONGTIN = tuDongTangMaThongTin(),
                MATAIKHOAN = tk.MATAIKHOAN
            };

            db.TAIKHOANs.Add(tk);
            db.THONGTINCANHANs.Add(tt);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { tenDangNhapDangKy = tk.TENTAIKHOAN }, tk);
        }

    }
}