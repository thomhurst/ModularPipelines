using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("bisect")]
[ExcludeFromCodeCoverage]
public record GitBisectOptions : GitOptions
{
    [BooleanCommandSwitch("--no-checkout")]
    public virtual bool? NoCheckout { get; set; }

    [BooleanCommandSwitch("--first-parent")]
    public virtual bool? FirstParent { get; set; }
}