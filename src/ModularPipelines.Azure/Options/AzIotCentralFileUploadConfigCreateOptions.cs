using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "central", "file-upload-config", "create")]
public record AzIotCentralFileUploadConfigCreateOptions(
[property: CliOption("--app-id")] string AppId,
[property: CliOption("--connection-string")] string ConnectionString,
[property: CliOption("--container")] string Container
) : AzOptions
{
    [CliOption("--account")]
    public int? Account { get; set; }

    [CliOption("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CliOption("--sas-ttl")]
    public string? SasTtl { get; set; }

    [CliOption("--token")]
    public string? Token { get; set; }
}