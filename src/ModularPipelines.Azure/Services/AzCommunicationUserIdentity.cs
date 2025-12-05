using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("communication")]
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