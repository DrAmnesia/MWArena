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
    public class VariantAssocMetricController : ApiController
    {
        private MWApiContext db = new MWApiContext();

        // GET api/VariantAssocMetric
        public IQueryable<vwVariantAssocMetric> GetvwVariantAssocMetrics()
        {
            return db.vwVariantAssocMetrics;
        }

        // GET api/VariantAssocMetric/5
        [ResponseType(typeof(vwVariantAssocMetric))]
        public IHttpActionResult GetvwVariantAssocMetric(int id)
        {
            vwVariantAssocMetric vwvariantassocmetric = db.vwVariantAssocMetrics.Find(id);
            if (vwvariantassocmetric == null)
            {
                return NotFound();
            }

            return Ok(vwvariantassocmetric);
        }

        // PUT api/VariantAssocMetric/5
        public IHttpActionResult PutvwVariantAssocMetric(int id, vwVariantAssocMetric vwvariantassocmetric)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != vwvariantassocmetric.vwVariantAssocMetricId)
            {
                return BadRequest();
            }

            db.Entry(vwvariantassocmetric).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!vwVariantAssocMetricExists(id))
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

        // POST api/VariantAssocMetric
        [ResponseType(typeof(vwVariantAssocMetric))]
        public IHttpActionResult PostvwVariantAssocMetric(vwVariantAssocMetric vwvariantassocmetric)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.vwVariantAssocMetrics.Add(vwvariantassocmetric);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = vwvariantassocmetric.vwVariantAssocMetricId }, vwvariantassocmetric);
        }

        // DELETE api/VariantAssocMetric/5
        [ResponseType(typeof(vwVariantAssocMetric))]
        public IHttpActionResult DeletevwVariantAssocMetric(int id)
        {
            vwVariantAssocMetric vwvariantassocmetric = db.vwVariantAssocMetrics.Find(id);
            if (vwvariantassocmetric == null)
            {
                return NotFound();
            }

            db.vwVariantAssocMetrics.Remove(vwvariantassocmetric);
            db.SaveChanges();

            return Ok(vwvariantassocmetric);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool vwVariantAssocMetricExists(int id)
        {
            return db.vwVariantAssocMetrics.Count(e => e.vwVariantAssocMetricId == id) > 0;
        }
    }
}