using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("fast-import")]
public record GitFastImportOptions : GitOptions
{
    [BooleanCommandSwitch("force")]
    public bool? Force { get; set; }

    [BooleanCommandSwitch("quiet")]
    public bool? Quiet { get; set; }

    [BooleanCommandSwitch("stats")]
    public bool? Stats { get; set; }

    [BooleanCommandSwitch("allow-unsafe-features")]
    public bool? AllowUnsafeFeatures { get; set; }

    [CommandLongSwitch("cat-blob-fd")]
    public string? CatBlobFd { get; set; }

    [CommandLongSwitch("date-format")]
    public string? DateFormat { get; set; }

    [BooleanCommandSwitch("done")]
    public bool? Done { get; set; }

    [CommandLongSwitch("export-marks")]
    public string? ExportMarks { get; set; }

    [CommandLongSwitch("import-marks")]
    public string? ImportMarks { get; set; }

    [CommandLongSwitch("import-marks-if-exists")]
    public string? ImportMarksIfExists { get; set; }

    [BooleanCommandSwitch("no-relative-marks")]
    public bool? NoRelativeMarks { get; set; }

    [BooleanCommandSwitch("relative-marks")]
    public bool? RelativeMarks { get; set; }

    [CommandLongSwitch("rewrite-submodules-from")]
    public string? RewriteSubmodulesFrom { get; set; }

    [CommandLongSwitch("rewrite-submodules-to")]
    public string? RewriteSubmodulesTo { get; set; }

    [CommandLongSwitch("active-branches")]
    public string? ActiveBranches { get; set; }

    [CommandLongSwitch("big-file-threshold")]
    public string? BigFileThreshold { get; set; }

    [CommandLongSwitch("depth")]
    public string? Depth { get; set; }

    [CommandLongSwitch("export-pack-edges")]
    public string? ExportPackEdges { get; set; }

    [CommandLongSwitch("max-pack-size")]
    public string? MaxPackSize { get; set; }

}