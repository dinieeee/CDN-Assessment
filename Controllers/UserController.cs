using Assessment.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Diagnostics.Eventing.Reader;

namespace Assessment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserContext _dbContext;

        public UserController(UserContext dbContext)
        {
            _dbContext = dbContext;
        }

        //List all user's data from database
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            if (_dbContext.Users == null)
            {
                return NotFound();
            }
            return await _dbContext.Users.ToListAsync();
        }

        //Retrieve user's data from database by ID
        [HttpGet("{ID}")]
        public async Task<ActionResult<User>> GetUser(int ID)
        {
            if (_dbContext.Users == null)
            {
                return NotFound();
            }
            var user = await _dbContext.Users.FindAsync(ID);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        //Add new user data into database
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.ID }, user);
        }

        //Update existing user's data
        [HttpPut]

        public async Task<IActionResult> PutUser(int ID, User user)
        {
            if (ID != user.ID)
            {
                return BadRequest();
            }
            _dbContext.Entry(user).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserAvailable(ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        private bool UserAvailable(int ID)
        {
            return (_dbContext.Users?.Any(x => x.ID == ID)).GetValueOrDefault();
        }

        //Delete user's data from database
        [HttpDelete("{ID}")]
        public async Task<IActionResult> DeleteUser(int ID)
        {
            if(_dbContext.Users == null)
            {
                return NotFound();
            }

            var brand = await _dbContext.Users.FindAsync(ID);
            if (brand == null)
            {
                return NotFound();
            }

            _dbContext.Users.Remove(brand);

            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
