﻿using System;
using System.Net;
using System.Threading;
using System.Web.Http;
using MupetJoy.Models;
using MupetJoy.Security;

namespace MupetJoy.Controllers
{
    /// <summary>
    /// login controller class for authenticate users
    /// </summary>
    /*[AllowAnonymous]
    [RoutePrefix("api/login")]*/
    public class LoginController : ApiController
    {      
        /*[HttpPost]
        [Route("authenticate")]
        public IHttpActionResult Authenticate(LoginRequest login)
        {
            if (login == null)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            ////TODO: This code is only for demo - extract method in new class & validate correctly in your application !!
            var isUserValid = (login.Username == "user" && login.Password == "123456");
            if (isUserValid)
            {
                var rolename = "Developer";
                var token = TokenGenerator.GenerateTokenJwt(login.Username, rolename);
                return Ok(token);
            }
          
            // Unauthorized access 
            return Unauthorized();
        }*/
    }
}