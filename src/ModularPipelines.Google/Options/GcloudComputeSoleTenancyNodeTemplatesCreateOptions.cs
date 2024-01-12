using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "sole-tenancy", "node-templates", "create")]
public record GcloudComputeSoleTenancyNodeTemplatesCreateOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--node-requirements")] string[] NodeRequirements,
[property: BooleanCommandSwitch("vCPU")] bool Vcpu,
[property: BooleanCommandSwitch("memory")] bool Memory,
[property: BooleanCommandSwitch("localSSD")] bool LocalSSD,
[property: CommandSwitch("--node-type")] string NodeType
) : GcloudOptions
{
    [CommandSwitch("--accelerator")]
    public string[]? Accelerator { get; set; }

    [CommandSwitch("--cpu-overcommit-type")]
    public string? CpuOvercommitType { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--disk")]
    public string[]? Disk { get; set; }

    [CommandSwitch("--node-affinity-labels")]
    public IEnumerable<KeyValue>? NodeAffinityLabels { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--server-binding")]
    public string? ServerBinding { get; set; }
}