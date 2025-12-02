using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.Examples.Modules;

[DependsOn<DependentOnSuccessModule>]
public class DependentOn2 : Module<IDictionary<string, object>?>
{
    /// <inheritdoc/>
    public override async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        context.Logger.LogInformation("Some message");
        await Task.Delay(TimeSpan.FromMilliseconds(500), cancellationToken);
        return null;
    }
}