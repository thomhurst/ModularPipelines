using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("batch")]
public class AzBatchNode
{
    public AzBatchNode(
        AzBatchNodeFile file,
        AzBatchNodeRemoteDesktop remoteDesktop,
        AzBatchNodeRemoteLoginSettings remoteLoginSettings,
        AzBatchNodeScheduling scheduling,
        AzBatchNodeServiceLogs serviceLogs,
        AzBatchNodeUser user,
        ICommand internalCommand
    )
    {
        File = file;
        RemoteDesktop = remoteDesktop;
        RemoteLoginSettings = remoteLoginSettings;
        Scheduling = scheduling;
        ServiceLogs = serviceLogs;
        User = user;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzBatchNodeFile File { get; }

    public AzBatchNodeRemoteDesktop RemoteDesktop { get; }

    public AzBatchNodeRemoteLoginSettings RemoteLoginSettings { get; }

    public AzBatchNodeScheduling Scheduling { get; }

    public AzBatchNodeServiceLogs ServiceLogs { get; }

    public AzBatchNodeUser User { get; }

    public async Task<CommandResult> Delete(AzBatchNodeDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzBatchNodeListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Reboot(AzBatchNodeRebootOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Reimage(AzBatchNodeReimageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzBatchNodeShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}