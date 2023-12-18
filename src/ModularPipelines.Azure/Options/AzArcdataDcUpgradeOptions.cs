using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("arcdata", "dc", "upgrade")]
public record AzArcdataDcUpgradeOptions : AzOptions
{
    [CommandSwitch("--desired-version")]
    public string? DesiredVersion { get; set; }

    [CommandSwitch("--dry-run")]
    public string? DryRun { get; set; }

    [CommandSwitch("--k8s-namespace")]
    public string? K8sNamespace { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [BooleanCommandSwitch("--use-k8s")]
    public bool? UseK8s { get; set; }
}