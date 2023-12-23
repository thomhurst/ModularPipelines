using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("evidently", "update-launch")]
public record AwsEvidentlyUpdateLaunchOptions(
[property: CommandSwitch("--launch")] string Launch,
[property: CommandSwitch("--project")] string Project
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--groups")]
    public string[]? Groups { get; set; }

    [CommandSwitch("--metric-monitors")]
    public string[]? MetricMonitors { get; set; }

    [CommandSwitch("--randomization-salt")]
    public string? RandomizationSalt { get; set; }

    [CommandSwitch("--scheduled-splits-config")]
    public string? ScheduledSplitsConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}