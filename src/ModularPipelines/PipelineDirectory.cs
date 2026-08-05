using System.Runtime.CompilerServices;

namespace ModularPipelines;

/// <summary>
/// Locates conventional directories used by a pipeline.
/// </summary>
public static class PipelineDirectory
{
    private const string DirectoryVariable = "MODULAR_PIPELINES_DIRECTORY";

    /// <summary>
    /// Finds the pipeline project containing <c>appsettings.json</c> and a project file.
    /// </summary>
    /// <param name="sourceFilePath">The calling source file path, supplied by the compiler.</param>
    /// <returns>The absolute pipeline project directory.</returns>
    /// <exception cref="InvalidOperationException">The pipeline project cannot be located.</exception>
    public static string FindPipelineProject([CallerFilePath] string sourceFilePath = "") =>
        TryFindPipelineProject(sourceFilePath)
        ?? throw new InvalidOperationException(
            $"Could not locate the pipeline project directory. Set {DirectoryVariable} to its path.");

    /// <summary>
    /// Finds the nearest Git repository root.
    /// </summary>
    /// <param name="sourceFilePath">The calling source file path, supplied by the compiler.</param>
    /// <returns>The absolute Git repository root.</returns>
    /// <exception cref="InvalidOperationException">A Git repository root cannot be located.</exception>
    public static string FindGitRoot([CallerFilePath] string sourceFilePath = "") =>
        FindAncestor(GetSearchDirectory(sourceFilePath), IsGitRoot)
        ?? throw new InvalidOperationException("Could not locate a Git repository root.");

    internal static string? TryFindPipelineProject(string sourceFilePath)
    {
        var configuredDirectory = Environment.GetEnvironmentVariable(DirectoryVariable);
        if (!string.IsNullOrWhiteSpace(configuredDirectory))
        {
            var fullPath = Path.GetFullPath(configuredDirectory);
            return IsPipelineProject(fullPath)
                ? fullPath
                : throw new InvalidOperationException(
                    $"{DirectoryVariable} must point to a directory containing appsettings.json and a project file.");
        }

        var sourceDirectory = Path.GetDirectoryName(sourceFilePath);
        var sourceProject = Directory.Exists(sourceDirectory)
            ? FindAncestor(sourceDirectory!, IsPipelineProject)
            : null;

        return sourceProject ?? FindAncestor(AppContext.BaseDirectory, IsPipelineProject);
    }

    private static string GetSearchDirectory(string sourceFilePath)
    {
        var sourceDirectory = Path.GetDirectoryName(sourceFilePath);
        return Directory.Exists(sourceDirectory)
            ? sourceDirectory!
            : Directory.GetCurrentDirectory();
    }

    private static string? FindAncestor(string startDirectory, Func<string, bool> predicate)
    {
        for (var directory = new DirectoryInfo(startDirectory);
             directory is not null;
             directory = directory.Parent)
        {
            if (predicate(directory.FullName))
            {
                return directory.FullName;
            }
        }

        return null;
    }

    private static bool IsPipelineProject(string? directory) =>
        directory is not null
        && Directory.Exists(directory)
        && File.Exists(Path.Combine(directory, "appsettings.json"))
        && Directory.EnumerateFiles(directory, "*.csproj").Any();

    private static bool IsGitRoot(string directory) =>
        Directory.Exists(Path.Combine(directory, ".git"))
        || File.Exists(Path.Combine(directory, ".git"));
}
