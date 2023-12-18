using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "kusto", "database-principal-assignment", "create")]
public record AzSynapseKustoDatabasePrincipalAssignmentCreateOptions(
[property: CommandSwitch("--database-name")] string DatabaseName,
[property: CommandSwitch("--kusto-pool-name")] string KustoPoolName,
[property: CommandSwitch("--principal-assignment-name")] string PrincipalAssignmentName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--principal-id")]
    public string? PrincipalId { get; set; }

    [CommandSwitch("--principal-type")]
    public string? PrincipalType { get; set; }

    [CommandSwitch("--role")]
    public string? Role { get; set; }

    [CommandSwitch("--tenant-id")]
    public string? TenantId { get; set; }
}