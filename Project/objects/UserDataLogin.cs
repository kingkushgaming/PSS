using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace Project.objects
{


    public class UserDataLogin
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        [JsonConstructor] 
        public UserDataLogin(string Email, string Password)
        {
            this.Email = Email;
            this.PasswordHash = Password; 
        }

        public UserDataLogin(string email, string password, bool hashPassword)
        {
            Email = email;
            PasswordHash = hashPassword ? HashPassword(password) : password;
        }

        public UserDataLogin(string email)
        {
            Email = email;
        }

        public void SetPassword(string password)
        {
            PasswordHash = HashPassword(password);
        }

        public bool ValidatePassword(string password)
        {
            return PasswordHash == HashPassword(password);
        }

        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder sb = new StringBuilder();
                foreach (byte b in bytes)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }


}
