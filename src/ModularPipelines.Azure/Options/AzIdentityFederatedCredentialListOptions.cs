using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("identity", "federated-credential", "list")]
public record AzIdentityFederatedCredentialListOptions(
[property: CliOption("--identity-name")] string IdentityName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;