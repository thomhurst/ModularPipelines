using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devcenter", "dev", "project", "show")]
public record AzDevcenterDevProjectShowOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--dev-center")]
    public string? DevCenter { get; set; }

    [CommandSwitch("--endpoint")]
    public string? Endpoint { get; set; }
}