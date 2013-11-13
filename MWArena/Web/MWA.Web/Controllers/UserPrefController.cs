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
    public class UserPrefController : ApiController
    {
        private MwaDBContext db = new MwaDBContext();

        // GET api/UserPref
         [Authorize]
        public IQueryable<UserPref> GetUserPrefs()
        {
            string uid = User.Identity.GetUserId();
            if (uid.IsNullOrWhiteSpace() )
                return new List<UserPref>().AsQueryable();

            var mechWarrior = db.Users.Find(uid);
             return db.UserPrefs.Where(up=>up.MechWarrior.Id==uid);
        }

        // GET api/UserPref/5
        [ResponseType(typeof(UserPref))]
        public IHttpActionResult GetUserPref(int id)
        {
            UserPref userpref = db.UserPrefs.Find(id);
            if (userpref == null)
            {
                return NotFound();
            }

            return Ok(userpref);
        }

        // PUT api/UserPref/5
        public IHttpActionResult PutUserPref(int id, UserPref userpref)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userpref.UserPrefId)
            {
                return BadRequest();
            }

            db.Entry(userpref).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserPrefExists(id))
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

        // POST api/UserPref
        [ResponseType(typeof(UserPref))]
        public IHttpActionResult PostUserPref(UserPref userpref)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.UserPrefs.Add(userpref);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = userpref.UserPrefId }, userpref);
        }

        // DELETE api/UserPref/5
        [ResponseType(typeof(UserPref))]
        public IHttpActionResult DeleteUserPref(int id)
        {
            UserPref userpref = db.UserPrefs.Find(id);
            if (userpref == null)
            {
                return NotFound();
            }

            db.UserPrefs.Remove(userpref);
            db.SaveChanges();

            return Ok(userpref);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserPrefExists(int id)
        {
            return db.UserPrefs.Count(e => e.UserPrefId == id) > 0;
        }
    }
}