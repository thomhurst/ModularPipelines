using System.Diagnostics;
using System.IO.Compression;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Build.Modules;
using ModularPipelines.Build.Settings;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Extensions;
using ModularPipelines.Exceptions;
using ModularPipelines.FileSystem;
using ModularPipelines.GitHub;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Testing;
using Moq;
using Octokit;

namespace ModularPipelines.Build.UnitTests;

public class PublishRehearsalTests
{
    [Test]
    public async Task Pushes_Packages_And_Symbols_To_Scratch_Feed_And_Retries()
    {
        var root = RepositoryRoot();
        var directory = Directory.CreateDirectory(Path.Combine(root, "artifacts", "publish-rehearsal-tests", Guid.NewGuid().ToString("N")));
        try
        {
            var library = await PackPackage(root, directory.FullName, "ModularPipelines.Testing", symbols: true);
            // Exercise packages larger than Kestrel's default 30,000,000-byte request limit.
            using (var archive = ZipFile.Open(library, ZipArchiveMode.Update))
            {
                var payload = archive.CreateEntry("large-payload.bin", CompressionLevel.NoCompression);
                await using var stream = payload.Open();
                await stream.WriteAsync(new byte[32 * 1024 * 1024]);
            }

            var content = await PackPackage(root, directory.FullName, "ModularPipelines.Templates", symbols: false);
            var feed = Path.Combine(directory.FullName, "scratch feed");
            var commands = 0;
            var tester = ModuleTester.For<RehearsalExecutionModule>()
                .WithService<IOptions<PublishSettings>>(Microsoft.Extensions.Options.Options.Create(new PublishSettings { RehearsalFeedPath = feed }))
                .WithDependencyResult<PackagePathsParserModule, List<FilePath>>([new(library), new(content)])
                .ConfigurePipeline(builder => builder.Services.RegisterDotNetContext())
                .InterceptCommands(async (command, token) =>
                {
                    if (command.CommandLine.Tool != "dotnet")
                    {
                        return CommandResult.Ok();
                    }

                    Interlocked.Increment(ref commands);
                    return await RunDotNet(root, [.. command.CommandLine.Arguments], token);
                });

            var result = await tester.ExecuteAsync();
            await Assert.That(result.Exception).IsNull();
            await Assert.That(commands).IsEqualTo(4);
            foreach (var package in new[] { library, Path.ChangeExtension(library, ".snupkg"), content })
            {
                var uploaded = Directory.GetFiles(feed, Path.GetFileName(package), SearchOption.AllDirectories).Single();
                var actual = await File.ReadAllBytesAsync(uploaded);
                var expected = await File.ReadAllBytesAsync(package);
                await Assert.That(actual.SequenceEqual(expected)).IsTrue();
            }

            using var manifest = JsonDocument.Parse(await File.ReadAllTextAsync(Path.Combine(feed, "manifest.json")));
            await Assert.That(manifest.RootElement.GetProperty("PackageCount").GetInt32()).IsEqualTo(2);
            await Assert.That(manifest.RootElement.GetProperty("Files").GetArrayLength()).IsEqualTo(3);

            // Recover symbols after a partial publish, even when the primary package already exists.
            var uploadedSymbols = Path.Combine(feed, Path.GetFileName(Path.ChangeExtension(library, ".snupkg")));
            File.Delete(uploadedSymbols);
            var recovered = await tester.ExecuteAsync();
            await Assert.That(recovered.Exception).IsNull();
            await Assert.That(File.Exists(uploadedSymbols)).IsTrue();

            // A missing package must fail the rehearsal, rather than silently validating a partial release.
            File.Delete(content);
            var failed = await tester.ExecuteAsync();
            await Assert.That(failed.Result.Status).IsEqualTo(ModuleStatus.Failed);
            await Assert.That(failed.Exception).IsTypeOf<CommandException>();
            await Assert.That(File.Exists(Path.Combine(feed, "manifest.json"))).IsFalse();
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    [Arguments("https://api.nuget.org/v3/index.json")]
    [Arguments("relative-feed")]
    public async Task Rejects_Remote_And_Relative_Feeds(string feed)
    {
        var result = await ModuleTester.For<RehearsalExecutionModule>()
            .WithService<IOptions<PublishSettings>>(Microsoft.Extensions.Options.Options.Create(new PublishSettings { RehearsalFeedPath = feed }))
            .WithDependencyResult<PackagePathsParserModule, List<FilePath>>([])
            .ExecuteAsync();

        await Assert.That(result.Exception).IsTypeOf<InvalidOperationException>();
        await Assert.That(result.Exception!.Message).Contains("absolute local feed");
    }

    [Test]
    public async Task Rejects_An_Empty_Release()
    {
        var directory = Directory.CreateDirectory(Path.Combine(RepositoryRoot(), "artifacts", "publish-rehearsal-tests", Guid.NewGuid().ToString("N")));
        try
        {
            var result = await ModuleTester.For<RehearsalExecutionModule>()
                .WithService<IOptions<PublishSettings>>(Microsoft.Extensions.Options.Options.Create(new PublishSettings { RehearsalFeedPath = directory.FullName }))
                .WithDependencyResult<PackagePathsParserModule, List<FilePath>>([])
                .ExecuteAsync();
            await Assert.That(result.Exception).IsTypeOf<InvalidOperationException>();
            await Assert.That(result.Exception!.Message).Contains("no packages");
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    [Arguments("4.0.0", false)]
    [Arguments("4.0.0-rc.rehearsal", true)]
    [Arguments("4.0.0+build-1", false)]
    public async Task Release_Uses_The_Packed_Version_And_Generated_Notes(string version, bool prerelease)
    {
        NewRelease? request = null;
        var client = new Mock<IGitHubClient>(MockBehavior.Strict);
        client.Setup(x => x.Repository.Release.Create(123L, It.IsAny<NewRelease>()))
            .Callback<long, NewRelease>((_, release) => request = release)
            .ReturnsAsync(new Release());
        var github = Mock.Of<IGitHub>(x => x.Client == client.Object && x.EnvironmentVariables.RepositoryId == "123");

        var result = await ModuleTester.For<ReleaseExecutionModule>()
            .WithService<IOptions<GitHubSettings>>(Microsoft.Extensions.Options.Options.Create(new GitHubSettings()))
            .WithService<IGitHub>(github)
            .WithDependencyResult<NugetVersionGeneratorModule, string>(version)
            .ExecuteAsync();

        await Assert.That(result.Exception).IsNull();
        await Assert.That(request!.TagName).IsEqualTo($"v{version}");
        await Assert.That(request.Name).IsEqualTo(version);
        await Assert.That(request.GenerateReleaseNotes).IsTrue();
        await Assert.That(request.Prerelease).IsEqualTo(prerelease);
        client.Verify(x => x.Repository.Release.Create(123L, It.IsAny<NewRelease>()), Times.Once);
    }

    // Exercise module bodies without CI-only branch, token, capability, or OS conditions.
    [DependsOn<PackagePathsParserModule>]
    public class RehearsalExecutionModule(IOptions<PublishSettings> settings) : Module<CommandResult[]>
    {
        protected override Task<CommandResult[]> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            new ExposedRehearsalModule(settings).Execute(context, cancellationToken);
    }

    private class ExposedRehearsalModule(IOptions<PublishSettings> settings) : RehearsePublishModule(settings)
    {
        public Task<CommandResult[]> Execute(IModuleContext context, CancellationToken token) => base.ExecuteAsync(context, token);
    }

    [DependsOn<NugetVersionGeneratorModule>]
    public class ReleaseExecutionModule(IOptions<GitHubSettings> settings) : Module<Release>
    {
        protected override Task<Release> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            new ExposedReleaseModule(settings).Execute(context, cancellationToken);
    }

    private class ExposedReleaseModule(IOptions<GitHubSettings> settings) : CreateReleaseModule(settings, Microsoft.Extensions.Options.Options.Create(new PublishSettings()))
    {
        public Task<Release> Execute(IModuleContext context, CancellationToken token) => base.ExecuteAsync(context, token);
    }

    private static async Task<string> PackPackage(string root, string directory, string id, bool symbols)
    {
        var configuration = new DirectoryInfo(AppContext.BaseDirectory).Parent!.Name;
        var arguments = new List<string>
        {
            "pack", Path.Combine(root, "src", id, id + ".csproj"), "-c", configuration,
            "-p:PackageVersion=4.0.0-rc.rehearsal+build-1", "--output", directory,
        };
        if (symbols)
        {
            arguments.Add("--no-build");
            arguments.Add("--include-symbols");
        }

        var result = await RunDotNet(root, [.. arguments], CancellationToken.None);
        await Assert.That(result.ExitCode).IsEqualTo(0);
        return Path.Combine(directory, $"{id}.4.0.0-rc.rehearsal.nupkg");
    }

    private static string RepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory != null && !File.Exists(Path.Combine(directory.FullName, "scripts", "Invoke-AgentDotNet.ps1")))
        {
            directory = directory.Parent;
        }

        return directory?.FullName ?? throw new InvalidOperationException("Could not locate the guarded .NET runner.");
    }

    private static async ValueTask<CommandResult> RunDotNet(string root, string[] arguments, CancellationToken token)
    {
        static string Quote(string value) => "'" + value.Replace("'", "''") + "'";
        var guard = Path.Combine(root, "scripts", "Invoke-AgentDotNet.ps1");
        var script = $"& {Quote(guard)} -TimeoutSeconds 120 -SingleNode -DotNetArguments @({string.Join(',', arguments.Select(Quote))}); exit $LASTEXITCODE";
        var startInfo = new ProcessStartInfo("pwsh")
        {
            WorkingDirectory = root,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
        };
        startInfo.ArgumentList.Add("-NoProfile");
        startInfo.ArgumentList.Add("-EncodedCommand");
        startInfo.ArgumentList.Add(Convert.ToBase64String(Encoding.Unicode.GetBytes(script)));
        using var process = Process.Start(startInfo) ?? throw new InvalidOperationException("Could not start the guarded NuGet command.");
        var output = process.StandardOutput.ReadToEndAsync(token);
        var error = process.StandardError.ReadToEndAsync(token);
        await process.WaitForExitAsync(token);
        var standardOutput = await output;
        var standardError = await error;
        if (process.ExitCode != 0)
        {
            System.Console.WriteLine(standardOutput + standardError);
        }

        return CommandResult.Ok(standardOutput, standardError) with { ExitCode = process.ExitCode };
    }
}
