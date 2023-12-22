using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognitiveservices", "model", "list")]
public record AzCognitiveservicesModelListOptions(
[property: CommandSwitch("--location")] string Location
) : AzOptions;