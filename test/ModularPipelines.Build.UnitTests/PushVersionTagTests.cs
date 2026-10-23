using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Build.Modules;
using ModularPipelines.Build.Settings;
using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Git;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.Testing;
using Moq;

namespace ModularPipelines.Build.UnitTests;

public class PushVersionTagTests
{
    [Test]
    [Arguments(null, false)]
    [Arguments("", false)]
    [Arguments("Build", false)]
    [Arguments("Build,VersionTag", false)]
    [Arguments("VersionTag", true)]
    [Arguments("versiontag", true)]
    public async Task Requires_Explicit_VersionTag_Execution(string? categories, bool expected)
    {
        var condition = typeof(PushVersionTagModule).GetCustomAttributes<RunConditionAttribute>()
            .SingleOrDefault(attribute => attribute.ConditionNames == "ExplicitVersionTag");
        await Assert.That(condition).IsNotNull();
        var options = new PipelineOptions { RunOnlyCategories = categories?.Split(',') };
        var services = Mock.Of<IServicesContext>(servicesContext => servicesContext.Options == options);
        var context = Mock.Of<IPipelineContext>(pipeline => pipeline.Services == services);

        await Assert.That(await condition!.EvaluateAsync(context, CancellationToken.None)).IsEqualTo(expected);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Pushes_Only_Selected_Tag_And_Propagates_Rejection(bool rejectPush)
    {
        var root = Directory.CreateTempSubdirectory("version-tag-test-").FullName;
        try
        {
            var remote = Path.Combine(root, "remote.git");
            var local = Path.Combine(root, "local");
            await Git(root, "init", "--bare", remote);
            await Git(root, "init", local);
            await Git(local, "-c", "user.name=Test", "-c", "user.email=test@example.invalid",
                "-c", "commit.gpgsign=false", "commit", "--allow-empty", "-m", "initial");
            await Git(local, "remote", "add", "origin", remote);
            await Git(local, "tag", "unrelated-local-tag");
            if (rejectPush)
            {
                var hook = Path.Combine(remote, "hooks", "pre-receive");
                await File.WriteAllTextAsync(hook, "#!/bin/sh\necho 'version tag rejected by test hook' >&2\nexit 1\n");
                if (!OperatingSystem.IsWindows())
                {
                    File.SetUnixFileMode(hook, UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.UserExecute);
                }
            }

            async ValueTask<CommandResult> ExecuteGit(CommandInvocation invocation, CancellationToken token)
            {
                var arguments = invocation.CommandLine.Arguments.ToArray();
                // Keep the local test remote instead of configuring GitHub authentication.
                if (arguments is ["remote", "set-url", ..])
                {
                    return CommandResult.Ok();
                }

                return await RunGit(local, arguments, token);
            }

            var tester = ModuleTester.For<TagExecutionModule>()
                .WithService<IOptions<GitHubSettings>>(Microsoft.Extensions.Options.Options.Create(new GitHubSettings
                {
                    StandardToken = "test-token",
                }))
                .WithService<IOptions<PublishSettings>>(Microsoft.Extensions.Options.Options.Create(new PublishSettings()))
                .WithDependencyResult<NugetVersionGeneratorModule, string>("4.0.0")
                .ConfigurePipeline(builder => builder.Services.RegisterGitContext())
                .InterceptCommands(ExecuteGit);

            var run = await tester.ExecuteAsync();
            if (rejectPush)
            {
                await Assert.That(run.Result.Status).IsEqualTo(ModuleStatus.Failed);
                await Assert.That(run.Exception).IsTypeOf<CommandException>();
                var exception = (CommandException) run.Exception!;
                await Assert.That(exception.Result.StandardError).Contains("version tag rejected by test hook");
                await Assert.That(await Git(root, "--git-dir", remote, "tag", "--list")).IsEmpty();
            }
            else
            {
                await Assert.That(run.Exception).IsNull();
                await Assert.That(await Git(root, "--git-dir", remote, "tag", "--list")).IsEqualTo("v4.0.0");
                await Assert.That(await Git(root, "--git-dir", remote, "rev-parse", "refs/tags/v4.0.0"))
                    .IsEqualTo(await Git(local, "rev-parse", "HEAD"));

                // Rerunning a successful workflow is safe for the same version and commit.
                var repeated = await tester.ExecuteAsync();
                await Assert.That(repeated.Exception).IsNull();
            }
        }
        finally
        {
            foreach (var file in Directory.EnumerateFiles(root, "*", SearchOption.AllDirectories))
            {
                File.SetAttributes(file, FileAttributes.Normal);
            }

            Directory.Delete(root, recursive: true);
        }
    }

    // Exercise the command body on every OS, independently of CI-only run conditions.
    [DependsOn<NugetVersionGeneratorModule>]
    public class TagExecutionModule(IOptions<GitHubSettings> settings) : Module<CommandResult>
    {
        protected override Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            new ExposedTagModule(settings).Execute(context, cancellationToken);
    }

    private class ExposedTagModule(IOptions<GitHubSettings> settings) : PushVersionTagModule(settings)
    {
        public Task<CommandResult> Execute(IModuleContext context, CancellationToken cancellationToken) =>
            base.ExecuteAsync(context, cancellationToken);
    }

    private static async Task<string> Git(string directory, params string[] arguments)
    {
        var result = await RunGit(directory, arguments, CancellationToken.None);
        if (result.ExitCode != 0)
        {
            throw new InvalidOperationException(result.StandardError);
        }

        return result.StandardOutput.Trim();
    }

    private static async Task<CommandResult> RunGit(string directory, string[] arguments, CancellationToken token)
    {
        var startInfo = new ProcessStartInfo("git")
        {
            WorkingDirectory = directory,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
        };
        foreach (var argument in arguments)
        {
            startInfo.ArgumentList.Add(argument);
        }

        using var process = Process.Start(startInfo) ?? throw new InvalidOperationException("Could not start git.");
        var output = process.StandardOutput.ReadToEndAsync(token);
        var error = process.StandardError.ReadToEndAsync(token);
        await process.WaitForExitAsync(token);
        return CommandResult.Ok(await output, await error) with { ExitCode = process.ExitCode };
    }
}
