using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Travel.API.Data;
using Travel.API.Models;

namespace Travel.API.Controllers
{
    // secure authorization, use JWT
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class LogController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LogController(ApplicationDbContext context)
        {
            _context = context;
        }



        // Returns the last N logs (ordered by Timestamp DESC)
        [HttpGet("get/{n}")]
        public async Task<ActionResult<IEnumerable<Log>>> GetLastNLogs(int n)
        {
            var logs = await _context.Logs
                .Include(l => l.User)
                .OrderByDescending(l => l.Timestamp)
                .Take(n)
                .ToListAsync();

            return logs;
        }

        // Returns total number of logs
        [HttpGet("count")]
        public async Task<ActionResult<int>> GetLogCount()
        {
            return await _context.Logs.CountAsync();
        }

    }
}