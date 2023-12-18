using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("identity", "federated-credential", "update")]
public record AzIdentityFederatedCredentialUpdateOptions(
[property: CommandSwitch("--identity-name")] string IdentityName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--audiences")]
    public string? Audiences { get; set; }

    [CommandSwitch("--issuer")]
    public string? Issuer { get; set; }

    [CommandSwitch("--subject")]
    public string? Subject { get; set; }
}