using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("configservice", "delete-configuration-aggregator")]
public record AwsConfigserviceDeleteConfigurationAggregatorOptions(
[property: CliOption("--configuration-aggregator-name")] string ConfigurationAggregatorName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}