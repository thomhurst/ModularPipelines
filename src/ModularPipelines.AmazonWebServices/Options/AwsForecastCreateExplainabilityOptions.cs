using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("forecast", "create-explainability")]
public record AwsForecastCreateExplainabilityOptions(
[property: CommandSwitch("--explainability-name")] string ExplainabilityName,
[property: CommandSwitch("--resource-arn")] string ResourceArn,
[property: CommandSwitch("--explainability-config")] string ExplainabilityConfig
) : AwsOptions
{
    [CommandSwitch("--data-source")]
    public string? DataSource { get; set; }

    [CommandSwitch("--schema")]
    public string? Schema { get; set; }

    [CommandSwitch("--start-date-time")]
    public string? StartDateTime { get; set; }

    [CommandSwitch("--end-date-time")]
    public string? EndDateTime { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}