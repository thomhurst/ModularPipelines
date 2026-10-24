using ModularPipelines.Context;
using ModularPipelines.FileSystem;
using ModularPipelines.Modules;

namespace ModularPipelines.Testing.UnitTests;

public class FilePathProviderBoundaryTests
{
    [Test]
    [MatrixDataSource]
    public async Task DifferentProvidersRejectTransferWithoutMutations(
        [Matrix(false, true)] bool move,
        [Matrix(false, true)] bool useAsync,
        [Matrix("missing", "empty", "file")] string sourceDestinationState,
        [Matrix(false, true)] bool destinationExists)
    {
        var rootPath = Path.Combine(Path.GetTempPath(), $"provider-boundary-{Guid.NewGuid():N}");
        var sourceRoot = await CreateRootAsync(rootPath);
        var otherRoot = await CreateRootAsync(rootPath);
        var source = sourceRoot.GetFile("source.txt");
        var otherSource = otherRoot.GetFile("source.txt");
        await source.WriteAsync("source");
        await otherSource.WriteAsync("other source");
        var sourceDestination = sourceRoot.GetFolder("destination");
        var destination = otherRoot.GetFolder("destination");
        var sourceDestinationExists = sourceDestinationState != "missing";
        if (sourceDestinationExists)
        {
            sourceDestination.Create();
            if (sourceDestinationState == "file")
            {
                await sourceDestination.GetFile(source.Name).WriteAsync("source destination");
            }
        }

        if (destinationExists)
        {
            destination.Create();
            await destination.GetFile(source.Name).WriteAsync("other destination");
        }

        var exception = (await Assert.ThrowsAsync<ArgumentException>(() => TransferAsync(source, destination, move, useAsync)))!;

        await Assert.That(exception.ParamName).IsEqualTo("folder");
        await Assert.That(exception.Message).Contains("same file-system provider");
        await Assert.That(await source.ReadAsync()).IsEqualTo("source");
        await Assert.That(await otherSource.ReadAsync()).IsEqualTo("other source");
        await AssertDestinationAsync(sourceDestination, sourceDestinationExists, sourceDestinationState == "file", "source destination");
        await AssertDestinationAsync(destination, destinationExists, destinationExists, "other destination");
    }

    [Test]
    [MatrixDataSource]
    public async Task SameProviderTransfersPreserveProvider(
        [Matrix(false, true)] bool move,
        [Matrix(false, true)] bool useAsync,
        [Matrix(false, true)] bool destinationExists)
    {
        var root = await CreateRootAsync(Path.Combine(Path.GetTempPath(), $"provider-transfer-{Guid.NewGuid():N}"));
        var source = root.GetFile("source.txt");
        await source.WriteAsync("source");
        var destination = root.GetFolder("destination");
        if (destinationExists)
        {
            destination.Create();
        }

        var transferred = await TransferAsync(source, destination, move, useAsync);

        await Assert.That(transferred).IsEqualTo(destination.GetFile(source.Name));
        await Assert.That(await transferred.ReadAsync()).IsEqualTo("source");
        await Assert.That(source.Exists).IsEqualTo(!move);
    }

    [Test]
    [MatrixDataSource]
    public async Task PreCanceledTransferPreservesProviders(
        [Matrix(false, true)] bool move,
        [Matrix(false, true)] bool sameProvider)
    {
        var rootPath = Path.Combine(Path.GetTempPath(), $"provider-canceled-{Guid.NewGuid():N}");
        var sourceRoot = await CreateRootAsync(rootPath);
        var destinationRoot = sameProvider ? sourceRoot : await CreateRootAsync(rootPath);
        var source = sourceRoot.GetFile("source.txt");
        await source.WriteAsync("source");
        var destination = destinationRoot.GetFolder("destination");
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();

        await Assert.ThrowsAsync<OperationCanceledException>(() => move
            ? source.MoveToAsync(destination, cancellation.Token)
            : source.CopyToAsync(destination, cancellation.Token));

        await Assert.That(await source.ReadAsync()).IsEqualTo("source");
        await Assert.That(sourceRoot.GetFolder("destination").Exists).IsFalse();
        await Assert.That(destination.Exists).IsFalse();
    }

    private static Task<FilePath> TransferAsync(FilePath source, FolderPath destination, bool move, bool useAsync)
    {
        if (useAsync)
        {
            return move ? source.MoveToAsync(destination) : source.CopyToAsync(destination);
        }

        return Task.FromResult(move ? source.MoveTo(destination) : source.CopyTo(destination));
    }

    private static async Task AssertDestinationAsync(FolderPath destination, bool exists, bool fileExists, string contents)
    {
        await Assert.That(destination.Exists).IsEqualTo(exists);
        var file = destination.GetFile("source.txt");
        await Assert.That(file.Exists).IsEqualTo(fileExists);
        if (fileExists)
        {
            await Assert.That(await file.ReadAsync()).IsEqualTo(contents);
        }
    }

    private static async Task<FolderPath> CreateRootAsync(string rootPath)
    {
        var run = await ModuleTester.For<PathsModule, FolderPath>().WithService(rootPath).ExecuteAsync();
        await Assert.That(run.Exception).IsNull();
        return run.Value!.Create();
    }

    public sealed class PathsModule(string rootPath) : Module<FolderPath>
    {
        protected override Task<FolderPath> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult(context.Files.GetFolder(rootPath));
        }
    }
}
