using Microsoft.AspNetCore.Mvc;
using CodeData.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeData.Controllers
{
    [Route("api/")]
    [ApiController]
    public class CodersController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public CodersController(ApiDbContext context)
        {
            _context = context;
        }

        
        // GET: api/get-coder-list
        [HttpGet("get-coder-list")]
        public async Task<ActionResult<IEnumerable<Coders>>> GetCoders()
        {
            var coders = await _context.Coders.ToListAsync();

            // Kiểm tra giá trị null trước khi trả về
            if (coders == null)
            {
                return NotFound();
            }

            return coders;
        }


        // GET: api/get-coder-list-by-id/{idCode}
        [HttpGet("get-coder-list-by-id/{idCode}")]
        public async Task<ActionResult<Coders>> GetCoder(int idCode)
        {
            var coder = await _context.Coders.FindAsync(idCode);

            if (coder == null)
            {
                return NotFound();
            }

            return coder;
        }

        // POST: api/add-new-coder
        [HttpPost("add-new-coder")]
        public async Task<ActionResult<Coders>> PostCoder(Coders coder)
        {
            // Generate a new ID
            coder.ID = Guid.NewGuid().ToString();

            _context.Coders.Add(coder);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCoder), new { idCode = coder.IDCode }, coder);
        }

        // PUT: api/put-by-id/{idCode}
        [HttpPut("put-by-id/{idCode}")]
        public async Task<IActionResult> PutCoder(int idCode, Coders coder)
        {
            if (idCode != coder.IDCode)
            {
                return BadRequest();
            }

            _context.Entry(coder).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CoderExists(idCode))
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

        // DELETE: api/delete-by-id/{idCode}
        [HttpDelete("delete-by-id/{idCode}")]
        public async Task<IActionResult> DeleteCoder(int idCode)
        {
            var coder = await _context.Coders.FindAsync(idCode);
            if (coder == null)
            {
                return NotFound();
            }

            _context.Coders.Remove(coder);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CoderExists(int idCode)
        {
            return _context.Coders.Any(e => e.IDCode == idCode);
        }
    }
}
