using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("acr", "task", "credential", "update")]
public record AzAcrTaskCredentialUpdateOptions(
[property: CliOption("--login-server")] string LoginServer,
[property: CliOption("--name")] string Name,
[property: CliOption("--registry")] string Registry
) : AzOptions
{
    [CliOption("--password")]
    public string? Password { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliFlag("--use-identity")]
    public bool? UseIdentity { get; set; }

    [CliOption("--username")]
    public string? Username { get; set; }
}