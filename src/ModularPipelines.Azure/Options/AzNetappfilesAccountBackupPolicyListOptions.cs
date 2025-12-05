using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("netappfiles", "account", "backup-policy", "list")]
public record AzNetappfilesAccountBackupPolicyListOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;