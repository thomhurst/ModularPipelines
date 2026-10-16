using System.Diagnostics;
using ModularPipelines.FileSystem;
using ModularPipelines.UnitTests.Attributes;
using Moq;

namespace ModularPipelines.UnitTests.FileSystem;

public class FolderSourceLinkTests
{
    [Test]
    [MatrixDataSource]
    public async Task CopyRejectsExternalSymbolicLinksBeforeMutation(
        [Matrix(false, true)] bool useAsync,
        [Matrix("default", "false", "true")] string timestamps,
        [Matrix(false, true)] bool existingDestination)
    {
        using var temporary = new TempFolder();
        var source = temporary.Folder.CreateFolder("source");
        var external = temporary.Folder.CreateFolder("external");
        external.CreateFile("external.txt");
        SymbolicLinkTestHelper.CreateOrSkip(() => Directory.CreateSymbolicLink(Path.Combine(source, "link"), external));

        await AssertCopyRejected(source, temporary.Folder, useAsync, timestamps, existingDestination);
    }

    [Test]
    [WindowsOnlyTest]
    [MatrixDataSource]
    public async Task CopyRejectsExternalJunctionsBeforeMutation(
        [Matrix(false, true)] bool useAsync,
        [Matrix("default", "false", "true")] string timestamps,
        [Matrix(false, true)] bool existingDestination)
    {
        using var temporary = new TempFolder();
        var source = temporary.Folder.CreateFolder("source");
        var external = temporary.Folder.CreateFolder("external");
        external.CreateFile("external.txt");
        await CreateJunction(Path.Combine(source, "link"), external);

        await AssertCopyRejected(source, temporary.Folder, useAsync, timestamps, existingDestination);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task AsyncCopyRejectsSourceSymbolicLinkCycle(bool preserveTimestamps)
    {
        using var temporary = new TempFolder();
        var source = temporary.Folder.CreateFolder("source");
        SymbolicLinkTestHelper.CreateOrSkip(() => Directory.CreateSymbolicLink(Path.Combine(source, "link"), source));

        await AssertCopyRejected(source, temporary.Folder, true, preserveTimestamps ? "true" : "false", false);
    }

    [Test]
    [WindowsOnlyTest]
    [Arguments(false)]
    [Arguments(true)]
    public async Task AsyncCopyRejectsSourceJunctionCycle(bool preserveTimestamps)
    {
        using var temporary = new TempFolder();
        var source = temporary.Folder.CreateFolder("source");
        await CreateJunction(Path.Combine(source, "link"), source);

        await AssertCopyRejected(source, temporary.Folder, true, preserveTimestamps ? "true" : "false", false);
    }

    [Test]
    [MatrixDataSource]
    public async Task CopyRejectsProviderDirectoryLinksWithoutDescending(
        [Matrix(false, true)] bool useAsync,
        [Matrix("default", "false", "true")] string timestamps)
    {
        var source = Path.GetFullPath("source");
        var destination = Path.GetFullPath("destination");
        var nested = Path.Combine(source, "nested");
        var link = Path.Combine(nested, "link");
        var provider = new Mock<IFileSystemProvider>(MockBehavior.Strict);
        provider.Setup(p => p.GetRelativePath(source, destination)).Returns(Path.GetRelativePath(source, destination));
        provider.SetupGet(p => p.DirectorySeparatorChar).Returns(Path.DirectorySeparatorChar);
        provider.Setup(p => p.EnumerateDirectories(source, "*", SearchOption.TopDirectoryOnly)).Returns([nested]);
        provider.Setup(p => p.EnumerateDirectories(nested, "*", SearchOption.TopDirectoryOnly)).Returns([link]);
        provider.Setup(p => p.EnumerateFiles(source, "*", SearchOption.TopDirectoryOnly)).Returns([]);
        provider.Setup(p => p.GetAttributes(nested)).Returns(FileAttributes.Directory);
        provider.Setup(p => p.GetAttributes(link)).Returns(FileAttributes.Directory | FileAttributes.ReparsePoint);
        // Strict calls bound regressions: recursive enumeration, descent into the link, or writes fail immediately.
        var folder = new FolderPath(source, provider.Object);

        await Assert.That(() => Copy(folder, destination, useAsync, timestamps)).Throws<IOException>();
    }

    private static async Task AssertCopyRejected(
        FolderPath source, FolderPath temporary, bool useAsync, string timestamps, bool existingDestination)
    {
        await source.CreateFolder("nested").GetFile("payload.txt").WriteAsync("source contents");
        await source.GetFile("payload.txt").WriteAsync("source contents");
        var destination = temporary.GetFolder("destination");
        if (existingDestination)
        {
            destination.Create();
            await destination.GetFile("payload.txt").WriteAsync("unchanged");
        }

        // Cancellation bounds a regression in the production snapshot when the source links back to itself.
        using var cancellation = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        var exception = await Assert.That(() => Copy(source, destination, useAsync, timestamps, cancellation.Token)).Throws<IOException>();
        exception = await Assert.That(exception).IsNotNull();
        await Assert.That(exception.Message).Contains("directory link");
        await Assert.That(destination.Exists).IsEqualTo(existingDestination);
        if (existingDestination)
        {
            await Assert.That(await destination.GetFile("payload.txt").ReadAsync()).IsEqualTo("unchanged");
            await Assert.That(Directory.EnumerateFileSystemEntries(destination).Count()).IsEqualTo(1);
        }
    }

    private static async Task Copy(
        FolderPath source, string destination, bool useAsync, string timestamps, CancellationToken cancellationToken = default)
    {
        if (useAsync)
        {
            if (timestamps == "default")
            {
                await source.CopyToAsync(destination, cancellationToken);
            }
            else
            {
                await source.CopyToAsync(destination, timestamps == "true", cancellationToken);
            }
        }
        else if (timestamps == "default")
        {
            source.CopyTo(destination);
        }
        else
        {
            source.CopyTo(destination, timestamps == "true");
        }
    }

    private static async Task CreateJunction(string link, string target)
    {
        using var process = Process.Start(new ProcessStartInfo("cmd.exe", $"/c mklink /J \"{link}\" \"{target}\"")
        {
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
        })!;
        await process.WaitForExitAsync();
        await Assert.That(process.ExitCode).IsEqualTo(0);
    }
}
