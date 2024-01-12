using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup-dr", "operations", "list")]
public record GcloudBackupDrOperationsListOptions(
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;