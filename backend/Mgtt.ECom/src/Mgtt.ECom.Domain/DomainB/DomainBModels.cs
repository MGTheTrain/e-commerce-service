using System.ComponentModel.DataAnnotations;

namespace Mgtt.ECom.Domain.DomainB;

/// <summary>
/// Represents domain model A information
/// </summary>
public class DomainModelB : IValidatableObject
{
    /// <summary>
    /// Default constructor
    /// </summary>
    public DomainModelB()
    {
        Id = Guid.NewGuid();
        DateTimeCreated = DateTime.UtcNow;
        DateTimeUpdated = DateTime.UtcNow;
        // More attributes
    }

    /// <summary>
    /// Sets the properties
    /// </summary>
    /// <param name="DomainModelB">The updated domain model A.</param>
    public void SetProperties(DomainModelB DomainModelB)
    {
        DateTimeUpdated = DomainModelB.DateTimeUpdated;
        // More attributes
    }

    public Guid Id { get; internal set; } 
    public DateTime DateTimeCreated { get; internal set; }
    public DateTime DateTimeUpdated { get; set; }
    // More attributes
    

    /// <summary>
    /// Validates properties
    /// </summary>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Id == Guid.Empty)
        {
            yield return new ValidationResult($"{nameof(DomainModelB)}.{nameof(Id)} can't be empty");
        }

        if (DateTimeCreated == default(DateTime))
        {
            yield return new ValidationResult($"{nameof(DomainModelB)}.{nameof(DateTimeCreated)} can't be empty");
        }

        if (DateTimeUpdated == default(DateTime))
        {
            yield return new ValidationResult($"{nameof(DomainModelB)}.{nameof(DateTimeUpdated)} can't be empty");
        }
        yield return ValidationResult.Success;
    }

    // NOTE: Add other domain models for domain A
}