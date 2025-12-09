using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliSubCommand("bisect")]
[ExcludeFromCodeCoverage]
public record GitBisectOptions : GitOptions
{
    [CliFlag("--no-checkout")]
    public virtual bool? NoCheckout { get; set; }

    [CliFlag("--first-parent")]
    public virtual bool? FirstParent { get; set; }
}