using Ces_Platform_Server_Side.Interfaces;
using Microsoft.EntityFrameworkCore;
using Ces_Platform_Server_Side.Models;

public class StudentInfoRepository(AppDbContext context) : IStudentInfoRepository
{
    public async Task<(int,List<StudentInfo>)> GetStudentInfosPageAsync(StudentInfoFilter? filter, CancellationToken ct = default)
    {
        IQueryable<StudentInfo> studentInfos = context.StudentInfos
        .Include(si => si.Sources)
        .Include(si => si.Skills);

        List<StudentInfo> page;
        int totalCount;

        if(filter is null)
        {
            page = await studentInfos.Take(10).ToListAsync(ct);

            totalCount = await studentInfos.CountAsync();
            return (totalCount,page);
        }

        filter.PageSize = Math.Max(1, filter.PageSize);
        filter.Page = Math.Clamp(filter.Page, 1, 100);

        if(!string.IsNullOrWhiteSpace(filter.Search))
            studentInfos = studentInfos.Where(u =>
                     u.Name.Contains(filter.Search) || 
                     u.CreatedAtUtc.ToString().Contains(filter.Search)  
                );

        totalCount = await studentInfos.CountAsync(ct);

        page = await studentInfos.Skip((filter.Page - 1) * filter.PageSize)
                          .Take(filter.PageSize)
                          .ToListAsync(ct);
        
        return (totalCount,page);
    }

    public async Task<StudentInfo?> GetStudentInfoByIdAsync(Guid studentInfoId, CancellationToken ct = default)
    {
        return await context.StudentInfos
        .Include(u => u.Skills)
        .Include(u => u.Sources)
        .FirstOrDefaultAsync(u => u.Id == studentInfoId, ct);
    }

    public async Task<bool> AddStudentInfoAsync(StudentInfo studentInfo, CancellationToken ct = default)
    {
        context.StudentInfos.Add(studentInfo);
        return await context.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> UpdateStudentInfoAsync(CancellationToken ct = default) => await context.SaveChangesAsync(ct) > 0;

    public async Task<bool> DeleteStudentInfoAsync(StudentInfo studentInfo, CancellationToken ct = default)
    {
        context.StudentInfos.Remove(studentInfo);
        return await context.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> CheckForNewSkills(StudentInfo studentInfo, CancellationToken ct = default)
    {
        context.StudentInfos.Remove(studentInfo);
        return await context.SaveChangesAsync(ct) > 0;
    }

    public async Task<int> GetStudentInfosCountAsync(CancellationToken ct = default) => await context.StudentInfos.CountAsync(ct);

    public async Task<IEnumerable<Skill>> GetOldSkills(IEnumerable<string> skills, CancellationToken ct = default)
    {
        IQueryable<Skill> skillsTable = context.Skills;
        
        var oldSkills = skillsTable.Where(s => skills.Any(name => s.Name.ToLower() == name.ToLower()));
        
        return  oldSkills.ToList();
    } 
}
