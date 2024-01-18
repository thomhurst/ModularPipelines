namespace ModularPipelines.FileSystem;

internal static class SafeWalk
{
    public static IEnumerable<string> EnumerateFiles(Folder path, Func<Folder, bool> directoryExclusionFilters)
    {
        foreach (var file in Directory.EnumerateFiles(path, "*", SearchOption.TopDirectoryOnly))
        {
            yield return file;
        }

        var innerFiles = new List<string>();
        foreach (var folder in Directory.EnumerateDirectories(path, "*", SearchOption.TopDirectoryOnly)
                     .Where(x => !directoryExclusionFilters(x!)))
        {
            try
            {
                innerFiles.AddRange(EnumerateFiles(folder!, directoryExclusionFilters));
            }
            catch (UnauthorizedAccessException)
            {
                continue;
            }

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
        foreach (var folder in Directory.EnumerateDirectories(path, "*", SearchOption.TopDirectoryOnly)
                     .Where(x => !exclusionFilters(x!)))
        {
            yield return folder;

            try
            {
                innerFolders.AddRange(EnumerateFolders(folder!, exclusionFilters));
            }
            catch (UnauthorizedAccessException)
            {
                continue;
            }

            foreach (var innerFolder in innerFolders)
            {
                yield return innerFolder;
            }

            innerFolders.Clear();
        }
    }
}