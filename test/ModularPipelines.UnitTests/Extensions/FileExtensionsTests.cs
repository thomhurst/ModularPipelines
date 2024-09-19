using ModularPipelines.Extensions;
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
        await Assert.That(paths).IsAssignableTo<IEnumerable<string>>();
        await Assert.That(paths).IsNotAssignableTo<List<string>>();
        await Assert.That(paths).IsEquivalentTo(new List<string>
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
        await Assert.That(paths).IsAssignableTo<IEnumerable<string>>();
        await Assert.That(paths).IsAssignableTo<List<string>>();
        await Assert.That(paths).IsEquivalentTo(new List<string>
        {
            Path.Combine(TestContext.WorkingDirectory, "File1.txt"),
            Path.Combine(TestContext.WorkingDirectory, "File2.txt"),
        });
    }
}