using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "central", "export", "create")]
public record AzIotCentralExportCreateOptions(
[property: CommandSwitch("--app-id")] string AppId,
[property: CommandSwitch("--destinations")] string Destinations,
[property: CommandSwitch("--display-name")] string DisplayName,
[property: CommandSwitch("--export-id")] string ExportId,
[property: CommandSwitch("--source")] string Source
) : AzOptions
{
    [CommandSwitch("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CommandSwitch("--en")]
    public string? En { get; set; }

    [BooleanCommandSwitch("--enabled")]
    public bool? Enabled { get; set; }

    [CommandSwitch("--filter")]
    public string? Filter { get; set; }

    [CommandSwitch("--token")]
    public string? Token { get; set; }
}