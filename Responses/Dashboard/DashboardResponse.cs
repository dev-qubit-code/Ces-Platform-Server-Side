namespace Ces_Platform_Server_Side.Responses;
public class DashboardResponse
{
   public int UsersCount { get; set; }
   public int TeachersCount { get; set; }
   public int ApprovedTestsCount { get; set; }
   public int PendingTestsCount { get; set; }
   public int NotesCount { get; set; }
   public int StudentInfoCount { get; set; }
   public List<DashboardLogResponse> DashboardLogs  { get; set; } = [];
}
