using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("update-server-info")]
[ExcludeFromCodeCoverage]
public record GitUpdateServerInfoOptions : GitOptions
{
    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }
}