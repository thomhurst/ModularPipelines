using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("communication", "identity", "token", "get-for-teams-user")]
public record AzCommunicationIdentityTokenGetForTeamsUserOptions(
[property: CommandSwitch("--aad-token")] string AadToken,
[property: CommandSwitch("--aad-user")] string AadUser,
[property: CommandSwitch("--client")] string Client
) : AzOptions
{
    [CommandSwitch("--connection-string")]
    public string? ConnectionString { get; set; }
}