using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.Examples.Modules;

public class SuccessModule3 : Module
{
    /// <inheritdoc/>
    protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromSeconds(12), cancellationToken);

        await GetModule<GitVersionModule>();

        return null;
    }
}