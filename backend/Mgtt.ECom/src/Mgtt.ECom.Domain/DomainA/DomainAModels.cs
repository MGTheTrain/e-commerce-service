using System.ComponentModel.DataAnnotations;

namespace Mgtt.ECom.Domain.DomainA;

/// <summary>
/// Represents domain model A information
/// </summary>
public class DomainModelA : IValidatableObject
{
    /// <summary>
    /// Default constructor
    /// </summary>
    public DomainModelA()
    {
        Id = Guid.NewGuid();
        DateTimeCreated = DateTime.UtcNow;
        DateTimeUpdated = DateTime.UtcNow;
        // More attributes
    }

    /// <summary>
    /// Sets the properties
    /// </summary>
    /// <param name="DomainModelA">The updated domain model A.</param>
    public void SetProperties(DomainModelA DomainModelA)
    {
        DateTimeUpdated = DomainModelA.DateTimeUpdated;
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
            yield return new ValidationResult($"{nameof(DomainModelA)}.{nameof(Id)} can't be empty");
        }

        if (DateTimeCreated == default(DateTime))
        {
            yield return new ValidationResult($"{nameof(DomainModelA)}.{nameof(DateTimeCreated)} can't be empty");
        }

        if (DateTimeUpdated == default(DateTime))
        {
            yield return new ValidationResult($"{nameof(DomainModelA)}.{nameof(DateTimeUpdated)} can't be empty");
        }
        yield return ValidationResult.Success;
    }

    // NOTE: Add other domain models for domain A
}