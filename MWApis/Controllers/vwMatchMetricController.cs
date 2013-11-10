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

namespace MWApis.Controllers
{
    public class vwMatchMetricController : ApiController
    {
        private MWApiContext db = new MWApiContext();

        // GET api/vwMatchMetric
        public IQueryable<vwMatchMetric> GetvwMatchMetrics()
        {
            return db.vwMatchMetrics;
        }

        // GET api/vwMatchMetric/5
        [ResponseType(typeof(vwMatchMetric))]
        public IHttpActionResult GetvwMatchMetric(int id)
        {
            vwMatchMetric vwmatchmetric = db.vwMatchMetrics.Find(id);
            if (vwmatchmetric == null)
            {
                return NotFound();
            }

            return Ok(vwmatchmetric);
        }

        // PUT api/vwMatchMetric/5
        public IHttpActionResult PutvwMatchMetric(int id, vwMatchMetric vwmatchmetric)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != vwmatchmetric.vwMatchMetricId)
            {
                return BadRequest();
            }

            db.Entry(vwmatchmetric).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!vwMatchMetricExists(id))
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

        // POST api/vwMatchMetric
        [ResponseType(typeof(vwMatchMetric))]
        public IHttpActionResult PostvwMatchMetric(vwMatchMetric vwmatchmetric)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.vwMatchMetrics.Add(vwmatchmetric);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = vwmatchmetric.vwMatchMetricId }, vwmatchmetric);
        }

        // DELETE api/vwMatchMetric/5
        [ResponseType(typeof(vwMatchMetric))]
        public IHttpActionResult DeletevwMatchMetric(int id)
        {
            vwMatchMetric vwmatchmetric = db.vwMatchMetrics.Find(id);
            if (vwmatchmetric == null)
            {
                return NotFound();
            }

            db.vwMatchMetrics.Remove(vwmatchmetric);
            db.SaveChanges();

            return Ok(vwmatchmetric);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool vwMatchMetricExists(int id)
        {
            return db.vwMatchMetrics.Count(e => e.vwMatchMetricId == id) > 0;
        }
    }
}