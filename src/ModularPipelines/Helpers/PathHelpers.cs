namespace ModularPipelines.Helpers;

internal static class PathHelpers
{
    public static PathType GetPathType(this string path)
    {
        if(File.Exists(path))
        {
            return PathType.File;
        }

        if(Directory.Exists(path))
        {
            return PathType.Directory;
        }

        if (Path.HasExtension(path))
        {
            return PathType.File;
        }

        return PathType.Directory;
    }

    public static string? GetDirectory(this string path)
    {
        return Path.GetDirectoryName(path) ?? new FileInfo(path).Directory?.FullName;
    }
}

internal enum PathType
{
    Directory,
    File
}