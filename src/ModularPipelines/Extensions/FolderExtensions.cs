using ModularPipelines.FileSystem;

namespace ModularPipelines.Extensions;

public static class FolderExtensions
{
    public static IEnumerable<string> AsPaths(this IEnumerable<Folder> folders) => folders.Select(f => f.Path);

    public static List<string> AsPaths(this IList<Folder> folders) => folders.Select(f => f.Path).ToList();
}