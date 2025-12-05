using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "enable-vpc-classic-link-dns-support")]
public record AwsEc2EnableVpcClassicLinkDnsSupportOptions : AwsOptions
{
    [CliOption("--vpc-id")]
    public string? VpcId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}