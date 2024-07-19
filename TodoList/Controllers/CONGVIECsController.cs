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
    public class CONGVIECsController : ApiController
    {
        private QL_CONGVIEC db = new QL_CONGVIEC();

        // GET: api/CONGVIECs
        public IQueryable<CONGVIEC> GetCONGVIECs()
        {
            return db.CONGVIECs;
        }

        // GET: api/CONGVIECs/5
        [ResponseType(typeof(CONGVIEC))]
        public IHttpActionResult GetCONGVIEC(int id)
        {
            CONGVIEC cONGVIEC = db.CONGVIECs.Find(id);
            if (cONGVIEC == null)
            {
                return NotFound();
            }

            return Ok(cONGVIEC);
        }

        // PUT: api/CONGVIECs/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCONGVIEC(int id, CONGVIEC cONGVIEC)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cONGVIEC.ID)
            {
                return BadRequest();
            }

            db.Entry(cONGVIEC).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CONGVIECExists(id))
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
        // POST: api/CONGVIECs
        [ResponseType(typeof(CONGVIEC))]
        public IHttpActionResult PostCONGVIEC(CONGVIEC cONGVIEC)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CONGVIECs.Add(cONGVIEC);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = cONGVIEC.ID }, cONGVIEC);
        }

        // DELETE: api/CONGVIECs/5
        [ResponseType(typeof(CONGVIEC))]
        public IHttpActionResult DeleteCONGVIEC(int id)
        {
            CONGVIEC cONGVIEC = db.CONGVIECs.Find(id);
            if (cONGVIEC == null)
            {
                return NotFound();
            }

            db.CONGVIECs.Remove(cONGVIEC);
            db.SaveChanges();

            return Ok(cONGVIEC);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CONGVIECExists(int id)
        {
            return db.CONGVIECs.Count(e => e.ID == id) > 0;
        }
        [ResponseType(typeof(CONGVIEC))]
        public IHttpActionResult GetCONGVIECs(string MaLoai)
        {
            var cONGVIEC = db.CONGVIECs.Where(t => t.MALOAI == MaLoai).ToList();
            if (cONGVIEC == null)
            {
                return NotFound();
            }
            return Ok(cONGVIEC);
        }
        [ResponseType(typeof(CONGVIEC))]
        public IHttpActionResult GetCongViecTheoTaiKhoan(string maTaiKhoan)
        {
            CONGVIEC congViec = db.CONGVIECs.Where(t => t.MATAIKHOAN == maTaiKhoan).FirstOrDefault();
            if (congViec == null)
            {
                return NotFound();
            }
            return Ok(congViec);
        }
    }
}