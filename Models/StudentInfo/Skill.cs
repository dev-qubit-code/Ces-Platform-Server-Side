namespace Ces_Platform_Server_Side.Models;

public class Skill : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    // navigation properties
    public List<StudentInfo> StudentInfos { get; set; } = [];

    public Skill(string createdBy) : base(createdBy)
    {
        
    }

    public static Skill Create(string name, string createdBy) => new Skill(createdBy)
    {
        Name = name,
    };

      public static bool IsEqual(List<Skill> skills1, List<string> skills2)
    {
        var uniqueSkills2 = skills2.DistinctBy(name => name);
        if(skills1.Count != uniqueSkills2.Count())
            return false;

        bool isEqual = true;

        foreach (var (s,name) in skills1.OrderBy(s =>s.Name).Zip(uniqueSkills2.Order()))
        {
            if(!s.Name.Equals(name,StringComparison.OrdinalIgnoreCase))
            {
                isEqual = false;
                break;
            }
        }

        return isEqual;  
    }

}