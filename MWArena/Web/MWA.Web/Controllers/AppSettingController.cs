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
    public class AppSettingController : ApiController
    {
        private MwaDBContext db = new MwaDBContext();

        // GET api/AppSetting
        public IQueryable<AppSetting> GetAppSettings()
        {
            return db.AppSettings;
        }

        // GET api/AppSetting/5
        [ResponseType(typeof(AppSetting))]
        public IHttpActionResult GetAppSetting(int id)
        {
            AppSetting appsetting = db.AppSettings.Find(id);
            if (appsetting == null)
            {
                return NotFound();
            }

            return Ok(appsetting);
        }

        // PUT api/AppSetting/5
        public IHttpActionResult PutAppSetting(int id, AppSetting appsetting)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != appsetting.AppSettingId)
            {
                return BadRequest();
            }

            db.Entry(appsetting).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppSettingExists(id))
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

        // POST api/AppSetting
        [ResponseType(typeof(AppSetting))]
        public IHttpActionResult PostAppSetting(AppSetting appsetting)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.AppSettings.Add(appsetting);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = appsetting.AppSettingId }, appsetting);
        }

        // DELETE api/AppSetting/5
        [ResponseType(typeof(AppSetting))]
        public IHttpActionResult DeleteAppSetting(int id)
        {
            AppSetting appsetting = db.AppSettings.Find(id);
            if (appsetting == null)
            {
                return NotFound();
            }

            db.AppSettings.Remove(appsetting);
            db.SaveChanges();

            return Ok(appsetting);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AppSettingExists(int id)
        {
            return db.AppSettings.Count(e => e.AppSettingId == id) > 0;
        }
    }
}