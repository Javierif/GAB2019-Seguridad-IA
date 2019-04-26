using GAB2019_ia_cybersecurity.Models.BD;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GAB2019_ia_cybersecurity.Repository
{
    public class TableRepository
    {
        private CloudTable table;
        private CloudTableClient client;
        private CloudStorageAccount storageAccount;

        public TableRepository()
        {
            string tableStorageConnectionString = "Enter Connection String";
            storageAccount = CloudStorageAccount.Parse(tableStorageConnectionString);
            client = storageAccount.CreateCloudTableClient();
            table = client.GetTableReference("users");
        }

        public void AddorReplaceUser(UserEntity user)
        {
            table.CreateIfNotExistsAsync();
            TableOperation insertOp = TableOperation.InsertOrReplace(user);

            table.ExecuteAsync(insertOp);
        }

        public async Task<UserEntity> RetrieveUserAsync(string id, string name)
        {

            bool b = table.ExistsAsync().Result;

            TableOperation retOp = TableOperation.Retrieve<UserEntity>(id, name);

            TableResult tr = await table.ExecuteAsync(retOp);

            UserEntity user = ((UserEntity)tr.Result);

            return user;
        }

        public async Task<UserEntity> GetUserByNamePassword(string name, string password)
        {
            TableQuery<UserEntity> query = new TableQuery<UserEntity>()
    .Where(
        TableQuery.CombineFilters(
            TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, name),
            TableOperators.And,
            TableQuery.GenerateFilterCondition("Password", QueryComparisons.Equal, password)
    ));

            TableQuerySegment<UserEntity> tr = await table.ExecuteQuerySegmentedAsync<UserEntity>(query, null);

            return tr.Results.Count() > 0 ? tr.Results[0] : null;
        }

        public async Task<List<UserEntity>> RetrieveAllUsersAsync()
        {

            List<UserEntity> users = new List<UserEntity>();
            var query = new TableQuery<UserEntity>();

            TableContinuationToken continuationToken = null;
            do
            {
                var page = await table.ExecuteQuerySegmentedAsync(query, continuationToken);
                continuationToken = page.ContinuationToken;
                users.AddRange(page.Results);
            }
            while (continuationToken != null);

            return users;
        }
    }
}
