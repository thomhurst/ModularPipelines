using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("devicefarm", "create-test-grid-url")]
public record AwsDevicefarmCreateTestGridUrlOptions(
[property: CliOption("--project-arn")] string ProjectArn,
[property: CliOption("--expires-in-seconds")] int ExpiresInSeconds
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}