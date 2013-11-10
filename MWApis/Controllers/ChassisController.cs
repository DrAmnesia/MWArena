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
    public class ChassisController : ApiController
    {
        private MWApiContext db = new MWApiContext();

        // GET api/Chassis
        public IQueryable<Chassis> GetChassis()
        {
            return db.Chassis;
        }

        // GET api/Chassis/5
        [ResponseType(typeof(Chassis))]
        public IHttpActionResult GetChassis(int id)
        {
            Chassis chassis = db.Chassis.Find(id);
            if (chassis == null)
            {
                return NotFound();
            }

            return Ok(chassis);
        }

        // PUT api/Chassis/5
        public IHttpActionResult PutChassis(int id, Chassis chassis)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != chassis.Id)
            {
                return BadRequest();
            }

            db.Entry(chassis).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChassisExists(id))
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

        // POST api/Chassis
        [ResponseType(typeof(Chassis))]
        public IHttpActionResult PostChassis(Chassis chassis)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Chassis.Add(chassis);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = chassis.Id }, chassis);
        }

        // DELETE api/Chassis/5
        [ResponseType(typeof(Chassis))]
        public IHttpActionResult DeleteChassis(int id)
        {
            Chassis chassis = db.Chassis.Find(id);
            if (chassis == null)
            {
                return NotFound();
            }

            db.Chassis.Remove(chassis);
            db.SaveChanges();

            return Ok(chassis);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ChassisExists(int id)
        {
            return db.Chassis.Count(e => e.Id == id) > 0;
        }
    }
}