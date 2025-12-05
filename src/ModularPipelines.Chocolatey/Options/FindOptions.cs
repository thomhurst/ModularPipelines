using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
[CliCommand("find")]
public record FindOptions(
    [property: CliArgument] string Filter
) : ChocoOptions
{
    [CliOption("--source")]
    public virtual string? Source { get; set; }

    [CliFlag("--id-only")]
    public virtual bool? IdOnly { get; set; }

    [CliFlag("--prerelease")]
    public virtual bool? Prerelease { get; set; }

    [CliFlag("--include-programs")]
    public virtual bool? IncludePrograms { get; set; }

    [CliFlag("--all-versions")]
    public virtual bool? AllVersions { get; set; }

    [CliOption("--version")]
    public virtual string? Version { get; set; }

    [CliOption("--user")]
    public virtual string? User { get; set; }

    [CliOption("--password")]
    public virtual string? Password { get; set; }

    [CliOption("--cert")]
    public virtual string? Cert { get; set; }

    [CliOption("--certpassword")]
    public virtual string? Certpassword { get; set; }

    [CliOption("--page")]
    public virtual string? Page { get; set; }

    [CliOption("--page-size")]
    public virtual string? PageSize { get; set; }

    [CliFlag("--exact")]
    public virtual bool? Exact { get; set; }

    [CliFlag("--by-id-only")]
    public virtual bool? ByIdOnly { get; set; }

    [CliFlag("--by-tags-only")]
    public virtual bool? ByTagsOnly { get; set; }

    [CliFlag("--id-starts-with")]
    public virtual bool? IdStartsWith { get; set; }

    [CliFlag("--order-by-popularity")]
    public virtual bool? OrderByPopularity { get; set; }

    [CliFlag("--approved-only")]
    public virtual bool? ApprovedOnly { get; set; }

    [CliFlag("--download-cache-only")]
    public virtual bool? DownloadCacheOnly { get; set; }

    [CliFlag("--not-broken")]
    public virtual bool? NotBroken { get; set; }

    [CliFlag("--detailed")]
    public virtual bool? Detailed { get; set; }

    [CliFlag("--disable-package-repository-optimizations")]
    public virtual bool? DisablePackageRepositoryOptimizations { get; set; }

    [CliFlag("--show-audit-info")]
    public virtual bool? ShowAuditInfo { get; set; }

    [CliFlag("--force-self-service")]
    public virtual bool? ForceSelfService { get; set; }
}