using System.IO.Abstractions.TestingHelpers;
using System.IO.Compression;
using ModularPipelines.Context;
using ModularPipelines.Context.Domains.Implementations;
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.Artifacts;
using ModularPipelines.FileSystem;
using Moq;

namespace ModularPipelines.UnitTests.Artifacts;

public class TypedArtifactContextTests
{
    [Test]
    [Arguments("PublishFileAsync", typeof(FilePath), typeof(ArtifactReference), false)]
    [Arguments("PublishDirectoryAsync", typeof(FolderPath), typeof(ArtifactReference), false)]
    [Arguments("DownloadAsync", typeof(FilePath), typeof(FilePath), false)]
    [Arguments("DownloadAsync", typeof(FolderPath), typeof(FolderPath), false)]
    [Arguments("DownloadAsync", typeof(FilePath), typeof(FilePath), true)]
    [Arguments("DownloadAsync", typeof(FolderPath), typeof(FolderPath), true)]
    public async Task Artifact_Context_Exposes_Typed_Paths(string name, Type pathType, Type resultType, bool generic)
    {
        var method = typeof(IArtifactContext).GetMethods().SingleOrDefault(method =>
            method.Name == name && method.IsGenericMethod == generic &&
            method.GetParameters()[^2].ParameterType == pathType);

        await Assert.That(method).IsNotNull();
        await Assert.That(method!.ReturnType).IsEqualTo(typeof(Task<>).MakeGenericType(resultType));
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Typed_Files_Preserve_Providers_And_Producer_Identity(bool generic)
    {
        var (provider, fileSystem) = CreateProvider();
        var source = new FilePath(Path.Combine("typed-artifacts", "input.txt"), provider);
        var destination = new FilePath(Path.Combine("typed-artifacts", "output.txt"), provider);
        fileSystem.AddFile(source.Path, new MockFileData("payload"));
        var store = new InMemoryDistributedArtifactStore();
        var context = new ArtifactContextImpl(store, new ArtifactOptions()).ForModule(typeof(ArtifactProducer));

        var reference = await context.PublishFileAsync("package", source);
        var result = generic
            ? await context.DownloadAsync<ArtifactProducer>("package", destination)
            : await context.DownloadAsync(reference.ModuleId, "package", destination);

        await Assert.That(reference.ModuleId).IsEqualTo(ModuleId.FromType(typeof(ArtifactProducer)));
        await Assert.That(reference.Name).IsEqualTo("package");
        await Assert.That(result).IsSameReferenceAs(destination);
        await Assert.That(fileSystem.File.ReadAllText(result.Path)).IsEqualTo("payload");
        await Assert.That(File.Exists(destination.Path)).IsFalse();
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Typed_Directories_Preserve_Providers_And_Nested_Content(bool generic)
    {
        var (provider, fileSystem) = CreateProvider();
        var source = new FolderPath(Path.Combine("typed-artifacts", "input"), provider);
        var destination = new FolderPath(Path.Combine("typed-artifacts", "output"), provider);
        fileSystem.AddFile(Path.Combine(source.Path, "nested", "file.txt"), new MockFileData("payload"));
        fileSystem.AddDirectory(Path.Combine(source.Path, "empty"));
        var store = new InMemoryDistributedArtifactStore();
        var context = new ArtifactContextImpl(store, new ArtifactOptions()).ForModule(typeof(ArtifactProducer));

        var reference = await context.PublishDirectoryAsync("directory", source);
        var result = generic
            ? await context.DownloadAsync<ArtifactProducer>("directory", destination)
            : await context.DownloadAsync(reference.ModuleId, "directory", destination);

        await Assert.That(reference.ContentType).IsEqualTo("application/zip");
        await Assert.That(result).IsSameReferenceAs(destination);
        await Assert.That(fileSystem.File.ReadAllText(Path.Combine(result.Path, "nested", "file.txt"))).IsEqualTo("payload");
        await Assert.That(fileSystem.Directory.Exists(Path.Combine(result.Path, "empty"))).IsTrue();
        await Assert.That(Directory.Exists(destination.Path)).IsFalse();
    }

    [Test]
    public async Task Typed_And_String_Paths_Round_Trip_Through_The_Same_Store()
    {
        var directory = Directory.CreateTempSubdirectory("typed-artifacts-");
        try
        {
            var source = new FilePath(Path.Combine(directory.FullName, "input.txt"));
            await File.WriteAllTextAsync(source.Path, "payload");
            var store = new InMemoryDistributedArtifactStore();
            var context = new ArtifactContextImpl(store, new ArtifactOptions()).ForModule(typeof(ArtifactProducer));
            await context.PublishFileAsync("typed", source);
            var stringDestination = Path.Combine(directory.FullName, "string.txt");
            var stringResult = await context.DownloadAsync<ArtifactProducer>("typed", stringDestination);
            await context.PublishFileAsync("string", stringDestination);
            var typedDestination = new FilePath(Path.Combine(directory.FullName, "typed.txt"));
            var typedResult = await context.DownloadAsync<ArtifactProducer>("string", typedDestination);

            await Assert.That(stringResult).IsEqualTo(stringDestination);
            await Assert.That(await typedResult.ReadAsync()).IsEqualTo("payload");

            var typedFolder = new FolderPath(Path.Combine(directory.FullName, "archive"));
            await context.PublishDirectoryAsync("folder", directory.FullName);
            await context.DownloadAsync<ArtifactProducer>("folder", typedFolder);
            await Assert.That(await typedFolder.GetFile("typed.txt").ReadAsync()).IsEqualTo("payload");
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    public async Task Typed_Destinations_Reject_The_Wrong_Artifact_Kind_Before_Writing()
    {
        var (provider, fileSystem) = CreateProvider();
        var source = new FilePath(Path.Combine("typed-artifacts", "input.txt"), provider);
        fileSystem.AddFile(source.Path, new MockFileData("payload"));
        var store = new InMemoryDistributedArtifactStore();
        var context = new ArtifactContextImpl(store, new ArtifactOptions()).ForModule(typeof(ArtifactProducer));
        await context.PublishFileAsync("file", source);
        await context.PublishDirectoryAsync("folder", source.Folder!);
        var destinationFile = new FilePath(Path.Combine("typed-artifacts", "new.txt"), provider);
        var destinationFolder = new FolderPath(Path.Combine("typed-artifacts", "new"), provider);

        await Assert.ThrowsAsync<InvalidOperationException>(() => context.DownloadAsync<ArtifactProducer>("file", destinationFolder));
        await Assert.ThrowsAsync<InvalidOperationException>(() => context.DownloadAsync<ArtifactProducer>("folder", destinationFile));
        await Assert.That(destinationFile.Exists).IsFalse();
        await Assert.That(destinationFolder.Exists).IsFalse();
    }

    [Test]
    public async Task Typed_Operations_Honor_PreCanceled_Tokens_Before_Provider_Access()
    {
        var provider = new Mock<IFileSystemProvider>(MockBehavior.Strict);
        var file = new FilePath("typed-file", provider.Object);
        var folder = new FolderPath("typed-folder", provider.Object);
        var store = new InMemoryDistributedArtifactStore();
        var context = new ArtifactContextImpl(store, new ArtifactOptions()).ForModule(typeof(ArtifactProducer));
        var token = new CancellationToken(canceled: true);

        await Assert.ThrowsAsync<OperationCanceledException>(() => context.PublishFileAsync("file", file, token));
        await Assert.ThrowsAsync<OperationCanceledException>(() => context.PublishDirectoryAsync("folder", folder, token));
        await Assert.ThrowsAsync<OperationCanceledException>(() => context.DownloadAsync<ArtifactProducer>("file", file, token));
        await Assert.ThrowsAsync<OperationCanceledException>(() => context.DownloadAsync<ArtifactProducer>("folder", folder, token));
        provider.VerifyNoOtherCalls();
    }

    [Test]
    public async Task Context_Paths_Keep_The_Pipeline_Working_Directory()
    {
        var (provider, fileSystem) = CreateProvider();
        var workingDirectory = Path.Combine(Path.GetTempPath(), $"artifact-working-directory-{Guid.NewGuid():N}");
        var files = new FilesContext(provider, new PipelineWorkingDirectory(workingDirectory), Mock.Of<IZipContext>());
        fileSystem.AddFile(Path.Combine(workingDirectory, "input.txt"), new MockFileData("pipeline content"));
        var context = new ArtifactContextImpl(new InMemoryDistributedArtifactStore(), new ArtifactOptions())
            .ForModule(typeof(ArtifactProducer));

        await context.PublishFileAsync("file", files.GetFile("input.txt"));
        var result = await context.DownloadAsync<ArtifactProducer>("file", files.GetFile("downloads/output.txt"));

        await Assert.That(result.Path).IsEqualTo(Path.Combine(workingDirectory, "downloads", "output.txt"));
        await Assert.That(fileSystem.File.ReadAllText(result.Path)).IsEqualTo("pipeline content");
    }

    [Test]
    public async Task Typed_Directory_Extraction_Rejects_Traversal_On_Custom_Providers()
    {
        var (provider, fileSystem) = CreateProvider();
        var destination = new FolderPath(Path.Combine("typed-artifacts", "destination"), provider);
        await using var data = new MemoryStream();
        using (var archive = new ZipArchive(data, ZipArchiveMode.Create, leaveOpen: true))
        {
            archive.CreateEntry("../outside.txt");
        }

        data.Position = 0;
        var store = new InMemoryDistributedArtifactStore();
        await store.UploadAsync(new ArtifactDescriptor
        {
            Name = "traversal",
            ModuleId = ModuleId.FromType(typeof(ArtifactProducer)),
            ContentType = "application/zip",
        }, data, CancellationToken.None);
        IArtifactContext context = new ArtifactContextImpl(store, new ArtifactOptions());

        await Assert.ThrowsAsync<IOException>(() => context.DownloadAsync<ArtifactProducer>("traversal", destination));
        await Assert.That(fileSystem.FileExists(Path.GetFullPath(Path.Combine(destination.Path, "..", "outside.txt")))).IsFalse();
    }

    private static (IFileSystemProvider Provider, MockFileSystem FileSystem) CreateProvider()
    {
        var fileSystem = new MockFileSystem();
        var provider = new Mock<IFileSystemProvider>();
        provider.Setup(p => p.OpenRead(It.IsAny<string>())).Returns((string path) => fileSystem.File.OpenRead(path));
        provider.Setup(p => p.Create(It.IsAny<string>())).Returns((string path) => fileSystem.File.Create(path));
        provider.Setup(p => p.Open(It.IsAny<string>(), It.IsAny<FileMode>(), It.IsAny<FileAccess>()))
            .Returns((string path, FileMode mode, FileAccess access) => fileSystem.File.Open(path, mode, access));
        provider.Setup(p => p.CreateDirectory(It.IsAny<string>())).Callback((string path) => fileSystem.Directory.CreateDirectory(path));
        provider.Setup(p => p.EnumerateDirectories(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<SearchOption>()))
            .Returns((string path, string pattern, SearchOption search) => fileSystem.Directory.EnumerateDirectories(path, pattern, search));
        provider.Setup(p => p.EnumerateFiles(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<SearchOption>()))
            .Returns((string path, string pattern, SearchOption search) => fileSystem.Directory.EnumerateFiles(path, pattern, search));
        provider.Setup(p => p.GetAttributes(It.IsAny<string>())).Returns((string path) => fileSystem.File.GetAttributes(path));
        provider.Setup(p => p.GetLastWriteTimeUtc(It.IsAny<string>())).Returns((string path) => fileSystem.File.GetLastWriteTimeUtc(path));
        provider.Setup(p => p.SetLastWriteTimeUtc(It.IsAny<string>(), It.IsAny<DateTime>()))
            .Callback((string path, DateTime value) => fileSystem.File.SetLastWriteTimeUtc(path, value));
        provider.Setup(p => p.MoveFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
            .Callback((string source, string destination, bool overwrite) => fileSystem.File.Move(source, destination, overwrite));
        provider.Setup(p => p.DeleteFile(It.IsAny<string>())).Callback((string path) => fileSystem.File.Delete(path));
        provider.Setup(p => p.FileExists(It.IsAny<string>())).Returns((string path) => fileSystem.File.Exists(path));
        provider.Setup(p => p.DirectoryExists(It.IsAny<string>())).Returns((string path) => fileSystem.Directory.Exists(path));
        return (provider.Object, fileSystem);
    }

    private sealed class ArtifactProducer : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult(string.Empty);
    }
}
