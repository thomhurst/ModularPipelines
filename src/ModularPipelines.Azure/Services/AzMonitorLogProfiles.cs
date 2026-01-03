using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("monitor")]
public class AzMonitorLogProfiles
{
    public AzMonitorLogProfiles(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzMonitorLogProfilesCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzMonitorLogProfilesDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorLogProfilesDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzMonitorLogProfilesListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorLogProfilesListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzMonitorLogProfilesShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorLogProfilesShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzMonitorLogProfilesUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorLogProfilesUpdateOptions(), cancellationToken: token);
    }
}