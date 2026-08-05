using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ModularPipelines.Engine;
using ModularPipelines.Exceptions;
using ModularPipelines.Options;

namespace ModularPipelines.Validation;

internal sealed class ModuleSelectionValidator(IOptions<PipelineOptions> options) : IPipelineValidator
{
    public int Order => 250;

    public async Task<ValidationResult> ValidateAsync(IServiceProvider services)
    {
        if (options.Value.TargetModules?.Count is not > 0
            && options.Value.SkippedModules?.Count is not > 0)
        {
            return ValidationResult.Success();
        }

        try
        {
            await services.GetRequiredService<ModuleRetriever>()
                .ValidateSelectionAsync()
                .ConfigureAwait(false);
            return ValidationResult.Success();
        }
        catch (ModuleSelectionException exception)
        {
            return ValidationResult.WithError(new ValidationError(
                ValidationErrorCategory.ModuleConfiguration,
                exception.Message));
        }
    }
}
