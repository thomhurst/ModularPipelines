using ModularPipelines.FileSystem;

namespace ModularPipelines.Extensions;

/// <summary>
/// Extensions for folders
/// </summary>
public static class FolderExtensions
{
    /// <summary>
    /// Converts the folders into string paths.
    /// </summary>
    /// <param name="folders">The folder collection.</param>
    /// <returns>The folders as paths.</returns>
    public static IEnumerable<string> AsPaths(this IEnumerable<Folder> folders) => folders.Select(f => f.Path);

    /// <summary>
    /// Converts the folders into string paths.
    /// </summary>
    /// <param name="folders">The folder collection.</param>
    /// <returns>The folders as paths.</returns>
    public static List<string> AsPaths(this IList<Folder> folders) => folders.Select(f => f.Path).ToList();
}