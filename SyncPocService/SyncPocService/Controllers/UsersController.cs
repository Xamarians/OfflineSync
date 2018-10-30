using Microsoft.AspNetCore.Mvc;
using SyncPocService.Models;
using System;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SyncPocService.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        AppDbContext _dbContext;
        public UsersController(AppDbContext context)
        {
            _dbContext = context;
        }

        // GET: api/<controller>
        [HttpGet]
        public IActionResult Get()
        {
            var users = _dbContext.Users.OrderByDescending(x => x.UpdatedAt).Take(50).ToArray();
            return Ok(new ApiResponse<User[]>(users));
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var dbUser = _dbContext.Users.Find(id);
            if (dbUser == null)
            {
                return new JsonResult(new ApiResponse("User doesn't exist or has been deleted.")) { StatusCode = 400 };
            }
            return Ok(new ApiResponse<User>(dbUser));
        }

        // POST api/<controller>
        [HttpPost]
        public IActionResult Post([FromBody]UserModel model)
        {
            if (model == null)
            {
                return new JsonResult(new ApiResponse("Model should not be null.")) { StatusCode = 400 };
            }
            if (!ModelState.IsValid)
            {
                return new JsonResult(new ApiResponse(ModelState.Values
                    .SelectMany(e => e.Errors.Select(x => x.ErrorMessage)).FirstOrDefault()))
                {
                    StatusCode = 400
                };
            }
            if (_dbContext.Users.Any(e => e.Email == model.Email))
            {
                return new JsonResult(new ApiResponse("Email already exist.")) { StatusCode = 400 };
            }
            var user = new User()
            {
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Address = model.Address,
                Phone = model.Phone,
            };
            _dbContext.Add(user);
            _dbContext.SaveChanges();
            return Ok(new ApiResponse<int>(user.Id));
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]UserModel model)
        {
            if (model == null)
            {
                return new JsonResult(new ApiResponse("Model should not be null.")) { StatusCode = 400 };
            }
            if (!ModelState.IsValid)
            {
                return new JsonResult(new ApiResponse(ModelState.Values
                    .SelectMany(e => e.Errors.Select(x => x.ErrorMessage)).FirstOrDefault()))
                {
                    StatusCode = 400
                };
            }
            var dbUser = _dbContext.Users.Find(id);
            if (dbUser == null)
            {
                return new JsonResult(new ApiResponse("User doesn't exist or has been deleted.")) { StatusCode = 400 };
            }
            if (_dbContext.Users.Any(e => e.Email == model.Email && e.Id != id))
            {
                return new JsonResult(new ApiResponse("Email already exist.")) { StatusCode = 400 };
            }
            dbUser.UpdatedAt = DateTime.UtcNow;
            dbUser.FirstName = model.FirstName;
            dbUser.LastName = model.LastName;
            dbUser.Email = model.Email;
            dbUser.Address = model.Address;
            dbUser.Phone = model.Phone;
            _dbContext.SaveChanges();
            return Ok(new ApiResponse<int>(id));
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var dbUser = _dbContext.Users.Find(id);
            if (dbUser != null)
            {
                _dbContext.Remove(dbUser);
                _dbContext.SaveChanges();
            }
            return Ok(new ApiResponse<int>(id));
        }

    }

}
