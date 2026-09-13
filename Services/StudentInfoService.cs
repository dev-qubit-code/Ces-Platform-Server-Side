using Ces_Platform_Server_Side.Interfaces;
using Ces_Platform_Server_Side.Responses;
using SPMS_PROJECT.Exceptions;
using Ces_Platform_Server_Side.Models;
using Ces_Platform_Server_Side.Requests;

public class StudentInfoService(IStudentInfoRepository repository) : IStudentInfoService 
{
    public async Task<StudentInfoResponse> CreateStudentInfo(CreateStudentInfoRequest request, CancellationToken ct = default)
    {        
        var newStudentInfo =  StudentInfo.Create(request,"testName");

        newStudentInfo.Sources = request.Sources.Select(s => Source.Create(s,"testName")).ToList();

        var skills = request.Skills.Select(s => Skill.Create(s,"testName"));

        // adding the new skills from the request to the skills table 
        
        // 1- search to check what is the old skills
            var oldSkills = await repository.GetOldSkills(request.Skills,ct);

        // 2- adding the new skills to the table of skills
            var uniqueSkills = skills.ExceptBy(oldSkills.Select(s => s.Name.ToLower()),s => s.Name.ToLower());

        // adding all the skills from the request to the many to many table
            newStudentInfo.StudentInfoSkills.AddRange(uniqueSkills.Select(s => new StudentInfoSkill(){Skill = s}));
            newStudentInfo.StudentInfoSkills.AddRange(oldSkills.Select(s => new StudentInfoSkill(){SkillId = s.Id}));

        if(!await repository.AddStudentInfoAsync(newStudentInfo,ct))
            throw new InvalidOperationException("Error occured while adding the studentInfo");

        var studentInfo = await repository.GetStudentInfoByIdAsync(newStudentInfo.Id);

        return StudentInfoResponse.FromModel(studentInfo!);
    } 
    
    public async Task UpdateStudentInfo(Guid studentInfoId,UpdateStudentInfoRequest request, CancellationToken ct = default)
    {   
        var studentInfo = await repository.GetStudentInfoByIdAsync(studentInfoId);

        if(studentInfo is null)
            throw new BusinessRuleException("StudentInfo not found",StatusCodes.Status404NotFound);

        if(studentInfo.IsEqual(request))
            throw new BusinessRuleException("studentInfo already updated",StatusCodes.Status409Conflict);

        studentInfo.Assign(request,"testName");

        // update the sources list 
        studentInfo.Sources.Clear();
        studentInfo.Sources = request.Sources.DistinctBy(s => new { s.Name,s.Url }).Select(s => Source.Create(s,"testName")).ToList();
        

        var requestSkills = request.Skills.Select(s => Skill.Create(s,"testName"));

        // seperate the new skills from the duplicate ones
            var oldSkills = await repository.GetOldSkills(request.Skills,ct);
            var uniqueSkills = requestSkills.ExceptBy(oldSkills.Select(s => s.Name.ToLower()), s => s.Name.ToLower());
        // adding the new skills and reference the old ones
            studentInfo.StudentInfoSkills = oldSkills.Select(s => new StudentInfoSkill(){SkillId = s.Id}).ToList();
            studentInfo.StudentInfoSkills.AddRange(uniqueSkills.Select(s => new StudentInfoSkill(){Skill = s}));


 
        if(!await repository.UpdateStudentInfoAsync(ct))
            throw new InvalidOperationException("Error occured while updating the studentInfo");
    } 

    public async Task<PagedResult<StudentInfoPageResponse>> GetPagedStudentInfos(StudentInfoFilter? filter, CancellationToken ct = default)
    {
         
        
        (int totalCount,var studentInfos) = await repository.GetStudentInfosPageAsync(filter, ct);

        filter ??= new();

        if(studentInfos is null || !studentInfos.Any()) 
            return PagedResult<StudentInfoPageResponse>.Create(
            [],
            totalCount,
            filter.Page,
            filter.PageSize);

        var pagedResult = PagedResult<StudentInfoPageResponse>.Create(
            StudentInfoPageResponse.FromModels(studentInfos),
            totalCount,
            filter.Page,
            filter.PageSize);

        return pagedResult;
    }
    public async Task<StudentInfoResponse> GetStudentInfoById(Guid studentInfoId,CancellationToken ct)
    {
        var studentInfo = await repository.GetStudentInfoByIdAsync(studentInfoId,ct) ?? throw new BusinessRuleException("StudentInfo not found",StatusCodes.Status404NotFound); 

        return StudentInfoResponse.FromModel(studentInfo);
    } 

    public async Task DeleteStudentInfo(Guid studentInfoId, CancellationToken ct = default)
    {
        var studentInfo = await repository.GetStudentInfoByIdAsync(studentInfoId);

        if(studentInfo is null)
            throw new BusinessRuleException("StudentInfo not found",StatusCodes.Status404NotFound);
        
        if(!await repository.DeleteStudentInfoAsync(studentInfo,ct)) 
            throw new InvalidOperationException("Error occurd while deleting the studentInfo");
    }

}
