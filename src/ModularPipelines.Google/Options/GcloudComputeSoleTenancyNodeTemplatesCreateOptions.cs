using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "sole-tenancy", "node-templates", "create")]
public record GcloudComputeSoleTenancyNodeTemplatesCreateOptions(
[property: CliArgument] string Name,
[property: CliOption("--node-requirements")] string[] NodeRequirements,
[property: CliFlag("vCPU")] bool Vcpu,
[property: CliFlag("memory")] bool Memory,
[property: CliFlag("localSSD")] bool LocalSSD,
[property: CliOption("--node-type")] string NodeType
) : GcloudOptions
{
    [CliOption("--accelerator")]
    public string[]? Accelerator { get; set; }

    [CliOption("--cpu-overcommit-type")]
    public string? CpuOvercommitType { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--disk")]
    public string[]? Disk { get; set; }

    [CliOption("--node-affinity-labels")]
    public IEnumerable<KeyValue>? NodeAffinityLabels { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--server-binding")]
    public string? ServerBinding { get; set; }
}