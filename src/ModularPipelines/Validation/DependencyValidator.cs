using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Dependencies;
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
    public async Task<ValidationResult> ValidateAsync(IServiceProvider services)
    {
        var result = ValidateDependencies(services, services.GetServices<IModule>());
        if (!result.HasErrors)
        {
            return result;
        }

        var runnableModules = await services.GetRequiredService<ModuleRetriever>()
            .GetRunnableModulesForValidation()
            .ConfigureAwait(false);
        return ValidateDependencies(services, runnableModules);
    }

    /// <inheritdoc />
    public ValidationResult ValidateDependencies(IEnumerable<Type> moduleTypes)
        => Validate(() => ModuleDependencyValidator.Validate(moduleTypes), moduleTypes.Any());

    private static ValidationResult ValidateDependencies(
        IServiceProvider services,
        IEnumerable<IModule> modules)
    {
        var moduleList = modules.ToList();
        return Validate(
            () => ModuleDependencyValidator.Validate(
                moduleList,
                services.GetRequiredService<IModuleDependencyRegistry>(),
                services.GetRequiredService<IModuleMetadataRegistry>()),
            moduleList.Count > 0);
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
