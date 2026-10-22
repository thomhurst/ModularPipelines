using System.Diagnostics;
using System.IO.Compression;
using System.Text;
using ModularPipelines.Distributed.Artifacts;
using ModularPipelines.FileSystem;
using TUnit.Core.Exceptions;

namespace ModularPipelines.UnitTests.Artifacts;

public class BoundArtifactPathTests
{
    [Test]
    public async Task Source_Read_Stays_In_Bound_Directory_After_Ancestor_Substitution()
    {
        var root = Directory.CreateTempSubdirectory("artifact-bound-source-");
        try
        {
            var parent = root.CreateSubdirectory("parent").FullName;
            var source = Directory.CreateDirectory(Path.Combine(parent, "source")).FullName;
            var outside = root.CreateSubdirectory("outside").FullName;
            Directory.CreateDirectory(Path.Combine(outside, "source"));
            await File.WriteAllTextAsync(Path.Combine(source, "payload-λ.txt"), "inside");
            await File.WriteAllTextAsync(Path.Combine(outside, "source", "payload-λ.txt"), "outside");
            using (var bound = BoundArtifactDirectory.Open(source, create: false))
            {
                SubstituteAncestor(parent, outside);
                await using var stream = bound.OpenRead("payload-λ.txt");
                using var reader = new StreamReader(stream);
                await Assert.That(await reader.ReadToEndAsync()).IsEqualTo("inside");
            }
        }
        finally
        {
            RemoveSubstitutedLink(root.FullName);
            root.Delete(recursive: true);
        }
    }

    [Test]
    public async Task Move_Stays_In_Bound_Directory_After_Temporary_Stream_Is_Closed()
    {
        var root = Directory.CreateTempSubdirectory("artifact-bound-move-");
        try
        {
            var parent = root.CreateSubdirectory("parent").FullName;
            var destination = Directory.CreateDirectory(Path.Combine(parent, "destination")).FullName;
            var outside = root.CreateSubdirectory("outside").FullName;
            var outsideDestination = Directory.CreateDirectory(Path.Combine(outside, "destination")).FullName;
            await File.WriteAllTextAsync(Path.Combine(outsideDestination, "payload.txt"), "outside");
            var substituted = false;
            using (var bound = BoundArtifactDirectory.Open(destination, create: false))
            {
                await using (var output = bound.CreateNew("temporary"))
                {
                    await output.WriteAsync("replacement"u8.ToArray());
                }
                substituted = SubstituteAncestor(parent, outside);
                bound.Move("temporary", "payload.txt");
            }
            var actual = substituted ? Path.Combine(parent + "-original", "destination") : destination;
            await Assert.That(await File.ReadAllTextAsync(Path.Combine(actual, "payload.txt"))).IsEqualTo("replacement");
            await Assert.That(await File.ReadAllTextAsync(Path.Combine(outsideDestination, "payload.txt"))).IsEqualTo("outside");
        }
        finally
        {
            RemoveSubstitutedLink(root.FullName);
            root.Delete(recursive: true);
        }
    }

    [Test]
    public async Task Trusted_Alias_Is_Resolved_Once_Before_Read()
    {
        var root = Directory.CreateTempSubdirectory("artifact-bound-alias-");
        try
        {
            var source = root.CreateSubdirectory("source").FullName;
            var outside = root.CreateSubdirectory("outside").FullName;
            var alias = Path.Combine(root.FullName, "alias");
            await File.WriteAllTextAsync(Path.Combine(source, "payload.txt"), "inside");
            await File.WriteAllTextAsync(Path.Combine(outside, "payload.txt"), "outside");
            CreateDirectoryLink(alias, source);
            using (var bound = BoundArtifactDirectory.Open(alias, create: false, trustedRoot: alias))
            {
                Directory.Delete(alias);
                CreateDirectoryLink(alias, outside);
                await using var input = bound.OpenRead("payload.txt");
                using var reader = new StreamReader(input);
                await Assert.That(await reader.ReadToEndAsync()).IsEqualTo("inside");
            }
        }
        finally
        {
            root.Delete(recursive: true);
        }
    }

    [Test]
    public async Task Source_Open_Rejects_Leaf_Link_Inserted_After_Validation()
    {
        var root = Directory.CreateTempSubdirectory("artifact-bound-leaf-");
        try
        {
            var source = root.CreateSubdirectory("source").FullName;
            var file = Path.Combine(source, "payload.txt");
            var outside = Path.Combine(root.FullName, "outside.txt");
            await File.WriteAllTextAsync(file, "inside");
            await File.WriteAllTextAsync(outside, "outside");
            using (var bound = BoundArtifactDirectory.Open(source, create: false))
            {
                ArtifactContextImpl.EnsureSourcePathContainsNoLinks(SystemFileSystemProvider.Instance, file);
                File.Delete(file);
                try
                {
                    File.CreateSymbolicLink(file, outside);
                }
                catch (IOException exception) when (OperatingSystem.IsWindows() && exception.HResult == unchecked((int) 0x80070522))
                {
                    throw new SkipTestException("Symbolic-link creation requires an unavailable Windows privilege.");
                }
                await Assert.That(() => bound.OpenRead("payload.txt")).Throws<IOException>();
            }
            await Assert.That(await File.ReadAllTextAsync(outside)).IsEqualTo("outside");
        }
        finally
        {
            root.Delete(recursive: true);
        }
    }

