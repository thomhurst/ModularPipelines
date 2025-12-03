using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("identity", "federated-credential", "create")]
public record AzIdentityFederatedCredentialCreateOptions(
[property: CliOption("--identity-name")] string IdentityName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--audiences")]
    public string? Audiences { get; set; }

    [CliOption("--issuer")]
    public string? Issuer { get; set; }

    [CliOption("--subject")]
    public string? Subject { get; set; }
}