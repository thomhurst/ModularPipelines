using ModularPipelines.Context;
using ModularPipelines.Docker.Extensions;
using ModularPipelines.Git;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Helpers;

public class DockerTests : TestBase
{
    private class DockerBuildModule : Module<CommandResult>
    {
        public override async Task<CommandResult?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            var pretendPath = context.Git()
                .RootDirectory
                .GetFolder("src")
                .GetFolder("MyApp")
                .GetFile("Dockerfile");

            return await context.Docker().DockerImage.Build(new(pretendPath)
            {
                InternalDryRun = true,
                BuildArg = new List<KeyValue>
                {
                    ("Arg1", "Value1"),
                    ("Arg2", "Value2"),
                    ("Arg3", "Value3"),
                },
                Tag = "mytaggedimage",
                Target = "build-env",
            }, token: cancellationToken);
        }
    }

    [Test]
    public async Task DockerBuild_CorrectInputCommand()
    {
        var pipelineSummary = await TestPipelineHostBuilder.Create()
            .AddModule<DockerBuildModule>()
            .ExecutePipelineAsync();

        var module = pipelineSummary.GetModule<DockerBuildModule>();
        var result = (ModuleResult<CommandResult>)await module.GetModuleResult();

        // TODO v3.0: PipelineSummary.Context removed - use GetService<IGit>() instead
        var git = await GetService<IGit>();
        var dockerfilePath = git.RootDirectory
            .GetFolder("src")
            .GetFolder("MyApp")
            .GetFile("Dockerfile").Path;

        await Assert.That(result.Value!.CommandInput).IsEqualTo($"docker image build --build-arg Arg1=Value1 --build-arg Arg2=Value2 --build-arg Arg3=Value3 --tag mytaggedimage --target build-env {dockerfilePath}");
    }
}