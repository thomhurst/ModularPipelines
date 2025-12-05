using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliSubCommand("fast-import")]
[ExcludeFromCodeCoverage]
public record GitFastImportOptions : GitOptions
{
    [CliFlag("--force")]
    public virtual bool? Force { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }

    [CliFlag("--stats")]
    public virtual bool? Stats { get; set; }

    [CliFlag("--allow-unsafe-features")]
    public virtual bool? AllowUnsafeFeatures { get; set; }

    [CliOption("--cat-blob-fd", Format = OptionFormat.EqualsSeparated)]
    public virtual string? CatBlobFd { get; set; }

    [CliOption("--date-format", Format = OptionFormat.EqualsSeparated)]
    public virtual string? DateFormat { get; set; }

    [CliFlag("--done")]
    public virtual bool? Done { get; set; }

    [CliOption("--export-marks", Format = OptionFormat.EqualsSeparated)]
    public virtual string? ExportMarks { get; set; }

    [CliOption("--import-marks", Format = OptionFormat.EqualsSeparated)]
    public virtual string? ImportMarks { get; set; }

    [CliOption("--import-marks-if-exists", Format = OptionFormat.EqualsSeparated)]
    public virtual string? ImportMarksIfExists { get; set; }

    [CliFlag("--no-relative-marks")]
    public virtual bool? NoRelativeMarks { get; set; }

    [CliFlag("--relative-marks")]
    public virtual bool? RelativeMarks { get; set; }

    [CliOption("--rewrite-submodules-from", Format = OptionFormat.EqualsSeparated)]
    public virtual string? RewriteSubmodulesFrom { get; set; }

    [CliOption("--rewrite-submodules-to", Format = OptionFormat.EqualsSeparated)]
    public virtual string? RewriteSubmodulesTo { get; set; }

    [CliOption("--active-branches", Format = OptionFormat.EqualsSeparated)]
    public virtual string? ActiveBranches { get; set; }

    [CliOption("--big-file-threshold", Format = OptionFormat.EqualsSeparated)]
    public virtual string? BigFileThreshold { get; set; }

    [CliOption("--depth", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Depth { get; set; }

    [CliOption("--export-pack-edges", Format = OptionFormat.EqualsSeparated)]
    public virtual string? ExportPackEdges { get; set; }

    [CliOption("--max-pack-size", Format = OptionFormat.EqualsSeparated)]
    public virtual string? MaxPackSize { get; set; }
}