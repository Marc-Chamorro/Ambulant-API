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
    [Authorize]
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        [HttpGet]
        [Route("list")]
        //list all available users id, name and email
        public HttpResponseMessage Get()
        {
            return UserData.List(this.Request);
        }

        [HttpPost]
        [Route("add")]
        [AllowAnonymous]
        public HttpResponseMessage Post([FromBody] PersonSignUp person)
        {
            return UserData.SignUp(this.Request, person);
        }
    }
}
