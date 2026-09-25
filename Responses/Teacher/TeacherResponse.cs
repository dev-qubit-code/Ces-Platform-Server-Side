using Ces_Platform_Server_Side.Models;

namespace Ces_Platform_Server_Side.Responses;
public class TeacherResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int TestsCount { get; set; }
    public int NotesCount { get; set; }

    public static TeacherResponse FromModel(Teacher teacher) => new()
    {
        Id = teacher.Id,
        Name = teacher.Name,
        // the rest added later with relations
    };

    public static IEnumerable<TeacherResponse> FromModel(IEnumerable<Teacher> teachers) => teachers.Select(FromModel);
}
