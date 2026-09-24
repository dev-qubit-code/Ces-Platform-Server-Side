using Ces_Platform_Server_Side.Models;

namespace Ces_Platform_Server_Side.Responses
{ 
    public class NotePageResponse 
    {
        public Guid Id { set; get; }
        public string Name { set; get; } = string.Empty;
        public DateOnly Date { set; get; }
        public string TeacherName { set; get; } = string.Empty;
        public string CourseName { set; get; } = string.Empty;
       // public IFormFile NoteFile { set; get; }

        public static NotePageResponse FromModel(Note note)
        {
            return new NotePageResponse
            {
                Id = note.Id,
                Name = note.Name,
                Date = note.Date,
                TeacherName = note.Teacher.Name,
                CourseName = note.Course.Name,
                //NoteFile = note.File
            };

        }
        public static IEnumerable<NotePageResponse> FromModels(IEnumerable<Note> notes) => notes.Select(FromModel);
    }
}
