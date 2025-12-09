using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("configservice", "batch-get-aggregate-resource-config")]
public record AwsConfigserviceBatchGetAggregateResourceConfigOptions(
[property: CliOption("--configuration-aggregator-name")] string ConfigurationAggregatorName,
[property: CliOption("--resource-identifiers")] string[] ResourceIdentifiers
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}