using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("security", "va", "sql", "baseline", "list")]
public record AzSecurityVaSqlBaselineListOptions(
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--server-name")] string ServerName,
[property: CliOption("--vm-resource-id")] string VmResourceId,
[property: CliOption("--workspace-id")] string WorkspaceId
) : AzOptions
{
    [CliOption("--agent-id")]
    public string? AgentId { get; set; }

    [CliOption("--vm-name")]
    public string? VmName { get; set; }

    [CliOption("--vm-uuid")]
    public string? VmUuid { get; set; }
}