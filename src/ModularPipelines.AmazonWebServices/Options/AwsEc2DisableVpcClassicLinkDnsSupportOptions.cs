using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "disable-vpc-classic-link-dns-support")]
public record AwsEc2DisableVpcClassicLinkDnsSupportOptions : AwsOptions
{
    [CliOption("--vpc-id")]
    public string? VpcId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}