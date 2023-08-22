using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using MyFace.Models.Database;
using MyFace.Repositories;
using MyFace.Helpers;

namespace MyFace.Helpers
{
    public class AuthHelper
    {
        public string DecodedString { get; set; }
        public bool IsAuthenticated { get; set; }
        public UsersRepo usersRepo{ get; set; }

        public AuthHelper(string encodedString, IUsersRepo usersRepo)
        {
            byte[] data = Convert.FromBase64String(encodedString.Substring(6));
            DecodedString = System.Text.Encoding.UTF8.GetString(data);
            
            string [] authParts = DecodedString.Split(":");
            var user = usersRepo.GetByUsername(authParts[0]);
            var passwordHelper = new PasswordHelper(authParts[1]);

            if (passwordHelper.getHashedPassword(authParts[1], user.Salt) == user.HashedPassword)
            {
                IsAuthenticated = true;
            } else {
                IsAuthenticated = false;
            };
        }
    }
}