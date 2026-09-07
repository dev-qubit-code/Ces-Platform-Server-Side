namespace Ces_Platform_Server_Side.Models;

public class StudentInfoSkill 
{
    public Guid Id { get; set; }
    public Guid SkillId { get; set; }
    public Skill Skill { get; set; } = null!;
    public Guid StudentInfoId { get; set; }
    public StudentInfo StudentInfo { get; set; } = null!;
}