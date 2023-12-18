using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("arcdata", "dc", "config", "remove")]
public record AzArcdataDcConfigRemoveOptions(
[property: CommandSwitch("--json-path")] string JsonPath,
[property: CommandSwitch("--path")] string Path
) : AzOptions
{
}