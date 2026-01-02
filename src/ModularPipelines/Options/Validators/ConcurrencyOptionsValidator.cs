using Microsoft.Extensions.Options;

namespace ModularPipelines.Options.Validators;

/// <summary>
/// Validates <see cref="ConcurrencyOptions"/> configuration.
/// </summary>
internal sealed class ConcurrencyOptionsValidator : IValidateOptions<ConcurrencyOptions>
{
    public ValidateOptionsResult Validate(string? name, ConcurrencyOptions options)
    {
        var failures = new List<string>();

        if (options.MaxParallelism <= 0)
        {
            failures.Add($"{nameof(ConcurrencyOptions.MaxParallelism)} must be > 0, but was {options.MaxParallelism}.");
        }

        if (options.MaxCpuIntensiveModules is <= 0)
        {
            failures.Add($"{nameof(ConcurrencyOptions.MaxCpuIntensiveModules)} must be > 0 or null, but was {options.MaxCpuIntensiveModules}.");
        }

        if (options.MaxIoIntensiveModules is <= 0)
        {
            failures.Add($"{nameof(ConcurrencyOptions.MaxIoIntensiveModules)} must be > 0 or null, but was {options.MaxIoIntensiveModules}.");
        }

        return failures.Count > 0
            ? ValidateOptionsResult.Fail(failures)
            : ValidateOptionsResult.Success;
    }
}
