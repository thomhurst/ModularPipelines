using Microsoft.Extensions.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Modules;

namespace ModularPipelines.UnitTests.Registration;

[TUnit.Core.NotInParallel("ProcessEnvironment")]
public class PipelineWorkingDirectoryTests
{
    private sealed record WorkingDirectoryObservation(
        string EnvironmentDirectory,
        string FilePath,
        string CommandDirectory);

    private sealed class ObserveWorkingDirectoryModule : Module<WorkingDirectoryObservation>
    {
        protected internal override async Task<WorkingDirectoryObservation> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            await context.Files.WriteAsync("relative.txt", "content", cancellationToken);
            var command = await context.Shell.PowerShell.ScriptAsync(
                new("Write-Output $PWD.Path"),
                cancellationToken: cancellationToken);

            return new WorkingDirectoryObservation(
                context.Environment.WorkingDirectory,
                context.Files.GetFile("relative.txt").Path,
                command.WorkingDirectory);
        }
    }

    [Test]
    public async Task ConfiguredWorkingDirectoryScopesCommandsAndFiles()
    {
        var processDirectory = Environment.CurrentDirectory;
        var workingDirectory = Directory.CreateTempSubdirectory("pipeline-working-directory-");

        try
        {
            using var builder = Pipeline.CreateBuilder(new PipelineBuilderOptions
            {
                WorkingDirectory = workingDirectory.FullName,
            });
            builder.AddModule<ObserveWorkingDirectoryModule>();

            var summary = await builder.ExecutePipelineAsync();
            var result = await summary.Modules.OfType<ObserveWorkingDirectoryModule>().Single();
            var observation = result.ValueOrDefault!;

            using (Assert.Multiple())
            {
                await Assert.That(builder.WorkingDirectory).IsEqualTo(workingDirectory.FullName);
                await Assert.That(builder.Environment.ContentRootPath).IsEqualTo(workingDirectory.FullName);
                await Assert.That(observation.EnvironmentDirectory).IsEqualTo(workingDirectory.FullName);
                await Assert.That(observation.FilePath)
                    .IsEqualTo(Path.Combine(workingDirectory.FullName, "relative.txt"));
                await Assert.That(observation.CommandDirectory).IsEqualTo(workingDirectory.FullName);
                await Assert.That(Environment.CurrentDirectory).IsEqualTo(processDirectory);
            }
        }
        finally
        {
            workingDirectory.Delete(recursive: true);
        }
    }

    [Test]
    public async Task CreateBuilderFindsPipelineProjectFromCallerPath()
    {
        var projectDirectory = Directory.CreateTempSubdirectory("pipeline-project-");
        await File.WriteAllTextAsync(
            Path.Combine(projectDirectory.FullName, "appsettings.json"),
            "{\"PipelineProject\":true}");
        await File.WriteAllTextAsync(Path.Combine(projectDirectory.FullName, "Pipeline.csproj"), "<Project />");

        try
        {
            using var builder = Pipeline.CreateBuilder(
                sourceFilePath: Path.Combine(projectDirectory.FullName, "Program.cs"));
            builder.Configuration.AddJsonFile("appsettings.json");

            using (Assert.Multiple())
            {
                await Assert.That(builder.WorkingDirectory).IsEqualTo(projectDirectory.FullName);
                await Assert.That(builder.Configuration.GetValue<bool>("PipelineProject")).IsTrue();
            }
        }
        finally
        {
            projectDirectory.Delete(recursive: true);
        }
    }

    [Test]
    public async Task FindGitRootWalksParentDirectories()
    {
        var repositoryDirectory = Directory.CreateTempSubdirectory("pipeline-git-root-");
        var nestedDirectory = Directory.CreateDirectory(Path.Combine(repositoryDirectory.FullName, "src", "Pipeline"));
        Directory.CreateDirectory(Path.Combine(repositoryDirectory.FullName, ".git"));

        try
        {
            var result = PipelineDirectory.FindGitRoot(Path.Combine(nestedDirectory.FullName, "Program.cs"));

            await Assert.That(result).IsEqualTo(repositoryDirectory.FullName);
        }
        finally
        {
            repositoryDirectory.Delete(recursive: true);
        }
    }
}
