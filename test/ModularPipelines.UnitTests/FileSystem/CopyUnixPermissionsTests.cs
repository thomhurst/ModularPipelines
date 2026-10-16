using ModularPipelines.FileSystem;
using TUnit.Core.Exceptions;

namespace ModularPipelines.UnitTests.FileSystem;

public class CopyUnixPermissionsTests
{
    [Test]
    [MatrixDataSource]
    public async Task File_CopyTo_Preserves_Unix_Mode(
        [Matrix(false, true)] bool useAsync,
        [Matrix(false, true)] bool useFolder,
        [Matrix(false, true)] bool overwrite,
        [Matrix(false, true)] bool executable)
    {
        if (OperatingSystem.IsWindows())
        {
            throw new SkipTestException("Unix file permissions are unavailable on Windows.");
        }

        await using var temporary = new TempFolder();
        var source = temporary.Folder.CreateFolder("source").GetFile("file");
        await source.WriteAsync("contents");
        var mode = UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.GroupRead;
        if (executable)
        {
            mode |= UnixFileMode.UserExecute | UnixFileMode.GroupExecute | UnixFileMode.OtherExecute;
        }

        File.SetUnixFileMode(source.Path, mode);
        var destinationFolder = temporary.Folder.CreateFolder("destination");
        var destination = destinationFolder.GetFile(source.Name);
        if (overwrite)
        {
            await destination.WriteAsync("old contents");
            File.SetUnixFileMode(destination.Path, UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.OtherExecute);
        }

        FilePath copied;
        if (useFolder)
        {
            copied = useAsync ? await source.CopyToAsync(destinationFolder) : source.CopyTo(destinationFolder);
        }
        else
        {
            copied = useAsync ? await source.CopyToAsync(destination.Path) : source.CopyTo(destination.Path);
        }

        await Assert.That(File.GetUnixFileMode(copied.Path)).IsEqualTo(mode);
        await Assert.That(await copied.ReadAsync()).IsEqualTo("contents");
    }

    [Test]
    [MatrixDataSource]
    public async Task Folder_CopyTo_Preserves_File_Unix_Modes(
        [Matrix(false, true)] bool useAsync,
        [Matrix("default", "false", "true")] string timestamps,
        [Matrix(false, true)] bool overwrite)
    {
        if (OperatingSystem.IsWindows())
        {
            throw new SkipTestException("Unix file permissions are unavailable on Windows.");
        }

        await using var temporary = new TempFolder();
        var source = temporary.Folder.CreateFolder("source");
        var nested = source.CreateFolder("nested");
        var files = new[] { source.GetFile("script"), nested.GetFile("binary"), nested.GetFile("data") };
        var modes = new[]
        {
            UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.UserExecute | UnixFileMode.GroupExecute,
            UnixFileMode.UserRead | UnixFileMode.UserExecute | UnixFileMode.OtherExecute,
            UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.GroupRead,
        };
        var destination = temporary.Folder.GetFolder("destination");
        for (var i = 0; i < files.Length; i++)
        {
            await files[i].WriteAsync("contents");
            File.SetUnixFileMode(files[i].Path, modes[i]);
            if (overwrite)
            {
                var existing = destination.GetFile(Path.GetRelativePath(source.Path, files[i].Path));
                existing.Folder!.Create();
                await existing.WriteAsync("old contents");
                File.SetUnixFileMode(existing.Path, UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.OtherExecute);
            }
        }

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

        for (var i = 0; i < files.Length; i++)
        {
            var copied = destination.GetFile(Path.GetRelativePath(source.Path, files[i].Path));
            await Assert.That(File.GetUnixFileMode(copied.Path)).IsEqualTo(modes[i]);
            await Assert.That(await copied.ReadAsync()).IsEqualTo("contents");
        }
    }
}
