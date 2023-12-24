using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("configservice", "batch-get-aggregate-resource-config")]
public record AwsConfigserviceBatchGetAggregateResourceConfigOptions(
[property: CommandSwitch("--configuration-aggregator-name")] string ConfigurationAggregatorName,
[property: CommandSwitch("--resource-identifiers")] string[] ResourceIdentifiers
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}