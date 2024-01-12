using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("edge-cloud", "container", "get-server-config")]
public record GcloudEdgeCloudContainerGetServerConfigOptions : GcloudOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }
}