using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("identity", "federated-credential", "show")]
public record AzIdentityFederatedCredentialShowOptions(
[property: CliOption("--identity-name")] string IdentityName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;