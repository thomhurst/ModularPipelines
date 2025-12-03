using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ad", "app", "federated-credential", "list")]
public record AzAdAppFederatedCredentialListOptions(
[property: CliOption("--id")] string Id
) : AzOptions;