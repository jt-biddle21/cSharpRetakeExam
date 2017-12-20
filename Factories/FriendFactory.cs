using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;
using MySql.Data.MySqlClient;
using cSharpRetakeExam.Models;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace cSharpRetakeExam.Factory
{
    public class FriendFactory : IFactory<User>
    {
        private readonly IOptions<MySqlOptions> MySqlConfig;

        public FriendFactory(IOptions<MySqlOptions> config)
        {
            MySqlConfig = config;
        }
        internal IDbConnection Connection
        {
            get
            {
                return new MySqlConnection(MySqlConfig.Value.ConnectionString);
            }
        }
        public void AddFriend(int x, string Name)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string query = $"UPDATE friends SET InviteId = '{x}' WHERE FriendName = '{Name}'";
                dbConnection.Open();
                dbConnection.Execute(query);
            }
        }
        public IEnumerable<Friend> ShowAllFriends(int x)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string query = $"SELECT * FROM friends WHERE InviteId = {x}";
                return dbConnection.Query<Friend>(query);
            }
        }

         public IEnumerable<Friend> Invitations(int x)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string query = $"SELECT FriendName, Users_Id, InviteId FROM friends WHERE friends.Users_Id = {x}";
                return dbConnection.Query<Friend>(query);
            }
        }
    }
}
