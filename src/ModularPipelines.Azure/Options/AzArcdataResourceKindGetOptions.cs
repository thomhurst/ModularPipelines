using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("arcdata", "resource-kind", "get")]
public record AzArcdataResourceKindGetOptions(
[property: CommandSwitch("--kind")] string Kind
) : AzOptions
{
    [CommandSwitch("--dest")]
    public string? Dest { get; set; }
}