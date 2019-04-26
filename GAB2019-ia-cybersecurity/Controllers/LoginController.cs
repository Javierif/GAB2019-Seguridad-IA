using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using GAB2019_ia_cybersecurity.Models;
using GAB2019_ia_cybersecurity.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.Extensions.Primitives;
using GAB2019_ia_cybersecurity.Repository;
using GAB2019_ia_cybersecurity.Models.BD;

namespace GAB2019_ia_cybersecurity.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private TableRepository table;

        public LoginController()
        {
            table = new TableRepository();
        }
        [HttpPost("firstlogin")]
        public async Task<string> FirstLoginAsync()
        {
            HttpContext.Request.Headers.TryGetValue("name", out StringValues name);
            HttpContext.Request.Headers.TryGetValue("password", out StringValues password);

            var user = await table.GetUserByNamePassword(name,password);
            return user == null ? "{ \"error\": true }" : JsonConvert.SerializeObject(user);
        }
        [HttpGet("getall")]
        public async Task<List<UserEntity>> GetAllUsers()
        {
            return await table.RetrieveAllUsersAsync();
        }

        [HttpGet("get/{id}/{name}")]
        public async Task<UserEntity> GetUserAsync(string id, string name)
        {
            return await table.RetrieveUserAsync(id, name);
        }
    }
}
