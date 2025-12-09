using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sql", "mi-arc", "upgrade")]
public record AzSqlMiArcUpgradeOptions : AzOptions
{
    [CliOption("--desired-version")]
    public string? DesiredVersion { get; set; }

    [CliOption("--dry-run")]
    public string? DryRun { get; set; }

    [CliOption("--field-filter")]
    public string? FieldFilter { get; set; }

    [CliFlag("--force")]
    public bool? Force { get; set; }

    [CliOption("--k8s-namespace")]
    public string? K8sNamespace { get; set; }

    [CliOption("--label-filter")]
    public string? LabelFilter { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliFlag("--use-k8s")]
    public bool? UseK8s { get; set; }
}