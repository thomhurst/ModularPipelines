using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudsearch", "describe-domains")]
public record AwsCloudsearchDescribeDomainsOptions : AwsOptions
{
    [CliOption("--domain-names")]
    public string[]? DomainNames { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}