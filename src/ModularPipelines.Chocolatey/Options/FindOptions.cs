using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("find")]
public record FindOptions(
    [property: PositionalArgument] string Filter
) : ChocoOptions
{
    [CommandSwitch("--source")]
    public virtual string? Source { get; set; }

    [BooleanCommandSwitch("--id-only")]
    public virtual bool? IdOnly { get; set; }

    [BooleanCommandSwitch("--prerelease")]
    public virtual bool? Prerelease { get; set; }

    [BooleanCommandSwitch("--include-programs")]
    public virtual bool? IncludePrograms { get; set; }

    [BooleanCommandSwitch("--all-versions")]
    public virtual bool? AllVersions { get; set; }

    [CommandSwitch("--version")]
    public virtual string? Version { get; set; }

    [CommandSwitch("--user")]
    public virtual string? User { get; set; }

    [CommandSwitch("--password")]
    public virtual string? Password { get; set; }

    [CommandSwitch("--cert")]
    public virtual string? Cert { get; set; }

    [CommandSwitch("--certpassword")]
    public virtual string? Certpassword { get; set; }

    [CommandSwitch("--page")]
    public virtual string? Page { get; set; }

    [CommandSwitch("--page-size")]
    public virtual string? PageSize { get; set; }

    [BooleanCommandSwitch("--exact")]
    public virtual bool? Exact { get; set; }

    [BooleanCommandSwitch("--by-id-only")]
    public virtual bool? ByIdOnly { get; set; }

    [BooleanCommandSwitch("--by-tags-only")]
    public virtual bool? ByTagsOnly { get; set; }

    [BooleanCommandSwitch("--id-starts-with")]
    public virtual bool? IdStartsWith { get; set; }

    [BooleanCommandSwitch("--order-by-popularity")]
    public virtual bool? OrderByPopularity { get; set; }

    [BooleanCommandSwitch("--approved-only")]
    public virtual bool? ApprovedOnly { get; set; }

    [BooleanCommandSwitch("--download-cache-only")]
    public virtual bool? DownloadCacheOnly { get; set; }

    [BooleanCommandSwitch("--not-broken")]
    public virtual bool? NotBroken { get; set; }

    [BooleanCommandSwitch("--detailed")]
    public virtual bool? Detailed { get; set; }

    [BooleanCommandSwitch("--disable-package-repository-optimizations")]
    public virtual bool? DisablePackageRepositoryOptimizations { get; set; }

    [BooleanCommandSwitch("--show-audit-info")]
    public virtual bool? ShowAuditInfo { get; set; }

    [BooleanCommandSwitch("--force-self-service")]
    public virtual bool? ForceSelfService { get; set; }
}