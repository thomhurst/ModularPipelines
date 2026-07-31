using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Text;
using ModularPipelines.FileSystem;

namespace ModularPipelines.Testing;

/// <summary>
/// Thread-safe in-memory implementation of <see cref="IFileSystemProvider"/>.
/// </summary>
public sealed class InMemoryFileSystemProvider : IFileSystemProvider
{
    private readonly ConcurrentDictionary<string, byte[]> _files;
    private readonly ConcurrentDictionary<string, byte> _directories;
    private readonly Lock _sync = new();
    private readonly StringComparer _pathComparer;
    private readonly StringComparison _pathComparison;

    /// <summary>
    /// Initializes an empty in-memory filesystem.
    /// </summary>
    public InMemoryFileSystemProvider()
    {
        _pathComparer = OperatingSystem.IsWindows()
            ? StringComparer.OrdinalIgnoreCase
            : StringComparer.Ordinal;
        _pathComparison = OperatingSystem.IsWindows()
            ? StringComparison.OrdinalIgnoreCase
            : StringComparison.Ordinal;
        _files = new ConcurrentDictionary<string, byte[]>(_pathComparer);
        _directories = new ConcurrentDictionary<string, byte>(_pathComparer);
        CreateDirectory(Path.GetPathRoot(Environment.CurrentDirectory) ?? Environment.CurrentDirectory);
        CreateDirectory(GetTempPath());
    }

