using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup-dr", "management-servers", "create")]
public record GcloudBackupDrManagementServersCreateOptions(
[property: CliArgument] string ManagementServer,
[property: CliArgument] string Location,
[property: CliOption("--network")] string Network
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}