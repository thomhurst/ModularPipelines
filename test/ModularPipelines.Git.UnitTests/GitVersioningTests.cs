using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ModularPipelines.Context;
using ModularPipelines.FileSystem;
using ModularPipelines.Git;
using ModularPipelines.Git.Options;
using ModularPipelines.Logging;
using ModularPipelines.Options;
using Moq;

namespace ModularPipelines.Git.UnitTests;

public class GitVersioningTests
{
    [Test]
    public async Task Constructor_Does_Not_Touch_The_File_System()
    {
        var fileSystemProvider = new Mock<IFileSystemProvider>(MockBehavior.Strict);

        _ = new GitVersioning(
            Mock.Of<IGitInformation>(),
            Mock.Of<IServiceScopeFactory>(MockBehavior.Strict),
            fileSystemProvider.Object,
            Microsoft.Extensions.Options.Options.Create(new GitVersioningOptions()));

        await Assert.That(fileSystemProvider.Invocations.Count).IsEqualTo(0);
    }

    [Test]
    public async Task Missing_Repository_Does_Not_Create_Temporary_Folder()
    {
        var fileSystemProvider = new Mock<IFileSystemProvider>(MockBehavior.Strict);
        var gitInformation = new Mock<IGitInformation>();
        gitInformation.Setup(x => x.GetInfoAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync((GitRepositoryInfo?) null);

        var versioning = new GitVersioning(
            gitInformation.Object,
            Mock.Of<IServiceScopeFactory>(MockBehavior.Strict),
            fileSystemProvider.Object,
            Microsoft.Extensions.Options.Options.Create(new GitVersioningOptions()));

        await Assert.That(async () => { await versioning.GetVersioningInformationAsync(); })
            .Throws<InvalidOperationException>();
        await Assert.That(fileSystemProvider.Invocations.Count).IsEqualTo(0);
    }

    [Test]
    public async Task Missing_Configuration_Uses_Temporary_Defaults_Without_Changing_Repository()
    {
        await using var fixture = new VersioningFixture();
        string? configurationPath = null;
        fixture.ExecuteVersion = async (options, execution, cancellationToken) =>
        {
            var arguments = options.Arguments!.ToArray();
            configurationPath = arguments[Array.IndexOf(arguments, "/config") + 1];
            await Assert.That(File.Exists(Path.Combine(fixture.Repository.Path, "GitVersion.yml"))).IsFalse();
            await Assert.That(Path.GetDirectoryName(configurationPath)).IsEqualTo(fixture.ToolFolder);
            await Assert.That((await File.ReadAllTextAsync(configurationPath, cancellationToken)).ReplaceLineEndings("\n"))
                .IsEqualTo("mode: ContinuousDeployment\nstrategies:\n  - Mainline");
            await Assert.That(arguments.Contains("/nocache")).IsTrue();
            await Assert.That(execution!.WorkingDirectory).IsEqualTo(fixture.Repository.Path);
            return CommandResult.Ok("{\"SemVer\":\"1.2.3\"}");
        };

        var result = await fixture.CreateVersioning().GetVersioningInformationAsync();

        await Assert.That(result.SemVer).IsEqualTo("1.2.3");
        await Assert.That(Directory.GetFileSystemEntries(fixture.Repository.Path)).IsEmpty();
        await Assert.That(File.Exists(configurationPath)).IsFalse();
        await Assert.That(Directory.Exists(fixture.ToolFolder)).IsFalse();
    }

    [Test]
    [Arguments("GitVersion.yml")]
    [Arguments("GitVersion.yaml")]
    [Arguments(".GitVersion.yml")]
    [Arguments(".GitVersion.yaml")]
    [Arguments("gitversion.YML")]
    public async Task Existing_Configuration_Is_Left_To_GitVersion(string fileName)
    {
        await using var fixture = new VersioningFixture();
        var configurationPath = Path.Combine(fixture.Repository.Path, fileName);
        const string configuration = "next-version: 7.0.0";
        await File.WriteAllTextAsync(configurationPath, configuration);
        fixture.ExecuteVersion = async (options, _, _) =>
        {
            await Assert.That(options.Arguments!.Contains("/config")).IsFalse();
            await Assert.That(File.Exists(Path.Combine(fixture.ToolFolder!, "GitVersion.yml"))).IsFalse();
            return CommandResult.Ok("{\"SemVer\":\"7.0.0\"}");
        };

        await fixture.CreateVersioning().GetVersioningInformationAsync();

        await Assert.That(await File.ReadAllTextAsync(configurationPath)).IsEqualTo(configuration);
        await Assert.That(Directory.GetFiles(fixture.Repository.Path).Length).IsEqualTo(1);
        await Assert.That(Directory.Exists(fixture.ToolFolder)).IsFalse();
    }

    [Test]
    public async Task Separate_Repositories_Have_Independent_Cached_Results()
    {
        await using var first = new VersioningFixture();
        await using var second = new VersioningFixture();
        second.ExecuteVersion = (_, _, _) => Task.FromResult(CommandResult.Ok("{\"SemVer\":\"4.5.6\"}"));
        var firstVersioning = first.CreateVersioning();
        var secondVersioning = second.CreateVersioning();

        var firstResult = await firstVersioning.GetVersioningInformationAsync();
        var secondResult = await secondVersioning.GetVersioningInformationAsync();

        await Assert.That(firstResult.SemVer).IsEqualTo("1.2.3");
        await Assert.That(secondResult.SemVer).IsEqualTo("4.5.6");
        await Assert.That(ReferenceEquals(firstResult, await firstVersioning.GetVersioningInformationAsync())).IsTrue();
        await Assert.That(ReferenceEquals(secondResult, await secondVersioning.GetVersioningInformationAsync())).IsTrue();
        await Assert.That(first.VersionCalls).IsEqualTo(1);
        await Assert.That(second.VersionCalls).IsEqualTo(1);
    }

    [Test]
    public async Task Concurrent_Module_Scopes_Compute_Once_Per_Pipeline()
    {
        await using var fixture = new VersioningFixture();
        var started = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var release = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        fixture.ExecuteVersion = async (_, _, _) =>
        {
            started.TrySetResult();
            await release.Task;
            return CommandResult.Ok("{\"SemVer\":\"1.2.3\"}");
        };
        var versioning = fixture.CreateVersioning();
        var first = versioning.GetVersioningInformationAsync();
        try
        {
            await started.Task.WaitAsync(TimeSpan.FromSeconds(10));
            var others = Enumerable.Range(0, 10)
                .Select(_ => fixture.CreateVersioning().GetVersioningInformationAsync()).ToArray();
            release.SetResult();
            var results = await Task.WhenAll([first, .. others]);

            await Assert.That(results.All(result => ReferenceEquals(result, results[0]))).IsTrue();
            await Assert.That(fixture.InstallCalls).IsEqualTo(1);
            await Assert.That(fixture.VersionCalls).IsEqualTo(1);
            await Assert.That(fixture.CommandScopesCreated).IsEqualTo(1);
            await Assert.That(fixture.CommandScopesDisposed).IsEqualTo(1);
        }
        finally
        {
            release.TrySetResult();
            await first;
        }
    }

    [Test]
    public async Task Canceling_A_Waiter_Does_Not_Interrupt_The_Shared_Calculation()
    {
        await using var fixture = new VersioningFixture();
        var started = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var release = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        fixture.ExecuteVersion = async (_, _, token) =>
        {
            started.TrySetResult();
            await release.Task.WaitAsync(token);
            return CommandResult.Ok("{\"SemVer\":\"1.2.3\"}");
        };
        var first = fixture.CreateVersioning().GetVersioningInformationAsync();
        try
        {
            await started.Task.WaitAsync(TimeSpan.FromSeconds(10));
            using var cancellation = new CancellationTokenSource();
            var waiter = fixture.CreateVersioning().GetVersioningInformationAsync(cancellation.Token);
            cancellation.Cancel();
            await Assert.That(async () => { await waiter; }).Throws<OperationCanceledException>();
            await Assert.That(fixture.CommandScopesDisposed).IsEqualTo(0);
            release.TrySetResult();
            var result = await first;
            var cached = await fixture.CreateVersioning().GetVersioningInformationAsync();

            await Assert.That(ReferenceEquals(result, cached)).IsTrue();
            await Assert.That(fixture.InstallCalls).IsEqualTo(1);
            await Assert.That(fixture.VersionCalls).IsEqualTo(1);
            await Assert.That(fixture.CommandScopesDisposed).IsEqualTo(1);
        }
        finally
        {
            release.TrySetResult();
            await first;
        }
    }

    [Test]
    public async Task Sequential_Module_Scopes_Share_The_Cached_Result()
    {
        await using var fixture = new VersioningFixture();
        await using var provider = fixture.CreateProvider();
        GitVersionInformation first;
        await using (var scope = provider.CreateAsyncScope())
        {
            first = await scope.ServiceProvider.GetRequiredService<IGitVersioning>().GetVersioningInformationAsync();
        }

        await using var nextScope = provider.CreateAsyncScope();
        var second = await nextScope.ServiceProvider.GetRequiredService<IGitVersioning>().GetVersioningInformationAsync();

        await Assert.That(ReferenceEquals(first, second)).IsTrue();
        await Assert.That(fixture.InstallCalls).IsEqualTo(1);
        await Assert.That(fixture.VersionCalls).IsEqualTo(1);
        await Assert.That(fixture.CommandScopesCreated).IsEqualTo(1);
        await Assert.That(fixture.CommandScopesDisposed).IsEqualTo(1);
    }

    [Test]
    public async Task Independent_Providers_For_The_Same_Repository_Have_Independent_Results()
    {
        await using var fixture = new VersioningFixture();
        await using var first = fixture.CreateProvider();
        await using var second = fixture.CreateProvider();
        await using var firstScope = first.CreateAsyncScope();
        await using var secondScope = second.CreateAsyncScope();
        var firstResult = await firstScope.ServiceProvider.GetRequiredService<IGitVersioning>().GetVersioningInformationAsync();
        fixture.ExecuteVersion = (_, _, _) => Task.FromResult(CommandResult.Ok("{\"SemVer\":\"4.5.6\"}"));
        var secondResult = await secondScope.ServiceProvider.GetRequiredService<IGitVersioning>().GetVersioningInformationAsync();

        await Assert.That(firstResult.SemVer).IsEqualTo("1.2.3");
        await Assert.That(secondResult.SemVer).IsEqualTo("4.5.6");
        await Assert.That(fixture.InstallCalls).IsEqualTo(2);
        await Assert.That(fixture.VersionCalls).IsEqualTo(2);
        await Assert.That(fixture.CommandScopesDisposed).IsEqualTo(2);
    }

    [Test]
    public async Task Independent_Providers_Can_Compute_Concurrently()
    {
        await using var first = new VersioningFixture();
        await using var second = new VersioningFixture();
        var started = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var release = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        first.ExecuteVersion = async (_, _, _) =>
        {
            started.TrySetResult();
            await release.Task;
            return CommandResult.Ok("{\"SemVer\":\"1.2.3\"}");
        };
        var firstResult = first.CreateVersioning().GetVersioningInformationAsync();
        try
        {
            await started.Task.WaitAsync(TimeSpan.FromSeconds(10));
            var secondResult = await second.CreateVersioning().GetVersioningInformationAsync()
                .WaitAsync(TimeSpan.FromSeconds(10));

            await Assert.That(secondResult.SemVer).IsEqualTo("1.2.3");
            await Assert.That(second.ToolFolder).IsNotEqualTo(first.ToolFolder);
        }
        finally
        {
            release.TrySetResult();
            await firstResult;
        }
    }

    [Test]
    [Arguments(null)]
    [Arguments("6.7.0")]
    public async Task Registration_Uses_Pinned_Or_Configured_Tool_Version(string? requestedVersion)
    {
        await using var fixture = new VersioningFixture();
        var services = new ServiceCollection();
        if (requestedVersion is null)
        {
            services.RegisterGitContext();
        }
        else
        {
            services.RegisterGitContext(options => options.ToolVersion = requestedVersion);
        }

        using var provider = services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<GitVersioningOptions>>().Value;
        await fixture.CreateVersioning(options).GetVersioningInformationAsync();

        var arguments = fixture.InstallArguments;
        await Assert.That(arguments[Array.IndexOf(arguments, "--version") + 1])
            .IsEqualTo(requestedVersion ?? "6.8.2");
    }

    [Test]
    [Arguments("install")]
    [Arguments("execute")]
    [Arguments("deserialize")]
    [Arguments("cancel")]
    public async Task Failed_Computation_Cleans_Temporary_Files_And_Can_Be_Retried(string failure)
    {
        await using var fixture = new VersioningFixture();
        using var cancellation = new CancellationTokenSource();
        if (failure == "install")
        {
            fixture.InstallFailure = new InvalidOperationException("Installation failed.");
        }
        else
        {
            fixture.ExecuteVersion = (_, _, token) =>
            {
                if (failure == "deserialize")
                {
                    return Task.FromResult(CommandResult.Ok("invalid json"));
                }

                if (failure == "cancel")
                {
                    cancellation.Cancel();
                    token.ThrowIfCancellationRequested();
                }

                throw new InvalidOperationException("Execution failed.");
            };
        }

        var versioning = fixture.CreateVersioning();
        await Assert.That(async () => { await versioning.GetVersioningInformationAsync(cancellation.Token); }).ThrowsException();
        await Assert.That(Directory.Exists(fixture.ToolFolder)).IsFalse();
        await Assert.That(Directory.GetFileSystemEntries(fixture.Repository.Path)).IsEmpty();

        fixture.InstallFailure = null;
        fixture.ExecuteVersion = (_, _, _) => Task.FromResult(CommandResult.Ok("{\"SemVer\":\"1.2.3\"}"));
        await Assert.That((await fixture.CreateVersioning().GetVersioningInformationAsync()).SemVer).IsEqualTo("1.2.3");
        await Assert.That(fixture.CommandScopesCreated).IsEqualTo(2);
        await Assert.That(fixture.CommandScopesDisposed).IsEqualTo(2);
        await Assert.That(Directory.Exists(fixture.ToolFolder)).IsFalse();
    }

    private sealed class VersioningFixture : IAsyncDisposable
    {
        private readonly List<AsyncServiceScope> _moduleScopes = [];
        private readonly Mock<IGitInformation> _gitInformation = new();
        private ServiceProvider? _provider;

        public VersioningFixture()
        {
            _gitInformation.Setup(x => x.GetInfoAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GitRepositoryInfo(Repository));
        }

        private ICommandContext CreateCommand()
        {
            CommandScopesCreated++;
            var command = new Mock<ICommandContext>();
            command.As<IAsyncDisposable>().Setup(x => x.DisposeAsync())
                .Callback(() => CommandScopesDisposed++)
                .Returns(ValueTask.CompletedTask);
            command.Setup(x => x.ExecuteCommandLineToolAsync(
                    It.IsAny<CommandLineToolOptions>(), It.IsAny<CommandExecutionOptions?>(), It.IsAny<CancellationToken>()))
                .Returns<CommandLineToolOptions, CommandExecutionOptions?, CancellationToken>(async (options, execution, token) =>
                {
                    if (options.Tool == "dotnet")
                    {
                        InstallCalls++;
                        var arguments = options.Arguments!.ToArray();
                        InstallArguments = arguments;
                        ToolFolder = arguments[Array.IndexOf(arguments, "--tool-path") + 1];
                        if (InstallFailure is not null)
                        {
                            throw InstallFailure;
                        }

                        return CommandResult.Ok();
                    }

                    VersionCalls++;
                    return await ExecuteVersion(options, execution, token);
                });
            return command.Object;
        }

        public FolderPath Repository { get; } = FolderPath.CreateTemporaryFolder();

        public string? ToolFolder { get; private set; }

        public string[] InstallArguments { get; private set; } = [];

        public int InstallCalls { get; private set; }

        public int VersionCalls { get; private set; }

        public Exception? InstallFailure { get; set; }

        public Func<CommandLineToolOptions, CommandExecutionOptions?, CancellationToken, Task<CommandResult>> ExecuteVersion { get; set; } =
            (_, _, _) => Task.FromResult(CommandResult.Ok("{\"SemVer\":\"1.2.3\"}"));

        public int CommandScopesCreated { get; private set; }

        public int CommandScopesDisposed { get; private set; }

        public ServiceProvider CreateProvider(GitVersioningOptions? options = null)
        {
            var services = new ServiceCollection();
            services.RegisterGitContext(configuration => configuration.ToolVersion =
                (options ?? new GitVersioningOptions()).ToolVersion);
            services.AddSingleton(_gitInformation.Object);
            services.AddScoped<ICommandContext>(_ => CreateCommand());
            services.AddScoped<IModuleLoggerAccessor>(_ => Mock.Of<IModuleLoggerAccessor>());
            services.AddSingleton<IFileSystemProvider>(SystemFileSystemProvider.Instance);
            return services.BuildServiceProvider(new ServiceProviderOptions { ValidateScopes = true });
        }

        public IGitVersioning CreateVersioning(GitVersioningOptions? options = null)
        {
            _provider ??= CreateProvider(options);
            var scope = _provider.CreateAsyncScope();
            _moduleScopes.Add(scope);
            return scope.ServiceProvider.GetRequiredService<IGitVersioning>();
        }

        public async ValueTask DisposeAsync()
        {
            foreach (var scope in _moduleScopes)
            {
                await scope.DisposeAsync();
            }

            if (_provider is not null)
            {
                await _provider.DisposeAsync();
            }

            Repository.Delete();
        }
    }
}
