using Microsoft.Extensions.Logging;

namespace ModularPipelines.DocumentationSnippets;

public class LoggingModule : SyncModule
{
    protected override void Execute(
        IModuleContext context, CancellationToken cancellationToken)
    {
        context.Logger.LogInformation("Pipeline executed at {Time}", DateTime.UtcNow);
    }
}
