using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("configservice", "describe-configuration-aggregator-sources-status")]
public record AwsConfigserviceDescribeConfigurationAggregatorSourcesStatusOptions(
[property: CliOption("--configuration-aggregator-name")] string ConfigurationAggregatorName
) : AwsOptions
{
    [CliOption("--update-status")]
    public string[]? UpdateStatus { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}