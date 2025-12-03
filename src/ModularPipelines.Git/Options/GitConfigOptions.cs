using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliCommand("config")]
[ExcludeFromCodeCoverage]
public record GitConfigOptions : GitOptions
{
    [CliFlag("--replace-all")]
    public virtual bool? ReplaceAll { get; set; }

    [CliFlag("--add")]
    public virtual bool? Add { get; set; }

    [CliFlag("--get")]
    public virtual bool? Get { get; set; }

    [CliFlag("--get-all")]
    public virtual bool? GetAll { get; set; }

    [CliFlag("--get-regexp")]
    public virtual bool? GetRegexp { get; set; }

    [CliOption("--get-urlmatch", Format = OptionFormat.EqualsSeparated)]
    public string? GetUrlmatch { get; set; }

    [CliFlag("--global")]
    public virtual bool? Global { get; set; }

    [CliFlag("--system")]
    public virtual bool? System { get; set; }

    [CliFlag("--local")]
    public virtual bool? Local { get; set; }

    [CliFlag("--worktree")]
    public virtual bool? Worktree { get; set; }

    [CliOption("--file", Format = OptionFormat.EqualsSeparated)]
    public string? File { get; set; }

    [CliOption("--blob", Format = OptionFormat.EqualsSeparated)]
    public string? Blob { get; set; }

    [CliFlag("--remove-section")]
    public virtual bool? RemoveSection { get; set; }

    [CliFlag("--rename-section")]
    public virtual bool? RenameSection { get; set; }

    [CliFlag("--unset")]
    public virtual bool? Unset { get; set; }

    [CliFlag("--unset-all")]
    public virtual bool? UnsetAll { get; set; }

    [CliFlag("--list")]
    public virtual bool? List { get; set; }

    [CliFlag("--fixed-value")]
    public virtual bool? FixedValue { get; set; }

    [CliOption("--type", Format = OptionFormat.EqualsSeparated)]
    public string? Type { get; set; }

    [CliFlag("--bool")]
    public virtual bool? Bool { get; set; }

    [CliFlag("--int")]
    public virtual bool? Int { get; set; }

    [CliFlag("--bool-or-int")]
    public virtual bool? BoolOrInt { get; set; }

    [CliFlag("--path")]
    public virtual bool? Path { get; set; }

    [CliFlag("--expiry-date")]
    public virtual bool? ExpiryDate { get; set; }

    [CliFlag("--no-type")]
    public virtual bool? NoType { get; set; }

    [CliFlag("--null")]
    public virtual bool? Null { get; set; }

    [CliFlag("--name-only")]
    public virtual bool? NameOnly { get; set; }

    [CliFlag("--show-origin")]
    public virtual bool? ShowOrigin { get; set; }

    [CliFlag("--show-scope")]
    public virtual bool? ShowScope { get; set; }

    [CliOption("--get-colorbool", Format = OptionFormat.EqualsSeparated)]
    public string? GetColorbool { get; set; }

    [CliOption("--get-color", Format = OptionFormat.EqualsSeparated)]
    public string? GetColor { get; set; }

    [CliFlag("--edit")]
    public virtual bool? Edit { get; set; }

    [CliFlag("--no-includes")]
    public virtual bool? NoIncludes { get; set; }

    [CliFlag("--includes")]
    public virtual bool? Includes { get; set; }

    [CliOption("--default", Format = OptionFormat.EqualsSeparated)]
    public string? Default { get; set; }
}