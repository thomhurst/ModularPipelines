using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognitiveservices", "model", "list")]
public record AzCognitiveservicesModelListOptions(
[property: CommandSwitch("--location")] string Location
) : AzOptions
{
}

