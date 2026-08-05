using ModularPipelines.Attributes;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Exceptions;
using ModularPipelines.FileSystem;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;

namespace ModularPipelines.Testing.UnitTests;

public class ModuleTesterTests
{
    private const string InterceptedSecret = "intercepted-command-secret";

    [Test]
    public async Task ExecutesModuleAndReturnsTypedValue()
    {
        var run = await ModuleTester.For<ValueModule, string>().ExecuteAsync();

        using (Assert.Multiple())
        {
            await Assert.That(run.Value).IsEqualTo("value");
            await Assert.That(run.Result).IsTypeOf<ModuleResult<string>.Success>();
            await Assert.That(run.Exception).IsNull();
            await Assert.That(run.SkipDecision).IsNull();
        }
    }

    [Test]
    public async Task ProposalCompatibleApiSeedsDependencyResult()
    {
        var run = await ModuleTester.For<DependentModule>()
            .WithDependencyResult<DependencyModule, string>("seeded")
            .ExecuteAsync();

        await Assert.That(run.Value).IsEqualTo("seeded consumed");
    }

    [Test]
    public async Task InterceptsAndRecordsParsedCommands()
    {
        var run = await ModuleTester.For<CommandModule, string>()
            .InterceptCommands(invocation =>
                CommandResult.Ok($"stubbed {invocation.CommandLine.Tool}"))
            .ExecuteAsync();

        using (Assert.Multiple())
        {
            await Assert.That(run.Value).IsEqualTo("stubbed imaginary-tool");
            await Assert.That(run.Commands).Count().IsEqualTo(1);
            await Assert.That(run.Commands[0].CommandLine.Tool).IsEqualTo("imaginary-tool");
            await Assert.That(run.Commands[0].CommandLine.Arguments)
                .IsEquivalentTo(["build", "--configuration", "Release"]);
            await Assert.That(run.Commands[0].Result.StandardOutput)
                .IsEqualTo("stubbed imaginary-tool");
            await Assert.That(run.Commands[0].Result.WorkingDirectory)
                .IsEqualTo(Path.GetTempPath());
            await Assert.That(run.Commands[0].Result.EnvironmentVariables["RECORDED_VALUE"])
                .IsEqualTo("effective");
        }
    }

    [Test]
    public async Task UsesSafeDefaultCommandStub()
    {
        var run = await ModuleTester.For<CommandModule, string>().ExecuteAsync();

        using (Assert.Multiple())
        {
            await Assert.That(run.Commands).Count().IsEqualTo(1);
            await Assert.That(run.Commands[0].Result.ExitCode).IsEqualTo(0);
            await Assert.That(run.Exception).IsNull();
        }
    }

    [Test]
    public async Task InterceptedCommandHonorsThrowOnNonZeroExitCode()
    {
        var run = await ModuleTester.For<CommandModule, string>()
            .InterceptCommands(_ => CommandResult.Ok(standardError: "failed") with
            {
                ExitCode = 1,
            })
            .ExecuteAsync();

        await Assert.That(run.Exception).IsTypeOf<CommandException>();
    }

    [Test]
    public async Task InterceptedCommandFailureObfuscatesExceptionResult()
    {
        var run = await ModuleTester.For<SecretCommandModule, string>()
            .WithService<ISecretObfuscator>(new TestSecretObfuscator())
            .InterceptCommands(_ => CommandResult.Ok(InterceptedSecret, InterceptedSecret) with
            {
                ExitCode = 1,
            })
            .ExecuteAsync();

        var exception = (CommandException) run.Exception!;
        using (Assert.Multiple())
        {
            await Assert.That(exception.Result.CommandInput).DoesNotContain(InterceptedSecret);
            await Assert.That(exception.Result.StandardOutput).DoesNotContain(InterceptedSecret);
            await Assert.That(exception.Result.StandardError).DoesNotContain(InterceptedSecret);
            await Assert.That(exception.Result.EnvironmentVariables["SECRET_VALUE"])
                .DoesNotContain(InterceptedSecret);
        }
    }

