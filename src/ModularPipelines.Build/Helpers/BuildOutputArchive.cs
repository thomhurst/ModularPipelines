using System.Formats.Tar;
using System.IO.Compression;
using System.Security.Cryptography;

namespace ModularPipelines.Build.Helpers;

internal static class BuildOutputArchive
{
    public const string FileName = "_build-output.tar.gz";

    public static async Task CreateAsync(string repositoryRoot, IEnumerable<string> projectFileNames, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var projects = projectFileNames.ToHashSet(StringComparer.Ordinal);
        var originals = new Dictionary<(string Hash, UnixFileMode Mode), string>();
        await using var output = File.Create(Path.Combine(repositoryRoot, FileName));
        await using var compressed = new GZipStream(output, CompressionLevel.SmallestSize);
        await using var writer = new TarWriter(compressed, leaveOpen: true);
        foreach (var project in Directory.EnumerateFiles(repositoryRoot, "*.csproj", SearchOption.AllDirectories))
        {
            if (!projects.Remove(Path.GetFileName(project)))
            {
                continue;
            }

            var releaseDirectory = Path.Combine(Path.GetDirectoryName(project)!, "bin", "Release");
            foreach (var file in Directory.EnumerateFiles(releaseDirectory, "*", SearchOption.AllDirectories))
            {
                cancellationToken.ThrowIfCancellationRequested();
                await using var source = File.OpenRead(file);
                var hash = Convert.ToHexString(await SHA256.HashDataAsync(source, cancellationToken).ConfigureAwait(false));
                var mode = OperatingSystem.IsWindows()
                    ? UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.GroupRead | UnixFileMode.OtherRead
                    : File.GetUnixFileMode(file);
                var name = Path.GetRelativePath(repositoryRoot, file).Replace(Path.DirectorySeparatorChar, '/');
                var duplicate = originals.TryGetValue((hash, mode), out var original);
                var entry = new PaxTarEntry(duplicate ? TarEntryType.HardLink : TarEntryType.RegularFile, name)
                {
                    Mode = mode,
                    ModificationTime = File.GetLastWriteTimeUtc(file),
                };
                if (duplicate)
                {
                    entry.LinkName = original!;
                }
                else
                {
                    originals.Add((hash, mode), name);
                    source.Position = 0;
                    entry.DataStream = source;
                }

                await writer.WriteEntryAsync(entry, cancellationToken).ConfigureAwait(false);
            }
        }

        if (projects.Count != 0)
        {
            throw new InvalidOperationException($"Test projects were not found: {string.Join(", ", projects)}");
        }

        if (originals.Count == 0)
        {
            throw new InvalidOperationException("No test build outputs were found to share.");
        }
    }

    public static async Task RestoreAsync(string repositoryRoot, CancellationToken cancellationToken)
    {
        var temporary = Directory.CreateTempSubdirectory("modularpipelines-build-output-");
        try
        {
            await using var archive = File.OpenRead(Path.Combine(repositoryRoot, FileName));
            await using var compressed = new GZipStream(archive, CompressionMode.Decompress);
            await TarFile.ExtractToDirectoryAsync(compressed, temporary.FullName, overwriteFiles: false, cancellationToken).ConfigureAwait(false);
            foreach (var file in Directory.EnumerateFiles(temporary.FullName, "*", SearchOption.AllDirectories))
            {
                cancellationToken.ThrowIfCancellationRequested();
                var relativePath = Path.GetRelativePath(temporary.FullName, file);
                var destination = Path.Combine(repositoryRoot, relativePath);
                // Reject existing links before copying into the checkout. TAR extraction
                // already validates archive paths and link targets inside the fresh directory.
                for (var path = destination; path is not null; path = Path.GetDirectoryName(path))
                {
                    if (new FileInfo(path).LinkTarget is not null
                        || ((File.Exists(path) || Directory.Exists(path))
                            && (File.GetAttributes(path) & FileAttributes.ReparsePoint) != 0))
                    {
                        throw new IOException($"Build output destination contains a link: {path}");
                    }
                }

                Directory.CreateDirectory(Path.GetDirectoryName(destination)!);
                // Materialize independent files: coverage tools may rewrite assemblies.
                File.Copy(file, destination, overwrite: true);
            }
        }
        finally
        {
            temporary.Delete(recursive: true);
        }
    }
}
