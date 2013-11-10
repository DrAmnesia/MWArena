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

namespace MwoArenaWeb.Controllers
{
    public class VariantAssocMapMetricController : ApiController
    {
        private MwoADbContext db = new MwoADbContext();

        // GET api/VariantAssocMapMetric
        public IQueryable<vwVariantAssocMapMetric> GetvwVariantAssocMapMetrics()
        {
            return db.vwVariantAssocMapMetrics;
        }

        // GET api/VariantAssocMapMetric/5
        [ResponseType(typeof(vwVariantAssocMapMetric))]
        public IHttpActionResult GetvwVariantAssocMapMetric(int id)
        {
            vwVariantAssocMapMetric vwvariantassocmapmetric = db.vwVariantAssocMapMetrics.Find(id);
            if (vwvariantassocmapmetric == null)
            {
                return NotFound();
            }

            return Ok(vwvariantassocmapmetric);
        }

        // PUT api/VariantAssocMapMetric/5
        public IHttpActionResult PutvwVariantAssocMapMetric(int id, vwVariantAssocMapMetric vwvariantassocmapmetric)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != vwvariantassocmapmetric.id)
            {
                return BadRequest();
            }

            db.Entry(vwvariantassocmapmetric).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!vwVariantAssocMapMetricExists(id))
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

        // POST api/VariantAssocMapMetric
        [ResponseType(typeof(vwVariantAssocMapMetric))]
        public IHttpActionResult PostvwVariantAssocMapMetric(vwVariantAssocMapMetric vwvariantassocmapmetric)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.vwVariantAssocMapMetrics.Add(vwvariantassocmapmetric);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = vwvariantassocmapmetric.id }, vwvariantassocmapmetric);
        }

        // DELETE api/VariantAssocMapMetric/5
        [ResponseType(typeof(vwVariantAssocMapMetric))]
        public IHttpActionResult DeletevwVariantAssocMapMetric(int id)
        {
            vwVariantAssocMapMetric vwvariantassocmapmetric = db.vwVariantAssocMapMetrics.Find(id);
            if (vwvariantassocmapmetric == null)
            {
                return NotFound();
            }

            db.vwVariantAssocMapMetrics.Remove(vwvariantassocmapmetric);
            db.SaveChanges();

            return Ok(vwvariantassocmapmetric);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool vwVariantAssocMapMetricExists(int id)
        {
            return db.vwVariantAssocMapMetrics.Count(e => e.id == id) > 0;
        }
    }
}