using Ces_Platform_Server_Side.FIlters.QueryFilters;
using Ces_Platform_Server_Side.Interfaces;
using Ces_Platform_Server_Side.Models;
using Microsoft.EntityFrameworkCore;
 
namespace Ces_Platform_Server_Side.Repositories
{
    public class TestRepository(AppDbContext context) : ITestRepository
    {
        public async Task<bool> AddTestAsync(Test Test, CancellationToken ct = default)
        {
            await context.Tests.AddAsync(Test,ct);
            return await context.SaveChangesAsync(ct) > 0;
        }

        public async Task<bool> DeleteTestAsync(Guid TestId, CancellationToken ct = default)
        {
            Test? test = await context.Tests.FindAsync(TestId);

            if (test is null)
                return false;

            context.Tests.Remove(test);
            return await context.SaveChangesAsync(ct) > 0;

        }

        public async Task<Test?> GetTestByIdAsync(Guid TestId, CancellationToken ct = default)
        {
            Test? test = await context.Tests
                .Include(t => t.Teacher)
                .Include(c => c.Course)
                .FirstOrDefaultAsync(t => t.Id == TestId,ct);
            return test;
        }
        public async Task<int> GetTestsCountAsync(CancellationToken ct = default)
            => await context.Tests.CountAsync(ct);

        public async Task<(int, List<Test>)> GetTestsPageAsync(TestFilter? filter, CancellationToken ct = default)
        {
            int totalTests;
            List<Test> PageItem;
            IQueryable<Test> tests = context.Tests;
           
            if(filter is null)
            {
                totalTests = await context.Tests.CountAsync(ct);

                PageItem = await context.Tests
                    .Include(i => i.Teacher)
                    .Include(i => i.Course)
                    .Take(10)
                    .ToListAsync(ct);
                return (totalTests, PageItem);
            }

            filter.Page = Math.Max(1, filter.Page);
            filter.PageSize = Math.Clamp(filter.PageSize, 1, 100);

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                tests = tests.Where(n => n.Teacher.Name.Contains(filter.Search))
                    .Where(n => n.Course.Name.Contains(filter.Search));

                totalTests = await context.Tests.CountAsync(ct);
            }
            totalTests = await context.Tests.CountAsync(ct);

            PageItem = await tests.Include(t => t.Teacher).Include(c => c.Course).Skip((filter.Page - 1) * filter.PageSize)
                          .Take(filter.PageSize)
                          .ToListAsync(ct);
         
            return (totalTests, PageItem);

        }

        public async Task<bool> UpdateTestAsync(CancellationToken ct = default)
            => await context.SaveChangesAsync(ct) > 0 ;
    }
}
