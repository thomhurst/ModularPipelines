using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("communication")]
public class AzCommunicationUserIdentity
{
    public AzCommunicationUserIdentity(
        AzCommunicationUserIdentityToken token,
        AzCommunicationUserIdentityUser user
    )
    {
        Token = token;
        User = user;
    }

    public AzCommunicationUserIdentityToken Token { get; }

    public AzCommunicationUserIdentityUser User { get; }
}