using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;

namespace ModularPipelines.Testing.UnitTests;

public class ModuleTesterTests
{
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

    public sealed class ValueModule : Module<string>
    {
        protected override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
            => Task.FromResult<string?>("value");
    }

    public sealed class DependencyModule : Module<string>
    {
        protected override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
            => throw new InvalidOperationException("Seeded dependency must not execute.");
    }

    public sealed class DependentModule : Module<string>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .DependsOn<DependencyModule>()
            .Build();

        protected override async Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            var dependency = await context.GetModule<DependencyModule>();
            return $"{dependency.ValueOrDefault} consumed";
        }
    }

    public sealed class CommandModule : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            var result = await context.Shell.Command.ExecuteCommandLineToolAsync(
                new GenericCommandLineToolOptions("imaginary-tool")
                {
                    Arguments = ["build", "--configuration", "Release"],
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

        protected override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
            => throw new InvalidOperationException("Skipped module must not execute.");
    }

    public sealed class FailingModule : Module<string>
    {
        protected override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
            => throw new InvalidOperationException("expected");
    }

    public sealed class FileModule(FilePath filePath) : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            var file = context.Files.GetFile(filePath.Value);
            await file.WriteAsync("contents", cancellationToken);
            return file.Exists ? await file.ReadAsync(cancellationToken) : null;
        }
    }

    public sealed record FilePath(string Value);
}
