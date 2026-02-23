namespace ModularPipelines.Distributed.Serialization;

/// <summary>
/// Finds the git repository root by walking up the directory tree.
/// Used by portable path converters for cross-platform path serialization.
/// </summary>
internal static class GitRootFinder
{
    public static string? Find(string? startingDirectory = null)
    {
        var directory = startingDirectory ?? Environment.CurrentDirectory;

        while (!string.IsNullOrEmpty(directory))
        {
            if (Directory.Exists(Path.Combine(directory, ".git")))
            {
                return directory;
            }

            var parent = Directory.GetParent(directory)?.FullName;

            if (parent == directory)
            {
                break;
            }

            directory = parent;
        }

        return null;
    }
}
