using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("communication", "user-identity", "token", "revoke")]
public record AzCommunicationUserIdentityTokenRevokeOptions(
[property: CommandSwitch("--user")] string User
) : AzOptions
{
    [CommandSwitch("--connection-string")]
    public string? ConnectionString { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}