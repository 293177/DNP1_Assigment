using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using DNP1_A1.Models;

namespace DNP1_A1.Login
{
    public class UserService : IUser
    {
        private List<User> users;
        private string userFile = "users.json";

        public UserService()
        {
            string content = File.ReadAllText(userFile);
            users = JsonSerializer.Deserialize<List<User>>(content);
        }

        public User ValidateUser(string username, string password)
        {
            User first = users.FirstOrDefault(user => user.username.Equals(username));
            if (first == null)
            {
                throw new Exception("user not found");
            }

            if (!first.password.Equals(password))
            {
                throw new Exception("Password is incorrect");
            }

            return first;
        }

        public void AddUser(string username, string password, string role)
        {
            User user = new User()
            {
                username = username,
                password = password,
                Role = role
            };
            users.Add(user);
            writeUserFile(users);
        }

        private void writeUserFile(List<User> user)
        {
            string productAsJson = JsonSerializer.Serialize(user);
            File.WriteAllText(userFile, productAsJson);
        }

        public List<User> getUsers()
        {
            return users;
        }

        public void RemoveUser(User toRemove)
        {
            User toBeRemoved = users.First(t => t.username == toRemove.username);
            users.Remove(toRemove);
            writeUserFile(users);
        }
    }
}