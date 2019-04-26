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
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;

namespace GAB2019_ia_cybersecurity.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private TableRepository table;
        private string SpeakerApiKey = "Enter Speaker API KEY";

        public UserController()
        {
            table = new TableRepository();
        }
        [HttpPost("add")]
        public async Task<bool> AddUserAsync()
        {
            HttpContext.Request.Headers.TryGetValue("id", out StringValues id);
            HttpContext.Request.Headers.TryGetValue("name", out StringValues name);
            HttpContext.Request.Headers.TryGetValue("password", out StringValues password);

            SpeakerRepository speakerApi = new SpeakerRepository();
            CreateVerificationProfileModel profile = await speakerApi.CreateVerificationProfile(SpeakerApiKey);
            FaceRepository faceApi = new FaceRepository();
            Person person = await faceApi.CreatePerson("authorized", name);

            UserEntity user = new UserEntity(Int32.Parse(id), name, password, profile.VerificationProfileId.ToString(), person.PersonId.ToString(), 0,0);
            table.AddorReplaceUser(user);

            return true;
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
