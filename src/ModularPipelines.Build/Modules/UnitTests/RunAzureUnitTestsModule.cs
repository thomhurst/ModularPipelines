using Microsoft.Extensions.Options;
using ModularPipelines.Build.Settings;

namespace ModularPipelines.Build.Modules.UnitTests;

public class RunAzureUnitTestsModule(IOptions<PipelineSettings> pipelineSettings) : RunUnitTestModule(pipelineSettings)
{
    protected override string TestProjectFileName => "ModularPipelines.Azure.UnitTests.csproj";
}
