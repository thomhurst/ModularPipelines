using System.Diagnostics;
using ModularPipelines.Context;
using ModularPipelines.FileSystem;
using ModularPipelines.UnitTests.Attributes;
using ModularPipelines.UnitTests.FileSystem;
using Moq;

namespace ModularPipelines.UnitTests.Helpers;

public class ZipSourceLinkTests
{
    [Test]
    [MatrixDataSource]
    public async Task RejectsSymbolicLinksBeforeCreatingArchive(
        [Matrix(false, true)] bool useAsync,
        [Matrix(false, true)] bool existingOutput,
        [Matrix("directory", "cycle", "file", "root")] string linkKind)
    {
        using var temporary = new TempFolder();
        var source = temporary.Folder.CreateFolder("source");
        var external = temporary.Folder.CreateFolder("external");
        var externalFile = external.CreateFile("private.txt");
        await externalFile.WriteAsync("external contents");
        var nested = source.CreateFolder("nested");
        if (linkKind == "file")
        {
            SymbolicLinkTestHelper.CreateOrSkip(() => File.CreateSymbolicLink(Path.Combine(nested, "link.txt"), externalFile));
        }
        else if (linkKind == "root")
        {
            var alias = temporary.Folder.GetFolder("alias");
            SymbolicLinkTestHelper.CreateOrSkip(() => Directory.CreateSymbolicLink(alias, source));
            source = alias;
        }
        else
        {
            SymbolicLinkTestHelper.CreateOrSkip(() => Directory.CreateSymbolicLink(
                Path.Combine(nested, "link"), linkKind == "cycle" ? source : external));
        }

        await AssertRejected(source, temporary.Folder, useAsync, existingOutput);
    }

    [Test]
    [WindowsOnlyTest]
    [MatrixDataSource]
    public async Task RejectsJunctionsBeforeCreatingArchive(
        [Matrix(false, true)] bool useAsync,
        [Matrix(false, true)] bool existingOutput,
        [Matrix(false, true)] bool cycle)
    {
        using var temporary = new TempFolder();
        var source = temporary.Folder.CreateFolder("source");
        var external = temporary.Folder.CreateFolder("external");
        external.CreateFile("private.txt");
        var link = Path.Combine(source.CreateFolder("nested"), "link");
        var target = cycle ? source : external;
        using var process = Process.Start(new ProcessStartInfo("cmd.exe", $"/c mklink /J \"{link}\" \"{target}\"")
        {
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
        })!;
        await process.WaitForExitAsync();
        await Assert.That(process.ExitCode).IsEqualTo(0);

        await AssertRejected(source, temporary.Folder, useAsync, existingOutput);
    }

    [Test]
    [MatrixDataSource]
    public async Task RejectsProviderLinksWithoutDescentOrWrites(
        [Matrix(false, true)] bool useAsync,
        [Matrix(false, true)] bool fileLink)
    {
        var root = Path.GetFullPath("virtual-zip-links");
        var source = Path.Combine(root, "source");
        var nested = Path.Combine(source, "nested");
        var link = Path.Combine(nested, "link");
        var provider = new Mock<IFileSystemProvider>(MockBehavior.Strict);
        provider.Setup(p => p.GetAttributes(source)).Returns(FileAttributes.Directory);
        provider.Setup(p => p.GetAttributes(nested)).Returns(FileAttributes.Directory);
        provider.Setup(p => p.GetAttributes(link)).Returns(FileAttributes.ReparsePoint);
        provider.Setup(p => p.EnumerateDirectories(source, "*", SearchOption.TopDirectoryOnly)).Returns([nested]);
        provider.Setup(p => p.EnumerateFiles(source, "*", SearchOption.TopDirectoryOnly)).Returns([]);
        provider.Setup(p => p.EnumerateDirectories(nested, "*", SearchOption.TopDirectoryOnly)).Returns(fileLink ? [] : [link]);
        provider.Setup(p => p.EnumerateFiles(nested, "*", SearchOption.TopDirectoryOnly)).Returns(fileLink ? [link] : []);
        // Strict calls reject recursive enumeration, descent into links, source reads, and output writes.
        IZipContext zip = new Zip(provider.Object, new PipelineWorkingDirectory(root));

        var exception = await Assert.That(() => Create(zip, new FolderPath(source), "archive.zip", useAsync)).Throws<IOException>();
        await Assert.That(exception!.Message).Contains("source link");
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task CancellationDuringSnapshotDoesNotCreateArchive(bool enumerateFiles)
    {
        var root = Path.GetFullPath("virtual-zip-cancellation");
        var source = Path.Combine(root, "source");
        var entry = Path.Combine(source, "entry");
        using var cancellation = new CancellationTokenSource();
        var provider = new Mock<IFileSystemProvider>(MockBehavior.Strict);
        provider.Setup(p => p.GetAttributes(source)).Returns(FileAttributes.Directory);
        provider.Setup(p => p.EnumerateDirectories(source, "*", SearchOption.TopDirectoryOnly))
            .Returns(enumerateFiles ? [] : CancelDuringEnumeration());
        provider.Setup(p => p.EnumerateFiles(source, "*", SearchOption.TopDirectoryOnly))
            .Returns(CancelDuringEnumeration);
        IZipContext zip = new Zip(provider.Object, new PipelineWorkingDirectory(root));

        await Assert.ThrowsAsync<OperationCanceledException>(() => zip.CreateFromDirectoryAsync(
            new FolderPath(source), "archive.zip", cancellationToken: cancellation.Token));

        IEnumerable<string> CancelDuringEnumeration()
        {
            cancellation.Cancel();
            yield return entry;
            throw new InvalidOperationException("Enumeration continued after cancellation.");
        }
    }

    private static async Task AssertRejected(FolderPath source, FolderPath root, bool useAsync, bool existingOutput)
    {
        var output = root.GetFile("archive.zip");
        if (existingOutput)
        {
            await output.WriteAsync("unchanged");
        }

        IZipContext zip = new Zip(SystemFileSystemProvider.Instance, new PipelineWorkingDirectory(root));
        var exception = await Assert.That(() => Create(zip, source, output, useAsync)).Throws<IOException>();
        await Assert.That(exception!.Message).Contains("source link");
        await Assert.That(output.Exists).IsEqualTo(existingOutput);
        if (existingOutput)
        {
            await Assert.That(await output.ReadAsync()).IsEqualTo("unchanged");
        }
    }

    private static async Task Create(IZipContext zip, FolderPath source, string output, bool useAsync)
    {
        if (useAsync)
        {
            await zip.CreateFromDirectoryAsync(source, output);
        }
        else
        {
            zip.CreateFromDirectory(source, output);
        }
    }
}
