using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("cognitiveservices", "model", "list")]
public record AzCognitiveservicesModelListOptions(
[property: CliOption("--location")] string Location
) : AzOptions;