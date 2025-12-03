using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "hub", "connection-string", "show")]
public record AzIotHubConnectionStringShowOptions : AzOptions
{
    [CliFlag("--all")]
    public bool? All { get; set; }

    [CliFlag("--default-eventhub")]
    public bool? DefaultEventhub { get; set; }

    [CliOption("--hub-name")]
    public string? HubName { get; set; }

    [CliOption("--key-type")]
    public string? KeyType { get; set; }

    [CliOption("--pn")]
    public string? Pn { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}