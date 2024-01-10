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

    /// <summary>
    /// Turns a nullable File object in a non-nullable File object if the file exists.
    /// </summary>
    /// <param name="file">The file to check.</param>
    /// <returns>The input object if not null.</returns>
    public static File AssertExists(this File? file)
    {
        if (file == null)
        {
            throw new FileNotFoundException("The file does not exist");
        }
        
        if (!file.Exists)
        {
            throw new FileNotFoundException("The file does not exist", file.Path);
        }

        return file;
    }
}