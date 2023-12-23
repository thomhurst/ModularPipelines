using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("emr", "add-instance-fleet")]
public record AwsEmrAddInstanceFleetOptions(
[property: CommandSwitch("--cluster-id")] string ClusterId,
[property: CommandSwitch("--instance-fleet")] string InstanceFleet
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}