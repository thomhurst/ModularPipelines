using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "app-insights", "component")]
public class AzMonitorAppInsightsComponentLinkedStorage
{
    public AzMonitorAppInsightsComponentLinkedStorage(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Link(AzMonitorAppInsightsComponentLinkedStorageLinkOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzMonitorAppInsightsComponentLinkedStorageShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorAppInsightsComponentLinkedStorageShowOptions(), token);
    }

    public async Task<CommandResult> Unlink(AzMonitorAppInsightsComponentLinkedStorageUnlinkOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorAppInsightsComponentLinkedStorageUnlinkOptions(), token);
    }

    public async Task<CommandResult> Update(AzMonitorAppInsightsComponentLinkedStorageUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}