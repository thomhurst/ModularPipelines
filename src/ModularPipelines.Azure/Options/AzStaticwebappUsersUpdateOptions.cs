using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("staticwebapp", "users", "update")]
public record AzStaticwebappUsersUpdateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--roles")] string Roles
) : AzOptions
{
    [CommandSwitch("--authentication-provider")]
    public string? AuthenticationProvider { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--user-details")]
    public string? UserDetails { get; set; }

    [CommandSwitch("--user-id")]
    public string? UserId { get; set; }
}