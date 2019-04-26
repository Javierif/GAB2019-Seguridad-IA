using GAB2019_ia_cybersecurity.Models;
using GAB2019_ia_cybersecurity.Models.BD;
using GAB2019_ia_cybersecurity.Models.Speaker;
using GAB2019_ia_cybersecurity.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Microsoft.Extensions.Primitives;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace GAB2019_ia_cybersecurity.Controllers
{
    [Route("api/[controller]")]
    public class FaceController : Controller
    {
        private FaceRepository api;
        public FaceController()
        {
            api = new FaceRepository();
        }


        [HttpPost("creategroup")]
        public async Task<Person> CreatePerson()
        {
            HttpContext.Request.Headers.TryGetValue("name", out StringValues name);
            
            string persongroupPersonGroupId = "authorized";

           return await api.CreatePerson(persongroupPersonGroupId,name);
        }
        // GET: api/Speaker
        [HttpPost("addFaceToPerson")]
        public async Task<PersistedFace> AddFaceToPerson([FromBody] string image)
        {
            HttpContext.Request.Headers.TryGetValue("personId", out StringValues personId);
            HttpContext.Request.Headers.TryGetValue("id", out StringValues id);
            HttpContext.Request.Headers.TryGetValue("name", out StringValues name);
            TableRepository table = new TableRepository();
            UserEntity user = await table.RetrieveUserAsync(id, name);
            user.FaceCount += 1;
            table.AddorReplaceUser(user);
            
            return await api.AddFaceToPerson("authorized", personId, image);
        }
                // GET: api/Speaker
        [HttpPost("detectPerson")]
        public async Task<string> DetectPerson([FromBody] string image)
        {
            HttpContext.Request.Headers.TryGetValue("personId", out StringValues personId);
            HttpContext.Request.Headers.TryGetValue("id", out StringValues id);
            HttpContext.Request.Headers.TryGetValue("name", out StringValues name);
            TableRepository table = new TableRepository();
            UserEntity user = await table.RetrieveUserAsync(id, name);

            Person personDetected = await api.DetectPerson(image);

            if(personDetected == null)
            {
                return "{\"error\":true,\"msg\":\"No detectado personas conocidas en la imagen\"}";
            } else
            {
                if(personDetected.Name == name) {
                    return "{\"error\":false}";

                }
                else 
                {
                    return "{\"error\":true,\"msg\":\"No se ha detectado el usuario, se ha detectado a "+ personDetected.Name+"\"}";
                }
            }
        }

        [HttpGet("getPerson/{personId}")]
        public async Task<Person> GetPerson( string personId)
        {
            return await api.GetPerson("authorized", personId);
        }

    }
}
