using Ces_Platform_Server_Side.Interfaces;
using Microsoft.EntityFrameworkCore;
using Ces_Platform_Server_Side.Models;
using Ces_Platform_Server_Side.Responses;
using Ces_Platform_Server_Side.FIlters.QueryFilters;

public class StudentInfoRepository(AppDbContext context) : IStudentInfoRepository
{
    public async Task<(int,List<StudentInfo>)> GetStudentInfosPageAsync(StudentInfoFilter? filter, CancellationToken ct = default)
    {
        IQueryable<StudentInfo> studentInfos = context.StudentInfos
        .Include(si => si.Sources)
        .Include(si => si.Skills)
        .AsSplitQuery();

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

        ApplyFilters(studentInfos, filter);

        if(!string.IsNullOrWhiteSpace(filter.Search))
            ApplySearchTerm(studentInfos, filter.Search);

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

    public async Task<int> GetStudentInfosCountAsync(CancellationToken ct = default) => await context.StudentInfos.CountAsync(ct);

    public async Task<IEnumerable<Skill>> GetOldSkills(IEnumerable<string> skills, CancellationToken ct = default)
    {
        IQueryable<Skill> skillsTable = context.Skills;
        
        var oldSkills = skillsTable.Where(s => skills.Any(name => s.Name.ToLower() == name.ToLower()));
        
        return  oldSkills.ToList();
    } 

    // skills query
    public async Task<List<SkillResponse>> GetAllSkills() => await context.Skills
        .Select(s => new SkillResponse { Id = s.Id, Name = s.Name})
        .ToListAsync();

        private static IQueryable<StudentInfo> ApplyFilters(IQueryable<StudentInfo> query,StudentInfoFilter filter)
        {
            var skillNameNormalized = filter.SkillName.Trim().ToLower();
            
            if(!string.IsNullOrWhiteSpace(filter.SkillName))
                query = query.Where(si => si.Skills.Any(s => s.Name.ToLower().Contains(skillNameNormalized)));
            return query;
        }

        private static IQueryable<StudentInfo> ApplySearchTerm(IQueryable<StudentInfo> query, string searchTerm)
        {      
            var normalized = searchTerm.Trim().ToLower();

            query = query.Where(t => 
            t.Name.ToLower().Contains(normalized) || 
            t.Major.ToLower().Contains(normalized) || 
            t.About.ToLower().Contains(normalized));

            return query;
        }

         
}
