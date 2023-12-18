using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ad", "app", "federated-credential", "list")]
public record AzAdAppFederatedCredentialListOptions(
[property: CommandSwitch("--id")] string Id
) : AzOptions
{
}