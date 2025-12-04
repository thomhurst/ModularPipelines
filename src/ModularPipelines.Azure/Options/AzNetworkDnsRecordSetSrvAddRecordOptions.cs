using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "dns", "record-set", "srv", "add-record")]
public record AzNetworkDnsRecordSetSrvAddRecordOptions(
[property: CliOption("--port")] int Port,
[property: CliOption("--priority")] string Priority,
[property: CliOption("--record-set-name")] string RecordSetName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--target")] string Target,
[property: CliOption("--weight")] string Weight,
[property: CliOption("--zone-name")] string ZoneName
) : AzOptions
{
    [CliOption("--if-none-match")]
    public string? IfNoneMatch { get; set; }
}