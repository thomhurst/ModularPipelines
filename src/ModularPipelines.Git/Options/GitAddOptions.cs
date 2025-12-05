using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliSubCommand("add")]
[ExcludeFromCodeCoverage]
public record GitAddOptions : GitOptions
{
    [CliFlag("--dry-run")]
    public virtual bool? DryRun { get; set; }

    [CliFlag("--verbose")]
    public virtual bool? Verbose { get; set; }

    [CliFlag("--force")]
    public virtual bool? Force { get; set; }

    [CliFlag("--sparse")]
    public virtual bool? Sparse { get; set; }

    [CliFlag("--interactive")]
    public virtual bool? Interactive { get; set; }

    [CliFlag("--patch")]
    public virtual bool? Patch { get; set; }

    [CliFlag("--edit")]
    public virtual bool? Edit { get; set; }

    [CliFlag("--update")]
    public virtual bool? Update { get; set; }

    [CliFlag("--all")]
    public virtual bool? All { get; set; }

    [CliFlag("--no-ignore-removal")]
    public virtual bool? NoIgnoreRemoval { get; set; }

    [CliFlag("--no-all")]
    public virtual bool? NoAll { get; set; }

    [CliFlag("--ignore-removal")]
    public virtual bool? IgnoreRemoval { get; set; }

    [CliFlag("--intent-to-add")]
    public virtual bool? IntentToAdd { get; set; }

    [CliFlag("--refresh")]
    public virtual bool? Refresh { get; set; }

    [CliFlag("--ignore-errors")]
    public virtual bool? IgnoreErrors { get; set; }

    [CliFlag("--ignore-missing")]
    public virtual bool? IgnoreMissing { get; set; }

    [CliFlag("--no-warn-embedded-repo")]
    public virtual bool? NoWarnEmbeddedRepo { get; set; }

    [CliFlag("--renormalize")]
    public virtual bool? Renormalize { get; set; }

    [CliFlag("--chmod")]
    public virtual bool? Chmod { get; set; }

    [CliOption("--pathspec-from-file", Format = OptionFormat.EqualsSeparated)]
    public virtual string? PathspecFromFile { get; set; }

    [CliFlag("--pathspec-file-nul")]
    public virtual bool? PathspecFileNul { get; set; }
}