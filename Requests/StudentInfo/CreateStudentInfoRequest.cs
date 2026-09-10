namespace Ces_Platform_Server_Side.Requests;

public class CreateStudentInfoRequest
{
    public string Name { get; set; } = string.Empty;
    public string About { get; set; } = string.Empty;
    public string Major { get; set; } = string.Empty;
    public List<string> Skills { get; set; } = [];
    public List<CreateSourceRequest> Sources { get; set; } = [];
}
