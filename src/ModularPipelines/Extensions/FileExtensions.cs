using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Extensions;

/// <summary>
/// Extensions for files.
/// </summary>
public static class FileExtensions
{
    /// <summary>
    /// Converts the files into string paths.
    /// </summary>
    /// <param name="files">The file collection.</param>
    /// <returns>The files as paths.</returns>
    public static IEnumerable<string> AsPaths(this IEnumerable<File> files) => files.Select(f => f.Path);

    /// <summary>
    /// Converts the files into string paths.
    /// </summary>
    /// <param name="files">The file collection.</param>
    /// <returns>The files as paths.</returns>
    public static List<string> AsPaths(this IList<File> files) => files.Select(f => f.Path).ToList();
}