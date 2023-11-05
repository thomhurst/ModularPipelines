using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sources", "enable")]
public record SourcesEnableOptions : ChocoOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--source")]
    public string? Source { get; set; }

    [CommandSwitch("--user")]
    public string? User { get; set; }

    [CommandSwitch("--password")]
    public string? Password { get; set; }

    [CommandSwitch("--cert")]
    public string? Cert { get; set; }

    [CommandSwitch("--certpassword")]
    public string? Certpassword { get; set; }

    [CommandSwitch("--priority")]
    public string? Priority { get; set; }

    [BooleanCommandSwitch("--bypass-proxy")]
    public bool? BypassProxy { get; set; }

    [BooleanCommandSwitch("--allow-self-service")]
    public bool? AllowSelfService { get; set; }

    [BooleanCommandSwitch("--admin-only")]
    public bool? AdminOnly { get; set; }
}