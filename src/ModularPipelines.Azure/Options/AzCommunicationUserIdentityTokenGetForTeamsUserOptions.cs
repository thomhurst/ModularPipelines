using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("communication", "user-identity", "token", "get-for-teams-user")]
public record AzCommunicationUserIdentityTokenGetForTeamsUserOptions(
[property: CommandSwitch("--aad-token")] string AadToken,
[property: CommandSwitch("--aad-user")] string AadUser,
[property: CommandSwitch("--client")] string Client
) : AzOptions
{
    [CommandSwitch("--connection-string")]
    public string? ConnectionString { get; set; }
}