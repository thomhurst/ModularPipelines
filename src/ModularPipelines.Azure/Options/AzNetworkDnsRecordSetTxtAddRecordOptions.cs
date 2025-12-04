using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "dns", "record-set", "txt", "add-record")]
public record AzNetworkDnsRecordSetTxtAddRecordOptions(
[property: CliOption("--record-set-name")] string RecordSetName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--value")] string Value,
[property: CliOption("--zone-name")] string ZoneName
) : AzOptions
{
    [CliOption("--if-none-match")]
    public string? IfNoneMatch { get; set; }
}