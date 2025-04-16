using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("config")]
[ExcludeFromCodeCoverage]
public record GitConfigOptions : GitOptions
{
    [BooleanCommandSwitch("--replace-all")]
    public virtual bool? ReplaceAll { get; set; }

    [BooleanCommandSwitch("--add")]
    public virtual bool? Add { get; set; }

    [BooleanCommandSwitch("--get")]
    public virtual bool? Get { get; set; }

    [BooleanCommandSwitch("--get-all")]
    public virtual bool? GetAll { get; set; }

    [BooleanCommandSwitch("--get-regexp")]
    public virtual bool? GetRegexp { get; set; }

    [CommandEqualsSeparatorSwitch("--get-urlmatch")]
    public string? GetUrlmatch { get; set; }

    [BooleanCommandSwitch("--global")]
    public virtual bool? Global { get; set; }

    [BooleanCommandSwitch("--system")]
    public virtual bool? System { get; set; }

    [BooleanCommandSwitch("--local")]
    public virtual bool? Local { get; set; }

    [BooleanCommandSwitch("--worktree")]
    public virtual bool? Worktree { get; set; }

    [CommandEqualsSeparatorSwitch("--file")]
    public string? File { get; set; }

    [CommandEqualsSeparatorSwitch("--blob")]
    public string? Blob { get; set; }

    [BooleanCommandSwitch("--remove-section")]
    public virtual bool? RemoveSection { get; set; }

    [BooleanCommandSwitch("--rename-section")]
    public virtual bool? RenameSection { get; set; }

    [BooleanCommandSwitch("--unset")]
    public virtual bool? Unset { get; set; }

    [BooleanCommandSwitch("--unset-all")]
    public virtual bool? UnsetAll { get; set; }

    [BooleanCommandSwitch("--list")]
    public virtual bool? List { get; set; }

    [BooleanCommandSwitch("--fixed-value")]
    public virtual bool? FixedValue { get; set; }

    [CommandEqualsSeparatorSwitch("--type")]
    public string? Type { get; set; }

    [BooleanCommandSwitch("--bool")]
    public virtual bool? Bool { get; set; }

    [BooleanCommandSwitch("--int")]
    public virtual bool? Int { get; set; }

    [BooleanCommandSwitch("--bool-or-int")]
    public virtual bool? BoolOrInt { get; set; }

    [BooleanCommandSwitch("--path")]
    public virtual bool? Path { get; set; }

    [BooleanCommandSwitch("--expiry-date")]
    public virtual bool? ExpiryDate { get; set; }

    [BooleanCommandSwitch("--no-type")]
    public virtual bool? NoType { get; set; }

    [BooleanCommandSwitch("--null")]
    public virtual bool? Null { get; set; }

    [BooleanCommandSwitch("--name-only")]
    public virtual bool? NameOnly { get; set; }

    [BooleanCommandSwitch("--show-origin")]
    public virtual bool? ShowOrigin { get; set; }

    [BooleanCommandSwitch("--show-scope")]
    public virtual bool? ShowScope { get; set; }

    [CommandEqualsSeparatorSwitch("--get-colorbool")]
    public string? GetColorbool { get; set; }

    [CommandEqualsSeparatorSwitch("--get-color")]
    public string? GetColor { get; set; }

    [BooleanCommandSwitch("--edit")]
    public virtual bool? Edit { get; set; }

    [BooleanCommandSwitch("--no-includes")]
    public virtual bool? NoIncludes { get; set; }

    [BooleanCommandSwitch("--includes")]
    public virtual bool? Includes { get; set; }

    [CommandEqualsSeparatorSwitch("--default")]
    public string? Default { get; set; }
}