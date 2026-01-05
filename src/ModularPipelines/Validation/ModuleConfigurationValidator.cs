using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Modules;
using ModularPipelines.Modules.Behaviors;

namespace ModularPipelines.Validation;

/// <summary>
/// Validates module configuration including behavior implementations.
/// </summary>
internal class ModuleConfigurationValidator : IModuleConfigurationValidator
{
    /// <inheritdoc />
    public int Order => 300;

    /// <inheritdoc />
    public ValidationResult Validate(IServiceProvider services)
    {
        var modules = services.GetServices<IModule>();
        return ValidateModules(modules);
    }

    /// <inheritdoc />
    public ValidationResult ValidateModules(IEnumerable<IModule> modules)
    {
        var result = new ValidationResult();
        var moduleList = modules.ToList();

        if (moduleList.Count == 0)
        {
            result.AddError(new ValidationError(
                ValidationErrorCategory.ModuleConfiguration,
                "No modules are registered. A pipeline must have at least one module."));
            return result;
        }

        foreach (var module in moduleList)
        {
            ValidateModuleBehaviors(module, result);
        }

        ValidateModuleUniqueness(moduleList, result);

        return result;
    }

    /// <summary>
    /// Validates module behavior implementations.
    /// </summary>
    private static void ValidateModuleBehaviors(IModule module, ValidationResult result)
    {
        var moduleType = module.GetType();

        // Validate timeout configuration if module implements ITimeoutable
        if (module is ITimeoutable timeoutable)
        {
            var timeout = timeoutable.Timeout;
            if (timeout <= TimeSpan.Zero)
            {
                result.AddError(new ValidationError(
                    ValidationErrorCategory.ModuleConfiguration,
                    $"Module '{moduleType.Name}' implements ITimeoutable with an invalid timeout value: {timeout}. Timeout must be positive.",
                    moduleType));
            }
        }

        // Validate that result type is valid (not void or Task without generic type)
        var resultType = module.ResultType;
        if (resultType == typeof(void))
        {
            result.AddError(new ValidationError(
                ValidationErrorCategory.TypeCompatibility,
                $"Module '{moduleType.Name}' has an invalid result type: void. Use a nullable type or object if no result is needed.",
                moduleType));
        }
    }

    /// <summary>
    /// Validates that module types are unique (no duplicate registrations of the same type).
    /// </summary>
    private static void ValidateModuleUniqueness(List<IModule> modules, ValidationResult result)
    {
        var duplicates = modules
            .GroupBy(m => m.GetType())
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

        foreach (var duplicateType in duplicates)
        {
            result.AddError(new ValidationError(
                ValidationErrorCategory.ModuleConfiguration,
                $"Module '{duplicateType.Name}' is registered multiple times. Each module type should only be registered once.",
                duplicateType));
        }
    }
}
