using System.Diagnostics;
using ModularPipelines.FileSystem;
using ModularPipelines.UnitTests.Attributes;

namespace ModularPipelines.UnitTests.FileSystem;

public class FolderCopyLinkTests
{
    [Test]
    [Arguments(false, false, false)]
    [Arguments(false, false, true)]
    [Arguments(false, true, false)]
    [Arguments(false, true, true)]
    [Arguments(true, false, false)]
    [Arguments(true, false, true)]
    [Arguments(true, true, false)]
    [Arguments(true, true, true)]
    public async Task GuardRejectsDestinationAliases(bool sourceAlias, bool missingChild, bool linkedTargetParent)
    {
        using var temporary = new TempFolder();
        var root = temporary.Folder.Path;
        var source = Directory.CreateDirectory(Path.Combine(root, "source")).FullName;
        var child = Directory.CreateDirectory(Path.Combine(source, "child")).FullName;
        var parentAlias = Path.Combine(root, "parent-alias");
        SymbolicLinkTestHelper.CreateOrSkip(() => Directory.CreateSymbolicLink(parentAlias, root));
        var destinationAlias = Path.Combine(root, "destination-alias");
        var linkTarget = linkedTargetParent ? Path.Combine(parentAlias, "source", "child") : child;
        SymbolicLinkTestHelper.CreateOrSkip(() => Directory.CreateSymbolicLink(destinationAlias, Path.GetRelativePath(root, linkTarget)));
        var folder = new FolderPath(sourceAlias ? Path.Combine(parentAlias, "source") : source);
        var destination = missingChild ? Path.Combine(destinationAlias, "missing", "nested") : destinationAlias;

        // Exercise the production guard directly so a regression cannot start an unbounded copy.
        var exception = await Assert.That(() => folder.ValidateCopyTarget(destination)).Throws<ArgumentException>();
        exception = await Assert.That(exception).IsNotNull();

        await Assert.That(exception.ParamName).IsEqualTo("targetPath");
        await Assert.That(Directory.EnumerateFileSystemEntries(child)).IsEmpty();
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task GuardRejectsSourceAliasAndDanglingChildAlias(bool danglingChild)
    {
        using var temporary = new TempFolder();
        var root = temporary.Folder.Path;
        var source = Directory.CreateDirectory(Path.Combine(root, "source")).FullName;
        var alias = Path.Combine(root, "alias");
        SymbolicLinkTestHelper.CreateOrSkip(() => Directory.CreateSymbolicLink(alias, danglingChild ? Path.Combine(source, "missing") : source));

        await Assert.That(() => new FolderPath(source).ValidateCopyTarget(alias)).Throws<ArgumentException>();
        await Assert.That(Directory.EnumerateFileSystemEntries(source)).IsEmpty();
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task AsyncCopyRejectsAliasesBeforeMutation(bool preserveTimestamps)
    {
        using var temporary = new TempFolder();
        var root = temporary.Folder.Path;
        var source = Directory.CreateDirectory(Path.Combine(root, "source")).FullName;
        var alias = Path.Combine(root, "alias");
        SymbolicLinkTestHelper.CreateOrSkip(() => Directory.CreateSymbolicLink(alias, source));
        using var cancellation = new CancellationTokenSource();
        cancellation.CancelAfter(TimeSpan.FromSeconds(5));

        // Cancellation bounds enumeration if validation regresses; a pre-cancelled token takes precedence over validation.
        await Assert.That(() => (Task) new FolderPath(source).CopyToAsync(
            Path.Combine(alias, "missing"), preserveTimestamps, cancellation.Token)).Throws<ArgumentException>();

        await Assert.That(Directory.EnumerateFileSystemEntries(source)).IsEmpty();
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    public async Task CopyAllowsUnrelatedDestinationAlias(int overload)
    {
        using var temporary = new TempFolder();
        var root = temporary.Folder.Path;
        var source = Directory.CreateDirectory(Path.Combine(root, "source")).FullName;
        var output = Directory.CreateDirectory(Path.Combine(root, "output")).FullName;
        var alias = Path.Combine(root, "alias");
        SymbolicLinkTestHelper.CreateOrSkip(() => Directory.CreateSymbolicLink(alias, output));
        await System.IO.File.WriteAllTextAsync(Path.Combine(source, "payload.txt"), "contents");
        var destination = Path.Combine(alias, "missing");
        var folder = new FolderPath(source);

        var copied = overload switch
        {
            0 => folder.CopyTo(destination),
            1 => folder.CopyTo(destination, preserveTimestamps: true),
            2 => await folder.CopyToAsync(destination),
            _ => await folder.CopyToAsync(destination, preserveTimestamps: true),
        };

        await Assert.That(copied.OriginalPath).IsEqualTo(destination);
        await Assert.That(await System.IO.File.ReadAllTextAsync(Path.Combine(output, "missing", "payload.txt"))).IsEqualTo("contents");
    }

    [Test]
    public async Task GuardBoundsLinkCycles()
    {
        using var temporary = new TempFolder();
        var root = temporary.Folder.Path;
        var first = Path.Combine(root, "first");
        var second = Path.Combine(root, "second");
        SymbolicLinkTestHelper.CreateOrSkip(() => Directory.CreateSymbolicLink(first, second));
        SymbolicLinkTestHelper.CreateOrSkip(() => Directory.CreateSymbolicLink(second, first));

        await Assert.That(() => temporary.Folder.ValidateCopyTarget(first)).Throws<IOException>();
    }

    [Test]
    [WindowsOnlyTest]
    public async Task GuardRejectsJunctionDestination()
    {
        using var temporary = new TempFolder();
        var root = temporary.Folder.Path;
        var source = Directory.CreateDirectory(Path.Combine(root, "source")).FullName;
        var junction = Path.Combine(root, "junction");
        using var process = Process.Start(new ProcessStartInfo("cmd.exe", $"/c mklink /J \"{junction}\" \"{source}\"")
        {
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
        })!;
        await process.WaitForExitAsync();
        await Assert.That(process.ExitCode).IsEqualTo(0);

        await Assert.That(() => new FolderPath(source).ValidateCopyTarget(Path.Combine(junction, "missing")))
            .Throws<ArgumentException>();
        await Assert.That(Directory.EnumerateFileSystemEntries(source)).IsEmpty();
    }
}