    [Test]
    [Arguments(false, false, false)]
    [Arguments(true, false, false)]
    [Arguments(false, true, false)]
    [Arguments(true, true, false)]
    [Arguments(false, false, true)]
    [Arguments(false, true, true)]
    public async Task Restore_And_Cleanup_Stay_In_Bound_Directory_During_Copy(bool cancel, bool zip, bool replaceLeaf)
    {
        var root = Directory.CreateTempSubdirectory("artifact-bound-restore-");
        try
        {
            var parent = root.CreateSubdirectory("parent").FullName;
            var destination = Directory.CreateDirectory(Path.Combine(parent, "destination")).FullName;
            var outside = root.CreateSubdirectory("outside").FullName;
            var outsideDestination = Directory.CreateDirectory(Path.Combine(outside, "destination")).FullName;
            await File.WriteAllTextAsync(Path.Combine(destination, "payload.txt"), "original");
            await File.WriteAllTextAsync(Path.Combine(outsideDestination, "payload.txt"), "outside");
            using var cancellation = new CancellationTokenSource();
            var substituted = false;
            var attempted = false;
            void Substitute()
            {
                attempted = true;
                if (replaceLeaf)
                {
                    // A concurrently inserted destination link must be replaced itself,
                    // never followed to the outside file.
                    File.Delete(Path.Combine(destination, "payload.txt"));
                    try
                    {
                        File.CreateSymbolicLink(Path.Combine(destination, "payload.txt"), Path.Combine(outsideDestination, "payload.txt"));
                    }
                    catch (IOException exception) when (OperatingSystem.IsWindows() && exception.HResult == unchecked((int) 0x80070522))
                    {
                        throw new SkipTestException("Symbolic-link creation requires an unavailable Windows privilege.");
                    }
                }
                else
                {
                    substituted = SubstituteAncestor(parent, outside);
                }
                if (cancel)
                {
                    cancellation.Cancel();
                }
            }
            using var payload = new MemoryStream();
            if (zip)
            {
                using (var archive = new ZipArchive(payload, ZipArchiveMode.Create, leaveOpen: true))
                using (var writer = new StreamWriter(archive.CreateEntry("payload.txt", CompressionLevel.NoCompression).Open()))
                {
                    writer.Write("replacement");
                }
                payload.Position = 0;
            }
            using var input = new SubstitutingStream(Substitute);
            using var archiveInput = new SubstitutingArchiveStream(payload.ToArray(), () =>
            {
                if (!Directory.GetFiles(destination, ".modularpipelines-*.tmp").Any())
                {
                    return false;
                }
                Substitute();
                return true;
            });
            using var readArchive = zip ? new ZipArchive(archiveInput, ZipArchiveMode.Read) : null;
            var task = readArchive is not null
                ? ArtifactContextImpl.ExtractDirectoryArchiveAsync(readArchive, destination, cancellation.Token)
                : ArtifactContextImpl.RestoreFileAsync(input, Path.Combine(destination, "payload.txt"), cancellation.Token);
            if (cancel)
            {
                await Assert.That(async () => await task).Throws<OperationCanceledException>();
            }
            else
            {
                await task;
            }
            await Assert.That(attempted).IsTrue();
            var actualDestination = substituted ? Path.Combine(parent + "-original", "destination") : destination;
            await Assert.That(await File.ReadAllTextAsync(Path.Combine(actualDestination, "payload.txt")))
                .IsEqualTo(cancel ? "original" : "replacement");
            await Assert.That(Directory.GetFiles(actualDestination, ".modularpipelines-*.tmp")).IsEmpty();
            await Assert.That(await File.ReadAllTextAsync(Path.Combine(outsideDestination, "payload.txt"))).IsEqualTo("outside");
            await Assert.That(Directory.GetFiles(outsideDestination).Select(path => Path.GetFileName(path)!)).IsEquivalentTo(["payload.txt"]);
        }
        finally
        {
            RemoveSubstitutedLink(root.FullName);
            root.Delete(recursive: true);
        }
    }

    // This runs after the directory handles have been validated and before file I/O.
    // Windows must deny the rename; Unix must continue relative to the original handle.
    private static bool SubstituteAncestor(string parent, string outside)
    {
        try
        {
            Directory.Move(parent, parent + "-original");
        }
        catch (IOException) when (OperatingSystem.IsWindows())
        {
            return false;
        }
        CreateDirectoryLink(parent, outside);
        return true;
    }

    private static void CreateDirectoryLink(string link, string target)
    {
        if (!OperatingSystem.IsWindows())
        {
            Directory.CreateSymbolicLink(link, target);
            return;
        }

        // Junctions exercise Windows ancestor replacement without symlink privileges.
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
    }

    private static void RemoveSubstitutedLink(string root)
    {
        var parent = Path.Combine(root, "parent");
        if (Directory.Exists(parent + "-original"))
        {
            Directory.Delete(parent);
        }
    }

    private sealed class SubstitutingArchiveStream(byte[] contents, Func<bool> substitute) : MemoryStream(contents)
    {
        private bool _substituted;

        private void BeforeRead()
        {
            if (!_substituted)
            {
                _substituted = substitute();
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            BeforeRead();
            return base.Read(buffer, offset, count);
        }

        public override int Read(Span<byte> buffer)
        {
            BeforeRead();
            return base.Read(buffer);
        }

        public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
        {
            BeforeRead();
            return base.ReadAsync(buffer, cancellationToken);
        }
    }

    private sealed class SubstitutingStream(Action substitute) : MemoryStream(Encoding.UTF8.GetBytes("replacement"))
    {
        public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
        {
            substitute();
            return base.CopyToAsync(destination, bufferSize, cancellationToken);
        }
    }
}
