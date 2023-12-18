using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "central", "device-template", "list")]
public record AzIotCentralDeviceTemplateListOptions(
[property: CommandSwitch("--app-id")] string AppId
) : AzOptions
{
    [CommandSwitch("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [BooleanCommandSwitch("--compact")]
    public bool? Compact { get; set; }

    [CommandSwitch("--token")]
    public string? Token { get; set; }
}