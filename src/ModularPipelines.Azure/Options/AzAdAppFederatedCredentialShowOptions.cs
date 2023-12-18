using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ad", "app", "federated-credential", "show")]
public record AzAdAppFederatedCredentialShowOptions(
[property: CommandSwitch("--federated-credential-id")] string FederatedCredentialId,
[property: CommandSwitch("--id")] string Id
) : AzOptions
{
}