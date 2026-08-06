namespace StudentsAssociation.Models;

public abstract class AuditableEntity : Entity
{
    public DateTimeOffset CreatedAtUtc { get;}

    public string CreatedBy { get; } = string.Empty;

    public DateTimeOffset LastModifiedAtUtc { get; set; }

    public string LastModifiedBy { get; set; } = string.Empty;
    protected AuditableEntity()
    { }

    protected AuditableEntity(string createdBy)
    {
        DateTimeOffset createdAtUtc = DateTimeOffset.UtcNow;
        
        CreatedAtUtc = createdAtUtc;
        CreatedBy = createdBy;
        LastModifiedAtUtc = createdAtUtc;
        LastModifiedBy = createdBy;
    }

}