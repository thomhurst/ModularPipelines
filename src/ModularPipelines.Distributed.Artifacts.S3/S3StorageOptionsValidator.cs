using Microsoft.Extensions.Options;

namespace ModularPipelines.Distributed.Artifacts.S3;

/// <summary>
/// Validates <see cref="S3StorageOptions"/> at startup.
/// </summary>
internal sealed class S3StorageOptionsValidator : IValidateOptions<S3StorageOptions>
{
    internal const int MinimumPartSizeBytes = 5 * 1024 * 1024;
    internal const int MaximumPartSizeBytes = 1024 * 1024 * 1024;

    public ValidateOptionsResult Validate(string? name, S3StorageOptions options)
    {
        var failures = new List<string>();
        ValidateEndpoint(options.ServiceUrl, failures);

        if (string.IsNullOrWhiteSpace(options.BucketName))
        {
            failures.Add($"{nameof(S3StorageOptions)}.{nameof(S3StorageOptions.BucketName)} is required.");
        }

        if (string.IsNullOrWhiteSpace(options.KeyPrefix) || string.IsNullOrWhiteSpace(options.KeyPrefix.Trim('/')))
        {
            failures.Add($"{nameof(S3StorageOptions)}.{nameof(S3StorageOptions.KeyPrefix)} must not be empty.");
        }

        if (string.IsNullOrWhiteSpace(options.Region))
        {
            failures.Add($"{nameof(S3StorageOptions)}.{nameof(S3StorageOptions.Region)} must not be empty.");
        }

        if (string.IsNullOrEmpty(options.AccessKey) != string.IsNullOrEmpty(options.SecretKey))
        {
            failures.Add(
                $"{nameof(S3StorageOptions)}.{nameof(S3StorageOptions.AccessKey)} and " +
                $"{nameof(S3StorageOptions.SecretKey)} must be configured together.");
        }

        if (options.MultipartPartSizeBytes is < MinimumPartSizeBytes or > MaximumPartSizeBytes)
        {
            failures.Add(
                $"{nameof(S3StorageOptions)}.{nameof(S3StorageOptions.MultipartPartSizeBytes)} must be between " +
                $"{MinimumPartSizeBytes} and {MaximumPartSizeBytes} bytes.");
        }

        if (options.TimeToLive <= TimeSpan.Zero)
        {
            failures.Add($"{nameof(S3StorageOptions)}.{nameof(S3StorageOptions.TimeToLive)} must be positive.");
        }

        return failures.Count == 0
            ? ValidateOptionsResult.Success
            : ValidateOptionsResult.Fail(failures);
    }

    private static void ValidateEndpoint(Uri? serviceUrl, List<string> failures)
    {
        if (serviceUrl is not null
            && (!serviceUrl.IsAbsoluteUri || serviceUrl.Scheme is not ("http" or "https")))
        {
            failures.Add($"{nameof(S3StorageOptions)}.{nameof(S3StorageOptions.ServiceUrl)} must be an absolute http or https URL.");
        }
    }
}
