using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "spark", "job", "show")]
public record AzSynapseSparkJobShowOptions(
[property: CommandSwitch("--livy-id")] string LivyId,
[property: CommandSwitch("--spark-pool-name")] string SparkPoolName,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CommandSwitch("--archives")]
    public string? Archives { get; set; }

    [CommandSwitch("--arguments")]
    public string? Arguments { get; set; }

    [CommandSwitch("--configuration")]
    public string? Configuration { get; set; }

    [CommandSwitch("--language")]
    public string? Language { get; set; }

    [CommandSwitch("--main-class-name")]
    public string? MainClassName { get; set; }

    [CommandSwitch("--reference-files")]
    public string? ReferenceFiles { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}