using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WEB_API.Helpers;
using WEB_API.models;


namespace WEB_API.Services
{
    public interface IUserService
    {
        Users Authenticate(string username, string password);
        IEnumerable<Users> GetAll();
    }

    public class UserService : IUserService
    {
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications

        private readonly AppSettings _appSettings;
        private readonly StartupDbContext _context;


        public UserService(IOptions<AppSettings> appSettings, StartupDbContext context)
        {
            _appSettings = appSettings.Value;
            _context = context;

        }


       

        public Users Authenticate(string username, string password)
        {

            var user = _context.Users.SingleOrDefault(x => x.Username == username && x.Password == password);
           
            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            return user.WithoutPassword();
        }
          [HttpDelete("{id}")]
        public ActionResult<Users> DeleteItem(int id)
        {
            var Items = _context.Users.Find(id);

            // if (Items == null)
            // {
            //     return NotFound();
            // }

            _context.Users.Remove(Items);
            _context.SaveChanges();

            return Items;
        }

        public IEnumerable<Users> GetAll()
        {

            return _context.Users.WithoutPasswords();
        }
    }
}