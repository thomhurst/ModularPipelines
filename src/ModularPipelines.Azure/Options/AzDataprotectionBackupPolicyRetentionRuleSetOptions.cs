using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dataprotection", "backup-policy", "retention-rule", "set")]
public record AzDataprotectionBackupPolicyRetentionRuleSetOptions(
[property: CliOption("--lifecycles")] string Lifecycles,
[property: CliOption("--name")] string Name,
[property: CliOption("--policy")] string Policy
) : AzOptions;