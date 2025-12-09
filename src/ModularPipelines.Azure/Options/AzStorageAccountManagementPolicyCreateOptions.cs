using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("storage", "account", "management-policy", "create")]
public record AzStorageAccountManagementPolicyCreateOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--policy")] string Policy,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;