using ModularPipelines.FileSystem;
using ModularPipelines.Helpers;
using ModularPipelines.UnitTests.Extensions;

namespace ModularPipelines.UnitTests.FileSystem;

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

    [Test]
    public async Task Directory_Path_Type_With_Trailing_Separator()
    {
        var path = Path.Combine(TestContext.WorkingDirectory, "Blah", "Foo") + Path.DirectorySeparatorChar;
        await Assert.That(path.GetPathType()).IsEqualTo(PathType.Directory);
    }

    [Test]
    public async Task Directory_Path_Type_With_Dots_And_Trailing_Separator()
    {
        // A directory with dots in the name should be detected as directory when it has a trailing separator
        var path = Path.Combine(TestContext.WorkingDirectory, "my.folder") + Path.DirectorySeparatorChar;
        await Assert.That(path.GetPathType()).IsEqualTo(PathType.Directory);
    }

    [Test]
    public async Task EndsWithDirectorySeparator_Returns_True_For_Trailing_Separator()
    {
        var path = Path.Combine(TestContext.WorkingDirectory, "foo") + Path.DirectorySeparatorChar;
        await Assert.That(PathHelpers.EndsWithDirectorySeparator(path)).IsTrue();
    }

    [Test]
    public async Task EndsWithDirectorySeparator_Returns_True_For_Alt_Separator()
    {
        var path = Path.Combine(TestContext.WorkingDirectory, "foo") + Path.AltDirectorySeparatorChar;
        await Assert.That(PathHelpers.EndsWithDirectorySeparator(path)).IsTrue();
    }

    [Test]
    public async Task EndsWithDirectorySeparator_Returns_False_For_No_Separator()
    {
        var path = Path.Combine(TestContext.WorkingDirectory, "foo");
        await Assert.That(PathHelpers.EndsWithDirectorySeparator(path)).IsFalse();
    }

    [Test]
    public async Task EndsWithDirectorySeparator_Returns_False_For_Empty_String()
    {
        await Assert.That(PathHelpers.EndsWithDirectorySeparator(string.Empty)).IsFalse();
    }

    [Test]
    public async Task EndsWithDirectorySeparator_Returns_False_For_Null()
    {
        await Assert.That(PathHelpers.EndsWithDirectorySeparator(null!)).IsFalse();
    }
}