using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliSubCommand("cat-file")]
[ExcludeFromCodeCoverage]
public record GitCatFileOptions : GitOptions
{
    [CliFlag("--no-mailmap")]
    public virtual bool? NoMailmap { get; set; }

    [CliFlag("--mailmap")]
    public virtual bool? Mailmap { get; set; }

    [CliFlag("--no-use-mailmap")]
    public virtual bool? NoUseMailmap { get; set; }

    [CliFlag("--use-mailmap")]
    public virtual bool? UseMailmap { get; set; }

    [CliFlag("--textconv")]
    public virtual bool? Textconv { get; set; }

    [CliFlag("--filters")]
    public virtual bool? Filters { get; set; }

    [CliOption("--path", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Path { get; set; }

    [CliFlag("--batch")]
    public virtual bool? Batch { get; set; }

    [CliFlag("--batch-check")]
    public virtual bool? BatchCheck { get; set; }

    [CliFlag("--batch-command")]
    public virtual bool? BatchCommand { get; set; }

    [CliFlag("--batch-all-objects")]
    public virtual bool? BatchAllObjects { get; set; }

    [CliFlag("--buffer")]
    public virtual bool? Buffer { get; set; }

    [CliFlag("--unordered")]
    public virtual bool? Unordered { get; set; }

    [CliFlag("--allow-unknown-type")]
    public virtual bool? AllowUnknownType { get; set; }

    [CliFlag("--follow-symlinks")]
    public virtual bool? FollowSymlinks { get; set; }
}