using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("verify-pack")]
[ExcludeFromCodeCoverage]
public record GitVerifyPackOptions : GitOptions
{
    [BooleanCommandSwitch("--verbose")]
    public virtual bool? Verbose { get; set; }

    [BooleanCommandSwitch("--stat-only")]
    public virtual bool? StatOnly { get; set; }
}