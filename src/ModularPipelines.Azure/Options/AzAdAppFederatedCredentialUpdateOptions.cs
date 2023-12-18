using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ad", "app", "federated-credential", "update")]
public record AzAdAppFederatedCredentialUpdateOptions(
[property: CommandSwitch("--federated-credential-id")] string FederatedCredentialId,
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--parameters")] string[] Parameters
) : AzOptions;