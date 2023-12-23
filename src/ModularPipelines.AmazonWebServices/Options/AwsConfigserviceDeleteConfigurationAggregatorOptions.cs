using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("configservice", "delete-configuration-aggregator")]
public record AwsConfigserviceDeleteConfigurationAggregatorOptions(
[property: CommandSwitch("--configuration-aggregator-name")] string ConfigurationAggregatorName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}