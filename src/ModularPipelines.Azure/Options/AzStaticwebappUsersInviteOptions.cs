using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("staticwebapp", "users", "invite")]
public record AzStaticwebappUsersInviteOptions(
[property: CliOption("--authentication-provider")] string AuthenticationProvider,
[property: CliOption("--domain")] string Domain,
[property: CliOption("--invitation-expiration-in-hours")] string InvitationExpirationInHours,
[property: CliOption("--name")] string Name,
[property: CliOption("--roles")] string Roles,
[property: CliOption("--user-details")] string UserDetails
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}