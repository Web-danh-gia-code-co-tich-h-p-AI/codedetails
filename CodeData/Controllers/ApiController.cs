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
            // Lấy danh sách coder từ cơ sở dữ liệu
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
            // Tìm coder theo ID
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
            // Kiểm tra tính hợp lệ của dữ liệu
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Tạo ID mới cho coder
            coder.ID = Guid.NewGuid().ToString();

            // Thêm coder vào cơ sở dữ liệu
            _context.Coders.Add(coder);
            await _context.SaveChangesAsync();

            // Trả về phản hồi thành công với dữ liệu của coder mới được tạo
            return CreatedAtAction(nameof(GetCoder), new { idCode = coder.IDCode }, coder);
        }

        // PUT: api/put-by-id/{idCode}
        [HttpPut("put-by-id/{idCode}")]
        public async Task<IActionResult> PutCoder(int idCode, Coders coder)
        {
            // Kiểm tra IDCode có khớp với ID của coder không
            if (idCode != coder.IDCode)
            {
                return BadRequest();
            }

            // Kiểm tra tính hợp lệ của dữ liệu
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Đánh dấu trạng thái của coder là đã bị thay đổi
            _context.Entry(coder).State = EntityState.Modified;

            try
            {
                // Lưu thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Kiểm tra coder có tồn tại không
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
            // Tìm coder theo ID
            var coder = await _context.Coders.FindAsync(idCode);
            if (coder == null)
            {
                return NotFound();
            }

            // Xóa coder khỏi cơ sở dữ liệu
            _context.Coders.Remove(coder);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Kiểm tra sự tồn tại của coder theo ID
        private bool CoderExists(int idCode)
        {
            return _context.Coders.Any(e => e.IDCode == idCode);
        }
    }
}
