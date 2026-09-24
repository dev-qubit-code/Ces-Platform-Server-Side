using Ces_Platform_Server_Side.Models;

namespace Ces_Platform_Server_Side.Responses;

public class SourceResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public static SourceResponse FromModel(Source source) => new()
    {
        Id = source.Id,
        Name = source.Name,
        Url = source.Url
    };

    public static IEnumerable<SourceResponse> FromModels(IEnumerable<Source> sources) => sources.Select(FromModel);
}
