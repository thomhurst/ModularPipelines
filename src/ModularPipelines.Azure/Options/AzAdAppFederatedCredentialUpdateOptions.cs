using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ad", "app", "federated-credential", "update")]
public record AzAdAppFederatedCredentialUpdateOptions(
[property: CliOption("--federated-credential-id")] string FederatedCredentialId,
[property: CliOption("--id")] string Id,
[property: CliOption("--parameters")] string[] Parameters
) : AzOptions;