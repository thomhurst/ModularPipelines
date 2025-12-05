using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("s3control", "create-job")]
public record AwsS3controlCreateJobOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--operation")] string Operation,
[property: CliOption("--report")] string Report,
[property: CliOption("--priority")] int Priority,
[property: CliOption("--role-arn")] string RoleArn
) : AwsOptions
{
    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--manifest")]
    public string? Manifest { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--manifest-generator")]
    public string? ManifestGenerator { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}