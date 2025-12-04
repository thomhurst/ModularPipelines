using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("provider", "list")]
public record AzProviderListOptions : AzOptions
{
    [CliOption("--expand")]
    public string? Expand { get; set; }
}