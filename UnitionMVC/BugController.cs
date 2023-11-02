using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/bugs")]
[ApiController]
public class BugController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public BugController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/bugs
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Bug>>> GetBugs()
    {
        return await _context.Bugs.ToListAsync();
    }

    // GET: api/bugs/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Bug>> GetBug(int id)
    {
        var bug = await _context.Bugs.FindAsync(id);

        if (bug == null)
        {
            return NotFound();
        }

        return bug;
    }

    // POST: api/bugs
    [HttpPost]
    public async Task<ActionResult<Bug>> CreateBug(Bug bug)
    {
        if (ModelState.IsValid)
        {
            var user = new User { Username = "exampleUser", Password = "examplePassword", Role = "QA" };
            _context.Users.Add(user);

            bug.CreatedBy = user;
            _context.Bugs.Add(bug);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBug", new { id = bug.Id }, bug);
        }

        return BadRequest(ModelState);
    }

    // PUT: api/bugs/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBug(int id, Bug bug)
    {
        if (id != bug.Id)
        {
            return BadRequest();
        }

        var existingBug = await _context.Bugs.FindAsync(id);

        if (existingBug == null)
        {
            return NotFound();
        }

        existingBug.Summary = bug.Summary;
        existingBug.Description = bug.Description;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!BugExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/bugs/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<Bug>> DeleteBug(int id)
    {
        var bug = await _context.Bugs.FindAsync(id);
        if (bug == null)
        {
            return NotFound();
        }

        _context.Bugs.Remove(bug);
        await _context.SaveChangesAsync();

        return bug;
    }

    private bool BugExists(int id)
    {
        return _context.Bugs.Any(e => e.Id == id);
    }
}