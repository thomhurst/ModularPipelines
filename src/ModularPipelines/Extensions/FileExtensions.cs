using System.Text;
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
    /// <param name="message">Helper message.</param>
    /// <returns>The input object if not null.</returns>
    public static File AssertExists(this File? file, string? message = null)
    {
        if (file is null || !file.Exists)
        {
            throw new FileNotFoundException($"The file does not exist{GetMessage(file, message)}", file?.Path);
        }

        return file;
    }

    private static string? GetMessage(File? file, string? message)
    {
        if (file is null && message is null)
        {
            return null;
        }

        var sb = new StringBuilder();

        if (file is not null)
        {
            sb.Append($" - {file}");
        }
        
        if (message is not null)
        {
            sb.Append($" - {message}");
        }

        return sb.ToString();
    }
}