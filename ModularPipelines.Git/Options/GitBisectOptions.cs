using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("bisect")]
public record GitBisectOptions : GitOptions
{
    [BooleanCommandSwitch("no-checkout")]
    public bool? NoCheckout { get; set; }

    [BooleanCommandSwitch("first-parent")]
    public bool? FirstParent { get; set; }

}