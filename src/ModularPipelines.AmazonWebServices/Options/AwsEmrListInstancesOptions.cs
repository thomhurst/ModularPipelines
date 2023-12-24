using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("emr", "list-instances")]
public record AwsEmrListInstancesOptions(
[property: CommandSwitch("--cluster-id")] string ClusterId
) : AwsOptions
{
    [CommandSwitch("--instance-group-id")]
    public string? InstanceGroupId { get; set; }

    [CommandSwitch("--instance-group-types")]
    public string[]? InstanceGroupTypes { get; set; }

    [CommandSwitch("--instance-fleet-id")]
    public string? InstanceFleetId { get; set; }

    [CommandSwitch("--instance-fleet-type")]
    public string? InstanceFleetType { get; set; }

    [CommandSwitch("--instance-states")]
    public string[]? InstanceStates { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}