using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("es", "describe-elasticsearch-instance-type-limits")]
public record AwsEsDescribeElasticsearchInstanceTypeLimitsOptions(
[property: CliOption("--instance-type")] string InstanceType,
[property: CliOption("--elasticsearch-version")] string ElasticsearchVersion
) : AwsOptions
{
    [CliOption("--domain-name")]
    public string? DomainName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}