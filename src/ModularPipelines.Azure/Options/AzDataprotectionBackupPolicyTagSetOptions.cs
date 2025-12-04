using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dataprotection", "backup-policy", "tag", "set")]
public record AzDataprotectionBackupPolicyTagSetOptions(
[property: CliOption("--criteria")] string Criteria,
[property: CliOption("--name")] string Name,
[property: CliOption("--policy")] string Policy
) : AzOptions;