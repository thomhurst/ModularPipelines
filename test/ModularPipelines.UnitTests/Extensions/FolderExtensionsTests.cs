using ModularPipelines.Extensions;
using ModularPipelines.FileSystem;
using TUnit.Assertions;
using TUnit.Core;

namespace ModularPipelines.UnitTests.Extensions;

public class FolderExtensionsTests
{
    [Test]
    public void EnumerablePaths()
    {
        var folders = new List<Folder>
        {
            new Folder(Path.Combine(Environment.CurrentDirectory, "Folder1")),
            new Folder(Path.Combine(Environment.CurrentDirectory, "Folder2")),
        }.AsEnumerable();

        var paths = folders.AsPaths();
        await Assert.That(paths).Is.AssignableTo<IEnumerable<string>>();
        await await Assert.That(paths).Is.Not.AssignableTo<List<string>>();
        await Assert.That(paths).Is.EquivalentTo(new List<string>
        {
            Path.Combine(Environment.CurrentDirectory, "Folder1"),
            Path.Combine(Environment.CurrentDirectory, "Folder2"),
        });
    }

    [Test]
    public void ListPaths()
    {
        var folders = new List<Folder>
        {
            new Folder(Path.Combine(Environment.CurrentDirectory, "Folder1")),
            new Folder(Path.Combine(Environment.CurrentDirectory, "Folder2")),
        };

        var paths = folders.AsPaths();
        await Assert.That(paths).Is.AssignableTo<IEnumerable<string>>();
        await Assert.That(paths).Is.AssignableTo<List<string>>();
        await Assert.That(paths).Is.EquivalentTo(new List<string>
        {
            Path.Combine(Environment.CurrentDirectory, "Folder1"),
            Path.Combine(Environment.CurrentDirectory, "Folder2"),
        });
    }
}