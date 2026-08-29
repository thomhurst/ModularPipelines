using ModularPipelines.FileSystem;

namespace ModularPipelines.Extensions;

/// <summary>
/// Extensions for folders.
/// </summary>
public static class FolderExtensions
{
    /// <summary>
    /// Converts the folders into string paths.
    /// </summary>
    /// <param name="folders">The folder collection.</param>
    /// <returns>The folders as paths.</returns>
    public static IEnumerable<string> AsPaths(this IEnumerable<FolderPath> folders) => folders.Select(f => f.Path);

    /// <summary>
    /// Converts the folders into string paths.
    /// </summary>
    /// <param name="folders">The folder collection.</param>
    /// <returns>The folders as paths.</returns>
    public static IReadOnlyList<string> AsPaths(this IList<FolderPath> folders) =>
        [.. folders.Select(f => f.Path)];

    /// <summary>
    /// Turns a nullable FolderPath object in a non-nullable FolderPath object if the folder exists.
    /// </summary>
    /// <param name="folder">The folder to check.</param>
    /// <returns>The input object if not null.</returns>
    public static FolderPath AssertExists(this FolderPath? folder)
    {
        if (folder == null)
        {
            throw new DirectoryNotFoundException("The folder does not exist");
        }

        if (!folder.Exists)
        {
            throw new DirectoryNotFoundException($"The folder does not exist: {folder.Path}");
        }

        return folder;
    }
}
