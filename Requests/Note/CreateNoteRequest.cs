namespace Ces_Platform_Server_Side.Requests
{
    public class CreateNoteRequest
    {
        public string NoteName { set; get; } = string.Empty;
        public string CourseName { set; get; } = string.Empty;
        public string TeacherName { set; get; } = string.Empty;
        public IFormFile NoteFile{ set; get; }


    }
}
