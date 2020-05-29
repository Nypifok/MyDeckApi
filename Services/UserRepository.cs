using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyDeckAPI.Interfaces;
using MyDeckAPI.Models;
using MyDeckAPI.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyDeckAPI.Services
{
    public class UserRepository<User> : IGenericRepository<User> where User : class
    {
        private MDContext _context;
        private DbSet<User> table;

        public UserRepository(MDContext context)
        {
            _context = context;
            table = _context.Set<User>();
        }

        public void Delete(object Id)
        {
            var subscriptions = _context.Subscribes.Where(p => p.Publisher_Id == (Guid)Id);
            _context.Subscribes.RemoveRange(subscriptions);
            subscriptions = _context.Subscribes.Where(f => f.Follower_Id == (Guid)Id);
            _context.Subscribes.RemoveRange(subscriptions);
            User exists = table.Find(Id);
            table.Remove(exists);
        }

        public List<User> FindAll()
        {   
            return table.ToList();
        }

        public User FindById(object Id)
        {
            return table.Find(Id);
        }

        public void Insert(User obj)
        {
            table.Add(obj);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(User obj)
        {
            table.Update(obj);
        }
        public string RefreshTokens(Tokens tkns)
        {
            if (ValidateExpiredAccessToken(tkns.Access_Token))
            {
                var handler = new JwtSecurityTokenHandler();
                var tkn = handler.ReadJwtToken(tkns.Access_Token);
                var access_tkns_sample = new { id = "" };
                var access_tkn_payload = JsonConvert.DeserializeAnonymousType(tkn.Payload.SerializeToJson(), access_tkns_sample);               
                var usr = _context.Users.Find(Guid.Parse(access_tkn_payload.id));
                if (tkns.Refresh_Token == usr.RefreshToken)
                {
                    return GetNewTokens(usr.User_Id);
                }
            }
            return null;  
        }
        public string SignInByGoogle(string token)
        {
            
            if (ValidateGoogleIdToken(token))
            {
                var googleIdTokenSample = new { sub = "", email = "", email_verified = false,
                                                 picture = "",locale = ""};
                var handler = new JwtSecurityTokenHandler();
                var tkn = handler.ReadJwtToken(token);
                var idToken = JsonConvert.DeserializeAnonymousType(tkn.Payload.SerializeToJson(), googleIdTokenSample);
                string name = idToken.email.Substring(0, idToken.email.IndexOf('@'));
                var googleusr = _context.Users.Where(u => u.GoogleId ==idToken.sub ).FirstOrDefault();
                Models.User usr;
                if (googleusr != null)
                {
                    return GetNewTokens(googleusr.User_Id);
                }
                else
                {
                    if (idToken.email_verified)
                    {
                        if (IsEmailUnique(idToken.email))
                        {
                            if (IsUserNameUnique(name))
                            {
                                usr = new Models.User
                                {
                                    UserName = name,
                                    Email = idToken.email,
                                    Avatar_Path = idToken.picture,
                                    Locale = idToken.locale,
                                    GoogleId = idToken.sub
                                };

                                _context.Users.AddAsync(usr);
                                _context.SaveChangesAsync();
                                return GetNewTokens(usr.User_Id);
                            }
                            else
                            {
                                name = GetUniqueUserName(name);
                                usr = new Models.User
                                {
                                    UserName = name,
                                    Email = idToken.email,
                                    Avatar_Path = idToken.picture,
                                    Locale = idToken.locale,
                                    GoogleId = idToken.sub
                                };

                                _context.Users.AddAsync(usr);
                                _context.SaveChangesAsync();
                                return GetNewTokens(usr.User_Id);
                            }
                        }
                        else return null;


                    }
                    else return null;
                }
            }
            else return null;
             

        }
        private bool ValidateGoogleIdToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,                                   //TODO SET TRUE
                ValidAudience = AuthOptions.GOOGLEAUDIENCE,
                ValidIssuer = AuthOptions.GOOGLEISSUER,
                IssuerSigningKey = AuthOptions.GetGoogleSecurityKey(),
                ValidateIssuerSigningKey = true
            };
            SecurityToken validatedToken;
            try
            {
                tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
            }
            catch (Exception)
            {
                return false;
            }
            return validatedToken != null;
        }
        private bool ValidateExpiredAccessToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,                                   //TODO SET TRUE
                ValidAudience = AuthOptions.AUDIENCE,
                ValidIssuer = AuthOptions.ISSUER,
                IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                ValidateIssuerSigningKey = true
            };
            SecurityToken validatedToken;
            try
            {
                tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
            }
            catch (Exception)
            {
                return false;
            }
            return validatedToken != null;
        }
        public bool IsUserNameUnique(string name) 
        {
            var username = name;
            var usr = _context.Users.FirstOrDefault(u => u.UserName == username);
            if (usr!=null) { return false;}
            return true;
        }
        public bool IsEmailUnique(string email)
        {
            var tmpemail = email;
            var usr = _context.Users.FirstOrDefault(u => u.Email == tmpemail);
            if (usr != null) { return false; }
            return true;
        }
        private string GetUniqueUserName(string name)
        {
            Random random = new Random();
            var tmp = random.Next(0,9999999);
            var tmpname = name + Convert.ToString(tmp);
            if (IsUserNameUnique(tmpname))
            {
                return tmpname;
            }
            else
            {
                return GetUniqueUserName(name);
            }
        }
        public string GetNewTokens(Guid userid)
        {
            var usr = _context.Users.Find(userid);
            var now = DateTime.UtcNow;
            List<Claim> claim = new List<Claim> { new Claim(ClaimsIdentity.DefaultRoleClaimType, usr.Role_Name),
                                                   new Claim(ClaimsIdentity.DefaultNameClaimType,usr.UserName),
                                                    new Claim(type:"id",value:usr.User_Id.ToString())};
            ClaimsIdentity claims = new ClaimsIdentity(claim, "Bearer",ClaimsIdentity.DefaultNameClaimType,ClaimsIdentity.DefaultRoleClaimType);
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: claims.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var jwtrfrsh = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: claims.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.REFRESH_LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            string refreshToken = new JwtSecurityTokenHandler().WriteToken(jwtrfrsh);
            usr.RefreshToken = refreshToken;
            _context.SaveChangesAsync();
            var response = new
            {
                access_token = encodedJwt,
                refresh_token=refreshToken,
                userid
            };
            return JsonConvert.SerializeObject(response);
        }
        public string UserProfile(Guid userid)
        {
            var usr = _context.Users.Find(userid);
            var user = new {usr.User_Id,usr.UserName,usr.Avatar_Path,usr.Locale};
            var subs = _context.Subscribes.Where(s => s.Publisher_Id == userid).Count();
            var follows = _context.Subscribes.Where(s => s.Follower_Id == userid).Count();
            var content = new {User=user,Subscribers=subs,Follows=follows};
            return JsonConvert.SerializeObject(content);
        }
        public string SubscribersOfDeck(Guid userid)
        {
            var usr = _context.Users.Find(userid);
            var subs = _context.Subscribes.Where(s => s.Publisher_Id == userid).Count();
            var follows = _context.Subscribes.Where(s => s.Follower_Id == userid).Count();
            var content = new { user = usr, subscribers = subs, follows };
            return JsonConvert.SerializeObject(content);
        }
    }
}
