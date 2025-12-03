using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("acr", "credential", "renew")]
public record AzAcrCredentialRenewOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--password-name")] string PasswordName
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}