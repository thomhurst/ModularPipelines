using ModularPipelines.Engine;
using ModularPipelines.Extensions;
using ModularPipelines.FileSystem;

namespace ModularPipelines.UnitTests.Extensions;

public class FilePathExtensionsTests
{
    [Test]
    public async Task EnumerablePaths()
    {
        var files = new List<FilePath>
        {
            new FilePath(Path.Combine(TestContext.WorkingDirectory, "File1.txt")),
            new FilePath(Path.Combine(TestContext.WorkingDirectory, "File2.txt")),
        }.AsEnumerable();

        var paths = files.AsPaths();
        await Assert.That((object) paths).IsAssignableTo<IEnumerable<string>>();
        await Assert.That((object) paths).IsNotAssignableTo<List<string>>();
        await Assert.That(paths).IsEquivalentTo(new List<string>
        {
            Path.Combine(TestContext.WorkingDirectory, "File1.txt"),
            Path.Combine(TestContext.WorkingDirectory, "File2.txt"),
        });
    }

    [Test]
    public async Task ListPaths()
    {
        var files = new List<FilePath>
        {
            new FilePath(Path.Combine(TestContext.WorkingDirectory, "File1.txt")),
            new FilePath(Path.Combine(TestContext.WorkingDirectory, "File2.txt")),
        };

        var paths = ((IList<FilePath>) files).AsPaths();
        files.Add(new(Path.Combine(TestContext.WorkingDirectory, "File3.txt")));

        await Assert.That((object) paths).IsAssignableTo<IEnumerable<string>>();
        await Assert.That((object) paths).IsAssignableTo<IReadOnlyList<string>>();
        await Assert.That((object) paths).IsNotAssignableTo<List<string>>();
        await Assert.That(paths).IsEquivalentTo([
            Path.Combine(TestContext.WorkingDirectory, "File1.txt"),
            Path.Combine(TestContext.WorkingDirectory, "File2.txt")
        ]);
    }

    [Test]
    public async Task NotFoundMessage()
    {
        var file = new FolderPath(Environment.CurrentDirectory).FindFile(_ => false);

        var exception = Assert.Throws<ArgumentNullException>(() => file.AssertExists("My message"));

        await Assert.That(exception.Message).IsEqualTo("FilePath reference is null - My message (Parameter 'file')");
    }

    [Test]
    public async Task NotFoundWithoutMessage()
    {
        var file = new FolderPath(Environment.CurrentDirectory).FindFile(_ => false);

        var exception = Assert.Throws<ArgumentNullException>(() => file.AssertExists());

        await Assert.That(exception.Message).IsEqualTo("FilePath reference is null (Parameter 'file')");
    }
}
