using ModularPipelines.Modules;

namespace ModularPipelines.Validation;

/// <summary>
/// Interface for validating module configuration.
/// </summary>
public interface IModuleConfigurationValidator : IPipelineValidator
{
    /// <summary>
    /// Validates the configuration of registered modules.
    /// </summary>
    /// <param name="modules">The registered modules to validate.</param>
    /// <returns>A validation result containing any errors found.</returns>
    ValidationResult ValidateModules(IEnumerable<IModule> modules);
}
