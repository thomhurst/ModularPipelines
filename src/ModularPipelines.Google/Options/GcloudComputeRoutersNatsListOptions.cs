using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "routers", "nats", "list")]
public record GcloudComputeRoutersNatsListOptions(
[property: CommandSwitch("--router")] string Router
) : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }
}