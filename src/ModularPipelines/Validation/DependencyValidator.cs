using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Exceptions;
using ModularPipelines.Modules;

namespace ModularPipelines.Validation;

/// <summary>
/// Validates module dependencies including circular dependencies, missing dependencies, and self-references.
/// Delegates to <see cref="ModuleDependencyValidator"/> for the actual validation logic.
/// </summary>
internal class DependencyValidator : IDependencyValidator
{
    /// <inheritdoc />
    public int Order => 200;

    /// <inheritdoc />
    public ValidationResult Validate(IServiceProvider services)
    {
        var modules = services.GetServices<IModule>();
        return ValidateDependencies(modules);
    }

    /// <inheritdoc />
    public ValidationResult ValidateDependencies(IEnumerable<Type> moduleTypes)
        => Validate(() => ModuleDependencyValidator.Validate(moduleTypes), moduleTypes.Any());

    private static ValidationResult ValidateDependencies(IEnumerable<IModule> modules)
    {
        var moduleList = modules.ToList();
        return Validate(() => ModuleDependencyValidator.Validate(moduleList), moduleList.Count > 0);
    }

    private static ValidationResult Validate(Action validation, bool hasModules)
    {
        var result = new ValidationResult();

        if (!hasModules)
        {
            return result;
        }

        // Delegate to ModuleDependencyValidator and convert exceptions to validation errors
        try
        {
            validation();
        }
        catch (ModuleReferencingSelfException ex)
        {
            result.AddError(new ValidationError(
                ValidationErrorCategory.Dependency,
                ex.Message ?? "A module references itself.",
                sourceType: null));
        }
        catch (ModuleNotRegisteredException ex)
        {
            result.AddError(new ValidationError(
                ValidationErrorCategory.Dependency,
                ex.Message ?? "A required dependency is not registered.",
                sourceType: null));
        }
        catch (DependencyCollisionException ex)
        {
            result.AddError(new ValidationError(
                ValidationErrorCategory.Dependency,
                ex.Message ?? "Circular dependency detected.",
                sourceType: null));
        }

        return result;
    }
}
