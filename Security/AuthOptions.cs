using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MyDeckAPI.Security
{
    public class AuthOptions
    {
        public const string ISSUER = "MyDeckAPI";
        public const string AUDIENCE = "MyDeck";
        public const string GOOGLEAUDIENCE = "292694254456-dau67egnnm7s513fofc4gvu5ftjut9le.apps.googleusercontent.com";
        public const string GOOGLEISSUER = "https://accounts.google.com";
        private const string KEY = "2tvPsIUvPTegtAH5TZ7e9ktUUGctOnOkwfiE98luLdcsoUeFECRk55wclKZWlXau";
        private const string EMAILCONFIRMATIONKEY = "2tvPsIUvPTegtAH5TZ7e9ktUUGsoUeFECRk55wclKZWlXauFECRkKZWlXauoUeFs";
        private const string GOOGLEKEY = "tLECRAH5TZ7e9ktUUGct2tvPdcsE98luk55wIUvPTegOnOkwficlKZWlXauoUeFs";
        public const int LIFETIME = 30;
        public const int REFRESH_LIFETIME = 43800;
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Convert.FromBase64String(KEY));
        }
        public static SymmetricSecurityKey GetEmailConfirmationSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Convert.FromBase64String(EMAILCONFIRMATIONKEY));
        }
        public static SymmetricSecurityKey GetGoogleSecurityKey()
        {
            return new SymmetricSecurityKey(Convert.FromBase64String(GOOGLEKEY));
        }
        public async Task<byte[]> GetPasswordWithSalt(byte[] password)
        {
            var rng = new RNGCryptoServiceProvider();
            Random random = new Random();
            var pass = password;
            int saltSize = random.Next(4, 8);
            var salt = new byte[saltSize];
            var passwordHash = new byte[saltSize + pass.Length];

            rng.GetNonZeroBytes(salt);
            for (int i = 0; i < pass.Length; i++)
                passwordHash[i] = pass[i];


            for (int i = 0; i < salt.Length; i++)
                passwordHash[pass.Length + i] = salt[i];

            using (SHA256 sha256Hash = SHA256.Create())
            {
                 byte[] hashBytes = sha256Hash.ComputeHash(passwordHash);
                 byte[] passwordHashWithSalt = new byte[hashBytes.Length +
                                           salt.Length];
                for (int i = 0; i < hashBytes.Length; i++)
                    passwordHashWithSalt[i] = hashBytes[i];

                for (int i = 0; i < salt.Length; i++)
                    passwordHashWithSalt[hashBytes.Length + i] = salt[i];
                return passwordHashWithSalt;
            }
        }
    }
}
