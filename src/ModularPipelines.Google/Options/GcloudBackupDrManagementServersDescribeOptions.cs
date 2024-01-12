using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup-dr", "management-servers", "describe")]
public record GcloudBackupDrManagementServersDescribeOptions(
[property: PositionalArgument] string ManagementServer,
[property: PositionalArgument] string Location
) : GcloudOptions;