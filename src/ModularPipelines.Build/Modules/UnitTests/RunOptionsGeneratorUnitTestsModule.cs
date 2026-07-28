using Microsoft.Extensions.Options;
using ModularPipelines.Build.Settings;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Build.Modules.UnitTests;

public class RunOptionsGeneratorUnitTestsModule(IOptions<PipelineSettings> pipelineSettings)
    : RunUnitTestModule(pipelineSettings)
{
    protected override string TestProjectFileName => "ModularPipelines.OptionsGenerator.Tests.csproj";

    protected override SkipDecision GetSkipDecision(IModuleContext context) =>
        FastFailValidation.IsComplete(context)
            ? SkipDecision.Skip("Validated by the fast-fail CI job")
            : SkipDecision.DoNotSkip;
}
