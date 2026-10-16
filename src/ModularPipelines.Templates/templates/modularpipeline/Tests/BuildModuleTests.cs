using Microsoft.Extensions.Options;
using ModularPipelines;
using ModularPipelines.Testing;
using TemplatePipeline.Modules;
using TemplatePipeline.Settings;
using TUnit.Assertions;
using TUnit.Assertions.Extensions;
using TUnit.Core;

namespace TemplatePipeline.Tests;

public class BuildModuleTests
{
    [Test]
    public async Task BuildsConfiguredSolutionWithoutRestoring()
    {
        var settings = new BuildSettings
        {
            Solution = "example.slnx",
            Configuration = "Debug",
        };

        var run = await ModuleTester.For<BuildModule, CommandResult>()
            .WithService<IOptions<BuildSettings>>(Options.Create(settings))
            .WithDependencyResult<RestoreModule, CommandResult>(CommandResult.Ok())
            .ExecuteAsync();

        await Assert.That(run.Exception).IsNull();
        await Assert.That(run.Commands).Count().IsEqualTo(1);
        await Assert.That(run.Commands[0].CommandLine.Tool).IsEqualTo("dotnet");
        await Assert.That(run.Commands[0].CommandLine.Arguments)
            .IsEquivalentTo(["build", settings.Solution, "--configuration", settings.Configuration, "--no-restore"]);
    }
}
