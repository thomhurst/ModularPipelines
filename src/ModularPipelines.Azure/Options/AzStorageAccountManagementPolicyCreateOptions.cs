using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "account", "management-policy", "create")]
public record AzStorageAccountManagementPolicyCreateOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--policy")] string Policy,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;