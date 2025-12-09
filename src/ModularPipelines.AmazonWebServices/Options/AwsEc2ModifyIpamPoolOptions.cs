using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "modify-ipam-pool")]
public record AwsEc2ModifyIpamPoolOptions(
[property: CliOption("--ipam-pool-id")] string IpamPoolId
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--allocation-min-netmask-length")]
    public int? AllocationMinNetmaskLength { get; set; }

    [CliOption("--allocation-max-netmask-length")]
    public int? AllocationMaxNetmaskLength { get; set; }

    [CliOption("--allocation-default-netmask-length")]
    public int? AllocationDefaultNetmaskLength { get; set; }

    [CliOption("--add-allocation-resource-tags")]
    public string[]? AddAllocationResourceTags { get; set; }

    [CliOption("--remove-allocation-resource-tags")]
    public string[]? RemoveAllocationResourceTags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}