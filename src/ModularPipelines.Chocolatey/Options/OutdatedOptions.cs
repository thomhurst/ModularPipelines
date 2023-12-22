using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("outdated")]
public record OutdatedOptions : ChocoOptions
{
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

    [BooleanCommandSwitch("--prerelease")]
    public bool? Prerelease { get; set; }

    [BooleanCommandSwitch("--ignore-pinned")]
    public bool? IgnorePinned { get; set; }

    [BooleanCommandSwitch("--ignore-unfound")]
    public bool? IgnoreUnfound { get; set; }

    [BooleanCommandSwitch("--disable-package-repository-optimizations")]
    public bool? DisablePackageRepositoryOptimizations { get; set; }

    [BooleanCommandSwitch("--force-self-service")]
    public bool? ForceSelfService { get; set; }
}