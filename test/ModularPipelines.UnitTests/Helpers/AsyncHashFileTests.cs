using System.Text;
using ModularPipelines.Context;
using ModularPipelines.FileSystem;
using Moq;

namespace ModularPipelines.UnitTests.Helpers;

public class AsyncHashFileTests
{
    private static readonly string WorkingDirectory = Path.Combine(Path.GetTempPath(), "virtual-hash-root");
    private static readonly byte[] Contents = Encoding.UTF8.GetBytes("Async file hashing\n");

    [Test]
    [Arguments("Md5")]
    [Arguments("Sha1")]
    [Arguments("Sha256")]
    [Arguments("Sha384")]
    [Arguments("Sha512")]
    public async Task Matches_Synchronous_Encoding_And_Uses_Provider_Async_Reads(string algorithm)
    {
        foreach (var encoding in new[] { HashEncoding.Hex, HashEncoding.Base64 })
        {
            var path = Path.Combine(WorkingDirectory, "input.txt");
            var stream = new AsyncIoTestStream(new MemoryStream(Contents));
            var provider = new Mock<IFileSystemProvider>(MockBehavior.Strict);
            provider.Setup(fileSystem => fileSystem.FileExists(path)).Returns(true);
            provider.SetupSequence(fileSystem => fileSystem.OpenRead(path))
                .Returns(new MemoryStream(Contents))
                .Returns(stream);
            var hash = CreateHash(provider.Object);
            var expected = HashSynchronously(hash, algorithm, "input.txt", encoding);

            var actual = await HashAsync(hash, algorithm, "input.txt", encoding);

            await Assert.That(actual).IsEqualTo(expected);
            await Assert.That(stream.AsyncReads).IsGreaterThan(0);
            await Assert.That(stream.DisposedAsynchronously).IsTrue();
            provider.Verify(fileSystem => fileSystem.OpenRead(path), Times.Exactly(2));
        }
    }

    [Test]
    public async Task Precancelled_Hash_Does_Not_Access_Provider()
    {
        var provider = new Mock<IFileSystemProvider>(MockBehavior.Strict);
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();

        await Assert.ThrowsAsync<OperationCanceledException>(() =>
            CreateHash(provider.Object).Sha256FileAsync("input.txt", cancellationToken: cancellation.Token));
        provider.VerifyNoOtherCalls();
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Read_Failure_Or_Cancellation_Disposes_Provider_Stream(bool cancel)
    {
        using var cancellation = new CancellationTokenSource();
        var stream = new AsyncIoTestStream(new MemoryStream(Contents))
        {
            BeforeRead = token =>
            {
                if (cancel)
                {
                    cancellation.Cancel();
                    token.ThrowIfCancellationRequested();
                }

                throw new IOException("Provider read failed.");
            },
        };
        var path = Path.Combine(WorkingDirectory, "input.txt");
        var provider = new Mock<IFileSystemProvider>(MockBehavior.Strict);
        provider.Setup(fileSystem => fileSystem.FileExists(path)).Returns(true);
        provider.Setup(fileSystem => fileSystem.OpenRead(path)).Returns(stream);
        var hash = CreateHash(provider.Object);

        if (cancel)
        {
            await Assert.ThrowsAsync<OperationCanceledException>(() =>
                hash.Sha256FileAsync("input.txt", cancellationToken: cancellation.Token));
        }
        else
        {
            var exception = await Assert.ThrowsAsync<IOException>(() => hash.Sha256FileAsync("input.txt"));
            await Assert.That(exception!.Message).IsEqualTo("Provider read failed.");
        }

        await Assert.That(stream.DisposedAsynchronously).IsTrue();
    }

    [Test]
    [Arguments("")]
    [Arguments(" ")]
    [Arguments("missing.txt")]
    public async Task Missing_Or_Blank_Path_Preserves_FileNotFound_Contract(string path)
    {
        var provider = new Mock<IFileSystemProvider>();
        await Assert.ThrowsAsync<FileNotFoundException>(() => CreateHash(provider.Object).Md5FileAsync(path));
        provider.Verify(fileSystem => fileSystem.OpenRead(It.IsAny<string>()), Times.Never);
    }

    private static IHashContext CreateHash(IFileSystemProvider provider) =>
        new HashContext(new Hex(), new Base64(), provider, new PipelineWorkingDirectory(WorkingDirectory));

    private static string HashSynchronously(IHashContext hash, string algorithm, string path, HashEncoding encoding) =>
        algorithm switch
        {
            "Md5" => hash.Md5File(path, encoding),
            "Sha1" => hash.Sha1File(path, encoding),
            "Sha256" => hash.Sha256File(path, encoding),
            "Sha384" => hash.Sha384File(path, encoding),
            "Sha512" => hash.Sha512File(path, encoding),
            _ => throw new ArgumentOutOfRangeException(nameof(algorithm)),
        };

    private static Task<string> HashAsync(IHashContext hash, string algorithm, string path, HashEncoding encoding) =>
        algorithm switch
        {
            "Md5" => hash.Md5FileAsync(path, encoding),
            "Sha1" => hash.Sha1FileAsync(path, encoding),
            "Sha256" => hash.Sha256FileAsync(path, encoding),
            "Sha384" => hash.Sha384FileAsync(path, encoding),
            "Sha512" => hash.Sha512FileAsync(path, encoding),
            _ => throw new ArgumentOutOfRangeException(nameof(algorithm)),
        };
}
