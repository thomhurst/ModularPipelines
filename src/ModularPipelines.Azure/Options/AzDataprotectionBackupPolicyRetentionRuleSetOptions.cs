using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataprotection", "backup-policy", "retention-rule", "set")]
public record AzDataprotectionBackupPolicyRetentionRuleSetOptions(
[property: CommandSwitch("--lifecycles")] string Lifecycles,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--policy")] string Policy
) : AzOptions
{
}

