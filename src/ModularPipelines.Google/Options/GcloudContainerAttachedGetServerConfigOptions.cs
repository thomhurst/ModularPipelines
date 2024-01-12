using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "attached", "get-server-config")]
public record GcloudContainerAttachedGetServerConfigOptions : GcloudOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }
}