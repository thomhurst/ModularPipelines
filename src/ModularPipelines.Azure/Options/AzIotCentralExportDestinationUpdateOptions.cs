using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "central", "export", "destination", "update")]
public record AzIotCentralExportDestinationUpdateOptions(
[property: CommandSwitch("--app-id")] string AppId,
[property: CommandSwitch("--content")] string Content,
[property: CommandSwitch("--dest-id")] string DestId
) : AzOptions
{
    [CommandSwitch("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CommandSwitch("--token")]
    public string? Token { get; set; }
}