using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "describe-vpc-classic-link")]
public record AwsEc2DescribeVpcClassicLinkOptions : AwsOptions
{
    [CliOption("--filters")]
    public string[]? Filters { get; set; }

    [CliOption("--vpc-ids")]
    public string[]? VpcIds { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}