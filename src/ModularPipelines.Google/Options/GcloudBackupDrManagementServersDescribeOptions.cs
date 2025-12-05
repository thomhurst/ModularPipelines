using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup-dr", "management-servers", "describe")]
public record GcloudBackupDrManagementServersDescribeOptions(
[property: CliArgument] string ManagementServer,
[property: CliArgument] string Location
) : GcloudOptions;