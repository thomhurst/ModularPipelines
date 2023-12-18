using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataprotection", "backup-policy", "retention-rule", "remove")]
public record AzDataprotectionBackupPolicyRetentionRuleRemoveOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--policy")] string Policy
) : AzOptions;