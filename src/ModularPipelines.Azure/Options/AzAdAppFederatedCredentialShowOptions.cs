using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ad", "app", "federated-credential", "show")]
public record AzAdAppFederatedCredentialShowOptions(
[property: CommandSwitch("--federated-credential-id")] string FederatedCredentialId,
[property: CommandSwitch("--id")] string Id
) : AzOptions;