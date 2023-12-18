using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "central", "export", "destination", "create")]
public record AzIotCentralExportDestinationCreateOptions(
[property: CommandSwitch("--app-id")] string AppId,
[property: CommandSwitch("--dest-id")] string DestId,
[property: CommandSwitch("--display-name")] string DisplayName,
[property: CommandSwitch("--type")] string Type
) : AzOptions
{
    [CommandSwitch("--api-version")]
    public string? ApiVersion { get; set; }

    [CommandSwitch("--au")]
    public string? Au { get; set; }

    [CommandSwitch("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CommandSwitch("--cluster-url")]
    public string? ClusterUrl { get; set; }

    [CommandSwitch("--database")]
    public string? Database { get; set; }

    [CommandSwitch("--header")]
    public string? Header { get; set; }

    [CommandSwitch("--table")]
    public string? Table { get; set; }

    [CommandSwitch("--token")]
    public string? Token { get; set; }

    [CommandSwitch("--url")]
    public string? Url { get; set; }
}