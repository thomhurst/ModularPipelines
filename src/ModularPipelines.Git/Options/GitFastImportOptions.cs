using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("fast-import")]
[ExcludeFromCodeCoverage]
public record GitFastImportOptions : GitOptions
{
    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

    [BooleanCommandSwitch("--stats")]
    public bool? Stats { get; set; }

    [BooleanCommandSwitch("--allow-unsafe-features")]
    public bool? AllowUnsafeFeatures { get; set; }

    [CommandEqualsSeparatorSwitch("--cat-blob-fd")]
    public string? CatBlobFd { get; set; }

    [CommandEqualsSeparatorSwitch("--date-format")]
    public string? DateFormat { get; set; }

    [BooleanCommandSwitch("--done")]
    public bool? Done { get; set; }

    [CommandEqualsSeparatorSwitch("--export-marks")]
    public string? ExportMarks { get; set; }

    [CommandEqualsSeparatorSwitch("--import-marks")]
    public string? ImportMarks { get; set; }

    [CommandEqualsSeparatorSwitch("--import-marks-if-exists")]
    public string? ImportMarksIfExists { get; set; }

    [BooleanCommandSwitch("--no-relative-marks")]
    public bool? NoRelativeMarks { get; set; }

    [BooleanCommandSwitch("--relative-marks")]
    public bool? RelativeMarks { get; set; }

    [CommandEqualsSeparatorSwitch("--rewrite-submodules-from")]
    public string? RewriteSubmodulesFrom { get; set; }

    [CommandEqualsSeparatorSwitch("--rewrite-submodules-to")]
    public string? RewriteSubmodulesTo { get; set; }

    [CommandEqualsSeparatorSwitch("--active-branches")]
    public string? ActiveBranches { get; set; }

    [CommandEqualsSeparatorSwitch("--big-file-threshold")]
    public string? BigFileThreshold { get; set; }

    [CommandEqualsSeparatorSwitch("--depth")]
    public string? Depth { get; set; }

    [CommandEqualsSeparatorSwitch("--export-pack-edges")]
    public string? ExportPackEdges { get; set; }

    [CommandEqualsSeparatorSwitch("--max-pack-size")]
    public string? MaxPackSize { get; set; }
}
