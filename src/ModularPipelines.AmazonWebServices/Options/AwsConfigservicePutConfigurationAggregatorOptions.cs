using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("configservice", "put-configuration-aggregator")]
public record AwsConfigservicePutConfigurationAggregatorOptions(
[property: CliOption("--configuration-aggregator-name")] string ConfigurationAggregatorName
) : AwsOptions
{
    [CliOption("--account-aggregation-sources")]
    public string[]? AccountAggregationSources { get; set; }

    [CliOption("--organization-aggregation-source")]
    public string? OrganizationAggregationSource { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}