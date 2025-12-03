using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognitiveservices", "model", "list")]
public record AzCognitiveservicesModelListOptions(
[property: CliOption("--location")] string Location
) : AzOptions;