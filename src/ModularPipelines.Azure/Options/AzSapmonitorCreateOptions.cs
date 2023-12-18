using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sapmonitor", "create")]
public record AzSapmonitorCreateOptions(
[property: CommandSwitch("--hana-subnet")] string HanaSubnet,
[property: CommandSwitch("--monitor-name")] string MonitorName,
[property: CommandSwitch("--region")] string Region,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--dca")]
    public bool? Dca { get; set; }

    [CommandSwitch("--lawsid")]
    public string? Lawsid { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}