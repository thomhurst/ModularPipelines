using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("metastore", "services", "import", "gcs")]
public record GcloudMetastoreServicesImportGcsOptions(
[property: CliArgument] string Service,
[property: CliArgument] string Location,
[property: CliOption("--database-dump")] string DatabaseDump,
[property: CliOption("--import-id")] string ImportId
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--dump-type")]
    public string? DumpType { get; set; }
}