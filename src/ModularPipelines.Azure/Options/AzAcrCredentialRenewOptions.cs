using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "credential", "renew")]
public record AzAcrCredentialRenewOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--password-name")] string PasswordName
) : AzOptions
{
    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}