using ModularPipelines.Helpers;
using TUnit.Assertions.Extensions;

namespace ModularPipelines.UnitTests;

public class PathHelpersTests
{
    [Test]
    public async Task Get_Directory()
    {
        var outputDirectory = new DirectoryInfo(Environment.CurrentDirectory);

        var fooTxt = outputDirectory.EnumerateFiles("*Foo.txt", SearchOption.AllDirectories).First();
        await Assert.That(fooTxt.FullName.GetDirectory()).Is.EqualTo(fooTxt.Directory!.FullName);
    }

    [Test]
    public async Task File_Path_Type()
    {
        var outputDirectory = new DirectoryInfo(Environment.CurrentDirectory);

        var fooTxt = outputDirectory.EnumerateFiles("*Foo.txt", SearchOption.AllDirectories).First();
        await Assert.That(fooTxt.FullName.GetPathType()).Is.EqualTo(PathType.File);
    }

    [Test]
    public async Task File_Path_Type2()
    {
        var path = Path.Combine(Environment.CurrentDirectory, "Blah", "Foo", "Bar", "Foo.txt");
        await Assert.That(path.GetPathType()).Is.EqualTo(PathType.File);
    }

    [Test]
    public async Task Directory_Path_Type()
    {
        var outputDirectory = new DirectoryInfo(Environment.CurrentDirectory);
        await Assert.That(outputDirectory.FullName.GetPathType()).Is.EqualTo(PathType.Directory);
    }

    [Test]
    public async Task Directory_Path_Type2()
    {
        var path = Path.Combine(Environment.CurrentDirectory, "Blah", "Foo", "Bar", "Foo");
        await Assert.That(path.GetPathType()).Is.EqualTo(PathType.Directory);
    }
}