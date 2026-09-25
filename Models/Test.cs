using Ces_Platform_Server_Side.Enums;
using Ces_Platform_Server_Side.Requests;

namespace Ces_Platform_Server_Side.Models;

public class Test:AuditableEntity
{

    public TestStatus Status { get; set; }
    //public IFormFile? File { get; set; }
    public DateOnly Date { get; set; }
    public TestKind Kind { get; set; }
    //fks
    public Guid TeacherId { get; set; }
    public Guid CourseId { get; set; }
    // Navigation
    public Teacher Teacher { get; set; } = null!;
    public Course Course { get; set; } = null!;
   
    private Test() { }

    public Test( DateOnly date, TestKind kind, Guid teacherId, Guid courseId, string CreatedBy):base(CreatedBy)
    {
        Date = date;
        Kind = kind;
        TeacherId = teacherId;
        CourseId = courseId;
        //File = file;
    }
    public static Test  Create(CreateTestRequest request,string CreatedBy)
    {
        return new Test(request.TestDate,request.kind,request.TeacherId,request.CourseId,CreatedBy);
    }

    public  void Assign(UpdateTestRequest request,string lastModifiedBy)
    {
        LastModifiedBy = lastModifiedBy;
        LastModifiedAtUtc = DateTime.UtcNow;
        TeacherId = request.TeacherId == default(Guid) ? TeacherId:request.TeacherId;
        CourseId = request.CourseId == default(Guid) ? CourseId : request.CourseId;
        Date = request.TestDate;
        Status = request.Status;
        Kind = request.Kind;
    }

    public bool IsEqual(UpdateTestRequest request)
    {
        return TeacherId == request.TeacherId 
            && CourseId == request.CourseId 
            && Date == request.TestDate 
            && Status == request.Status 
            && Kind == request.Kind;
    }

}
