using System.Linq;
using System.Threading.Tasks;
using communication.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace communication.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValuesController : ControllerBase
    {
        private readonly DataContext _context;
        public ValuesController(DataContext context)
        {
            _context = context;

        }

        [HttpGet]
        public async Task<IActionResult> GetValues()
        {
            var result = await _context.Values.ToListAsync();
            return Ok(result);
        }

    }
}