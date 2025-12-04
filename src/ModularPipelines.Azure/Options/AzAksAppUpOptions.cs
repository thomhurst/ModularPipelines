using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("aks", "app", "up")]
public record AzAksAppUpOptions : AzOptions
{
    [CliOption("--acr")]
    public string? Acr { get; set; }

    [CliOption("--aks-cluster")]
    public string? AksCluster { get; set; }

    [CliOption("--branch-name")]
    public string? BranchName { get; set; }

    [CliFlag("--do-not-wait")]
    public bool? DoNotWait { get; set; }

    [CliOption("--port")]
    public int? Port { get; set; }

    [CliOption("--repository")]
    public string? Repository { get; set; }
}