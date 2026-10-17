using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Events;
using ModularPipelines.FileSystem;
using ModularPipelines.Logging;
using ModularPipelines.Modules;
using ModularPipelines.Reporting;
using ModularPipelines.TestHelpers;
using Moq;

namespace ModularPipelines.UnitTests.FileSystem;

public class AmbientFileSystemTests
{
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task PipelineConstructionAndExecutionUseRegisteredProvider(bool useFactory)
    {
        var builder = TestPipelineBuilder.Create();
        builder.Services.AddSingleton(Mock.Of<IFileSystemProvider>());
        if (useFactory)
        {
            builder.AddModule<ProviderModule>(_ => new ProviderModule());
        }
        else
        {
            builder.AddModule<ProviderModule>();
        }

        var summary = await builder.RunAsync();
        var result = await summary.Modules.OfType<ProviderModule>().Single();

        await Assert.That(result.ValueOrDefault).IsTrue();
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task RegisteredInstanceConfigurationUsesRegisteredProvider(bool useCustomPlanningCopy)
    {
        var provider = Mock.Of<IFileSystemProvider>();
        var builder = TestPipelineBuilder.Create();
        builder.Services.AddSingleton(provider);
        if (useCustomPlanningCopy)
        {
            builder.AddModule(new CustomPlanningConfigurationProviderModule());
        }
        else
        {
            builder.AddModule(new ConfigurationProviderModule());
        }

        await using var pipeline = await builder.BuildAsync();
        if (useCustomPlanningCopy)
        {
            await pipeline.Services.GetRequiredService<IDependencyGraphExporter>()
                .RenderAsync(DependencyGraphFormat.Json);
            await Assert.That(AmbientModuleOutputContext.Current).IsNull();
        }

        var summary = await pipeline.RunAsync();
        var module = summary.Modules.OfType<ConfigurationProviderModule>().Single();
        var result = await module;

        await Assert.That(result.ValueOrDefault).IsTrue();
        await Assert.That(module.ConfiguredFile.Provider).IsSameReferenceAs(provider);
        await Assert.That(module.ConfiguredFolder.Provider).IsSameReferenceAs(provider);
        await Assert.That(AmbientModuleOutputContext.Current).IsNull();
    }

    [Test]
    public async Task ReadyHandlersUseRegisteredProvider()
    {
        var provider = Mock.Of<IFileSystemProvider>();
        var builder = TestPipelineBuilder.Create();
        builder.Services.AddSingleton(provider);
        builder.AddModule<ReadyProviderModule>();
        builder.AddModuleEventHandler<ReadyProviderHandler>();

        var summary = await builder.RunAsync();
        var module = summary.Modules.OfType<ReadyProviderModule>().Single();

        await Assert.That(module.ReadyFiles).Count().IsEqualTo(2);
        await Assert.That(module.ReadyFolders).Count().IsEqualTo(2);
        foreach (var file in module.ReadyFiles)
        {
            await Assert.That(file.Provider).IsSameReferenceAs(provider);
        }

        foreach (var folder in module.ReadyFolders)
        {
            await Assert.That(folder.Provider).IsSameReferenceAs(provider);
        }
    }

    [Test]
    public async Task NestedScopesRestoreProviderAndPathsRetainCapturedProvider()
    {
        var outerProvider = Mock.Of<IFileSystemProvider>();
        var innerProvider = Mock.Of<IFileSystemProvider>();
        FilePath retained;
        using (new ModuleOutputContextScope(typeof(AmbientFileSystemTests), fileSystemProvider: outerProvider))
        {
            using (new ModuleOutputContextScope(typeof(AmbientFileSystemTests), fileSystemProvider: innerProvider))
            {
                await Task.Yield();
                retained = new FilePath("file.txt");
                await Assert.That(retained.Provider).IsSameReferenceAs(innerProvider);
                await Assert.That(new FolderPath("folder").Provider).IsSameReferenceAs(innerProvider);
            }

            await Assert.That(new FilePath("file.txt").Provider).IsSameReferenceAs(outerProvider);
            await Assert.That(new FolderPath("folder").Provider).IsSameReferenceAs(outerProvider);
        }

        await Assert.That(retained.Provider).IsSameReferenceAs(innerProvider);
        await Assert.That(new FilePath("file.txt").Provider).IsSameReferenceAs(SystemFileSystemProvider.Instance);
    }

    [Test]
    public async Task FlowedScopeStopsSupplyingProviderWhenDisposed()
    {
        var release = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        Task<FilePath> latePath;
        using (new ModuleOutputContextScope(typeof(AmbientFileSystemTests), fileSystemProvider: Mock.Of<IFileSystemProvider>()))
        {
            latePath = Task.Run(async () =>
            {
                await release.Task;
                return new FilePath("file.txt");
            });
        }

        release.SetResult();
        await Assert.That((await latePath).Provider).IsSameReferenceAs(SystemFileSystemProvider.Instance);
    }

    [Test]
    public async Task PathsOutsideModuleScopeUseSystemProvider()
    {
        FilePath file = "file.txt"!;
        FolderPath folder = "folder"!;

        using (Assert.Multiple())
        {
            await Assert.That(file.Provider).IsSameReferenceAs(SystemFileSystemProvider.Instance);
            await Assert.That(folder.Provider).IsSameReferenceAs(SystemFileSystemProvider.Instance);
        }
    }

    [Test]
    public async Task EqualityIncludesProviderIdentity()
    {
        var firstProvider = Mock.Of<IFileSystemProvider>();
        var secondProvider = Mock.Of<IFileSystemProvider>();
        var file = new FilePath("file.txt", firstProvider);
        var sameFile = new FilePath("file.txt", firstProvider);
        var otherFile = new FilePath("file.txt", secondProvider);
        var folder = new FolderPath("folder", firstProvider);
        var sameFolder = new FolderPath("folder", firstProvider);
        var otherFolder = new FolderPath("folder", secondProvider);

        using (Assert.Multiple())
        {
            await Assert.That(file == sameFile).IsTrue();
            await Assert.That(file == otherFile).IsFalse();
            await Assert.That(file.GetHashCode()).IsEqualTo(sameFile.GetHashCode());
            await Assert.That(new HashSet<FilePath> { file, sameFile, otherFile }).Count().IsEqualTo(2);
            await Assert.That(folder == sameFolder).IsTrue();
            await Assert.That(folder == otherFolder).IsFalse();
            await Assert.That(folder.GetHashCode()).IsEqualTo(sameFolder.GetHashCode());
            await Assert.That(new HashSet<FolderPath> { folder, sameFolder, otherFolder }).Count().IsEqualTo(2);
        }
    }

    [ReadyProvider]
    private sealed class ReadyProviderModule : Module<bool>
    {
        public List<FilePath> ReadyFiles { get; } = [];

        public List<FolderPath> ReadyFolders { get; } = [];

        protected internal override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult(true);
    }

    [AttributeUsage(AttributeTargets.Class)]
    private sealed class ReadyProviderAttribute : Attribute, IModuleReadyHandler
    {
        public async Task OnModuleReadyAsync(IModuleHookContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            var module = (ReadyProviderModule) context.Module;
            module.ReadyFiles.Add(new FilePath("attribute.txt"));
            module.ReadyFolders.Add(new FolderPath("attribute"));
        }
    }

    private sealed class ReadyProviderHandler : IModuleEventHandler
    {
        public async Task OnModuleReadyAsync(IModuleHookContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            var module = (ReadyProviderModule) context.Module;
            module.ReadyFiles.Add(new FilePath("handler.txt"));
            module.ReadyFolders.Add(new FolderPath("handler"));
        }
    }

    private class ConfigurationProviderModule : Module<bool>
    {
        public FilePath ConfiguredFile { get; private set; } = null!;

        public FolderPath ConfiguredFolder { get; private set; } = null!;

        protected override void Configure(ModuleConfigurationBuilder module)
        {
            ConfiguredFile = new FilePath("configured.txt");
            ConfiguredFolder = new FolderPath("configured");
        }

        protected internal override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            var provider = context.Files.GetFile("file.txt").Provider;
            return Task.FromResult(ReferenceEquals(ConfiguredFile.Provider, provider)
                && ReferenceEquals(ConfiguredFolder.Provider, provider));
        }
    }

