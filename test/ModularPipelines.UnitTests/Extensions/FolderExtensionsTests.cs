using System.Collections;
using ModularPipelines.Extensions;
using ModularPipelines.FileSystem;
using TUnit.Assertions.Extensions;

namespace ModularPipelines.UnitTests.Extensions;

public class FolderExtensionsTests
{
    [Test]
    public async Task EnumerablePaths()
    {
        var folders = new List<Folder>
        {
            new Folder(Path.Combine(TestContext.WorkingDirectory, "Folder1")),
            new Folder(Path.Combine(TestContext.WorkingDirectory, "Folder2")),
        }.AsEnumerable();

        var paths = folders.AsPaths();
        await Assert.That(paths).Is.AssignableTo<IEnumerable<string>>();
        await Assert.That(paths).Is.Not.AssignableTo<List<string>>();
        await Assert.That(paths).Is.EquivalentTo(new List<string>
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
        await Assert.That(paths).Is.AssignableTo<IEnumerable>();
        await Assert.That(paths).Is.AssignableTo<IEnumerable<string>>();
        await Assert.That(paths).Is.AssignableTo<List<string>>();
        await Assert.That(paths).Is.EquivalentTo(new List<string>
        {
            Path.Combine(TestContext.WorkingDirectory, "Folder1"),
            Path.Combine(TestContext.WorkingDirectory, "Folder2"),
        });
    }
}