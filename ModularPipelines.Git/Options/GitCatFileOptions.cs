using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("cat-file")]
public record GitCatFileOptions : GitOptions
{
    [BooleanCommandSwitch("--no-mailmap")]
    public bool? NoMailmap { get; set; }

    [BooleanCommandSwitch("--mailmap")]
    public bool? Mailmap { get; set; }

    [BooleanCommandSwitch("--no-use-mailmap")]
    public bool? NoUseMailmap { get; set; }

    [BooleanCommandSwitch("--use-mailmap")]
    public bool? UseMailmap { get; set; }

    [BooleanCommandSwitch("--textconv")]
    public bool? Textconv { get; set; }

    [BooleanCommandSwitch("--filters")]
    public bool? Filters { get; set; }

    [CommandEqualsSeparatorSwitch("--path")]
    public string? Path { get; set; }

    [BooleanCommandSwitch("--batch")]
    public bool? Batch { get; set; }

    [BooleanCommandSwitch("--batch-check")]
    public bool? BatchCheck { get; set; }

    [BooleanCommandSwitch("--batch-command")]
    public bool? BatchCommand { get; set; }

    [BooleanCommandSwitch("--batch-all-objects")]
    public bool? BatchAllObjects { get; set; }

    [BooleanCommandSwitch("--buffer")]
    public bool? Buffer { get; set; }

    [BooleanCommandSwitch("--unordered")]
    public bool? Unordered { get; set; }

    [BooleanCommandSwitch("--allow-unknown-type")]
    public bool? AllowUnknownType { get; set; }

    [BooleanCommandSwitch("--follow-symlinks")]
    public bool? FollowSymlinks { get; set; }

}