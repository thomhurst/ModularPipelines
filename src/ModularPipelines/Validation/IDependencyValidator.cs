namespace ModularPipelines.Validation;

/// <summary>
/// Interface for validating module dependencies.
/// </summary>
public interface IDependencyValidator : IPipelineValidator
{
    /// <summary>
    /// Validates module dependencies for the specified module types.
    /// </summary>
    /// <param name="moduleTypes">The types of all registered modules.</param>
    /// <returns>A validation result containing any errors found.</returns>
    ValidationResult ValidateDependencies(IEnumerable<Type> moduleTypes);
}
