using Microsoft.Extensions.Options;

namespace ModularPipelines.Options.Validators;

/// <summary>
/// Validates <see cref="HttpResilienceOptions"/> configuration.
/// </summary>
internal sealed class HttpResilienceOptionsValidator : IValidateOptions<HttpResilienceOptions>
{
    public ValidateOptionsResult Validate(string? name, HttpResilienceOptions options)
    {
        var failures = new List<string>();

        if (options.MaxRetryAttempts < 0)
        {
            failures.Add($"{nameof(HttpResilienceOptions.MaxRetryAttempts)} must be >= 0, but was {options.MaxRetryAttempts}.");
        }

        if (options.InitialDelay < TimeSpan.Zero)
        {
            failures.Add($"{nameof(HttpResilienceOptions.InitialDelay)} must be >= 0, but was {options.InitialDelay}.");
        }

        if (options.MaxDelay < TimeSpan.Zero)
        {
            failures.Add($"{nameof(HttpResilienceOptions.MaxDelay)} must be >= 0, but was {options.MaxDelay}.");
        }

        if (options.InitialDelay > options.MaxDelay)
        {
            failures.Add($"{nameof(HttpResilienceOptions.InitialDelay)} ({options.InitialDelay}) must be <= {nameof(HttpResilienceOptions.MaxDelay)} ({options.MaxDelay}).");
        }

        if (options.JitterFactor < 0 || options.JitterFactor > 1)
        {
            failures.Add($"{nameof(HttpResilienceOptions.JitterFactor)} must be between 0 and 1, but was {options.JitterFactor}.");
        }

        return failures.Count > 0
            ? ValidateOptionsResult.Fail(failures)
            : ValidateOptionsResult.Success;
    }
}
