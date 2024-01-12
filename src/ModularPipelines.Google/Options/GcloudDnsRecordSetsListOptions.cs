using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dns", "record-sets", "list")]
public record GcloudDnsRecordSetsListOptions(
[property: CommandSwitch("--zone")] string Zone
) : GcloudOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }
}