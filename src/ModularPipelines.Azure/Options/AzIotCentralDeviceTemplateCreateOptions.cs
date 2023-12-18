using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "central", "device-template", "create")]
public record AzIotCentralDeviceTemplateCreateOptions(
[property: CommandSwitch("--app-id")] string AppId,
[property: CommandSwitch("--content")] string Content,
[property: CommandSwitch("--device-template-id")] string DeviceTemplateId
) : AzOptions
{
    [CommandSwitch("--api-version")]
    public string? ApiVersion { get; set; }

    [CommandSwitch("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CommandSwitch("--token")]
    public string? Token { get; set; }
}

