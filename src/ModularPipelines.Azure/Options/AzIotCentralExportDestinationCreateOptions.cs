using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "central", "export", "destination", "create")]
public record AzIotCentralExportDestinationCreateOptions(
[property: CliOption("--app-id")] string AppId,
[property: CliOption("--dest-id")] string DestId,
[property: CliOption("--display-name")] string DisplayName,
[property: CliOption("--type")] string Type
) : AzOptions
{
    [CliOption("--au")]
    public string? Au { get; set; }

    [CliOption("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CliOption("--cluster-url")]
    public string? ClusterUrl { get; set; }

    [CliOption("--database")]
    public string? Database { get; set; }

    [CliOption("--header")]
    public string? Header { get; set; }

    [CliOption("--table")]
    public string? Table { get; set; }

    [CliOption("--token")]
    public string? Token { get; set; }

    [CliOption("--url")]
    public string? Url { get; set; }
}