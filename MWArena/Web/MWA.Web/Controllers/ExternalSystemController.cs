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

namespace MWA.Web.Controllers
{
    public class ExternalSystemController : ApiController
    {
        private MwaDBContext db = new MwaDBContext();

        // GET api/ExternalSystem
        public IQueryable<ExternalSystem> GetExternalSystems()
        {
            return db.ExternalSystems;
        }

        // GET api/ExternalSystem/5
        [ResponseType(typeof(ExternalSystem))]
        public IHttpActionResult GetExternalSystem(int id)
        {
            ExternalSystem externalsystem = db.ExternalSystems.Find(id);
            if (externalsystem == null)
            {
                return NotFound();
            }

            return Ok(externalsystem);
        }

        // PUT api/ExternalSystem/5
        public IHttpActionResult PutExternalSystem(int id, ExternalSystem externalsystem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != externalsystem.ExternalSystemId)
            {
                return BadRequest();
            }

            db.Entry(externalsystem).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExternalSystemExists(id))
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

        // POST api/ExternalSystem
        [ResponseType(typeof(ExternalSystem))]
        public IHttpActionResult PostExternalSystem(ExternalSystem externalsystem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ExternalSystems.Add(externalsystem);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = externalsystem.ExternalSystemId }, externalsystem);
        }

        // DELETE api/ExternalSystem/5
        [ResponseType(typeof(ExternalSystem))]
        public IHttpActionResult DeleteExternalSystem(int id)
        {
            ExternalSystem externalsystem = db.ExternalSystems.Find(id);
            if (externalsystem == null)
            {
                return NotFound();
            }

            db.ExternalSystems.Remove(externalsystem);
            db.SaveChanges();

            return Ok(externalsystem);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ExternalSystemExists(int id)
        {
            return db.ExternalSystems.Count(e => e.ExternalSystemId == id) > 0;
        }
    }
}