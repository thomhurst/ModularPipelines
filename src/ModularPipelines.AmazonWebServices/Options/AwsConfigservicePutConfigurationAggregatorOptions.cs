using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("configservice", "put-configuration-aggregator")]
public record AwsConfigservicePutConfigurationAggregatorOptions(
[property: CommandSwitch("--configuration-aggregator-name")] string ConfigurationAggregatorName
) : AwsOptions
{
    [CommandSwitch("--account-aggregation-sources")]
    public string[]? AccountAggregationSources { get; set; }

    [CommandSwitch("--organization-aggregation-source")]
    public string? OrganizationAggregationSource { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}