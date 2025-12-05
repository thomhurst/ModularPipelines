using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("databrew", "update-profile-job")]
public record AwsDatabrewUpdateProfileJobOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--output-location")] string OutputLocation,
[property: CliOption("--role-arn")] string RoleArn
) : AwsOptions
{
    [CliOption("--configuration")]
    public string? Configuration { get; set; }

    [CliOption("--encryption-key-arn")]
    public string? EncryptionKeyArn { get; set; }

    [CliOption("--encryption-mode")]
    public string? EncryptionMode { get; set; }

    [CliOption("--log-subscription")]
    public string? LogSubscription { get; set; }

    [CliOption("--max-capacity")]
    public int? MaxCapacity { get; set; }

    [CliOption("--max-retries")]
    public int? MaxRetries { get; set; }

    [CliOption("--validation-configurations")]
    public string[]? ValidationConfigurations { get; set; }

    [CliOption("--timeout")]
    public int? Timeout { get; set; }

    [CliOption("--job-sample")]
    public string? JobSample { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}