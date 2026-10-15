using ModularPipelines.FileSystem;

namespace ModularPipelines.UnitTests.FileSystem;

public class FolderPathTimestampTests
{
    [Test]
    [MatrixDataSource]
    public async Task CopyTo_Restores_Directory_Timestamps_After_Copying_Contents(
        [Matrix(false, true)] bool useAsync,
        [Matrix("default", "false", "true")] string timestamps)
    {
        await using var temporary = new TempFolder();
        var source = temporary.Folder.CreateFolder("source");
        var nested = source.CreateFolder("nested");
        var deepest = nested.CreateFolder("deepest");
        var empty = nested.CreateFolder("empty");
        foreach (var folder in new[] { source, nested, deepest })
        {
            await folder.GetFile("file.txt").WriteAsync("contents");
        }

        var directories = new[] { source, nested, deepest, empty };
        var provider = SystemFileSystemProvider.Instance;
        for (var i = 0; i < directories.Length; i++)
        {
            provider.SetLastWriteTimeUtc(directories[i].Path, new DateTime(2001, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddDays(i));
        }

        var written = directories.Select(folder => provider.GetLastWriteTimeUtc(folder.Path)).ToArray();
        var destination = temporary.Folder.GetFolder("destination");
        if (useAsync)
        {
            if (timestamps == "default")
            {
                await source.CopyToAsync(destination.Path);
            }
            else
            {
                await source.CopyToAsync(destination.Path, preserveTimestamps: timestamps == "true");
            }
        }
        else if (timestamps == "default")
        {
            source.CopyTo(destination.Path);
        }
        else
        {
            source.CopyTo(destination.Path, preserveTimestamps: timestamps == "true");
        }

        for (var i = 0; i < directories.Length; i++)
        {
            var relativePath = Path.GetRelativePath(source.Path, directories[i].Path);
            var copied = Path.Combine(destination.Path, relativePath);
            if (timestamps == "true")
            {
                await Assert.That(provider.GetLastWriteTimeUtc(copied)).IsEqualTo(written[i]);
            }
            else
            {
                await Assert.That(provider.GetLastWriteTimeUtc(copied)).IsNotEqualTo(written[i]);
            }
        }

        await Assert.That(await destination.GetFolder("nested").GetFolder("deepest").GetFile("file.txt").ReadAsync()).IsEqualTo("contents");
    }
}
