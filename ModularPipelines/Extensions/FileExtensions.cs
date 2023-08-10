using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Extensions;

public static class FileExtensions
{
    public static IEnumerable<string> AsPaths(this IEnumerable<File> files) => files.Select(f => f.Path);
    public static List<string> AsPaths(this IList<File> files) => files.Select(f => f.Path).ToList();
}