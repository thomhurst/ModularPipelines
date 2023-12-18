using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("identity", "federated-credential", "list")]
public record AzIdentityFederatedCredentialListOptions(
[property: CommandSwitch("--identity-name")] string IdentityName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;