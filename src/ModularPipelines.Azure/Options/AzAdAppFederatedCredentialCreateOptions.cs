using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ad", "app", "federated-credential", "create")]
public record AzAdAppFederatedCredentialCreateOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--parameters")] string[] Parameters
) : AzOptions;