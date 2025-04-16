using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("outdated")]
public record OutdatedOptions : ChocoOptions
{
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

    [BooleanCommandSwitch("--prerelease")]
    public virtual bool? Prerelease { get; set; }

    [BooleanCommandSwitch("--ignore-pinned")]
    public virtual bool? IgnorePinned { get; set; }

    [BooleanCommandSwitch("--ignore-unfound")]
    public virtual bool? IgnoreUnfound { get; set; }

    [BooleanCommandSwitch("--disable-package-repository-optimizations")]
    public virtual bool? DisablePackageRepositoryOptimizations { get; set; }

    [BooleanCommandSwitch("--force-self-service")]
    public virtual bool? ForceSelfService { get; set; }
}