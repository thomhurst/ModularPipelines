using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "modify-ipam")]
public record AwsEc2ModifyIpamOptions(
[property: CliOption("--ipam-id")] string IpamId
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--add-operating-regions")]
    public string[]? AddOperatingRegions { get; set; }

    [CliOption("--remove-operating-regions")]
    public string[]? RemoveOperatingRegions { get; set; }

    [CliOption("--tier")]
    public string? Tier { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}