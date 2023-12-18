using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("group", "delete")]
public record AzGroupDeleteOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [BooleanCommandSwitch("--force-deletion-types")]
    public bool? ForceDeletionTypes { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}