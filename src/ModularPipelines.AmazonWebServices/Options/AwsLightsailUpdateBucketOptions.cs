using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "update-bucket")]
public record AwsLightsailUpdateBucketOptions(
[property: CliOption("--bucket-name")] string BucketName
) : AwsOptions
{
    [CliOption("--access-rules")]
    public string? AccessRules { get; set; }

    [CliOption("--versioning")]
    public string? Versioning { get; set; }

    [CliOption("--readonly-access-accounts")]
    public string[]? ReadonlyAccessAccounts { get; set; }

    [CliOption("--access-log-config")]
    public string? AccessLogConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}