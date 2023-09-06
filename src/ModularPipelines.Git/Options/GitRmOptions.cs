using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("rm")]
[ExcludeFromCodeCoverage]
public record GitRmOptions : GitOptions
{
    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }

    [BooleanCommandSwitch("--cached")]
    public bool? Cached { get; set; }

    [BooleanCommandSwitch("--ignore-unmatch")]
    public bool? IgnoreUnmatch { get; set; }

    [BooleanCommandSwitch("--sparse")]
    public bool? Sparse { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

    [CommandEqualsSeparatorSwitch("--pathspec-from-file")]
    public string? PathspecFromFile { get; set; }

    [BooleanCommandSwitch("--pathspec-file-nul")]
    public bool? PathspecFileNul { get; set; }
}
