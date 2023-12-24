using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opsworks", "assign-instance")]
public record AwsOpsworksAssignInstanceOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--layer-ids")] string[] LayerIds
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}