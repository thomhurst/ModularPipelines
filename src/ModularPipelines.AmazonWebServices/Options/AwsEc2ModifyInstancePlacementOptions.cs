using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "modify-instance-placement")]
public record AwsEc2ModifyInstancePlacementOptions(
[property: CommandSwitch("--instance-id")] string InstanceId
) : AwsOptions
{
    [CommandSwitch("--affinity")]
    public string? Affinity { get; set; }

    [CommandSwitch("--group-name")]
    public string? GroupName { get; set; }

    [CommandSwitch("--host-id")]
    public string? HostId { get; set; }

    [CommandSwitch("--tenancy")]
    public string? Tenancy { get; set; }

    [CommandSwitch("--partition-number")]
    public int? PartitionNumber { get; set; }

    [CommandSwitch("--host-resource-group-arn")]
    public string? HostResourceGroupArn { get; set; }

    [CommandSwitch("--group-id")]
    public string? GroupId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}