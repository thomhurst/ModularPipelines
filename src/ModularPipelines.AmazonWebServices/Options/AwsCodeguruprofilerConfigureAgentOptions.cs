using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codeguruprofiler", "configure-agent")]
public record AwsCodeguruprofilerConfigureAgentOptions(
[property: CliOption("--profiling-group-name")] string ProfilingGroupName
) : AwsOptions
{
    [CliOption("--fleet-instance-id")]
    public string? FleetInstanceId { get; set; }

    [CliOption("--metadata")]
    public IEnumerable<KeyValue>? Metadata { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}