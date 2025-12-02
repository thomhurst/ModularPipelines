using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Examples.Modules;

[DependsOn<GitVersionModule>]
public class SuccessModule3 : Module<IDictionary<string, object>?>
{
    /// <inheritdoc/>
    public override async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromSeconds(12), cancellationToken);

        context.GetModule<GitVersionModule, CommandResult>();

        return null;
    }
}