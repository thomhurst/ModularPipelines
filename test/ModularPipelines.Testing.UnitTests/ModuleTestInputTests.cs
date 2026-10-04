using Initialization.Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ModularPipelines.Caching;
using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.FileSystem;
using ModularPipelines.Modules;

namespace ModularPipelines.Testing.UnitTests;

public class ModuleTestInputTests
{
    [Test]
    public async Task SeedsFilesBeforeConfiguredInitializers()
    {
        var provider = new InMemoryFileSystemProvider();
        var initializer = new FileInitializer(provider);
        await ModuleTester.For<DependencyObserver, string>()
            .WithFile("initializer.txt", "ready")
            .WithService<IFileSystemProvider>(provider)
            .WithDependencyFailure<Dependency, string>(new Exception("seed"))
            .ConfigurePipeline(builder =>
            {
                initializer.Path = Path.Combine(builder.WorkingDirectory, "initializer.txt");
                builder.Services.AddSingleton<IInitializer>(initializer);
            })
            .ExecuteAsync();

        await Assert.That(initializer.Contents).IsEqualTo("ready");
    }

    [Test]
    public async Task RequiredFailurePreventsTargetExecution()
    {
        var failure = new InvalidOperationException("seeded failure");
        var run = await ModuleTester.For<RequiredDependencyObserver, string>()
            .WithDependencyFailure<Dependency, string>(failure)
            .ExecuteAsync();

        await Assert.That(run.Result.Status).IsEqualTo(ModuleStatus.DependencyFailed);
        await Assert.That(run.Exception).IsTypeOf<DependencyFailedException>();
        await Assert.That(run.Exception!.InnerException).IsSameReferenceAs(failure);
    }

    [Test]
    public async Task RequiredSkipPreventsTargetExecution()
    {
        var run = await ModuleTester.For<RequiredDependencyObserver, string>()
            .WithSkippedDependency<Dependency, string>("seeded skip")
            .ExecuteAsync();

        await Assert.That(run.Result.Status).IsEqualTo(ModuleStatus.Skipped);
        await Assert.That(run.Result.SkipDecisionOrDefault!.Reason).Contains("seeded skip");
    }

    [Test]
    public async Task AlwaysRunObservesRequiredFailure()
    {
        var run = await ModuleTester.For<AlwaysRunDependencyObserver, string>()
            .WithDependencyFailure<Dependency, string>(new Exception("expected failure"))
            .ExecuteAsync();

        await Assert.That(run.Value).IsEqualTo("expected failure");
    }

    [Test]
    public async Task OptionalSkipDoesNotSkipTarget()
    {
        var run = await ModuleTester.For<OptionalDependencyObserver, string>()
            .WithSkippedDependency<Dependency, string>("optional skip")
            .ExecuteAsync();

        await Assert.That(run.Value).IsEqualTo("optional skip");
    }

    [Test]
    public async Task RegisteredOptionalFailureStillBlocksTarget()
    {
        var run = await ModuleTester.For<OptionalDependencyObserver, string>()
            .WithDependencyFailure<Dependency, string>(new Exception("failed"))
            .ExecuteAsync();

        await Assert.That(run.Result.Status).IsEqualTo(ModuleStatus.DependencyFailed);
    }

    [Test]
    public async Task AlwaysRunDoesNotOverrideRequiredSkip()
    {
        var run = await ModuleTester.For<AlwaysRunDependencyObserver, string>()
            .WithSkippedDependency<Dependency, string>("required skip")
            .ExecuteAsync();

        await Assert.That(run.Result.Status).IsEqualTo(ModuleStatus.Skipped);
    }

    [Test]
    public async Task SeedsEffectiveProviderUsingPipelinePathResolution()
    {
        var provider = new InMemoryFileSystemProvider();
        var directory = Path.Combine(Path.GetTempPath(), "module-test-inputs", Guid.NewGuid().ToString("N"));
        string? workingDirectory = null;
        var run = await ModuleTester.For<InputModule, string>()
            .WithFile("inputs/text.txt", "original")
            .WithFile("inputs/text.txt", "replacement")
            .WithFile("inputs/data.bin", new byte[] { 7 })
            .WithFile(Path.Combine(directory, "absolute.txt"), "absolute")
            .WithService<IFileSystemProvider>(provider)
            .ConfigurePipeline(pipeline =>
            {
                workingDirectory = pipeline.WorkingDirectory;
                pipeline.Services.AddSingleton<IOptions<ModuleCacheOptions>>(
                    Microsoft.Extensions.Options.Options.Create(new ModuleCacheOptions { WorkingDirectory = directory }));
                pipeline.Configuration["test-value"] = "first";
            })
            .ConfigurePipeline(pipeline => pipeline.Configuration["test-value"] = "last")
            .ExecuteAsync();

        await Assert.That(run.Value).IsEqualTo("replacement:7:last");
        await Assert.That(run.FileSystem).IsSameReferenceAs(provider);
        await Assert.That(provider.FileExists(Path.Combine(workingDirectory!, "inputs/text.txt"))).IsTrue();
        await Assert.That(provider.FileExists(Path.Combine(directory, "absolute.txt"))).IsTrue();
        await Assert.That(Directory.Exists(directory)).IsFalse();
    }

