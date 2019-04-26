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
using GAB2019_ia_cybersecurity.Models.Speaker;

namespace GAB2019_ia_cybersecurity.Controllers
{
    [Route("api/[controller]")]
    public class SpeakerController : Controller
    {
        private SpeakerRepository api;
        private string SpeakerKey = "Enter Speaker API KEy";
        public SpeakerController()
        {
            api = new SpeakerRepository();
        }

        // GET: api/Speaker
        [HttpGet("GetVerificationPhrases")]
        public async Task<List<VerificationPhrase>> GetVerificationPhrases()
        {
            return await api.GetVerificationPhrases(SpeakerKey);
        }
        [HttpGet("CreateVerificationProfile")]
        public async Task<CreateVerificationProfileModel> CreateVerificationProfile()
        {
            return await api.CreateVerificationProfile(SpeakerKey);            
        }
        [HttpPost("CreateEnrollment")]
        public async Task<EnrollmentResult> CreateEnrollment( [FromBody] MemoryStream audioStream)
        {
            HttpContext.Request.Headers.TryGetValue("id", out StringValues id);
            HttpContext.Request.Headers.TryGetValue("name", out StringValues name);

            TableRepository table = new TableRepository();
            UserEntity user = await table.RetrieveUserAsync(id, name);

            var enrollResult = await api.CreateEnrollment(SpeakerKey,audioStream, user.SpeakerGuid);

            if(enrollResult != null && enrollResult.EnrollmentsCount >= 1)
            {
                user.Enroll += 1;
                table.AddorReplaceUser(user);
            }

            return enrollResult;
        }

        [HttpPost("Verify")]
        public async Task<VerifyResult> Verify( [FromBody] MemoryStream audioStream)
        {
            HttpContext.Request.Headers.TryGetValue("guid", out StringValues guid);
            
            var enrollResult = await api.Verify(SpeakerKey,audioStream, guid);

            return enrollResult;
        }

        [HttpPost("Test")]
        public string Test([FromBody] string content)
        {

            return "RESPONSE ES " + content;
        }

    }
}
