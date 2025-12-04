using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("offure", "hyperv", "machine", "show")]
public record AzOffazureHypervMachineShowOptions : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--machine-name")]
    public string? MachineName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--site-name")]
    public string? SiteName { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}