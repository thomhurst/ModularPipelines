using System.Diagnostics;
using System.IO.Compression;
using System.Text;
using Microsoft.Extensions.Logging;
using ModularPipelines.Distributed.Artifacts;
using Moq;
using TUnit.Core.Exceptions;

namespace ModularPipelines.Distributed.UnitTests.Artifacts;

public class ArtifactLifecycleLinkTests
{
    [Test]
    [Arguments("file", false)]
    [Arguments("directory", false)]
    [Arguments("nested-file", false)]
    [Arguments("nested-directory", false)]
    [Arguments("ancestor", false)]
    [Arguments("glob-file", false)]
    [Arguments("glob-directory", false)]
    [Arguments("glob-ancestor", false)]
    [Arguments("dangling-file", false)]
    [Arguments("directory", true)]
    [Arguments("nested-directory", true)]
    [Arguments("ancestor", true)]
    [Arguments("glob-directory", true)]
    [Arguments("glob-ancestor", true)]
    [Arguments("cycle", true)]
    public async Task Upload_Rejects_Links_Without_Uploading_Outside_Contents(string scenario, bool junction)
    {
        var root = Directory.CreateTempSubdirectory("artifact-source-links-");
        try
        {
            var outside = root.CreateSubdirectory("outside");
            var payload = Path.Combine(outside.FullName, "payload.txt");
            await File.WriteAllTextAsync(payload, "outside contents");
            var working = root.CreateSubdirectory("working").FullName;
            var source = Path.Combine(working, "test-output");
            var moduleType = typeof(ProducerModule);
            switch (scenario)
            {
                case "file":
                case "dangling-file":
                    CreateLink(() => File.CreateSymbolicLink(source,
                        scenario == "file" ? payload : Path.Combine(outside.FullName, "missing")));
                    break;
                case "directory":
                    CreateLink(() => DirectoryLink(source, outside.FullName, junction));
                    break;
                case "nested-file":
                case "nested-directory":
                case "cycle":
                    Directory.CreateDirectory(source);
                    var nested = Path.Combine(source, "nested");
                    CreateLink(() => scenario == "nested-file"
                        ? File.CreateSymbolicLink(nested, payload)
                        : DirectoryLink(nested, scenario == "cycle" ? source : outside.FullName, junction));
                    break;
                case "ancestor":
                    working = Path.Combine(root.FullName, "alias", "working");
                    Directory.CreateDirectory(Path.Combine(outside.FullName, "working"));
                    await File.WriteAllTextAsync(Path.Combine(outside.FullName, "working", "test-output"), "outside contents");
                    CreateLink(() => DirectoryLink(Path.Combine(root.FullName, "alias"), outside.FullName, junction));
                    break;
                case "glob-file":
                case "glob-directory":
                    moduleType = typeof(SingleGlobProducerModule);
                    var globPath = Path.Combine(working, "single-match-link");
                    CreateLink(() => scenario == "glob-file"
                        ? File.CreateSymbolicLink(globPath, payload)
                        : DirectoryLink(globPath, outside.FullName, junction));
                    break;
                case "glob-ancestor":
                    moduleType = typeof(SingleFileGlobProducerModule);
                    await File.WriteAllTextAsync(Path.Combine(outside.FullName, "manifest.json"), "outside contents");
                    CreateLink(() => DirectoryLink(Path.Combine(working, "release-v1"), outside.FullName, junction));
                    break;
            }

            var store = new InMemoryDistributedArtifactStore();
            var manager = Manager(store, working);
            await Assert.ThrowsAsync<IOException>(() => manager.UploadProducedArtifactsAsync(moduleType, CancellationToken.None));
            await Assert.That(await store.ListArtifactsAsync(ModuleId.FromType(moduleType), CancellationToken.None)).IsEmpty();
            await Assert.That(await File.ReadAllTextAsync(payload)).IsEqualTo("outside contents");
        }
        finally
        {
            root.Delete(recursive: true);
        }
    }

