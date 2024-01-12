using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "routers", "delete")]
public record GcloudComputeRoutersDeleteOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }
}