using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opsworks", "set-load-based-auto-scaling")]
public record AwsOpsworksSetLoadBasedAutoScalingOptions(
[property: CommandSwitch("--layer-id")] string LayerId
) : AwsOptions
{
    [CommandSwitch("--up-scaling")]
    public string? UpScaling { get; set; }

    [CommandSwitch("--down-scaling")]
    public string? DownScaling { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}