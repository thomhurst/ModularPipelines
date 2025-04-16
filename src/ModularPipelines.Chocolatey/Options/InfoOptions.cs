using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("info")]
public record InfoOptions : ChocoOptions
{
    [CommandSwitch("--source")]
    public virtual string? Source { get; set; }

    [BooleanCommandSwitch("--local-only")]
    public virtual bool? LocalOnly { get; set; }

    [CommandSwitch("--version")]
    public virtual string? Version { get; set; }

    [BooleanCommandSwitch("--prerelease")]
    public virtual bool? Prerelease { get; set; }

    [CommandSwitch("--user")]
    public virtual string? User { get; set; }

    [CommandSwitch("--password")]
    public virtual string? Password { get; set; }

    [CommandSwitch("--cert")]
    public virtual string? Cert { get; set; }

    [CommandSwitch("--certpassword")]
    public virtual string? Certpassword { get; set; }

    [BooleanCommandSwitch("--disable-package-repository-optimizations")]
    public virtual bool? DisablePackageRepositoryOptimizations { get; set; }

    [BooleanCommandSwitch("--force-self-service")]
    public virtual bool? ForceSelfService { get; set; }
}