using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("metastore", "services", "export", "gcs")]
public record GcloudMetastoreServicesExportGcsOptions(
[property: CliArgument] string Service,
[property: CliArgument] string Location,
[property: CliOption("--destination-folder")] string DestinationFolder
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--dump-type")]
    public string? DumpType { get; set; }
}