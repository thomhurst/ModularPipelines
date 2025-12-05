using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("metastore", "services", "backups", "delete")]
public record GcloudMetastoreServicesBackupsDeleteOptions(
[property: CliArgument] string Backup,
[property: CliArgument] string Location,
[property: CliArgument] string Service
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}