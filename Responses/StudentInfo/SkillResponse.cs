using Ces_Platform_Server_Side.Models;

namespace Ces_Platform_Server_Side.Responses;

public class SkillResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public static SkillResponse FromModel(Skill skill) => new()
    {
        Id = skill.Id,
        Name = skill.Name,
    };

    public static IEnumerable<SkillResponse> FromModels(IEnumerable<Skill> skills) => skills.Select(FromModel);
}
