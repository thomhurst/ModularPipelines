using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup-dr", "management-servers", "create")]
public record GcloudBackupDrManagementServersCreateOptions(
[property: PositionalArgument] string ManagementServer,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--network")] string Network
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}