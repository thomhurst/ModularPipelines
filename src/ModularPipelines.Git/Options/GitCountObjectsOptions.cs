using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("count-objects")]
[ExcludeFromCodeCoverage]
public record GitCountObjectsOptions : GitOptions
{
    [BooleanCommandSwitch("--verbose")]
    public virtual bool? Verbose { get; set; }

    [BooleanCommandSwitch("--human-readable")]
    public virtual bool? HumanReadable { get; set; }
}