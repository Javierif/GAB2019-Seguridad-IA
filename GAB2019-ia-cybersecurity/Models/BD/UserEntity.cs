using Microsoft.WindowsAzure.Storage.Table;

namespace GAB2019_ia_cybersecurity.Models.BD
{
    public class UserEntity : TableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string SpeakerGuid { get; set; }
        public string FaceGuid { get; set; }
        public int Enroll { get; set; }
        public int FaceCount { get; set; }

        public UserEntity() { }
        public UserEntity(int id, string name, string password, string speakerGuid, string faceGuid, int enroll, int faceCount)
        {
            Id = id;
            Name = name;
            Password = password;
            SpeakerGuid = speakerGuid;
            FaceGuid = faceGuid;
            Enroll = enroll;
            FaceCount = faceCount;
            PartitionKey = id.ToString();
            RowKey = name;
        }

    }
}