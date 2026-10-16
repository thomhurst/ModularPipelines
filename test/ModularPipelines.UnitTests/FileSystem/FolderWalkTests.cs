using System.Diagnostics;
using ModularPipelines.FileSystem;
using ModularPipelines.UnitTests.Attributes;

namespace ModularPipelines.UnitTests.FileSystem;

public class FolderWalkTests
{
    [Test]
    public async Task WalksIncludeHiddenFilesAndDirectories()
    {
        await AssertWalkIncludesEntries(FileAttributes.Hidden);
    }

    [Test]
    [WindowsOnlyTest]
    public async Task WalksIncludeSystemFilesAndDirectories()
    {
        await AssertWalkIncludesEntries(FileAttributes.System);
    }

    private static async Task AssertWalkIncludesEntries(FileAttributes attributes)
    {
        using var temporary = new TempFolder();
        var root = temporary.Folder;
        var rootFile = root.CreateFile(".editorconfig");
        var hidden = root.CreateFolder(".github");
        var nested = hidden.CreateFolder("workflows");
        var nestedFile = nested.CreateFile("build.yml");
        if (OperatingSystem.IsWindows())
        {
            rootFile.Attributes |= attributes;
            hidden.Attributes |= attributes;
        }

        await Assert.That(root.GetFiles(_ => true)).IsEquivalentTo(new[] { rootFile, nestedFile }).Using(EqualityComparer<FilePath>.Default);
        await Assert.That(root.FindFile(file => file.Name == ".editorconfig")).IsEqualTo(rootFile);
        await Assert.That(root.FindFile(file => file.Name == "build.yml")).IsEqualTo(nestedFile);
        await Assert.That(root.GetFolders(_ => true)).IsEquivalentTo(new[] { hidden, nested }).Using(EqualityComparer<FolderPath>.Default);
        await Assert.That(root.FindFolder(folder => folder.Name == ".github")).IsEqualTo(hidden);
        await Assert.That(root.FindFolder(folder => folder.Name == "workflows")).IsEqualTo(nested);

        await Assert.That(root.GetFiles(_ => true, folder => folder.Name == ".github"))
            .IsEquivalentTo(new[] { rootFile }).Using(EqualityComparer<FilePath>.Default);
        await Assert.That(root.GetFolders(_ => true, folder => folder.Name == ".github")).IsEmpty();
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task WalksSkipDirectorySymbolicLinks(bool cycle)
    {
        using var temporary = new TempFolder();
        var root = temporary.Folder.CreateFolder("root");
        var target = cycle ? root : temporary.Folder.CreateFolder("external");
        target.CreateFolder("nested").CreateFile("payload.txt");
        SymbolicLinkTestHelper.CreateOrSkip(() => Directory.CreateSymbolicLink(Path.Combine(root, "link"), target.Path));

        await AssertWalkSkipsLink(root, cycle);
    }

    [Test]
    [WindowsOnlyTest]
    [Arguments(false)]
    [Arguments(true)]
    public async Task WalksSkipDirectoryJunctions(bool cycle)
    {
        using var temporary = new TempFolder();
        var root = temporary.Folder.CreateFolder("root");
        var target = cycle ? root : temporary.Folder.CreateFolder("external");
        target.CreateFolder("nested").CreateFile("payload.txt");
        using var process = Process.Start(new ProcessStartInfo("cmd.exe", $"/c mklink /J \"{Path.Combine(root, "link")}\" \"{target.Path}\"")
        {
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
        })!;
        await process.WaitForExitAsync();
        await Assert.That(process.ExitCode).IsEqualTo(0);

        await AssertWalkSkipsLink(root, cycle);
    }

    private static async Task AssertWalkSkipsLink(FolderPath root, bool hasLocalContents)
    {
        // Bound a regression before it can recurse through a cycle indefinitely.
        bool Exclude(FolderPath folder) => folder.Path.StartsWith(Path.Combine(root, "link") + Path.DirectorySeparatorChar, StringComparison.Ordinal)
            ? throw new InvalidOperationException("Walk followed a directory link.")
            : false;

        var files = root.GetFiles(_ => true, Exclude).ToArray();
        var folders = root.GetFolders(_ => true, Exclude).ToArray();

        await Assert.That(files.Length).IsEqualTo(hasLocalContents ? 1 : 0);
        await Assert.That(folders.Length).IsEqualTo(hasLocalContents ? 1 : 0);
        await Assert.That(root.FindFile(_ => true, Exclude)?.Path)
            .IsEqualTo(hasLocalContents ? Path.Combine(root, "nested", "payload.txt") : null);
        await Assert.That(root.FindFolder(_ => true, Exclude)?.Path)
            .IsEqualTo(hasLocalContents ? Path.Combine(root, "nested") : null);
    }

    [Test]
    public async Task FindFileStopsBeforeEnumeratingDeeperDirectories()
    {
        using var temporary = new TempFolder();
        var child = temporary.Folder.CreateFolder("child");
        var expected = child.CreateFile("payload.txt");
        child.CreateFolder("unvisited");

        var found = temporary.Folder.FindFile(_ => true, folder => folder.Name == "unvisited"
            ? throw new InvalidOperationException("FindFile traversed beyond its first match.")
            : false);

        await Assert.That(found).IsEqualTo(expected);
    }

    [Test]
    public async Task FindFolderStopsBeforeEnumeratingDeeperDirectories()
    {
        using var temporary = new TempFolder();
        var expected = temporary.Folder.CreateFolder("child").CreateFolder("match");
        expected.CreateFolder("unvisited");

        var found = temporary.Folder.FindFolder(folder => folder.Name == "match", folder => folder.Name == "unvisited"
            ? throw new InvalidOperationException("FindFolder traversed beyond its first match.")
            : false);

        await Assert.That(found).IsEqualTo(expected);
    }

    [Test]
    public async Task WalksHonorDirectoryExclusions()
    {
        using var temporary = new TempFolder();
        var included = temporary.Folder.CreateFolder("included");
        var expected = included.CreateFolder("nested").CreateFile("payload.txt");
        temporary.Folder.CreateFolder("excluded").CreateFolder("nested").CreateFile("hidden.txt");

        var files = temporary.Folder.GetFiles(_ => true, folder => folder.Name == "excluded").ToArray();
        var folders = temporary.Folder.GetFolders(_ => true, folder => folder.Name == "excluded").ToArray();

        await Assert.That(files).Count().IsEqualTo(1);
        await Assert.That(files[0]).IsEqualTo(expected);
        await Assert.That(folders).Count().IsEqualTo(2);
        await Assert.That(folders.All(folder => folder.Path.StartsWith(included.Path, StringComparison.Ordinal))).IsTrue();
    }

    [Test]
    public async Task WalkStillReturnsFileSymbolicLinks()
    {
        using var temporary = new TempFolder();
        var root = temporary.Folder.CreateFolder("root");
        var target = temporary.Folder.CreateFile("target.txt");
        var link = Path.Combine(root, "link.txt");
        SymbolicLinkTestHelper.CreateOrSkip(() => System.IO.File.CreateSymbolicLink(link, target.Path));

        var files = root.GetFiles(_ => true).ToArray();

        await Assert.That(files).Count().IsEqualTo(1);
        await Assert.That(files[0].Path).IsEqualTo(link);
    }
}
