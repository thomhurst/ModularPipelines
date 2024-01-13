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
    public static IEnumerable<string> AsPaths(this IEnumerable<Folder> folders) => folders.Select(f => f.Path);

    /// <summary>
    /// Converts the folders into string paths.
    /// </summary>
    /// <param name="folders">The folder collection.</param>
    /// <returns>The folders as paths.</returns>
    public static List<string> AsPaths(this IList<Folder> folders) => folders.Select(f => f.Path).ToList();

    /// <summary>
    /// Turns a nullable Folder object in a non-nullable Folder object if the folder exists.
    /// </summary>
    /// <param name="folder">The folder to check.</param>
    /// <returns>The input object if not null.</returns>
    public static Folder AssertExists(this Folder? folder)
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