using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ad", "app", "federated-credential", "create")]
public record AzAdAppFederatedCredentialCreateOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--parameters")] string[] Parameters
) : AzOptions
{
}

