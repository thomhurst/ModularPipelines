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

    public static IEnumerable<string> EnumerateFiles(FolderPath path, Func<FolderPath, bool> directoryExclusionFilters)
    {
        foreach (var file in Directory.EnumerateFiles(path, "*", SafeEnumerationOptions))
        {
            yield return file;
        }

        foreach (var folder in Directory.EnumerateDirectories(path, "*", SafeDirectoryEnumerationOptions)
                     .Where(x => !directoryExclusionFilters(x!)))
        {
            foreach (var innerFile in EnumerateFiles(folder!, directoryExclusionFilters))
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
