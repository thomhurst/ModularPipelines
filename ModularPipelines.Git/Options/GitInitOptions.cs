using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("init")]
public record GitInitOptions : GitOptions
{
    [BooleanCommandSwitch("quiet")]
    public bool? Quiet { get; set; }

    [BooleanCommandSwitch("bare")]
    public bool? Bare { get; set; }

    [CommandLongSwitch("object-format")]
    public string? ObjectFormat { get; set; }

    [CommandLongSwitch("template")]
    public string? Template { get; set; }

    [CommandLongSwitch("separate-git-dir")]
    public string? SeparateGitDir { get; set; }

    [CommandLongSwitch("initial-branch")]
    public string? InitialBranch { get; set; }

    [CommandLongSwitch("shared")]
    public string? Shared { get; set; }

}