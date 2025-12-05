using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup-dr", "management-servers", "delete")]
public record GcloudBackupDrManagementServersDeleteOptions(
[property: CliArgument] string ManagementServer,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}