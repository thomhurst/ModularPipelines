using ModularPipelines.FileSystem;

namespace ModularPipelines.UnitTests.FileSystem;

public class FileSystemProviderMetadataTests
{
    [Test]
    [Arguments(DateTimeKind.Utc)]
    [Arguments(DateTimeKind.Unspecified)]
    public async Task Path_Timestamps_Interpret_Provider_Values_As_Utc(DateTimeKind kind)
    {
        var provider = new FakeFileSystemProvider();
        var folder = new FolderPath(Path.Combine(FakeFileSystemProvider.Root, "timestamps"), provider);
        var file = new FilePath(Path.Combine(folder.Path, "file.txt"), provider);
        provider.CreateDirectory(folder.Path);
        provider.AddFile(file.Path, []);
        var created = new DateTime(2020, 7, 2, 3, 4, 5, kind);
        var written = new DateTime(2021, 8, 3, 4, 5, 6, kind);
        foreach (var path in new[] { file.Path, folder.Path })
        {
            provider.SetCreationTimeUtc(path, created);
            provider.SetLastWriteTimeUtc(path, written);
        }

        foreach (var (creationTime, lastWriteTime) in new[]
        {
            (file.CreationTime, file.LastWriteTime),
            (folder.CreationTime, folder.LastWriteTime),
        })
        {
            await Assert.That(creationTime.Offset).IsEqualTo(TimeSpan.Zero);
            await Assert.That(lastWriteTime.Offset).IsEqualTo(TimeSpan.Zero);
            await Assert.That(creationTime.UtcDateTime).IsEqualTo(DateTime.SpecifyKind(created, DateTimeKind.Utc));
            await Assert.That(lastWriteTime.UtcDateTime).IsEqualTo(DateTime.SpecifyKind(written, DateTimeKind.Utc));
        }
    }

    private static readonly DateTime Created = new(2020, 1, 2, 3, 4, 5, DateTimeKind.Utc);
    private static readonly DateTime Written = new(2021, 2, 3, 4, 5, 6, DateTimeKind.Utc);
    private static readonly DateTime Accessed = new(2022, 3, 4, 5, 6, 7, DateTimeKind.Utc);

    [Test]
    public async Task FilePath_Metadata_Is_Read_And_Written_Through_Provider()
    {
        var provider = new FakeFileSystemProvider();
        var path = Path.Combine(FakeFileSystemProvider.Root, "file.txt");
        provider.AddFile(path, "contents"u8.ToArray());
        provider.SetCreationTimeUtc(path, Created);
        provider.SetLastWriteTimeUtc(path, Written);
        var file = new FilePath(path, provider);

        file.Attributes = FileAttributes.Hidden | FileAttributes.ReadOnly;

        using (Assert.Multiple())
        {
            await Assert.That(provider.GetAttributes(path)).IsEqualTo(FileAttributes.Hidden | FileAttributes.ReadOnly);
            await Assert.That(file.Hidden).IsTrue();
            await Assert.That(file.IsReadOnly).IsTrue();
            await Assert.That(file.CreationTime.UtcDateTime).IsEqualTo(Created);
            await Assert.That(file.LastWriteTime.UtcDateTime).IsEqualTo(Written);
            await Assert.That(file.CreationTime.Offset).IsEqualTo(TimeSpan.Zero);
            await Assert.That(file.LastWriteTime.Offset).IsEqualTo(TimeSpan.Zero);
            await Assert.That(file.Length).IsEqualTo(8);
        }
    }

