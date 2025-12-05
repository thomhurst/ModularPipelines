using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sources")]
public record SourcesOptions : ChocoOptions
{
    [CliOption("--name")]
    public virtual string? Name { get; set; }

    [CliOption("--source")]
    public virtual string? Source { get; set; }

    [CliOption("--user")]
    public virtual string? User { get; set; }

    [CliOption("--password")]
    public virtual string? Password { get; set; }

    [CliOption("--cert")]
    public virtual string? Cert { get; set; }

    [CliOption("--certpassword")]
    public virtual string? Certpassword { get; set; }

    [CliOption("--priority")]
    public virtual string? Priority { get; set; }

    [CliFlag("--bypass-proxy")]
    public virtual bool? BypassProxy { get; set; }

    [CliFlag("--allow-self-service")]
    public virtual bool? AllowSelfService { get; set; }

    [CliFlag("--admin-only")]
    public virtual bool? AdminOnly { get; set; }
}