using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataprotection", "backup-policy", "tag", "remove")]
public record AzDataprotectionBackupPolicyTagRemoveOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--policy")] string Policy
) : AzOptions;