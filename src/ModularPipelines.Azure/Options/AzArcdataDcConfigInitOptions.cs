using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("arcdata", "dc", "config", "init")]
public record AzArcdataDcConfigInitOptions : AzOptions
{
    [CliFlag("--force")]
    public bool? Force { get; set; }

    [CliOption("--path")]
    public string? Path { get; set; }

    [CliOption("--source")]
    public string? Source { get; set; }
}