using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opsworks", "describe-layers")]
public record AwsOpsworksDescribeLayersOptions : AwsOptions
{
    [CliOption("--stack-id")]
    public string? StackId { get; set; }

    [CliOption("--layer-ids")]
    public string[]? LayerIds { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}