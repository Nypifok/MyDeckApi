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
        public string SignInByGoogle(string token)
        {

            if (ValidateGoogleIdToken(token))
            {
                var handler = new JwtSecurityTokenHandler();
                var tkn = handler.ReadJwtToken(token);
                var jsontkn = tkn.Payload;
                object email;
                jsontkn.TryGetValue("email", out email);
                string tmpemail = Convert.ToString(email);
                string name = Convert.ToString(tmpemail.Split(new char[] { '@' }, 1));
                object isVerified;
                jsontkn.TryGetValue("email_verified", out isVerified);
                Models.User usr;
                if (Convert.ToBoolean(isVerified))
                {
                    if (IsEmailUnique(tmpemail))
                    {
                        if (IsUserNameUnique(name))
                        {
                            object googleId;
                            jsontkn.TryGetValue("sub", out googleId);
                            object avatarPath;
                            jsontkn.TryGetValue("picture", out avatarPath);
                            object locale;
                            jsontkn.TryGetValue("locale", out locale);
                            Guid tmpId = new Guid();
                            usr = new Models.User
                            {
                                User_Id = tmpId,
                                UserName = name,
                                Email = tmpemail,
                                Avatar_Path = Convert.ToString(avatarPath),
                                Locale = Convert.ToString(locale),
                                GoogleId = Convert.ToString(googleId)
                            };

                            _context.Users.Add(usr);
                            _context.SaveChanges();
                            return GetNewTokens(usr.User_Id);
                        }
                        else
                        {
                            name = GetUniqueUserName(name);
                            object googleId;
                            jsontkn.TryGetValue("sub", out googleId);
                            object avatarPath;
                            jsontkn.TryGetValue("picture", out avatarPath);
                            object locale;
                            jsontkn.TryGetValue("locale", out locale);
                            Guid tmpId = new Guid();
                            usr = new Models.User
                            {
                                User_Id = tmpId,
                                UserName = name,
                                Email = tmpemail,
                                Avatar_Path = Convert.ToString(avatarPath),
                                Locale = Convert.ToString(locale),
                                GoogleId = Convert.ToString(googleId)
                            };
                            return GetNewTokens(usr.User_Id);
                        }
                    }
                    else return null;
                    

                }
                else return null;
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
                ValidateLifetime = false,                                   //TODO SET TRUE
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
            var now = DateTime.UtcNow;
            ClaimsIdentity claims = new ClaimsIdentity("Google");
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: claims.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                userid
            };
            return JsonConvert.SerializeObject(response);
        }
    }
}
