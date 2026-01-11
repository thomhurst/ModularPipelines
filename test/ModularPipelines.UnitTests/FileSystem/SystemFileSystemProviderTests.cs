using ModularPipelines.FileSystem;

namespace ModularPipelines.UnitTests.FileSystem;

public class SystemFileSystemProviderTests : IDisposable
{
    private readonly string _tempDir;
    private readonly SystemFileSystemProvider _provider;

    public SystemFileSystemProviderTests()
    {
        _tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(_tempDir);
        _provider = SystemFileSystemProvider.Instance;
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempDir))
        {
            Directory.Delete(_tempDir, recursive: true);
        }
    }

    [Test]
    public async Task ReadAllTextAsync_ReadsFileContents()
    {
        var filePath = Path.Combine(_tempDir, "test.txt");
        await System.IO.File.WriteAllTextAsync(filePath, "Hello World");

        var result = await _provider.ReadAllTextAsync(filePath);

        await Assert.That(result).IsEqualTo("Hello World");
    }

    [Test]
    public async Task WriteAllTextAsync_WritesFileContents()
    {
        var filePath = Path.Combine(_tempDir, "write-test.txt");

        await _provider.WriteAllTextAsync(filePath, "Test Content");

        var result = await System.IO.File.ReadAllTextAsync(filePath);
        await Assert.That(result).IsEqualTo("Test Content");
    }

    [Test]
    public async Task FileExists_ReturnsTrueForExistingFile()
    {
        var filePath = Path.Combine(_tempDir, "exists.txt");
        await System.IO.File.WriteAllTextAsync(filePath, "content");

        var result = _provider.FileExists(filePath);

        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task FileExists_ReturnsFalseForNonExistingFile()
    {
        var filePath = Path.Combine(_tempDir, "does-not-exist.txt");

        var result = _provider.FileExists(filePath);

        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task CreateDirectory_CreatesNewDirectory()
    {
        var dirPath = Path.Combine(_tempDir, "new-dir");

        _provider.CreateDirectory(dirPath);

        await Assert.That(Directory.Exists(dirPath)).IsTrue();
    }

    [Test]
    public async Task DeleteDirectory_RemovesDirectoryRecursively()
    {
        var dirPath = Path.Combine(_tempDir, "to-delete");
        Directory.CreateDirectory(dirPath);
        await System.IO.File.WriteAllTextAsync(Path.Combine(dirPath, "file.txt"), "content");

        _provider.DeleteDirectory(dirPath, recursive: true);

        await Assert.That(Directory.Exists(dirPath)).IsFalse();
    }

    [Test]
    public async Task Combine_JoinsPaths()
    {
        var result = _provider.Combine("folder", "subfolder", "file.txt");

        await Assert.That(result).IsEqualTo(Path.Combine("folder", "subfolder", "file.txt"));
    }
}
