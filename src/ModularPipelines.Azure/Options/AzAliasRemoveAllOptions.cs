using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("alias", "remove-all")]
public record AzAliasRemoveAllOptions : AzOptions
{
    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}