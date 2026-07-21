using CliWrap;
using ModularPipelines.Options;

namespace ModularPipelines.Context;

internal static class CommandLineCredentialsMapper
{
    internal static Credentials ToCliWrapCredentials(this CommandLineCredentials credentials)
    {
        return new Credentials(
            credentials.Domain,
            credentials.UserName,
            credentials.Password,
            credentials.LoadUserProfile);
    }
}
