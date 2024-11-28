using System.Collections;
using ModularPipelines.Extensions;
using ModularPipelines.FileSystem;

namespace ModularPipelines.UnitTests.Extensions;

public class FolderExtensionsTests
{
    [Test]
    public async Task EnumerablePaths()
    {
        var folders = new List<Folder>
        {
            new(Path.Combine(TestContext.WorkingDirectory, "Folder1")),
            new(Path.Combine(TestContext.WorkingDirectory, "Folder2")),
        }.AsEnumerable();

        var paths = folders.AsPaths();
        await Assert.That(paths).IsAssignableTo(typeof(IEnumerable<string>))
            .And.IsNotAssignableTo(typeof(List<string>))
            .And.IsEquivalentTo(new List<string>
        {
            Path.Combine(TestContext.WorkingDirectory, "Folder1"),
            Path.Combine(TestContext.WorkingDirectory, "Folder2"),
        });
    }

    [Test]
    public async Task ListPaths()
    {
        var folders = new List<Folder>
        {
            new Folder(Path.Combine(TestContext.WorkingDirectory, "Folder1")),
            new Folder(Path.Combine(TestContext.WorkingDirectory, "Folder2")),
        };

        var paths = folders.AsPaths();
        await Assert.That(paths).IsAssignableTo(typeof(IEnumerable));
        await Assert.That(paths).IsAssignableTo(typeof(IEnumerable<string>));
        await Assert.That(paths).IsAssignableTo(typeof(List<string>));
        await Assert.That(paths).IsEquivalentTo([
            Path.Combine(TestContext.WorkingDirectory, "Folder1"),
            Path.Combine(TestContext.WorkingDirectory, "Folder2")
        ]);
    }
}