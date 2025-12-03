using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataprotection", "backup-policy", "trigger", "set")]
public record AzDataprotectionBackupPolicyTriggerSetOptions(
[property: CliOption("--policy")] string Policy,
[property: CliOption("--schedule")] string Schedule
) : AzOptions;