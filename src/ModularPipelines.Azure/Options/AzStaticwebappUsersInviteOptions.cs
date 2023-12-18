using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("staticwebapp", "users", "invite")]
public record AzStaticwebappUsersInviteOptions(
[property: CommandSwitch("--authentication-provider")] string AuthenticationProvider,
[property: CommandSwitch("--domain")] string Domain,
[property: CommandSwitch("--invitation-expiration-in-hours")] string InvitationExpirationInHours,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--roles")] string Roles,
[property: CommandSwitch("--user-details")] string UserDetails
) : AzOptions
{
    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}