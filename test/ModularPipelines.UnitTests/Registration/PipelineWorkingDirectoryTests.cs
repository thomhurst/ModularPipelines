using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ModularPipelines.Caching;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Extensions;
using ModularPipelines.Modules;

namespace ModularPipelines.UnitTests.Registration;

[TUnit.Core.NotInParallel("ProcessEnvironment")]
public class PipelineWorkingDirectoryTests
{
    private sealed record WorkingDirectoryObservation(
        string EnvironmentDirectory,
        string FilePath,
        string Checksum,
        string ZipPath,
        string UnzipPath,
        string CommandDirectory,
        string CacheWorkingDirectory,
        string RunReportPath,
        int WorkingDirectoryRegistrationCount);

    private sealed class ObserveWorkingDirectoryModule(
        IOptions<ModuleCacheOptions> cacheOptions,
        RunReportPathResolver runReportPathResolver,
        IEnumerable<PipelineWorkingDirectory> workingDirectories)
        : Module<WorkingDirectoryObservation>
    {
        protected internal override async Task<WorkingDirectoryObservation> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            await context.Files.WriteAsync("relative.txt", "content", cancellationToken);
            var checksum = context.Files.Checksum.Md5("relative.txt");
            var zip = context.Files.Zip.ZipFolder(context.Files.GetFolder("."), "out.zip");
            var unzipped = context.Files.Zip.UnZipToFolder("out.zip", "unzipped");
            var command = await context.Shell.PowerShell.RunAsync(
                "Write-Output $PWD.Path",
                cancellationToken: cancellationToken);

            return new WorkingDirectoryObservation(
                context.Environment.WorkingDirectory,
                context.Files.GetFile("relative.txt").Path,
                checksum,
                zip.Path,
                unzipped.Path,
                command.WorkingDirectory,
                cacheOptions.Value.WorkingDirectory,
                runReportPathResolver.Resolve(Path.Combine("artifacts", "run-report.json")),
                workingDirectories.Count());
        }
    }

    [Test]
    public async Task ConfiguredWorkingDirectoryScopesCommandsAndFiles()
    {
        var processDirectory = Environment.CurrentDirectory;
        var workingDirectory = Directory.CreateTempSubdirectory("pipeline-working-directory-");

        try
        {
            var builder = Pipeline.CreateBuilder(new PipelineBuilderSettings
            {
                WorkingDirectory = workingDirectory.FullName,
            });
            builder.AddModuleCache<FileSystemModuleCache>();
            builder.AddModule<ObserveWorkingDirectoryModule>();

            var summary = await builder.RunAsync();
            var result = await summary.Modules.OfType<ObserveWorkingDirectoryModule>().Single();
            var observation = result.ValueOrDefault!;

            using (Assert.Multiple())
            {
                await Assert.That(builder.WorkingDirectory).IsEqualTo(workingDirectory.FullName);
                await Assert.That(builder.Environment.ContentRootPath).IsEqualTo(workingDirectory.FullName);
                await Assert.That(observation.EnvironmentDirectory).IsEqualTo(workingDirectory.FullName);
                await Assert.That(observation.FilePath)
                    .IsEqualTo(Path.Combine(workingDirectory.FullName, "relative.txt"));
                await Assert.That(observation.Checksum).IsEqualTo("9A0364B9E99BB480DD25E1F0284C8555");
                await Assert.That(observation.ZipPath)
                    .IsEqualTo(Path.Combine(workingDirectory.FullName, "out.zip"));
                await Assert.That(observation.UnzipPath)
                    .IsEqualTo(Path.Combine(workingDirectory.FullName, "unzipped"));
                await Assert.That(observation.CommandDirectory).IsEqualTo(workingDirectory.FullName);
                await Assert.That(observation.CacheWorkingDirectory).IsEqualTo(workingDirectory.FullName);
                await Assert.That(observation.RunReportPath)
                    .IsEqualTo(Path.Combine(workingDirectory.FullName, "artifacts", "run-report.json"));
                await Assert.That(observation.WorkingDirectoryRegistrationCount).IsEqualTo(1);
                await Assert.That(Environment.CurrentDirectory).IsEqualTo(processDirectory);
            }
        }
        finally
        {
            workingDirectory.Delete(recursive: true);
        }
    }

    [Test]
    public async Task ExplicitModuleCacheWorkingDirectoryOverridesPipelineDirectory()
    {
        var pipelineDirectory = Directory.CreateTempSubdirectory("pipeline-working-directory-");
        var cacheWorkingDirectory = Directory.CreateTempSubdirectory("cache-working-directory-");

        try
        {
            var builder = Pipeline.CreateBuilder(new PipelineBuilderSettings
            {
                WorkingDirectory = pipelineDirectory.FullName,
            });
            builder.AddModuleCache<FileSystemModuleCache>(options =>
                options.WorkingDirectory = cacheWorkingDirectory.FullName);
            builder.AddModule<ObserveWorkingDirectoryModule>();

            var summary = await builder.RunAsync();
            var result = await summary.Modules.OfType<ObserveWorkingDirectoryModule>().Single();

            await Assert.That(result.ValueOrDefault!.CacheWorkingDirectory)
                .IsEqualTo(cacheWorkingDirectory.FullName);
        }
        finally
        {
            pipelineDirectory.Delete(recursive: true);
            cacheWorkingDirectory.Delete(recursive: true);
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
            var nestedDirectory = Directory.CreateDirectory(Path.Combine(projectDirectory.FullName, "src", "Pipeline"));
            var builder = Pipeline.CreateBuilder(
                sourceFilePath: Path.Combine(nestedDirectory.FullName, "Program.cs"));
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
    public async Task ExplicitContentRootOverridesInferredPipelineProject()
    {
        var projectDirectory = Directory.CreateTempSubdirectory("pipeline-project-");
        var contentRoot = Directory.CreateTempSubdirectory("pipeline-content-root-");
        await File.WriteAllTextAsync(Path.Combine(projectDirectory.FullName, "appsettings.json"), "{}");
        await File.WriteAllTextAsync(Path.Combine(projectDirectory.FullName, "Pipeline.csproj"), "<Project />");

        try
        {
            var builder = Pipeline.CreateBuilder(
                new PipelineBuilderSettings { ContentRootPath = contentRoot.FullName },
                Path.Combine(projectDirectory.FullName, "Program.cs"));

            using (Assert.Multiple())
            {
                await Assert.That(builder.WorkingDirectory).IsEqualTo(contentRoot.FullName);
                await Assert.That(builder.Environment.ContentRootPath).IsEqualTo(contentRoot.FullName);
            }
        }
        finally
        {
            projectDirectory.Delete(recursive: true);
            contentRoot.Delete(recursive: true);
        }
    }

    [Test]
    public async Task HostConfiguredContentRootOverridesInferredPipelineProject()
    {
        var projectDirectory = Directory.CreateTempSubdirectory("pipeline-project-");
        var contentRoot = Directory.CreateTempSubdirectory("pipeline-content-root-");
        await File.WriteAllTextAsync(Path.Combine(projectDirectory.FullName, "Pipeline.csproj"), "<Project />");

        try
        {
            var builder = Pipeline.CreateBuilder(
                new PipelineBuilderSettings
                {
                    Args = ["--contentRoot", contentRoot.FullName],
                },
                Path.Combine(projectDirectory.FullName, "Program.cs"));

            using (Assert.Multiple())
            {
                await Assert.That(builder.WorkingDirectory).IsEqualTo(contentRoot.FullName);
                await Assert.That(builder.Environment.ContentRootPath).IsEqualTo(contentRoot.FullName);
            }
        }
        finally
        {
            projectDirectory.Delete(recursive: true);
            contentRoot.Delete(recursive: true);
        }
    }

    [Test]
    public async Task ExplicitWorkingDirectorySkipsProjectInference()
    {
        var variableName = "MODULAR_PIPELINES_DIRECTORY";
        var previousValue = Environment.GetEnvironmentVariable(variableName);
        var workingDirectory = Directory.CreateTempSubdirectory("pipeline-working-directory-");
        Environment.SetEnvironmentVariable(
            variableName,
            Path.Combine(Path.GetTempPath(), $"missing-pipeline-{Guid.NewGuid():N}"));

        try
        {
            var builder = Pipeline.CreateBuilder(new PipelineBuilderSettings
            {
                WorkingDirectory = workingDirectory.FullName,
            });

            await Assert.That(builder.WorkingDirectory).IsEqualTo(workingDirectory.FullName);
        }
        finally
        {
            Environment.SetEnvironmentVariable(variableName, previousValue);
            workingDirectory.Delete(recursive: true);
        }
    }

    [Test]
    public async Task NonInferringBuilderIgnoresPipelineDirectoryEnvironmentVariable()
    {
        var variableName = "MODULAR_PIPELINES_DIRECTORY";
        var previousValue = Environment.GetEnvironmentVariable(variableName);
        Environment.SetEnvironmentVariable(
            variableName,
            Path.Combine(Path.GetTempPath(), $"missing-pipeline-{Guid.NewGuid():N}"));

        try
        {
            var builder = Pipeline.CreateBuilderWithoutProjectInference(new PipelineBuilderSettings());

            await Assert.That(builder.WorkingDirectory).IsEqualTo(Environment.CurrentDirectory);
        }
        finally
        {
            Environment.SetEnvironmentVariable(variableName, previousValue);
        }
    }

    [Test]
    public async Task CreateBuilderUsesCallerFilePath()
    {
        var method = typeof(Pipeline).GetMethod(
            nameof(Pipeline.CreateBuilder),
            [typeof(string[]), typeof(string)]);

        await Assert.That(method!.GetParameters()[1].GetCustomAttributesData()
                .Select(attribute => attribute.AttributeType))
            .Contains(typeof(System.Runtime.CompilerServices.CallerFilePathAttribute));
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
