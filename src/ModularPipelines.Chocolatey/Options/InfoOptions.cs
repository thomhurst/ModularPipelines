using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("info")]
public record InfoOptions : ChocoOptions
{
    [CliOption("--source")]
    public virtual string? Source { get; set; }

    [CliFlag("--local-only")]
    public virtual bool? LocalOnly { get; set; }

    [CliOption("--version")]
    public virtual string? Version { get; set; }

    [CliFlag("--prerelease")]
    public virtual bool? Prerelease { get; set; }

    [CliOption("--user")]
    public virtual string? User { get; set; }

    [CliOption("--password")]
    public virtual string? Password { get; set; }

    [CliOption("--cert")]
    public virtual string? Cert { get; set; }

    [CliOption("--certpassword")]
    public virtual string? Certpassword { get; set; }

    [CliFlag("--disable-package-repository-optimizations")]
    public virtual bool? DisablePackageRepositoryOptimizations { get; set; }

    [CliFlag("--force-self-service")]
    public virtual bool? ForceSelfService { get; set; }
}