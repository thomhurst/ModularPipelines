using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "update-workflow")]
public record AwsGlueUpdateWorkflowOptions(
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--default-run-properties")]
    public IEnumerable<KeyValue>? DefaultRunProperties { get; set; }

    [CommandSwitch("--max-concurrent-runs")]
    public int? MaxConcurrentRuns { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}