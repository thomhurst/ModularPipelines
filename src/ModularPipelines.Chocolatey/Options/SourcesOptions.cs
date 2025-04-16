using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sources")]
public record SourcesOptions : ChocoOptions
{
    [CommandSwitch("--name")]
    public virtual string? Name { get; set; }

    [CommandSwitch("--source")]
    public virtual string? Source { get; set; }

    [CommandSwitch("--user")]
    public virtual string? User { get; set; }

    [CommandSwitch("--password")]
    public virtual string? Password { get; set; }

    [CommandSwitch("--cert")]
    public virtual string? Cert { get; set; }

    [CommandSwitch("--certpassword")]
    public virtual string? Certpassword { get; set; }

    [CommandSwitch("--priority")]
    public virtual string? Priority { get; set; }

    [BooleanCommandSwitch("--bypass-proxy")]
    public virtual bool? BypassProxy { get; set; }

    [BooleanCommandSwitch("--allow-self-service")]
    public virtual bool? AllowSelfService { get; set; }

    [BooleanCommandSwitch("--admin-only")]
    public virtual bool? AdminOnly { get; set; }
}