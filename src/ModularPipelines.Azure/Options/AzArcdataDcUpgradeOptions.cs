using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("arcdata", "dc", "upgrade")]
public record AzArcdataDcUpgradeOptions(
[property: CommandSwitch("--path")] string Path
) : AzOptions
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

    [CommandSwitch("--target")]
    public string? Target { get; set; }

    [CommandSwitch("--use-k8s")]
    public string? UseK8s { get; set; }
}

