using System.Collections.Generic;
using System.IO;
using DatingApp.API.Models;
using Newtonsoft.Json;

namespace DatingApp.API.Data
{
    //This class is purely for development purpose, not for production.
    public class Seed
    {
        private readonly DataContext _context;
        
        public Seed(DataContext context)
        {
            _context = context;
            
        }

        public void SeedUsers()
        {
            var userData = File.ReadAllText("Data/UserSeedData.json");
            var users = JsonConvert.DeserializeObject<List<User>>(userData);

            foreach (var user in users)
            {
                 byte[] passwordHash, passwordSalt;

            createPasswordHash("password123", out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.UserName = user.UserName.ToLower();

            _context.Users.Add(user);

            }

            _context.SaveChanges();
        }

        private void createPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
               passwordSalt = hmac.Key;
               passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}