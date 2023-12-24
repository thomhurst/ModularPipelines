using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codeguruprofiler", "configure-agent")]
public record AwsCodeguruprofilerConfigureAgentOptions(
[property: CommandSwitch("--profiling-group-name")] string ProfilingGroupName
) : AwsOptions
{
    [CommandSwitch("--fleet-instance-id")]
    public string? FleetInstanceId { get; set; }

    [CommandSwitch("--metadata")]
    public IEnumerable<KeyValue>? Metadata { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}