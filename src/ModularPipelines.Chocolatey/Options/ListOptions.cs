using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("list")]
public record ListOptions(
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

    [CliOption("--version")]
    public virtual string? Version { get; set; }

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

    [CliFlag("--detailed")]
    public virtual bool? Detailed { get; set; }

    [CliFlag("--disable-package-repository-optimizations")]
    public virtual bool? DisablePackageRepositoryOptimizations { get; set; }

    [CliFlag("--show-audit-info")]
    public virtual bool? ShowAuditInfo { get; set; }

    [CliFlag("--force-self-service")]
    public virtual bool? ForceSelfService { get; set; }
}