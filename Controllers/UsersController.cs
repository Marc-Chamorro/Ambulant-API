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
    //[Authorize]
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        // GET api/<controller>
        [HttpGet]
        public List<Users> Get()
        {
            return UsersData.List();
        }

        // GET api/<controller>/5
        [HttpGet]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public bool Post([FromBody]Users uUser)
        {
            return UsersData.SignUp(uUser);
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