    [Test]
    public async Task SeedsFilesBeforeModuleExecutionAndKeepsRunsIsolated()
    {
        var bytes = new byte[] { 1, 2, 3 };
        var builder = ModuleTester.For<InputModule, string>()
            .WithFile("inputs/text.txt", "seeded")
            .WithFile("inputs/data.bin", bytes)
            .ConfigurePipeline(pipeline => pipeline.Configuration["test-value"] = "configured");
        bytes[0] = 9;

        var first = await builder.ExecuteAsync();
        var second = await builder.ExecuteAsync();

        await Assert.That(first.Value).IsEqualTo("seeded:1:configured");
        await Assert.That(second.Value).IsEqualTo(first.Value);
        await Assert.That(first.FileSystem).IsNotSameReferenceAs(second.FileSystem);
    }

    [Test]
    public async Task SeedsFailedDependencyWithoutExecutingIt()
    {
        var failure = new InvalidOperationException("seeded failure");
        var run = await ModuleTester.For<DependencyObserver, string>()
            .WithDependencyFailure<Dependency, string>(failure)
            .ExecuteAsync();

        await Assert.That(run.Value).IsEqualTo("seeded failure");
    }

    [Test]
    public async Task SeedsSkippedDependencyWithoutExecutingIt()
    {
        var run = await ModuleTester.For<DependencyObserver, string>()
            .WithSkippedDependency<Dependency, string>("seeded skip")
            .ExecuteAsync();

        await Assert.That(run.Value).IsEqualTo("seeded skip");
    }

    [Test]
    public async Task FailureHelperPreservesExitCodeAndOutput()
    {
        var result = CommandResult.Fail(17, "stdout", "stderr");
        await Assert.That(result.ExitCode).IsEqualTo(17);
        await Assert.That(result.StandardOutput).IsEqualTo("stdout");
        await Assert.That(result.StandardError).IsEqualTo("stderr");
        await Assert.That(() => CommandResult.Fail(0)).Throws<ArgumentOutOfRangeException>();

        var run = await ModuleTester.For<ModuleTesterTests.CommandModule, string>()
            .InterceptCommands(_ => result)
            .ExecuteAsync();
        await Assert.That(run.Exception).IsTypeOf<CommandException>();
    }

    public sealed class InputModule(IConfiguration configuration) : Module<string>
    {
        protected override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            var text = await context.Files.GetFile("inputs/text.txt").ReadAsync(cancellationToken);
            var data = await context.Files.GetFile("inputs/data.bin").ReadBytesAsync(cancellationToken);
            return $"{text}:{data[0]}:{configuration["test-value"]}";
        }
    }

    public sealed class Dependency : Module<string>
    {
        protected override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => throw new InvalidOperationException("Dependency must not execute");
    }

    public sealed class DependencyObserver : Module<string>
    {
        protected override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            var result = await context.GetModule<Dependency>();
            return result.ExceptionOrDefault?.Message ?? result.SkipDecisionOrDefault!.Reason!;
        }
    }

    [DependsOn<Dependency>]
    public sealed class RequiredDependencyObserver : Module<string>
    {
        protected override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => throw new InvalidOperationException("Target must not execute");
    }

    [DependsOn<Dependency>(Optional = true)]
    public sealed class OptionalDependencyObserver : Module<string>
    {
        protected override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => (await context.GetModule<Dependency>()).SkipDecisionOrDefault!.Reason!;
    }

    [DependsOn<Dependency>]
    public sealed class AlwaysRunDependencyObserver : Module<string>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module.WithAlwaysRun();

        protected override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => (await context.GetModule<Dependency>()).ExceptionOrDefault!.Message;
    }

    private sealed class FileInitializer(IFileSystemProvider provider) : IInitializer
    {
        public int Order => int.MinValue;

        public string Path { get; set; } = "";

        public string? Contents { get; private set; }

        public async Task InitializeAsync() => Contents = await provider.ReadAllTextAsync(Path);
    }
}
