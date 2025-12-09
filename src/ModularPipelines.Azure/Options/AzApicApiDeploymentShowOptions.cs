using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("apic", "api", "deployment", "show")]
public record AzApicApiDeploymentShowOptions : AzOptions
{
    [CliOption("--api")]
    public string? Api { get; set; }

    [CliOption("--deployment")]
    public string? Deployment { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--service")]
    public string? Service { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--workspace")]
    public string? Workspace { get; set; }
}