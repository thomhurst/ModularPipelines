using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("webpubsub")]
public class AzWebpubsubService
{
    public AzWebpubsubService(
        AzWebpubsubServiceConnection connection,
        AzWebpubsubServiceGroup group,
        AzWebpubsubServicePermission permission,
        AzWebpubsubServiceUser user,
        ICommand internalCommand
    )
    {
        Connection = connection;
        Group = group;
        Permission = permission;
        User = user;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzWebpubsubServiceConnection Connection { get; }

    public AzWebpubsubServiceGroup Group { get; }

    public AzWebpubsubServicePermission Permission { get; }

    public AzWebpubsubServiceUser User { get; }

    public async Task<CommandResult> Broadcast(AzWebpubsubServiceBroadcastOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}