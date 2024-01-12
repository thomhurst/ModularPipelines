using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("edge-cloud", "container", "machines", "list")]
public record GcloudEdgeCloudContainerMachinesListOptions : GcloudOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }
}