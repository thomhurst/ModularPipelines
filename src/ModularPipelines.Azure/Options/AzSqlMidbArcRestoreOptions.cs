using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "midb-arc", "restore")]
public record AzSqlMidbArcRestoreOptions : AzOptions
{
    [CliOption("--dest-name")]
    public string? DestName { get; set; }

    [CliOption("--dry-run")]
    public string? DryRun { get; set; }

    [CliOption("--k8s-namespace")]
    public string? K8sNamespace { get; set; }

    [CliOption("--managed-instance")]
    public string? ManagedInstance { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--time")]
    public string? Time { get; set; }

    [CliFlag("--use-k8s")]
    public bool? UseK8s { get; set; }
}