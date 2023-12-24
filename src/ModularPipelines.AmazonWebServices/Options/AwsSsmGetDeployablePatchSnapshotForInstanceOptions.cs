using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm", "get-deployable-patch-snapshot-for-instance")]
public record AwsSsmGetDeployablePatchSnapshotForInstanceOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--snapshot-id")] string SnapshotId
) : AwsOptions
{
    [CommandSwitch("--baseline-override")]
    public string? BaselineOverride { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}