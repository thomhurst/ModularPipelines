using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("schemas", "describe-discoverer")]
public record AwsSchemasDescribeDiscovererOptions(
[property: CliOption("--discoverer-id")] string DiscovererId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}