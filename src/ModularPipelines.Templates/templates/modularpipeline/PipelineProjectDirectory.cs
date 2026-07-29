using System.Runtime.CompilerServices;

namespace TemplatePipeline;

internal static class PipelineProjectDirectory
{
    private const string DirectoryVariable = "MODULAR_PIPELINES_DIRECTORY";

    public static string Find([CallerFilePath] string sourceFilePath = "")
    {
        var configuredDirectory = Environment.GetEnvironmentVariable(DirectoryVariable);
        if (!string.IsNullOrWhiteSpace(configuredDirectory))
        {
            return ValidateConfiguredDirectory(configuredDirectory);
        }

        var sourceDirectory = Path.GetDirectoryName(sourceFilePath);
        return IsPipelineDirectory(sourceDirectory)
            ? sourceDirectory!
            : FindFromBuildOutput();
    }

    private static string ValidateConfiguredDirectory(string configuredDirectory)
    {
        var fullPath = Path.GetFullPath(configuredDirectory);
        return IsPipelineDirectory(fullPath)
            ? fullPath
            : throw new InvalidOperationException(
                $"{DirectoryVariable} must point to a directory containing appsettings.json and a project file.");
    }

    private static string FindFromBuildOutput()
    {
        for (var directory = new DirectoryInfo(AppContext.BaseDirectory);
             directory is not null;
             directory = directory.Parent)
        {
            if (IsPipelineDirectory(directory.FullName))
            {
                return directory.FullName;
            }
        }

        throw new InvalidOperationException(
            $"Could not locate the pipeline project directory. Set {DirectoryVariable} to its path.");
    }

    private static bool IsPipelineDirectory(string? directory) =>
        directory is not null
        && Directory.Exists(directory)
        && File.Exists(Path.Combine(directory, "appsettings.json"))
        && Directory.EnumerateFiles(directory, "*.csproj").Any();
}
