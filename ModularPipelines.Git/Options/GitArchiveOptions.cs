using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("archive")]
public record GitArchiveOptions : GitOptions
{
    [CommandLongSwitch("format")]
    public string? Format { get; set; }

    [BooleanCommandSwitch("list")]
    public bool? List { get; set; }

    [BooleanCommandSwitch("verbose")]
    public bool? Verbose { get; set; }

    [CommandLongSwitch("prefix")]
    public string? Prefix { get; set; }

    [CommandLongSwitch("output")]
    public string? Output { get; set; }

    [CommandLongSwitch("add-file")]
    public string? AddFile { get; set; }

    [CommandLongSwitch("add-virtual-file")]
    public string? AddVirtualFile { get; set; }

    [BooleanCommandSwitch("worktree-attributes")]
    public bool? WorktreeAttributes { get; set; }

    [CommandLongSwitch("mtime")]
    public string? Mtime { get; set; }

    [CommandLongSwitch("remote")]
    public string? Remote { get; set; }

    [CommandLongSwitch("exec")]
    public string? Exec { get; set; }

}