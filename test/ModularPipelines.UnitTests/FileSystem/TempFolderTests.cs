using ModularPipelines.FileSystem;
using ModularPipelines.UnitTests.Attributes;

namespace ModularPipelines.UnitTests.FileSystem;

public class TempFolderTests
{
    [Test]
    [WindowsOnlyTest]
    public async Task Dispose_Removes_ReadOnly_Contents()
    {
        var path = Path.Combine(Path.GetTempPath(), $"modular-pipelines-{Guid.NewGuid():N}");
        var folder = new Folder(path).Create();
        var file = folder.CreateFile("read-only.txt");
        await file.WriteAsync("contents");
        System.IO.File.SetAttributes(file.Path, FileAttributes.ReadOnly);

        using (new TempFolder(folder))
        {
        }

        await Assert.That(Directory.Exists(path)).IsFalse();
    }

    [Test]
    [WindowsOnlyTest]
    public async Task Dispose_Does_Not_Clear_ReadOnly_Attributes_Through_Directory_Links()
    {
        var path = Path.Combine(Path.GetTempPath(), $"modular-pipelines-{Guid.NewGuid():N}");
        var externalPath = Path.Combine(Path.GetTempPath(), $"modular-pipelines-external-{Guid.NewGuid():N}");
        var externalFile = Path.Combine(externalPath, "read-only.txt");
        Directory.CreateDirectory(path);
        Directory.CreateDirectory(externalPath);

        try
        {
            await System.IO.File.WriteAllTextAsync(externalFile, "contents");
            System.IO.File.SetAttributes(externalFile, FileAttributes.ReadOnly);
            Directory.CreateSymbolicLink(Path.Combine(path, "external"), externalPath);

            using (new TempFolder(new Folder(path)))
            {
            }

            await Assert.That(Directory.Exists(path)).IsFalse();
            await Assert.That(System.IO.File.Exists(externalFile)).IsTrue();
            await Assert.That(System.IO.File.GetAttributes(externalFile).HasFlag(FileAttributes.ReadOnly)).IsTrue();
        }
        finally
        {
            if (System.IO.File.Exists(externalFile))
            {
                System.IO.File.SetAttributes(externalFile, FileAttributes.Normal);
            }

            if (Directory.Exists(path))
            {
                Directory.Delete(path, recursive: true);
            }

            if (Directory.Exists(externalPath))
            {
                Directory.Delete(externalPath, recursive: true);
            }
        }
    }

    [Test]
    [WindowsOnlyTest]
    public async Task Dispose_Does_Not_Clear_ReadOnly_Attributes_Through_File_Links()
    {
        var path = Path.Combine(Path.GetTempPath(), $"modular-pipelines-{Guid.NewGuid():N}");
        var externalPath = Path.Combine(Path.GetTempPath(), $"modular-pipelines-external-{Guid.NewGuid():N}");
        var externalFile = Path.Combine(externalPath, "read-only.txt");
        Directory.CreateDirectory(path);
        Directory.CreateDirectory(externalPath);

        try
        {
            await System.IO.File.WriteAllTextAsync(externalFile, "contents");
            System.IO.File.SetAttributes(externalFile, FileAttributes.ReadOnly);
            System.IO.File.CreateSymbolicLink(Path.Combine(path, "external.txt"), externalFile);

            using (new TempFolder(new Folder(path)))
            {
            }

            await Assert.That(Directory.Exists(path)).IsFalse();
            await Assert.That(System.IO.File.Exists(externalFile)).IsTrue();
            await Assert.That(System.IO.File.GetAttributes(externalFile).HasFlag(FileAttributes.ReadOnly)).IsTrue();
        }
        finally
        {
            if (System.IO.File.Exists(externalFile))
            {
                System.IO.File.SetAttributes(externalFile, FileAttributes.Normal);
            }

            if (Directory.Exists(path))
            {
                Directory.Delete(path, recursive: true);
            }

            if (Directory.Exists(externalPath))
            {
                Directory.Delete(externalPath, recursive: true);
            }
        }
    }
}
