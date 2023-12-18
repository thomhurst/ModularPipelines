using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "dns", "record-set", "srv", "add-record")]
public record AzNetworkDnsRecordSetSrvAddRecordOptions(
[property: CommandSwitch("--port")] int Port,
[property: CommandSwitch("--priority")] string Priority,
[property: CommandSwitch("--record-set-name")] string RecordSetName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--target")] string Target,
[property: CommandSwitch("--weight")] string Weight,
[property: CommandSwitch("--zone-name")] string ZoneName
) : AzOptions
{
    [CommandSwitch("--if-none-match")]
    public string? IfNoneMatch { get; set; }
}