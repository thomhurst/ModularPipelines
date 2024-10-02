using ModularPipelines.Engine;
using ModularPipelines.Extensions;
using ModularPipelines.FileSystem;
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
        await Assert.That(paths).IsAssignableTo(typeof(IEnumerable<string>));
        await Assert.That(paths).IsNotAssignableTo(typeof(List<string>));
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
        await Assert.That(paths).IsAssignableTo(typeof(IEnumerable<string>));
        await Assert.That(paths).IsAssignableTo(typeof(List<string>));
        await Assert.That(paths).IsEquivalentTo(new List<string>
        {
            Path.Combine(TestContext.WorkingDirectory, "File1.txt"),
            Path.Combine(TestContext.WorkingDirectory, "File2.txt"),
        });
    }

    [Test]
    public async Task NotFoundMessage()
    {
        var file = new Folder(Environment.CurrentDirectory).FindFile(_ => false);
        
        var exception = Assert.Throws<FileNotFoundException>(() => file.AssertExists("My message"));

        await Assert.That(exception.Message).IsEqualTo("The file does not exist - My message");
    }
    
    [Test]
    public async Task NotFoundWithoutMessage()
    {
        var file = new Folder(Environment.CurrentDirectory).FindFile(_ => false);
        
        var exception = Assert.Throws<FileNotFoundException>(() => file.AssertExists());

        await Assert.That(exception.Message).IsEqualTo("The file does not exist");
    }
}