    [Test]
    [Timeout(5_000)]
    public async Task InterceptedCommandHonorsExecutionTimeout(
        CancellationToken cancellationToken)
    {
        var run = await ModuleTester.For<TimedCommandModule, string>()
            .InterceptCommands(async (_, token) =>
            {
                await Task.Delay(Timeout.InfiniteTimeSpan, token);
                return CommandResult.Ok();
            })
            .ExecuteAsync(cancellationToken);

        await Assert.That(run.Exception).IsTypeOf<TimeoutException>();
    }

    [Test]
    [Timeout(5_000)]
    public async Task SynchronousInterceptorCannotReturnAfterExecutionTimeout(
        CancellationToken cancellationToken)
    {
        var run = await ModuleTester.For<TimedCommandModule, string>()
            .InterceptCommands(_ =>
            {
                Thread.Sleep(200);
                return CommandResult.Ok();
            })
            .ExecuteAsync(cancellationToken);

        await Assert.That(run.Exception).IsTypeOf<TimeoutException>();
    }

    [Test]
    [Timeout(5_000)]
    public async Task MissingRequiredDependencyFailsFast(CancellationToken cancellationToken)
    {
        async Task Act() =>
            _ = await ModuleTester.For<AttributedDependentModule>()
                .ExecuteAsync(cancellationToken);

        var exception = await Assert.That(Act)
            .Throws<InvalidOperationException>();

        await Assert.That(exception!.Message).Contains(nameof(DependencyModule));
    }

    [Test]
    public async Task MissingConsumedArtifactFailsBeforeModuleExecution()
    {
        var run = await ModuleTester.For<ArtifactConsumerModule, string>()
            .WithDependencyResult<ArtifactProducerModule, string>("producer result")
            .ExecuteAsync();

        using (Assert.Multiple())
        {
            await Assert.That(run.Exception).IsTypeOf<InvalidOperationException>();
            await Assert.That(run.Exception!.Message).Contains("Call WithArtifact to seed it");
        }
    }

    [Test]
    public async Task SeedsConsumedArtifactInVirtualFileSystem()
    {
        var fileSystem = new InMemoryFileSystemProvider();
        var run = await ModuleTester.For<ArtifactConsumerModule, string>()
            .WithService<IFileSystemProvider>(fileSystem)
            .WithDependencyResult<ArtifactProducerModule, string>("producer result")
            .WithArtifact<ArtifactProducerModule>("application", "artifact contents")
            .ExecuteAsync();

        using (Assert.Multiple())
        {
            await Assert.That(run.Value).IsEqualTo("artifact contents");
            await Assert.That(run.Exception).IsNull();
            await Assert.That(run.FileSystem).IsSameReferenceAs(fileSystem);
        }
    }

    [Test]
    public async Task ReportsSkipDecisionWithoutExecutingModule()
    {
        var run = await ModuleTester.For<SkippedModule, string>().ExecuteAsync();

        using (Assert.Multiple())
        {
            await Assert.That(run.SkipDecision).IsNotNull();
            await Assert.That(run.SkipDecision!.Reason).IsEqualTo("not needed");
            await Assert.That(run.Result).IsTypeOf<ModuleResult<string>.Skipped>();
        }
    }

    [Test]
    public async Task CapturesModuleFailure()
    {
        var run = await ModuleTester.For<FailingModule, string>().ExecuteAsync();

        using (Assert.Multiple())
        {
            await Assert.That(run.Exception).IsTypeOf<InvalidOperationException>();
            await Assert.That(run.Result).IsTypeOf<ModuleResult<string>.Failure>();
        }
    }

    [Test]
    public async Task ProvidesIsolatedInMemoryFileSystem()
    {
        var physicalPath = Path.Combine(
            Path.GetTempPath(),
            $"modular-pipelines-test-{Guid.NewGuid():N}",
            "artifact.txt");

        var run = await ModuleTester.For<FileModule, string>()
            .WithService(new FilePath(physicalPath))
            .ExecuteAsync();

        using (Assert.Multiple())
        {
            await Assert.That(run.Value).IsEqualTo("contents");
            await Assert.That(run.FileSystem.FileExists(physicalPath)).IsTrue();
            await Assert.That(System.IO.File.Exists(physicalPath)).IsFalse();
        }
    }

