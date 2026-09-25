using Ces_Platform_Server_Side.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ces_Platform_Server_Side.Responses
{
    public class NoteResponse
    {
        public Guid Id { set; get; }
        public string Name { set; get; } = string.Empty;
        public DateOnly Date { set; get; }
        public Guid TeacherId { set; get; }
        public Guid CourseId { set; get; }
       // public IFormFile NoteFile { set; get; }
        
        public static NoteResponse FromModel(Note note)
        {
            return new NoteResponse
            {
                Id = note.Id,
                Name = note.Name,
                Date = note.Date,
                TeacherId  = note.Teacher.Id ,
                CourseId = note.Course.Id,
                //NoteFile = note.File
            };
            
        }
        public static IEnumerable<NoteResponse> FromModels(IEnumerable<Note> notes) => notes.Select(FromModel);
    }
}
