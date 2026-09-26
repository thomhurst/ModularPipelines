using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Build.Settings;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Build.Modules.UnitTests;

public class RunOptionsGeneratorUnitTestsModule(IOptions<PipelineSettings> pipelineSettings)
    : RunUnitTestModule(pipelineSettings)
{
    protected override string TestProjectFileName => "ModularPipelines.OptionsGenerator.Tests.csproj";

    protected override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        if (FastFailValidation.IsComplete(context))
        {
            const string message = "Validated by the fast-fail CI job";
            context.Logger.LogInformation(message);
            return CommandResult.Ok(message);
        }

        return await base.ExecuteAsync(context, cancellationToken).ConfigureAwait(false);
    }
}
