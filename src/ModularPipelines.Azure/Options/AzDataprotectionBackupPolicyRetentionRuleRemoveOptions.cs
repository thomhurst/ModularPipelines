using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dataprotection", "backup-policy", "retention-rule", "remove")]
public record AzDataprotectionBackupPolicyRetentionRuleRemoveOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--policy")] string Policy
) : AzOptions;