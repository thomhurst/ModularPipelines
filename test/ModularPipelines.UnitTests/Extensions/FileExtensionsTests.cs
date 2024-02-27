using ModularPipelines.Extensions;
using TUnit.Assertions.Extensions;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.UnitTests.Extensions;

public class FileExtensionsTests
{
    [Test]
    public async Task EnumerablePaths()
    {
        var files = new List<File>
        {
            new File(Path.Combine(TestContext.WorkingDirectory, "File1.txt")),
            new File(Path.Combine(TestContext.WorkingDirectory, "File2.txt")),
        }.AsEnumerable();

        var paths = files.AsPaths();
        await Assert.That(paths).Is.AssignableTo<IEnumerable<string>>();
        await Assert.That(paths).Is.Not.AssignableTo<List<string>>();
        await Assert.That(paths).Is.EquivalentTo(new List<string>
        {
            Path.Combine(TestContext.WorkingDirectory, "File1.txt"),
            Path.Combine(TestContext.WorkingDirectory, "File2.txt"),
        });
    }

    [Test]
    public async Task ListPaths()
    {
        var files = new List<File>
        {
            new File(Path.Combine(TestContext.WorkingDirectory, "File1.txt")),
            new File(Path.Combine(TestContext.WorkingDirectory, "File2.txt")),
        };

        var paths = files.AsPaths();
        await Assert.That(paths).Is.AssignableTo<IEnumerable<string>>();
        await Assert.That(paths).Is.AssignableTo<List<string>>();
        await Assert.That(paths).Is.EquivalentTo(new List<string>
        {
            Path.Combine(TestContext.WorkingDirectory, "File1.txt"),
            Path.Combine(TestContext.WorkingDirectory, "File2.txt"),
        });
    }
}