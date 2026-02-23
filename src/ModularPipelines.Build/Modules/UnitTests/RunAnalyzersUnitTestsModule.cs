using Microsoft.Extensions.Options;
using ModularPipelines.Build.Settings;

namespace ModularPipelines.Build.Modules.UnitTests;

public class RunAnalyzersUnitTestsModule(IOptions<PipelineSettings> pipelineSettings) : RunUnitTestModule(pipelineSettings)
{
    protected override string TestProjectFileName => "ModularPipelines.Development.Analyzers.UnitTests.csproj";
}
