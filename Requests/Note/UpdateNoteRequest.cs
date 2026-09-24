using System.Data.SqlTypes;

namespace Ces_Platform_Server_Side.Requests
{
    public class UpdateNoteRequest
    {
        public string Name { set; get; } = string.Empty;
        public DateOnly Date { get; set; }
        public Guid CourseId{ set; get; } 
        public Guid TeacherId { set; get; }

        //public  IFormFile File { set; get; }
    }
}
