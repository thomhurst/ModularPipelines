using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "hub", "features", "list")]
public record GcloudContainerHubFeaturesListOptions : GcloudOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }
}