using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliSubCommand("init")]
[ExcludeFromCodeCoverage]
public record GitInitOptions : GitOptions
{
    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }

    [CliFlag("--bare")]
    public virtual bool? Bare { get; set; }

    [CliOption("--object-format", Format = OptionFormat.EqualsSeparated)]
    public string? ObjectFormat { get; set; }

    [CliOption("--template", Format = OptionFormat.EqualsSeparated)]
    public string? Template { get; set; }

    [CliOption("--separate-git-dir", Format = OptionFormat.EqualsSeparated)]
    public string? SeparateGitDir { get; set; }

    [CliOption("--initial-branch", Format = OptionFormat.EqualsSeparated)]
    public string? InitialBranch { get; set; }

    [CliOption("--shared", Format = OptionFormat.EqualsSeparated)]
    public string? Shared { get; set; }
}