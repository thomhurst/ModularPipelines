using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("evidently", "create-experiment")]
public record AwsEvidentlyCreateExperimentOptions(
[property: CommandSwitch("--metric-goals")] string[] MetricGoals,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--project")] string Project,
[property: CommandSwitch("--treatments")] string[] Treatments
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--online-ab-config")]
    public string? OnlineAbConfig { get; set; }

    [CommandSwitch("--randomization-salt")]
    public string? RandomizationSalt { get; set; }

    [CommandSwitch("--sampling-rate")]
    public long? SamplingRate { get; set; }

    [CommandSwitch("--segment")]
    public string? Segment { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}