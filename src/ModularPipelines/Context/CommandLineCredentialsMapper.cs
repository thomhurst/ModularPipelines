using System.Diagnostics;
using ModularPipelines.Options;

namespace ModularPipelines.Context;

internal static class CommandLineCredentialsMapper
{
    internal static void Apply(CommandLineCredentials credentials, ProcessStartInfo startInfo)
    {
        if (credentials.UserName is not null)
        {
            startInfo.UserName = credentials.UserName;
        }

        var hasWindowsOnlyCredentials = credentials.Domain is not null
                                        || credentials.Password is not null
                                        || credentials.LoadUserProfile;
        if (!hasWindowsOnlyCredentials)
        {
            return;
        }

        if (!OperatingSystem.IsWindows())
        {
            throw new PlatformNotSupportedException(
                "Cannot start a process using the provided credentials. Setting a custom domain or password, "
                + "or loading the user profile, is only supported on Windows.");
        }

        if (credentials.Domain is not null)
        {
            startInfo.Domain = credentials.Domain;
        }

        if (credentials.Password is not null)
        {
            startInfo.PasswordInClearText = credentials.Password;
        }

        startInfo.LoadUserProfile = credentials.LoadUserProfile;
    }
}
