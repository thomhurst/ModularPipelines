using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("synapse", "kusto", "database-principal-assignment", "create")]
public record AzSynapseKustoDatabasePrincipalAssignmentCreateOptions(
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--kusto-pool-name")] string KustoPoolName,
[property: CliOption("--principal-assignment-name")] string PrincipalAssignmentName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--principal-id")]
    public string? PrincipalId { get; set; }

    [CliOption("--principal-type")]
    public string? PrincipalType { get; set; }

    [CliOption("--role")]
    public string? Role { get; set; }

    [CliOption("--tenant-id")]
    public string? TenantId { get; set; }
}