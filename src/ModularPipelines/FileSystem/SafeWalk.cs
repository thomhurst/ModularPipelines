namespace ModularPipelines.FileSystem;

internal static class SafeWalk
{
    private static readonly EnumerationOptions SafeEnumerationOptions = new()
    {
        IgnoreInaccessible = true,
    };

    public static IEnumerable<string> EnumerateFiles(Folder path, Func<Folder, bool> directoryExclusionFilters)
    {
        foreach (var file in Directory.EnumerateFiles(path, "*", SafeEnumerationOptions))
        {
            yield return file;
        }

        var innerFiles = new List<string>();
        foreach (var folder in Directory.EnumerateDirectories(path, "*", SafeEnumerationOptions)
                     .Where(x => !directoryExclusionFilters(x!)))
        {
            innerFiles.AddRange(EnumerateFiles(folder!, directoryExclusionFilters));

            foreach (var innerFile in innerFiles)
            {
                yield return innerFile;
            }

            innerFiles.Clear();
        }
    }

    public static IEnumerable<string> EnumerateFolders(Folder path, Func<Folder, bool> exclusionFilters)
    {
        var innerFolders = new List<string>();
        foreach (var folder in Directory.EnumerateDirectories(path, "*", SafeEnumerationOptions)
                     .Where(x => !exclusionFilters(x!)))
        {
            yield return folder;

            innerFolders.AddRange(EnumerateFolders(folder!, exclusionFilters));

            foreach (var innerFolder in innerFolders)
            {
                yield return innerFolder;
            }

            innerFolders.Clear();
        }
    }
}