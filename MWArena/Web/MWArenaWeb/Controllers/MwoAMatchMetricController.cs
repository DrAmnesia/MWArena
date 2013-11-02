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
using MWA.Models;

namespace MWArenaWeb.Controllers
{
    public class MwoAMatchMetricController : ApiController
    {
        private MwoADbContext db = new MwoADbContext();

        // GET api/MwoAMatchMetric
        public IQueryable<MwoAMatchMetric> GetMwoAMatchMetrics()
        {
            return db.MwoAMatchMetrics;
        }

        // GET api/MwoAMatchMetric/5
        [ResponseType(typeof(MwoAMatchMetric))]
        public IHttpActionResult GetMwoAMatchMetric(int id)
        {
            MwoAMatchMetric mwoamatchmetric = db.MwoAMatchMetrics.Find(id);
            if (mwoamatchmetric == null)
            {
                return NotFound();
            }

            return Ok(mwoamatchmetric);
        }

        // PUT api/MwoAMatchMetric/5
        public IHttpActionResult PutMwoAMatchMetric(int id, MwoAMatchMetric mwoamatchmetric)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != mwoamatchmetric.MwoAMatchMetricId)
            {
                return BadRequest();
            }

            db.Entry(mwoamatchmetric).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MwoAMatchMetricExists(id))
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

        // POST api/MwoAMatchMetric
        [ResponseType(typeof(MwoAMatchMetric))]
        public IHttpActionResult PostMwoAMatchMetric(MwoAMatchMetric mwoamatchmetric)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.MwoAMatchMetrics.Add(mwoamatchmetric);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = mwoamatchmetric.MwoAMatchMetricId }, mwoamatchmetric);
        }

        // DELETE api/MwoAMatchMetric/5
        [ResponseType(typeof(MwoAMatchMetric))]
        public IHttpActionResult DeleteMwoAMatchMetric(int id)
        {
            MwoAMatchMetric mwoamatchmetric = db.MwoAMatchMetrics.Find(id);
            if (mwoamatchmetric == null)
            {
                return NotFound();
            }

            db.MwoAMatchMetrics.Remove(mwoamatchmetric);
            db.SaveChanges();

            return Ok(mwoamatchmetric);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MwoAMatchMetricExists(int id)
        {
            return db.MwoAMatchMetrics.Count(e => e.MwoAMatchMetricId == id) > 0;
        }
    }
}