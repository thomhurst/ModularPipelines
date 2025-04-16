using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("cat-file")]
[ExcludeFromCodeCoverage]
public record GitCatFileOptions : GitOptions
{
    [BooleanCommandSwitch("--no-mailmap")]
    public virtual bool? NoMailmap { get; set; }

    [BooleanCommandSwitch("--mailmap")]
    public virtual bool? Mailmap { get; set; }

    [BooleanCommandSwitch("--no-use-mailmap")]
    public virtual bool? NoUseMailmap { get; set; }

    [BooleanCommandSwitch("--use-mailmap")]
    public virtual bool? UseMailmap { get; set; }

    [BooleanCommandSwitch("--textconv")]
    public virtual bool? Textconv { get; set; }

    [BooleanCommandSwitch("--filters")]
    public virtual bool? Filters { get; set; }

    [CommandEqualsSeparatorSwitch("--path")]
    public string? Path { get; set; }

    [BooleanCommandSwitch("--batch")]
    public virtual bool? Batch { get; set; }

    [BooleanCommandSwitch("--batch-check")]
    public virtual bool? BatchCheck { get; set; }

    [BooleanCommandSwitch("--batch-command")]
    public virtual bool? BatchCommand { get; set; }

    [BooleanCommandSwitch("--batch-all-objects")]
    public virtual bool? BatchAllObjects { get; set; }

    [BooleanCommandSwitch("--buffer")]
    public virtual bool? Buffer { get; set; }

    [BooleanCommandSwitch("--unordered")]
    public virtual bool? Unordered { get; set; }

    [BooleanCommandSwitch("--allow-unknown-type")]
    public virtual bool? AllowUnknownType { get; set; }

    [BooleanCommandSwitch("--follow-symlinks")]
    public virtual bool? FollowSymlinks { get; set; }
}