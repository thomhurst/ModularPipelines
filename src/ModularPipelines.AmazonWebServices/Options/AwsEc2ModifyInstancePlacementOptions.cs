using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "modify-instance-placement")]
public record AwsEc2ModifyInstancePlacementOptions(
[property: CliOption("--instance-id")] string InstanceId
) : AwsOptions
{
    [CliOption("--affinity")]
    public string? Affinity { get; set; }

    [CliOption("--group-name")]
    public string? GroupName { get; set; }

    [CliOption("--host-id")]
    public string? HostId { get; set; }

    [CliOption("--tenancy")]
    public string? Tenancy { get; set; }

    [CliOption("--partition-number")]
    public int? PartitionNumber { get; set; }

    [CliOption("--host-resource-group-arn")]
    public string? HostResourceGroupArn { get; set; }

    [CliOption("--group-id")]
    public string? GroupId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}