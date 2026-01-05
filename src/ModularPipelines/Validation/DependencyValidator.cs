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
        var moduleTypes = modules.Select(m => m.GetType());
        return ValidateDependencies(moduleTypes);
    }

    /// <inheritdoc />
    public ValidationResult ValidateDependencies(IEnumerable<Type> moduleTypes)
    {
        var result = new ValidationResult();
        var types = moduleTypes.ToHashSet();

        if (types.Count == 0)
        {
            return result;
        }

        // Delegate to ModuleDependencyValidator and convert exceptions to validation errors
        try
        {
            ModuleDependencyValidator.Validate(types);
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
