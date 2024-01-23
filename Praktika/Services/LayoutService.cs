using Microsoft.EntityFrameworkCore;
using Praktika.DAL;

namespace Praktika.Services
{
    public class LayoutService
    {
        private readonly AppDbContext _context;

        public LayoutService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Dictionary<string,string>> GetSettingAsync()
        {
            var settings=await _context.Settings.ToDictionaryAsync(s=>s.Key,s=>s.Value);
            return settings;
        }
    }
}
