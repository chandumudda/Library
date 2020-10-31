
using System;
using LibraryCore.DataBaseContext;
using LibraryCore.TenantContext;
using System.Threading.Tasks;
using Book.API.Models;

namespace Book.API.Infrastructure.Services
{
    public class HistoryService : IHistoryService
    {
        private readonly IDbRepository<BookHistory> _dbRepo;
        private readonly ITenantContext _tenantContext;

        public HistoryService(IDbRepository<BookHistory> dbRepo, ITenantContext tenantContext)
        {
            _dbRepo = dbRepo;
            _tenantContext = tenantContext;
        }

        public async Task AddReviewAsync(string bookId)
        {
            var history = new BookHistory
            {
                BookId = bookId,
                ReadyBy = _tenantContext.User,
                ReadDate = DateTime.UtcNow
            };

            await _dbRepo.AddAsync(history);
        }
    }
}
