using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("cognitiveservices", "usage", "list")]
public record AzCognitiveservicesUsageListOptions(
[property: CliOption("--location")] string Location
) : AzOptions
{
    [CliOption("--filter")]
    public string? Filter { get; set; }
}