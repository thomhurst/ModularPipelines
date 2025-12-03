using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "create-ipam")]
public record AwsEc2CreateIpamOptions : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--operating-regions")]
    public string[]? OperatingRegions { get; set; }

    [CliOption("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--tier")]
    public string? Tier { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}