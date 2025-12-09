using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dataprotection", "backup-policy", "tag", "create-absolute-criteria")]
public record AzDataprotectionBackupPolicyTagCreateAbsoluteCriteriaOptions(
[property: CliOption("--absolute-criteria")] string AbsoluteCriteria
) : AzOptions;