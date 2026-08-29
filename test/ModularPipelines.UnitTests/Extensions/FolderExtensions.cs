using ModularPipelines.FileSystem;

namespace ModularPipelines.UnitTests.Extensions;

public static class FolderExtensions
{
    public static FolderPath? FindAncestorContainingProject(this FolderPath original)
    {
        var folder = original;

        while (folder != null)
        {
            if (folder.ListFiles().Any(x => x.Extension == ".csproj"))
            {
                return folder;
            }

            folder = folder.Parent;
        }

        return null;
    }
}
