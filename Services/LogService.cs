using LostAndFoundApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LostAndFoundApp.Services
{
    public class LogService
    {
        private readonly ApplicationDbContext _context;

        public LogService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task LogAsync(string logType, string executor, string logContent)
        {
            var log = new Logs
            {
                LogType = logType,
                Executor = executor,
                LogContent = logContent,
                CreatedAt = DateTime.Now
            };

            _context.Logs.Add(log);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Logs>> GetRecentLogsAsync(int count = 50)
        {
            return await _context.Logs
                .OrderByDescending(l => l.CreatedAt)
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<Logs>> GetLogsByTypeAsync(string logType)
        {
            return await _context.Logs
                .Where(l => l.LogType == logType || (logType == "Login" && l.LogType == "LOGIN"))
                .OrderByDescending(l => l.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Logs>> GetRecentLogsExcludingLoginLogoutAsync(int count = 10)
        {
            return await _context.Logs
                .Where(l => l.LogType.ToUpper() != "LOGIN" && l.LogType.ToUpper() != "LOGOUT")
                .OrderByDescending(l => l.CreatedAt)
                .Take(count)
                .ToListAsync();
        }
    }
}