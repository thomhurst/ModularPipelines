using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "modify-ipam-resource-discovery")]
public record AwsEc2ModifyIpamResourceDiscoveryOptions(
[property: CommandSwitch("--ipam-resource-discovery-id")] string IpamResourceDiscoveryId
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--add-operating-regions")]
    public string[]? AddOperatingRegions { get; set; }

    [CommandSwitch("--remove-operating-regions")]
    public string[]? RemoveOperatingRegions { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}