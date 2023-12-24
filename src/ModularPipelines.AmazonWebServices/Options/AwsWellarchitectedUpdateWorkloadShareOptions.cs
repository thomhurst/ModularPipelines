using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("wellarchitected", "update-workload-share")]
public record AwsWellarchitectedUpdateWorkloadShareOptions(
[property: CommandSwitch("--share-id")] string ShareId,
[property: CommandSwitch("--workload-id")] string WorkloadId,
[property: CommandSwitch("--permission-type")] string PermissionType
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}