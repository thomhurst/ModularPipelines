using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dns", "record-sets", "describe")]
public record GcloudDnsRecordSetsDescribeOptions(
[property: PositionalArgument] string DnsName,
[property: CommandSwitch("--type")] string Type,
[property: CommandSwitch("--zone")] string Zone
) : GcloudOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }
}