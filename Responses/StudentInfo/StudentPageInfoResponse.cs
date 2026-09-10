using Ces_Platform_Server_Side.Models;

namespace Ces_Platform_Server_Side.Responses;

public class StudentInfoPageResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string About { get; set; } = string.Empty;
    public string Major { get; set; } = string.Empty;
    public List<SkillResponse> Skills { get; set; } = [];
    public List<SourceResponse> Sources { get; set; } = [];

    public static StudentInfoPageResponse FromModel(StudentInfo studentInfo) => new()
    {
        Id = studentInfo.Id,
        Name = studentInfo.Name,
        About = studentInfo.About,
        Major = studentInfo.Major,
        Skills = SkillResponse.FromModels(studentInfo.Skills).ToList(),
        Sources = SourceResponse.FromModels(studentInfo.Sources).ToList()
        
    };

    public static IEnumerable<StudentInfoPageResponse> FromModels(IEnumerable<StudentInfo> studentInfos) => studentInfos.Select(FromModel);
}
