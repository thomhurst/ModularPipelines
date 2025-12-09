using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "create-ipam-resource-discovery")]
public record AwsEc2CreateIpamResourceDiscoveryOptions : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--operating-regions")]
    public string[]? OperatingRegions { get; set; }

    [CliOption("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}