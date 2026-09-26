using Ces_Platform_Server_Side.Models;

namespace Ces_Platform_Server_Side.Responses;

public class TeacherPageResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int TestsCount { get; set; }
    public int NotesCount { get; set; }

    public static TeacherPageResponse FromModel(Teacher teacher) => new()
    {
        Id = teacher.Id,
        Name = teacher.Name,
        TestsCount = teacher.Tests.Count,
        NotesCount = teacher.Notes.Count
        // the rest added later with relations
    };

    public static IEnumerable<TeacherPageResponse> FromModels(IEnumerable<Teacher> teachers) => teachers.Select(FromModel);
}