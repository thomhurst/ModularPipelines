using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("lab", "vm", "claim")]
public record AzLabVmClaimOptions : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--lab-name")]
    public string? LabName { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}