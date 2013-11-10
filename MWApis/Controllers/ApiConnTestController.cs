using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MWApis.Controllers
{
    public class ApiConnTestController : ApiController
    {
        // GET api/apiconntest
        public IEnumerable<string> Get()
        {
            return new string[] { "MWApi Connected" };
        }

        // GET api/apiconntest/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/apiconntest
        public void Post([FromBody]string value)
        {
        }

        // PUT api/apiconntest/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/apiconntest/5
        public void Delete(int id)
        {
        }
    }
}
