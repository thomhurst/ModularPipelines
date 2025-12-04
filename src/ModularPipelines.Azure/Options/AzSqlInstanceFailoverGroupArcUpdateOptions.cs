using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sql", "instance-failover-group-arc", "update")]
public record AzSqlInstanceFailoverGroupArcUpdateOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--k8s-namespace")]
    public string? K8sNamespace { get; set; }

    [CliOption("--mi")]
    public string? Mi { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--partner-sync-mode")]
    public string? PartnerSyncMode { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--role")]
    public string? Role { get; set; }

    [CliFlag("--use-k8s")]
    public bool? UseK8s { get; set; }
}