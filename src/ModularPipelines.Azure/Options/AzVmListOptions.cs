using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vm", "list")]
public record AzVmListOptions : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliFlag("--show-details")]
    public bool? ShowDetails { get; set; }

    [CliOption("--vmss")]
    public string? Vmss { get; set; }
}