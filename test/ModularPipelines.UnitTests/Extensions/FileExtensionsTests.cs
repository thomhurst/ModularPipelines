using ModularPipelines.Extensions;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.UnitTests.Extensions;

public class FileExtensionsTests
{
    [Test]
    public void EnumerablePaths()
    {
        var files = new List<File>
        {
            new File(Path.Combine(Environment.CurrentDirectory, "File1.txt")),
            new File(Path.Combine(Environment.CurrentDirectory, "File2.txt")),
        }.AsEnumerable();

        var paths = files.AsPaths();

        Assert.That(paths, Is.AssignableTo<IEnumerable<string>>());
        Assert.That(paths, Is.Not.AssignableTo<List<string>>());
        Assert.That(paths, Is.EquivalentTo(new List<string>
        {
            Path.Combine(Environment.CurrentDirectory, "File1.txt"),
            Path.Combine(Environment.CurrentDirectory, "File2.txt"),
        }));
    }

    [Test]
    public void ListPaths()
    {
        var files = new List<File>
        {
            new File(Path.Combine(Environment.CurrentDirectory, "File1.txt")),
            new File(Path.Combine(Environment.CurrentDirectory, "File2.txt")),
        };

        var paths = files.AsPaths();

        Assert.That(paths, Is.AssignableTo<IEnumerable<string>>());
        Assert.That(paths, Is.AssignableTo<List<string>>());
        Assert.That(paths, Is.EquivalentTo(new List<string>
        {
            Path.Combine(Environment.CurrentDirectory, "File1.txt"),
            Path.Combine(Environment.CurrentDirectory, "File2.txt"),
        }));
    }
}
