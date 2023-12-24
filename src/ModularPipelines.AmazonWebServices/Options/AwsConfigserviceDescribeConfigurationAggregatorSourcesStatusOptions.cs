using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("configservice", "describe-configuration-aggregator-sources-status")]
public record AwsConfigserviceDescribeConfigurationAggregatorSourcesStatusOptions(
[property: CommandSwitch("--configuration-aggregator-name")] string ConfigurationAggregatorName
) : AwsOptions
{
    [CommandSwitch("--update-status")]
    public string[]? UpdateStatus { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}