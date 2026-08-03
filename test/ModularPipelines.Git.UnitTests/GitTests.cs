using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Git;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Git.Options;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;
using ModularPipelines.TestHelpers.Assertions;

namespace ModularPipelines.Git.UnitTests;

public class GitTests : TestBase
{
    private class GitVersionModule : Module<CommandResult>
    {
        protected override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return await context.Git().Commands.Repository.GitAsync(new GitBaseOptions
            {
                Version = true,
            }, cancellationToken: cancellationToken);
        }
    }

    [Test]
    public async Task Has_Not_Errored()
    {
        var moduleResult = await await RunModule<GitVersionModule>();

        await ModuleResultAssertions.AssertSuccessWithValue(moduleResult);
    }

    [Test]
    public async Task Standard_Output_Starts_With_Git_Version()
    {
        var moduleResult = await await RunModule<GitVersionModule>();

        using (Assert.Multiple())
        {
            await Assert.That(moduleResult.ValueOrDefault!.StandardError).IsNull().Or.IsEmpty();
            await Assert.That(moduleResult.ValueOrDefault.StandardOutput).Matches(@"git version \d+.*");
        }
    }

    [Test]
    public async Task Live_Checkout_Can_Be_Discovered()
    {
        var git = await GetService<IGit>();
        var repositoryInfo = await git.Information.GetInfoAsync();

        using (Assert.Multiple())
        {
            await Assert.That(repositoryInfo).IsNotNull();
            await Assert.That(Directory.Exists(repositoryInfo!.Root.Path)).IsTrue();
        }
    }

    [Test]
    public async Task Repository_Info_Comes_From_Known_Repository()
    {
        using var repository = await TemporaryGitRepository.CreateAsync();
        var gitInformation = await CreateGitInformationAsync(repository.WorkingDirectory);

        var repositoryInfo = await gitInformation.GetInfoAsync();

        using (Assert.Multiple())
        {
            await Assert.That(repositoryInfo).IsNotNull();
            await Assert.That(repositoryInfo!.Root.Path).IsEqualTo(repository.WorkingDirectory);
            await Assert.That(repositoryInfo.BranchName).IsEqualTo("main");
            await Assert.That(repositoryInfo.DefaultBranchName).IsEqualTo("main");
            await Assert.That(repositoryInfo.LastCommitSha).IsEqualTo(repository.LastCommitSha);
            await Assert.That(repositoryInfo.CommitsOnBranch).IsEqualTo(2);
        }
    }

    [Test]
    public async Task Commits_Are_Available_Through_Interface()
    {
        using var repository = await TemporaryGitRepository.CreateAsync();
        var gitInformation = await CreateGitInformationAsync(repository.WorkingDirectory);
        await using var commits = gitInformation.Commits().GetAsyncEnumerator();

        using (Assert.Multiple())
        {
            await Assert.That(await commits.MoveNextAsync()).IsTrue();
            await Assert.That(commits.Current.Message?.Subject).IsEqualTo("second commit");
            await Assert.That(await commits.MoveNextAsync()).IsTrue();
            await Assert.That(commits.Current.Message?.Subject).IsEqualTo("first commit");
            await Assert.That(await commits.MoveNextAsync()).IsFalse();
        }
    }

    [Test]
    public async Task Commands_Are_Grouped_And_Asynchronous()
    {
        var groups = typeof(IGitCommands).GetProperties();
        var commandMethods = groups
            .SelectMany(property => property.PropertyType.GetMethods())
            .Where(method => !method.IsSpecialName)
            .ToList();

        using (Assert.Multiple())
        {
            await Assert.That(groups).Count().IsEqualTo(6);
            await Assert.That(typeof(IGitCommands).GetMethods().All(method => method.IsSpecialName)).IsTrue();
            await Assert.That(commandMethods).Count().IsEqualTo(80);
            await Assert.That(commandMethods.All(method => method.Name.EndsWith("Async", StringComparison.Ordinal))).IsTrue();
        }
    }

    private async Task<GitInformation> CreateGitInformationAsync(string workingDirectory)
    {
        var scopeFactory = await GetService<IServiceScopeFactory>((Action<IServiceCollection>?) null);
        var commitMapper = scopeFactory.Pipeline.Services.GetRequiredService<IGitCommitMapper>();
        return new GitInformation(
            scopeFactory.T,
            commitMapper,
            new CommandExecutionOptions { WorkingDirectory = workingDirectory });
    }

    private sealed class TemporaryGitRepository : IDisposable
    {
        private readonly string _temporaryRoot;

        private TemporaryGitRepository(string temporaryRoot, string workingDirectory, string lastCommitSha)
        {
            _temporaryRoot = temporaryRoot;
            WorkingDirectory = workingDirectory;
            LastCommitSha = lastCommitSha;
        }

        public string WorkingDirectory { get; }

        public string LastCommitSha { get; }

        public static async Task<TemporaryGitRepository> CreateAsync()
        {
            var temporaryRoot = Path.Combine(
                Path.GetTempPath(),
                "ModularPipelines-GitTests",
                Guid.NewGuid().ToString("N"));
            var workingDirectory = Path.Combine(temporaryRoot, "repository");
            var remoteDirectory = Path.Combine(temporaryRoot, "remote.git");
            var hooksDirectory = Path.Combine(temporaryRoot, "hooks");
            Directory.CreateDirectory(temporaryRoot);
            Directory.CreateDirectory(hooksDirectory);

            try
            {
                await RunGitAsync(temporaryRoot, "init", "--bare", "--initial-branch=main", remoteDirectory);
                await RunGitAsync(remoteDirectory, "config", "core.hooksPath", hooksDirectory);
                await RunGitAsync(temporaryRoot, "init", "--initial-branch=main", workingDirectory);
                workingDirectory = Path.GetFullPath(
                    await RunGitAsync(workingDirectory, "rev-parse", "--show-toplevel"));
                await RunGitAsync(workingDirectory, "config", "user.name", "Modular Pipelines Tests");
                await RunGitAsync(workingDirectory, "config", "user.email", "tests@modularpipelines.local");
                await RunGitAsync(workingDirectory, "config", "commit.gpgSign", "false");
                await RunGitAsync(workingDirectory, "config", "push.gpgSign", "false");
                await RunGitAsync(workingDirectory, "config", "protocol.file.allow", "always");
                await RunGitAsync(workingDirectory, "config", "core.hooksPath", hooksDirectory);

                await File.WriteAllTextAsync(Path.Combine(workingDirectory, "first.txt"), "first");
                await RunGitAsync(workingDirectory, "add", "--force", "first.txt");
                await RunGitAsync(workingDirectory, "commit", "-m", "first commit");

                await File.WriteAllTextAsync(Path.Combine(workingDirectory, "second.txt"), "second");
                await RunGitAsync(workingDirectory, "add", "--force", "second.txt");
                await RunGitAsync(workingDirectory, "commit", "-m", "second commit");
                await RunGitAsync(workingDirectory, "remote", "add", "origin", remoteDirectory);
                await RunGitAsync(workingDirectory, "push", "--set-upstream", "origin", "main");

                var lastCommitSha = await RunGitAsync(workingDirectory, "rev-parse", "HEAD");
                return new TemporaryGitRepository(temporaryRoot, workingDirectory, lastCommitSha);
            }
            catch
            {
                DeleteTemporaryRoot(temporaryRoot);
                throw;
            }
        }

        public void Dispose()
        {
            DeleteTemporaryRoot(_temporaryRoot);
        }

        private static void DeleteTemporaryRoot(string temporaryRoot)
        {
            if (!Directory.Exists(temporaryRoot))
            {
                return;
            }

            foreach (var file in Directory.EnumerateFiles(temporaryRoot, "*", SearchOption.AllDirectories))
            {
                File.SetAttributes(file, FileAttributes.Normal);
            }

            foreach (var directory in Directory.EnumerateDirectories(temporaryRoot, "*", SearchOption.AllDirectories))
            {
                File.SetAttributes(directory, FileAttributes.Normal);
            }

            Directory.Delete(temporaryRoot, true);
        }

        private static async Task<string> RunGitAsync(string workingDirectory, params string[] arguments)
        {
            var startInfo = new ProcessStartInfo("git")
            {
                WorkingDirectory = workingDirectory,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
            };
            foreach (var argument in arguments)
            {
                startInfo.ArgumentList.Add(argument);
            }

            using var process = Process.Start(startInfo)
                                ?? throw new InvalidOperationException("Could not start git.");
            var outputTask = process.StandardOutput.ReadToEndAsync();
            var errorTask = process.StandardError.ReadToEndAsync();

            await process.WaitForExitAsync();
            var output = await outputTask;
            var error = await errorTask;
            if (process.ExitCode != 0)
            {
                throw new InvalidOperationException($"git {string.Join(' ', arguments)} failed: {error}");
            }

            return output.Trim();
        }
    }
}
