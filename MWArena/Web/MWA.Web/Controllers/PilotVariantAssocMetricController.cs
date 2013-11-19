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
using Microsoft.AspNet.Identity;
using MWA.Models;

namespace MWA.Web.Controllers
{
    public class PilotVariantAssocMetricController : ApiController
    {
        private MwaDBContext db = new MwaDBContext();

        // GET api/PilotVariantAssocMetric
        [HttpGet]
        [Authorize]
        public IQueryable<vwPilotVariantAssocMetric> GetPilotVariantAssocMetrics()
        {
            string uid = User.Identity.GetUserId();
            string uname = User.Identity.Name;
            if (uname.IsNullOrWhiteSpace())
                return new List<vwPilotVariantAssocMetric>().AsQueryable();

            return db.vwPilotVariantAssocMetrics.Where(m => (m.username==uname || m.name == uname)).OrderByDescending(o => o.vwPilotVariantAssocMetricId).AsQueryable();
        }

        // GET api/PilotVariantAssocMetric/5
        [ResponseType(typeof(vwPilotVariantAssocMetric))]
        public IHttpActionResult GetvwPilotVariantAssocMetric(int id)
        {
            vwPilotVariantAssocMetric vwpilotvariantassocmetric = db.vwPilotVariantAssocMetrics.Find(id);
            if (vwpilotvariantassocmetric == null)
            {
                return NotFound();
            }

            return Ok(vwpilotvariantassocmetric);
        }

        // PUT api/PilotVariantAssocMetric/5
        public IHttpActionResult PutvwPilotVariantAssocMetric(int id, vwPilotVariantAssocMetric vwpilotvariantassocmetric)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != vwpilotvariantassocmetric.vwPilotVariantAssocMetricId)
            {
                return BadRequest();
            }

            db.Entry(vwpilotvariantassocmetric).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!vwPilotVariantAssocMetricExists(id))
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

        // POST api/PilotVariantAssocMetric
        [ResponseType(typeof(vwPilotVariantAssocMetric))]
        public IHttpActionResult PostvwPilotVariantAssocMetric(vwPilotVariantAssocMetric vwpilotvariantassocmetric)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.vwPilotVariantAssocMetrics.Add(vwpilotvariantassocmetric);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = vwpilotvariantassocmetric.vwPilotVariantAssocMetricId }, vwpilotvariantassocmetric);
        }

        // DELETE api/PilotVariantAssocMetric/5
        [ResponseType(typeof(vwPilotVariantAssocMetric))]
        public IHttpActionResult DeletevwPilotVariantAssocMetric(int id)
        {
            vwPilotVariantAssocMetric vwpilotvariantassocmetric = db.vwPilotVariantAssocMetrics.Find(id);
            if (vwpilotvariantassocmetric == null)
            {
                return NotFound();
            }

            db.vwPilotVariantAssocMetrics.Remove(vwpilotvariantassocmetric);
            db.SaveChanges();

            return Ok(vwpilotvariantassocmetric);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool vwPilotVariantAssocMetricExists(int id)
        {
            return db.vwPilotVariantAssocMetrics.Count(e => e.vwPilotVariantAssocMetricId == id) > 0;
        }
    }
}