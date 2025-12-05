using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opsworks", "set-load-based-auto-scaling")]
public record AwsOpsworksSetLoadBasedAutoScalingOptions(
[property: CliOption("--layer-id")] string LayerId
) : AwsOptions
{
    [CliOption("--up-scaling")]
    public string? UpScaling { get; set; }

    [CliOption("--down-scaling")]
    public string? DownScaling { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}