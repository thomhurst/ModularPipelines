using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("security", "va", "sql", "baseline", "update")]
public record AzSecurityVaSqlBaselineUpdateOptions(
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--rule-id")] string RuleId,
[property: CliOption("--server-name")] string ServerName,
[property: CliOption("--vm-resource-id")] string VmResourceId,
[property: CliOption("--workspace-id")] string WorkspaceId
) : AzOptions
{
    [CliOption("--agent-id")]
    public string? AgentId { get; set; }

    [CliOption("--baseline")]
    public string? Baseline { get; set; }

    [CliFlag("--latest")]
    public bool? Latest { get; set; }

    [CliOption("--vm-name")]
    public string? VmName { get; set; }

    [CliOption("--vm-uuid")]
    public string? VmUuid { get; set; }
}