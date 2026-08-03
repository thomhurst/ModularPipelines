using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Docker.Extensions;
using ModularPipelines.Docker.Options;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.Docker.UnitTests.Helpers;

public class DockerTests : TestBase
{
    private class DockerBuildModule : Module<CommandResult>
    {
        protected internal override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            var pretendPath = context.Files
                .GetFolder(Environment.CurrentDirectory)
                .GetFolder("src")
                .GetFolder("MyApp")
                .GetFile("Dockerfile");

            return await context.Docker().Image.BuildAsync(new DockerImageBuildOptions(pretendPath.Path)
            {
                BuildArg =
                [
                    ("Arg1", "Value1"),
                    ("Arg2", "Value2"),
                    ("Arg3", "Value3"),
                ],
                Tag = ["mytaggedimage"],
                Target = "build-env",
            },
            new CommandExecutionOptions
            {
                InternalDryRun = true,
            },
            cancellationToken);
        }
    }

    [Test]
    public async Task DockerBuild_CorrectInputCommand()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<DockerBuildModule>()
            .BuildAsync();

        await host.RunAsync();

        var resultRegistry = host.Services.GetRequiredService<ModularPipelines.Engine.IModuleResultRegistry>();
        var result = resultRegistry.GetResult<CommandResult>(typeof(DockerBuildModule))!;

        // IPipelineContext is a scoped service, so we need to create a scope
        await using var scope = host.Services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<IPipelineContext>();
        var dockerfilePath = context.Files.GetFolder(Environment.CurrentDirectory)
            .GetFolder("src")
            .GetFolder("MyApp")
            .GetFile("Dockerfile").Path;

        await Assert.That(result.ValueOrDefault!.CommandInput).IsEqualTo($"docker image build --build-arg=Arg1=Value1 --build-arg=Arg2=Value2 --build-arg=Arg3=Value3 --tag=mytaggedimage --target=build-env {dockerfilePath}");
    }
}
