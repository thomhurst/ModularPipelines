using System.Collections;
using ModularPipelines.Extensions;
using ModularPipelines.FileSystem;

namespace ModularPipelines.UnitTests.Extensions;

public class FolderPathExtensionsTests
{
    [Test]
    public async Task EnumerablePaths()
    {
        var folders = new List<FolderPath>
        {
            new(Path.Combine(TestContext.WorkingDirectory, "Folder1")),
            new(Path.Combine(TestContext.WorkingDirectory, "Folder2")),
        }.AsEnumerable();

        var paths = folders.AsPaths();
        await Assert.That((object) paths).IsAssignableTo<IEnumerable<string>>();
        await Assert.That((object) paths).IsNotAssignableTo<List<string>>();
        await Assert.That(paths).IsEquivalentTo(new List<string>
        {
            Path.Combine(TestContext.WorkingDirectory, "Folder1"),
            Path.Combine(TestContext.WorkingDirectory, "Folder2"),
        });
    }

    [Test]
    public async Task ListPaths()
    {
        var folders = new List<FolderPath>
        {
            new FolderPath(Path.Combine(TestContext.WorkingDirectory, "Folder1")),
            new FolderPath(Path.Combine(TestContext.WorkingDirectory, "Folder2")),
        };

        var paths = ((IList<FolderPath>) folders).AsPaths();
        folders.Add(new(Path.Combine(TestContext.WorkingDirectory, "Folder3")));

        await Assert.That((object) paths).IsAssignableTo<IEnumerable>();
        await Assert.That((object) paths).IsAssignableTo<IEnumerable<string>>();
        await Assert.That((object) paths).IsAssignableTo<IReadOnlyList<string>>();
        await Assert.That((object) paths).IsNotAssignableTo<List<string>>();
        await Assert.That(paths).IsEquivalentTo([
            Path.Combine(TestContext.WorkingDirectory, "Folder1"),
            Path.Combine(TestContext.WorkingDirectory, "Folder2")
        ]);
    }
}