    [Test]
    public async Task FolderPath_Metadata_Is_Read_And_Written_Through_Provider()
    {
        var provider = new FakeFileSystemProvider();
        var path = Path.Combine(FakeFileSystemProvider.Root, "folder");
        provider.CreateDirectory(path);
        provider.SetCreationTimeUtc(path, Created);
        provider.SetLastWriteTimeUtc(path, Written);
        var folder = new FolderPath(path, provider);

        folder.Attributes = FileAttributes.Directory | FileAttributes.Hidden;

        using (Assert.Multiple())
        {
            await Assert.That(folder.Hidden).IsTrue();
            await Assert.That(folder.CreationTime.UtcDateTime).IsEqualTo(Created);
            await Assert.That(folder.LastWriteTime.UtcDateTime).IsEqualTo(Written);
            await Assert.That(folder.CreationTime.Offset).IsEqualTo(TimeSpan.Zero);
            await Assert.That(folder.LastWriteTime.Offset).IsEqualTo(TimeSpan.Zero);
        }
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task FolderPath_CopyTo_Preserves_Timestamps_With_Custom_Provider(bool useAsync)
    {
        var provider = new FakeFileSystemProvider();
        var source = Path.Combine(FakeFileSystemProvider.Root, "source");
        var nested = Path.Combine(source, "nested");
        var file = Path.Combine(nested, "file.txt");
        var target = Path.Combine(FakeFileSystemProvider.Root, "target");
        provider.CreateDirectory(nested);
        provider.AddFile(file, "contents"u8.ToArray());
        foreach (var entry in new[] { source, nested, file })
        {
            provider.SetCreationTimeUtc(entry, Created);
            provider.SetLastWriteTimeUtc(entry, Written);
            provider.SetLastAccessTimeUtc(entry, Accessed);
        }

        provider.SetAttributes(file, FileAttributes.ReadOnly);
        var folder = new FolderPath(source, provider);

        if (useAsync)
        {
            await folder.CopyToAsync(target, preserveTimestamps: true);
        }
        else
        {
            folder.CopyTo(target, preserveTimestamps: true);
        }

        var copiedFile = Path.Combine(target, "nested", "file.txt");
        using (Assert.Multiple())
        {
            await Assert.That(await ((IFileSystemProvider) provider).ReadAllTextAsync(copiedFile)).IsEqualTo("contents");
            await Assert.That(provider.GetAttributes(copiedFile)).IsEqualTo(FileAttributes.ReadOnly);
            foreach (var entry in new[] { target, Path.Combine(target, "nested"), copiedFile })
            {
                await Assert.That(provider.GetCreationTimeUtc(entry)).IsEqualTo(Created);
                await Assert.That(provider.GetLastWriteTimeUtc(entry)).IsEqualTo(Written);
                await Assert.That(provider.GetLastAccessTimeUtc(entry)).IsEqualTo(Accessed);
            }
        }
    }

    [Test]
    public async Task FolderPath_Clean_Removes_ReadOnly_Attributes_Through_Provider()
    {
        var provider = new FakeFileSystemProvider();
        var root = Path.Combine(FakeFileSystemProvider.Root, "clean");
        var child = Path.Combine(root, "child");
        var nestedFile = Path.Combine(child, "nested.txt");
        var topFile = Path.Combine(root, "top.txt");
        provider.CreateDirectory(child);
        provider.AddFile(nestedFile, [1]);
        provider.AddFile(topFile, [1]);
        provider.SetAttributes(child, FileAttributes.Directory | FileAttributes.ReadOnly);
        provider.SetAttributes(nestedFile, FileAttributes.ReadOnly);
        provider.SetAttributes(topFile, FileAttributes.ReadOnly);

        new FolderPath(root, provider).Clean(removeReadOnlyAttribute: true);

        using (Assert.Multiple())
        {
            await Assert.That(provider.ClearedReadOnly).Contains(child);
            await Assert.That(provider.ClearedReadOnly).Contains(nestedFile);
            await Assert.That(provider.ClearedReadOnly).Contains(topFile);
            await Assert.That(provider.DirectoryExists(child)).IsFalse();
            await Assert.That(provider.FileExists(topFile)).IsFalse();
        }
    }

    [Test]
    public async Task Default_Members_Are_Built_On_Primitives()
    {
        IFileSystemProvider provider = new FakeFileSystemProvider();
        var path = provider.Combine(FakeFileSystemProvider.Root, "defaults.txt");
        var copy = provider.Combine(FakeFileSystemProvider.Root, "copy.txt");

        await provider.WriteAllTextAsync(path, "first");
        await provider.AppendAllTextAsync(path, "-second");
        await provider.AppendAllLinesAsync(path, ["", "line"]);
        provider.CopyFile(path, copy, overwrite: false);

        var lines = new List<string>();
        await foreach (var line in provider.ReadLinesAsync(copy))
        {
            lines.Add(line);
        }

        await provider.WriteAllBytesAsync(path, [1, 2, 3]);

        using (Assert.Multiple())
        {
            await Assert.That(lines).IsEquivalentTo(new[] { "first-second", "line" });
            await Assert.That(await provider.ReadAllBytesAsync(path)).IsEquivalentTo(new byte[] { 1, 2, 3 });
            await Assert.That(provider.GetFileLength(path)).IsEqualTo(3);
            await Assert.That(() => provider.CopyFile(path, copy, overwrite: false)).Throws<IOException>();
            await Assert.That(provider.GetRelativePath(FakeFileSystemProvider.Root, path)).IsEqualTo("defaults.txt");
        }
    }

    /// <summary>
    /// Implements only the members of <see cref="IFileSystemProvider"/> without defaults.
    /// </summary>
    private sealed class FakeFileSystemProvider : IFileSystemProvider
    {
        public static readonly string Root = Path.Combine(Path.GetTempPath(), "fake-file-system-provider");

        private readonly Dictionary<string, byte[]> _files = new(StringComparer.Ordinal);
        private readonly HashSet<string> _directories = new(StringComparer.Ordinal) { Root };
        private readonly Dictionary<string, Metadata> _metadata = new(StringComparer.Ordinal);

        public List<string> ClearedReadOnly { get; } = [];

        public void AddFile(string path, byte[] contents)
        {
            _files[path] = contents;
            _metadata[path] = new Metadata { Attributes = FileAttributes.Normal };
        }

        public Stream Open(string path, FileMode mode, FileAccess access)
        {
            var exists = _files.TryGetValue(path, out var existing);
            if (mode == FileMode.Open && !exists)
            {
                throw new FileNotFoundException(path);
            }

            var initial = mode is FileMode.Create or FileMode.CreateNew or FileMode.Truncate ? [] : existing ?? [];
            if (!exists)
            {
                AddFile(path, []);
            }

            var stream = new CommitStream(bytes => _files[path] = bytes);
            stream.Write(initial);
            stream.Position = mode == FileMode.Append ? stream.Length : 0;
            return stream;
        }

        public void DeleteFile(string path)
        {
            _files.Remove(path);
            _metadata.Remove(path);
        }

        public void MoveFile(string sourcePath, string destinationPath)
        {
            _files[destinationPath] = _files[sourcePath];
            _metadata[destinationPath] = _metadata[sourcePath];
            DeleteFile(sourcePath);
        }

        public bool FileExists(string path) => _files.ContainsKey(path);

        public void CreateDirectory(string path)
        {
            for (var current = path; current is not null && current.Length > Root.Length; current = Path.GetDirectoryName(current))
            {
                if (_directories.Add(current))
                {
                    _metadata[current] = new Metadata { Attributes = FileAttributes.Directory };
                }
            }
        }

        public void DeleteDirectory(string path, bool recursive)
        {
            foreach (var file in _files.Keys.Where(key => IsUnder(key, path)).ToArray())
            {
                DeleteFile(file);
            }

            foreach (var directory in _directories.Where(key => key == path || IsUnder(key, path)).ToArray())
            {
                _directories.Remove(directory);
                _metadata.Remove(directory);
            }
        }

        public void MoveDirectory(string sourcePath, string destinationPath) => throw new NotSupportedException();

        public bool DirectoryExists(string path) => _directories.Contains(path);

        public IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOption) =>
            _files.Keys.Where(key => IsIn(key, path, searchOption)).ToArray();

        public IEnumerable<string> EnumerateDirectories(string path, string searchPattern, SearchOption searchOption) =>
            _directories.Where(key => IsIn(key, path, searchOption)).ToArray();

        public FileAttributes GetAttributes(string path) => _metadata[path].Attributes;

        public void SetAttributes(string path, FileAttributes attributes)
        {
            if ((_metadata[path].Attributes & FileAttributes.ReadOnly) != 0
                && (attributes & FileAttributes.ReadOnly) == 0)
            {
                ClearedReadOnly.Add(path);
            }

            _metadata[path].Attributes = attributes;
        }

        public DateTime GetCreationTimeUtc(string path) => _metadata[path].CreationTimeUtc;

        public void SetCreationTimeUtc(string path, DateTime creationTimeUtc) =>
            _metadata[path].CreationTimeUtc = creationTimeUtc;

        public DateTime GetLastWriteTimeUtc(string path) => _metadata[path].LastWriteTimeUtc;

        public void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc) =>
            _metadata[path].LastWriteTimeUtc = lastWriteTimeUtc;

        public DateTime GetLastAccessTimeUtc(string path) => _metadata[path].LastAccessTimeUtc;

        public void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc) =>
            _metadata[path].LastAccessTimeUtc = lastAccessTimeUtc;

        private static bool IsUnder(string candidate, string parent) =>
            candidate.StartsWith(parent + Path.DirectorySeparatorChar, StringComparison.Ordinal);

        private static bool IsIn(string candidate, string parent, SearchOption searchOption) =>
            IsUnder(candidate, parent)
            && (searchOption == SearchOption.AllDirectories
                || string.Equals(Path.GetDirectoryName(candidate), parent, StringComparison.Ordinal));

        private sealed class Metadata
        {
            public FileAttributes Attributes { get; set; }

            public DateTime CreationTimeUtc { get; set; }

            public DateTime LastWriteTimeUtc { get; set; }

            public DateTime LastAccessTimeUtc { get; set; }
        }

        private sealed class CommitStream(Action<byte[]> commit) : MemoryStream
        {
            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    commit(ToArray());
                }

                base.Dispose(disposing);
            }
        }
    }
}
