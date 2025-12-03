using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("metastore", "services", "restore")]
public record GcloudMetastoreServicesRestoreOptions(
[property: CliArgument] string Service,
[property: CliArgument] string Location,
[property: CliOption("--backup")] string Backup
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--restore-type")]
    public string? RestoreType { get; set; }
}