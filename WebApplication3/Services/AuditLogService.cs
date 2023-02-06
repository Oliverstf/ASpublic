using WebApplication3.Model;

namespace WebApplication3.Services
{
    public interface IAuditLogService
    {
        Task LogAsync(AuditLog log);
    }

    public class AuditLogService : IAuditLogService
    {
        private readonly AuthDbContext _dbContext;

        public AuditLogService(AuthDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task LogAsync(AuditLog log)
        {
            _dbContext.AuditLog.Add(log);
            await _dbContext.SaveChangesAsync();
        }
    }
}
