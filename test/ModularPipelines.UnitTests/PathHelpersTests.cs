using ModularPipelines.Helpers;

namespace ModularPipelines.UnitTests;

public class PathHelpersTests
{
    [Test]
    public void Get_Directory()
    {
        var outputDirectory = new DirectoryInfo(Environment.CurrentDirectory);

        var fooTxt = outputDirectory.EnumerateFiles("*Foo.txt", SearchOption.AllDirectories).First();

        Assert.That(fooTxt.FullName.GetDirectory(), Is.EqualTo(fooTxt.Directory!.FullName));
    }

    [Test]
    public void File_Path_Type()
    {
        var outputDirectory = new DirectoryInfo(Environment.CurrentDirectory);

        var fooTxt = outputDirectory.EnumerateFiles("*Foo.txt", SearchOption.AllDirectories).First();

        Assert.That(fooTxt.FullName.GetPathType(), Is.EqualTo(PathType.File));
    }

    [Test]
    public void File_Path_Type2()
    {
        var path = Path.Combine(Environment.CurrentDirectory, "Blah", "Foo", "Bar", "Foo.txt");

        Assert.That(path.GetPathType(), Is.EqualTo(PathType.File));
    }

    [Test]
    public void Directory_Path_Type()
    {
        var outputDirectory = new DirectoryInfo(Environment.CurrentDirectory);

        Assert.That(outputDirectory.FullName.GetPathType(), Is.EqualTo(PathType.Directory));
    }

    [Test]
    public void Directory_Path_Type2()
    {
        var path = Path.Combine(Environment.CurrentDirectory, "Blah", "Foo", "Bar", "Foo");

        Assert.That(path.GetPathType(), Is.EqualTo(PathType.Directory));
    }
}