using API.Data;
using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    public class TypeController : ApiController
    {
        // GET api/<controller>
        public List<Models.Type> Get()
        {
            return TypeData.List();
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "Example Type";
        }

        // POST api/<controller>
        public void Post([FromBody] Models.Type type)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}