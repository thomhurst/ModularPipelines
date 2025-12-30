namespace ModularPipelines.Helpers;

internal static class PathHelpers
{
    /// <summary>
    /// Determines whether a path represents a file or directory.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The detection logic follows this order of precedence:
    /// </para>
    /// <list type="number">
    /// <item><description>If the path exists as a file on disk, returns <see cref="PathType.File"/>.</description></item>
    /// <item><description>If the path exists as a directory on disk, returns <see cref="PathType.Directory"/>.</description></item>
    /// <item><description>If the path ends with a directory separator character (e.g., "/" or "\"), returns <see cref="PathType.Directory"/>.</description></item>
    /// <item><description>If the path has a file extension (e.g., ".txt", ".cs"), returns <see cref="PathType.File"/>.</description></item>
    /// <item><description>Otherwise, defaults to <see cref="PathType.Directory"/>.</description></item>
    /// </list>
    /// <para>
    /// <strong>Limitations:</strong> When the path does not exist on disk, this method uses heuristics
    /// that may not always be accurate. For example, a directory named "my.folder" would be incorrectly
    /// identified as a file due to the extension heuristic. Use explicit directory separators when
    /// working with non-existent directory paths that contain dots.
    /// </para>
    /// </remarks>
    /// <param name="path">The path to analyze.</param>
    /// <returns>The determined <see cref="PathType"/>.</returns>
    public static PathType GetPathType(this string path)
    {
        if (File.Exists(path))
        {
            return PathType.File;
        }

        if (Directory.Exists(path))
        {
            return PathType.Directory;
        }

        // Check if the path explicitly ends with a directory separator
        // This is a reliable indicator that the user intends this to be a directory
        if (EndsWithDirectorySeparator(path))
        {
            return PathType.Directory;
        }

        // Use extension heuristic as a fallback
        // Note: This can be unreliable for directory names containing dots (e.g., "my.folder")
        if (Path.HasExtension(path))
        {
            return PathType.File;
        }

        return PathType.Directory;
    }

    /// <summary>
    /// Gets the directory portion of a path.
    /// </summary>
    /// <param name="path">The path to extract the directory from.</param>
    /// <returns>The directory path, or <c>null</c> if it cannot be determined.</returns>
    public static string? GetDirectory(this string path)
    {
        return Path.GetDirectoryName(path) ?? new FileInfo(path).Directory?.FullName;
    }

    /// <summary>
    /// Determines whether the path ends with a directory separator character.
    /// </summary>
    /// <param name="path">The path to check.</param>
    /// <returns><c>true</c> if the path ends with a directory separator; otherwise, <c>false</c>.</returns>
    private static bool EndsWithDirectorySeparator(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return false;
        }

        var lastChar = path[path.Length - 1];
        return lastChar == Path.DirectorySeparatorChar || lastChar == Path.AltDirectorySeparatorChar;
    }
}