    /// <inheritdoc />
    public Task<string> ReadAllTextAsync(
        string path,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(Encoding.UTF8.GetString(GetFile(path)));
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<string> ReadLinesAsync(
        string path,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var text = await ReadAllTextAsync(path, cancellationToken).ConfigureAwait(false);
        using var reader = new StringReader(text);
        while (await reader.ReadLineAsync(cancellationToken).ConfigureAwait(false) is { } line)
        {
            yield return line;
        }
    }

    /// <inheritdoc />
    public Task<byte[]> ReadAllBytesAsync(
        string path,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(GetFile(path).ToArray());
    }

    /// <inheritdoc />
    public Task WriteAllTextAsync(
        string path,
        string contents,
        CancellationToken cancellationToken = default) =>
        WriteAllBytesAsync(path, Encoding.UTF8.GetBytes(contents), cancellationToken);

    /// <inheritdoc />
    public Task WriteAllBytesAsync(
        string path,
        byte[] contents,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        SetFile(path, contents);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task WriteAllLinesAsync(
        string path,
        IEnumerable<string> contents,
        CancellationToken cancellationToken = default) =>
        WriteAllTextAsync(path, JoinLines(contents), cancellationToken);

    /// <inheritdoc />
    public Task AppendAllTextAsync(
        string path,
        string contents,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        lock (_sync)
        {
            var existing = FileExists(path) ? Encoding.UTF8.GetString(GetFile(path)) : string.Empty;
            SetFile(path, Encoding.UTF8.GetBytes(existing + contents));
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task AppendAllLinesAsync(
        string path,
        IEnumerable<string> contents,
        CancellationToken cancellationToken = default) =>
        AppendAllTextAsync(path, JoinLines(contents), cancellationToken);

    /// <inheritdoc />
    public Stream OpenRead(string path) => Open(path, FileMode.Open, FileAccess.Read);

    /// <inheritdoc />
    public Stream Create(string path) => Open(path, FileMode.Create, FileAccess.ReadWrite);

    /// <inheritdoc />
    public Stream Open(string path, FileMode mode, FileAccess access)
    {
        lock (_sync)
        {
            var normalized = Normalize(path);
            var exists = _files.TryGetValue(normalized, out var existing);

            ValidateOpenArguments(mode, access, exists, normalized);
            var initial = GetInitialContents(mode, existing);
            if (ShouldInitializeFile(mode, exists))
            {
                SetFile(normalized, initial);
            }

            return CreateStream(normalized, mode, access, initial);
        }
    }

    private static void ValidateOpenArguments(
        FileMode mode,
        FileAccess access,
        bool exists,
        string normalizedPath)
    {
        if (mode == FileMode.Append && access != FileAccess.Write)
        {
            throw new ArgumentException("Append mode requires write-only access.", nameof(access));
        }

        if (mode is FileMode.Create or FileMode.CreateNew or FileMode.Truncate
            && access == FileAccess.Read)
        {
            throw new ArgumentException($"{mode} mode requires write access.", nameof(access));
        }

        if (mode is FileMode.Open or FileMode.Truncate && !exists)
        {
            throw new FileNotFoundException("The in-memory file does not exist.", normalizedPath);
        }

        if (mode == FileMode.CreateNew && exists)
        {
            throw new IOException($"The in-memory file '{normalizedPath}' already exists.");
        }
    }

    private static byte[] GetInitialContents(FileMode mode, byte[]? existing) =>
        mode is FileMode.Create or FileMode.CreateNew or FileMode.Truncate
            ? []
            : existing ?? [];

    private static bool ShouldInitializeFile(FileMode mode, bool exists) =>
        mode is FileMode.Create or FileMode.CreateNew or FileMode.Truncate
        || (!exists && mode is (FileMode.OpenOrCreate or FileMode.Append));

    private CommittingMemoryStream CreateStream(
        string normalizedPath,
        FileMode mode,
        FileAccess access,
        byte[] initial)
    {
        var stream = new CommittingMemoryStream(
            initial,
            access != FileAccess.Read,
            bytes => SetFile(normalizedPath, bytes));

        if (mode == FileMode.Append)
        {
            stream.Position = stream.Length;
        }

        return stream;
    }

    /// <inheritdoc />
    public void DeleteFile(string path)
    {
        lock (_sync)
        {
            _files.TryRemove(Normalize(path), out _);
        }
    }

    /// <inheritdoc />
    public void CopyFile(string sourcePath, string destinationPath, bool overwrite)
    {
        lock (_sync)
        {
            var destination = Normalize(destinationPath);
            if (!overwrite && _files.ContainsKey(destination))
            {
                throw new IOException($"The in-memory file '{destination}' already exists.");
            }

            SetFile(destination, GetFile(sourcePath));
        }
    }

    /// <inheritdoc />
    public void MoveFile(string sourcePath, string destinationPath)
    {
        lock (_sync)
        {
            var source = Normalize(sourcePath);
            var destination = Normalize(destinationPath);
            if (_files.ContainsKey(destination))
            {
                throw new IOException($"The in-memory file '{destination}' already exists.");
            }

            if (!_files.TryRemove(source, out var contents))
            {
                throw new FileNotFoundException("The in-memory file does not exist.", source);
            }

            SetFile(destination, contents);
        }
    }

    /// <inheritdoc />
    public bool FileExists(string path) => _files.ContainsKey(Normalize(path));

    /// <inheritdoc />
    public void CreateDirectory(string path)
    {
        lock (_sync)
        {
            var current = Normalize(path);
            while (!string.IsNullOrEmpty(current))
            {
                _directories.TryAdd(current, 0);
                var parent = Path.GetDirectoryName(current);
                if (string.IsNullOrEmpty(parent) || _pathComparer.Equals(parent, current))
                {
                    break;
                }

                current = parent;
            }
        }
    }

    /// <inheritdoc />
    public void DeleteDirectory(string path, bool recursive)
    {
        lock (_sync)
        {
            var normalized = Normalize(path);
            if (!_directories.ContainsKey(normalized))
            {
                throw new DirectoryNotFoundException(normalized);
            }

            var descendants = GetDescendantDirectories(normalized).ToArray();
            var files = GetDescendantFiles(normalized).ToArray();
            if (!recursive && (descendants.Length > 0 || files.Length > 0))
            {
                throw new IOException($"The in-memory directory '{normalized}' is not empty.");
            }

            foreach (var file in files)
            {
                _files.TryRemove(file, out _);
            }

            foreach (var directory in descendants.Append(normalized))
            {
                _directories.TryRemove(directory, out _);
            }
        }
    }

    /// <inheritdoc />
    public void MoveDirectory(string sourcePath, string destinationPath)
    {
        lock (_sync)
        {
            var source = Normalize(sourcePath);
            var destination = Normalize(destinationPath);
            if (!_directories.ContainsKey(source))
            {
                throw new DirectoryNotFoundException(source);
            }

            if (_directories.ContainsKey(destination))
            {
                throw new IOException($"The in-memory directory '{destination}' already exists.");
            }

            if (IsDescendant(destination, source))
            {
                throw new IOException("A directory cannot be moved inside itself.");
            }

            CreateDirectory(destination);
            foreach (var directory in GetDescendantDirectories(source).ToArray())
            {
                CreateDirectory(ReplacePrefix(directory, source, destination));
            }

            foreach (var file in GetDescendantFiles(source).ToArray())
            {
                MoveFile(file, ReplacePrefix(file, source, destination));
            }

            foreach (var directory in GetDescendantDirectories(source).Append(source).ToArray())
            {
                _directories.TryRemove(directory, out _);
            }
        }
    }

    /// <inheritdoc />
    public bool DirectoryExists(string path) => _directories.ContainsKey(Normalize(path));

    /// <inheritdoc />
    public IEnumerable<string> EnumerateFiles(
        string path,
        string searchPattern,
        SearchOption searchOption) =>
        EnumerateEntries(_files.Keys, path, searchPattern, searchOption);

    /// <inheritdoc />
    public IEnumerable<string> EnumerateDirectories(
        string path,
        string searchPattern,
        SearchOption searchOption) =>
        EnumerateEntries(_directories.Keys, path, searchPattern, searchOption);

    /// <inheritdoc />
    public string GetTempPath() =>
        Path.Combine(Path.GetTempPath(), "ModularPipelines.Testing");

    /// <inheritdoc />
    public string GetRandomFileName() => Path.GetRandomFileName();

    /// <inheritdoc />
    public string Combine(params string[] paths) => Path.Combine(paths);

    /// <inheritdoc />
    public string GetRelativePath(string relativeTo, string path) =>
        Path.GetRelativePath(relativeTo, path);

    private static string JoinLines(IEnumerable<string> contents) =>
        string.Join(Environment.NewLine, contents) + Environment.NewLine;

    private byte[] GetFile(string path)
    {
        var normalized = Normalize(path);
        return _files.TryGetValue(normalized, out var contents)
            ? contents
            : throw new FileNotFoundException("The in-memory file does not exist.", normalized);
    }

    private void SetFile(string path, byte[] contents)
    {
        lock (_sync)
        {
            var normalized = Normalize(path);
            CreateDirectory(Path.GetDirectoryName(normalized) ?? normalized);
            _files[normalized] = [.. contents];
        }
    }

    private string Normalize(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        var fullPath = Path.GetFullPath(path);
        var root = Path.GetPathRoot(fullPath);
        return root is not null && _pathComparer.Equals(root, fullPath)
            ? fullPath
            : fullPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
    }

    private IEnumerable<string> EnumerateEntries(
        IEnumerable<string> entries,
        string path,
        string searchPattern,
        SearchOption searchOption)
    {
        var root = Normalize(path);
        if (!_directories.ContainsKey(root))
        {
            throw new DirectoryNotFoundException(root);
        }

        return [.. entries
            .Where(entry => IsDescendant(entry, root))
            .Where(entry => searchOption == SearchOption.AllDirectories
                || !Path.GetRelativePath(root, entry)
                    .Contains(Path.DirectorySeparatorChar, StringComparison.Ordinal))
            .Where(entry => System.IO.Enumeration.FileSystemName.MatchesSimpleExpression(
                searchPattern,
                Path.GetFileName(entry),
                _pathComparison == StringComparison.OrdinalIgnoreCase))
            .OrderBy(static entry => entry, _pathComparer)];
    }

    private IEnumerable<string> GetDescendantFiles(string path) =>
        _files.Keys.Where(entry => IsDescendant(entry, path));

    private IEnumerable<string> GetDescendantDirectories(string path) =>
        _directories.Keys.Where(entry => IsDescendant(entry, path));

    private bool IsDescendant(string candidate, string parent)
    {
        if (!candidate.StartsWith(parent, _pathComparison) || candidate.Length <= parent.Length)
        {
            return false;
        }

        return parent[^1] is '/' or '\\'
            || candidate[parent.Length] is '/' or '\\';
    }

    private static string ReplacePrefix(string path, string source, string destination) =>
        destination + path[source.Length..];

    private sealed class CommittingMemoryStream : MemoryStream
    {
        private readonly Action<byte[]> _commit;
        private readonly bool _writable;
        private readonly bool _initializing = true;
        private bool _committed;

        public CommittingMemoryStream(
            byte[] contents,
            bool writable,
            Action<byte[]> commit)
            : base(Math.Max(contents.Length, 256))
        {
            _writable = writable;
            _commit = commit;
            base.Write(contents, 0, contents.Length);
            Position = 0;
            _initializing = false;
        }

        public override bool CanWrite => (_initializing || _writable) && base.CanWrite;

        public override void Write(byte[] buffer, int offset, int count)
        {
            EnsureWritable();
            base.Write(buffer, offset, count);
        }

        public override void Write(ReadOnlySpan<byte> buffer)
        {
            EnsureWritable();
            base.Write(buffer);
        }

        public override ValueTask WriteAsync(
            ReadOnlyMemory<byte> buffer,
            CancellationToken cancellationToken = default)
        {
            EnsureWritable();
            return base.WriteAsync(buffer, cancellationToken);
        }

        public override void WriteByte(byte value)
        {
            EnsureWritable();
            base.WriteByte(value);
        }

        public override void SetLength(long value)
        {
            EnsureWritable();
            base.SetLength(value);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _writable && !_committed)
            {
                _committed = true;
                _commit(ToArray());
            }

            base.Dispose(disposing);
        }

        private void EnsureWritable()
        {
            if (!_writable)
            {
                throw new NotSupportedException("The in-memory stream does not support writing.");
            }
        }
    }
}
