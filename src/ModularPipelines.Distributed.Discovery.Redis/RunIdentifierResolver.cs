using System.Diagnostics;

namespace ModularPipelines.Distributed.Discovery.Redis;

/// <summary>
/// Resolves a run identifier for isolating concurrent pipeline runs in Redis.
/// </summary>
internal static class RunIdentifierResolver
{
    private static readonly string[] CiEnvironmentVariables =
    [
        "GITHUB_SHA",
        "BUILD_SOURCEVERSION",
        "CI_COMMIT_SHA",
    ];

    public static string Resolve(string? configured)
    {
        if (!string.IsNullOrWhiteSpace(configured))
        {
            return configured;
        }

        foreach (var environmentVariable in CiEnvironmentVariables)
        {
            var value = Environment.GetEnvironmentVariable(environmentVariable);
            if (!string.IsNullOrWhiteSpace(value))
            {
                return value;
            }
        }

        return TryGetGitSha() ?? Guid.NewGuid().ToString("N");
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
