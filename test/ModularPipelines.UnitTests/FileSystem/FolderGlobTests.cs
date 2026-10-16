using System.Diagnostics;
using ModularPipelines.FileSystem;
using ModularPipelines.UnitTests.Attributes;
using Moq;

namespace ModularPipelines.UnitTests.FileSystem;

public class FolderGlobTests
{
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task GlobSkipsDirectorySymbolicLinks(bool cycle)
    {
        using var temporary = new TempFolder();
        var root = temporary.Folder.CreateFolder("root");
        var expected = root.CreateFolder("nested").CreateFile("local.txt");
        root.CreateFile("ignored.bin");
        var target = cycle ? root : temporary.Folder.CreateFolder("external");
        if (!cycle)
        {
            target.CreateFolder("nested").CreateFile("external.txt");
        }
        SymbolicLinkTestHelper.CreateOrSkip(() => Directory.CreateSymbolicLink(Path.Combine(root, "link"), target));

        // Two matches bound a regression even when the link points back to the root.
        var matches = root.GetFiles("**/*.txt").Take(2).ToArray();

        // Compare path identity, not live metadata on the file and its ancestor folders.
        await Assert.That(matches).IsEquivalentTo(new[] { expected }).Using(EqualityComparer<FilePath>.Default);
    }

    [Test]
    [WindowsOnlyTest]
    [Arguments(false)]
    [Arguments(true)]
    public async Task GlobSkipsDirectoryJunctions(bool cycle)
    {
        using var temporary = new TempFolder();
        var root = temporary.Folder.CreateFolder("root");
        var expected = root.CreateFolder("nested").CreateFile("local.txt");
        var target = cycle ? root : temporary.Folder.CreateFolder("external");
        if (!cycle)
        {
            target.CreateFile("external.txt");
        }
        var link = Path.Combine(root, "link");
        using var process = Process.Start(new ProcessStartInfo("cmd.exe", $"/c mklink /J \"{link}\" \"{target.Path}\"")
        {
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
        })!;
        await process.WaitForExitAsync();
        await Assert.That(process.ExitCode).IsEqualTo(0);

        var matches = root.GetFiles("**/*.txt").Take(2).ToArray();

        await Assert.That(matches).IsEquivalentTo(new[] { expected }).Using(EqualityComparer<FilePath>.Default);
    }

    [Test]
    public async Task GlobIncludesHiddenFilesAndDirectories()
    {
        using var temporary = new TempFolder();
        var hidden = temporary.Folder.CreateFolder(".hidden");
        var expected = hidden.CreateFile(".local.txt");
        if (OperatingSystem.IsWindows())
        {
            hidden.Attributes |= FileAttributes.Hidden;
            expected.Attributes |= FileAttributes.Hidden;
        }

        await Assert.That(temporary.Folder.GetFiles("**/*.txt")).IsEquivalentTo(new[] { expected }).Using(EqualityComparer<FilePath>.Default);
    }

    [Test]
    public async Task GlobUsesCustomProvider()
    {
        var root = Path.GetFullPath("virtual-source");
        var file = Path.Combine(root, "nested", "local.txt");
        var provider = new Mock<IFileSystemProvider>(MockBehavior.Strict);
        provider.Setup(p => p.EnumerateFiles(root, "*", SearchOption.AllDirectories)).Returns([file]);
        provider.Setup(p => p.GetRelativePath(root, file)).Returns(Path.Combine("nested", "local.txt"));

        var matches = new FolderPath(root, provider.Object).GetFiles("**/*.txt").ToArray();

        await Assert.That(matches).Count().IsEqualTo(1);
        await Assert.That(matches[0].Path).IsEqualTo(file);
        await Assert.That(matches[0].Provider).IsSameReferenceAs(provider.Object);
    }

    [Test]
    public async Task GlobDefersTraversalUntilEnumeration()
    {
        using var temporary = new TempFolder();
        var root = temporary.Folder.GetFolder("missing");

        var matches = root.GetFiles("**/*.txt");
        var expected = root.Create().CreateFolder("nested").CreateFile("local.txt");

        await Assert.That(matches).IsEquivalentTo(new[] { expected }).Using(EqualityComparer<FilePath>.Default);
    }
}
