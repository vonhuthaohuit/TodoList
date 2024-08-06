using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TodoList.Models;

namespace TodoList.Controllers
{
    public class LOAICONGVIECsController : ApiController
    {
        private QL_CONGVIEC db = new QL_CONGVIEC();

        // GET: api/LOAICONGVIECs
        public IQueryable<LOAICONGVIEC> GetLOAICONGVIECs()
        {
            return db.LOAICONGVIECs;
        }

        // GET: api/LOAICONGVIECs/5
        [ResponseType(typeof(LOAICONGVIEC))]
        public IHttpActionResult GetLOAICONGVIEC(int id)
        {
            LOAICONGVIEC lOAICONGVIEC = db.LOAICONGVIECs.Find(id);
            if (lOAICONGVIEC == null)
            {
                return NotFound();
            }

            return Ok(lOAICONGVIEC);
        }  

        // PUT: api/LOAICONGVIECs/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutLOAICONGVIEC(int id, LOAICONGVIEC lOAICONGVIEC)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != lOAICONGVIEC.ID)
            {
                return BadRequest();
            }

            db.Entry(lOAICONGVIEC).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LOAICONGVIECExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/LOAICONGVIECs
        [ResponseType(typeof(LOAICONGVIEC))]
        public IHttpActionResult PostLOAICONGVIEC(LOAICONGVIEC lOAICONGVIEC)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.LOAICONGVIECs.Add(lOAICONGVIEC);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = lOAICONGVIEC.ID }, lOAICONGVIEC);
        }

        // DELETE: api/LOAICONGVIECs/5
        [ResponseType(typeof(LOAICONGVIEC))]
        public IHttpActionResult DeleteLOAICONGVIEC(int id)
        {
            LOAICONGVIEC lOAICONGVIEC = db.LOAICONGVIECs.Find(id);
            if (lOAICONGVIEC == null)
            {
                return NotFound();
            }

            db.LOAICONGVIECs.Remove(lOAICONGVIEC);
            db.SaveChanges();

            return Ok(lOAICONGVIEC);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LOAICONGVIECExists(int id)
        {
            return db.LOAICONGVIECs.Count(e => e.ID == id) > 0;
        }

        [ResponseType(typeof(LOAICONGVIEC))]
        public IHttpActionResult GetLOAICONGVIEC(string maTaiKhoan)
        {
            List<LOAICONGVIEC> loaicongviecs = db.LOAICONGVIECs.Where(t => t.MATAIKHOAN == maTaiKhoan).ToList();
            if (loaicongviecs == null)
            {
                return NotFound();
            }

            return Ok(loaicongviecs);
        }
        [HttpPost]
        [Route("api/loaicongviecs/CreateLoaiCongViec")]
        [ResponseType(typeof(LOAICONGVIEC))]
        public IHttpActionResult CreateLoaiCongViec(LOAICONGVIEC lOAICONGVIEC)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.LOAICONGVIECs.Add(lOAICONGVIEC);
            db.SaveChanges();

            return Ok(lOAICONGVIEC);
        }
        [HttpDelete]
        [Route("api/loaicongviecs/DeleteLoaiCongViec")]
        [ResponseType(typeof(LOAICONGVIEC))]
        public IHttpActionResult DeleteLoaiCongViec(string maLoaiCongViec)
        {
            if (maLoaiCongViec == null)
            {
                return NotFound();
            }
            LOAICONGVIEC loaiCongViec = db.LOAICONGVIECs.Where(t => t.MALOAI == maLoaiCongViec).FirstOrDefault();
            db.LOAICONGVIECs.Remove(loaiCongViec);
            db.SaveChanges();

            return Ok();
        }
        [HttpPut]
        [Route("api/loaicongviecs/UpdateLoaiCongViec")]
        [ResponseType(typeof(LOAICONGVIEC))]
        public IHttpActionResult UpdateLoaiCongViec(string maLoaiCongViec, LOAICONGVIEC loaiCongViec)
        {
            if (loaiCongViec == null)
                return BadRequest();
            LOAICONGVIEC loaiCongViecUpdate = db.LOAICONGVIECs.SingleOrDefault(t => t.MALOAI == maLoaiCongViec);
            if (loaiCongViecUpdate == null)
                return NotFound();

            loaiCongViecUpdate.MOTALOAI = loaiCongViec.MOTALOAI;
            loaiCongViecUpdate.NGAYCAPNHATLOAI = DateTime.Today;
            loaiCongViecUpdate.TENLOAI = loaiCongViec.TENLOAI;
            db.SaveChanges();
            return Ok(loaiCongViecUpdate);
        }
    }
}