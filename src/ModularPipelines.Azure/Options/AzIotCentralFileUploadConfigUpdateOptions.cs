using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "central", "file-upload-config", "update")]
public record AzIotCentralFileUploadConfigUpdateOptions(
[property: CliOption("--app-id")] string AppId
) : AzOptions
{
    [CliOption("--account")]
    public int? Account { get; set; }

    [CliOption("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliOption("--container")]
    public string? Container { get; set; }

    [CliOption("--sas-ttl")]
    public string? SasTtl { get; set; }

    [CliOption("--token")]
    public string? Token { get; set; }
}