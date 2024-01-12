using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "bare-metal", "clusters", "list")]
public record GcloudContainerBareMetalClustersListOptions : GcloudOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }
}