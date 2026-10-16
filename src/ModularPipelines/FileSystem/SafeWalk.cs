namespace ModularPipelines.FileSystem;

internal static class SafeWalk
{
    private static readonly EnumerationOptions SafeEnumerationOptions = new()
    {
        IgnoreInaccessible = true,
    };

    private static readonly EnumerationOptions SafeDirectoryEnumerationOptions = new()
    {
        IgnoreInaccessible = true,
        AttributesToSkip = SafeEnumerationOptions.AttributesToSkip | FileAttributes.ReparsePoint,
    };

    private static readonly EnumerationOptions GlobEnumerationOptions = new()
    {
        IgnoreInaccessible = false,
        AttributesToSkip = 0,
    };

    private static readonly EnumerationOptions GlobDirectoryEnumerationOptions = new()
    {
        IgnoreInaccessible = false,
        AttributesToSkip = FileAttributes.ReparsePoint,
    };

    public static IEnumerable<string> EnumerateFiles(FolderPath path, Func<FolderPath, bool> directoryExclusionFilters)
        => EnumerateFiles(path, directoryExclusionFilters, SafeEnumerationOptions, SafeDirectoryEnumerationOptions);

    public static IEnumerable<string> EnumerateGlobFiles(FolderPath path)
        => EnumerateFiles(path, _ => false, GlobEnumerationOptions, GlobDirectoryEnumerationOptions);

    private static IEnumerable<string> EnumerateFiles(
        FolderPath path,
        Func<FolderPath, bool> directoryExclusionFilters,
        EnumerationOptions fileOptions,
        EnumerationOptions directoryOptions)
    {
        foreach (var file in Directory.EnumerateFiles(path, "*", fileOptions))
        {
            yield return file;
        }

        foreach (var folder in Directory.EnumerateDirectories(path, "*", directoryOptions)
                     .Where(x => !directoryExclusionFilters(x!)))
        {
            foreach (var innerFile in EnumerateFiles(folder!, directoryExclusionFilters, fileOptions, directoryOptions))
            {
                yield return innerFile;
            }
        }
    }

    public static IEnumerable<string> EnumerateFolders(FolderPath path, Func<FolderPath, bool> exclusionFilters)
    {
        foreach (var folder in Directory.EnumerateDirectories(path, "*", SafeDirectoryEnumerationOptions)
                     .Where(x => !exclusionFilters(x!)))
        {
            yield return folder;

            foreach (var innerFolder in EnumerateFolders(folder!, exclusionFilters))
            {
                yield return innerFolder;
            }
        }
    }
}
