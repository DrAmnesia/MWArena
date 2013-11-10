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
    public class DropDeck12Controller : ApiController
    {
        private MwaDBContext db = new MwaDBContext();

        // GET api/DropDeck12
        public IQueryable<DropDeck12> GetDropDeck12s()
        {
            return db.DropDeck12s;
        }

        // GET api/DropDeck12/5
        [ResponseType(typeof(DropDeck12))]
        public IHttpActionResult GetDropDeck12(int id)
        {
            DropDeck12 dropdeck12 = db.DropDeck12s.Find(id);
            if (dropdeck12 == null)
            {
                return NotFound();
            }

            return Ok(dropdeck12);
        }

        // PUT api/DropDeck12/5
        public IHttpActionResult PutDropDeck12(int id, DropDeck12 dropdeck12)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != dropdeck12.DropDeck12Id)
            {
                return BadRequest();
            }

            db.Entry(dropdeck12).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DropDeck12Exists(id))
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

        // POST api/DropDeck12
        public DropDeck12 PostDropDeck12(DropDeck12 dropdeck12)
        {
            //todo update the search algorithym to use the chassis not the mechnames, for greater fidelity
            if (!ModelState.IsValid)
            {
                return null;
            }
            // todo:    logic to match the mechlist in dropdeck12 to a drop deck
            //          try concatentating the mechnames (ordered by mechname alphabetically)
            List<Chassis> chassisList= db.Chassis.ToList();
            
            String[] dropDeckMechNames = new String[]{dropdeck12.MechName1,dropdeck12.MechName2,dropdeck12.MechName3,dropdeck12.MechName4,dropdeck12.MechName5,dropdeck12.MechName6,dropdeck12.MechName7,dropdeck12.MechName8,dropdeck12.MechName9,dropdeck12.MechName10,dropdeck12.MechName11,dropdeck12.MechName12}.OrderBy(s=>s).ToArray();
            DropDeck12 ddCriteria = new DropDeck12
            {
                MechName1 = dropDeckMechNames[0],
                MechName2 = dropDeckMechNames[1],
                MechName3 = dropDeckMechNames[2],
                MechName4 = dropDeckMechNames[3],
                MechName5 = dropDeckMechNames[4],
                MechName6 = dropDeckMechNames[5],
                MechName7 = dropDeckMechNames[6],
                MechName8 = dropDeckMechNames[7],
                MechName9 = dropDeckMechNames[8],
                MechName10 = dropDeckMechNames[9],
                MechName11 = dropDeckMechNames[10],
                MechName12 = dropDeckMechNames[10],
            };
                ddCriteria.Chassis1 = chassisList.FirstOrDefault(c=>c.AltName2 ==ddCriteria.MechName1);
                ddCriteria.Chassis2 = chassisList.FirstOrDefault(c=>c.AltName2 ==ddCriteria.MechName2);
                ddCriteria.Chassis3 = chassisList.FirstOrDefault(c=>c.AltName2 ==ddCriteria.MechName3);
                ddCriteria.Chassis4 = chassisList.FirstOrDefault(c=>c.AltName2 ==ddCriteria.MechName4);
                ddCriteria.Chassis5 = chassisList.FirstOrDefault(c=>c.AltName2 ==ddCriteria.MechName5);
                ddCriteria.Chassis6 = chassisList.FirstOrDefault(c=>c.AltName2 ==ddCriteria.MechName6);
                ddCriteria.Chassis7 = chassisList.FirstOrDefault(c=>c.AltName2 ==ddCriteria.MechName7);
                ddCriteria.Chassis8 = chassisList.FirstOrDefault(c=>c.AltName2 ==ddCriteria.MechName8);
                ddCriteria.Chassis9 = chassisList.FirstOrDefault(c=>c.AltName2 ==ddCriteria.MechName9);
                ddCriteria.Chassis10 = chassisList.FirstOrDefault(c=>c.AltName2 ==ddCriteria.MechName10);
                ddCriteria.Chassis11 = chassisList.FirstOrDefault(c=>c.AltName2 ==ddCriteria.MechName11);
                ddCriteria.Chassis12 = chassisList.FirstOrDefault(c=>c.AltName2 ==ddCriteria.MechName12);
            
            DropDeck12 dd12 = db.DropDeck12s.FirstOrDefault(dd=>
                dd.Chassis1==ddCriteria.Chassis1 && 
                dd.Chassis2==ddCriteria.Chassis2 && 
                dd.Chassis3==ddCriteria.Chassis3 && 
                dd.Chassis4==ddCriteria.Chassis4 && 
                dd.Chassis5==ddCriteria.Chassis5 && 
                dd.Chassis6==ddCriteria.Chassis6 && 
                dd.Chassis7==ddCriteria.Chassis7 && 
                dd.Chassis8==ddCriteria.Chassis8 && 
                dd.Chassis9==ddCriteria.Chassis9 && 
                dd.Chassis10==ddCriteria.Chassis10 && 
                dd.Chassis11==ddCriteria.Chassis11 && 
                dd.Chassis12==ddCriteria.Chassis12  );
            if (dd12==null){
                db.DropDeck12s.Add(ddCriteria);
                db.SaveChanges();
                }
            else{
                ddCriteria.DropDeck12Id=dd12.DropDeck12Id;
                }



            return ddCriteria;
        }

        // DELETE api/DropDeck12/5
        [ResponseType(typeof(DropDeck12))]
        public IHttpActionResult DeleteDropDeck12(int id)
        {
            DropDeck12 dropdeck12 = db.DropDeck12s.Find(id);
            if (dropdeck12 == null)
            {
                return NotFound();
            }

            db.DropDeck12s.Remove(dropdeck12);
            db.SaveChanges();

            return Ok(dropdeck12);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DropDeck12Exists(int id)
        {
            return db.DropDeck12s.Count(e => e.DropDeck12Id == id) > 0;
        }
    }
}