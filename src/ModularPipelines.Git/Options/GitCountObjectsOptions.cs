using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("count-objects")]
public record GitCountObjectsOptions : GitOptions
{
    [BooleanCommandSwitch("--verbose")]
    public bool? Verbose { get; set; }

    [BooleanCommandSwitch("--human-readable")]
    public bool? HumanReadable { get; set; }
}