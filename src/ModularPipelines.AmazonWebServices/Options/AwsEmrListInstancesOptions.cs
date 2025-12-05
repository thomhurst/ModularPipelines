using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("emr", "list-instances")]
public record AwsEmrListInstancesOptions(
[property: CliOption("--cluster-id")] string ClusterId
) : AwsOptions
{
    [CliOption("--instance-group-id")]
    public string? InstanceGroupId { get; set; }

    [CliOption("--instance-group-types")]
    public string[]? InstanceGroupTypes { get; set; }

    [CliOption("--instance-fleet-id")]
    public string? InstanceFleetId { get; set; }

    [CliOption("--instance-fleet-type")]
    public string? InstanceFleetType { get; set; }

    [CliOption("--instance-states")]
    public string[]? InstanceStates { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}