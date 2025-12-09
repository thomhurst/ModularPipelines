using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "describe-account-attributes")]
public record AwsRedshiftDescribeAccountAttributesOptions : AwsOptions
{
    [CliOption("--attribute-names")]
    public string[]? AttributeNames { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}