using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "app-insights", "component")]
public class AzMonitorAppInsightsComponentBilling
{
    public AzMonitorAppInsightsComponentBilling(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Show(AzMonitorAppInsightsComponentBillingShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorAppInsightsComponentBillingShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzMonitorAppInsightsComponentBillingUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorAppInsightsComponentBillingUpdateOptions(), token);
    }
}

