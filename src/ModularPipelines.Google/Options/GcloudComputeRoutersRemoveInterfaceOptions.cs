using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "routers", "remove-interface")]
public record GcloudComputeRoutersRemoveInterfaceOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--interface-name")] string InterfaceName,
[property: CommandSwitch("--interface-names")] string[] InterfaceNames
) : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }
}