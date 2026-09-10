using Ces_Platform_Server_Side.Requests;
namespace Ces_Platform_Server_Side.Models;

public class StudentInfo:AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string About { get; set; } = string.Empty;
    public string Major { get; set; } = string.Empty;

    // Navigation Properties
    public List<Skill> Skills { get; set; } = [];
    public List<StudentInfoSkill> StudentInfoSkills { get; set; } = [];
    public List<Source> Sources { get; set; } = [];
    public StudentInfo()
    {}
    public StudentInfo(string createdBy) : base(createdBy)
    {}

    public static StudentInfo Create(CreateStudentInfoRequest request, string createdBy) => new StudentInfo(createdBy)
    {
            Name = request.Name,
            Major = request.Major,
            About = request.About,
    };

    public void Assign(UpdateStudentInfoRequest request, string lastModifiedBy)
    {
        Name = request.Name;
        Major = request.Major;
        About = request.About;

        LastModifiedBy = lastModifiedBy;
        LastModifiedAtUtc = DateTimeOffset.Now;
    } 

    public bool IsEqual(UpdateStudentInfoRequest request)
    {
        if(
            Name == request.Name &&
            Major == request.Major &&
            About == request.About &&
            Source.IsEqual(Sources,request.Sources) &&
            Skill.IsEqual(Skills,request.Skills)
            )
            return true;

        return false;
    } 
}
