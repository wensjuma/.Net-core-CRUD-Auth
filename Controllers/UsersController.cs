using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using System.Linq;
using WEB_API.models;
using WEB_API.Services;
using System;

namespace WEB_API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly StartupDbContext _context;
        // +private readonly RandomNumberGenerator  _random;
        private IUserService _userService;


        public UsersController(IUserService userService, StartupDbContext context)
        {
            _userService = userService;
            _context = context;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]AuthenticateModel model)
        {
            var user = _userService.Authenticate(model.Username, model.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }
        [HttpDelete("{id}")]


        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }
        [HttpPost("register")]
        [AllowAnonymous]
        public IActionResult Create(Users user, string password)
        {
            var userExists = _context.Users.FirstOrDefault(x => x.Username == user.Username);

            
            if (userExists != null)
            {
               
               //ModelState.AddModelError("Username", "Username taken");
                 return BadRequest(new { message = "Username is already taken!" });
            }
            else{
            var _userdetails = new Users
            {
                FirstName = user.FirstName,
                Username = user.Username,
                Password = user.Password
            };
            _context.Users.Add(_userdetails);
            _context.SaveChanges();
            return CreatedAtAction("GetAll", new Users { Id = user.Id }, user);
            }
        }

    }
}