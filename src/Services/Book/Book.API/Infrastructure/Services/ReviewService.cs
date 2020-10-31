
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Book.API.Models;
using LibraryCore.DataBaseContext;
using LibraryCore.TenantContext;

namespace Book.API.Infrastructure.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IDbRepository<BookReview> _dbRepo;
        private readonly ITenantContext _tenantContext;

        public ReviewService(IDbRepository<BookReview> dbRepo, ITenantContext tenantContext)
        {
            _dbRepo = dbRepo;
            _tenantContext = tenantContext;
        }
        public async Task AddReviewAsync(string bookId, AddReviewResource resource)
        {
            var review = new BookReview
            {
                BookId = bookId,
                ReviewComment = resource.ReviewComment,
                ReviewedBy = _tenantContext.User
            };
            await _dbRepo.AddAsync(review);
        }

        public async Task<IList<BookReview>> GetAllReviewsAsync(string bookId)
        {
            Expression<Func<BookReview, bool>> predicate = p => p.BookId == bookId;
            return await _dbRepo.GetAllAsync(predicate);
        }
    }
}