    [Test]
    public async Task VirtualizesFolderEnumerationAndCleaning()
    {
        var root = Path.Combine(
            Path.GetTempPath(),
            $"modular-pipelines-test-{Guid.NewGuid():N}");

        var run = await ModuleTester.For<FolderModule, string>()
            .WithService(new FilePath(root))
            .ExecuteAsync();

        using (Assert.Multiple())
        {
            await Assert.That(run.Value).IsEqualTo("1:2:0");
            await Assert.That(Directory.Exists(root)).IsFalse();
        }
    }

    [Test]
    public async Task UnsupportedVirtualMetadataFailsLoudly()
    {
        var root = Path.Combine(
            Path.GetTempPath(),
            $"modular-pipelines-test-{Guid.NewGuid():N}");

        var run = await ModuleTester.For<FileMetadataModule, long>()
            .WithService(new FilePath(Path.Combine(root, "artifact.txt")))
            .ExecuteAsync();

        await Assert.That(run.Exception).IsTypeOf<NotSupportedException>();
    }

    [Test]
    public async Task CopiesFoldersInVirtualFileSystem()
    {
        var root = Path.Combine(
            Path.GetTempPath(),
            $"modular-pipelines-test-{Guid.NewGuid():N}");

        var run = await ModuleTester.For<FolderCopyModule, string>()
            .WithService(new FilePath(root))
            .ExecuteAsync();

        using (Assert.Multiple())
        {
            await Assert.That(run.Value).IsEqualTo("contents");
            await Assert.That(run.Exception).IsNull();
            await Assert.That(Directory.Exists(root)).IsFalse();
        }
    }

    [Test]
    public async Task ConcurrentCommandsPreserveInvocationOrder()
    {
        var releaseFirstCommand = new TaskCompletionSource(
            TaskCreationOptions.RunContinuationsAsynchronously);
        var run = await ModuleTester.For<ConcurrentCommandModule, string>()
            .InterceptCommands(async (invocation, cancellationToken) =>
            {
                if (invocation.CommandLine.Tool == "first-tool")
                {
                    await releaseFirstCommand.Task.WaitAsync(cancellationToken);
                }
                else
                {
                    releaseFirstCommand.TrySetResult();
                }

                return CommandResult.Ok(invocation.CommandLine.Tool);
            })
            .ExecuteAsync();

        using (Assert.Multiple())
        {
            await Assert.That(run.Commands).Count().IsEqualTo(2);
            await Assert.That(run.Commands[0].CommandLine.Tool).IsEqualTo("first-tool");
            await Assert.That(run.Commands[1].CommandLine.Tool).IsEqualTo("second-tool");
        }
    }

    [Test]
    public async Task ReturnsOverriddenFileSystemProvider()
    {
        var fileSystem = new InMemoryFileSystemProvider();
        var path = Path.Combine(fileSystem.GetTempPath(), "overridden-provider.txt");

        var run = await ModuleTester.For<FileModule, string>()
            .WithService<IFileSystemProvider>(fileSystem)
            .WithService(new FilePath(path))
            .ExecuteAsync();

        using (Assert.Multiple())
        {
            await Assert.That(run.Value).IsEqualTo("contents");
            await Assert.That(run.FileSystem).IsSameReferenceAs(fileSystem);
            await Assert.That(fileSystem.FileExists(path)).IsTrue();
        }
    }

    [Test]
    public async Task VirtualizesCompleteFilesContext()
    {
        var relativeRoot = $"module-tester-{Guid.NewGuid():N}";
        var physicalRoot = Path.Combine(Environment.CurrentDirectory, relativeRoot);

        var run = await ModuleTester.For<FilesContextModule, string>()
            .WithService(new FilePath(relativeRoot))
            .ExecuteAsync();

        using (Assert.Multiple())
        {
            await Assert.That(run.Value).IsEqualTo("contents:1:1:1:1");
            await Assert.That(run.Exception).IsNull();
            await Assert.That(run.FileSystem.FileExists(
                Path.Combine(physicalRoot, "artifact.txt"))).IsTrue();
            await Assert.That(run.FileSystem.FileExists($"{physicalRoot}.zip")).IsTrue();
            await Assert.That(run.FileSystem.FileExists(
                Path.Combine($"{physicalRoot}-unzipped", "artifact.txt"))).IsTrue();
            await Assert.That(Directory.Exists(physicalRoot)).IsFalse();
            await Assert.That(System.IO.File.Exists($"{physicalRoot}.zip")).IsFalse();
        }
    }

