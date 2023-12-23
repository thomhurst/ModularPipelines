using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "modify-ipam-pool")]
public record AwsEc2ModifyIpamPoolOptions(
[property: CommandSwitch("--ipam-pool-id")] string IpamPoolId
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--allocation-min-netmask-length")]
    public int? AllocationMinNetmaskLength { get; set; }

    [CommandSwitch("--allocation-max-netmask-length")]
    public int? AllocationMaxNetmaskLength { get; set; }

    [CommandSwitch("--allocation-default-netmask-length")]
    public int? AllocationDefaultNetmaskLength { get; set; }

    [CommandSwitch("--add-allocation-resource-tags")]
    public string[]? AddAllocationResourceTags { get; set; }

    [CommandSwitch("--remove-allocation-resource-tags")]
    public string[]? RemoveAllocationResourceTags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}