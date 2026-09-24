using Ces_Platform_Server_Side.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ces_Platform_Server_Side.Responses
{
    public class NoteResponse
    {
        public Guid Id { set; get; }
        public string Name { set; get; } = string.Empty;
        public DateOnly Date { set; get; }
        public string TeacherName { set; get; } = string.Empty;
        public string CourseName { set; get; } = string.Empty;
       // public IFormFile NoteFile { set; get; }
        
        public static NoteResponse FromModel(Note note)
        {
            return new NoteResponse
            {
                Id = note.Id,
                Name = note.Name,
                Date = note.Date,
                TeacherName  = note.Teacher.Name ,
                CourseName = note.Course.Name,
                //NoteFile = note.File
            };
            
        }
        public static IEnumerable<NoteResponse> FromModels(IEnumerable<Note> notes) => notes.Select(FromModel);
    }
}
