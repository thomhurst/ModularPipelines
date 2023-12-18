using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "midb-arc", "restore")]
public record AzSqlMidbArcRestoreOptions : AzOptions
{
    [CommandSwitch("--dest-name")]
    public string? DestName { get; set; }

    [CommandSwitch("--dry-run")]
    public string? DryRun { get; set; }

    [CommandSwitch("--k8s-namespace")]
    public string? K8sNamespace { get; set; }

    [CommandSwitch("--managed-instance")]
    public string? ManagedInstance { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--time")]
    public string? Time { get; set; }

    [BooleanCommandSwitch("--use-k8s")]
    public bool? UseK8s { get; set; }
}