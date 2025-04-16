using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("add")]
[ExcludeFromCodeCoverage]
public record GitAddOptions : GitOptions
{
    [BooleanCommandSwitch("--dry-run")]
    public virtual bool? DryRun { get; set; }

    [BooleanCommandSwitch("--verbose")]
    public virtual bool? Verbose { get; set; }

    [BooleanCommandSwitch("--force")]
    public virtual bool? Force { get; set; }

    [BooleanCommandSwitch("--sparse")]
    public virtual bool? Sparse { get; set; }

    [BooleanCommandSwitch("--interactive")]
    public virtual bool? Interactive { get; set; }

    [BooleanCommandSwitch("--patch")]
    public virtual bool? Patch { get; set; }

    [BooleanCommandSwitch("--edit")]
    public virtual bool? Edit { get; set; }

    [BooleanCommandSwitch("--update")]
    public virtual bool? Update { get; set; }

    [BooleanCommandSwitch("--all")]
    public virtual bool? All { get; set; }

    [BooleanCommandSwitch("--no-ignore-removal")]
    public virtual bool? NoIgnoreRemoval { get; set; }

    [BooleanCommandSwitch("--no-all")]
    public virtual bool? NoAll { get; set; }

    [BooleanCommandSwitch("--ignore-removal")]
    public virtual bool? IgnoreRemoval { get; set; }

    [BooleanCommandSwitch("--intent-to-add")]
    public virtual bool? IntentToAdd { get; set; }

    [BooleanCommandSwitch("--refresh")]
    public virtual bool? Refresh { get; set; }

    [BooleanCommandSwitch("--ignore-errors")]
    public virtual bool? IgnoreErrors { get; set; }

    [BooleanCommandSwitch("--ignore-missing")]
    public virtual bool? IgnoreMissing { get; set; }

    [BooleanCommandSwitch("--no-warn-embedded-repo")]
    public virtual bool? NoWarnEmbeddedRepo { get; set; }

    [BooleanCommandSwitch("--renormalize")]
    public virtual bool? Renormalize { get; set; }

    [BooleanCommandSwitch("--chmod")]
    public virtual bool? Chmod { get; set; }

    [CommandEqualsSeparatorSwitch("--pathspec-from-file")]
    public string? PathspecFromFile { get; set; }

    [BooleanCommandSwitch("--pathspec-file-nul")]
    public virtual bool? PathspecFileNul { get; set; }
}