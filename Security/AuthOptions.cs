using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
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
        const string KEY = "2tvPsIUvPTegtAH5TZ7e9ktUUGctOnOkwfiE98luLdcsoUeFECRk55wclKZWlXau";
        const string GOOGLEKEY = "tLECRAH5TZ7e9ktUUGct2tvPdcsE98luk55wIUvPTegOnOkwficlKZWlXauoUeFs";
        public const int LIFETIME = 1; 
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Convert.FromBase64String(KEY));
        }
        public static SymmetricSecurityKey GetGoogleSecurityKey()
        {
            return new SymmetricSecurityKey(Convert.FromBase64String(GOOGLEKEY));
        }
    }
}
