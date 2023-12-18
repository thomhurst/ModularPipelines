using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "spark-job-definition", "wait")]
public record AzSynapseSparkJobDefinitionWaitOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [BooleanCommandSwitch("--created")]
    public bool? Created { get; set; }

    [CommandSwitch("--custom")]
    public string? Custom { get; set; }

    [BooleanCommandSwitch("--deleted")]
    public bool? Deleted { get; set; }

    [BooleanCommandSwitch("--exists")]
    public bool? Exists { get; set; }

    [CommandSwitch("--interval")]
    public int? Interval { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }

    [BooleanCommandSwitch("--updated")]
    public bool? Updated { get; set; }
}