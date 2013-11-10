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
    public class MatchDropController : ApiController
    {
        private MWApiContext db = new MWApiContext();

        // GET api/MatchDrop
        public IQueryable<MatchDrop> GetMatchDrops()
        {
            return db.MatchDrops;
        }

        // GET api/MatchDrop/5
        [ResponseType(typeof(MatchDrop))]
        public IHttpActionResult GetMatchDrop(int id)
        {
            MatchDrop matchdrop = db.MatchDrops.Find(id);
            if (matchdrop == null)
            {
                return NotFound();
            }

            return Ok(matchdrop);
        }

        // PUT api/MatchDrop/5
        public IHttpActionResult PutMatchDrop(int id, MatchDrop matchdrop)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != matchdrop.MatchDropId)
            {
                return BadRequest();
            }

            db.Entry(matchdrop).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MatchDropExists(id))
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

        // POST api/MatchDrop

        public MatchDrop PostMatchDrop(MatchDrop matchdrop)
        {

            if (!ModelState.IsValid)
            {
                return null;
            }
            MatchDrop md;
            md = db.MatchDrops.FirstOrDefault(m => m.MatchHash == matchdrop.MatchHash && m.AssociationName == matchdrop.AssociationName);
            if (md == null)
            {
            md = matchdrop;
            db.MatchDrops.Add(md);
            db.SaveChanges();
            }


            return md;
        }

        // DELETE api/MatchDrop/5
        [ResponseType(typeof(MatchDrop))]
        public IHttpActionResult DeleteMatchDrop(int id)
        {
            MatchDrop matchdrop = db.MatchDrops.Find(id);
            if (matchdrop == null)
            {
                return NotFound();
            }

            db.MatchDrops.Remove(matchdrop);
            db.SaveChanges();

            return Ok(matchdrop);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MatchDropExists(int id)
        {
            return db.MatchDrops.Count(e => e.MatchDropId == id) > 0;
        }
    }
}