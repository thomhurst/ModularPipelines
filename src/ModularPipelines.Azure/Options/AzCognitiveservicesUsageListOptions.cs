using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognitiveservices", "usage", "list")]
public record AzCognitiveservicesUsageListOptions(
[property: CommandSwitch("--location")] string Location
) : AzOptions
{
    [CommandSwitch("--filter")]
    public string? Filter { get; set; }
}