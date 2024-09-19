using ModularPipelines.FileSystem;
using ModularPipelines.Helpers;
using ModularPipelines.UnitTests.Extensions;

namespace ModularPipelines.UnitTests;

public class PathHelpersTests
{
    [Test]
    public async Task Get_Directory()
    {
        var outputDirectory = new DirectoryInfo(new Folder(TestContext.WorkingDirectory).FindAncestorContainingProject()!);

        var fooTxt = outputDirectory.EnumerateFiles("*Foo.txt", SearchOption.AllDirectories).First();
        await Assert.That(fooTxt.FullName.GetDirectory()).IsEqualTo(fooTxt.Directory!.FullName);
    }

    [Test]
    public async Task File_Path_Type()
    {
        var outputDirectory = new DirectoryInfo(new Folder(TestContext.WorkingDirectory).FindAncestorContainingProject()!);

        var fooTxt = outputDirectory.EnumerateFiles("*Foo.txt", SearchOption.AllDirectories).First();
        await Assert.That(fooTxt.FullName.GetPathType()).IsEqualTo(PathType.File);
    }

    [Test]
    public async Task File_Path_Type2()
    {
        var path = Path.Combine(TestContext.WorkingDirectory, "Blah", "Foo", "Bar", "Foo.txt");
        await Assert.That(path.GetPathType()).IsEqualTo(PathType.File);
    }

    [Test]
    public async Task Directory_Path_Type()
    {
        var outputDirectory = new DirectoryInfo(TestContext.WorkingDirectory);
        await Assert.That(outputDirectory.FullName.GetPathType()).IsEqualTo(PathType.Directory);
    }

    [Test]
    public async Task Directory_Path_Type2()
    {
        var path = Path.Combine(TestContext.WorkingDirectory, "Blah", "Foo", "Bar", "Foo");
        await Assert.That(path.GetPathType()).IsEqualTo(PathType.Directory);
    }
}