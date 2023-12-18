using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "va", "sql", "scans", "list")]
public record AzSecurityVaSqlScansListOptions(
[property: CommandSwitch("--database-name")] string DatabaseName,
[property: CommandSwitch("--server-name")] string ServerName,
[property: CommandSwitch("--vm-resource-id")] string VmResourceId,
[property: CommandSwitch("--workspace-id")] string WorkspaceId
) : AzOptions
{
    [CommandSwitch("--agent-id")]
    public string? AgentId { get; set; }

    [CommandSwitch("--vm-name")]
    public string? VmName { get; set; }

    [CommandSwitch("--vm-uuid")]
    public string? VmUuid { get; set; }
}