using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ad", "app", "federated-credential", "show")]
public record AzAdAppFederatedCredentialShowOptions(
[property: CliOption("--federated-credential-id")] string FederatedCredentialId,
[property: CliOption("--id")] string Id
) : AzOptions;