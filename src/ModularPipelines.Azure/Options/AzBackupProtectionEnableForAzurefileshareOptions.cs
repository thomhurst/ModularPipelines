using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("backup", "protection", "enable-for-urefileshare")]
public record AzBackupProtectionEnableForAzurefileshareOptions(
[property: CliOption("--azure-file-share")] string AzureFileShare,
[property: CliOption("--policy-name")] string PolicyName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--storage-account")] int StorageAccount,
[property: CliOption("--vault-name")] string VaultName
) : AzOptions;