    [Test]
    [Arguments(false, "file", false)]
    [Arguments(false, "root", false)]
    [Arguments(false, "ancestor", false)]
    [Arguments(true, "file", false)]
    [Arguments(true, "directory", false)]
    [Arguments(true, "root", false)]
    [Arguments(true, "ancestor", false)]
    [Arguments(false, "root", true)]
    [Arguments(false, "ancestor", true)]
    [Arguments(true, "directory", true)]
    [Arguments(true, "root", true)]
    [Arguments(true, "ancestor", true)]
    public async Task Restore_Rejects_Links_Without_Changing_Outside_Contents(bool zip, string scenario, bool junction)
    {
        var root = Directory.CreateTempSubdirectory("artifact-destination-links-");
        try
        {
            var working = root.CreateSubdirectory("working").FullName;
            var outside = root.CreateSubdirectory("outside");
            var payload = Path.Combine(outside.FullName, "payload.txt");
            await File.WriteAllTextAsync(payload, "outside contents");
            var entryName = scenario == "directory" ? "nested/payload.txt" : "payload.txt";
            if (scenario == "ancestor")
            {
                working = Path.Combine(root.FullName, "alias", "new-working");
                CreateLink(() => DirectoryLink(Path.Combine(root.FullName, "alias"), outside.FullName, junction));
            }
            else if (scenario == "root")
            {
                CreateLink(() => DirectoryLink(Path.Combine(working, "restored"), outside.FullName, junction));
            }
            else
            {
                var restored = Directory.CreateDirectory(Path.Combine(working, "restored"));
                CreateLink(() => scenario == "file"
                    ? File.CreateSymbolicLink(Path.Combine(restored.FullName, zip ? "payload.txt" : "build-output"), payload)
                    : DirectoryLink(Path.Combine(restored.FullName, "nested"), outside.FullName, junction));
            }

            var store = new InMemoryDistributedArtifactStore();
            using var stream = Payload(zip, entryName);
            await store.UploadAsync(new ArtifactDescriptor
            {
                Name = "build-output",
                ModuleId = ModuleId.FromType(typeof(ProducerModule)),
                ContentType = zip ? "application/zip" : "application/octet-stream",
            }, stream, CancellationToken.None);
            var manager = Manager(store, working);

            await Assert.That(() => manager.DownloadConsumedArtifactsAsync(typeof(ConsumerModule), CancellationToken.None)).Throws<IOException>();
            await Assert.That(await File.ReadAllTextAsync(payload)).IsEqualTo("outside contents");
            await Assert.That(outside.EnumerateFileSystemInfos().Select(entry => entry.Name)).IsEquivalentTo(["payload.txt"]);
            await Assert.That(Directory.GetFiles(root.FullName, ".modularpipelines-*.tmp", SearchOption.AllDirectories)).IsEmpty();
        }
        finally
        {
            root.Delete(recursive: true);
        }
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Ordinary_Artifacts_Roundtrip(bool directory)
    {
        var root = Directory.CreateTempSubdirectory("artifact-roundtrip-");
        try
        {
            var source = Path.Combine(root.FullName, "test-output");
            if (directory)
            {
                Directory.CreateDirectory(Path.Combine(source, "empty"));
                source = Path.Combine(source, "payload.txt");
            }
            await File.WriteAllTextAsync(source, "ordinary contents");
            var manager = Manager(new InMemoryDistributedArtifactStore(), root.FullName);
            await manager.UploadProducedArtifactsAsync(typeof(ProducerModule), CancellationToken.None);
            await manager.DownloadConsumedArtifactsAsync(typeof(ConsumerModule), CancellationToken.None);
            var restored = Path.Combine(root.FullName, "restored");
            await Assert.That(await File.ReadAllTextAsync(Path.Combine(restored, directory ? "payload.txt" : "build-output"))).IsEqualTo("ordinary contents");
            if (directory)
            {
                await Assert.That(Directory.Exists(Path.Combine(restored, "empty"))).IsTrue();
            }
        }
        finally
        {
            root.Delete(recursive: true);
        }
    }

    [Test]
    public async Task Restore_Rejects_Archive_Traversal()
    {
        var root = Directory.CreateTempSubdirectory("artifact-traversal-");
        try
        {
            var outside = Path.Combine(root.FullName, "payload.txt");
            await File.WriteAllTextAsync(outside, "outside contents");
            var store = new InMemoryDistributedArtifactStore();
            using var stream = Payload(true, "../payload.txt");
            await store.UploadAsync(new ArtifactDescriptor
            {
                Name = "build-output",
                ModuleId = ModuleId.FromType(typeof(ProducerModule)),
                ContentType = "application/zip",
            }, stream, CancellationToken.None);
            var manager = Manager(store, root.FullName);
            await Assert.That(() => manager.DownloadConsumedArtifactsAsync(typeof(ConsumerModule), CancellationToken.None)).Throws<IOException>();
            await Assert.That(await File.ReadAllTextAsync(outside)).IsEqualTo("outside contents");
        }
        finally
        {
            root.Delete(recursive: true);
        }
    }

    [Test]
    [Arguments(typeof(ProducerModule), "test-output/payload.txt", false)]
    [Arguments(typeof(ProducerModule), "test-output/payload.txt", true)]
    [Arguments(typeof(SingleGlobProducerModule), "single-match-directory/payload.txt", false)]
    [Arguments(typeof(SingleGlobProducerModule), "single-match-directory/payload.txt", true)]
    public async Task Upload_Deletes_Temporary_Archive_When_Store_Succeeds_Or_Fails(
        Type moduleType, string relativePath, bool failUpload)
    {
        var root = Directory.CreateTempSubdirectory("artifact-archive-cleanup-");
        try
        {
            var source = Path.Combine(root.FullName, relativePath);
            Directory.CreateDirectory(Path.GetDirectoryName(source)!);
            await File.WriteAllTextAsync(source, "payload");
            string? temporaryArchive = null;
            var store = new Mock<IDistributedArtifactStore>();
            store.Setup(value => value.UploadAsync(It.IsAny<ArtifactDescriptor>(), It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .Returns((ArtifactDescriptor descriptor, Stream stream, CancellationToken _) =>
                {
                    temporaryArchive = ((FileStream) stream).Name;
                    return failUpload
                        ? Task.FromException<ArtifactReference>(new IOException("Store upload failed."))
                        : Task.FromResult(new ArtifactReference
                        {
                            ArtifactId = "uploaded",
                            Name = descriptor.Name,
                            ModuleId = descriptor.ModuleId,
                            SizeBytes = stream.Length,
                            ContentType = descriptor.ContentType,
                            UploadedAt = DateTimeOffset.UtcNow,
                        });
                });
            var manager = Manager(store.Object, root.FullName);
            if (failUpload)
            {
                await Assert.ThrowsAsync<IOException>(() => manager.UploadProducedArtifactsAsync(moduleType, CancellationToken.None));
            }
            else
            {
                await manager.UploadProducedArtifactsAsync(moduleType, CancellationToken.None);
            }
            await Assert.That(temporaryArchive).IsNotNull();
            await Assert.That(File.Exists(temporaryArchive)).IsFalse();
        }
        finally
        {
            root.Delete(recursive: true);
        }
    }

    [Test]
    public async Task Failed_Single_File_Restore_Preserves_Existing_File_And_Removes_Temporary_File()
    {
        var root = Directory.CreateTempSubdirectory("artifact-file-cleanup-");
        try
        {
            var restored = root.CreateSubdirectory("restored");
            var destination = Path.Combine(restored.FullName, "build-output");
            await File.WriteAllTextAsync(destination, "existing contents");
            var reference = new ArtifactReference
            {
                ArtifactId = "uploaded",
                Name = "build-output",
                ModuleId = ModuleId.FromType(typeof(ProducerModule)),
                SizeBytes = 7,
                ContentType = "application/octet-stream",
                UploadedAt = DateTimeOffset.UtcNow,
            };
            var store = new Mock<IDistributedArtifactStore>();
            store.Setup(value => value.ListArtifactsAsync(reference.ModuleId, It.IsAny<CancellationToken>())).ReturnsAsync([reference]);
            store.Setup(value => value.DownloadAsync(reference, It.IsAny<CancellationToken>())).ReturnsAsync(new FailingReadStream());
            var manager = Manager(store.Object, root.FullName);
            await Assert.ThrowsAsync<IOException>(() => manager.DownloadConsumedArtifactsAsync(typeof(ConsumerModule), CancellationToken.None));
            await Assert.That(await File.ReadAllTextAsync(destination)).IsEqualTo("existing contents");
            await Assert.That(Directory.GetFiles(restored.FullName)).IsEquivalentTo([destination]);
        }
        finally
        {
            root.Delete(recursive: true);
        }
    }

    private sealed class FailingReadStream : MemoryStream
    {
        public override async Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
        {
            await destination.WriteAsync("partial"u8.ToArray(), cancellationToken);
            throw new IOException("Download failed after a partial copy.");
        }
    }

    private static ArtifactLifecycleManager Manager(IDistributedArtifactStore store, string working) => new(
        store, Microsoft.Extensions.Options.Options.Create(new DistributedOptions()),
        Mock.Of<ILogger<ArtifactLifecycleManager>>(), working);

    private static MemoryStream Payload(bool zip, string entryName)
    {
        var stream = new MemoryStream();
        if (zip)
        {
            using var archive = new ZipArchive(stream, ZipArchiveMode.Create, leaveOpen: true);
            using var writer = new StreamWriter(archive.CreateEntry(entryName).Open());
            writer.Write("replacement");
        }
        else
        {
            stream.Write(Encoding.UTF8.GetBytes("replacement"));
        }
        stream.Position = 0;
        return stream;
    }

    private static FileSystemInfo DirectoryLink(string link, string target, bool junction)
    {
        if (!junction)
        {
            return Directory.CreateSymbolicLink(link, target);
        }
        if (!OperatingSystem.IsWindows())
        {
            throw new SkipTestException("Junction regression requires Windows.");
        }
        using var process = Process.Start(new ProcessStartInfo("cmd.exe", $"/c mklink /J \"{link}\" \"{target}\"")
        {
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
        })!;
        if (!process.WaitForExit(10_000))
        {
            process.Kill(entireProcessTree: true);
            throw new System.TimeoutException("Junction creation timed out.");
        }
        if (process.ExitCode != 0)
        {
            throw new IOException(process.StandardError.ReadToEnd());
        }
        return new DirectoryInfo(link);
    }

    private static void CreateLink(Func<FileSystemInfo> createLink)
    {
        try
        {
            createLink();
        }
        catch (IOException exception) when (OperatingSystem.IsWindows() && exception.HResult == unchecked((int) 0x80070522))
        {
            throw new SkipTestException("Symbolic-link creation requires an unavailable Windows privilege (ERROR_PRIVILEGE_NOT_HELD).");
        }
    }
}
