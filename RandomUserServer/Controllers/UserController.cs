using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RandomUserServer.Models;
using System.Net.Http.Headers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RandomUserServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        // GET: api/<UserController>

        private readonly Context _context;
        public UserController(Context context) { _context = context; }


        [HttpGet]
        //public async Task<ActionResult<string>> Get()
        public async Task<ActionResult<IEnumerable<dynamic>>> Get()
        {

            var list = await _context.Countries.Select(a => new Country() { Id = a.Id, Users = a.Users }).ToListAsync();

            return list.Select(a => new
            {
                Name = a.Id,
                Users = a.Users.Select(b => new { Name = b.Id, Gender = b.Gender, Email = b.Email }).ToList()
            }).ToList();
            //return new string[] { "value1", "value2" };
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            //var item = new Country() { 
            //    Name = "DE",
            //    Users = new List<User>() { 
            //        new User(){ Name = "name", EMail = "email", Gender = "male" }
            //    }
            //};


            //_context.Countries.Add(item);
            //_context.SaveChanges();

            return "value";
        }

        // POST api/<UserController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
