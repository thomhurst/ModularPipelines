using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opsworks", "describe-apps")]
public record AwsOpsworksDescribeAppsOptions : AwsOptions
{
    [CliOption("--stack-id")]
    public string? StackId { get; set; }

    [CliOption("--app-ids")]
    public string[]? AppIds { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}