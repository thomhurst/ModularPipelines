namespace ModularPipelines.Attributes;

/// <summary>
/// Used to control modules not running in parallel with other modules.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class NotInParallelAttribute : Attribute
{
    /// <summary>
    /// Gets the constraint key.
    /// If set, only modules with the same constraint key will not be run in parallel.
    /// Other modules with a different constraint key can run in parallel still.
    /// If null or empty, then the module will not be run in parallel with any other module.
    /// </summary>
    public string[] ConstraintKeys { get; } = Array.Empty<string>();

    public NotInParallelAttribute()
    {
    }

    public NotInParallelAttribute(string constraintKey) : this([constraintKey])
    {
        ArgumentNullException.ThrowIfNull(constraintKey);
    }

    public NotInParallelAttribute(params string[] constraintKeys)
    {
        if (constraintKeys.Length != constraintKeys.Distinct().Count())
        {
            throw new ArgumentException("Duplicate constraint keys are not allowed.");
        }
        
        ConstraintKeys = constraintKeys;
    }
}