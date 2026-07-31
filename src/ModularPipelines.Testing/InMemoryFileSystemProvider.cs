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
    private readonly HashSet<string> _exclusiveOpenFiles;
    private readonly Dictionary<string, int> _openReaders;
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
#pragma warning disable IDE0028 // Collection expressions cannot retain the platform path comparer.
        _exclusiveOpenFiles = new HashSet<string>(_pathComparer);
        _openReaders = new Dictionary<string, int>(_pathComparer);
#pragma warning restore IDE0028
        CreateDirectory(Environment.CurrentDirectory);
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
    public Stream OpenRead(string path)
    {
        lock (_sync)
        {
            var normalized = Normalize(path);
            var initial = GetFile(normalized);
            if (_exclusiveOpenFiles.Contains(normalized))
            {
                throw new IOException(
                    $"The in-memory file '{normalized}' is already open.");
            }

            _openReaders.TryGetValue(normalized, out var readerCount);
            _openReaders[normalized] = readerCount + 1;
            try
            {
                return CreateStream(
                    normalized,
                    FileMode.Open,
                    FileAccess.Read,
                    initial,
                    () => ReleaseOpenReader(normalized));
            }
            catch
            {
                ReleaseOpenReader(normalized);
                throw;
            }
        }
    }

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
            if (IsOpen(normalized))
            {
                throw new IOException(
                    $"The in-memory file '{normalized}' is already open.");
            }

            var initial = GetInitialContents(mode, existing);
            if (ShouldInitializeFile(mode, exists))
            {
                SetFile(normalized, initial);
            }

            _exclusiveOpenFiles.Add(normalized);
            try
            {
                return CreateStream(
                    normalized,
                    mode,
                    access,
                    initial,
                    () => ReleaseExclusiveOpenFile(normalized));
            }
            catch
            {
                _exclusiveOpenFiles.Remove(normalized);
                throw;
            }
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
        byte[] initial,
        Action release)
    {
        var stream = new CommittingMemoryStream(
            initial,
            access,
            mode == FileMode.Append,
            bytes => SetFile(normalizedPath, bytes, allowOpenFile: true),
            release);

        if (mode == FileMode.Append)
        {
            stream.Position = stream.Length;
        }

        return stream;
    }

    private void ReleaseExclusiveOpenFile(string normalizedPath)
    {
        lock (_sync)
        {
            _exclusiveOpenFiles.Remove(normalizedPath);
        }
    }

    private void ReleaseOpenReader(string normalizedPath)
    {
        lock (_sync)
        {
            var readerCount = _openReaders[normalizedPath];
            if (readerCount == 1)
            {
                _openReaders.Remove(normalizedPath);
            }
            else
            {
                _openReaders[normalizedPath] = readerCount - 1;
            }
        }
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
            var source = Normalize(sourcePath);
            var destination = Normalize(destinationPath);
            if (_pathComparer.Equals(source, destination))
            {
                throw new IOException(
                    $"The source and destination paths are the same: '{source}'.");
            }

            if (!overwrite && _files.ContainsKey(destination))
            {
                throw new IOException($"The in-memory file '{destination}' already exists.");
            }

            SetFile(destination, GetFile(source));
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

            ValidateFileDestination(destination);

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
            var directoriesToCreate = new List<string>();
            while (!string.IsNullOrEmpty(current))
            {
                if (_files.ContainsKey(current))
                {
                    throw new IOException(
                        $"The in-memory path '{current}' is already a file.");
                }

                directoriesToCreate.Add(current);
                var parent = Path.GetDirectoryName(current);
                if (string.IsNullOrEmpty(parent) || _pathComparer.Equals(parent, current))
                {
                    break;
                }

                current = parent;
            }

            foreach (var directory in directoriesToCreate)
            {
                _directories.TryAdd(directory, 0);
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

            if (_files.ContainsKey(destination))
            {
                throw new IOException($"The in-memory path '{destination}' is already a file.");
            }

            if (IsDescendant(destination, source))
            {
                throw new IOException("A directory cannot be moved inside itself.");
            }

            var destinationParent = Path.GetDirectoryName(destination);
            if (string.IsNullOrEmpty(destinationParent)
                || !_directories.ContainsKey(destinationParent))
            {
                throw new DirectoryNotFoundException(destinationParent);
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

    private static string JoinLines(IEnumerable<string> contents)
    {
        string[] lines = [.. contents];
        return lines.Length == 0
            ? string.Empty
            : string.Join(Environment.NewLine, lines) + Environment.NewLine;
    }

    private byte[] GetFile(string path)
    {
        var normalized = Normalize(path);
        return _files.TryGetValue(normalized, out var contents)
            ? contents
            : throw new FileNotFoundException("The in-memory file does not exist.", normalized);
    }

    private void SetFile(string path, byte[] contents, bool allowOpenFile = false)
    {
        lock (_sync)
        {
            var normalized = Normalize(path);
            if (!allowOpenFile && IsOpen(normalized))
            {
                throw new IOException(
                    $"The in-memory file '{normalized}' is already open.");
            }

            ValidateFileDestination(normalized);
            _files[normalized] = [.. contents];
        }
    }

    private void ValidateFileDestination(string normalizedPath)
    {
        if (_directories.ContainsKey(normalizedPath))
        {
            throw new IOException(
                $"The in-memory path '{normalizedPath}' is already a directory.");
        }

        var parent = Path.GetDirectoryName(normalizedPath);
        if (string.IsNullOrEmpty(parent) || !_directories.ContainsKey(parent))
        {
            throw new DirectoryNotFoundException(parent);
        }
    }

    private bool IsOpen(string normalizedPath) =>
        _exclusiveOpenFiles.Contains(normalizedPath)
        || _openReaders.ContainsKey(normalizedPath);

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
        private readonly bool _readable;
        private readonly bool _writable;
        private readonly long _appendStart;
        private readonly bool _initializing = true;
        private bool _committed;
        private readonly Action _release;
        private bool _released;

        public CommittingMemoryStream(
            byte[] contents,
            FileAccess access,
            bool append,
            Action<byte[]> commit,
            Action release)
            : base(Math.Max(contents.Length, 256))
        {
            _readable = access != FileAccess.Write;
            _writable = access != FileAccess.Read;
            _appendStart = append ? contents.Length : 0;
            _commit = commit;
            _release = release;
            base.Write(contents, 0, contents.Length);
            Position = 0;
            _initializing = false;
        }

        public override bool CanRead => _readable && base.CanRead;

        public override bool CanWrite => (_initializing || _writable) && base.CanWrite;

        public override long Position
        {
            get => base.Position;
            set
            {
                EnsureValidPosition(value);
                base.Position = value;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            EnsureReadable();
            return base.Read(buffer, offset, count);
        }

        public override int Read(Span<byte> buffer)
        {
            EnsureReadable();
            return base.Read(buffer);
        }

        public override ValueTask<int> ReadAsync(
            Memory<byte> buffer,
            CancellationToken cancellationToken = default)
        {
            EnsureReadable();
            return base.ReadAsync(buffer, cancellationToken);
        }

        public override Task<int> ReadAsync(
            byte[] buffer,
            int offset,
            int count,
            CancellationToken cancellationToken)
        {
            EnsureReadable();
            return base.ReadAsync(buffer, offset, count, cancellationToken);
        }

        public override int ReadByte()
        {
            EnsureReadable();
            return base.ReadByte();
        }

        public override long Seek(long offset, SeekOrigin loc)
        {
            var position = loc switch
            {
                SeekOrigin.Begin => offset,
                SeekOrigin.Current => Position + offset,
                SeekOrigin.End => Length + offset,
                _ => throw new ArgumentOutOfRangeException(nameof(loc)),
            };
            EnsureValidPosition(position);
            return base.Seek(offset, loc);
        }

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

        public override Task WriteAsync(
            byte[] buffer,
            int offset,
            int count,
            CancellationToken cancellationToken)
        {
            EnsureWritable();
            return base.WriteAsync(buffer, offset, count, cancellationToken);
        }

        public override void WriteByte(byte value)
        {
            EnsureWritable();
            base.WriteByte(value);
        }

        public override void SetLength(long value)
        {
            EnsureWritable();
            EnsureValidPosition(value);
            base.SetLength(value);
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && _writable && !_committed)
                {
                    _committed = true;
                    _commit(ToArray());
                }
            }
            finally
            {
                if (disposing && !_released)
                {
                    _released = true;
                    _release();
                }

                base.Dispose(disposing);
            }
        }

        private void EnsureWritable()
        {
            if (!_writable)
            {
                throw new NotSupportedException("The in-memory stream does not support writing.");
            }

            EnsureValidPosition(Position);
        }

        private void EnsureReadable()
        {
            if (!_readable)
            {
                throw new NotSupportedException("The in-memory stream does not support reading.");
            }
        }

        private void EnsureValidPosition(long position)
        {
            if (!_initializing && position < _appendStart)
            {
                throw new IOException("Cannot seek before the append boundary.");
            }
        }
    }
}
