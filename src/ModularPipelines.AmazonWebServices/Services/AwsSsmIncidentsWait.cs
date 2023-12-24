using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm-incidents")]
public class AwsSsmIncidentsWait
{
    public AwsSsmIncidentsWait(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> WaitForReplicationSetActive(AwsSsmIncidentsWaitWaitForReplicationSetActiveOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> WaitForReplicationSetDeleted(AwsSsmIncidentsWaitWaitForReplicationSetDeletedOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}