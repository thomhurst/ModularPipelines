using Microsoft.Extensions.Options;
using ModularPipelines.Build.Settings;

namespace ModularPipelines.Build.Modules.UnitTests;

public class RunGitHubUnitTestsModule(IOptions<PipelineSettings> pipelineSettings) : RunUnitTestModule(pipelineSettings)
{
    protected override string TestProjectFileName => "ModularPipelines.GitHub.UnitTests.csproj";
}
