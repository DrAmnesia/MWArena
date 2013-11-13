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
    public class UserAppSettingController : ApiController
    {
        private MwaDBContext db = new MwaDBContext();

        // GET api/UserAppSetting
        public IQueryable<UserAppSetting> GetUserAppSettings()
        {
            return db.UserAppSettings;
        }

        // GET api/UserAppSetting/5
        [ResponseType(typeof(UserAppSetting))]
        public IHttpActionResult GetUserAppSetting(int id)
        {
            UserAppSetting userappsetting = db.UserAppSettings.Find(id);
            if (userappsetting == null)
            {
                return NotFound();
            }

            return Ok(userappsetting);
        }

        // PUT api/UserAppSetting/5
        public IHttpActionResult PutUserAppSetting(int id, UserAppSetting userappsetting)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userappsetting.UserAppSettingId)
            {
                return BadRequest();
            }

            db.Entry(userappsetting).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserAppSettingExists(id))
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

        // POST api/UserAppSetting
        [ResponseType(typeof(UserAppSetting))]
        public IHttpActionResult PostUserAppSetting(UserAppSetting userappsetting)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.UserAppSettings.Add(userappsetting);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = userappsetting.UserAppSettingId }, userappsetting);
        }

        // DELETE api/UserAppSetting/5
        [ResponseType(typeof(UserAppSetting))]
        public IHttpActionResult DeleteUserAppSetting(int id)
        {
            UserAppSetting userappsetting = db.UserAppSettings.Find(id);
            if (userappsetting == null)
            {
                return NotFound();
            }

            db.UserAppSettings.Remove(userappsetting);
            db.SaveChanges();

            return Ok(userappsetting);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserAppSettingExists(int id)
        {
            return db.UserAppSettings.Count(e => e.UserAppSettingId == id) > 0;
        }
    }
}