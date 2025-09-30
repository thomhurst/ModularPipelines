using Microsoft.Extensions.Logging;

namespace ModularPipelines.Distributed.Serialization;

/// <summary>
/// Handles serialization of minimal context information for remote execution.
/// </summary>
internal sealed class ContextSerializer
{
    private readonly ILogger<ContextSerializer> _logger;

    public ContextSerializer(ILogger<ContextSerializer> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Extracts environment variables from the current process.
    /// </summary>
    /// <param name="includePatterns">Patterns of environment variables to include (case-insensitive).</param>
    /// <returns>A dictionary of environment variables.</returns>
    public Dictionary<string, string> ExtractEnvironmentVariables(params string[] includePatterns)
    {
        var variables = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var allVariables = Environment.GetEnvironmentVariables();

        foreach (var key in allVariables.Keys)
        {
            var keyString = key?.ToString();
            if (string.IsNullOrEmpty(keyString))
            {
                continue;
            }

            // If no patterns specified, include common CI/build-related variables
            if (includePatterns.Length == 0)
            {
                if (IsCommonBuildVariable(keyString))
                {
                    var value = allVariables[key]?.ToString();
                    if (value != null)
                    {
                        variables[keyString] = value;
                    }
                }
            }
            else
            {
                // Check if key matches any include pattern
                foreach (var pattern in includePatterns)
                {
                    if (keyString.Contains(pattern, StringComparison.OrdinalIgnoreCase))
                    {
                        var value = allVariables[key]?.ToString();
                        if (value != null)
                        {
                            variables[keyString] = value;
                        }
                        break;
                    }
                }
            }
        }

        _logger.LogDebug(
            "Extracted {Count} environment variables for remote execution",
            variables.Count);

        return variables;
    }

    /// <summary>
    /// Gets the current working directory.
    /// </summary>
    /// <returns>The current working directory path.</returns>
    public string GetWorkingDirectory()
    {
        return Directory.GetCurrentDirectory();
    }

    /// <summary>
    /// Applies environment variables to the current process.
    /// </summary>
    /// <param name="variables">The environment variables to apply.</param>
    public void ApplyEnvironmentVariables(Dictionary<string, string> variables)
    {
        ArgumentNullException.ThrowIfNull(variables);

        foreach (var (key, value) in variables)
        {
            Environment.SetEnvironmentVariable(key, value);
        }

        _logger.LogDebug(
            "Applied {Count} environment variables",
            variables.Count);
    }

    private static bool IsCommonBuildVariable(string key)
    {
        // Common CI/CD and build-related environment variables
        var patterns = new[]
        {
            "CI",
            "BUILD",
            "GITHUB",
            "AZURE",
            "AWS",
            "PIPELINE",
            "RUNNER",
            "AGENT",
            "PATH",
            "HOME",
            "TEMP",
            "TMP",
            "DOTNET",
            "NUGET",
            "NODE",
            "JAVA",
        };

        return patterns.Any(pattern => key.Contains(pattern, StringComparison.OrdinalIgnoreCase));
    }
}