    [Test]
    public async Task RootMatchingExclusionDoesNotHideDescendants()
    {
        var root = Path.Combine(
            Path.GetTempPath(),
            $"root-exclusion-{Guid.NewGuid():N}");

        var run = await ModuleTester.For<FolderExclusionModule, int>()
            .WithService(new FilePath(root))
            .ExecuteAsync();

        await Assert.That(run.Value).IsEqualTo(2);
    }

    [Test]
    [Timeout(5_000)]
    public async Task CallerCancellationStopsRunningModule(CancellationToken cancellationToken)
    {
        var started = new TaskCompletionSource(
            TaskCreationOptions.RunContinuationsAsynchronously);
        using var cancellationTokenSource =
            CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        var executionTask = ModuleTester.For<CancellableModule, string>()
            .WithService(started)
            .ExecuteAsync(cancellationTokenSource.Token);

        await started.Task.WaitAsync(cancellationToken);
        await cancellationTokenSource.CancelAsync();
        var run = await executionTask.WaitAsync(cancellationToken);

        await Assert.That(run.Exception).IsTypeOf<OperationCanceledException>();
    }

    [Test]
    [Timeout(5_000)]
    public async Task CallerCancellationStopsAlwaysRunModule(CancellationToken cancellationToken)
    {
        var started = new TaskCompletionSource(
            TaskCreationOptions.RunContinuationsAsynchronously);
        using var cancellationTokenSource =
            CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        var executionTask = ModuleTester.For<CancellableAlwaysRunModule, string>()
            .WithService(started)
            .ExecuteAsync(cancellationTokenSource.Token);

        await started.Task.WaitAsync(cancellationToken);
        await cancellationTokenSource.CancelAsync();
        var run = await executionTask.WaitAsync(cancellationToken);

        await Assert.That(run.Exception).IsTypeOf<OperationCanceledException>();
    }

    [Test]
    public async Task ZipOutputHandlingUsesVirtualFileSystem()
    {
        var root = Path.Combine(
            Path.GetTempPath(),
            $"zip-output-{Guid.NewGuid():N}");
        var hostCollision = Path.Combine(root, "host-collision.zip");
        Directory.CreateDirectory(hostCollision);

        try
        {
            var run = await ModuleTester.For<ZipOutputModule, string>()
                .WithService(new FilePath(root))
                .ExecuteAsync();

            await Assert.That(run.Value).IsEqualTo("True:True:True");
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    public sealed class ValueModule : Module<string>
    {
        protected override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
            => Task.FromResult("value");
    }

    public sealed class DependencyModule : Module<string>
    {
        protected override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
            => throw new InvalidOperationException("Seeded dependency must not execute.");
    }

    public sealed class DependentModule : Module<string>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .DependsOn<DependencyModule>()
            .Build();

        protected override async Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            var dependency = await context.GetModule<DependencyModule>();
            return $"{dependency.Value} consumed";
        }
    }

