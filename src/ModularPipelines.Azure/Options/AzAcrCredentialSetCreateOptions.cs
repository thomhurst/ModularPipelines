using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("acr", "credential-set", "create")]
public record AzAcrCredentialSetCreateOptions(
[property: CliOption("--login-server")] string LoginServer,
[property: CliOption("--name")] string Name,
[property: CliOption("--password-id")] string PasswordId,
[property: CliOption("--registry")] string Registry,
[property: CliOption("--username-id")] string UsernameId
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}