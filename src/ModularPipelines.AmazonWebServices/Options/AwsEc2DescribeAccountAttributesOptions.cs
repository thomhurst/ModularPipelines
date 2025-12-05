using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "describe-account-attributes")]
public record AwsEc2DescribeAccountAttributesOptions : AwsOptions
{
    [CliOption("--attribute-names")]
    public string[]? AttributeNames { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}