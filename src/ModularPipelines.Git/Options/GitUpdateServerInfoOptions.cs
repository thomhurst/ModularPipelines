using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("update-server-info")]
[ExcludeFromCodeCoverage]
public record GitUpdateServerInfoOptions : GitOptions
{
    [BooleanCommandSwitch("--force")]
    public virtual bool? Force { get; set; }
}