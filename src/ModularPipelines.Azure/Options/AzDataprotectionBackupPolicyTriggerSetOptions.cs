using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataprotection", "backup-policy", "trigger", "set")]
public record AzDataprotectionBackupPolicyTriggerSetOptions(
[property: CommandSwitch("--policy")] string Policy,
[property: CommandSwitch("--schedule")] string Schedule
) : AzOptions;