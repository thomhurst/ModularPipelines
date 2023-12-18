using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cdn", "origin-group", "create")]
public record AzCdnOriginGroupCreateOptions(
[property: CommandSwitch("--endpoint-name")] string EndpointName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--profile-name")] string ProfileName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--origins")]
    public string? Origins { get; set; }

    [CommandSwitch("--probe-interval")]
    public string? ProbeInterval { get; set; }

    [CommandSwitch("--probe-method")]
    public string? ProbeMethod { get; set; }

    [CommandSwitch("--probe-path")]
    public string? ProbePath { get; set; }

    [CommandSwitch("--probe-protocol")]
    public string? ProbeProtocol { get; set; }
}