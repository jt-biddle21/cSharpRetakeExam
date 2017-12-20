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
    public class UserFactory : IFactory<User>
    {
        private readonly IOptions<MySqlOptions> MySqlConfig;

        public UserFactory(IOptions<MySqlOptions> config)
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
        public void AddUser(User T, string pass)
        {
            //Hash @Password right here and then put into database
            using (IDbConnection dbConnection = Connection)
            {
                string query = $"INSERT INTO users (Name, Email, Password, Description) VALUES (@Name, @Email, '{pass}', @Description)";
                dbConnection.Open();
                dbConnection.Execute(query, T);
            }
        }
        public IEnumerable<User> ShowAllUsers(int x)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string query = $"SELECT * FROM users WHERE Id != {x}";
                dbConnection.Open();
                return dbConnection.Query<User>(query);
            }
        }

        public IEnumerable<User> GetUserByEmail(string Email)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string query = $"SELECT Password, Id FROM users WHERE Email = '{Email}'";
                dbConnection.Open();
                return dbConnection.Query<User>(query);
            }
        }

        public IEnumerable<User> GetUserById(int x)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string query = $"SELECT * FROM users WHERE users.Id = '{x}'";
                return dbConnection.Query<User>(query);
            }
        }

        public IEnumerable<User> GetUserByName(string Name)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string query = $"SELECT * FROM users WHERE users.Name = '{Name}'";
                return dbConnection.Query<User>(query);
            }
        }

         public void Ignore(string Name)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string query = $"DELETE FROM friends WHERE friends.FriendName = '{Name}'";
                dbConnection.Execute(query);
            }
        }

        public IEnumerable<User> RequestConnection(int Id, int x, string name, string des)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string query = $"INSERT INTO friends (FriendName, FriendDescription, Users_Id, InviteId) VALUES ('{name}', '{des}', '{Id}', 0)";
                return dbConnection.Query<User>(query);
            }
        }
    }
}
