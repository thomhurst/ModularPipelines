using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("list")]
public record ListOptions(
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

    [CommandSwitch("--version")]
    public virtual string? Version { get; set; }

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

    [BooleanCommandSwitch("--detailed")]
    public virtual bool? Detailed { get; set; }

    [BooleanCommandSwitch("--disable-package-repository-optimizations")]
    public virtual bool? DisablePackageRepositoryOptimizations { get; set; }

    [BooleanCommandSwitch("--show-audit-info")]
    public virtual bool? ShowAuditInfo { get; set; }

    [BooleanCommandSwitch("--force-self-service")]
    public virtual bool? ForceSelfService { get; set; }
}