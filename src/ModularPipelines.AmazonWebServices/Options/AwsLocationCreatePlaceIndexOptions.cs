using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("location", "create-place-index")]
public record AwsLocationCreatePlaceIndexOptions(
[property: CliOption("--data-source")] string DataSource,
[property: CliOption("--index-name")] string IndexName
) : AwsOptions
{
    [CliOption("--data-source-configuration")]
    public string? DataSourceConfiguration { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--pricing-plan")]
    public string? PricingPlan { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}