using System.Diagnostics;

namespace ModularPipelines.Distributed.Redis.Configuration;

/// <summary>
/// Resolves the run identifier for Redis key isolation.
/// Priority: explicit config > GITHUB_SHA > BUILD_SOURCEVERSION > CI_COMMIT_SHA > git rev-parse HEAD > GUID fallback.
/// </summary>
internal static class RunIdentifierResolver
{
    private static readonly string[] CiEnvironmentVariables =
    [
        "GITHUB_SHA",
        "BUILD_SOURCEVERSION",
        "CI_COMMIT_SHA",
    ];

    public static string Resolve(string? explicitValue)
    {
        if (!string.IsNullOrWhiteSpace(explicitValue))
        {
            return explicitValue;
        }

        foreach (var envVar in CiEnvironmentVariables)
        {
            var value = Environment.GetEnvironmentVariable(envVar);
            if (!string.IsNullOrWhiteSpace(value))
            {
                return value;
            }
        }

        var gitSha = TryGetGitSha();
        if (!string.IsNullOrWhiteSpace(gitSha))
        {
            return gitSha;
        }

        return Guid.NewGuid().ToString("N");
    }

    private static string? TryGetGitSha()
    {
        try
        {
            using var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "git",
                    Arguments = "rev-parse HEAD",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                },
            };

            process.Start();
            var output = process.StandardOutput.ReadToEnd().Trim();
            process.WaitForExit(5000);

            return process.ExitCode == 0 && !string.IsNullOrWhiteSpace(output)
                ? output
                : null;
        }
        catch
        {
            return null;
        }
    }
}
