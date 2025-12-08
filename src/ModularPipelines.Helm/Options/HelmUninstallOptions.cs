using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CliSubCommand("uninstall")]
[ExcludeFromCodeCoverage]
public record HelmUninstallOptions : HelmOptions
{
    [CliOption("--cascade")]
    public virtual string? Cascade { get; set; }

    [CliOption("--description")]
    public virtual string? Description { get; set; }

    [CliFlag("--dry-run")]
    public virtual bool? DryRun { get; set; }

    [CliFlag("--keep-history")]
    public virtual bool? KeepHistory { get; set; }

    [CliFlag("--no-hooks")]
    public virtual bool? NoHooks { get; set; }

    [CliOption("--timeout")]
    public virtual string? Timeout { get; set; }

    [CliFlag("--wait")]
    public virtual bool? Wait { get; set; }
}