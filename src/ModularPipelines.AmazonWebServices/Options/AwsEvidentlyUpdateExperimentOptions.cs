using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("evidently", "update-experiment")]
public record AwsEvidentlyUpdateExperimentOptions(
[property: CommandSwitch("--experiment")] string Experiment,
[property: CommandSwitch("--project")] string Project
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--metric-goals")]
    public string[]? MetricGoals { get; set; }

    [CommandSwitch("--online-ab-config")]
    public string? OnlineAbConfig { get; set; }

    [CommandSwitch("--randomization-salt")]
    public string? RandomizationSalt { get; set; }

    [CommandSwitch("--sampling-rate")]
    public long? SamplingRate { get; set; }

    [CommandSwitch("--segment")]
    public string? Segment { get; set; }

    [CommandSwitch("--treatments")]
    public string[]? Treatments { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}