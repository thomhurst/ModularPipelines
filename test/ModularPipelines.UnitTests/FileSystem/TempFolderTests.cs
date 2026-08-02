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
}
