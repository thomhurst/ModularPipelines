using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ad", "app", "federated-credential", "delete")]
public record AzAdAppFederatedCredentialDeleteOptions(
[property: CliOption("--federated-credential-id")] string FederatedCredentialId,
[property: CliOption("--id")] string Id
) : AzOptions;