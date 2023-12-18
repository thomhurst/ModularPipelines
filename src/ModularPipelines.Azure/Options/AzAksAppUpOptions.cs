using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks", "app", "up")]
public record AzAksAppUpOptions : AzOptions
{
    [CommandSwitch("--acr")]
    public string? Acr { get; set; }

    [CommandSwitch("--aks-cluster")]
    public string? AksCluster { get; set; }

    [CommandSwitch("--branch-name")]
    public string? BranchName { get; set; }

    [BooleanCommandSwitch("--do-not-wait")]
    public bool? DoNotWait { get; set; }

    [CommandSwitch("--port")]
    public int? Port { get; set; }

    [CommandSwitch("--repository")]
    public string? Repository { get; set; }
}