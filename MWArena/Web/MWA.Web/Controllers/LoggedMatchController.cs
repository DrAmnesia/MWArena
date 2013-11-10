using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using MWA.Models;
using MatchLogger;
namespace MwoArenaWeb.Controllers
{
    public class LoggedMatchController : ApiController
    {
        private MwaDBContext db = new MwaDBContext();

        // GET api/loggedmatch
        public IEnumerable<MatchDrop> Get()
        {
            return db.MatchDrops.Where(o =>  o.AssociationName == "TEST");
        }

        // GET api/loggedmatch/5
        public string Get(int id)
        {
            return "value";
        }




 
        // POST api/MwoAMatchMetric
        [ResponseType(typeof(MatchDrop))]
        public IHttpActionResult PostLoggedMatch(MatchLogger.MatchLogger.LoggedMatch lm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            MatchDrop md = lm.GetLoggedMatchDrop(db);

            return CreatedAtRoute("DefaultApi", new { id = md.MatchDropId }, md);
        }
       

        // PUT api/loggedmatch/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/loggedmatch/5
        public void Delete(int id)
        {
        }


    
 
    }
}
