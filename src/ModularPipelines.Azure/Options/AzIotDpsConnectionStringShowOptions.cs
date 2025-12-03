using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "dps", "connection-string", "show")]
public record AzIotDpsConnectionStringShowOptions : AzOptions
{
    [CliFlag("--all")]
    public bool? All { get; set; }

    [CliOption("--dps-name")]
    public string? DpsName { get; set; }

    [CliOption("--key-type")]
    public string? KeyType { get; set; }

    [CliOption("--pn")]
    public string? Pn { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}