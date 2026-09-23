namespace Ces_Platform_Server_Side.Requests
{
    public class CreateNoteRequest
    {
        public string NoteName { set; get; } = string.Empty;
        public Guid CourseId { set; get; }  
        public Guid TeacherId { set; get; }  
        
        //public IFormFile NoteFile{ set; get; }


    }
}
