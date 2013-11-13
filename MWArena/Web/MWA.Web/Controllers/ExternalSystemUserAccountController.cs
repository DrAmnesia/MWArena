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
    public class ExternalSystemUserAccountController : ApiController
    {
        private MwaDBContext db = new MwaDBContext();

        // GET api/ExternalSystemUserAccount
        public IQueryable<ExternalSystemUserAccount> GetExternalSystemUserAccounts()
        {
            return db.ExternalSystemUserAccounts;
        }

        // GET api/ExternalSystemUserAccount/5
        [ResponseType(typeof(ExternalSystemUserAccount))]
        public IHttpActionResult GetExternalSystemUserAccount(int id)
        {
            ExternalSystemUserAccount externalsystemuseraccount = db.ExternalSystemUserAccounts.Find(id);
            if (externalsystemuseraccount == null)
            {
                return NotFound();
            }

            return Ok(externalsystemuseraccount);
        }
        // GET api/ExternalSystemUserAccount/5/1
        [ResponseType(typeof(ExternalSystemUserAccount))]
        // POST api/Account/RemoveLogin
        [Route("GetUserExtSysAccount")]
        public IHttpActionResult GetExternalSystemUserAccount(int userid, int sysid)
        {
            ExternalSystemUserAccount externalsystemuseraccount = db.ExternalSystemUserAccounts.FirstOrDefault(e=>e.ExternalSystem.ExternalSystemId==sysid&& e.ExternalSystemAccountName=="DrAmnesia");
            if (externalsystemuseraccount == null)
            {
                return NotFound();
            }

            return Ok(externalsystemuseraccount);
        }
        // PUT api/ExternalSystemUserAccount/5
        public IHttpActionResult PutExternalSystemUserAccount(int id, ExternalSystemUserAccount externalsystemuseraccount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != externalsystemuseraccount.ExternalSystemUserAccountId)
            {
                return BadRequest();
            }

            db.Entry(externalsystemuseraccount).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExternalSystemUserAccountExists(id))
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

        // POST api/ExternalSystemUserAccount
        [ResponseType(typeof(ExternalSystemUserAccount))]
        public IHttpActionResult PostExternalSystemUserAccount(ExternalSystemUserAccount externalsystemuseraccount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ExternalSystemUserAccounts.Add(externalsystemuseraccount);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = externalsystemuseraccount.ExternalSystemUserAccountId }, externalsystemuseraccount);
        }

        // DELETE api/ExternalSystemUserAccount/5
        [ResponseType(typeof(ExternalSystemUserAccount))]
        public IHttpActionResult DeleteExternalSystemUserAccount(int id)
        {
            ExternalSystemUserAccount externalsystemuseraccount = db.ExternalSystemUserAccounts.Find(id);
            if (externalsystemuseraccount == null)
            {
                return NotFound();
            }

            db.ExternalSystemUserAccounts.Remove(externalsystemuseraccount);
            db.SaveChanges();

            return Ok(externalsystemuseraccount);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ExternalSystemUserAccountExists(int id)
        {
            return db.ExternalSystemUserAccounts.Count(e => e.ExternalSystemUserAccountId == id) > 0;
        }
    }
}