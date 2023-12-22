using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("add")]
[ExcludeFromCodeCoverage]
public record GitAddOptions : GitOptions
{
    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }

    [BooleanCommandSwitch("--verbose")]
    public bool? Verbose { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [BooleanCommandSwitch("--sparse")]
    public bool? Sparse { get; set; }

    [BooleanCommandSwitch("--interactive")]
    public bool? Interactive { get; set; }

    [BooleanCommandSwitch("--patch")]
    public bool? Patch { get; set; }

    [BooleanCommandSwitch("--edit")]
    public bool? Edit { get; set; }

    [BooleanCommandSwitch("--update")]
    public bool? Update { get; set; }

    [BooleanCommandSwitch("--all")]
    public bool? All { get; set; }

    [BooleanCommandSwitch("--no-ignore-removal")]
    public bool? NoIgnoreRemoval { get; set; }

    [BooleanCommandSwitch("--no-all")]
    public bool? NoAll { get; set; }

    [BooleanCommandSwitch("--ignore-removal")]
    public bool? IgnoreRemoval { get; set; }

    [BooleanCommandSwitch("--intent-to-add")]
    public bool? IntentToAdd { get; set; }

    [BooleanCommandSwitch("--refresh")]
    public bool? Refresh { get; set; }

    [BooleanCommandSwitch("--ignore-errors")]
    public bool? IgnoreErrors { get; set; }

    [BooleanCommandSwitch("--ignore-missing")]
    public bool? IgnoreMissing { get; set; }

    [BooleanCommandSwitch("--no-warn-embedded-repo")]
    public bool? NoWarnEmbeddedRepo { get; set; }

    [BooleanCommandSwitch("--renormalize")]
    public bool? Renormalize { get; set; }

    [BooleanCommandSwitch("--chmod")]
    public bool? Chmod { get; set; }

    [CommandEqualsSeparatorSwitch("--pathspec-from-file")]
    public string? PathspecFromFile { get; set; }

    [BooleanCommandSwitch("--pathspec-file-nul")]
    public bool? PathspecFileNul { get; set; }
}