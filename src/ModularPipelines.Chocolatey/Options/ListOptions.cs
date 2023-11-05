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
    public string? Source { get; set; }

    [BooleanCommandSwitch("--id-only")]
    public bool? IdOnly { get; set; }

    [BooleanCommandSwitch("--prerelease")]
    public bool? Prerelease { get; set; }

    [BooleanCommandSwitch("--include-programs")]
    public bool? IncludePrograms { get; set; }

    [CommandSwitch("--version")]
    public string? Version { get; set; }

    [CommandSwitch("--page")]
    public string? Page { get; set; }

    [CommandSwitch("--page-size")]
    public string? PageSize { get; set; }

    [BooleanCommandSwitch("--exact")]
    public bool? Exact { get; set; }

    [BooleanCommandSwitch("--by-id-only")]
    public bool? ByIdOnly { get; set; }

    [BooleanCommandSwitch("--by-tags-only")]
    public bool? ByTagsOnly { get; set; }

    [BooleanCommandSwitch("--id-starts-with")]
    public bool? IdStartsWith { get; set; }

    [BooleanCommandSwitch("--detailed")]
    public bool? Detailed { get; set; }

    [BooleanCommandSwitch("--disable-package-repository-optimizations")]
    public bool? DisablePackageRepositoryOptimizations { get; set; }

    [BooleanCommandSwitch("--show-audit-info")]
    public bool? ShowAuditInfo { get; set; }

    [BooleanCommandSwitch("--force-self-service")]
    public bool? ForceSelfService { get; set; }
}