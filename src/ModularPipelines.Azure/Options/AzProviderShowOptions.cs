using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("provider", "show")]
public record AzProviderShowOptions(
[property: CliOption("--namespace")] string Namespace
) : AzOptions
{
    [CliOption("--expand")]
    public string? Expand { get; set; }
}