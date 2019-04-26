using GAB2019_ia_cybersecurity.Models;
using GAB2019_ia_cybersecurity.Models.Speaker;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GAB2019_ia_cybersecurity.Repository
{
    public class SpeakerRepository
    {
        public async Task<List<VerificationPhrase>> GetVerificationPhrases(string APIKEY)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", APIKEY);

            var uri = $"https://westus.api.cognitive.microsoft.com/spid/v1.0/verificationPhrases?locale=en-US";

            var response = await client.GetAsync(uri);

            string responseText = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<VerificationPhrase>>(responseText);
        }
        public async Task<CreateVerificationProfileModel> CreateVerificationProfile(string APIKEY)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", APIKEY);

            var uri = "https://westus.api.cognitive.microsoft.com/spid/v1.0/verificationProfiles?";

            HttpResponseMessage response;

            byte[] byteData = Encoding.UTF8.GetBytes("{\"locale\":\"en-us\",}");

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response = await client.PostAsync(uri, content);

                string responseText = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<CreateVerificationProfileModel>(responseText);
            }
        }
        public async Task<EnrollmentResult> CreateEnrollment(string APIKEY, MemoryStream audioStream, string SpeakerGUID)
        {
            byte[] result = audioStream.ToArray();

            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", APIKEY);

            var uri = $"https://westus.api.cognitive.microsoft.com/spid/v1.0/verificationProfiles/{SpeakerGUID}/enroll";

            HttpResponseMessage response;

            using (var content = new ByteArrayContent(result))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response = await client.PostAsync(uri, content);

                string responseStr = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<EnrollmentResult>(responseStr);
            }

        }

        public async Task<VerifyResult> Verify(string APIKEY, MemoryStream audioStream, string SpeakerGUID)
        {
            byte[] result = audioStream.ToArray();

            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", APIKEY);

            var uri = $"https://westus.api.cognitive.microsoft.com/spid/v1.0/verify?verificationProfileId={SpeakerGUID}";

            HttpResponseMessage response;

            using (var content = new ByteArrayContent(result))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response = await client.PostAsync(uri, content);

                string responseStr = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<VerifyResult>(responseStr);
            }

        }
    }
}
