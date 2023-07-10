using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("checkout-index")]
public record GitCheckoutIndexOptions : GitOptions
{
    [BooleanCommandSwitch("index")]
    public bool? Index { get; set; }

    [BooleanCommandSwitch("quiet")]
    public bool? Quiet { get; set; }

    [BooleanCommandSwitch("force")]
    public bool? Force { get; set; }

    [BooleanCommandSwitch("all")]
    public bool? All { get; set; }

    [BooleanCommandSwitch("no-create")]
    public bool? NoCreate { get; set; }

    [CommandLongSwitch("prefix")]
    public string? Prefix { get; set; }

    [CommandLongSwitch("stage")]
    public string? Stage { get; set; }

    [BooleanCommandSwitch("temp")]
    public bool? Temp { get; set; }

    [BooleanCommandSwitch("ignore-skip-worktree-bits")]
    public bool? IgnoreSkipWorktreeBits { get; set; }

    [BooleanCommandSwitch("stdin")]
    public bool? Stdin { get; set; }

}