using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Docker.Extensions;
using ModularPipelines.Extensions;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Helpers;

public class DockerTests : TestBase
{
    private class DockerBuildModule : IModule<CommandResult>
    {
        public async Task<CommandResult?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<DockerBuildModule>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var resultRegistry = host.RootServices.GetRequiredService<Engine.IModuleResultRegistry>();
        var result = resultRegistry.GetResult<CommandResult>(typeof(DockerBuildModule))!;

        // IPipelineContext is a scoped service, so we need to create a scope
        await using var scope = host.RootServices.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<IPipelineContext>();
        var dockerfilePath = context.Git().RootDirectory
            .GetFolder("src")
            .GetFolder("MyApp")
            .GetFile("Dockerfile").Path;

        await Assert.That(result.Value!.CommandInput).IsEqualTo($"docker image build --build-arg Arg1=Value1 --build-arg Arg2=Value2 --build-arg Arg3=Value3 --tag mytaggedimage --target build-env {dockerfilePath}");
    }
}