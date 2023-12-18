using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aosm", "nfd", "generate-config")]
public record AzAosmNfdGenerateConfigOptions(
[property: CommandSwitch("--definition-type")] string DefinitionType
) : AzOptions
{
    [CommandSwitch("--output-file")]
    public string? OutputFile { get; set; }
}