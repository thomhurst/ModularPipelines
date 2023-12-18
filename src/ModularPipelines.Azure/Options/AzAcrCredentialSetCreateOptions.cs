using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "credential-set", "create")]
public record AzAcrCredentialSetCreateOptions(
[property: CommandSwitch("--login-server")] string LoginServer,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--password-id")] string PasswordId,
[property: CommandSwitch("--registry")] string Registry,
[property: CommandSwitch("--username-id")] string UsernameId
) : AzOptions
{
    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}