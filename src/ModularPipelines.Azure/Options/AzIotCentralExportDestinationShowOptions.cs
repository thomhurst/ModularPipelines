using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "central", "export", "destination", "show")]
public record AzIotCentralExportDestinationShowOptions(
[property: CommandSwitch("--app-id")] string AppId,
[property: CommandSwitch("--dest-id")] string DestId
) : AzOptions
{
    [CommandSwitch("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CommandSwitch("--token")]
    public string? Token { get; set; }
}