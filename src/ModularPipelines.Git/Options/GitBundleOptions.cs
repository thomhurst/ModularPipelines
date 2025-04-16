using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("bundle")]
[ExcludeFromCodeCoverage]
public record GitBundleOptions : GitOptions
{
    [BooleanCommandSwitch("--progress")]
    public virtual bool? Progress { get; set; }

    [CommandEqualsSeparatorSwitch("--version")]
    public string? Version { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }
}