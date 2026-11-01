using Ces_Platform_Server_Side.Enums;
using Ces_Platform_Server_Side.FIlters.QueryFilters;
using Ces_Platform_Server_Side.Interfaces;
using Ces_Platform_Server_Side.Models;
using Ces_Platform_Server_Side.Responses;
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

        public async Task<bool> DeleteTestAsync(Test test, CancellationToken ct = default)
        {
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

        public async Task<(int, List<TestPageResponse>)> GetTestsPageAsync(TestFilter? filter, CancellationToken ct = default)
        {
            int totalTests;
            List<TestPageResponse> PageItem;
            IQueryable<Test> tests = context.Tests;
           
            if(filter is null)
            {
                totalTests = await context.Tests.CountAsync(ct);

                PageItem = TestPageResponse.FromModels(await tests.Take(10).ToListAsync(ct)).ToList();
                return (totalTests, PageItem);
            }

            filter.Page = Math.Max(1, filter.Page);
            filter.PageSize = Math.Clamp(filter.PageSize, 1, 100);

            tests = ApplyFilters(tests, filter);

            if (!string.IsNullOrWhiteSpace(filter.Search))
                tests = ApplySearchTerm(tests, filter.Search);

            tests = ApplySorting(tests, filter.SortColumn, filter.SortDirection);
                
            totalTests = await tests.CountAsync(ct);

            PageItem = await tests.Skip((filter.Page - 1) * filter.PageSize)
                          .Take(filter.PageSize)
                          .Select(t => new TestPageResponse
                          { 
                            Id = t.Id,
                            Status = t.Status,
                            TeacherId = t.TeacherId,
                            CourseId = t.CourseId,
                            CourseName = t.Course.Name,
                            TeacherName = t.Teacher.Name,
                            Date = t.Date,
                            Kind = t.Kind,
                          })
                          .ToListAsync(ct);
         
            return (totalTests, PageItem);

        }

        public async Task<bool> UpdateTestAsync(CancellationToken ct = default)
            => await context.SaveChangesAsync(ct) > 0 ;

        private static IQueryable<Test> ApplyFilters(IQueryable<Test> query,TestFilter filter)
        {
            var thisYear = DateTimeOffset.UtcNow.Year;

            if(filter.TestKind != TestKind.None)
                query = query.Where(t => t.Kind == filter.TestKind);

            if(filter.Status != TestStatus.None)
                query = query.Where(t => t.Status == filter.Status);

            if(filter.Year != thisYear)
                query = query.Where(t => t.Date.Year == filter.Year);
            else
                query = query.Where(t => t.Date.Year == thisYear);

            return query;
        }

        private static IQueryable<Test> ApplySearchTerm(IQueryable<Test> query, string searchTerm)
        {      
            var normalized = searchTerm.Trim().ToLower();

            query = query.Where(t => t.Course.Name.ToLower().Contains(normalized));

            return query;
        }

        private static IQueryable<Test> ApplySorting(IQueryable<Test> query, string sortColumn, string sortDirection)
        {
            var isDescending = sortDirection.Equals("desc", StringComparison.CurrentCultureIgnoreCase);

            query = sortColumn.ToLower() switch
            {
                "createdatutc" => isDescending ? query.OrderByDescending(t => t.CreatedAtUtc) : query.OrderBy(t => t.CreatedAtUtc),
                "teachername" => isDescending ? query.OrderByDescending(t => t.Teacher.Name) : query.OrderBy(t => t.Teacher.Name),
                "coursename" => isDescending ? query.OrderByDescending(t => t.Course.Name) : query.OrderBy(t => t.Course.Name),
                _ => query.OrderByDescending(t => t.CreatedAtUtc)
            };

            return query;
        }
    }
}
