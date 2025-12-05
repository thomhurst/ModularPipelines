using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("comprehend", "start-pii-entities-detection-job")]
public record AwsComprehendStartPiiEntitiesDetectionJobOptions(
[property: CliOption("--input-data-config")] string InputDataConfig,
[property: CliOption("--output-data-config")] string OutputDataConfig,
[property: CliOption("--mode")] string Mode,
[property: CliOption("--data-access-role-arn")] string DataAccessRoleArn,
[property: CliOption("--language-code")] string LanguageCode
) : AwsOptions
{
    [CliOption("--redaction-config")]
    public string? RedactionConfig { get; set; }

    [CliOption("--job-name")]
    public string? JobName { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}