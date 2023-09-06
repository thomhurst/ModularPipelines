using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("config")]
[ExcludeFromCodeCoverage]
public record GitConfigOptions : GitOptions
{
    [BooleanCommandSwitch("--replace-all")]
    public bool? ReplaceAll { get; set; }

    [BooleanCommandSwitch("--add")]
    public bool? Add { get; set; }

    [BooleanCommandSwitch("--get")]
    public bool? Get { get; set; }

    [BooleanCommandSwitch("--get-all")]
    public bool? GetAll { get; set; }

    [BooleanCommandSwitch("--get-regexp")]
    public bool? GetRegexp { get; set; }

    [CommandEqualsSeparatorSwitch("--get-urlmatch")]
    public string? GetUrlmatch { get; set; }

    [BooleanCommandSwitch("--global")]
    public bool? Global { get; set; }

    [BooleanCommandSwitch("--system")]
    public bool? System { get; set; }

    [BooleanCommandSwitch("--local")]
    public bool? Local { get; set; }

    [BooleanCommandSwitch("--worktree")]
    public bool? Worktree { get; set; }

    [CommandEqualsSeparatorSwitch("--file")]
    public string? File { get; set; }

    [CommandEqualsSeparatorSwitch("--blob")]
    public string? Blob { get; set; }

    [BooleanCommandSwitch("--remove-section")]
    public bool? RemoveSection { get; set; }

    [BooleanCommandSwitch("--rename-section")]
    public bool? RenameSection { get; set; }

    [BooleanCommandSwitch("--unset")]
    public bool? Unset { get; set; }

    [BooleanCommandSwitch("--unset-all")]
    public bool? UnsetAll { get; set; }

    [BooleanCommandSwitch("--list")]
    public bool? List { get; set; }

    [BooleanCommandSwitch("--fixed-value")]
    public bool? FixedValue { get; set; }

    [CommandEqualsSeparatorSwitch("--type")]
    public string? Type { get; set; }

    [BooleanCommandSwitch("--bool")]
    public bool? Bool { get; set; }

    [BooleanCommandSwitch("--int")]
    public bool? Int { get; set; }

    [BooleanCommandSwitch("--bool-or-int")]
    public bool? BoolOrInt { get; set; }

    [BooleanCommandSwitch("--path")]
    public bool? Path { get; set; }

    [BooleanCommandSwitch("--expiry-date")]
    public bool? ExpiryDate { get; set; }

    [BooleanCommandSwitch("--no-type")]
    public bool? NoType { get; set; }

    [BooleanCommandSwitch("--null")]
    public bool? Null { get; set; }

    [BooleanCommandSwitch("--name-only")]
    public bool? NameOnly { get; set; }

    [BooleanCommandSwitch("--show-origin")]
    public bool? ShowOrigin { get; set; }

    [BooleanCommandSwitch("--show-scope")]
    public bool? ShowScope { get; set; }

    [CommandEqualsSeparatorSwitch("--get-colorbool")]
    public string? GetColorbool { get; set; }

    [CommandEqualsSeparatorSwitch("--get-color")]
    public string? GetColor { get; set; }

    [BooleanCommandSwitch("--edit")]
    public bool? Edit { get; set; }

    [BooleanCommandSwitch("--no-includes")]
    public bool? NoIncludes { get; set; }

    [BooleanCommandSwitch("--includes")]
    public bool? Includes { get; set; }

    [CommandEqualsSeparatorSwitch("--default")]
    public string? Default { get; set; }
}
