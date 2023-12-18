using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("internet-analyzer", "test", "update")]
public record AzInternetAnalyzerTestUpdateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--profile-name")] string ProfileName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--enabled-state")]
    public bool? EnabledState { get; set; }

    [CommandSwitch("--endpoint-a-endpoint")]
    public string? EndpointAEndpoint { get; set; }

    [CommandSwitch("--endpoint-a-name")]
    public string? EndpointAName { get; set; }

    [CommandSwitch("--endpoint-b-endpoint")]
    public string? EndpointBEndpoint { get; set; }

    [CommandSwitch("--endpoint-b-name")]
    public string? EndpointBName { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}