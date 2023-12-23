using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("evidently", "create-launch")]
public record AwsEvidentlyCreateLaunchOptions(
[property: CommandSwitch("--groups")] string[] Groups,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--project")] string Project
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--metric-monitors")]
    public string[]? MetricMonitors { get; set; }

    [CommandSwitch("--randomization-salt")]
    public string? RandomizationSalt { get; set; }

    [CommandSwitch("--scheduled-splits-config")]
    public string? ScheduledSplitsConfig { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}