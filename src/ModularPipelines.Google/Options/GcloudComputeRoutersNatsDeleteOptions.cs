using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "routers", "nats", "delete")]
public record GcloudComputeRoutersNatsDeleteOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--router")] string Router
) : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }
}