using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "modify-ipam")]
public record AwsEc2ModifyIpamOptions(
[property: CommandSwitch("--ipam-id")] string IpamId
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--add-operating-regions")]
    public string[]? AddOperatingRegions { get; set; }

    [CommandSwitch("--remove-operating-regions")]
    public string[]? RemoveOperatingRegions { get; set; }

    [CommandSwitch("--tier")]
    public string? Tier { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}