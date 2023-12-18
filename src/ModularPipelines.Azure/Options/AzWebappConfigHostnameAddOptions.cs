using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("webapp", "config", "hostname", "add")]
public record AzWebappConfigHostnameAddOptions : AzOptions
{
    [CommandSwitch("--hostname")]
    public string? Hostname { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--slot")]
    public string? Slot { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--webapp-name")]
    public string? WebappName { get; set; }
}