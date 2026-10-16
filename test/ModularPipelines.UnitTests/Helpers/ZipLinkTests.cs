using System.Diagnostics;
using System.IO.Compression;
using ModularPipelines.Context;
using ModularPipelines.FileSystem;
using ModularPipelines.UnitTests.Attributes;
using ModularPipelines.UnitTests.FileSystem;

namespace ModularPipelines.UnitTests.Helpers;

public class ZipLinkTests
{
    [Test]
    [Arguments(false, false, "link/missing/payload.txt")]
    [Arguments(false, true, "link/missing/payload.txt")]
    [Arguments(true, false, "link/missing/payload.txt")]
    [Arguments(true, true, "link/missing/payload.txt")]
    [Arguments(false, true, "link/payload.txt")]
    [Arguments(true, true, "link/payload.txt")]
    [Arguments(false, false, "link/missing/")]
    [Arguments(true, false, "link/missing/")]
    [Arguments(false, true, "link/")]
    [Arguments(true, true, "link/")]
    public async Task RejectsDirectoryLink(bool asynchronous, bool overwrite, string entryName)
    {
        await CheckDirectoryLink(asynchronous, overwrite, entryName, junction: false);
    }

    [Test]
    [WindowsOnlyTest]
    [Arguments(false, false, "link/missing/payload.txt")]
    [Arguments(false, true, "link/payload.txt")]
    [Arguments(true, false, "link/missing/payload.txt")]
    [Arguments(true, true, "link/payload.txt")]
    [Arguments(false, false, "link/missing/")]
    [Arguments(true, false, "link/missing/")]
    public async Task RejectsJunction(bool asynchronous, bool overwrite, string entryName)
    {
        await CheckDirectoryLink(asynchronous, overwrite, entryName, junction: true);
    }

    [Test]
    [Arguments(false, false)]
    [Arguments(true, false)]
    [Arguments(false, true)]
    [Arguments(true, true)]
    public async Task RejectsFileLink(bool asynchronous, bool dangling)
    {
        using var temporary = new TempFolder();
        var root = temporary.Folder.Path;
        var destination = Directory.CreateDirectory(Path.Combine(root, "destination")).FullName;
        var external = Path.Combine(root, "external.txt");
        if (!dangling)
        {
            await File.WriteAllTextAsync(external, "preserve");
            File.SetLastWriteTimeUtc(external, new DateTime(2020, 1, 2, 3, 4, 6, DateTimeKind.Utc));
        }

        var lastWriteTime = File.GetLastWriteTimeUtc(external);
        SymbolicLinkTestHelper.CreateOrSkip(() => File.CreateSymbolicLink(Path.Combine(destination, "payload.txt"), external));
        var archive = CreateArchive(root, "payload.txt");
        var zip = new Zip(SystemFileSystemProvider.Instance, new PipelineWorkingDirectory(root));

        await Assert.That(() => Extract(zip, archive, destination, asynchronous, overwrite: true))
            .Throws<InvalidOperationException>();

        await Assert.That(File.Exists(external)).IsEqualTo(!dangling);
        await Assert.That(File.GetLastWriteTimeUtc(external)).IsEqualTo(lastWriteTime);
        if (!dangling)
        {
            await Assert.That(await File.ReadAllTextAsync(external)).IsEqualTo("preserve");
        }
    }

    private static async Task CheckDirectoryLink(bool asynchronous, bool overwrite, string entryName, bool junction)
    {
        using var temporary = new TempFolder();
        var root = temporary.Folder.Path;
        var destination = Directory.CreateDirectory(Path.Combine(root, "destination")).FullName;
        var external = Directory.CreateDirectory(Path.Combine(root, "external")).FullName;
        var externalFile = Path.Combine(external, "payload.txt");
        await File.WriteAllTextAsync(externalFile, "preserve");
        File.SetLastWriteTimeUtc(externalFile, new DateTime(2020, 1, 2, 3, 4, 6, DateTimeKind.Utc));
        var lastWriteTime = File.GetLastWriteTimeUtc(externalFile);
        var directoryWriteTime = Directory.GetLastWriteTimeUtc(external);
        var link = Path.Combine(destination, "link");
        if (junction)
        {
            using var process = Process.Start(new ProcessStartInfo("cmd.exe", $"/c mklink /J \"{link}\" \"{external}\"")
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            })!;
            await process.WaitForExitAsync();
            await Assert.That(process.ExitCode).IsEqualTo(0);
        }
        else
        {
            SymbolicLinkTestHelper.CreateOrSkip(() => Directory.CreateSymbolicLink(link, external));
        }

        var archive = CreateArchive(root, entryName);
        var zip = new Zip(SystemFileSystemProvider.Instance, new PipelineWorkingDirectory(root));

        // A trailing separator must not conceal the link at the start of the entry path.
        await Assert.That(() => Extract(zip, archive, destination + Path.DirectorySeparatorChar, asynchronous, overwrite))
            .Throws<InvalidOperationException>();

        await Assert.That(Directory.GetFileSystemEntries(external)).IsEquivalentTo([externalFile]);
        await Assert.That(await File.ReadAllTextAsync(externalFile)).IsEqualTo("preserve");
        await Assert.That(File.GetLastWriteTimeUtc(externalFile)).IsEqualTo(lastWriteTime);
        await Assert.That(Directory.GetLastWriteTimeUtc(external)).IsEqualTo(directoryWriteTime);
    }

    private static async Task Extract(Zip zip, string archive, string destination, bool asynchronous, bool overwrite)
    {
        if (asynchronous)
        {
            await zip.ExtractToDirectoryAsync(archive, destination, overwrite);
        }
        else
        {
            zip.ExtractToDirectory(archive, destination, overwrite);
        }
    }

    private static string CreateArchive(string root, string entryName)
    {
        var path = Path.Combine(root, "archive.zip");
        using var archive = ZipFile.Open(path, ZipArchiveMode.Create);
        var entry = archive.CreateEntry(entryName);
        if (!entryName.EndsWith('/'))
        {
            using var writer = new StreamWriter(entry.Open());
            writer.Write("archive contents");
        }

        return path;
    }
}
