using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("webapp", "config", "backup", "delete")]
public record AzWebappConfigBackupDeleteOptions(
[property: CommandSwitch("--backup-id")] string BackupId,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--webapp-name")] string WebappName
) : AzOptions
{
    [CommandSwitch("--slot")]
    public string? Slot { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}