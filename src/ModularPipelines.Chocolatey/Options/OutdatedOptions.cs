using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("outdated")]
public record OutdatedOptions : ChocoOptions
{
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

    [CliFlag("--prerelease")]
    public virtual bool? Prerelease { get; set; }

    [CliFlag("--ignore-pinned")]
    public virtual bool? IgnorePinned { get; set; }

    [CliFlag("--ignore-unfound")]
    public virtual bool? IgnoreUnfound { get; set; }

    [CliFlag("--disable-package-repository-optimizations")]
    public virtual bool? DisablePackageRepositoryOptimizations { get; set; }

    [CliFlag("--force-self-service")]
    public virtual bool? ForceSelfService { get; set; }
}