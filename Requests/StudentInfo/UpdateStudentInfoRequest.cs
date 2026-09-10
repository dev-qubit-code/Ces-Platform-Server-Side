namespace Ces_Platform_Server_Side.Requests;

public class UpdateStudentInfoRequest
{
    public string Name { get; set; } = string.Empty;
    public string About { get; set; } = string.Empty;
    public string Major { get; set; } = string.Empty;
    public List<string> Skills { get; set; } = [];
    public List<UpdateSourceRequest> Sources { get; set; } = [];
}
