using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEB_API.models;

namespace WEB_API.Controllers
{
    [Authorize]
    
    [Route("api/[controller]")]
    [ApiController] 
    public class StartupController : ControllerBase
    {
        private readonly StartupDbContext _context;
        public StartupController(StartupDbContext context) => _context = context;
        //GET /api/startup
        [HttpGet]
        public ActionResult<IEnumerable<Startup>> Start()
        {
            var result = _context.StartupItems;
            return Ok(result);
            // return new string[] { "one", "two", "three", "four" };
        }
        //GET /api/startup/1
        [HttpGet("{id}")]
        public ActionResult<Startup> StartFindOne(int id)
        {
            var getUser = _context.StartupItems.Find(id);
            if (getUser == null)
            {
                return NotFound();
            }

            return Ok(getUser);

        }
        //POST  /api/startup/
        [HttpPost]
        public ActionResult<Startup_db> AddUser(Startup_db startup)
        {
            _context.StartupItems.Add(startup);
            _context.SaveChanges();
             return CreatedAtAction("Start", new Startup_db{Id = startup.Id}, startup);
        }
                //PUT:      api/startup/n
        [HttpPut("{id}")]
        public ActionResult EditItem(int id, Startup_db startup)
        {
            if (id != startup.Id)
            {
                return BadRequest();
            }

            _context.Entry(startup).State = EntityState.Modified;
            _context.SaveChanges();

            return NoContent();
        }

        //DELETE:   api/startups/n
        [HttpDelete("{id}")]
        public ActionResult<Startup_db> DeleteStartItem(int id)
        {
            var startItems = _context.StartupItems.Find(id);

            if (startItems == null)
            {
                return NotFound();
            }

            _context.StartupItems.Remove(startItems);
            _context.SaveChanges();

            return startItems;
        }
    }
}