    [ModularPipelines.Attributes.DependsOn<DependencyModule>]
    public sealed class AttributedDependentModule : Module<string>
    {
        protected override async Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            var dependency = await context.GetModule<DependencyModule>();
            return $"{dependency.Value} consumed";
        }
    }

    [ProducesArtifact("application", "application")]
    public sealed class ArtifactProducerModule : Module<string>
    {
        protected override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
            => throw new InvalidOperationException("Seeded producer must not execute.");
    }

    [ModularPipelines.Attributes.DependsOn<ArtifactProducerModule>]
    [ConsumesArtifact(
        typeof(ArtifactProducerModule),
        "application",
        RestorePath = "restored-artifacts")]
    public sealed class ArtifactConsumerModule : Module<string>
    {
        protected override async Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            var artifact = context.Files.GetFile(
                Path.Combine("restored-artifacts", "application"));
            return await artifact.ReadAsync(cancellationToken)
                ?? throw new InvalidOperationException("Seeded artifact was not restored.");
        }
    }

    public sealed class CommandModule : Module<string>
    {
        protected override async Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            var result = await context.Shell.Command.ExecuteCommandLineToolAsync(
                new GenericCommandLineToolOptions("imaginary-tool")
                {
                    Arguments = ["build", "--configuration", "Release"],
                },
                new CommandExecutionOptions
                {
                    WorkingDirectory = Path.GetTempPath(),
                    EnvironmentVariables = new Dictionary<string, string?>
                    {
                        ["RECORDED_VALUE"] = "effective",
                    },
                },
                cancellationToken: cancellationToken);

            return result.StandardOutput;
        }
    }

    public sealed class SkippedModule : Module<string>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithSkipWhen(_ => SkipDecision.Skip("not needed"))
            .Build();

        protected override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
            => throw new InvalidOperationException("Skipped module must not execute.");
    }

    public sealed class FailingModule : Module<string>
    {
        protected override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
            => throw new InvalidOperationException("expected");
    }

    public sealed class FileModule(FilePath filePath) : Module<string>
    {
        protected override async Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            var file = context.Files.GetFile(filePath.Value);
            file.Folder!.Create();
            await file.WriteAsync("contents", cancellationToken);
            return await file.ReadAsync(cancellationToken)
                ?? throw new InvalidOperationException("The written test file could not be read.");
        }
    }

    public sealed class FolderModule(FilePath rootPath) : Module<string>
    {
        protected override async Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            var folder = context.Files.GetFolder(rootPath.Value).Create();
            await folder.GetFile("first.txt").WriteAsync("first", cancellationToken);
            var nested = folder.CreateFolder("nested");
            await nested.GetFile("second.txt").WriteAsync("second", cancellationToken);

            var listedCount = folder.ListFiles().Count();
            var recursiveCount = folder.GetFiles(file => file.Extension == ".txt").Count();
            folder.Clean();
            return $"{listedCount}:{recursiveCount}:{folder.ListFiles().Count()}";
        }
    }

    public sealed class FileMetadataModule(FilePath filePath) : Module<long>
    {
        protected override async Task<long> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            var file = context.Files.GetFile(filePath.Value);
            file.Folder!.Create();
            await file.WriteAsync("contents", cancellationToken);
            return file.Length;
        }
    }

    public sealed class FolderCopyModule(FilePath rootPath) : Module<string>
    {
        protected override async Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            var root = context.Files.GetFolder(rootPath.Value).Create();
            var source = root.CreateFolder("source");
            await source.GetFile("artifact.txt").WriteAsync("contents", cancellationToken);

            var copy = source.CopyTo(root.GetFolder("copy").Path);
            return await copy.GetFile("artifact.txt").ReadAsync(cancellationToken);
        }
    }

    public sealed class SecretCommandModule : Module<string>
    {
        protected override async Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            var result = await context.Shell.Command.ExecuteCommandLineToolAsync(
                new GenericCommandLineToolOptions("imaginary-tool")
                {
                    Arguments = [InterceptedSecret],
                },
                new CommandExecutionOptions
                {
                    EnvironmentVariables = new Dictionary<string, string?>
                    {
                        ["SECRET_VALUE"] = InterceptedSecret,
                    },
                },
                cancellationToken: cancellationToken);

            return result.StandardOutput;
        }
    }

    private sealed class TestSecretObfuscator : ISecretObfuscator
    {
        public string Obfuscate(string? input, object? optionsObject) =>
            input?.Replace(InterceptedSecret, "********", StringComparison.Ordinal)
            ?? string.Empty;
    }

    public sealed class ConcurrentCommandModule : Module<string>
    {
        protected override async Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            var first = context.Shell.Command.ExecuteCommandLineToolAsync(
                new GenericCommandLineToolOptions("first-tool"),
                cancellationToken: cancellationToken);
            var second = context.Shell.Command.ExecuteCommandLineToolAsync(
                new GenericCommandLineToolOptions("second-tool"),
                cancellationToken: cancellationToken);

            var results = await Task.WhenAll(first, second);
            return string.Join(',', results.Select(result => result.StandardOutput));
        }
    }

    public sealed class TimedCommandModule : Module<string>
    {
        protected override async Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            var result = await context.Shell.Command.ExecuteCommandLineToolAsync(
                new GenericCommandLineToolOptions("imaginary-tool"),
                new CommandExecutionOptions
                {
                    ExecutionTimeout = TimeSpan.FromMilliseconds(100),
                },
                cancellationToken: cancellationToken);

            return result.StandardOutput;
        }
    }

    public sealed class FilesContextModule(FilePath rootPath) : Module<string>
    {
        protected override async Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            var root = context.Files.GetFolder(rootPath.Value).Create();
            var filePath = Path.Combine(rootPath.Value, "artifact.txt");
            await context.Files.WriteAsync(filePath, "contents", cancellationToken);
            var contents = await context.Files.ReadAsync(filePath, cancellationToken);
            var exists = await context.Files.ExistsAsync(filePath, cancellationToken);
            var files = context.Files.Glob($"{rootPath.Value}/**/*.txt").Count();
            var folders = context.Files.GlobFolders(rootPath.Value).Count();
            var checksum = context.Files.Checksum.Md5(filePath);

            var zipPath = Path.GetFullPath($"{rootPath.Value}.zip");
            context.Files.Zip.ZipFolder(root, zipPath);
            context.Files.Zip.UnZipToFolder(
                zipPath,
                Path.GetFullPath($"{rootPath.Value}-unzipped"));

            return $"{contents}:{(exists ? 1 : 0)}:{files}:{folders}:{checksum.Length / 32}";
        }
    }

    public sealed class FolderExclusionModule(FilePath rootPath) : Module<int>
    {
        protected override async Task<int> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            var root = context.Files.GetFolder(rootPath.Value).Create();
            await root.GetFile("root.txt").WriteAsync("root", cancellationToken);
            await root.CreateFolder("nested")
                .GetFile("nested.txt")
                .WriteAsync("nested", cancellationToken);

            return root.GetFiles(
                _ => true,
                candidate => candidate.Path == root.Path).Count();
        }
    }

    public sealed class CancellableModule(TaskCompletionSource started) : Module<string>
    {
        protected override async Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            started.TrySetResult();
            await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);
            return "unreachable";
        }
    }

    public sealed class CancellableAlwaysRunModule(TaskCompletionSource started) : Module<string>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithAlwaysRun()
            .Build();

        protected override async Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            started.TrySetResult();
            await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);
            return "unreachable";
        }
    }

    public sealed class ZipOutputModule(FilePath rootPath) : Module<string>
    {
        protected override async Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            var root = context.Files.GetFolder(rootPath.Value).Create();
            var source = root.CreateFolder("source");
            await source.GetFile("artifact.txt").WriteAsync("contents", cancellationToken);

            var dottedDirectory = root.CreateFolder("archives.v1");
            var directoryZip = context.Files.Zip.ZipFolder(source, dottedDirectory.Path);

            var extensionlessFile = root.GetFile("archive");
            await extensionlessFile.WriteAsync("existing", cancellationToken);
            var rejectedExistingFile = false;
            try
            {
                context.Files.Zip.ZipFolder(source, extensionlessFile.Path);
            }
            catch (IOException)
            {
                rejectedExistingFile = true;
            }

            var preservedExistingFile =
                await extensionlessFile.ReadAsync(cancellationToken) == "existing";
            var collidingOutput = root.GetFile("host-collision.zip");
            var isolatedZip = context.Files.Zip.ZipFolder(source, collidingOutput.Path);

            return $"{directoryZip.Folder?.Path == dottedDirectory.Path}:"
                   + $"{rejectedExistingFile && preservedExistingFile}:"
                   + $"{isolatedZip.Path == collidingOutput.Path}";
        }
    }

    public sealed record FilePath(string Value);
}
