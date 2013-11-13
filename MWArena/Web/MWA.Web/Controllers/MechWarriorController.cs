using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using MWA.Models;
 

namespace MWA.Web.Controllers
{
    public class MechWarriorController : ApiController
    {
        private MwaDBContext db = new MwaDBContext();

        // GET api/mechwarrior
        [Authorize]
        public  MechWarriorInfo  Get()
        {

            string uid = User.Identity.GetUserId();
            if (uid.IsNullOrWhiteSpace())
                return new  MechWarriorInfo() ;

            var mechWarriors = db.Users.Where(mw => mw.Id == uid).ToList();
            var mwi =  mechWarriors.Select(o=>new MechWarriorInfo{BattalionName = o.BattalionName,CompanyName=o.CompanyName,FactionName=o.FactionName,LanceName=o.LanceName,ImageUrl=o.ImageUrl,RankName=o.RankName,RegimentName=o.RegimentName}).FirstOrDefault();
            mwi = (mwi) ?? new MechWarriorInfo(); 
            var userPrefs = db.UserPrefs.Where(up => up.MechWarrior.Id == uid).ToList();
            mwi.UserPrefs = (userPrefs.Any()) ?   userPrefs:new List<UserPref>() ;

            return mwi;
        }

        // GET api/mechwarrior/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/mechwarrior
        public void Post([FromBody]string value)
        {
        }

        // PUT api/mechwarrior/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/mechwarrior/5
        public void Delete(int id)
        {
        }
    }
}