    private sealed class CustomPlanningConfigurationProviderModule : ConfigurationProviderModule
    {
        protected override Module<bool> CreatePlanningCopy(IServiceProvider serviceProvider) =>
            new CustomPlanningConfigurationProviderModule();
    }

    private sealed class ProviderModule : Module<bool>
    {
        private readonly FilePath _file = new("file.txt");
        private readonly FolderPath _folder = new("folder");
        private FilePath _configuredFile = null!;
        private FolderPath _configuredFolder = null!;

        protected override void Configure(ModuleConfigurationBuilder module)
        {
            _configuredFile = new FilePath("configured.txt");
            _configuredFolder = new FolderPath("configured");
        }

        protected internal override async Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            await Task.Yield();
            var provider = context.Files.GetFile("file.txt").Provider;
            FilePath convertedFile = "file.txt"!;
            FolderPath convertedFolder = "folder"!;
            return ReferenceEquals(_file.Provider, provider)
                && ReferenceEquals(_folder.Provider, provider)
                && ReferenceEquals(_configuredFile.Provider, provider)
                && ReferenceEquals(_configuredFolder.Provider, provider)
                && ReferenceEquals(convertedFile.Provider, provider)
                && ReferenceEquals(convertedFolder.Provider, provider);
        }
    }
}
