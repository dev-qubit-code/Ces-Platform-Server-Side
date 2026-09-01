using Ces_Platform_Server_Side.Interfaces;
using Microsoft.EntityFrameworkCore;
using Ces_Platform_Server_Side.Models;

public class TeacherRepository(AppDbContext context) : ITeacherRepository
{
    public async Task<(int,List<Teacher>)> GetTeachersPageAsync(TeacherFilter? filter, CancellationToken ct = default)
    {
        IQueryable<Teacher> teachers = context.Teachers;

        List<Teacher> page;
        int totalCount;

        if(filter is null)
        {
            page = await teachers.Take(10).ToListAsync(ct);

            totalCount = await teachers.CountAsync();
            return (totalCount,page);
        }

        filter.PageSize = Math.Max(1, filter.PageSize);
        filter.Page = Math.Clamp(filter.Page, 1, 100);

        if(!string.IsNullOrWhiteSpace(filter.Search))
            teachers = teachers.Where(u => u.Name.Contains(filter.Search));

        totalCount = await teachers.CountAsync(ct);

        page = await teachers.Skip((filter.Page - 1) * filter.PageSize)
                          .Take(filter.PageSize)
                          .ToListAsync(ct);
        
        return (totalCount,page);
    }

    public async Task<Teacher?> GetTeacherByIdAsync(Guid teacherId, CancellationToken ct = default)
    {
        return await context.Teachers.FirstOrDefaultAsync(u => u.Id == teacherId, ct);
    }

    public async Task<bool> AddTeacherAsync(Teacher teacher, CancellationToken ct = default)
    {
        context.Teachers.Add(teacher);
        return await context.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> UpdateTeacherAsync(CancellationToken ct = default) => await context.SaveChangesAsync(ct) > 0;

    public async Task<bool> DeleteTeacherAsync(Guid teacherId, CancellationToken ct = default)
    {
        var teacher = await context.Teachers.FirstOrDefaultAsync(u => u.Id == teacherId, ct);

        if (teacher == null)
            return false;

        context.Teachers.Remove(teacher);
        return await context.SaveChangesAsync(ct) > 0;
    }

    public async Task<int> GetTeachersCountAsync(CancellationToken ct = default) => await context.Teachers.CountAsync(ct);
}
