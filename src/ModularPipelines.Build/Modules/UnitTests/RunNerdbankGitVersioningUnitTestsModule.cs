using Microsoft.Extensions.Options;
using ModularPipelines.Build.Settings;

namespace ModularPipelines.Build.Modules.UnitTests;

public class RunNerdbankGitVersioningUnitTestsModule(IOptions<PipelineSettings> pipelineSettings)
    : RunUnitTestModule(pipelineSettings)
{
    protected override string TestProjectFileName =>
        "ModularPipelines.NerdbankGitVersioning.UnitTests.csproj";
}
