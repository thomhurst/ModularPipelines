using System.Formats.Tar;
using System.IO.Compression;
using ModularPipelines.Build.Helpers;

namespace ModularPipelines.UnitTests.Build;

public class BuildOutputArchiveTests
{
    [Test]
    public async Task Archive_Shares_Duplicate_Content_But_Restores_Independent_Files()
    {
        var source = Directory.CreateTempSubdirectory("build-archive-source-");
        var destination = Directory.CreateTempSubdirectory("build-archive-destination-");
        try
        {
            foreach (var name in new[] { "First", "Second", "Unused" })
            {
                var project = Directory.CreateDirectory(Path.Combine(source.FullName, name));
                await File.WriteAllTextAsync(Path.Combine(project.FullName, $"{name}.csproj"), "<Project />");
                var output = Directory.CreateDirectory(Path.Combine(project.FullName, "bin", "Release", "net10.0"));
                await File.WriteAllTextAsync(Path.Combine(output.FullName, "shared.dll"), "same dependency");
                await File.WriteAllTextAsync(Path.Combine(output.FullName, "distinct.dll"), name);
                if (!OperatingSystem.IsWindows())
                {
                    File.SetUnixFileMode(Path.Combine(output.FullName, "shared.dll"),
                        UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.UserExecute);
                }
            }

            await BuildOutputArchive.CreateAsync(source.FullName, ["First.csproj", "Second.csproj"], CancellationToken.None);
            var archivePath = Path.Combine(source.FullName, BuildOutputArchive.FileName);
            await using (var archive = File.OpenRead(archivePath))
            await using (var compressed = new GZipStream(archive, CompressionMode.Decompress))
            await using (var reader = new TarReader(compressed))
            {
                var regular = 0;
                var links = 0;
                while (await reader.GetNextEntryAsync() is { } entry)
                {
                    regular += entry.EntryType == TarEntryType.RegularFile ? 1 : 0;
                    links += entry.EntryType == TarEntryType.HardLink ? 1 : 0;
                }

                await Assert.That(regular).IsEqualTo(3);
                await Assert.That(links).IsEqualTo(1);
            }

            File.Copy(archivePath, Path.Combine(destination.FullName, BuildOutputArchive.FileName));
            await BuildOutputArchive.RestoreAsync(destination.FullName, CancellationToken.None);
            var first = Path.Combine(destination.FullName, "First", "bin", "Release", "net10.0", "shared.dll");
            var second = Path.Combine(destination.FullName, "Second", "bin", "Release", "net10.0", "shared.dll");
            await Assert.That(await File.ReadAllTextAsync(first)).IsEqualTo("same dependency");
            if (!OperatingSystem.IsWindows())
            {
                await Assert.That(File.GetUnixFileMode(first).HasFlag(UnixFileMode.UserExecute)).IsTrue();
                await Assert.That(File.GetUnixFileMode(second).HasFlag(UnixFileMode.UserExecute)).IsTrue();
            }
            await File.WriteAllTextAsync(first, "instrumented");
            await Assert.That(await File.ReadAllTextAsync(second)).IsEqualTo("same dependency");
            await Assert.That(Directory.Exists(Path.Combine(destination.FullName, "Unused"))).IsFalse();
            await Assert.That(await File.ReadAllTextAsync(Path.Combine(destination.FullName, "Second", "bin", "Release", "net10.0", "distinct.dll"))).IsEqualTo("Second");
        }
        finally
        {
            source.Delete(recursive: true);
            destination.Delete(recursive: true);
        }
    }

    [Test]
    public async Task Restore_Rejects_Archive_Paths_Outside_The_Extraction_Directory()
    {
        var repository = Directory.CreateTempSubdirectory("build-archive-traversal-");
        try
        {
            await using (var archive = File.Create(Path.Combine(repository.FullName, BuildOutputArchive.FileName)))
            await using (var compressed = new GZipStream(archive, CompressionLevel.Fastest))
            await using (var writer = new TarWriter(compressed))
            {
                await writer.WriteEntryAsync(new PaxTarEntry(TarEntryType.RegularFile, "../escaped.txt")
                {
                    DataStream = new MemoryStream([1, 2, 3]),
                });
            }

            await Assert.That(() => BuildOutputArchive.RestoreAsync(repository.FullName, CancellationToken.None))
                .Throws<IOException>();
        }
        finally
        {
            repository.Delete(recursive: true);
        }
    }

    [Test]
    public async Task Missing_Test_Project_Fails_Before_Publishing()
    {
        var repository = Directory.CreateTempSubdirectory("build-archive-missing-");
        try
        {
            await Assert.That(() => BuildOutputArchive.CreateAsync(repository.FullName, ["Missing.csproj"], CancellationToken.None))
                .Throws<InvalidOperationException>();
        }
        finally
        {
            repository.Delete(recursive: true);
        }
    }
}
