using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("interactive")]
public record AzInteractiveOptions : AzOptions
{
    [CliOption("--style")]
    public string? Style { get; set; }

    [CliOption("--update")]
    public string? Update { get; set; }
}