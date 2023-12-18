using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "mi-arc", "upgrade")]
public record AzSqlMiArcUpgradeOptions : AzOptions
{
    [CommandSwitch("--desired-version")]
    public string? DesiredVersion { get; set; }

    [CommandSwitch("--dry-run")]
    public string? DryRun { get; set; }

    [CommandSwitch("--field-filter")]
    public string? FieldFilter { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [CommandSwitch("--k8s-namespace")]
    public string? K8sNamespace { get; set; }

    [CommandSwitch("--label-filter")]
    public string? LabelFilter { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--target")]
    public string? Target { get; set; }

    [CommandSwitch("--use-k8s")]
    public string? UseK8s { get; set; }
}