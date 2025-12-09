using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("staticwebapp", "users", "update")]
public record AzStaticwebappUsersUpdateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--roles")] string Roles
) : AzOptions
{
    [CliOption("--authentication-provider")]
    public string? AuthenticationProvider { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--user-details")]
    public string? UserDetails { get; set; }

    [CliOption("--user-id")]
    public string? UserId { get; set; }
}