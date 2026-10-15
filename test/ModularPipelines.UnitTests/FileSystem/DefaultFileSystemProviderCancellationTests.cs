using System.Text;
using ModularPipelines.FileSystem;

namespace ModularPipelines.UnitTests.FileSystem;

public class DefaultFileSystemProviderCancellationTests
{
    [Test]
    [MatrixDataSource]
    public async Task Canceled_Write_Does_Not_Open_Destination(
        [Matrix("text", "bytes", "lines", "empty-text", "empty-bytes", "empty-lines", "append-text", "append-lines", "append-empty-text", "append-empty-lines")] string operation,
        [Matrix(false, true)] bool exists,
        [Matrix(false, true)] bool useFilePath)
    {
        await using var temporary = new TempFolder();
        var path = Path.Combine(temporary.Folder.Path, "destination.txt");
        if (exists)
        {
            await File.WriteAllTextAsync(path, "preserved");
        }

        var provider = new PrimitiveFileSystemProvider();
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();

        await Assert.ThrowsAsync<OperationCanceledException>(() =>
            WriteAsync(provider, path, operation, useFilePath, cancellation.Token));

        await Assert.That(provider.OpenCount).IsEqualTo(0);
        await Assert.That(File.Exists(path)).IsEqualTo(exists);
        if (exists)
        {
            await Assert.That(await File.ReadAllTextAsync(path)).IsEqualTo("preserved");
        }
    }

    [Test]
    [MatrixDataSource]
    public async Task Uncanceled_Write_Writes_Expected_Contents(
        [Matrix("text", "bytes", "lines", "empty-text", "empty-bytes", "empty-lines", "append-text", "append-lines", "append-empty-text", "append-empty-lines")] string operation,
        [Matrix(false, true)] bool exists,
        [Matrix(false, true)] bool useFilePath)
    {
        await using var temporary = new TempFolder();
        var path = Path.Combine(temporary.Folder.Path, "destination.txt");
        if (exists)
        {
            await File.WriteAllTextAsync(path, "preserved");
        }

        var provider = new PrimitiveFileSystemProvider();
        using var cancellation = new CancellationTokenSource();

        await WriteAsync(provider, path, operation, useFilePath, cancellation.Token);

        var expected = operation.Contains("empty", StringComparison.Ordinal) ? string.Empty : "replacement";
        if (operation is "lines" or "append-lines")
        {
            expected += Environment.NewLine;
        }

        if (exists && operation.StartsWith("append", StringComparison.Ordinal))
        {
            expected = "preserved" + expected;
        }

        await Assert.That(provider.OpenCount).IsEqualTo(1);
        await Assert.That(await File.ReadAllBytesAsync(path)).IsEquivalentTo(Encoding.UTF8.GetBytes(expected));
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Line_Write_Observes_Cancellation_During_Enumeration(bool append)
    {
        await using var temporary = new TempFolder();
        var path = Path.Combine(temporary.Folder.Path, "destination.txt");
        IFileSystemProvider provider = new PrimitiveFileSystemProvider();
        using var cancellation = new CancellationTokenSource();

        await Assert.ThrowsAsync<OperationCanceledException>(() => append
            ? provider.AppendAllLinesAsync(path, Lines(), cancellation.Token)
            : provider.WriteAllLinesAsync(path, Lines(), cancellation.Token));

        IEnumerable<string> Lines()
        {
            yield return "first";
            cancellation.Cancel();
            yield return "second";
        }
    }

    private static Task WriteAsync(
        IFileSystemProvider provider,
        string path,
        string operation,
        bool useFilePath,
        CancellationToken cancellationToken)
    {
        var file = new FilePath(path, provider);
        var text = operation.Contains("empty", StringComparison.Ordinal) ? string.Empty : "replacement";
        var bytes = Encoding.UTF8.GetBytes(text);
        string[] lines = text.Length == 0 ? [] : [text];

        return (operation, useFilePath) switch
        {
            ("text" or "empty-text", false) => provider.WriteAllTextAsync(path, text, cancellationToken),
            ("text" or "empty-text", true) => file.WriteAsync(text, cancellationToken),
            ("bytes" or "empty-bytes", false) => provider.WriteAllBytesAsync(path, bytes, cancellationToken),
            ("bytes" or "empty-bytes", true) => file.WriteAsync(bytes, cancellationToken),
            ("lines" or "empty-lines", false) => provider.WriteAllLinesAsync(path, lines, cancellationToken),
            ("lines" or "empty-lines", true) => file.WriteAsync(lines, cancellationToken),
            ("append-text" or "append-empty-text", false) => provider.AppendAllTextAsync(path, text, cancellationToken),
            ("append-text" or "append-empty-text", true) => file.AppendAsync(text, cancellationToken),
            ("append-lines" or "append-empty-lines", false) => provider.AppendAllLinesAsync(path, lines, cancellationToken),
            ("append-lines" or "append-empty-lines", true) => file.AppendAsync(lines, cancellationToken),
            _ => throw new ArgumentOutOfRangeException(nameof(operation)),
        };
    }

    // Content methods deliberately inherit the default interface implementations.
    private sealed class PrimitiveFileSystemProvider : IFileSystemProvider
    {
        public int OpenCount { get; private set; }

        public Stream Open(string path, FileMode mode, FileAccess access)
        {
            OpenCount++;
            return new FileStream(path, mode, access);
        }

        public void DeleteFile(string path) => File.Delete(path);

        public void MoveFile(string sourcePath, string destinationPath) => File.Move(sourcePath, destinationPath);

        public bool FileExists(string path) => File.Exists(path);

        public void CreateDirectory(string path) => Directory.CreateDirectory(path);

        public void DeleteDirectory(string path, bool recursive) => Directory.Delete(path, recursive);

        public void MoveDirectory(string sourcePath, string destinationPath) => Directory.Move(sourcePath, destinationPath);

        public bool DirectoryExists(string path) => Directory.Exists(path);

        public IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOption) =>
            Directory.EnumerateFiles(path, searchPattern, searchOption);

        public IEnumerable<string> EnumerateDirectories(string path, string searchPattern, SearchOption searchOption) =>
            Directory.EnumerateDirectories(path, searchPattern, searchOption);

        public FileAttributes GetAttributes(string path) => File.GetAttributes(path);

        public void SetAttributes(string path, FileAttributes attributes) => File.SetAttributes(path, attributes);

        public DateTime GetCreationTimeUtc(string path) => File.GetCreationTimeUtc(path);

        public void SetCreationTimeUtc(string path, DateTime creationTimeUtc) => File.SetCreationTimeUtc(path, creationTimeUtc);

        public DateTime GetLastWriteTimeUtc(string path) => File.GetLastWriteTimeUtc(path);

        public void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc) => File.SetLastWriteTimeUtc(path, lastWriteTimeUtc);

        public DateTime GetLastAccessTimeUtc(string path) => File.GetLastAccessTimeUtc(path);

        public void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc) => File.SetLastAccessTimeUtc(path, lastAccessTimeUtc);
    }
}
