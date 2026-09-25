namespace Ces_Platform_Server_Side.Requests
{
    public class CreateNoteRequest
    {
        public string Name { set; get; } = string.Empty;
        public Guid CourseId { set; get; }  
        public Guid TeacherId { set; get; }
        public DateOnly Date { get; set; }  
        
        //public IFormFile NoteFile{ set; get; }


    }
}
