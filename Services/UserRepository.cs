using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyDeckAPI.Data.MediaContent;
using MyDeckAPI.Exceptions;
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
    public class UserRepository : IGenericRepository
    {
        private MDContext _context;
        private DbSet<User> table;
        private readonly SnakeCaseConverter snakeCaseConverter;
        private readonly AuthOptions security;
        private readonly MailService mailService;

        public UserRepository(MDContext context, SnakeCaseConverter snakeCaseConverter, AuthOptions security, MailService mailService)
        {
            _context = context;
            table = _context.Set<User>();
            this.snakeCaseConverter = snakeCaseConverter;
            this.mailService = mailService;
            this.security = security;
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
        //TODO cellphone  signin/signup      
        //TODO email signin/signup           
        //TODO apple ID signin               
        //TODO facebook signin               
        //TODO cellphone verify + code verfiy
        //TODO email verify + code verify    
        //TODO password change               
        //TODO username change               
        //TODO email change                  
        //TODO problems with username        
        //TODO pт others problems               
        public async Task<string> RefreshTokens(Tokens tkns, Guid sessionId)
        {
            if (ValidateExpiredAccessToken(tkns.Access_Token))
            {
                var handler = new JwtSecurityTokenHandler();
                var tkn = handler.ReadJwtToken(tkns.Access_Token);
                var access_tkns_sample = new { id = "" };
                var access_tkn_payload = JsonConvert.DeserializeAnonymousType(tkn.Payload.SerializeToJson(), access_tkns_sample);
                var session = await _context.Sessions.FindAsync(sessionId, Guid.Parse(access_tkn_payload.id));
                if (session == null) { throw new Exception("Non existent session"); }

                var usr = await _context.Users.FindAsync(Guid.Parse(access_tkn_payload.id));
                if (usr == null) { throw new Exception("Non existent user"); }

                if (tkns.Refresh_Token == session.RefreshToken)
                {
                    return await GetNewTokens(usr.User_Id, sessionId);
                }
            }
            throw new Exception();
        }
        public async Task<string> SignInByGoogle(string token, Guid sessionId)
        {
            try
            {
                if (await ValidateGoogleIdToken(token))
                {
                    var googleIdTokenSample = new
                    {
                        sub = "",
                        email = "",
                        email_verified = false,
                        picture = "",
                        locale = ""
                    };
                    var handler = new JwtSecurityTokenHandler();
                    var tkn = handler.ReadJwtToken(token);
                    var idToken = JsonConvert.DeserializeAnonymousType(tkn.Payload.SerializeToJson(), googleIdTokenSample);
                    string name = idToken.email.Substring(0, idToken.email.IndexOf('@'));
                    var googleusr = _context.Users.Where(u => u.GoogleId == idToken.sub).FirstOrDefault();
                    User usr;

                    if (googleusr != null)
                    {
                        return await GetNewTokens(googleusr.User_Id, sessionId);
                    }
                    else
                    {
                        if (idToken.email_verified)
                        {
                            if (IsEmailUnique(idToken.email))
                            {
                                var guid = Guid.NewGuid();
                                _context.Files.Add(new File() { File_Id = guid, Md5 = "", Path = "", Size = 12, Type = "" });
                                if (IsUserNameUnique(name))
                                {
                                    usr = new User
                                    {
                                        UserName = name,
                                        Email = idToken.email,
                                        Locale = idToken.locale,
                                        GoogleId = idToken.sub,
                                        Role_Name = "User",
                                        Avatar = guid
                                    };
                                    //TODO AVATAR

                                    await _context.Users.AddAsync(usr);
                                    await _context.SaveChangesAsync();
                                    return await GetNewTokens(usr.User_Id, sessionId);
                                }
                                else
                                {
                                    name = GetUniqueUserName(name);
                                    usr = new User
                                    {
                                        UserName = name,
                                        Email = idToken.email,
                                        Locale = idToken.locale,
                                        GoogleId = idToken.sub,
                                        Role_Name = "User",
                                        Avatar = guid
                                    };

                                    _context.Users.AddAsync(usr);
                                    _context.SaveChangesAsync();
                                    return await GetNewTokens(usr.User_Id, sessionId);
                                }
                            }
                            else throw new AlreadyUsedEmailException();


                        }
                        else throw new NonConfirmedEmailException();
                    }
                }
                else throw new NonValidatedGoogleIdTokenException();
            }
            catch { throw; }
        }
        private async Task<bool> ValidateGoogleIdToken(string token)
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
                ValidateLifetime = false,
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
            if (usr != null) { return false; }
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
            var tmp = random.Next(0, 9999999);
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
        public async Task<string> GetNewTokens(Guid userid, Guid sessionId)
        {
            var usr = _context.Users.Find(userid);
            var now = DateTime.UtcNow;
            List<Claim> claim = new List<Claim> { new Claim(ClaimsIdentity.DefaultRoleClaimType, usr.Role_Name),
                                                   new Claim(ClaimsIdentity.DefaultNameClaimType,usr.UserName),
                                                    new Claim(type:"id",value:usr.User_Id.ToString()),
                                                    new Claim(type:"role",value:usr.Role_Name)};
            ClaimsIdentity claims = new ClaimsIdentity(claim, "Bearer", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: claims.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var jwtrfrsh = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    notBefore: now,
                    claims: claims.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.REFRESH_LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            string refreshToken = new JwtSecurityTokenHandler().WriteToken(jwtrfrsh);
            var session = await _context.Sessions.FindAsync(sessionId, userid);
            if (session == null)
            {
                _context.Sessions.AddAsync(new Session() { User_Id = userid, Session_Id = sessionId, RefreshToken = refreshToken });
            }
            else
            {
                session.User_Id = userid;
                session.Session_Id = sessionId;
                session.RefreshToken = refreshToken;
            }

            _context.SaveChangesAsync();
            var response = new
            {
                access_token = encodedJwt,
                refresh_token = refreshToken,
                user_Id = userid
            };
            return snakeCaseConverter.ConvertToSnakeCase(response);
        }

        private async Task<string> GetEmailConfirmationToken(Guid userid)
        {
            var now = DateTime.UtcNow;
            List<Claim> claim = new List<Claim> { new Claim(type: "id", value: userid.ToString()) };
            ClaimsIdentity claims = new ClaimsIdentity(claim, "Bearer", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: claims.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetEmailConfirmationSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);


            return encodedJwt;
        }
        public string UserProfile(Guid userid)
        {
            var usr = _context.Users.Find(userid);
            var user = new { usr.User_Id, usr.UserName/*,usr.Avatar_Path*/, usr.Locale };
            var subs = _context.Subscribes.Where(s => s.Publisher_Id == userid).Count();
            var follows = _context.Subscribes.Where(s => s.Follower_Id == userid).Count();
            var content = new { User = user, Subscribers = subs, Follows = follows };
            return snakeCaseConverter.ConvertToSnakeCase(content);
        }
        public string SubscribersOfDeck(Guid userid)
        {
            var usr = _context.Users.Find(userid);
            var subs = _context.Subscribes.Where(s => s.Publisher_Id == userid).Count();
            var follows = _context.Subscribes.Where(s => s.Follower_Id == userid).Count();
            var content = new { user = usr, subscribers = subs, follows };
            return snakeCaseConverter.ConvertToSnakeCase(content);
        }

        public async Task<string> SignUpWithEmail(User usr, Guid sessionId)
        {
            try
            {
                var convertedUsr = usr;
                if (IsEmailUnique(convertedUsr.Email))
                {
                    if (IsUserNameUnique(convertedUsr.UserName))
                    {
                        var usrGuid = Guid.NewGuid();
                        var guid = Guid.NewGuid();
                        _context.Files.Add(new File() { File_Id = guid, Md5 = "", Path = "", Size = 12, Type = "" });
                        var user = new User()
                        {
                            User_Id = usrGuid,
                            Avatar = guid,
                            Role_Name = "User",
                            UserName = convertedUsr.UserName,
                            Email = convertedUsr.Email,
                            Password = await security.GetPasswordWithSalt(convertedUsr.Password),
                            Tag = "NonConfirmed"
                        };
                        _context.Users.AddAsync(user);
                        _context.SaveChangesAsync();
                        var token = await GetEmailConfirmationToken(usrGuid);
                        mailService.SendConfirmationEmail(convertedUsr.Email, token);

                        return token;

                    }
                    else
                    {
                        throw new AlreadyUsedNameException();
                    }
                }
                else
                {
                    throw new AlreadyUsedEmailException();
                }

            }
            catch
            {
                throw;
            }
        }

    }
}
