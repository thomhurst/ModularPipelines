using Microsoft.Extensions.Options;
using ModularPipelines.Build.Settings;

namespace ModularPipelines.Build.Modules.UnitTests;

public class RunEksctlUnitTestsModule(IOptions<PipelineSettings> pipelineSettings) : RunUnitTestModule(pipelineSettings)
{
    protected override string TestProjectFileName => "ModularPipelines.Eksctl.UnitTests.csproj";
}
