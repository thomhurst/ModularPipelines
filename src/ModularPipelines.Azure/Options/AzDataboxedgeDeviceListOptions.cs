using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("databoxedge", "device", "list")]
public record AzDataboxedgeDeviceListOptions : AzOptions
{
    [CliOption("--expand")]
    public string? Expand { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}