using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataprotection", "backup-policy", "tag", "set")]
public record AzDataprotectionBackupPolicyTagSetOptions(
[property: CommandSwitch("--criteria")] string Criteria,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--policy")] string Policy
) : AzOptions;