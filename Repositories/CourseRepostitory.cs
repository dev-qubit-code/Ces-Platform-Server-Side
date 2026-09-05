using Ces_Platform_Server_Side.FIlters.QueryFilters;
using Ces_Platform_Server_Side.Interfaces;
using Ces_Platform_Server_Side.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace Ces_Platform_Server_Side.Repositories.Course
{
    public class CourseRepository(AppDbContext context) : ICourseRepository
    {
        public async Task<bool> AddCourseAsync(Models.Course course, CancellationToken ct = default)
        {
            await context.Courses.AddAsync(course);
            int RowAffrcted = await context.SaveChangesAsync(ct);
            return RowAffrcted > 0;
        }

        public async Task<bool> DeleteCourseAsync(Guid CourseId, CancellationToken ct = default)
        {
            Models.Course? course = await context.Courses.FirstOrDefaultAsync(C => C.Id == CourseId);

            if (course is null) return false;

            context.Courses.Remove(course);
            return await context.SaveChangesAsync(ct) > 0;

        }

        public Task<int> GetCountAsync(Models.Course course, CancellationToken ct = default) => context.Courses.CountAsync(ct);

        public async Task<Models.Course?> GetCourseByIdAsync(Guid Id, CancellationToken ct = default) => await context.Courses.FirstOrDefaultAsync(C => C.Id == Id,ct);

        public async Task<(int, List<Models.Course>)> GetCoursePageAsync(CourseFilter? filter, CancellationToken ct = default)
        {
            IQueryable<Models.Course> courses = context.Courses;
            List<Models.Course> Page;
            int TotalItems;
            
            if(filter is null)
            {
               Page = await context.Courses.Take(10).ToListAsync(ct); //we should to make this var is dynamic and get value from json
                TotalItems = await courses.CountAsync(ct);

                return (TotalItems, Page);
            }

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                courses = courses.Where(c => c.Name.Contains(filter.Search) );

            }
                TotalItems = await courses.CountAsync(ct);

            Page = await courses.Skip((filter.Page - 1) * filter.PageSize)
                          .Take(filter.PageSize)
                          .ToListAsync(ct);

            return (TotalItems, Page);
        }

        public async Task<bool> UpdateCourseAsync(CancellationToken ct = default) => await context.SaveChangesAsync(ct) > 0;
        
    }
}
