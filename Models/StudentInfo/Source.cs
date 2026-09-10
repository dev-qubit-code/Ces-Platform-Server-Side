using Ces_Platform_Server_Side.Requests;

namespace Ces_Platform_Server_Side.Models;

public class Source : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    // fks
    public Guid StudentInfoId { get; set; }
    // navigation properties
    public StudentInfo StudentInfo { get; set; } = null!;

    public Source(string createdBy) : base(createdBy)
    {
        
    }

    public static Source Create(CreateSourceRequest request, string createdBy) => new Source(createdBy)
    {
        Name = request.Name,
        Url = request.Url,
    };

    public static Source Create(UpdateSourceRequest request, string createdBy) => new Source(createdBy)
    {
        Name = request.Name,
        Url = request.Url,
    };

    public static bool IsEqual(List<Source> sources1, List<UpdateSourceRequest> sources2)
    {
        var uniqeSources2 = sources2.DistinctBy(s => new { s.Name, s.Url});
        if(sources1.Count != uniqeSources2.Count())
            return false;

        bool isEqual = true;

        foreach (var (s,r) in sources1.OrderBy(s => s.Name).Zip(uniqeSources2.OrderBy(s=> s.Name)))
        {
            if(s.Name != r.Name || s.Url != r.Url)
            {
                isEqual = false;
                break;
            }
        }

        return isEqual;  
    }
}
