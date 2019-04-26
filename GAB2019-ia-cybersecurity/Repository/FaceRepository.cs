using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GAB2019_ia_cybersecurity.Repository
{
    public class FaceRepository
    {
        private FaceClient faceClient;
        private string Facekey = "Enter Face Key";

        public FaceRepository()
        {
            faceClient = new FaceClient(new ApiKeyServiceClientCredentials(Facekey),
    new DelegatingHandler[] { });
            faceClient.Endpoint = "https://westeurope.api.cognitive.microsoft.com";
        }

        
        public async Task<Person> CreatePerson(string persongroupPersonGroupId, string name)
        {
            //await faceClient.PersonGroup.CreateAsync(persongroupPersonGroupId, persongroupName);
            Person p = await faceClient.PersonGroupPerson.CreateAsync(persongroupPersonGroupId, name);
            await faceClient.PersonGroup.TrainAsync(persongroupPersonGroupId);
            return p;
        }
        public async Task<PersistedFace> AddFaceToPerson(string groupId, string personId, string data)
        {
            Guid personGuid = Guid.Parse(personId);

            var base64Data = Regex.Match(data, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
            var binData = Convert.FromBase64String(base64Data);

            using (Stream imageFileStream = new MemoryStream(binData))
            {
                var result = await faceClient.PersonGroupPerson.AddFaceFromStreamAsync(
                    groupId, personGuid, imageFileStream);
                await faceClient.PersonGroup.TrainAsync(groupId);
                return result;
            }
        }
        
        public async Task<Person> GetPerson(string groupId, string personId)
        {
            Guid personGuid = Guid.Parse(personId);
            return await faceClient.PersonGroupPerson.GetAsync(groupId, personGuid);
        }
        public async Task<Person> DetectPerson(string data)
        {
            var base64Data = Regex.Match(data, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
            var binData = Convert.FromBase64String(base64Data);
            Person p = null;
            using (Stream imageFileStream = new MemoryStream(binData))
            {
                var faces = await faceClient.Face.DetectWithStreamAsync(imageFileStream);
                List<Guid> faceIds = faces.Select(face => face.FaceId.Value).ToList();
                if (faceIds.Count == 0) return p;

                var results = await faceClient.Face.IdentifyAsync(faceIds, "authorized");
                foreach (var identifyResult in results)
                {
                    Console.WriteLine("Result of face: {0}", identifyResult.FaceId);
                    if (identifyResult.Candidates.Count == 0)
                    {
                        Console.WriteLine("No one identified");
                    }
                    else
                    {
                        // Get top 1 among all candidates returned
                        var candidateId = identifyResult.Candidates[0].PersonId;
                        var person = await faceClient.PersonGroupPerson.GetAsync("authorized", candidateId);
                        Console.WriteLine("Identified as {0}", person.Name);
                        if (p == null)
                        {
                            p = person;
                        }
                    }
                }
            }
            return p;
        }
    }
}
