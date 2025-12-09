using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("kusto", "cluster-principal-assignment", "create")]
public record AzKustoClusterPrincipalAssignmentCreateOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--principal-assignment-name")] string PrincipalAssignmentName,
[property: CliOption("--resource-group")] string ResourceGroup
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